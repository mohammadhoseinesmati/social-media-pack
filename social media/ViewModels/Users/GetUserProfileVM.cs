using social_media.Data.Models;

namespace social_media.ViewModels.Users
{
    public class GetUserProfileVM
    {
        public User User {  get; set; }
        public List<Post> Posts { get; set; }
    }
}
