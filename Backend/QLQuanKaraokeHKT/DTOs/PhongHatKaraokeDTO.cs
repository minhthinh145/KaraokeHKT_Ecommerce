using QLQuanKaraokeHKT.Models;

namespace QLQuanKaraokeHKT.DTOs
{
    public class PhongHatKaraokeDTO
    {
        public int MaSanPham { get; set; }

        public int MaLoaiPhong { get; set; }

        public bool DangSuDung { get; set; }
        public virtual LoaiPhongHatKaraoke MaLoaiPhongNavigation { get; set; } = null!;

        public virtual SanPhamDichVu MaSanPhamNavigation { get; set; } = null!;

    }
}
