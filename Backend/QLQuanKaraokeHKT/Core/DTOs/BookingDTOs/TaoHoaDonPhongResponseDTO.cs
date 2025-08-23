namespace QLQuanKaraokeHKT.Core.DTOs.BookingDTOs
{
    public class TaoHoaDonPhongResponseDTO
    {
        public Guid MaThuePhong { get; set; }
        public Guid MaHoaDon { get; set; }
        public string TenPhong { get; set; } = null!;
        public string TenKhachHang { get; set; } = null!;
        public DateTime ThoiGianBatDau { get; set; }
        public DateTime ThoiGianKetThucDuKien { get; set; }
        public int SoGioSuDung { get; set; }
        public decimal GiaPhong { get; set; }
        public decimal TongTien { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime HanThanhToan { get; set; } // 15 phút
        public string TrangThai { get; set; } = null!; // "Pending"
        public string? GhiChu { get; set; }

        // Chi tiết hóa đơn để frontend render
        public HoaDonDetailDTO HoaDonDetail { get; set; } = null!;
    }

    public class HoaDonDetailDTO
    {
        public Guid MaHoaDon { get; set; }
        public string TenHoaDon { get; set; } = null!;
        public string MoTaHoaDon { get; set; } = null!;
        public DateTime NgayTao { get; set; }
        public decimal TongTienHoaDon { get; set; }
        public List<ChiTietHoaDonDTO> ChiTietItems { get; set; } = new();
    }

    public class ChiTietHoaDonDTO
    {
        public string TenSanPham { get; set; } = null!;
        public int SoLuong { get; set; }
        public decimal GiaPhong { get; set; }
        public decimal ThanhTien => SoLuong * GiaPhong;
    }
}