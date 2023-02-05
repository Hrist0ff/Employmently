using employmently_be.Data.Entities;

namespace employmently_be.Data.Models.ViewModels
{
    public class ApplicationToCompanyView
    {
        public int id { get; set; }
        public string cv { get; set; }
        public string motivationalLetter { get; set; }
        public int listingId { get; set; }
        public string listingName { get; set; }
        public string? rejectionPurpose { get; set; }
        public ListingStatus status { get; set; }
        public DateTime? suggestedInterviewDate { get; set; }
        public string userId { get; set; }
        public string userName { get; set; }
        public string? userDescription { get; set; }
        public string userPic { get; set; }
        public IEnumerable<string> listingCategories { get; set; }
        public string? listingArrangement { get; set; }
        public DateTime applicationTime { get; set; }






    }
}
