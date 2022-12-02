using System.ComponentModel.DataAnnotations;

namespace employmently_be.Models
{
    public class PointOfInterestForCreationDto
    {
        [Required]
        [MaxLength(100)]

        public string Name { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? Description { get; set; }
    }
}
