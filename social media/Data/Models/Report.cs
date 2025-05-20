using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace social_media.Data.Models
{
    public class Report
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public int PostId { get; set; }
        public DateTime Created { get; set; }

        public User User { get; set; }
        public Post Post { get; set; }
    }
}
