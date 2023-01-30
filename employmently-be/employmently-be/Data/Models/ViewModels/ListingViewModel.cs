namespace employmently_be.Data.Models.ViewModels
{
    public class ListingViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        // include other properties of the listing here
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string? AuthorPic { get; set; }
        public IEnumerable<string> CategoryNames { get; set; }
        public string? Location { get; set; }
        public string? Arrangement { get; set; }
        public int? Salary { get; set; }
        public int? CompanyId { get; set; }


    }

}
