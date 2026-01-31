using System.ComponentModel.DataAnnotations;

namespace QLQuanKaraokeHKT.Application.DTOs.AuthDTOs
{
    public class VerifyAccountDTO
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "OTP phải có đúng 6 ký tự")]
        public string OtpCode { get; set; } = string.Empty;
    }
}