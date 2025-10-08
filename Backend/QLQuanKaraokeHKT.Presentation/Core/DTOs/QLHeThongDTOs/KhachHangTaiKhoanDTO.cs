namespace QLQuanKaraokeHKT.Core.DTOs.QLHeThongDTOs
{
    public class KhachHangTaiKhoanDTO
    {
        // Thông tin khách hàng
        public Guid MaKhachHang { get; set; }
        public string TenKhachHang { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateOnly? NgaySinh { get; set; }

        // Thông tin tài khoản
        public Guid MaTaiKhoan { get; set; }
        public string UserName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public bool DaKichHoat { get; set; }
        public bool DaBiKhoa { get; set; }
        public string LoaiTaiKhoan { get; set; } = null!;
        public bool EmailConfirmed { get; set; }
    }
}
