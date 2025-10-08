using System;
using System.Collections.Generic;

namespace QLQuanKaraokeHKT.Domain.Entities;

public partial class SanPhamDichVu
{
    public int MaSanPham { get; set; }

    public string TenSanPham { get; set; } = null!;

    public string? HinhAnhSanPham { get; set; }

    public virtual ICollection<ChiTietHoaDonDichVu> ChiTietHoaDonDichVus { get; set; } = new List<ChiTietHoaDonDichVu>();

    public virtual ICollection<GiaDichVu> GiaDichVus { get; set; } = new List<GiaDichVu>();

    public virtual ICollection<MonAn> MonAns { get; set; } = new List<MonAn>();

    public virtual ICollection<PhongHatKaraoke> PhongHatKaraokes { get; set; } = new List<PhongHatKaraoke>();
}
