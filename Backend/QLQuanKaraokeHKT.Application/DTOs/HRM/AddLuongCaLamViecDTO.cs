namespace QLQuanKaraokeHKT.Application.DTOs.QLNhanSuDTOs
{
    public class AddLuongCaLamViecDTO
    {
        public int MaCa { get; set; }
        public DateOnly? NgayApDung { get; set; }
        public DateOnly? NgayKetThuc { get; set; }
        public decimal GiaCa { get; set; }
        public bool IsDefault { get; set; } = false;
    }
}
