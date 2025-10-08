using AutoMapper;
using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Core.Interfaces;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Auth;
using QLQuanKaraokeHKT.Core.Interfaces.Services.External;

namespace QLQuanKaraokeHKT.Application.Services.Auth
{
    public class UserRegistrationService : IUserRegistrationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMaOtpService _otpService;
        private readonly IMapper _mapper;
        private readonly ILogger<UserRegistrationService> _logger;

        public UserRegistrationService(
            IUnitOfWork unitOfWork,
            IMaOtpService otpService,
            IMapper mapper,
            ILogger<UserRegistrationService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _otpService = otpService ?? throw new ArgumentNullException(nameof(otpService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ServiceResult> ValidateRegistrationDataAsync(SignUpDTO signUpDto)
        {
            try
            {
                if (signUpDto == null)
                    return ServiceResult.Failure("Registration data is required.");

                if (string.IsNullOrWhiteSpace(signUpDto.Email))
                    return ServiceResult.Failure("Email is required.");

                if (string.IsNullOrWhiteSpace(signUpDto.Password))
                    return ServiceResult.Failure("Password is required.");

                // Check if email already exists - exactly like original
                var existingUser = await _unitOfWork.SignUpRepository.FindByEmailSignUpAsync(signUpDto.Email);
                if (existingUser != null)
                {
                    return ServiceResult.Failure("Email đã được sử dụng bởi tài khoản khác.");
                }

                return ServiceResult.Success("Registration data is valid.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating registration data for email: {Email}", signUpDto?.Email);
                return ServiceResult.Failure("System error during registration validation.");
            }
        }

        public async Task<(bool Success, TaiKhoan? User, string[] Errors)> CreateUserAccountAsync(SignUpDTO signUpDto)
        {
            try
            {
                if (signUpDto == null)
                    return (false, null, new[] { "Registration data is required." });

                var applicationUser = _mapper.Map<TaiKhoan>(signUpDto);
                var khachHang = _mapper.Map<KhachHang>(signUpDto);

                var (result, createdUser) = await _unitOfWork.SignUpRepository.CreateCustomerAccountForSignUpAsync(
                    applicationUser,
                    signUpDto.Password,
                    khachHang);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToArray();
                    _logger.LogWarning("Failed to create user account for email: {Email}. Errors: {Errors}", 
                        signUpDto.Email, string.Join(", ", errors));
                    return (false, null, errors);
                }

                return (true, createdUser, Array.Empty<string>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user account for email: {Email}", signUpDto?.Email);
                return (false, null, new[] { $"Lỗi hệ thống khi đăng ký: {ex.Message}" });
            }
        }

        public async Task<ServiceResult> SendAccountVerificationEmailAsync(TaiKhoan user)
        {
            try
            {
                if (user == null)
                    return ServiceResult.Failure("User is required.");

                if (string.IsNullOrWhiteSpace(user.Email))
                    return ServiceResult.Failure("User email is not available.");

                var otpResult = await _otpService.GenerateAndSendOtpAsync(user.Email);
                if (!otpResult.IsSuccess)
                {
                    _logger.LogWarning("Failed to send verification email for user: {UserId}. Error: {Error}", 
                        user.Id, otpResult.Message);
                    return ServiceResult.Failure($"Failed to send verification email: {otpResult.Message}");
                }

                return ServiceResult.Success("Verification email sent successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending verification email for user: {UserId}", user?.Id);
                return ServiceResult.Failure("System error while sending verification email.");
            }
        }
    }
}