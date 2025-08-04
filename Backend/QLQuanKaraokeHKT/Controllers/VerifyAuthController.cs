using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLQuanKaraokeHKT.AuthenticationService;
using QLQuanKaraokeHKT.Controllers.Helper;
using QLQuanKaraokeHKT.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Helpers;
using QLQuanKaraokeHKT.Services.Interfaces;

namespace QLQuanKaraokeHKT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Apply to entire controller since all actions require authentication
    public class VerifyAuthController : ControllerBase
    {
        private readonly IVerifyAuthService _verifyAuthService;
        private readonly IMaOtpService _otpService;

        public VerifyAuthController(IVerifyAuthService verifyAuthService, IMaOtpService otpService)
        {
            _verifyAuthService = verifyAuthService ?? throw new ArgumentNullException(nameof(verifyAuthService));
            _otpService = otpService ?? throw new ArgumentNullException(nameof(otpService));
        }

        /// <summary>
        /// Sends OTP to authenticated user's email
        /// </summary>
        /// <returns>Result of OTP sending process</returns>
        [HttpPost("sendOtp")]
        public async Task<IActionResult> SendOtp()
        {
            // Single responsibility: validate authentication
            var validationResult = this.ValidateUserAuthentication(out var userId);
            if (validationResult != null)
                return validationResult;

            var result = await _otpService.GenerateAndSendOtpAsync(userId);

            return result.IsSuccess
                ? Ok(new { message = result.Message, success = true })
                : BadRequest(new { message = result.Message, success = false });
        }

        /// <summary>
        /// Verifies account using OTP code
        /// </summary>
        /// <param name="verifyAccountDto">OTP verification data</param>
        /// <returns>Result of account verification</returns>
        [HttpPost("verify-account")]
        public async Task<IActionResult> VerifyAccountByEmail([FromBody] VerifyAccountDTO verifyAccountDto)
        {
            var validationResult = this.ValidateAuthenticationAndModel(out var userId);
            if (validationResult != null)
                return validationResult;

            try
            {
                var result = await _verifyAuthService.VerifyAccountByEmail(verifyAccountDto, userId);

                return result.IsSuccess
                    ? Ok(new { message = result.Message, success = true })
                    : BadRequest(new { message = result.Message, success = false });
            }
            catch (Exception ex)
            {
                // Log exception here if needed
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi xác thực tài khoản.",
                    success = false
                });
            }
        }
    }
}