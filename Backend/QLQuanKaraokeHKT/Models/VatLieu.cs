using System;
using System.Collections.Generic;

namespace QLQuanKaraokeHKT.Models;

public partial class VatLieu
{
    public int MaVatLieu { get; set; }

    public string TenVatLieu { get; set; } = null!;

    public string DonViTinh { get; set; } = null!;

    public int SoLuongTonKho { get; set; }

    public virtual ICollection<ChiTietPhieuHuyHang> ChiTietPhieuHuyHangs { get; set; } = new List<ChiTietPhieuHuyHang>();

    public virtual ICollection<ChiTietPhieuKiemHang> ChiTietPhieuKiemHangs { get; set; } = new List<ChiTietPhieuKiemHang>();

    public virtual ICollection<ChiTietPhieuNhapHang> ChiTietPhieuNhapHangs { get; set; } = new List<ChiTietPhieuNhapHang>();

    public virtual ICollection<GiaVatLieu> GiaVatLieus { get; set; } = new List<GiaVatLieu>();
}
