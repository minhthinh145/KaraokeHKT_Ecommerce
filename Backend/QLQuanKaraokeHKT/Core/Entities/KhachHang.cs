using System;
using System.Collections.Generic;

namespace QLQuanKaraokeHKT.Core.Entities;

public partial class KhachHang
{
    public Guid MaKhachHang { get; set; }

    public string TenKhachHang { get; set; } = null!;

    public string? Email { get; set; }

    public DateOnly? NgaySinh { get; set; }

    public Guid MaTaiKhoan { get; set; }

    public virtual ICollection<HoaDonDichVu> HoaDonDichVus { get; set; } = new List<HoaDonDichVu>();

    public virtual ICollection<LichSuSuDungPhong> LichSuSuDungPhongs { get; set; } = new List<LichSuSuDungPhong>();

    public virtual TaiKhoan MaTaiKhoanNavigation { get; set; } = null!;

    public virtual ICollection<ThuePhong> ThuePhongs { get; set; } = new List<ThuePhong>();
}
