namespace employmently_be.Data.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Listing> Listings { get; set;}
    }
}
