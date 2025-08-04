using System;
using System.Collections.Generic;

namespace QLQuanKaraokeHKT.Models;

public partial class GiaVatLieu
{
    public int MaGiaVatLieu { get; set; }

    public decimal DonGia { get; set; }

    public int MaVatLieu { get; set; }

    public DateOnly NgayApDung { get; set; }

    public string TrangThai { get; set; } = null!;

    public virtual VatLieu MaVatLieuNavigation { get; set; } = null!;
}
