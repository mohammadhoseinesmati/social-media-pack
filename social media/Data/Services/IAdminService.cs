using social_media.Data.Models;

namespace social_media.Data.Services
{
    public interface IAdminService
    {
        Task<List<Post>> GetReportedPostAsync();
        Task ApproveReportAsync(int postId);
        Task RejectReportAsync(int postId);
    }
}
