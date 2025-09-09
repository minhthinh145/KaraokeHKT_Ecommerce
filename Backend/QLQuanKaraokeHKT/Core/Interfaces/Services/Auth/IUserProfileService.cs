using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.AuthDTOs;

namespace QLQuanKaraokeHKT.Core.Interfaces.Services.Auth
{
    public interface IUserProfileService
    {
        Task<ServiceResult> GetUserProfileAsync(Guid userId);

        Task<ServiceResult> UpdateUserProfileAsync(Guid userId, UserProfileDTO userProfileDto);
    }
}