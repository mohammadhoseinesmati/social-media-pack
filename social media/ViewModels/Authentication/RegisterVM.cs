using System.ComponentModel.DataAnnotations;

namespace social_media.ViewModels.Authentication
{
    public class RegisterVM
    {
        [Required(ErrorMessage ="firstname is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "lastname is required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "password is required")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage ="passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
