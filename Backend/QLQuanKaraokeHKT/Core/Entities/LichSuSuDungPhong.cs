using System;
using System.Collections.Generic;

namespace QLQuanKaraokeHKT.Core.Entities;

public partial class LichSuSuDungPhong
{
    public int IdLichSu { get; set; }

    public int MaPhong { get; set; }

    public DateTime ThoiGianBatDau { get; set; }

    public DateTime ThoiGianKetThuc { get; set; }

    public Guid MaKhachHang { get; set; }

    public Guid? MaHoaDon { get; set; }
    public Guid? MaThuePhong { get; set; }
    public virtual ThuePhong? MaThuePhongNavigation { get; set; }

    public virtual HoaDonDichVu? MaHoaDonNavigation { get; set; }

    public virtual KhachHang MaKhachHangNavigation { get; set; } = null!;

    public virtual PhongHatKaraoke MaPhongNavigation { get; set; } = null!;
}
