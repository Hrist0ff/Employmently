using System.ComponentModel.DataAnnotations;

namespace employmently_be.Data.Models
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
