using employmently_be.Data.Entities;

namespace employmently_be.Data.Models.ViewModels
{
    public class MyApplicationsViewModel
    {
        public string? ListingName { get; set; }
        public string? ListingCompany { get; set; }
        public string listingStatus { get; set; }
        public string listingLocation { get; set; }
        public int? listingSalary { get; set; }
        public string CV { get; set; }
        public string ML { get; set; }
        public DateTime applicationTime { get; set; }
        public string? rejectionPurpose { get; set; }
        public DateTime? suggestedInterviewDate { get; set; }
        public string? arrangement { get; set; }
        public string? companyPic { get; set; }
        public int? listingId { get; set; }
        public int? companyId { get; set; }
    }
}
