using System.ComponentModel.DataAnnotations;

namespace QLQuanKaraokeHKT.Application.DTOs.QLNhanSuDTOs
{
    public class AddNhanVienDTO
    {
        [Required]
        public string HoTen { get; set; } = string.Empty;
        [Required]
        public string SoDienThoai { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;
       
        [Required]
        public DateTime NgaySinh { get; set; }

        [Required]
        public string LoaiTaiKhoan { get; set; } = string.Empty;

    }
}