using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace social_media.Data.Models
{
    public class User :IdentityUser<int>
    {
        [MaxLength(500)]
        public string FullName { get; set; }

        [MaxLength(100)]
        public override string UserName { get; set; }
        public string?ProfilePictureUrl { get; set; }
        public string?Bio { get; set; }

        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<Story> Stories { get; set; } = new List<Story>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
        public ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}
