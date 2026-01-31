namespace QLQuanKaraokeHKT.Application.DTOs.QLNhanSuDTOs
{
    public class CaLamViecDTO
    {
        public int MaCa { get; set; }
        public string TenCa { get; set; } = null!;
        public TimeOnly GioBatDauCa { get; set; }
        public TimeOnly GioKetThucCa { get; set; }

    }
}
