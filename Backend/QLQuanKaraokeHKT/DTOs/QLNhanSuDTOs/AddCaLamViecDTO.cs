namespace QLQuanKaraokeHKT.DTOs.QLNhanSuDTOs
{
    public class AddCaLamViecDTO
    {
        public string TenCa { get; set; } = null!;
        public TimeOnly GioBatDauCa { get; set; }
        public TimeOnly GioKetThucCa { get; set; }

    }
}
