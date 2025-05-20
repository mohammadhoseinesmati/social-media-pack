namespace social_media.Data.Models
{
    public class Friendship
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }

        public int SenderId { get; set; }
        public virtual User Sender { get; set; }

        public int ReciverId { get; set; }
        public virtual User Reciver { get; set; }
    }
}
