using Microsoft.EntityFrameworkCore;
using social_media.Data.Models;

namespace social_media.Data.Services
{
    public class AdminService : IAdminService
    {
        private readonly AppDBContext _dbContext;

        public AdminService(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Post>> GetReportedPostAsync()
        {
            var posts = await _dbContext.Posts
                .Include(p => p.Reports)
                .Include(n => n.User)
                .Where(p => p.NrOfReports > 0 && !p.IsDeleted)
                .ToListAsync();

            return posts;
        }

        public async Task RejectReportAsync(int postId)
        {
            var post = await _dbContext.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            if (post != null)
            {
                post.NrOfReports = 0;
                post.IsDeleted = false;

                _dbContext.Posts.Update(post);
                await _dbContext.SaveChangesAsync();
            }

            var reportpost = await _dbContext.Reports.Where(p => p.PostId == postId).ToListAsync();

            if (reportpost.Any())
            {
                _dbContext.Reports.RemoveRange(reportpost);
                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task ApproveReportAsync(int postId)
        {
            var post = await _dbContext.Posts.FirstOrDefaultAsync(p => p.Id == postId);

            if (post != null)
            {
                post.IsDeleted = true;
                _dbContext.Posts.Update(post);
                await _dbContext.SaveChangesAsync();
            }
        }
    }

    
    //Task RejectReportAsync();
}
