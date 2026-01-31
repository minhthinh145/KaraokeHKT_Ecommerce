﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLQuanKaraokeHKT.Application.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Application.Services.Interfaces.Auth;
using QLQuanKaraokeHKT.Presentation.Extensions;

namespace QLQuanKaraokeHKT.Presentation.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Yêu cầu authentication cho tất cả endpoints
    public class ChangePasswordController : ControllerBase
    {
        private readonly IAuthOrchestrator _authOrchestrator ;

        public ChangePasswordController(IAuthOrchestrator authOrchestrator )
        {
            _authOrchestrator = authOrchestrator ?? throw new ArgumentNullException(nameof(authOrchestrator));
        }

        /// <summary>
        /// Request to change password - Gửi OTP để xác thực
        /// </summary>
        /// <param name="changePasswordDto">Thông tin mật khẩu mới</param>
        /// <returns>Kết quả yêu cầu đổi mật khẩu</returns>
        [HttpPost("request")]
        public async Task<IActionResult> RequestChangePassword([FromBody] ChangePasswordDTO changePasswordDto)
        {
            try
            {
                // Validate model state
                var modelValidation = this.ValidateModelState();
                if (modelValidation != null)
                    return modelValidation;

                // Validate user authentication
                var authValidation = this.ValidateUserAuthentication(out var userId);
                if (authValidation != null)
                    return authValidation;

                // Call service để request change password
                var result = await _authOrchestrator.ExecutePasswordChangeWorkflowAsync(userId, changePasswordDto);

                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi yêu cầu đổi mật khẩu.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Confirm change password with OTP - Xác nhận đổi mật khẩu bằng OTP
        /// </summary>
        /// <param name="confirmChangePasswordDto">Thông tin OTP và mật khẩu mới</param>
        /// <returns>Kết quả xác nhận đổi mật khẩu</returns>
        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmChangePassword([FromBody] ConfirmChangePasswordDTO confirmChangePasswordDto)
        {
            try
            {
                // Validate model state
                var modelValidation = this.ValidateModelState();
                if (modelValidation != null)
                    return modelValidation;

                // Validate user authentication
                var authValidation = this.ValidateUserAuthentication(out var userId);
                if (authValidation != null)
                    return authValidation;

                // Additional validation
                if (string.IsNullOrWhiteSpace(confirmChangePasswordDto.OtpCode))
                {
                    return BadRequest(new
                    {
                        message = "Mã OTP không được để trống.",
                        success = false
                    });
                }

                if (string.IsNullOrWhiteSpace(confirmChangePasswordDto.NewPassword))
                {
                    return BadRequest(new
                    {
                        message = "Mật khẩu mới không được để trống.",
                        success = false
                    });
                }

                // Call service để confirm change password
                var result = await _authOrchestrator.ExecutePasswordChangeConfirmationWorkflowAsync(userId, confirmChangePasswordDto);

                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi xác nhận đổi mật khẩu.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Cancel change password request - Hủy yêu cầu đổi mật khẩu
        /// </summary>
        /// <returns>Kết quả hủy yêu cầu</returns>
        [HttpPost("cancel")]
        public async Task<IActionResult> CancelChangePasswordRequest()
        {
            try
            {
                // Validate user authentication
                var authValidation = this.ValidateUserAuthentication(out var userId);
                if (authValidation != null)
                    return authValidation;

                // Logic để cancel request (nếu service có support)
                // Ví dụ: revoke OTP codes, cleanup temporary data, etc.

                return Ok(new
                {
                    message = "Yêu cầu đổi mật khẩu đã được hủy.",
                    success = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi hủy yêu cầu đổi mật khẩu.",
                    success = false,
                    error = ex.Message
                });
            }
        }
    }
}