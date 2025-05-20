using social_media.Data.Models;

namespace social_media.Data.DTOS
{
    public class UserWithFriendsDTO
    {
        public User User { get; set; }
        public int FriendsCount { get; set; }
    }
}
