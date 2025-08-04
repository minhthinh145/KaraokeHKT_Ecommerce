using QLQuanKaraokeHKT.Models;

namespace QLQuanKaraokeHKT.Repositories.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task SaveAsync(Guid userId, string token, DateTime created, DateTime expires);
        Task<RefreshToken> FindByTokenAsync(string token);
        Task RevokeAsync(string token);
    }
}
