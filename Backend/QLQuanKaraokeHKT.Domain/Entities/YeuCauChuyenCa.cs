namespace QLQuanKaraokeHKT.Domain.Entities
{
    public class YeuCauChuyenCa
    {
        public int MaYeuCau { get; set; }

        // Khóa ngoại đến LichLamViec gốc (lịch cần chuyển)
        public int MaLichLamViecGoc { get; set; }

        // Thông tin ca mới mà nhân viên muốn chuyển đến
        public DateOnly NgayLamViecMoi { get; set; }
        public int MaCaMoi { get; set; }

        // Lý do chuyển ca
        public string? LyDoChuyenCa { get; set; }

        // Thông tin phê duyệt
        public bool DaPheDuyet { get; set; } = false;
        public bool? KetQuaPheDuyet { get; set; } // true: chấp nhận, false: từ chối, null: chưa xử lý
        public string? GhiChuPheDuyet { get; set; }

        // Thời gian
        public DateTime NgayTaoYeuCau { get; set; } = DateTime.Now;
        public DateTime? NgayPheDuyet { get; set; }

        // Navigation properties
        public virtual LichLamViec LichLamViecGoc { get; set; } = null!;
        public virtual CaLamViec CaMoi { get; set; } = null!;
    }
}
