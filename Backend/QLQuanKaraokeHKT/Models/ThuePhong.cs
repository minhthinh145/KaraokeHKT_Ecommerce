using System;
using System.Collections.Generic;

namespace QLQuanKaraokeHKT.Models;

public partial class ThuePhong
{
    public Guid MaThuePhong { get; set; }

    public int MaPhong { get; set; }

    public Guid MaKhachHang { get; set; }

    public DateTime ThoiGianBatDau { get; set; }

    public DateTime? ThoiGianKetThuc { get; set; }

    public string TrangThai { get; set; } = null!;

    public virtual KhachHang MaKhachHangNavigation { get; set; } = null!;

    public virtual PhongHatKaraoke MaPhongNavigation { get; set; } = null!;
}
