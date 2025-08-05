using System.ComponentModel.DataAnnotations;

namespace QLQuanKaraokeHKT.DTOs.AuthDTOs
{
    public class ChangePasswordDTO
    {
        [Required(ErrorMessage = "Vui lòng điền mật khẩu cũ")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Vui lòng điền mật khẩu mới")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải nằm trong khoảng 6 đến 100 từ")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Vui lòng điền xác nhận mật khẩu")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu không khớp")]
        public string ConfirmPassword { get; set; }
    }
}
