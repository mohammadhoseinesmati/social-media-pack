using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using social_media.Data;
using social_media.Data.Constants;
using social_media.Data.Services;
using System.Security.Claims;

namespace social_media.Controllers
{
    [Authorize(Roles = AppRoles.User)]
    public class FavoritesController : Controller
    {
        private readonly AppDBContext _dbContext;
        private readonly IPostService _postService;

        public FavoritesController(AppDBContext dbContext, IPostService postService)
        {
            _dbContext = dbContext;
            _postService = postService;
        }

        public async Task<IActionResult> Index()
        {
            int loggeduser = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var favoritesposts = await _postService.SelectFavoritePostAsync(loggeduser);
            return View(favoritesposts);
        }
    }
}
