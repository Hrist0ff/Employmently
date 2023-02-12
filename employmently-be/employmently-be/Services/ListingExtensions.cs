using employmently_be.Data.Entities;

namespace employmently_be.Services
{
    public static class ListingExtensions
    {
        public static IEnumerable<Listing> GetPendingListings(this IEnumerable<Listing> listings)
        {
            return listings.Where(l => l.Status == 0);
        }


    }
}
