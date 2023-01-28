namespace employmently_be.Data.Entities
{
    public class ListingApplications
    {
        public int Id { get; set; }
        public string userId { get; set; }
        public int listingId { get; set; }
        public ListingStatus status { get; set; } = ListingStatus.Pending;
        public DateTime applicationTime { get; set; }
        public string CV { get; set; }
        public string? motivationalLetter { get; set; }
    }
}
