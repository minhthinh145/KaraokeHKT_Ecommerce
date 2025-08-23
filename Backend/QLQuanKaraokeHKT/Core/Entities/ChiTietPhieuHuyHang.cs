using System;
using System.Collections.Generic;

namespace QLQuanKaraokeHKT.Core.Entities;

public partial class ChiTietPhieuHuyHang
{
    public int MaChiTietPhieu { get; set; }

    public int MaPhieuHuyHang { get; set; }

    public int SoLuongVatLieuHuy { get; set; }

    public int MaVatLieu { get; set; }

    public virtual PhieuHuyHang MaPhieuHuyHangNavigation { get; set; } = null!;

    public virtual VatLieu MaVatLieuNavigation { get; set; } = null!;
}
