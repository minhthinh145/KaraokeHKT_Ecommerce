using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLQuanKaraokeHKT.Domain.Entities;

public partial class MonAn
{
    public int MaMonAn { get; set; }

    public int MaSanPham { get; set; }

    public int SoLuongConLai { get; set; }

    public int? MaVatLieu { get; set; }
    public virtual VatLieu? VatLieu { get; set; }
    public virtual SanPhamDichVu MaSanPhamNavigation { get; set; } = null!;

}
