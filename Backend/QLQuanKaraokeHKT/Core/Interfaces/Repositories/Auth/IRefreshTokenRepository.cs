using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Base;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.Auth
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken, int>
    {
        Task<RefreshToken?> FindByTokenAsync(string token);

        Task<bool> RevokeTokenAsync(string token);

        Task<int> RevokeAllByUserIdAsync(Guid userId);

        Task<IList<RefreshToken>> GetActiveTokensByUserIdAsync(Guid userId);

        Task<int> CleanupExpiredTokensAsync();

        Task<bool> IsTokenValidAsync(string token);

    }
}