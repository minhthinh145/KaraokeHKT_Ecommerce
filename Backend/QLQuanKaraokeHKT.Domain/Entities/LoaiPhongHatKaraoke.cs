using System;
using System.Collections.Generic;

namespace QLQuanKaraokeHKT.Domain.Entities;

public partial class LoaiPhongHatKaraoke
{
    public int MaLoaiPhong { get; set; }

    public string TenLoaiPhong { get; set; } = null!;

    public int SucChua { get; set; }

    public virtual ICollection<PhongHatKaraoke> PhongHatKaraokes { get; set; } = new List<PhongHatKaraoke>();
}
