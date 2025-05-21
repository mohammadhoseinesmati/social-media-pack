using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;
using social_media.Data.DTOS;
using social_media.Data.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace social_media.Data.Services
{
    public class ChatMessageService : IChatMessageService
    {
        private readonly AppDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChatMessageService(AppDBContext dBContext , IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dBContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<ChatMessage>> Messages(int userGoal)
        {
            List<ChatMessage> listOfMessage = new List<ChatMessage>();
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (userId != null)
            {
                int user = int.Parse(userId);
                listOfMessage = await _dbContext.ChatMessages
                    .Where(p => (p.SenderId == user && p.ReciverId == userGoal) || (p.SenderId == userGoal && p.ReciverId == user))
                    .OrderBy( p => p.TimeSend )
                    .ToListAsync();
            }
            return listOfMessage;    
        }
        public async Task<List<ChatContactDto>> GetContectsAsync(int userGoal)
        {
            var message = await _dbContext.ChatMessages
                .Include(p => p.Sender)
                .Include(p => p.Reciver)
                .Where(p => p.SenderId == userGoal || p.ReciverId == userGoal)
                .ToListAsync();

            var group = message
                .GroupBy(p => p.SenderId == userGoal ? p.ReciverId : p.SenderId)
                .Select(p => p.OrderByDescending(g => g.TimeSend)
                .FirstOrDefault())
                .OrderByDescending(p => p.TimeSend)
                .ToList();

            var contactList = group.Select(m =>
            {
                var otherUser = m.SenderId == userGoal ? m.Reciver : m.Sender;

                return new ChatContactDto
                {
                    UserId = otherUser.Id,
                    UserName = otherUser.UserName,
                    LastMessage = m.Message,
                    LastTimeMessage = m.TimeSend
                };
            }).ToList();

            return contactList;
        }
    }
}
