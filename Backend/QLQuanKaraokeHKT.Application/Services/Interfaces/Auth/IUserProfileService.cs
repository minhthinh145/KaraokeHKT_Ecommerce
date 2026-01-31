using QLQuanKaraokeHKT.Application.DTOs.AuthDTOs;

namespace QLQuanKaraokeHKT.Application.Services.Interfaces.Auth
{
    public interface IUserProfileService
    {
        Task<ServiceResult> GetUserProfileAsync(Guid userId);

        Task<ServiceResult> UpdateUserProfileAsync(Guid userId, UserProfileDTO userProfileDto);
    }
}