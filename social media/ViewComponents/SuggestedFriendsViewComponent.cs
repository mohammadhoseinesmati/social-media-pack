using Microsoft.AspNetCore.Mvc;
using social_media.Data.Services;
using social_media.ViewModels.Friends;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;


namespace social_media.ViewComponents
{
    public class SuggestedFriendsViewComponent : ViewComponent
    {
        private readonly IFriendsServices _friendsServices;
        public SuggestedFriendsViewComponent(IFriendsServices friendsServices)
        {
                _friendsServices = friendsServices;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var id = ((ClaimsPrincipal)User).FindFirstValue(ClaimTypes.NameIdentifier);
            var userid = int.Parse(id);
            var suggestedfriends = await _friendsServices.GetSuggestedFriendsAsync(userid);
            var suggestedfriendsvm = suggestedfriends.Select(n => new UserWithFriendsCountVM
            {
                UserId = n.User.Id,
                PictureUrl = n.User.ProfilePictureUrl,
                FullName = n.User.FullName,
                Count = n.FriendsCount
            }).ToList();
            return View(suggestedfriendsvm);   
        }
            
    }
}
