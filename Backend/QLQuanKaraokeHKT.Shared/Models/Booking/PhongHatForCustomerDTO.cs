
namespace QLQuanKaraokeHKT.Shared.Models.Booking
{
    public class PhongHatForCustomerDTO
    {
        public int MaPhong { get; set; }
        public string TenPhong { get; set; } = null!;
        public string TenLoaiPhong { get; set; } = null!;
        public int SucChua { get; set; }
        public string? HinhAnhPhong { get; set; }

        public decimal GiaThueHienTai { get; set; }
        public decimal? GiaThueCa1 { get; set; }
        public decimal? GiaThueCa2 { get; set; }
        public decimal? GiaThueCa3 { get; set; }

        public bool CoGiaTheoCa => GiaThueCa1.HasValue || GiaThueCa2.HasValue || GiaThueCa3.HasValue;
    }
}
