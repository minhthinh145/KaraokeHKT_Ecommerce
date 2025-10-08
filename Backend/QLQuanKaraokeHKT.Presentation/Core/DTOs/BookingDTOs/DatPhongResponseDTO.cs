namespace QLQuanKaraokeHKT.Core.DTOs.BookingDTOs
{
    public class DatPhongResponseDTO
    {
        public Guid MaThuePhong { get; set; }
        public Guid MaHoaDon { get; set; }
        public string TenPhong { get; set; } = null!;
        public DateTime ThoiGianBatDau { get; set; }
        public DateTime ThoiGianKetThucDuKien { get; set; }
        public decimal TongTien { get; set; }
        public string TrangThai { get; set; } = null!;
        public DateTime NgayTao { get; set; }
        public DateTime? HanThanhToan { get; set; } // 15 phút
        public string? UrlThanhToan { get; set; } // VNPay URL
    }
}