using System.ComponentModel.DataAnnotations;

namespace social_media.Data.Models
{
    public class Story
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime Created { get; set; }

        //forignkey
        public int UserId { get; set; }

        public User User { get; set; }
    }
}
