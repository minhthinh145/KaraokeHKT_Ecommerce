namespace QLQuanKaraokeHKT.Core.DTOs.QLKhoDTOs
{
    public class VatLieuDetailDTO
    {
        public int MaVatLieu { get; set; }
        public string TenVatLieu { get; set; } = null!;
        public string DonViTinh { get; set; } = null!;
        public int SoLuongTonKho { get; set; }
        public bool NgungCungCap { get; set; }

        // Thông tin sản phẩm dịch vụ
        public int? MaSanPham { get; set; }
        public string? TenSanPham { get; set; }
        public string? HinhAnhSanPham { get; set; }

        // Thông tin giá nhập (GiaVatLieu)
        public decimal? GiaNhapHienTai { get; set; }
        public DateOnly? NgayApDungGiaNhap { get; set; }
        public string? TrangThaiGiaNhap { get; set; }

        // Thông tin giá bán (GiaDichVu)
        public decimal? GiaBanHienTai { get; set; }
        public DateOnly? NgayApDungGiaBan { get; set; }
        public string? TrangThaiGiaBan { get; set; }

        // Thông tin món ăn (nếu có)
        public int? MaMonAn { get; set; }
        public int? SoLuongConLai { get; set; }

        // --- BỔ SUNG THÔNG TIN GIÁ BÁN THEO CA ---
        public bool? DongGiaAllCa { get; set; }
        public decimal? GiaBanChung { get; set; }
        public decimal? GiaBanCa1 { get; set; }
        public decimal? GiaBanCa2 { get; set; }
        public decimal? GiaBanCa3 { get; set; }
    }
}