using System.ComponentModel.DataAnnotations;

namespace QLQuanKaraokeHKT.DTOs.BookingDTOs
{
    public class DatPhongDTO
    {
        [Required(ErrorMessage = "Mã phòng không được để trống")]
        public int MaPhong { get; set; }

        public Guid MaKhachHang { get; set; } // Sẽ được gán tự động trong controller

        [Required(ErrorMessage = "Thời gian bắt đầu không được để trống")]
        public DateTime ThoiGianBatDau { get; set; }

        [Range(1, 24, ErrorMessage = "Số giờ sử dụng phải từ 1-24 giờ")]
        public int SoGioSuDung { get; set; } = 2; // Mặc định 2 giờ

        public string? GhiChu { get; set; }
    }
}