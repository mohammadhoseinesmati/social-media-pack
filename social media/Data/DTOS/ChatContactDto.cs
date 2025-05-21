namespace social_media.Data.DTOS
{
    public class ChatContactDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string LastMessage { get; set; }
        public DateTime LastTimeMessage { get; set; }
    }
}
