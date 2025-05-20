using social_media.Data.Models;

namespace social_media.Data.Services
{
    public interface IStoryService
    {
        Task<List<Story>> GetStoryListAsync();
        Task<Story> Create(Story story, IFormFile image);
    }
}
