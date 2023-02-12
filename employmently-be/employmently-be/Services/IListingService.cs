using employmently_be.Data.Entities;

namespace employmently_be.Services
{
    public interface IListingService
    {
        void UpdateListingExpiredStatus(Listing listing);
        void UpdateAllListingExpiredStatuses();
    }
}
