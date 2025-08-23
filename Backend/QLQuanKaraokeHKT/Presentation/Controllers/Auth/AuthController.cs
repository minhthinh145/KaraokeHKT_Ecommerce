using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QLQuanKaraokeHKT.Core.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Auth;
using QLQuanKaraokeHKT.Presentation.Extensions;

namespace QLQuanKaraokeHKT.Presentation.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITaiKhoanService _taiKhoanService;
        private readonly IAuthService _authService;

        public AuthController(ITaiKhoanService taiKhoanService, IAuthService authService)
        {
            _taiKhoanService = taiKhoanService;
            _authService = authService;
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInDTO signIn)
        {
            var modelValidation = this.ValidateModelState();
            if (modelValidation != null)
                return modelValidation;

            var result = await _taiKhoanService.SignInAsync(signIn);
            return result.IsSuccess ? Ok(result) : Unauthorized(result);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpDTO signUp)
        {
            var modelValidation = this.ValidateModelState();
            if (modelValidation != null)
                return modelValidation;

            var result = await _taiKhoanService.SignUpAsync(signUp);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO request)
        {
            var modelValidation = this.ValidateModelState();
            if (modelValidation != null)
                return modelValidation;

            if (string.IsNullOrEmpty(request.RefreshToken))
            {
                return BadRequest(new
                {
                    message = "Refresh token is required.",
                    success = false,
                    requireLogin = true
                });
            }

            try
            {
                var accessToken = await _authService.RefreshAccessTokenAsync(request.RefreshToken);
                return Ok(new
                {
                    message = "Token refreshed successfully.",
                    success = true,
                    data = new { AccessToken = accessToken }
                });
            }
            catch (SecurityTokenException ex)
            {
                return Unauthorized(new
                {
                    message = ex.Message,
                    success = false,
                    requireLogin = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi làm mới token.",
                    success = false
                });
            }
        }

        [HttpPost("signout")]
        public async Task<IActionResult> SignOut([FromBody] RefreshTokenRequestDTO request)
        {
            var modelValidation = this.ValidateModelState();
            if (modelValidation != null)
                return modelValidation;

            try
            {
                if (!string.IsNullOrEmpty(request.RefreshToken))
                {
                    await _authService.RevokeRefreshTokenAsync(request.RefreshToken);
                }

                return Ok(new { message = "Đăng xuất thành công.", success = true });
            }
            catch
            {
                // Ngay cả khi có lỗi revoke, vẫn trả về success vì client đã logout
                return Ok(new { message = "Đăng xuất thành công.", success = true });
            }
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var validationResult = this.ValidateUserAuthentication(out var userId);
                if (validationResult != null)
                    return validationResult;

                var result = await _taiKhoanService.GetProfileUserAsync(userId);
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while retrieving the profile.",
                    success = false
                });
            }
        }

        [HttpPatch("update")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UserProfileDTO user)
        {
            var validationResult = this.ValidateAuthenticationAndModel(out var userId);
            if (validationResult != null)
                return validationResult;

            var result = await _taiKhoanService.UpdateUserById(userId, user);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("checkpassword")]
        [Authorize]
        public async Task<IActionResult> CheckPasswordUser([FromBody] string password)
        {
            var validationResult = this.ValidateUserAuthentication(out var userId);
            if (validationResult != null)
                return validationResult;

            if (string.IsNullOrEmpty(password))
            {
                return BadRequest(new
                {
                    message = "Password is required.",
                    success = false
                });
            }

            var result = await _taiKhoanService.CheckPasswordAsync(userId, password);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}