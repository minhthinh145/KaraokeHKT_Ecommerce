using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Models;
using QLQuanKaraokeHKT.Repositories.Interfaces;

namespace QLQuanKaraokeHKT.Repositories.Implementation
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly QlkaraokeHktContext _context;

        public RefreshTokenRepository(QlkaraokeHktContext context)
        {
            _context = context;
        }
        public async Task<RefreshToken> FindByTokenAsync(string token)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.ChuoiRefreshToken == token);
        }

        public async Task RevokeAsync(string token)
        {
            var refreshToken = await FindByTokenAsync(token);
            if(refreshToken != null)
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
                MaTaiKhoan = userId
            };
            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();
        }
    }
}
