namespace QLQuanKaraokeHKT.Core.DTOs.QLPhongDTOs
{
    public class PhongHatDetailDTO
    {
        public int MaPhong { get; set; }
        public bool DangSuDung { get; set; }
        public bool NgungHoatDong { get; set; }

        // Thông tin loại phòng
        public int MaLoaiPhong { get; set; }
        public string? TenLoaiPhong { get; set; }
        public int SucChua { get; set; }

        // Thông tin sản phẩm dịch vụ
        public int MaSanPham { get; set; }
        public string? TenSanPham { get; set; }
        public string? HinhAnhSanPham { get; set; }

        // Thông tin giá thuê - tương tự như VatLieuDetailDTO
        public bool? DongGiaAllCa { get; set; }

        // Giá thuê chung
        public decimal? GiaThueChung { get; set; }

        // Giá thuê theo ca
        public decimal? GiaThueCa1 { get; set; }
        public decimal? GiaThueCa2 { get; set; }
        public decimal? GiaThueCa3 { get; set; }

        // Giá thuê hiện tại (ưu tiên theo ca, fallback đồng giá)
        public decimal? GiaThueHienTai { get; set; }
        public DateOnly? NgayApDungGia { get; set; }
        public string? TrangThaiGia { get; set; }
    }
}