using social_media.Data.Models;

namespace social_media.Data.Services
{
    public interface INotificationService
    {
        Task AddNewNotficition(int userid, string type , string userFullName , int? postId);
        Task<int> GetUnreadNotificationAsync(int userid);
        Task<List<Notification>> GetNotifications(int userid);

        Task SetNotificationAsReadAsync(int notifId);
    }
}
