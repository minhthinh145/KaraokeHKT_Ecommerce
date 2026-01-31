using System;
using System.Collections.Generic;

namespace QLQuanKaraokeHKT.Domain.Entities;

public partial class NhanVien
{
    public Guid MaNv { get; set; }

    public string HoTen { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateOnly? NgaySinh { get; set; }

    public string? SoDienThoai { get; set; }

    public string? LoaiNhanVien { get; set; }

    public Guid MaTaiKhoan { get; set; }

    public bool DaNghiViec { get; set; } = false;

    public virtual TaiKhoan MaTaiKhoanNavigation { get; set; } = null!;
    public virtual ICollection<LichLamViec> LichLamViecs { get; set; } = new List<LichLamViec>();

    public virtual ICollection<PhieuLuong> PhieuLuongs { get; set; } = new List<PhieuLuong>();
    public virtual ICollection<PhieuHuyHang> PhieuHuyHangs { get; set; } = new List<PhieuHuyHang>();
    public virtual ICollection<PhieuNhapHang> PhieuNhapHangs { get; set; } = new List<PhieuNhapHang>();
    public virtual ICollection<PhieuKiemHang> PhieuKiemHangs { get; set; } = new List<PhieuKiemHang>();
}
