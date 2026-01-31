using System.ComponentModel.DataAnnotations;

namespace QLQuanKaraokeHKT.Application.DTOs.QLPhongDTOs
{
    public class AddPhongHatDTO
    {
        [Required(ErrorMessage = "Loại phòng không được để trống")]
        public int MaLoaiPhong { get; set; }

        // Thông tin sản phẩm dịch vụ
        [Required(ErrorMessage = "Tên phòng không được để trống")]
        [MaxLength(200, ErrorMessage = "Tên phòng không quá 200 ký tự")]
        public string TenPhong { get; set; } = null!;
        public string? HinhAnhPhong { get; set; }

        // Cấu hình giá thuê - tương tự như MonAn
        [Required(ErrorMessage = "Cần chọn cách tính giá")]
        public bool DongGiaAllCa { get; set; } = true;

        // Giá thuê chung (khi DongGiaAllCa = true)
        public decimal? GiaThueChung { get; set; }

        // Giá thuê riêng từng ca (khi DongGiaAllCa = false)
        public decimal? GiaThueCa1 { get; set; }
        public decimal? GiaThueCa2 { get; set; }
        public decimal? GiaThueCa3 { get; set; }

        // Thông tin chung cho giá
        public DateOnly NgayApDungGia { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public string TrangThaiGia { get; set; } = "HieuLuc";
    }
}