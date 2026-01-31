using QLQuanKaraokeHKT.Application.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Domain.Entities;

namespace QLQuanKaraokeHKT.Application.Services.Interfaces.Auth
{
    public interface IPasswordManagementService
    {
        Task<ServiceResult> ValidatePasswordChangeRequestAsync(TaiKhoan user, ChangePasswordDTO changePasswordDto);

        Task<ServiceResult> ValidatePasswordStrengthAsync(TaiKhoan user, string newPassword);

        Task<ServiceResult> UpdatePasswordAsync(TaiKhoan user, string newPassword);

        Task<ServiceResult> InitiatePasswordChangeAsync(TaiKhoan user);

    }
}