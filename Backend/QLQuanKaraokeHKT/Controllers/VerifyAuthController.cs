using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLQuanKaraokeHKT.AuthenticationService;
using QLQuanKaraokeHKT.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Services.Interfaces;

namespace QLQuanKaraokeHKT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerifyAuthController : ControllerBase
    {
        private readonly IVerifyAuthService _verifyAuthService;
        private readonly IMaOtpService _otpService;

        public VerifyAuthController(IVerifyAuthService verifyAuthService, IMaOtpService otpService)
        {
            _verifyAuthService = verifyAuthService ?? throw new ArgumentNullException(nameof(verifyAuthService));
            _otpService = otpService ?? throw new ArgumentNullException(nameof(otpService));
        }

        [HttpPost("sendOtp")]
        [AllowAnonymous]
        public async Task<IActionResult> SendOtp([FromBody] string email)
        {
            var result = await _otpService.GenerateAndSendOtpAsync(email);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("verify-account")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyAccountByEmail([FromBody] VerifyAccountDTO verifyAccountDto)
        {
            var result = await _verifyAuthService.VerifyAccountByEmail(verifyAccountDto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}