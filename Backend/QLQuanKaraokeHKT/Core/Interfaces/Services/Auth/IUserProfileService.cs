using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.AuthDTOs;

namespace QLQuanKaraokeHKT.Core.Interfaces.Services.Auth
{
    /// <summary>
    /// User Profile Service Interface
    /// Handles user profile operations
    /// </summary>
    public interface IUserProfileService
    {
        /// <summary>
        /// Get user profile by user ID
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>User profile data</returns>
        Task<ServiceResult> GetUserProfileAsync(Guid userId);

        /// <summary>
        /// Update user profile
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="userProfileDto">Updated profile data</param>
        /// <returns>Update result with updated profile</returns>
        Task<ServiceResult> UpdateUserProfileAsync(Guid userId, UserProfileDTO userProfileDto);
    }
}