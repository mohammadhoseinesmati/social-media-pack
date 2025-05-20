namespace social_media.Data.Models
{
    public class Hashtag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public int Count { get; set; }
    }
}
