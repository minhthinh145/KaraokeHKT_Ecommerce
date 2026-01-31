﻿using QLQuanKaraokeHKT.Application.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Application.Services.Interfaces.Auth;
using QLQuanKaraokeHKT.Application.Services.Interfaces.External;

namespace QLQuanKaraokeHKT.Application.Services.Implementations.Auth
{
    public class VerifyAuthService : IVerifyAuthService
    {
        private readonly IMaOtpService _maOtpService;
        private readonly IUnitOfWork _unitOfWork;

        public VerifyAuthService(IMaOtpService maOtpService, IUnitOfWork unitOfWork )
        {
            _maOtpService = maOtpService ?? throw new ArgumentNullException(nameof(maOtpService));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<ServiceResult> VerifyAccountByEmail(VerifyAccountDTO verifyAccountDto)
        {
            try
            {
                if (verifyAccountDto == null)
                    return ServiceResult.Failure("Dữ liệu xác thực không hợp lệ.");

                if (string.IsNullOrWhiteSpace(verifyAccountDto.Email) || string.IsNullOrWhiteSpace(verifyAccountDto.OtpCode))
                    return ServiceResult.Failure("Email và mã OTP không được để trống.");

                var otpVerificationResult = await _maOtpService.VerifyOtpAsync(verifyAccountDto.Email, verifyAccountDto.OtpCode);
                if (!otpVerificationResult.IsSuccess)
                    return ServiceResult.Failure(otpVerificationResult.Message);

                var user = await _unitOfWork.IdentityRepository.FindByEmailAsync(verifyAccountDto.Email);
                if (user == null)
                    return ServiceResult.Failure("Không tìm thấy tài khoản người dùng.");

                if (user.daKichHoat)
                    return ServiceResult.Failure("Tài khoản đã được kích hoạt trước đó.");

                user.daKichHoat = true;
                user.EmailConfirmed = true;

                var updateResult = await _unitOfWork.IdentityRepository.UpdateUserAsync(user);
                if (!updateResult.Succeeded)
                    return ServiceResult.Failure("Không thể cập nhật trạng thái tài khoản.");

                await _maOtpService.MarkOtpAsUsedAsync(verifyAccountDto.Email, verifyAccountDto.OtpCode);

                return ServiceResult.Success("Tài khoản đã được kích hoạt thành công.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi hệ thống khi xác thực tài khoản: {ex.Message}");
            }
        }
    }
}