using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using social_media.Controllers.Base;
using social_media.Data.Constants;
using social_media.Data.Models;
using social_media.Data.Services;

namespace social_media.Controllers
{
    [Authorize(Roles = AppRoles.User)]
    public class NotificationsController : BaseController
    {
        private readonly INotificationService _notificationService;
        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetCount()
        {
            var userid = GetUserId();
            if (userid == null)
            {
                return GoToLogin();
            }

             var count = await _notificationService.GetUnreadNotificationAsync(userid.Value);

            return Json(count);
        }

        public async Task<IActionResult> GetNotification()
        {
            var id = GetUserId();
            var list = await _notificationService.GetNotifications(id.Value);

            return PartialView("Notifications/_Notifications" , list);
        }

        [HttpPost]
        public async Task<IActionResult> SetNotificationAsReadAsync(int notifId)
        {
            var id = GetUserId();
            await _notificationService.SetNotificationAsReadAsync(notifId);

            var list = await _notificationService.GetNotifications(id.Value);

            return PartialView("Notifications/_Notifications", list);
        }
    }
}
