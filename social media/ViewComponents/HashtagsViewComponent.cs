
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using social_media.Data;

namespace CircleApp.ViewComponents
{
    public class HashtagsViewComponent: ViewComponent
    {
        private readonly AppDBContext _dbContext;
        public HashtagsViewComponent(AppDBContext context)
        {
            _dbContext = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var oneWeekAgoNow = DateTime.UtcNow.AddDays(-30);

            var top3Hashtags = await _dbContext.Hashtags
                .Where(h => h.Created >= oneWeekAgoNow)
                .OrderByDescending(n => n.Count)
                .Take(3)
                .ToListAsync();

            return View(top3Hashtags);
        }
    }
}
