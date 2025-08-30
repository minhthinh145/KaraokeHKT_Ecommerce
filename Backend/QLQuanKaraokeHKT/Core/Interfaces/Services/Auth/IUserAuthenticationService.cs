using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Core.Entities;

namespace QLQuanKaraokeHKT.Core.Interfaces.Services.Auth
{
    /// <summary>
    /// Handles user authentication logic - Single Responsibility
    /// </summary>
    public interface IUserAuthenticationService
    {
        /// <summary>
        /// Validate user credentials
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="password">User password</param>
        /// <returns>User entity if valid, null otherwise</returns>
        Task<TaiKhoan?> ValidateUserCredentialsAsync(string email, string password);

        /// <summary>
        /// Check if user account is active and can sign in
        /// </summary>
        /// <param name="user">User entity</param>
        /// <returns>True if account is active</returns>
        bool IsAccountActive(TaiKhoan user);

        /// <summary>
        /// Check password for user
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="password">Password to check</param>
        /// <returns>True if password is correct</returns>
        Task<bool> CheckPasswordAsync(Guid userId, string password);

        /// <summary>
        /// Generate complete login response
        /// </summary>
        /// <param name="user">Authenticated user</param>
        /// <returns>Login response with tokens</returns>
        Task<LoginResponseDTO> GenerateLoginResponseAsync(TaiKhoan user);
    }
}