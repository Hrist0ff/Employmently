using employmently_be.Data.Entities;
using employmently_be.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace employmently_be.Services
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly dbContext _dbContext;

        public RefreshTokenRepository(dbContext context)
        {
            _dbContext = context;
        }

        public async Task<RefreshToken> GetAsync(string token)
        {
            return await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);
        }

        public async Task<List<RefreshToken>> GetExpiredAsync()
        {
            return await _dbContext.RefreshTokens.Where(x => x.ExpirationDate < DateTime.UtcNow).ToListAsync();
        }

        public async Task CreateAsync(RefreshToken refreshToken)
        {
            _dbContext.RefreshTokens.Add(refreshToken);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(RefreshToken refreshToken)
        {
            _dbContext.RefreshTokens.Remove(refreshToken);
            await _dbContext.SaveChangesAsync();
        }
        public async Task Logout(string refreshToken)
        {
            var token = await GetAsync(refreshToken);
            if (token != null)
            {
                await DeleteAsync(token);
            }
        }

        public async Task<RefreshToken> GetRefreshToken(string userId)
        {
            return await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<DateTime> GetExpireDateOfToken(string refreshToken)
        {
            var token = await GetAsync(refreshToken);
            return token.ExpirationDate;
        }
    }

}
