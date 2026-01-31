using System.ComponentModel.DataAnnotations;

namespace QLQuanKaraokeHKT.Application.DTOs.QLPhongDTOs
{
    public class UpdatePhongHatDTO
    {
        [Required]
        public int MaPhong { get; set; }

        [Required(ErrorMessage = "Tên phòng không được để trống")]
        [MaxLength(200, ErrorMessage = "Tên phòng không quá 200 ký tự")]
        public string TenPhong { get; set; } = null!;
        public string? HinhAnhPhong { get; set; }

        public int? MaLoaiPhong { get; set; }

        // Cấp nhật giá thuê - tương tự như UpdateVatLieuDTO
        public bool CapNhatGiaThue { get; set; } = false;

        // Cấu hình giá thuê mới
        public bool? DongGiaAllCa { get; set; }

        // Giá thuê chung (khi DongGiaAllCa = true)
        public decimal? GiaThueChung { get; set; }

        // Giá thuê riêng từng ca (khi DongGiaAllCa = false)
        public decimal? GiaThueCa1 { get; set; }
        public decimal? GiaThueCa2 { get; set; }
        public decimal? GiaThueCa3 { get; set; }

        // Thông tin chung cho giá mới
        public DateOnly? NgayApDungGia { get; set; }
        public string? TrangThaiGia { get; set; }
    }
}