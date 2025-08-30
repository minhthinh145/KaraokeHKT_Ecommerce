using AutoMapper;
using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs;
using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Auth;
using QLQuanKaraokeHKT.Core.Interfaces.Services.External;

namespace QLQuanKaraokeHKT.Application.Services.External
{
    public class MaOtpService : IMaOtpService
    {
        private readonly ISendEmailService _sendEmailService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMaOtpRepository _maOtpRepository; // ✅ Will be assigned via UnitOfWork
        private readonly IMapper _mapper;
        private readonly int _otpExpirationTime = 15;

        public MaOtpService(ISendEmailService sendEmailService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _sendEmailService = sendEmailService;
            _unitOfWork = unitOfWork;
            _maOtpRepository = unitOfWork.MaOtpRepository; // ✅ FIX: Assign from UnitOfWork
            _mapper = mapper;
        }

        public async Task<ServiceResult> GenerateAndSendOtpAsync(string email)
        {
            try
            {
                var user = await _unitOfWork.IdentityRepository.FindByEmailAsync(email);
                if (user == null)
                {
                    return ServiceResult.Failure("Không tìm thấy User với email này.");
                }
                if (string.IsNullOrEmpty(user.Email))
                {
                    return ServiceResult.Failure("Email không hợp lệ.");
                }
                var currentTime = DateTimeOffset.UtcNow.DateTime;
                var expirationTime = currentTime.AddMinutes(_otpExpirationTime);

                var otpCode = GenerateOtpCode();
                var otpDTO = new CreateUserOtpDTO
                {
                    MaTaiKhoan = user.Id,
                    maOTP = otpCode,
                    ExpirationTime = expirationTime
                };
                var userOtp = _mapper.Map<MaOtp>(otpDTO);
                
                // ✅ Use UnitOfWork pattern
                await _unitOfWork.MaOtpRepository.CreateAsync(userOtp);
                await _unitOfWork.SaveChangesAsync();

                await _sendEmailService.SendOtpEmailAsync(user.Email, otpCode);
                return ServiceResult.Success("OTP đã được gửi thành công");
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi khi tạo OTP: {ex.Message}");
            }
        }

        public async Task<ServiceResult> VerifyOtpAsync(string email, string otpCode)
        {
            try
            {
                var user = await _unitOfWork.IdentityRepository.FindByEmailAsync(email);
                if (user == null)
                    return ServiceResult.Failure("Không tìm thấy User với email này.");

                // ✅ Use UnitOfWork pattern
                var userOTP = await _unitOfWork.MaOtpRepository.GetOtpByCodeAsync(otpCode);
                if (userOTP == null)
                    return ServiceResult.Failure("OTP không hợp lệ hoặc đã hết hạn.");

                if (userOTP.MaTaiKhoan != user.Id)
                    return ServiceResult.Failure("OTP không đúng với tài khoản.");

                if (userOTP.DaSuDung)
                    return ServiceResult.Failure("OTP đã được sử dụng.");

                if (userOTP.NgayHetHan < DateTime.UtcNow)
                    return ServiceResult.Failure("OTP đã hết hạn.");

                return ServiceResult.Success("OTP hợp lệ.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi khi xác thực OTP: {ex.Message}");
            }
        }

        public async Task<ServiceResult> MarkOtpAsUsedAsync(string email, string otpCode)
        {
            try
            {
                var user = await _unitOfWork.IdentityRepository.FindByEmailAsync(email);
                if (user == null)
                    return ServiceResult.Failure("Không tìm thấy User với email này.");

                // ✅ Use UnitOfWork pattern
                var result = await _unitOfWork.MaOtpRepository.MarkOtpAsUsedAsync(user.Id, otpCode);
                if (result)
                {
                    await _unitOfWork.SaveChangesAsync();
                }
                
                return result
                    ? ServiceResult.Success("OTP đã được đánh dấu là đã sử dụng.")
                    : ServiceResult.Failure("Không thể đánh dấu OTP là đã sử dụng hoặc OTP không hợp lệ.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi khi đánh dấu OTP là đã sử dụng: {ex.Message}");
            }
        }

        private string GenerateOtpCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }
}