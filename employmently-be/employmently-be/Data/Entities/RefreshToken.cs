using System.ComponentModel.DataAnnotations;

namespace employmently_be.Data.Entities
{
    public class RefreshToken
    {
        [Key]
        public string Token { get; set; }

        public string UserId { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}
