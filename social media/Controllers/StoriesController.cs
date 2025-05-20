using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using social_media.Data;
using social_media.Data.Constants;
using social_media.Data.Models;
using social_media.Data.Services;
using social_media.ViewModels.Stories;
using System.Security.Claims;

namespace social_media.Controllers
{
    [Authorize(Roles = AppRoles.User)]
    public class StoriesController : Controller
    {
        private readonly AppDBContext _dbContext;
        private readonly IStoryService _storyService;

        public StoriesController(AppDBContext dbContext,IStoryService storyService )
        {
            _dbContext = dbContext;
            _storyService = storyService;
        }
        public async Task< IActionResult> Index()
        {
             var stories = await _storyService.GetStoryListAsync();
            return View(stories);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStory(StoryVM storyvm)
        {
            int logeduser = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var story = new Story()
            {
                Created = DateTime.UtcNow,
                ImageUrl = "",
                UserId = logeduser,
            };

            await _storyService.Create(story , storyvm.Image);

            return RedirectToAction("Index");
        }
    }
}
