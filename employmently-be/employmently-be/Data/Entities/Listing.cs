using employmently_be.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace employmently_be.Data.Entities
{
    public class Listing
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? Description   { get; set; }

        public virtual IList<Category> Categories { get; set; } = new List<Category>();
        public User Author { get; set; }

        public ListingStatus Status { get; set; } = ListingStatus.Pending;


    }
}
