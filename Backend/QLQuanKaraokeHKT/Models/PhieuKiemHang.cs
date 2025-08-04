using System;
using System.Collections.Generic;

namespace QLQuanKaraokeHKT.Models;

public partial class PhieuKiemHang
{
    public int MaPhieuKiemHang { get; set; }

    public string? MoTaPhieu { get; set; }

    public DateOnly NgayKiemKe { get; set; }

    public int MaNhanVien { get; set; }
    public virtual NhanVien NhanVien { get; set; } = null!;

    public virtual ICollection<ChiTietPhieuKiemHang> ChiTietPhieuKiemHangs { get; set; } = new List<ChiTietPhieuKiemHang>();
}
