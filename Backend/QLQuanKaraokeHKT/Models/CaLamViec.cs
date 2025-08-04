using System;
using System.Collections.Generic;

namespace QLQuanKaraokeHKT.Models;

public partial class CaLamViec
{
    public int MaCaLamViec { get; set; }

    public DateOnly NgayLamViec { get; set; }

    public TimeOnly GioBatDauCa { get; set; }

    public TimeOnly GioKetThucCa { get; set; }

    public Guid MaNhanVien { get; set; }

    public virtual NhanVien MaNhanVienNavigation { get; set; } = null!;
}
