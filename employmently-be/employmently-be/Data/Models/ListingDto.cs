using employmently_be.Data.Entities;
using employmently_be.Entities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace employmently_be.Data.Models
{
    public class ListingDto
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; }
        
        //public virtual ICollection<Category> Categories { get; set; }


    }
}
