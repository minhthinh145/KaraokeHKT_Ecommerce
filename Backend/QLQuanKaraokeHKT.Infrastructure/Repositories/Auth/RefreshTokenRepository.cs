using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Application.Repositories.Auth;
using QLQuanKaraokeHKT.Infrastructure.Data;

namespace QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.Auth
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken,int>, IRefreshTokenRepository
    {

        public RefreshTokenRepository(QlkaraokeHktContext context) : base(context)
        {

        }


        public async Task<RefreshToken?> FindByTokenAsync(string token)
        {
            return await _dbSet
                .FirstOrDefaultAsync(rt => rt.ChuoiRefreshToken == token);
        }

        public async Task<bool> RevokeTokenAsync(string token)
        {
            var refreshToken = await FindByTokenAsync(token);
            if (refreshToken == null)
            {
             return false;
            }
            refreshToken.Revoked = DateTime.UtcNow;
            return await UpdateAsync(refreshToken);
        }

        public async Task<int> RevokeAllByUserIdAsync(Guid userId)
        {
            var activeTokens = await GetAllAsync(rt => rt.MaTaiKhoan == userId && rt.Revoked == null);

            if (!activeTokens.Any())
                return 0;

            foreach (var token in activeTokens)
            {
                token.Revoked = DateTime.UtcNow;
            }

            var count = 0;
            foreach (var token in activeTokens)
            {
                if (await UpdateAsync(token))
                    count++;
            }
            return count;
        }

        public async Task<IList<RefreshToken>> GetActiveTokensByUserIdAsync(Guid userId)
        {
            return await GetAllAsync(rt => rt.MaTaiKhoan == userId &&
                                        rt.Revoked == null &&
                                        rt.ThoiGianHetHan > DateTime.UtcNow);
        }

        public async Task<int> CleanupExpiredTokensAsync()
        {
            var expiredTokens = await _context.RefreshTokens
                .Where(rt => rt.ThoiGianHetHan < DateTime.UtcNow || rt.Revoked != null)
                .ToListAsync();

            if (!expiredTokens.Any())
                return 0;

            _context.RefreshTokens.RemoveRange(expiredTokens);
            await SaveChangesAsync(); 

            return expiredTokens.Count;
        }

        public async Task<bool> IsTokenValidAsync(string token)
        {
            return await ExistsAsync(rt => rt.ChuoiRefreshToken == token &&
                                         rt.Revoked == null &&
                                         rt.ThoiGianHetHan > DateTime.UtcNow);
        }
    }
}