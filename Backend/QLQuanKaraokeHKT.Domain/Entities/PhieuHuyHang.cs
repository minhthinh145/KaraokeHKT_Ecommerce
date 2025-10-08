using System;
using System.Collections.Generic;

namespace QLQuanKaraokeHKT.Domain.Entities;

public partial class PhieuHuyHang
{
    public int MaPhieuHuyHang { get; set; }

    public string TenPhieu { get; set; } = null!;

    public string? MoTaPhieu { get; set; }

    public DateOnly NgayTaoPhieu { get; set; }

    public decimal TongTienThatThoat { get; set; }

    public Guid MaNhanVien { get; set; }
    public virtual NhanVien NhanVien { get; set; } = null!;

    public virtual ICollection<ChiTietPhieuHuyHang> ChiTietPhieuHuyHangs { get; set; } = new List<ChiTietPhieuHuyHang>();
}
