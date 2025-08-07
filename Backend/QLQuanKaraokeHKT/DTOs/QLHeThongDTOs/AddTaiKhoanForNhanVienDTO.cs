using System.ComponentModel.DataAnnotations;

namespace QLQuanKaraokeHKT.DTOs.QLHeThongDTOs
{
    public class AddTaiKhoanForNhanVienDTO
    {
        [Required]
        /// <summary>
        /// Mã nhân viên cần gán tài khoản
        /// </summary>
        public Guid MaNhanVien { get; set; }

        [Required]
        [EmailAddress]
        /// <summary>
        /// Email để tạo tài khoản mới
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Mật khẩu tự động = Email
        /// </summary>
        public string Password => Email;
    }
}