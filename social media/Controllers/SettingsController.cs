using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using social_media.Data;
using social_media.Data.Constants;
using social_media.Data.Models;
using social_media.Data.Services;
using social_media.ViewModels.Setting;
using System.Security.Claims;

namespace social_media.Controllers
{
    [Authorize(Roles =$"{AppRoles.User},{AppRoles.Admin}")]
    public class SettingsController : Controller
    {
        private readonly AppDBContext _dbContext;
        private readonly IUserService _userService;
        private readonly IFilesService _filesService;

        public SettingsController(AppDBContext dbContext , IUserService userservice, IFilesService filesService)
        {
            _dbContext = dbContext;
            _userService = userservice;
            _filesService = filesService;
        }
        public async Task<IActionResult> Index()
        {
            int loggeduser = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _userService.GetUser(loggeduser);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfilePicture(ProfilePictureVM profilePictureVM)
        {
            int loggeduser = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var profilepictureurl =await _filesService.UploadImageAsync(profilePictureVM.ProfilePictureImage);

            await _userService.UpdateProfilePicture(loggeduser, profilepictureurl);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UpdatePassword()
        {
            return View();
        }
        public async Task<IActionResult> UpdateProfile()
        {
            return View();
        }

    }
}
