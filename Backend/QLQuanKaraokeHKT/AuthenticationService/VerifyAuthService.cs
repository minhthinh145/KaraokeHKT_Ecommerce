using QLQuanKaraokeHKT.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Helpers;
using QLQuanKaraokeHKT.Repositories.TaiKhoanRepo;
using QLQuanKaraokeHKT.Services.Interfaces;

namespace QLQuanKaraokeHKT.AuthenticationService
{
    public class VerifyAuthService : IVerifyAuthService
    {
        private readonly IMaOtpService _maOtpService;
        private readonly ITaiKhoanRepository _taiKhoanRepository;

        public VerifyAuthService(IMaOtpService maOtpService, ITaiKhoanRepository taiKhoanRepository)
        {
            _maOtpService = maOtpService ?? throw new ArgumentNullException(nameof(maOtpService));
            _taiKhoanRepository = taiKhoanRepository ?? throw new ArgumentNullException(nameof(taiKhoanRepository));
        }

        public async Task<ServiceResult> VerifyAccountByEmail(VerifyAccountDTO verifyAccountDto, Guid userID)
        {
            try
            {
                // Validate input
                if (verifyAccountDto == null)
                {
                    return ServiceResult.Failure("Dữ liệu xác thực không hợp lệ.");
                }

                if (string.IsNullOrWhiteSpace(verifyAccountDto.OtpCode))
                {
                    return ServiceResult.Failure("Mã OTP không được để trống.");
                }

                var otpVerificationResult = await _maOtpService.VerifyOtpAsync(userID, verifyAccountDto.OtpCode);

                if (!otpVerificationResult.IsSuccess)
                {
                    return ServiceResult.Failure(otpVerificationResult.Message);
                }

                var user = await _taiKhoanRepository.FindByUserIDAsync(userID.ToString());

                if (user == null)
                {
                    return ServiceResult.Failure("Không tìm thấy tài khoản người dùng.");
                }

                if (user.daKichHoat)
                {
                    return ServiceResult.Failure("Tài khoản đã được kích hoạt trước đó.");
                }

                user.daKichHoat = true;
                user.EmailConfirmed = true; 

                var updateResult = await _taiKhoanRepository.UpdateUserAsync(user);

                if (!updateResult.Succeeded)
                {
                    return ServiceResult.Failure("Không thể cập nhật trạng thái tài khoản.");
                }

                var markOtpResult = await _maOtpService.MarkOtpAsUsedAsync(userID, verifyAccountDto.OtpCode);

                if (!markOtpResult.IsSuccess)
                {
               
                }

                return ServiceResult.Success("Tài khoản đã được kích hoạt thành công.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi hệ thống khi xác thực tài khoản: {ex.Message}");
            }
        }
    }
}