namespace QLQuanKaraokeHKT.Application.DTOs.QLHeThongDTOs
{
    public class NhanVienTaiKhoanDTO
    {
        // Thông tin nhân viên
        public Guid MaNv { get; set; }
        public string HoTen { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateOnly? NgaySinh { get; set; }
        public string? SoDienThoai { get; set; }
        public string? LoaiNhanVien { get; set; }

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
