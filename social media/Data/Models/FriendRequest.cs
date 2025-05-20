namespace social_media.Data.Models
{
    public class FriendRequest
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public DateTime Created {  get; set; }
        public DateTime Updated { get; set; }

        public int SenderId { get; set; }
        public User Sender { get; set; }
        public int ReciverId {  get; set; }
        public User Reciver { get; set; }
    }
}
