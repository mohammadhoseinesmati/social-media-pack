namespace social_media.Data.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }

        public bool IsRead { get; set; }
        public string Type { get; set; }

        public int? PostId { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
