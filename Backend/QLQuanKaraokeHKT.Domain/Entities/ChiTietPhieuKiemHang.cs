using System;
using System.Collections.Generic;

namespace QLQuanKaraokeHKT.Domain.Entities;

public partial class ChiTietPhieuKiemHang
{
    public int MaChiTietPhieu { get; set; }

    public int MaPhieuKiemHang { get; set; }

    public int SoLuongTheoSo { get; set; }

    public int SoLuongThucTe { get; set; }

    public int? ChenhLech { get; set; }

    public string? GhiChu { get; set; }

    public int MaVatLieu { get; set; }

    public virtual PhieuKiemHang MaPhieuKiemHangNavigation { get; set; } = null!;

    public virtual VatLieu MaVatLieuNavigation { get; set; } = null!;
}
