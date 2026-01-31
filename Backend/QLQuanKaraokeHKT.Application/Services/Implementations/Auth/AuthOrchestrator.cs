﻿using AutoMapper;
using QLQuanKaraokeHKT.Application.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Application.Services.Interfaces.Auth;
using QLQuanKaraokeHKT.Application.Services.Interfaces.External;

namespace QLQuanKaraokeHKT.Application.Services.Implementations.Auth
{
    public class AuthOrchestrator : IAuthOrchestrator
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserAuthenticationService _authenticationService;
        private readonly IUserRegistrationService _registrationService;
        private readonly IPasswordManagementService _passwordService;
        private readonly ITokenService _tokenService;
        private readonly IVerifyAuthService _verifyService;
        private readonly IMaOtpService _otpService;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthOrchestrator> _logger;

        public AuthOrchestrator(
            IUnitOfWork unitOfWork,
            IUserAuthenticationService authenticationService,
            IUserRegistrationService registrationService,
            IPasswordManagementService passwordService,
            IVerifyAuthService verifyService,
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
            _verifyService = verifyService ?? throw new ArgumentNullException(nameof(verifyService));
            _otpService = otpService ?? throw new ArgumentNullException(nameof(otpService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

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

        public async Task<ServiceResult> ExecuteSignUpWorkflowAsync(SignUpDTO signUpDto)
        {
            return await _unitOfWork.ExecuteTransactionAsync(async () =>
            {
                try
                {
                    _logger.LogInformation("🎼 Starting sign-up workflow for email: {Email}", signUpDto.Email);

                    var validationResult = await _registrationService.ValidateRegistrationDataAsync(signUpDto);
                    if (!validationResult.IsSuccess)
                    {
                        return validationResult;
                    }

                    var (success, user, errors) = await _registrationService.CreateUserAccountAsync(signUpDto);
                    if (!success || user == null)
                    {
                        _logger.LogWarning("❌ Failed to create account for email: {Email}. Errors: {Errors}", 
                            signUpDto.Email, string.Join(", ", errors));
                        return ServiceResult.Failure("Đăng ký thất bại.", errors);
                    }

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

        public async Task<ServiceResult> ExecuteAccountVerificationWorkflowAsync(VerifyAccountDTO verifyDto)
        {
            return await _unitOfWork.ExecuteTransactionAsync(async () =>
            {
                try
                {
                    _logger.LogInformation("🎼 Starting account verification workflow for email: {Email}", verifyDto?.Email);

                    var result = await _verifyService.VerifyAccountByEmail(verifyDto);

                    if (result.IsSuccess)
                    {
                        _logger.LogInformation("✅ Account verification successful for email: {Email}", verifyDto.Email);
                    }
                    else
                    {
                        _logger.LogWarning("❌ Account verification failed for email: {Email}. Reason: {Reason}",
                            verifyDto?.Email, result.Message);
                    }

                    return result;
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

        public async Task<ServiceResult> ExecuteSignOutWorkflowAsync(Guid userId, string refreshToken)
        {
            try
            {
                _logger.LogInformation("🎼 Starting sign-out workflow for user: {UserId}", userId);

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