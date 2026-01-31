using QLQuanKaraokeHKT.Application.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Domain.Entities;

namespace QLQuanKaraokeHKT.Application.Services.Interfaces.Auth
{
    public interface IUserRegistrationService
    {

        Task<ServiceResult> ValidateRegistrationDataAsync(SignUpDTO signUpDto);

        Task<(bool Success, TaiKhoan? User, string[] Errors)> CreateUserAccountAsync(SignUpDTO signUpDto);

        Task<ServiceResult> SendAccountVerificationEmailAsync(TaiKhoan user);
    }
}