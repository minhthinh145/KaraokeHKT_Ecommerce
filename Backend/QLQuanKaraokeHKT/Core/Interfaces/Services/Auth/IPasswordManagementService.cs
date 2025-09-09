using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Core.Entities;

namespace QLQuanKaraokeHKT.Core.Interfaces.Services.Auth
{
    public interface IPasswordManagementService
    {
        Task<ServiceResult> ValidatePasswordChangeRequestAsync(TaiKhoan user, ChangePasswordDTO changePasswordDto);

        Task<ServiceResult> ValidatePasswordStrengthAsync(TaiKhoan user, string newPassword);

        Task<ServiceResult> UpdatePasswordAsync(TaiKhoan user, string newPassword);

        Task<ServiceResult> InitiatePasswordChangeAsync(TaiKhoan user);

    }
}