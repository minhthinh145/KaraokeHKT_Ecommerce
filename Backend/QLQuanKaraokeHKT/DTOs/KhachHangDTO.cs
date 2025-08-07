namespace QLQuanKaraokeHKT.DTOs
{
    public class KhachHangDTO
    {
        public string TenKhachHang { get; set; } = null!;

        public string? Email { get; set; }

        public DateOnly? NgaySinh { get; set; }
    }
}
