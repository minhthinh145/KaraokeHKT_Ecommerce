using AutoMapper;
using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Core.Interfaces;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Auth;
using QLQuanKaraokeHKT.Core.Interfaces.Services.External;

namespace QLQuanKaraokeHKT.Application.Services.Auth
{
    /// <summary>
    /// AUTH ORCHESTRATOR - Nhạc trưởng điều phối tất cả workflow phức tạp
    /// Tập hợp logic từ tất cả service cũ thành các method sạch cho controller
    /// </summary>
    public class AuthOrchestrator : IAuthOrchestrator
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserAuthenticationService _authenticationService;
        private readonly IUserRegistrationService _registrationService;
        private readonly IPasswordManagementService _passwordService;
        private readonly ITokenService _tokenService;
        private readonly IMaOtpService _otpService;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthOrchestrator> _logger;

        public AuthOrchestrator(
            IUnitOfWork unitOfWork,
            IUserAuthenticationService authenticationService,
            IUserRegistrationService registrationService,
            IPasswordManagementService passwordService,
            ITokenService tokenService,
            IMaOtpService otpService,
            IMapper mapper,
            ILogger<AuthOrchestrator> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
            _registrationService = registrationService ?? throw new ArgumentNullException(nameof(registrationService));
            _passwordService = passwordService ?? throw new ArgumentNullException(nameof(passwordService));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _otpService = otpService ?? throw new ArgumentNullException(nameof(otpService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // 🎼 ORCHESTRATE: Complete SignIn workflow from TaiKhoanService.SignInAsync()
        public async Task<ServiceResult> ExecuteSignInWorkflowAsync(SignInDTO signInDto)
        {
            try
            {
                _logger.LogInformation("🎼 Starting sign-in workflow for email: {Email}", signInDto.Email);

                // Step 1: Validate credentials (from TaiKhoanService)
                var user = await _authenticationService.ValidateUserCredentialsAsync(signInDto.Email, signInDto.Password);
                if (user == null)
                {
                    _logger.LogWarning("❌ Invalid credentials for email: {Email}", signInDto.Email);
                    return ServiceResult.Failure("Invalid email or password.");
                }

                // Step 2: Check account status (from TaiKhoanService.CheckAccountIsActive)
                if (!_authenticationService.IsAccountActive(user))
                {
                    _logger.LogWarning("❌ Inactive account attempted sign-in: {UserId}", user.Id);
                    return ServiceResult.Failure("Account is not active. Please activate your account first.", data: false);
                }

                // Step 3: Generate login response with tokens (from TaiKhoanService)
                var loginResponse = await _authenticationService.GenerateLoginResponseAsync(user);

                _logger.LogInformation("✅ Sign-in successful for user: {UserId}", user.Id);
                return ServiceResult.Success("Login successful.", loginResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 Error during sign-in workflow for email: {Email}", signInDto.Email);
                return ServiceResult.Failure("An error occurred during sign-in. Please try again.");
            }
        }

        // 🎼 ORCHESTRATE: Complete SignUp workflow from TaiKhoanService.SignUpAsync()
        public async Task<ServiceResult> ExecuteSignUpWorkflowAsync(SignUpDTO signUpDto)
        {
            return await _unitOfWork.ExecuteTransactionAsync(async () =>
            {
                try
                {
                    _logger.LogInformation("🎼 Starting sign-up workflow for email: {Email}", signUpDto.Email);

                    // Step 1: Validate registration data (extracted from TaiKhoanService)
                    var validationResult = await _registrationService.ValidateRegistrationDataAsync(signUpDto);
                    if (!validationResult.IsSuccess)
                    {
                        return validationResult;
                    }

                    // Step 2: Create user account (from TaiKhoanService.SignUpAsync)
                    var (success, user, errors) = await _registrationService.CreateUserAccountAsync(signUpDto);
                    if (!success || user == null)
                    {
                        _logger.LogWarning("❌ Failed to create account for email: {Email}. Errors: {Errors}", 
                            signUpDto.Email, string.Join(", ", errors));
                        return ServiceResult.Failure("Đăng ký thất bại.", errors);
                    }

                    // Step 3: Send verification email (enhanced feature)
                    var emailResult = await _registrationService.SendAccountVerificationEmailAsync(user);
                    if (!emailResult.IsSuccess)
                    {
                        _logger.LogWarning("⚠️ Failed to send verification email for user: {UserId}", user.Id);
                    }

                    var userProfile = _mapper.Map<UserProfileDTO>(user);
                    _logger.LogInformation("Sign-up successful for user: {UserId}", user.Id);
                    return ServiceResult.Success("Đăng ký thành công. Vui lòng kiểm tra email để kích hoạt tài khoản.", userProfile);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "💥 Error during sign-up workflow for email: {Email}", signUpDto.Email);
                    return ServiceResult.Failure($"Lỗi hệ thống khi đăng ký: {ex.Message}");
                }
            });
        }

        // 🎼 ORCHESTRATE: Account verification workflow from VerifyAuthService
        public async Task<ServiceResult> ExecuteAccountVerificationWorkflowAsync(VerifyAccountDTO verifyDto)
        {
            return await _unitOfWork.ExecuteTransactionAsync(async () =>
            {
                try
                {
                    _logger.LogInformation("🎼 Starting account verification workflow for email: {Email}", verifyDto.Email);

                    // Input validation
                    if (verifyDto == null)
                        return ServiceResult.Failure("Dữ liệu xác thực không hợp lệ.");

                    if (string.IsNullOrWhiteSpace(verifyDto.Email) || string.IsNullOrWhiteSpace(verifyDto.OtpCode))
                        return ServiceResult.Failure("Email và mã OTP không được để trống.");

                    // Step 1: Verify OTP (from VerifyAuthService)
                    var otpResult = await _otpService.VerifyOtpAsync(verifyDto.Email, verifyDto.OtpCode);
                    if (!otpResult.IsSuccess)
                        return ServiceResult.Failure(otpResult.Message);

                    // Step 2: Find and validate user (from VerifyAuthService)
                    var user = await _unitOfWork.IdentityRepository.FindByEmailAsync(verifyDto.Email);
                    if (user == null)
                        return ServiceResult.Failure("Không tìm thấy tài khoản người dùng.");

                    if (user.daKichHoat)
                        return ServiceResult.Failure("Tài khoản đã được kích hoạt trước đó.");

                    // Step 3: Activate account (from VerifyAuthService)
                    user.daKichHoat = true;
                    user.EmailConfirmed = true;

                    var updateResult = await _unitOfWork.IdentityRepository.UpdateUserAsync(user);
                    if (!updateResult.Succeeded)
                        return ServiceResult.Failure("Không thể cập nhật trạng thái tài khoản.");

                    // Step 4: Mark OTP as used (from VerifyAuthService)
                    await _otpService.MarkOtpAsUsedAsync(verifyDto.Email, verifyDto.OtpCode);

                    _logger.LogInformation("✅ Account verification successful for user: {UserId}", user.Id);
                    return ServiceResult.Success("Tài khoản đã được kích hoạt thành công.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "💥 Error during account verification workflow for email: {Email}", verifyDto?.Email);
                    return ServiceResult.Failure($"Lỗi hệ thống khi xác thực tài khoản: {ex.Message}");
                }
            });
        }

        public async Task<ServiceResult> ExecutePasswordChangeWorkflowAsync(Guid userId, ChangePasswordDTO changePasswordDto)
        {
            try
            {
                _logger.LogInformation("🎼 Starting password change workflow for user: {UserId}", userId);

                // Step 1: Find user
                var user = await _unitOfWork.IdentityRepository.FindByUserIDAsync(userId.ToString());
                if (user == null)
                {
                    return ServiceResult.Failure("User not found.");
                }

                // Step 2: Validate password change request (from ChangePasswordService)
                var validationResult = await _passwordService.ValidatePasswordChangeRequestAsync(user, changePasswordDto);
                if (!validationResult.IsSuccess)
                {
                    return validationResult;
                }

                // Step 3: Initiate password change with OTP (from ChangePasswordService)
                var otpResult = await _passwordService.InitiatePasswordChangeAsync(user);
                if (!otpResult.IsSuccess)
                {
                    return otpResult;
                }

                _logger.LogInformation("✅ Password change OTP sent for user: {UserId}", userId);
                return ServiceResult.Success("Mã OTP đã được gửi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 Error during password change workflow for user: {UserId}", userId);
                return ServiceResult.Failure("System error during password change request.");
            }
        }

        // 🎼 ORCHESTRATE: Password change confirmation from ChangePasswordService.ConfirmChangePasswordAsync()
        public async Task<ServiceResult> ExecutePasswordChangeConfirmationWorkflowAsync(Guid userId, ConfirmChangePasswordDTO confirmDto)
        {
            return await _unitOfWork.ExecuteTransactionAsync(async () =>
            {
                try
                {
                    _logger.LogInformation("🎼 Starting password change confirmation workflow for user: {UserId}", userId);

                    // Step 1: Find user
                    var user = await _unitOfWork.IdentityRepository.FindByUserIDAsync(userId.ToString());
                    if (user == null)
                    {
                        return ServiceResult.Failure("User not found.");
                    }

                    // Step 2: Validate new password strength (from ChangePasswordService)
                    var strengthResult = await _passwordService.ValidatePasswordStrengthAsync(user, confirmDto.NewPassword);
                    if (!strengthResult.IsSuccess)
                    {
                        return strengthResult;
                    }

                    // Step 3: Verify OTP (from ChangePasswordService)
                    var otpResult = await _otpService.VerifyOtpAsync(user.Email, confirmDto.OtpCode);
                    if (!otpResult.IsSuccess)
                    {
                        return ServiceResult.Failure(otpResult.Message);
                    }

                    // Step 4: Update password (from ChangePasswordService)
                    var updateResult = await _passwordService.UpdatePasswordAsync(user, confirmDto.NewPassword);
                    if (!updateResult.IsSuccess)
                    {
                        return updateResult;
                    }

                    // Step 5: Mark OTP as used (from ChangePasswordService)
                    await _otpService.MarkOtpAsUsedAsync(user.Email, confirmDto.OtpCode);

                    // Step 6: Revoke all refresh tokens for security (enhanced feature)
                    await _tokenService.RevokeAllUserRefreshTokensAsync(userId);

                    _logger.LogInformation("✅ Password change confirmed successfully for user: {UserId}", userId);
                    return ServiceResult.Success("Mật khẩu đã được thay đổi thành công.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "💥 Error during password change confirmation workflow for user: {UserId}", userId);
                    return ServiceResult.Failure("System error during password change confirmation.");
                }
            });
        }

        // 🎼 ORCHESTRATE: Sign out workflow
        public async Task<ServiceResult> ExecuteSignOutWorkflowAsync(Guid userId, string refreshToken)
        {
            try
            {
                _logger.LogInformation("🎼 Starting sign-out workflow for user: {UserId}", userId);

                // Step 1: Revoke refresh token if provided
                if (!string.IsNullOrEmpty(refreshToken))
                {
                    await _tokenService.RevokeRefreshTokenAsync(refreshToken);
                }

                 await _tokenService.RevokeAllUserRefreshTokensAsync(userId);

                _logger.LogInformation("✅ Sign-out successful for user: {UserId}", userId);
                return ServiceResult.Success("Đăng xuất thành công.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 Error during sign-out workflow for user: {UserId}", userId);

                return ServiceResult.Success("Đăng xuất thành công.");
            }
        }
    }
}