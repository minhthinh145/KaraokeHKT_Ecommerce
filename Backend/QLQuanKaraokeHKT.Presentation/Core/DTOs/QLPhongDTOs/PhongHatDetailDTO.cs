namespace QLQuanKaraokeHKT.Core.DTOs.QLPhongDTOs
{
    public class PhongHatDetailDTO
    {
        public int MaPhong { get; set; }
        public bool DangSuDung { get; set; }
        public bool NgungHoatDong { get; set; }

        public int MaLoaiPhong { get; set; }
        public string? TenLoaiPhong { get; set; }
        public int SucChua { get; set; }

        public int MaSanPham { get; set; }
        public string? TenSanPham { get; set; }
        public string? HinhAnhSanPham { get; set; }

        public bool? DongGiaAllCa { get; set; }

        public decimal? GiaThueChung { get; set; }

        public decimal? GiaThueCa1 { get; set; }
        public decimal? GiaThueCa2 { get; set; }
        public decimal? GiaThueCa3 { get; set; }

        public decimal? GiaThueHienTai { get; set; }
        public DateOnly? NgayApDungGia { get; set; }
        public string? TrangThaiGia { get; set; }
    }
}