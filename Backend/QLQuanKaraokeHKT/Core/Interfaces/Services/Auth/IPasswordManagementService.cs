using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Core.Entities;

namespace QLQuanKaraokeHKT.Core.Interfaces.Services.Auth
{
    /// <summary>
    /// Handles password operations - Single Responsibility
    /// </summary>
    public interface IPasswordManagementService
    {
        /// <summary>
        /// Validate password change request
        /// </summary>
        /// <param name="user">User entity</param>
        /// <param name="changePasswordDto">Password change request</param>
        /// <returns>Validation result</returns>
        Task<ServiceResult> ValidatePasswordChangeRequestAsync(TaiKhoan user, ChangePasswordDTO changePasswordDto);

        /// <summary>
        /// Validate new password strength
        /// </summary>
        /// <param name="user">User entity</param>
        /// <param name="newPassword">New password</param>
        /// <returns>Validation result</returns>
        Task<ServiceResult> ValidatePasswordStrengthAsync(TaiKhoan user, string newPassword);

        /// <summary>
        /// Update user password
        /// </summary>
        /// <param name="user">User entity</param>
        /// <param name="newPassword">New password</param>
        /// <returns>Update result</returns>
        Task<ServiceResult> UpdatePasswordAsync(TaiKhoan user, string newPassword);

        /// <summary>
        /// Initiate password change with OTP
        /// </summary>
        /// <param name="user">User entity</param>
        /// <returns>OTP send result</returns>
        Task<ServiceResult> InitiatePasswordChangeAsync(TaiKhoan user);
    }
}