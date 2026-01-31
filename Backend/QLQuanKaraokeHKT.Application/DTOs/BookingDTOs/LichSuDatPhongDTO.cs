namespace QLQuanKaraokeHKT.Application.DTOs.BookingDTOs
{
    public class LichSuDatPhongDTO
    {
        public Guid MaThuePhong { get; set; }
        public int MaPhong { get; set; }
        public string TenPhong { get; set; } = null!;
        public string? HinhAnhPhong { get; set; }
        public string? TenLoaiPhong { get; set; }
        public DateTime ThoiGianBatDau { get; set; }
        public DateTime? ThoiGianKetThuc { get; set; }
        public int SoGioSuDung { get; set; }

        /// <summary>
        /// ChuaThanhToan, DaThanhToan, DangSuDung, DaHoanThanh, DaHuy, HetHanThanhToan
        /// </summary>
        public string TrangThai { get; set; } = null!;

        public decimal TongTien { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime? NgayThanhToan { get; set; }

        // ✅ THÔNG TIN HÓA ĐƠN
        public Guid? MaHoaDon { get; set; }
        public string? TenHoaDon { get; set; }
        public string? MoTaHoaDon { get; set; }

        // ✅ THÔNG TIN THANH TOÁN
        public DateTime? HanThanhToan { get; set; }
        public bool DaHetHanThanhToan { get; set; }

        // ✅ THÔNG TIN KHÁCH HÀNG (nếu cần)
        public string? TenKhachHang { get; set; }
        public string? EmailKhachHang { get; set; }

        /// <summary>
        /// Có thể thanh toán lại không (cho trường hợp HetHanThanhToan)
        /// </summary>
        public bool CoTheThanhToanLai => TrangThai == "HetHanThanhToan";

        /// <summary>
        /// Có thể hủy không (chỉ với trạng thái ChuaThanhToan)
        /// </summary>
        public bool CoTheHuy => TrangThai == "ChuaThanhToan";

        /// <summary>
        /// Có thể xác nhận thanh toán không (với trạng thái ChuaThanhToan và chưa hết hạn)
        /// </summary>
        public bool CoTheXacNhanThanhToan => TrangThai == "ChuaThanhToan" && !DaHetHanThanhToan;

        /// <summary>
        /// Thời gian còn lại để thanh toán (phút)
        /// </summary>
        public int? PhutConLaiDeThanhToan =>
            HanThanhToan.HasValue && !DaHetHanThanhToan ?
                Math.Max(0, (int)(HanThanhToan.Value - DateTime.Now).TotalMinutes) :
                null;
    }
}