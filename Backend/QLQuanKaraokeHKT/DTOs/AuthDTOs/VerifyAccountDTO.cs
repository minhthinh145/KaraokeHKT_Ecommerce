using System.ComponentModel.DataAnnotations;

namespace QLQuanKaraokeHKT.DTOs.AuthDTOs
{
    public class VerifyAccountDTO
    {
        [Required(ErrorMessage = "Mã OTP là bắt buộc")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Mã OTP phải có đúng 6 ký tự")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Mã OTP phải là 6 chữ số")]
        public string OtpCode { get; set; } = string.Empty;
    }
}