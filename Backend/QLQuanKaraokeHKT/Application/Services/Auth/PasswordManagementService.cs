using Microsoft.AspNetCore.Identity;
using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Auth;
using QLQuanKaraokeHKT.Core.Interfaces.Services.External;

namespace QLQuanKaraokeHKT.Application.Services.Auth
{
    /// <summary>
    /// Password Management Service - Migrated from ChangePasswordService
    /// Handles ONLY password-related operations
    /// </summary>
    public class PasswordManagementService : IPasswordManagementService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMaOtpService _otpService;
        private readonly IPasswordValidator<TaiKhoan> _passwordValidator;
        private readonly UserManager<TaiKhoan> _userManager;
        private readonly ILogger<PasswordManagementService> _logger;

        public PasswordManagementService(
            IUnitOfWork unitOfWork,
            IMaOtpService otpService,
            IPasswordValidator<TaiKhoan> passwordValidator,
            UserManager<TaiKhoan> userManager,
            ILogger<PasswordManagementService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _otpService = otpService ?? throw new ArgumentNullException(nameof(otpService));
            _passwordValidator = passwordValidator ?? throw new ArgumentNullException(nameof(passwordValidator));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // 🔄 MIGRATED FROM: ChangePasswordService.RequestChangePasswordAsync() - validation part
        public async Task<ServiceResult> ValidatePasswordChangeRequestAsync(TaiKhoan user, ChangePasswordDTO changePasswordDto)
        {
            try
            {
                if (user == null)
                    return ServiceResult.Failure("User not found.");

                // Validate input data - exactly like original
                if (string.IsNullOrWhiteSpace(changePasswordDto.NewPassword) || 
                    string.IsNullOrWhiteSpace(changePasswordDto.ConfirmPassword))
                {
                    return ServiceResult.Failure("New password and confirm password cannot be empty.");
                }

                if (changePasswordDto.NewPassword != changePasswordDto.ConfirmPassword)
                {
                    return ServiceResult.Failure("New password and confirm password do not match.");
                }

                // Validate old password - exactly like original
                var checkOldPasswordResult = await _unitOfWork.IdentityRepository.CheckPasswordAsync(user, changePasswordDto.OldPassword);
                if (!checkOldPasswordResult)
                {
                    return ServiceResult.Failure("Mật khẩu cũ không đúng.");
                }

                return ServiceResult.Success("Password change request is valid.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating password change request for user {UserId}", user?.Id);
                return ServiceResult.Failure("System error during password validation.");
            }
        }

        // 🔄 MIGRATED FROM: ChangePasswordService.ConfirmChangePasswordAsync() - validation part
        public async Task<ServiceResult> ValidatePasswordStrengthAsync(TaiKhoan user, string newPassword)
        {
            try
            {
                if (user == null)
                    return ServiceResult.Failure("User not found.");

                if (string.IsNullOrWhiteSpace(newPassword))
                    return ServiceResult.Failure("New password cannot be empty.");

                // Use Identity password validator - exactly like original
                var validateResult = await _passwordValidator.ValidateAsync(_userManager, user, newPassword);
                if (!validateResult.Succeeded)
                {
                    var errors = string.Join("; ", validateResult.Errors.Select(e => e.Description));
                    return ServiceResult.Failure($"Mật khẩu mới không hợp lệ: {errors}");
                }

                return ServiceResult.Success("Password strength is valid.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating password strength for user {UserId}", user?.Id);
                return ServiceResult.Failure("System error during password strength validation.");
            }
        }

        // 🔄 MIGRATED FROM: ChangePasswordService.ConfirmChangePasswordAsync() - update part
        public async Task<ServiceResult> UpdatePasswordAsync(TaiKhoan user, string newPassword)
        {
            try
            {
                if (user == null)
                    return ServiceResult.Failure("User not found.");

                if (string.IsNullOrWhiteSpace(newPassword))
                    return ServiceResult.Failure("New password cannot be empty.");

                // Update password using Identity repository - exactly like original
                var (success, errors) = await _unitOfWork.IdentityRepository.UpdatePasswordAsync(user, newPassword);
                
                if (success)
                {
                    await _unitOfWork.SaveChangesAsync();
                    return ServiceResult.Success("Mật khẩu đã được thay đổi thành công.");
                }
                else
                {
                    return ServiceResult.Failure($"Không thể thay đổi mật khẩu: {string.Join(", ", errors)}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating password for user {UserId}", user?.Id);
                return ServiceResult.Failure("System error during password update.");
            }
        }

        // 🔄 MIGRATED FROM: ChangePasswordService.RequestChangePasswordAsync() - OTP part
        public async Task<ServiceResult> InitiatePasswordChangeAsync(TaiKhoan user)
        {
            try
            {
                if (user == null)
                    return ServiceResult.Failure("User not found.");

                if (string.IsNullOrWhiteSpace(user.Email))
                    return ServiceResult.Failure("User email is not available.");

                // Generate and send OTP - exactly like original
                var otpResult = await _otpService.GenerateAndSendOtpAsync(user.Email);
                if (!otpResult.IsSuccess)
                {
                    return ServiceResult.Failure(otpResult.Message);
                }

                return ServiceResult.Success("Mã OTP đã được gửi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initiating password change for user {UserId}", user?.Id);
                return ServiceResult.Failure("System error during password change initiation.");
            }
        }
    }
}