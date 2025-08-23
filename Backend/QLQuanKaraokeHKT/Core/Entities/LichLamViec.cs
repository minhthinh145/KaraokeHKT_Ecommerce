namespace QLQuanKaraokeHKT.Core.Entities
{
    public partial class LichLamViec
    {
        public int MaLichLamViec { get; set; }
        public DateOnly NgayLamViec { get; set; }
        public Guid MaNhanVien { get; set; }
        public int MaCa { get; set; }

        public virtual NhanVien NhanVien { get; set; } = null!;
        public virtual CaLamViec CaLamViec { get; set; } = null!;
    }
}
