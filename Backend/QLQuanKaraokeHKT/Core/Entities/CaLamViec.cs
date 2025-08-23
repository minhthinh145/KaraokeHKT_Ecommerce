namespace QLQuanKaraokeHKT.Core.Entities
{
    public partial class CaLamViec
    {
        public int MaCa { get; set; }
        public string TenCa { get; set; } = null!;
        public TimeOnly GioBatDauCa { get; set; }
        public TimeOnly GioKetThucCa { get; set; }

        public virtual ICollection<LuongCaLamViec> LuongCaLamViecs { get; set; } = new List<LuongCaLamViec>();

        public virtual ICollection<LichLamViec> LichLamViecs { get; set; } = new List<LichLamViec>();
        public virtual ICollection<GiaDichVu> GiaDichVus { get; set; } = new List<GiaDichVu>();
    }
}
