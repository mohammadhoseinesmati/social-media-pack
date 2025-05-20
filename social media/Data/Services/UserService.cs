using Microsoft.EntityFrameworkCore;
using social_media.Data.Models;

namespace social_media.Data.Services
{
    public class UserService : IUserService
    {
        private readonly AppDBContext _dbContext;

        public UserService(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<User> GetUser(int userid)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(p => p.Id == userid);
            return user;         
        }

        public async Task<List<Post>> GetUserPosts(int id)
        {
            var posts = await _dbContext.Posts
                .Where(p => (!p.IsPrivate && p.UserId == id) && p.NrOfReports < 5 && !p.IsDeleted)
                .Include(x => x.User)
                .Include(y => y.Likes)
                .Include(x => x.Favorites)
                .Include(x => x.Comments).ThenInclude(l => l.User)
                .OrderByDescending(x => x.Created)
                .ToListAsync();

            return posts;
        }

        public async Task UpdateProfilePicture(int userid, string pictureurl)
        {
            var userdb = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userid);
            if (userdb != null) 
            {
                userdb.ProfilePictureUrl = pictureurl;
                _dbContext.Users.Update(userdb);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
