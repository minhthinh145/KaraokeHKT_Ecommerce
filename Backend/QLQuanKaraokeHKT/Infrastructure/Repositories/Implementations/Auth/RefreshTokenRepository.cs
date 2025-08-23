using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Auth;
using QLQuanKaraokeHKT.Infrastructure.Data;

namespace QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.Auth
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly QlkaraokeHktContext _context;

        public RefreshTokenRepository(QlkaraokeHktContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken?> FindByTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.ChuoiRefreshToken == token);
        }

        public async Task RevokeAsync(string token)
        {
            var refreshToken = await FindByTokenAsync(token);
            if (refreshToken != null)
            {
                refreshToken.Revoked = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task SaveAsync(Guid userId, string token, DateTime created, DateTime expires)
        {
            var refreshToken = new RefreshToken
            {
                ChuoiRefreshToken = token,
                ThoiGianTao = created,
                ThoiGianHetHan = expires,
                MaTaiKhoan = userId,
                Revoked = null // Ensure it's not revoked when created
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task RevokeAllByUserIdAsync(Guid userId)
        {
            var activeTokens = await _context.RefreshTokens
                .Where(rt => rt.MaTaiKhoan == userId && rt.Revoked == null)
                .ToListAsync();

            foreach (var token in activeTokens)
            {
                token.Revoked = DateTime.UtcNow;
            }

            if (activeTokens.Any())
            {
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IList<RefreshToken>> GetActiveTokensByUserIdAsync(Guid userId)
        {
            return await _context.RefreshTokens
                .Where(rt => rt.MaTaiKhoan == userId &&
                            rt.Revoked == null &&
                            rt.ThoiGianHetHan > DateTime.UtcNow)
                .OrderByDescending(rt => rt.ThoiGianTao)
                .ToListAsync();
        }

        public async Task<int> CleanupExpiredTokensAsync()
        {
            var expiredTokens = await _context.RefreshTokens
                .Where(rt => rt.ThoiGianHetHan < DateTime.UtcNow || rt.Revoked != null)
                .ToListAsync();

            if (expiredTokens.Any())
            {
                _context.RefreshTokens.RemoveRange(expiredTokens);
                await _context.SaveChangesAsync();
            }

            return expiredTokens.Count;
        }

        public async Task<bool> IsTokenValidAsync(string token)
        {
            var refreshToken = await FindByTokenAsync(token);

            if (refreshToken == null)
                return false;

            // Check if token is not revoked and not expired
            return refreshToken.Revoked == null &&
                   refreshToken.ThoiGianHetHan > DateTime.UtcNow;
        }

        public async Task<int> CountActiveTokensByUserIdAsync(Guid userId)
        {
            return await _context.RefreshTokens
                .CountAsync(rt => rt.MaTaiKhoan == userId &&
                                 rt.Revoked == null &&
                                 rt.ThoiGianHetHan > DateTime.UtcNow);
        }
    }
}