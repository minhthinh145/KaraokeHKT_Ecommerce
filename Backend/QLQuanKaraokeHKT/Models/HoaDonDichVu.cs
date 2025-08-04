using System;
using System.Collections.Generic;

namespace QLQuanKaraokeHKT.Models;

public partial class HoaDonDichVu
{
    public Guid MaHoaDon { get; set; }

    public string TenHoaDon { get; set; } = null!;

    public string? MoTaHoaDon { get; set; }

    public Guid MaKhachHang { get; set; }

    public decimal TongTienHoaDon { get; set; }

    public DateTime NgayTao { get; set; }

    public string TrangThai { get; set; } = null!;

    public virtual ICollection<ChiTietHoaDonDichVu> ChiTietHoaDonDichVus { get; set; } = new List<ChiTietHoaDonDichVu>();

    public virtual ICollection<LichSuSuDungPhong> LichSuSuDungPhongs { get; set; } = new List<LichSuSuDungPhong>();

    public virtual KhachHang MaKhachHangNavigation { get; set; } = null!;
}
