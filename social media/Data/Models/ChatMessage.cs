using System.ComponentModel.DataAnnotations;

namespace social_media.Data.Models
{
    public class ChatMessage
    {
        [Key]
        public int Id {  get; set; }
        public string Message { get; set; }         
        public DateTime TimeSend {  get; set; }
        public bool IsRead { get; set; } = false;

        public int SenderId { get; set; }
        public User Sender { get; set; }

        public int ReciverId { get; set; }
        public User Reciver { get; set; }
    }
}
