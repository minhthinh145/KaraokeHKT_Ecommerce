using Microsoft.AspNetCore.Identity;
using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Auth;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Auth;
using QLQuanKaraokeHKT.Core.Interfaces.Services.External;

namespace QLQuanKaraokeHKT.Application.Services.Auth
{
    public class ChangePasswordService : IChangePasswordService
    {
        private readonly ITaiKhoanRepository _taiKhoanRepository;
        private readonly IMaOtpService _maOtpService;
        private readonly IPasswordValidator<TaiKhoan> _passwordValidator;
        private readonly UserManager<TaiKhoan> _userManager;

        public ChangePasswordService(ITaiKhoanRepository taiKhoanRepository, IMaOtpService maOtpService, IPasswordValidator<TaiKhoan> passwordValidator, UserManager<TaiKhoan> userManager)
        {
            _taiKhoanRepository = taiKhoanRepository ?? throw new ArgumentNullException(nameof(taiKhoanRepository));
            _maOtpService = maOtpService ?? throw new ArgumentNullException(nameof(maOtpService));
            _passwordValidator = passwordValidator ?? throw new ArgumentNullException(nameof(passwordValidator));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }
        public async Task<ServiceResult> ConfirmChangePasswordAsync(string userId, ConfirmChangePasswordDTO ConfirmChangePasswordDTO)
        {
            var user = await _taiKhoanRepository.FindByUserIDAsync(userId);
            if (user == null)
            {
                return ServiceResult.Failure("User not found.");
            }
            var validateResult = await _passwordValidator.ValidateAsync(_userManager, user, ConfirmChangePasswordDTO.NewPassword);
            if (!validateResult.Succeeded)
            {
                var errorsS = string.Join("; ", validateResult.Errors.Select(e => e.Description));
                return ServiceResult.Failure($"Mật khẩu mới không hợp lệ: {errorsS}");
            }
            var verifyOtpResult = await _maOtpService.VerifyOtpAsync(user.Email, ConfirmChangePasswordDTO.OtpCode);
            if (!verifyOtpResult.IsSuccess)
            {
                return ServiceResult.Failure(verifyOtpResult.Message);
            }
            var markOtpResult = await _maOtpService.MarkOtpAsUsedAsync(user.Email, ConfirmChangePasswordDTO.OtpCode);
            if (!markOtpResult.IsSuccess)
            {
                return ServiceResult.Failure(markOtpResult.Message);
            }
            var (success, errors) = await _taiKhoanRepository.UpdatePasswordAsync(user, ConfirmChangePasswordDTO.NewPassword);
            return success
                ? ServiceResult.Success("Mật khẩu đã được thay đổi thành công.")
                : ServiceResult.Failure($"Không thể thay đổi mật khẩu: {string.Join(", ", errors)}");
        }

        public async Task<ServiceResult> RequestChangePasswordAsync(ChangePasswordDTO changePasswordDTO, string userID)
        {
            var user = _taiKhoanRepository.FindByUserIDAsync(userID);
            if( user == null)
            {
                return ServiceResult.Failure("User not found.");
            }
            if (string.IsNullOrWhiteSpace(changePasswordDTO.NewPassword) || string.IsNullOrWhiteSpace(changePasswordDTO.ConfirmPassword))
            {
                return ServiceResult.Failure("New password and confirm password cannot be empty.");
            }
            if (changePasswordDTO.NewPassword != changePasswordDTO.ConfirmPassword)
            {
                return ServiceResult.Failure("New password and confirm password do not match.");
            }
            var checkOldPasswordResult = await _taiKhoanRepository.CheckPasswordAsync(user.Result, changePasswordDTO.OldPassword);
            if (!checkOldPasswordResult)
            {
                return ServiceResult.Failure("Mật khẩu cũ không đúng.");
            }
            await _maOtpService.GenerateAndSendOtpAsync(user.Result.Email);
            return ServiceResult.Success("Mã OTP đã được gửi");
        }
    }
}
