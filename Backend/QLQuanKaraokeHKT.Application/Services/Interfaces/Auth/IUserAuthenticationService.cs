using QLQuanKaraokeHKT.Application.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Domain.Entities;

namespace QLQuanKaraokeHKT.Application.Services.Interfaces.Auth
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