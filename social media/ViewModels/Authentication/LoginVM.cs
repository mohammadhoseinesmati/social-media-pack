using System.ComponentModel.DataAnnotations;

namespace social_media.ViewModels.Authentication
{
    public class LoginVM
    {
        [Required(ErrorMessage ="Email is reauired")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Password is reauired")]
        public string Password { get; set; }
    }
}
