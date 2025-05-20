
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using social_media.Controllers;
using social_media.Data;

namespace CircleApp.ViewComponents
{
    public class StoriesViewComponent:ViewComponent
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDBContext _dbContext;

        public StoriesViewComponent(ILogger<HomeController> logger, AppDBContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var allStories = await _dbContext.Stories
                .Where(p => p.Created >= DateTime.UtcNow.AddHours(-24))
                .Include(x => x.User)
                .ToListAsync();
            return View(allStories);
        }
    }
}
