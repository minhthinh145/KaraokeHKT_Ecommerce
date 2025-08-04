using System;
using System.Collections.Generic;

namespace QLQuanKaraokeHKT.Models;

public partial class PhongHatKaraoke
{
    public int MaPhong { get; set; }

    public int MaSanPham { get; set; }

    public int MaLoaiPhong { get; set; }

    public bool DangSuDung { get; set; }

    public virtual ICollection<LichSuSuDungPhong> LichSuSuDungPhongs { get; set; } = new List<LichSuSuDungPhong>();

    public virtual LoaiPhongHatKaraoke MaLoaiPhongNavigation { get; set; } = null!;

    public virtual SanPhamDichVu MaSanPhamNavigation { get; set; } = null!;

    public virtual ICollection<ThuePhong> ThuePhongs { get; set; } = new List<ThuePhong>();
}
