namespace QLQuanKaraokeHKT.Core.DTOs.BookingDTOs
{
    public class PhongHatForCustomerDTO
    {
        public int MaPhong { get; set; }
        public string TenPhong { get; set; } = null!;
        public string TenLoaiPhong { get; set; } = null!;
        public int SucChua { get; set; }
        public string? HinhAnhPhong { get; set; }
        public decimal GiaThueHienTai { get; set; }
        public bool Available { get; set; }
        public string? MoTa { get; set; }
    }
}