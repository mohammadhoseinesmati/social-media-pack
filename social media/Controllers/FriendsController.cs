using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using social_media.Controllers.Base;
using social_media.Data.Constants;
using social_media.Data.Services;
using social_media.ViewModels.Friends;

namespace social_media.Controllers
{
    [Authorize(Roles = AppRoles.User)]
    public class FriendsController : BaseController
    {
        private readonly IFriendsServices _friendsServices;
        private readonly INotificationService _notificationService;

        public FriendsController(IFriendsServices friends, INotificationService notificationService)
        {
            _friendsServices = friends;
            _notificationService = notificationService;
        }
        public async Task<IActionResult> Index()
        {
            var userid = GetUserId();
            if (userid.Value == null)
                GoToLogin();
            var friendship = new FriendshipVM
            {
                FriendRequestsSent = await _friendsServices.GetRequestFriendsAsync(userid.Value),
                FriendRequestsRecive = await _friendsServices.GetReciveRequestFriendsAsync(userid.Value),
                Friends = await _friendsServices.GetFriendshipListAsync(userid.Value),
            };
            return View(friendship);
        }
        [HttpPost]
        public async Task<IActionResult> SendFriendRequest(int receiverId)
        {
            var userid = GetUserId();
            if (userid.Value == null)
                GoToLogin();
            var fullName = FullName();
            await _friendsServices.SendREquestAsync(userid.Value, receiverId);
            await _notificationService.AddNewNotficition(receiverId , NotificationType.FriendRequest , fullName,null);
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateFriendRequest(int requestId, string status)
        {
            var userid = GetUserId();
            if (userid.Value == null)
                GoToLogin();
            var fullName = FullName();
            
            var reciverId = await _friendsServices.UpdateREquestAsync(requestId, status);
            if (status == "Accepted")
            {
                await _notificationService.AddNewNotficition(reciverId.SenderId, NotificationType.FriendShipRequest, fullName, null);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFriends(int friendshipId)
        {
            await _friendsServices.RemoveFriendshipAsync(friendshipId);
            return RedirectToAction("Index");
        }
    }
}
