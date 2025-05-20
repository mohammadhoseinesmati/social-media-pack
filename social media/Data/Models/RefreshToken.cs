using Microsoft.IdentityModel.Tokens;

namespace social_media.Data.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpiredDate { get; set; }
        public bool IsExpired { get; set; }


        public int UserId { get; set; }
        public User User { get; set; }
    }
}
