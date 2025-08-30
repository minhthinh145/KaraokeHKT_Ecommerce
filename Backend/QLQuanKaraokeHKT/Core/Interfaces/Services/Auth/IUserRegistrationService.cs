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

        Task<ServiceResult> ValidateRegistrationDataAsync(SignUpDTO signUpDto);

        Task<(bool Success, TaiKhoan? User, string[] Errors)> CreateUserAccountAsync(SignUpDTO signUpDto);

        Task<ServiceResult> SendAccountVerificationEmailAsync(TaiKhoan user);
    }
}