namespace QLQuanKaraokeHKT.Core.DTOs
{
    public class NhanVienDTO
    {
        public Guid MaNv { get; set; }

        public string HoTen { get; set; } = null!;

        public string Email { get; set; } = null!;

        public DateOnly? NgaySinh { get; set; }

        public string? SoDienThoai { get; set; }

        public string? LoaiNhanVien { get; set; }

        public bool DaNghiViec { get; set; }

    }
}
