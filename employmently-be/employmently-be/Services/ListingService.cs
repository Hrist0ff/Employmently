using employmently_be.Data.Entities;
using employmently_be.DbContexts;
using Hangfire;

namespace employmently_be.Services
{
    public class ListingService : IListingService
    {
        public void UpdateListingExpiredStatus(Listing listing)
        {
            listing.Expired = DateTime.Now >= listing.ExpirationDate;
            if (listing.Expired)
            {
                listing.Status = ListingStatus.Expired;
            }

        }
        public void UpdateAllListingExpiredStatuses()
        {
            using (var context = new dbContext())
            {
                var listings = context.Listings.ToList();
                foreach (var listing in listings)
                {
                    UpdateListingExpiredStatus(listing);
                }
                context.SaveChanges();
            }
        }
    }
}
