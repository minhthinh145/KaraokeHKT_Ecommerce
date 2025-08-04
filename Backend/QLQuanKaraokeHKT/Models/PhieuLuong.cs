using System;
using System.Collections.Generic;

namespace QLQuanKaraokeHKT.Models;

public partial class PhieuLuong
{
    public int MaPhieuLuong { get; set; }

    public Guid MaNhanVien { get; set; }

    public DateOnly NgayIn { get; set; }

    public decimal HeSoLuong { get; set; }

    public decimal TongTienLuong { get; set; }

    public virtual NhanVien MaNhanVienNavigation { get; set; } = null!;
}
