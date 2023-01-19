using employmently_be.Data.Entities;

namespace employmently_be.Data.Models.ViewModels
{
    public class MyApplicationsViewModel
    {
        public string? ListingName { get; set; }
        public string? ListingCompany { get; set; }
        public string listingStatus { get; set; }
        public DateTime applicationTime { get; set; }
    }
}
