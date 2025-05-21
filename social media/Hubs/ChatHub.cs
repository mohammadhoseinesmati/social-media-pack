using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using social_media.Data;
using social_media.Data.Models;
using System.Security.Claims;

namespace social_media.Hubs
{
    public class ChatHub : Hub
    {
        private readonly AppDBContext _dbContext;

        public ChatHub(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task SendPrivateMessage(string toUserId, string message)
        {
            var fromUserId =Context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var fromUser = int.Parse(fromUserId);
            var toUser = int.Parse(toUserId);
            var senderUser = await _dbContext.Users.FindAsync(fromUser);
            var senderName = senderUser.UserName ?? "";
            var msg = new ChatMessage
            {
                SenderId = fromUser,
                ReciverId = toUser,
                Message = message,
                TimeSend = DateTime.UtcNow
            };

            _dbContext.ChatMessages.Add(msg);
            await _dbContext.SaveChangesAsync();

            await Clients.Client(fromUserId).SendAsync("ReceiveMessage", fromUserId, message);
            await Clients.User(toUserId).SendAsync("ReceiveMessage", senderName, message);
        }
    }
}
