namespace QLQuanKaraokeHKT.Application.DTOs.QLNhanSuDTOs
{
    public class LichLamViecDTO
    {
        public int MaLichLamViec { get; set; }
        public DateOnly NgayLamViec { get; set; }
        public Guid MaNhanVien { get; set; }
        public int MaCa { get; set; }

        // Thông tin từ bảng liên kết
        public string? TenNhanVien { get; set; }
        public string? LoaiNhanVien { get; set; }

    }
}
