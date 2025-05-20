
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using social_media.Data.Constants;
using social_media.Data.Models;
using social_media.Hubs;
using System.ComponentModel.DataAnnotations.Schema;

namespace social_media.Data.Services
{
    public class NotificationService : INotificationService
    {
        private readonly AppDBContext _dbcontext;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(AppDBContext dBContext , IHubContext<NotificationHub> hubContext)
        {
            _dbcontext = dBContext;
            _hubContext = hubContext;
        }
        public async Task AddNewNotficition(int userid, string type, string userFullName , int? postId)
        {
            var notif = new Notification
            {
                UserId = userid,
                Message = GetPostMessage(userFullName, type),
                Type = type,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                PostId = postId.HasValue ? postId.Value : null,
            };
            await _dbcontext.Notifications.AddAsync(notif);
            await _dbcontext.SaveChangesAsync();

            var countnotif = GetUnreadNotificationAsync(userid);

            await _hubContext.Clients.User(ToString())
                .SendAsync("ReceiveNotification", countnotif);

        }

        public async Task<int> GetUnreadNotificationAsync(int userid)
        {
            var count = await _dbcontext.Notifications
                .Where(p => p.UserId == userid && !p.IsRead)
                .CountAsync();

            return count;
                
        }

        private string GetPostMessage(string userFullName , string Type)
        {
            var message = "";
            switch (Type)
            {
                case NotificationType.Like:
                    message = $"{userFullName} liked your post";
                    break;

                case NotificationType.Comment:
                    message = $"{userFullName} commented on your post";
                    break;

                case NotificationType.Favorite:
                    message = $"your post is favorite for {userFullName}";
                    break;

                case NotificationType.FriendRequest:
                    message = $"{userFullName} send you a friendrequest";
                    break;

                case NotificationType.FriendShipRequest:
                    message = $"{userFullName} accept your friendrequest";
                    break;

                default:
                    message = "";
                    break;
            }
            return message;
        }
        public async Task<List<Notification>> GetNotifications(int userid)
        {
            var listofnotif =  await _dbcontext.Notifications
                .Where(p =>p.UserId == userid)
                .OrderBy(p => p.IsRead)
                .OrderByDescending(p => p.Created)
                .ToListAsync();

            return listofnotif;
        }

        public async Task SetNotificationAsReadAsync(int notifId)
        {
            var notif = await _dbcontext.Notifications.FirstOrDefaultAsync(p => p.Id == notifId);

            if (notif != null)
            {
                notif.IsRead = true;
                notif.Updated = DateTime.UtcNow;

                _dbcontext.Notifications.Update(notif);
                await _dbcontext.SaveChangesAsync();
            }    
        }

        
    }
}
