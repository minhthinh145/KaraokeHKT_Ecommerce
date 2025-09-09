using QLQuanKaraokeHKT.Core.Entities;

namespace QLQuanKaraokeHKT.Core.Interfaces.Services.Auth
{
    public interface ITokenService
    {
        Task<string> GenerateAccessTokenAsync(Guid userId);

        Task<string> GenerateAccessTokenForUserAsync(TaiKhoan user);

        Task<(string AccessToken, string RefreshToken)> GenerateTokensAsync(TaiKhoan user);

        Task<string> RefreshAccessTokenAsync(string refreshToken);

        Task SaveRefreshTokenAsync(Guid userId, string refreshToken, DateTime expires);

        Task RevokeRefreshTokenAsync(string refreshToken);

        Task<bool> RevokeAllUserRefreshTokensAsync(Guid userId);

        Task<bool> IsRefreshTokenValidAsync(string refreshToken);

        Task<int> CleanupExpiredRefreshTokensAsync();
    }
}