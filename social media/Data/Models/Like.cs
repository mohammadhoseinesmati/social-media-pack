using social_media.Data.Models;

namespace social_media.Data
{
    public class Like
    {
        public int id {  get; set; }

        public int UserId { get; set; }
        public int PostId { get; set; }


        public User User { get; set; }
        public Post Post { get; set; }

    }
}
