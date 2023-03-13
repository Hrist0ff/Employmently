using System.ComponentModel.DataAnnotations;

namespace employmently_be.Data.Models
{
    public class ChangePasswordDto
    {
        public string userId { get; set; }
        public string token { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

    }
}
