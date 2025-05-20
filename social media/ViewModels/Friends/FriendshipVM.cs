using social_media.Data.Models;

namespace social_media.ViewModels.Friends
{
    public class FriendshipVM
    {
        public List<FriendRequest> FriendRequestsSent = new List<FriendRequest>();
        public List<FriendRequest> FriendRequestsRecive = new List<FriendRequest>();
        public List<Friendship> Friends = new List<Friendship>();
    }
}
