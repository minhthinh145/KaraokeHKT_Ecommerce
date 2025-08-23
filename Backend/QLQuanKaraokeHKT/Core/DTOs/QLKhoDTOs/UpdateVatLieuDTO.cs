using System.ComponentModel.DataAnnotations;

namespace QLQuanKaraokeHKT.Core.DTOs.QLKhoDTOs
{
    public class UpdateVatLieuDTO
    {
        [Required]
        public int MaVatLieu { get; set; }

        [Required(ErrorMessage = "Tên vật liệu không được để trống")]
        public string TenVatLieu { get; set; } = null!;

        [Required(ErrorMessage = "Đơn vị tính không được để trống")]
        public string DonViTinh { get; set; } = null!;

        // Thông tin sản phẩm dịch vụ
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        public string TenSanPham { get; set; } = null!;
        public string? HinhAnhSanPham { get; set; }

        // Thông tin giá nhập mới (nullable - chỉ update khi có)
        public decimal? GiaNhapMoi { get; set; }
        public DateOnly? NgayApDungGiaNhap { get; set; }
        public string? TrangThaiGiaNhap { get; set; }

        // Thông tin giá bán mới - THÊM OPTION CẬP NHẬT THEO CA
        public bool? CapNhatGiaBan { get; set; } // Có cập nhật giá bán không
        public bool? DongGiaAllCa { get; set; } // Đồng giá hay riêng biệt

        // Nếu đồng giá
        public decimal? GiaBanChung { get; set; }

        // Nếu riêng biệt từng ca (nullable - chỉ update ca nào có giá trị)
        public decimal? GiaBanCa1 { get; set; }
        public decimal? GiaBanCa2 { get; set; }
        public decimal? GiaBanCa3 { get; set; }

        public DateOnly? NgayApDungGiaBan { get; set; }
        public string? TrangThaiGiaBan { get; set; }
    }
}