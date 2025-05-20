namespace social_media.ViewModels.Friends
{
    public class UserWithFriendsCountVM
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public int Count { get; set; }
        public string PictureUrl { get; set; }

        public string FriendsCountDisplay => Count == 0 ? "No Followers" :
            Count == 1 ? "1 Followers" : $"{Count} Followers";
    }
}
