namespace QLQuanKaraokeHKT.Application.DTOs.QLNhanSuDTOs
{
    public class LuongCaLamViecDTO
    {
        public int MaLuongCaLamViec { get; set; }
        public int MaCa { get; set; }
        public decimal GiaCa { get; set; }
        public bool IsDefault { get; set; }
        public DateOnly? NgayApDung { get; set; }
        public DateOnly? NgayKetThuc { get; set; }
        public string TenCaLamViec { get; set; } 
    }
}
