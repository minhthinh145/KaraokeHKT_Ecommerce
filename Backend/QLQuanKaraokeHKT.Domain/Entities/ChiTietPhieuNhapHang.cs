using System;
using System.Collections.Generic;

namespace QLQuanKaraokeHKT.Domain.Entities;

public partial class ChiTietPhieuNhapHang
{
    public int MaChiTietPhieu { get; set; }

    public int MaPhieuNhapHang { get; set; }

    public int SoLuongVatLieuNhap { get; set; }

    public int MaVatLieu { get; set; }

    public virtual PhieuNhapHang MaPhieuNhapHangNavigation { get; set; } = null!;

    public virtual VatLieu MaVatLieuNavigation { get; set; } = null!;
}
