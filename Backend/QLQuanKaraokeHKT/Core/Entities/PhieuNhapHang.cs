using System;
using System.Collections.Generic;

namespace QLQuanKaraokeHKT.Core.Entities;

public partial class PhieuNhapHang
{
    public int MaPhieuNhapHang { get; set; }

    public string TenPhieu { get; set; } = null!;

    public string? MoTaPhieu { get; set; }

    public DateOnly NgayTaoPhieu { get; set; }

    public decimal TongTienNhap { get; set; }

    public int MaNhaCungCap { get; set; }

    public Guid MaNhanVien { get; set; }
    public virtual NhanVien NhanVien { get; set; } = null!;
    public virtual ICollection<ChiTietPhieuNhapHang> ChiTietPhieuNhapHangs { get; set; } = new List<ChiTietPhieuNhapHang>();

    public virtual NhaCungCap MaNhaCungCapNavigation { get; set; } = null!;
}
