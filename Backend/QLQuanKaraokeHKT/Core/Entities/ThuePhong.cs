using System;
using System.Collections.Generic;

namespace QLQuanKaraokeHKT.Core.Entities;

public partial class ThuePhong
{
    public Guid MaThuePhong { get; set; }

    public int MaPhong { get; set; }

    public Guid MaKhachHang { get; set; }

    public DateTime ThoiGianBatDau { get; set; }

    public DateTime? ThoiGianKetThuc { get; set; }

    public string TrangThai { get; set; } = null!;

    public Guid? MaHoaDon { get; set; }

    public virtual KhachHang MaKhachHangNavigation { get; set; } = null!;

    public virtual PhongHatKaraoke MaPhongNavigation { get; set; } = null!;

    public virtual HoaDonDichVu? MaHoaDonNavigation { get; set; }

    public virtual ICollection<LichSuSuDungPhong> LichSuSuDungPhongs { get; set; } = new List<LichSuSuDungPhong>();
}