using social_media.Data.Models;

namespace social_media.Data.Services
{
    public interface IUserService
    {
        Task<User> GetUser(int userid);
        Task UpdateProfilePicture(int userid , string pictureurl);
        Task<List<Post>> GetUserPosts(int id);
    }
}
