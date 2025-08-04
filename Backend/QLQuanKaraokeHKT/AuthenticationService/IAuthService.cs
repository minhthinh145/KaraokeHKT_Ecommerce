using QLQuanKaraokeHKT.Models;

namespace QLQuanKaraokeHKT.AuthenticationService
{
    public interface IAuthService
    {
        /// <summary>
        /// Generate an access token for a user by their ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Token</returns>
        Task<string> GenerateAccessTokenAsync(Guid userId);
        /// <summary>
        /// Generate an access token for a user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<string> GenerateAccessTokenForUserAsync(TaiKhoan user);
        /// <summary>
        /// Generate both access and refresh tokens for a user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<(string AccessToken, string RefreshToken)> GenerateTokensAsync(TaiKhoan user);
        /// <summary>
        /// Generate a refresh token for a user.
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        Task<string> RefreshAccessTokenAsync(string refreshToken);
        /// <summary>
        /// Save a refresh token for a user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="refreshToken"></param>
        /// <param name="expires"></param>
        /// <returns></returns>
        Task SaveRefreshTokenAsync(Guid userId, string refreshToken, DateTime expires);
        /// <summary>
        /// Revoke a refresh token for a user.
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        Task RevokeRefreshTokenAsync(string refreshToken);
    }
}