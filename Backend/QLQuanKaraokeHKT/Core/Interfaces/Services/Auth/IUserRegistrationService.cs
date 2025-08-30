using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Core.Entities;

namespace QLQuanKaraokeHKT.Core.Interfaces.Services.Auth
{
    /// <summary>
    /// Handles user registration logic
    /// </summary>
    public interface IUserRegistrationService
    {
        /// <summary>
        /// Validate registration data
        /// </summary>
        /// <param name="signUpDto">Registration data</param>
        /// <returns>Validation result</returns>
        Task<ServiceResult> ValidateRegistrationDataAsync(SignUpDTO signUpDto);

        /// <summary>
        /// Create user account with customer profile
        /// </summary>
        /// <param name="signUpDto">Registration data</param>
        /// <returns>Created user and result</returns>
        Task<(bool Success, TaiKhoan? User, string[] Errors)> CreateUserAccountAsync(SignUpDTO signUpDto);

        /// <summary>
        /// Send account verification email
        /// </summary>
        /// <param name="user">Created user</param>
        /// <returns>Success result</returns>
        Task<ServiceResult> SendAccountVerificationEmailAsync(TaiKhoan user);
    }
}