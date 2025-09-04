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
        Task<ServiceResult> GetUserProfileAsync(Guid userId);

        Task<ServiceResult> UpdateUserProfileAsync(Guid userId, UserProfileDTO userProfileDto);
    }
}