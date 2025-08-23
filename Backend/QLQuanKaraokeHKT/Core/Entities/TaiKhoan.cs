using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLQuanKaraokeHKT.Core.Entities
{
    [Table("TaiKhoan")]
    public class TaiKhoan : IdentityUser<Guid>
    {
        [Column("maTaiKhoan")]
        public override Guid Id { get; set; }

        [Column("tenDangNhap")]
        public override string UserName { get; set; } = null!;

        public string FullName { get; set; } = null!;

        [Column("daKichHoat")]
        public bool daKichHoat { get; set; }

        [Column("daBiKhoa")]
        public bool daBiKhoa { get; set; }

        [Column("loaiTaiKhoan")]
        public string loaiTaiKhoan { get; set; } = null!;

        public virtual ICollection<KhachHang> KhachHangs { get; set; } = new List<KhachHang>();
        public virtual ICollection<NhanVien> NhanViens { get; set; } = new List<NhanVien>();
        public virtual ICollection<MaOtp> MaOtps { get; set; } = new List<MaOtp>();
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}