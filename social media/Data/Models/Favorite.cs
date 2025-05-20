namespace social_media.Data.Models
{
    public class Favorite
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public DateTime Created {  get; set; }

        public User User { get; set; }
        public Post Post { get; set; }
    }
}
