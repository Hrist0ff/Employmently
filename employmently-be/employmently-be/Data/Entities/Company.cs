using employmently_be.Entities;

namespace employmently_be.Data.Entities
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UniqueIdentifier { get; set; }
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}