using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Domain.Entities;

namespace QLQuanKaraokeHKT.Core.Interfaces.Services.Auth
{
    public interface IUserAuthenticationService
    {
        Task<TaiKhoan?> ValidateUserCredentialsAsync(string email, string password);

        bool IsAccountActive(TaiKhoan user);

        Task<LoginResponseDTO> GenerateLoginResponseAsync(TaiKhoan user);

        Task<ServiceResult> IsEmailAvailableAsync(string email);

        Task<bool> CheckPasswordAsync(Guid userId, string password);

    }
}