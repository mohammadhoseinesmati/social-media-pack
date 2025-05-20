namespace social_media.Data.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public User User { get; set; }
        public Post Post { get; set; }

    }
}
