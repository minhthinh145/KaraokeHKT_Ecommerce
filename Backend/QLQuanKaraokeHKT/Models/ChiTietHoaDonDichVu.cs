using System;
using System.Collections.Generic;

namespace QLQuanKaraokeHKT.Models;

public partial class ChiTietHoaDonDichVu
{
    public int MaCthoaDon { get; set; }

    public Guid MaHoaDon { get; set; }

    public int MaSanPham { get; set; }

    public int SoLuong { get; set; }

    public virtual HoaDonDichVu MaHoaDonNavigation { get; set; } = null!;

    public virtual SanPhamDichVu MaSanPhamNavigation { get; set; } = null!;
}
