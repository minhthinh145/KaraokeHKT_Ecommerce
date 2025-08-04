using System;
using System.Collections.Generic;

namespace QLQuanKaraokeHKT.Models;

public partial class GiaDichVu
{
    public int MaGiaDichVu { get; set; }

    public decimal DonGia { get; set; }

    public int MaSanPham { get; set; }

    public DateOnly NgayApDung { get; set; }

    public string TrangThai { get; set; } = null!;

    public virtual SanPhamDichVu MaSanPhamNavigation { get; set; } = null!;
}
