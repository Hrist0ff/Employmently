namespace employmently_be.Data.Models.ViewModels
{
    public class MyCompanyViewModel
    {
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string UniqueIdentifier { get; set; }
        public string? Description { get; set; }
        public string? YearCreated { get; set; }
        public string ProfilePicture { get; set; }
        public string Employees { get; set; }
        public string PhoneNumber { get; set; }
        public IEnumerable<string> Technologies { get; set; }
    }
}
