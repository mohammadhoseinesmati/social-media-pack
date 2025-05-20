using Microsoft.EntityFrameworkCore;
using social_media.Data.Models;
using social_media.ViewModels.Stories;

namespace social_media.Data.Services
{
    public class StoryService : IStoryService
    {
        private readonly AppDBContext _dbContext;
        private readonly IFilesService _filesService;

        public StoryService(AppDBContext dbContext, IFilesService filesService)
        {
            _dbContext = dbContext;
            _filesService = filesService;
        }

        public async Task<Story> Create(Story story, IFormFile image)
        {
            story.ImageUrl = await _filesService.UploadImageAsync(image);

            await _dbContext.Stories.AddAsync(story);
            await _dbContext.SaveChangesAsync();

            return story;
        }

        public async Task<List<Story>> GetStoryListAsync()
        {
            var stories = await _dbContext.Stories
                .Where(x => x.Created <= DateTime.Now.AddHours(-24))
                .Include(p => p.User)
                .ToListAsync();
            return stories;
        }
    }
}
