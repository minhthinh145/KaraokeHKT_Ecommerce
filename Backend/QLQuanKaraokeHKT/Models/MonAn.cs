using System;
using System.Collections.Generic;

namespace QLQuanKaraokeHKT.Models;

public partial class MonAn
{
    public int MaMonAn { get; set; }

    public int MaSanPham { get; set; }

    public int SoLuongConLai { get; set; }

    public virtual SanPhamDichVu MaSanPhamNavigation { get; set; } = null!;
}
