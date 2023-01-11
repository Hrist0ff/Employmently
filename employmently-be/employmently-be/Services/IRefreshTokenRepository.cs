using employmently_be.Data.Entities;

namespace employmently_be.Services
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> GetAsync(string token);
        Task<List<RefreshToken>> GetExpiredAsync();
        Task CreateAsync(RefreshToken refreshToken);
        Task DeleteAsync(RefreshToken refreshToken);
        Task Logout(string refreshToken);
        Task<RefreshToken> GetRefreshToken(string userId);
        Task<DateTime> GetExpireDateOfToken(string refreshToken);
    }
}
