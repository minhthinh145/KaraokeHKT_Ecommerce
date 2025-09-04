using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLQuanKaraokeHKT.Core.Entities;

public partial class PhongHatKaraoke
{
    public int MaPhong { get; set; }

    public int MaSanPham { get; set; }

    public int MaLoaiPhong { get; set; }

    public bool DangSuDung { get; set; }
    public bool NgungHoatDong { get; set; } = false;


    public virtual ICollection<LichSuSuDungPhong> LichSuSuDungPhongs { get; set; } = new List<LichSuSuDungPhong>();

    public virtual LoaiPhongHatKaraoke MaLoaiPhongNavigation { get; set; } = null!;

    public virtual SanPhamDichVu MaSanPhamNavigation { get; set; } = null!;

    public virtual ICollection<ThuePhong> ThuePhongs { get; set; } = new List<ThuePhong>();


    #region Computed Properties
    [NotMapped]
    public bool IsAvailableForBooking => !NgungHoatDong && !DangSuDung;
    [NotMapped]
    public bool IsActive => !NgungHoatDong;
    [NotMapped]
    public bool IsOutOfService => NgungHoatDong;
    [NotMapped]
    public bool IsOccupied => !NgungHoatDong && DangSuDung;
    #endregion
}
