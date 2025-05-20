using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using social_media.Data;
using social_media.Data.Constants;
using social_media.Data.Models;
using social_media.Data.Services;
using social_media.ViewModels.Users;

namespace social_media.Controllers
{
    [Authorize(Roles = AppRoles.User)]
    public class UsersController : Controller
    {
        private readonly UserManager<User> _usermanager;
        private readonly AppDBContext _dbcontext;
        private readonly IUserService _userService;

        public UsersController(IUserService userService , UserManager<User> userManager , AppDBContext dBContext)
        {
            _userService = userService;
            _usermanager = userManager;
            _dbcontext = dBContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Details(int userid)
        {
            var newuser = await _dbcontext.Users.FirstOrDefaultAsync(p => p.Id == userid);
            var newposts =await _userService.GetUserPosts(userid);
            var profilevm = new GetUserProfileVM()
            {
                User = newuser,
                Posts = newposts,
            };

            return View(profilevm);
        }
    }
}
