using System.ComponentModel.DataAnnotations;

namespace employmently_be.Data.Models.DTOs
{

    public class RefreshTokenResponse
    {
        [Required]
        public string AccessToken { get; set; }
    }
}
