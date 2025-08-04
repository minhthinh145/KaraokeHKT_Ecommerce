using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QLQuanKaraokeHKT.AuthenticationService;
using QLQuanKaraokeHKT.Controllers.Helper;
using QLQuanKaraokeHKT.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Services.TaiKhoanService;
using System.Security.Claims;

namespace QLQuanKaraokeHKT.Controllers
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
            if (result == null || !result.IsSuccess)
            {
                return Unauthorized(new { message = result?.Message ?? "Invalid credentials" });
            }

            return Ok(result);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpDTO signUp)
        {
            var modelValidation = this.ValidateModelState();
            if (modelValidation != null)
                return modelValidation;

            var result = await _taiKhoanService.SignUpAsync(signUp);
            if (!result.Succeeded)
            {
                return BadRequest(new { message = "Sign up failed.", errors = result.Errors });
            }

            return Ok(new { message = "Sign up successful." });
        }

        [HttpPost("signout")]
        public async Task<IActionResult> SignOut([FromBody] RefreshTokenRequestDTO request)
        {
            var modelValidation = this.ValidateModelState();
            if (modelValidation != null)
                return modelValidation;

            if (string.IsNullOrEmpty(request.RefreshToken))
            {
                return BadRequest(new { message = "Refresh token is required." });
            }

            await _authService.RevokeRefreshTokenAsync(request.RefreshToken);
            return Ok(new { message = "Signed out successfully." });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO request)
        {
            var modelValidation = this.ValidateModelState();
            if (modelValidation != null)
                return modelValidation;

            if (string.IsNullOrEmpty(request.RefreshToken))
            {
                return BadRequest(new { message = "Refresh token is required." });
            }

            try
            {
                var accessToken = await _authService.RefreshAccessTokenAsync(request.RefreshToken);
                return Ok(new { AccessToken = accessToken });
            }
            catch (SecurityTokenException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
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

                var userProfileResult = await _taiKhoanService.FindUserById(userId);
                if (userProfileResult == null || !userProfileResult.IsSuccess)
                {
                    return NotFound(new { message = userProfileResult?.Message ?? "User not found." });
                }

                return Ok(userProfileResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the profile.", error = ex.Message });
            }
        }

        [HttpPatch("update")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UserProfileDTO user)
        {
            var validationResult = this.ValidateAuthenticationAndModel(out var userId);
            if (validationResult != null)
                return validationResult;

            var userUpdate = await _taiKhoanService.UpdateUserById(userId, user);
            if (userUpdate == null || !userUpdate.IsSuccess)
            {
                return BadRequest(new { message = userUpdate?.Message ?? "Cannot update user" });
            }
            return Ok(new { message = "Cập nhật thông tin thành công" });
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
                return BadRequest(new { message = "Password is required." });
            }

            var result = await _taiKhoanService.CheckPasswordAsync(userId, password);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}