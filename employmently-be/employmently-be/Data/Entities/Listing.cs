using Microsoft.EntityFrameworkCore;

namespace employmently_be.Data.Entities
{
    public class Listing
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? Description   { get; set; }
        
        public virtual ICollection<Category> Categories { get; set; }


    }
}
