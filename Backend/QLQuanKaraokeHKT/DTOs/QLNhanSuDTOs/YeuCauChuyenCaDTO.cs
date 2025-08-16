namespace QLQuanKaraokeHKT.DTOs.QLNhanSuDTOs
{
    public class YeuCauChuyenCaDTO
    {
        public int MaYeuCau { get; set; }
        public int MaLichLamViecGoc { get; set; }
        public DateOnly NgayLamViecMoi { get; set; }
        public int MaCaMoi { get; set; }
        public string? LyDoChuyenCa { get; set; }
        public bool DaPheDuyet { get; set; }
        public bool? KetQuaPheDuyet { get; set; }
        public string? GhiChuPheDuyet { get; set; }
        public DateTime NgayTaoYeuCau { get; set; }
        public DateTime? NgayPheDuyet { get; set; }

        // Thông tin từ navigation properties
        public string? TenNhanVien { get; set; }
        public DateOnly NgayLamViecGoc { get; set; }
        public string? TenCaGoc { get; set; }
        public string? TenCaMoi { get; set; }
    }
}