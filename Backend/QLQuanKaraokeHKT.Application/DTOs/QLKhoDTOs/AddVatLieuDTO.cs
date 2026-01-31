using System.ComponentModel.DataAnnotations;

namespace QLQuanKaraokeHKT.Application.DTOs.QLKhoDTOs
{
    public class AddVatLieuDTO
    {
        [Required(ErrorMessage = "Tên vật liệu không được để trống")]
        public string TenVatLieu { get; set; } = null!;

        [Required(ErrorMessage = "Đơn vị tính không được để trống")]
        public string DonViTinh { get; set; } = null!;

        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn kho phải >= 0")]
        public int SoLuongTonKho { get; set; } // set = 0, mặc định khi thêm mới

        // Thông tin sản phẩm dịch vụ
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        public string TenSanPham { get; set; } = null!;
        public string? HinhAnhSanPham { get; set; }

        // Thông tin giá nhập (GiaVatLieu)
        [Range(0, double.MaxValue, ErrorMessage = "Giá nhập phải >= 0")]
        public decimal GiaNhap { get; set; } // 
        public DateOnly NgayApDungGiaNhap { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public string TrangThaiGiaNhap { get; set; } = "HieuLuc"; // 

        // Thông tin giá bán - THÊM OPTION ĐỒng GIÁ HOẶC RIÊNG BIỆT
        public bool DongGiaAllCa { get; set; } = true; // 3 ca 

        // Nếu đồng giá
        [Range(0, double.MaxValue, ErrorMessage = "Giá bán phải >= 0")]
        public decimal? GiaBanChung { get; set; } // 30k cho 3 ca 
        // Nếu riêng biệt từng ca
        public decimal? GiaBanCa1 { get; set; } 
        public decimal? GiaBanCa2 { get; set; }
        public decimal? GiaBanCa3 { get; set; }

        public DateOnly NgayApDungGiaBan { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public string TrangThaiGiaBan { get; set; } = "HieuLuc";
    }
}