using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLQuanKaraokeHKT.Application.Services.Auth;
using QLQuanKaraokeHKT.Core.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Auth;
using QLQuanKaraokeHKT.Core.Interfaces.Services.External;

namespace QLQuanKaraokeHKT.Presentation.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerifyAuthController : ControllerBase
    {
        private readonly IVerifyAuthService _verifyAuthService;
        private readonly IMaOtpService _otpService;
        private readonly IAuthOrchestrator _authOrchestrator;

        public VerifyAuthController(
            IVerifyAuthService verifyAuthService, 
            IMaOtpService otpService,
            IAuthOrchestrator authOrchestrator
            )
        {
            _verifyAuthService = verifyAuthService ?? throw new ArgumentNullException(nameof(verifyAuthService));
            _otpService = otpService ?? throw new ArgumentNullException(nameof(otpService));
            _authOrchestrator = authOrchestrator ?? throw new ArgumentNullException(nameof(authOrchestrator));
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
            var result = await _authOrchestrator.ExecuteAccountVerificationWorkflowAsync(verifyAccountDto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}