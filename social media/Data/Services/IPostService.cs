using social_media.Data.DTOS;
using social_media.Data.Models;

namespace social_media.Data.Services
{
    public interface IPostService
    {
        Task<List<Post>> GetAllPostsAsync(int LoggedUserId);

        Task CreatePostAsync(Post post , IFormFile image);
        Task<List<Post>> SelectFavoritePostAsync(int userid);
        Task<Post> GetPostByIdAsync(int postid);

        Task<Post> RemovePostAsync(int postid);

        Task AddCommentPostAsync(Comment comment);
        Task RemoveCommentPostAsync(int commentid);

        Task<GetNotificationDTO> TogglePostLikeAsync(int postid , int userid);
        Task TogglePostVisibilityAsync(int postid , int userid);
        Task<GetNotificationDTO> TogglePostFavoriteAsync(int postid , int userid);
        Task AddPostReportAsync(int postid , int userid);


    }
}
