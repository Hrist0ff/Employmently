using System.ComponentModel.DataAnnotations;

namespace employmently_be.Data.Models
{
    public class LoginDto
    {

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

    }
}
