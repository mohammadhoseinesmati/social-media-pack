using Microsoft.EntityFrameworkCore;
using social_media.Data.DTOS;
using social_media.Data.Helpers;
using social_media.Data.Models;
using social_media.Hubs;
using social_media.ViewModels.Home;

namespace social_media.Data.Services
{
    public class PostService : IPostService
    {
        private readonly AppDBContext _dbContext;
        private readonly IFilesService _filesService;
        private readonly INotificationService _notificationService;
        private readonly IHashtagService _hashtagService;

        public PostService(AppDBContext dbContext, IFilesService filesService 
            , INotificationService notificationService, IHashtagService hashtagService)
        {
            _dbContext = dbContext;
            _filesService = filesService;
            _notificationService = notificationService;
            _hashtagService = hashtagService;
        }
        public async Task AddCommentPostAsync(Comment comment)
        {          
            await _dbContext.Comments.AddAsync(comment);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<List<Post>> SelectFavoritePostAsync(int userid)
        {
            var allposts = await _dbContext.Favorites
                .Include(p => p.Post.Reports)
                .Include(p => p.Post.User)
                .Include(p => p.Post.Comments)
                    .ThenInclude(x => x.User)
                .Include(p => p.Post.Likes)
                .Include(p => p.Post.Favorites)
                .Where(p => p.UserId == userid && !p.Post.IsDeleted && p.Post.NrOfReports < 5)
                .Select(p => p.Post)
                .ToListAsync();
            
            return allposts;
        }

        public async Task CreatePostAsync(Post post, IFormFile image)
        {
            int logeduser = 1;

            post.ImageUrl = await _filesService.UploadImageAsync(image);

            await _dbContext.Posts.AddAsync(post);
            await _dbContext.SaveChangesAsync();

            
        }

        public async Task<List<Post>> GetAllPostsAsync(int LoggedUserId)
        {
            var posts = await _dbContext.Posts
                .Where(p => (!p.IsPrivate || p.UserId == LoggedUserId) && p.NrOfReports < 5 && !p.IsDeleted)
                .Include(x => x.User)
                .Include(y => y.Likes)
                .Include(x => x.Favorites)
                .Include(x => x.Comments).ThenInclude(l => l.User)
                .OrderByDescending(x => x.Created)
                .ToListAsync();
            return posts;
        }

        public async Task RemoveCommentPostAsync(int commentid)
        {
            var comment = await _dbContext.Comments.FirstOrDefaultAsync(p => p.Id == commentid);

            if (comment != null)
            {
                _dbContext.Comments.Remove(comment);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<Post> RemovePostAsync(int postid)
        {
            var post = await _dbContext.Posts.FirstOrDefaultAsync(p => p.Id == postid);

            if (post != null)
            {
                post.IsDeleted = true;
                _dbContext.Posts.Update(post);
                await _dbContext.SaveChangesAsync();
                await _hashtagService.ProccessHashtagForRemovePostAsync(post.Content);
            }
            return post;
        }

        public async Task<GetNotificationDTO> TogglePostFavoriteAsync(int postid, int userid)
        {
            var respons = new GetNotificationDTO
            {
                SendNotification = false,
                Success = false,
            };

            var favorite = await _dbContext.Favorites
                .Where(p => p.PostId == postid && p.UserId == userid)
                .FirstOrDefaultAsync();
            if (favorite != null)
            {
                _dbContext.Favorites.Remove(favorite);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                var newfavorite = new Favorite()
                {
                    PostId = postid,
                    UserId = userid,
                    Created = DateTime.UtcNow
                };
                await _dbContext.Favorites.AddAsync(newfavorite);
                await _dbContext.SaveChangesAsync();

                respons.SendNotification = true;
            }
            respons.Success = true;

            return respons;
        }

        public async Task<GetNotificationDTO> TogglePostLikeAsync(int postid, int userid)
        {
            var response = new GetNotificationDTO
            {
                Success = false,
                SendNotification = false,
            };

            var like = await _dbContext.Likes
                .Where(p => p.PostId == postid && p.UserId == userid)
                .FirstOrDefaultAsync();
            if (like != null)
            {
                _dbContext.Likes.Remove(like);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                var newlike = new Like()
                {
                    PostId = postid,
                    UserId = userid
                };
                await _dbContext.Likes.AddAsync(newlike);
                await _dbContext.SaveChangesAsync();

                response.SendNotification = true;
            }
            response.Success = true;
            return response;
        }

        public async Task AddPostReportAsync(int postid, int userid)
        {
            var post = await _dbContext.Posts
                .FirstOrDefaultAsync(p => p.Id == postid);
            var report = new Report()
            {
                PostId = postid,
                UserId = userid,
                Created = DateTime.UtcNow,
            };
            await _dbContext.Reports.AddAsync(report);
            await _dbContext.SaveChangesAsync();

            //var post = await _dbContext.Posts
            //    .FirstOrDefaultAsync(p => p.Id == postid);

            if (post != null)
            {
                post.NrOfReports += 1;
                _dbContext.Posts.Update(post);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task TogglePostVisibilityAsync(int postid, int userid)
        {
            var post = await _dbContext.Posts
                .FirstOrDefaultAsync(p => p.Id == postid && p.UserId == userid);

            if (post != null)
            {
                post.IsPrivate = !post.IsPrivate;
                _dbContext.Posts.Update(post);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<Post> GetPostByIdAsync(int postid)
        {
            var post = await _dbContext.Posts
                .Include(x => x.User)
                .Include(y => y.Likes)
                .Include(x => x.Favorites)
                .Include(x => x.Comments).ThenInclude(l => l.User)
                .Where(p => p.Id == postid)
                .FirstOrDefaultAsync();

            return post;
        }
    }
}
