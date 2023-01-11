using System.ComponentModel.DataAnnotations;

namespace employmently_be.Data.Models.DTOs
{
    public class RefreshTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
