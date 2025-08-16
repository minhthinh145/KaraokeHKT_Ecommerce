using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLQuanKaraokeHKT.Models
{
    public partial class LuongCaLamViec
    {
        [Key]
        public int MaLuongCaLamViec { get; set; }
        public int MaCa { get; set; }
        public DateOnly? NgayApDung { get; set; }
        public DateOnly? NgayKetThuc { get; set; }
        [Column(TypeName = "decimal(18,2)")]

        public decimal GiaCa { get; set; }
        public bool IsDefault { get; set; }
        [ForeignKey(nameof(MaCa))]
        public virtual CaLamViec CaLamViec { get; set; } = null!;
    }
}
