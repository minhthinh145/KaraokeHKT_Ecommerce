using Microsoft.EntityFrameworkCore;

namespace QLQuanKaraokeHKT.Domain.Entities.Views
{
    [Keyless]
    public class PhongHatForCustomerView
    {
        public int MaPhong { get; set; }
        public string TenPhong { get; set; } = null!;
        public string TenLoaiPhong { get; set; } = null!;
        public int SucChua { get; set; }
        public string? HinhAnhPhong { get; set; }
        public int MaSanPham { get; set; }

        public decimal? GiaThueCa1 { get; set; }
        public decimal? GiaThueCa2 { get; set; }
        public decimal? GiaThueCa3 { get; set; }
    }
}