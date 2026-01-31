using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLQuanKaraokeHKT.Domain.Entities;

public partial class GiaDichVu
{
    public int MaGiaDichVu { get; set; }

    public decimal DonGia { get; set; }

    public int MaSanPham { get; set; }

    public DateOnly NgayApDung { get; set; }

    public string TrangThai { get; set; } = null!;
    [Column("MaCa")]
    public int? MaCa { get; set; }

    public virtual SanPhamDichVu MaSanPhamNavigation { get; set; } = null!;
    public virtual CaLamViec? MaCaNavigation { get; set; }

}
