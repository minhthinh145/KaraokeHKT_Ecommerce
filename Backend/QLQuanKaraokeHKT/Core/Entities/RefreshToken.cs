using System;
using System.Collections.Generic;

namespace QLQuanKaraokeHKT.Core.Entities;

public partial class RefreshToken
{
    public int RefreshTokenId { get; set; }

    public string ChuoiRefreshToken { get; set; } = null!;

    public Guid MaTaiKhoan { get; set; }

    public DateTime ThoiGianTao { get; set; }

    public DateTime ThoiGianHetHan { get; set; }
    public bool IsActive => DateTime.UtcNow <= ThoiGianHetHan && Revoked == null;
    public DateTime? Revoked { get; set; }

    public virtual TaiKhoan MaTaiKhoanNavigation { get; set; } = null!;
}
