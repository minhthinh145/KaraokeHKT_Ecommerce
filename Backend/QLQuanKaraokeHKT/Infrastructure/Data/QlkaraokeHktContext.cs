using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Core.Entities;
using System;
using System.Collections.Generic;

namespace QLQuanKaraokeHKT.Infrastructure.Data;

public partial class QlkaraokeHktContext : IdentityDbContext<TaiKhoan, VaiTro, Guid>
{
    public QlkaraokeHktContext()
    {
    }

    public QlkaraokeHktContext(DbContextOptions<QlkaraokeHktContext> options)
        : base(options)
    {
    }


    public virtual DbSet<CaLamViec> CaLamViecs { get; set; }
    public virtual DbSet<ChiTietHoaDonDichVu> ChiTietHoaDonDichVus { get; set; }
    public virtual DbSet<ChiTietPhieuHuyHang> ChiTietPhieuHuyHangs { get; set; }
    public virtual DbSet<ChiTietPhieuKiemHang> ChiTietPhieuKiemHangs { get; set; }
    public virtual DbSet<ChiTietPhieuNhapHang> ChiTietPhieuNhapHangs { get; set; }
    public virtual DbSet<GiaDichVu> GiaDichVus { get; set; }
    public virtual DbSet<GiaVatLieu> GiaVatLieus { get; set; }
    public virtual DbSet<HoaDonDichVu> HoaDonDichVus { get; set; }
    public virtual DbSet<KhachHang> KhachHangs { get; set; }
    public virtual DbSet<LichSuSuDungPhong> LichSuSuDungPhongs { get; set; }
    public virtual DbSet<LoaiPhongHatKaraoke> LoaiPhongHatKaraokes { get; set; }
    public virtual DbSet<MaOtp> MaOtps { get; set; }
    public virtual DbSet<MonAn> MonAns { get; set; }
    public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; }
    public virtual DbSet<NhanVien> NhanViens { get; set; }
    public virtual DbSet<PhieuHuyHang> PhieuHuyHangs { get; set; }
    public virtual DbSet<PhieuKiemHang> PhieuKiemHangs { get; set; }
    public virtual DbSet<PhieuLuong> PhieuLuongs { get; set; }
    public virtual DbSet<PhieuNhapHang> PhieuNhapHangs { get; set; }
    public virtual DbSet<PhongHatKaraoke> PhongHatKaraokes { get; set; }
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
    public virtual DbSet<SanPhamDichVu> SanPhamDichVus { get; set; }
    public virtual DbSet<ThuePhong> ThuePhongs { get; set; }
    public virtual DbSet<VatLieu> VatLieus { get; set; }
    public virtual DbSet<LichLamViec> LichLamViecs { get; set; }
    public virtual DbSet<LuongCaLamViec> LuongCaLamViecs { get; set; }
    public virtual DbSet<YeuCauChuyenCa> YeuCauChuyenCas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=QLKaraokeHKT;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<YeuCauChuyenCa>(entity =>
        {
            entity.HasKey(e => e.MaYeuCau).HasName("PK_YeuCauChuyenCa");
            entity.ToTable("YeuCauChuyenCa");

            entity.Property(e => e.MaYeuCau).HasColumnName("maYeuCau");
            entity.Property(e => e.MaLichLamViecGoc).HasColumnName("maLichLamViecGoc");
            entity.Property(e => e.NgayLamViecMoi).HasColumnName("ngayLamViecMoi");
            entity.Property(e => e.MaCaMoi).HasColumnName("maCaMoi");
            entity.Property(e => e.LyDoChuyenCa)
                .HasMaxLength(500)
                .HasColumnName("lyDoChuyenCa");
            entity.Property(e => e.DaPheDuyet).HasColumnName("daPheDuyet");
            entity.Property(e => e.KetQuaPheDuyet).HasColumnName("ketQuaPheDuyet");
            entity.Property(e => e.GhiChuPheDuyet)
                .HasMaxLength(500)
                .HasColumnName("ghiChuPheDuyet");
            entity.Property(e => e.NgayTaoYeuCau)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngayTaoYeuCau");
            entity.Property(e => e.NgayPheDuyet)
                .HasColumnType("datetime")
                .HasColumnName("ngayPheDuyet");

            // Foreign key constraints
            entity.HasOne(d => d.LichLamViecGoc)
                .WithMany()
                .HasForeignKey(d => d.MaLichLamViecGoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_YeuCauChuyenCa_LichLamViec");

            entity.HasOne(d => d.CaMoi)
                .WithMany()
                .HasForeignKey(d => d.MaCaMoi)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_YeuCauChuyenCa_CaLamViec");
        });

        modelBuilder.Entity<CaLamViec>(entity =>
        {
            entity.HasKey(e => e.MaCa).HasName("PK_CaLamViec");
            entity.ToTable("CaLamViec");

            entity.Property(e => e.MaCa).HasColumnName("maCa");
            entity.Property(e => e.TenCa).HasColumnName("tenCa").HasMaxLength(50);
            entity.Property(e => e.GioBatDauCa).HasColumnName("gioBatDauCa");
            entity.Property(e => e.GioKetThucCa).HasColumnName("gioKetThucCa");
        });
        modelBuilder.Entity<ChiTietHoaDonDichVu>(entity =>
        {
            entity.HasKey(e => e.MaCthoaDon).HasName("PK__ChiTietH__22113E7CA9E09973");

            entity.ToTable("ChiTietHoaDonDichVu");

            entity.Property(e => e.MaCthoaDon).HasColumnName("maCTHoaDon");
            entity.Property(e => e.MaHoaDon).HasColumnName("maHoaDon");
            entity.Property(e => e.MaSanPham).HasColumnName("maSanPham");
            entity.Property(e => e.SoLuong)
                .HasDefaultValue(1)
                .HasColumnName("soLuong");

            entity.HasOne(d => d.MaHoaDonNavigation).WithMany(p => p.ChiTietHoaDonDichVus)
                .HasForeignKey(d => d.MaHoaDon)
                .HasConstraintName("FK__ChiTietHo__maHoa__3D2915A8");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.ChiTietHoaDonDichVus)
                .HasForeignKey(d => d.MaSanPham)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietHo__maSan__3E1D39E1");
        });

        modelBuilder.Entity<ChiTietPhieuHuyHang>(entity =>
        {
            entity.HasKey(e => e.MaChiTietPhieu).HasName("PK__ChiTietP__D8405232CEEF559A");

            entity.ToTable("ChiTietPhieuHuyHang");

            entity.Property(e => e.MaChiTietPhieu).HasColumnName("maChiTietPhieu");
            entity.Property(e => e.MaPhieuHuyHang).HasColumnName("maPhieuHuyHang");
            entity.Property(e => e.MaVatLieu).HasColumnName("maVatLieu");
            entity.Property(e => e.SoLuongVatLieuHuy).HasColumnName("soLuongVatLieuHuy");

            entity.HasOne(d => d.MaPhieuHuyHangNavigation).WithMany(p => p.ChiTietPhieuHuyHangs)
                .HasForeignKey(d => d.MaPhieuHuyHang)
                .HasConstraintName("FK__ChiTietPh__maPhi__690797E6");

            entity.HasOne(d => d.MaVatLieuNavigation).WithMany(p => p.ChiTietPhieuHuyHangs)
                .HasForeignKey(d => d.MaVatLieu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietPh__maVat__69FBBC1F");
        });

        modelBuilder.Entity<ChiTietPhieuKiemHang>(entity =>
        {
            entity.HasKey(e => e.MaChiTietPhieu).HasName("PK__ChiTietP__D8405232A82E4C91");

            entity.ToTable("ChiTietPhieuKiemHang");

            entity.Property(e => e.MaChiTietPhieu).HasColumnName("maChiTietPhieu");
            entity.Property(e => e.ChenhLech)
                .HasComputedColumnSql("([soLuongThucTe]-[soLuongTheoSo])", false)
                .HasColumnName("chenhLech");
            entity.Property(e => e.GhiChu)
                .HasMaxLength(300)
                .HasColumnName("ghiChu");
            entity.Property(e => e.MaPhieuKiemHang).HasColumnName("maPhieuKiemHang");
            entity.Property(e => e.MaVatLieu).HasColumnName("maVatLieu");
            entity.Property(e => e.SoLuongTheoSo).HasColumnName("soLuongTheoSo");
            entity.Property(e => e.SoLuongThucTe).HasColumnName("soLuongThucTe");

            entity.HasOne(d => d.MaPhieuKiemHangNavigation).WithMany(p => p.ChiTietPhieuKiemHangs)
                .HasForeignKey(d => d.MaPhieuKiemHang)
                .HasConstraintName("FK__ChiTietPh__maPhi__719CDDE7");

            entity.HasOne(d => d.MaVatLieuNavigation).WithMany(p => p.ChiTietPhieuKiemHangs)
                .HasForeignKey(d => d.MaVatLieu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietPh__maVat__72910220");
        });

        modelBuilder.Entity<ChiTietPhieuNhapHang>(entity =>
        {
            entity.HasKey(e => e.MaChiTietPhieu).HasName("PK__ChiTietP__D8405232B9AC7D2C");

            entity.ToTable("ChiTietPhieuNhapHang");

            entity.Property(e => e.MaChiTietPhieu).HasColumnName("maChiTietPhieu");
            entity.Property(e => e.MaPhieuNhapHang).HasColumnName("maPhieuNhapHang");
            entity.Property(e => e.MaVatLieu).HasColumnName("maVatLieu");
            entity.Property(e => e.SoLuongVatLieuNhap).HasColumnName("soLuongVatLieuNhap");

            entity.HasOne(d => d.MaPhieuNhapHangNavigation).WithMany(p => p.ChiTietPhieuNhapHangs)
                .HasForeignKey(d => d.MaPhieuNhapHang)
                .HasConstraintName("FK__ChiTietPh__maPhi__607251E5");

            entity.HasOne(d => d.MaVatLieuNavigation).WithMany(p => p.ChiTietPhieuNhapHangs)
                .HasForeignKey(d => d.MaVatLieu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietPh__maVat__6166761E");
        });

        modelBuilder.Entity<GiaDichVu>(entity =>
        {
            entity.HasKey(e => e.MaGiaDichVu).HasName("PK__GiaDichV__E5242F374B0CF2DF");

            entity.ToTable("GiaDichVu");

            entity.HasIndex(e => new { e.MaSanPham, e.NgayApDung }, "IX_GiaDichVu_SanPham_NgayApDung");
            entity.Property(e => e.MaCa).HasColumnName("MaCa");
    
            entity.Property(e => e.MaGiaDichVu).HasColumnName("maGiaDichVu");
            entity.Property(e => e.DonGia)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("donGia");
            entity.Property(e => e.MaSanPham).HasColumnName("maSanPham");
            entity.Property(e => e.NgayApDung)
                .HasDefaultValueSql("(CONVERT([date],getdate()))")
                .HasColumnName("ngayApDung");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasDefaultValue("HieuLuc")
                .HasColumnName("trangThai");


            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.GiaDichVus)
                .HasForeignKey(d => d.MaSanPham)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GiaDichVu__maSan__30C33EC3");

            entity.HasOne(d => d.MaCaNavigation)
                .WithMany(p => p.GiaDichVus)
                .HasForeignKey(d => d.MaCa)
                .OnDelete(DeleteBehavior.SetNull);
        });
      

        modelBuilder.Entity<GiaVatLieu>(entity =>
        {
            entity.HasKey(e => e.MaGiaVatLieu).HasName("PK__GiaVatLi__7CD9F14AF5AAD9C1");

            entity.ToTable("GiaVatLieu");

            entity.HasIndex(e => new { e.MaVatLieu, e.NgayApDung }, "IX_GiaVatLieu_VatLieu_NgayApDung");

            entity.Property(e => e.MaGiaVatLieu).HasColumnName("maGiaVatLieu");
            entity.Property(e => e.DonGia)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("donGia");
            entity.Property(e => e.MaVatLieu).HasColumnName("maVatLieu");
            entity.Property(e => e.NgayApDung)
                .HasDefaultValueSql("(CONVERT([date],getdate()))")
                .HasColumnName("ngayApDung");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasDefaultValue("HieuLuc")
                .HasColumnName("trangThai");

            entity.HasOne(d => d.MaVatLieuNavigation).WithMany(p => p.GiaVatLieus)
                .HasForeignKey(d => d.MaVatLieu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GiaVatLie__maVat__57DD0BE4");
        });

        modelBuilder.Entity<HoaDonDichVu>(entity =>
        {
            entity.HasKey(e => e.MaHoaDon).HasName("PK__HoaDonDi__026B4D9A25DB1AE2");

            entity.ToTable("HoaDonDichVu");

            entity.HasIndex(e => e.MaKhachHang, "IX_HoaDon_KhachHang");

            entity.HasIndex(e => e.NgayTao, "IX_HoaDon_NgayTao");
            entity.HasIndex(e => e.TrangThai, "IX_HoaDon_TrangThai");

            entity.Property(e => e.MaHoaDon)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("maHoaDon");
            entity.Property(e => e.MaKhachHang).HasColumnName("maKhachHang");
            entity.Property(e => e.MoTaHoaDon)
                .HasMaxLength(500)
                .HasColumnName("moTaHoaDon");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngayTao");
            entity.Property(e => e.TenHoaDon)
                .HasMaxLength(200)
                .HasColumnName("tenHoaDon");
            entity.Property(e => e.TongTienHoaDon)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("tongTienHoaDon");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasDefaultValue("ChoThanhToan")
                .HasColumnName("trangThai");
            
            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.HoaDonDichVus)
                .HasForeignKey(d => d.MaKhachHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HoaDonDic__maKha__3864608B");

          
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.MaKhachHang).HasName("PK__KhachHan__0CCB3D497F337DE2");

            entity.ToTable("KhachHang");

            entity.HasIndex(e => e.MaTaiKhoan, "IX_KhachHang_TaiKhoan");

            entity.Property(e => e.MaKhachHang)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("maKhachHang");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.MaTaiKhoan).HasColumnName("maTaiKhoan");
            entity.Property(e => e.NgaySinh).HasColumnName("ngaySinh");
            entity.Property(e => e.TenKhachHang)
                .HasMaxLength(100)
                .HasColumnName("tenKhachHang");

            entity.HasOne(d => d.MaTaiKhoanNavigation).WithMany(p => p.KhachHangs)
                .HasForeignKey(d => d.MaTaiKhoan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KhachHang__maTai__123EB7A3");
        });

        modelBuilder.Entity<LichSuSuDungPhong>(entity =>
        {
            entity.HasKey(e => e.IdLichSu).HasName("PK__LichSuSu__C6FF38160F0FFDA4");

            entity.ToTable("LichSuSuDungPhong");

            entity.Property(e => e.IdLichSu).HasColumnName("idLichSu");
            entity.Property(e => e.MaHoaDon).HasColumnName("maHoaDon");
            entity.Property(e => e.MaKhachHang).HasColumnName("maKhachHang");
            entity.Property(e => e.MaPhong).HasColumnName("maPhong");
            entity.Property(e => e.ThoiGianBatDau)
                .HasColumnType("datetime")
                .HasColumnName("thoiGianBatDau");
            entity.Property(e => e.ThoiGianKetThuc)
                .HasColumnType("datetime")
                .HasColumnName("thoiGianKetThuc");

            entity.HasOne(d => d.MaHoaDonNavigation).WithMany(p => p.LichSuSuDungPhongs)
                .HasForeignKey(d => d.MaHoaDon)
                .HasConstraintName("FK__LichSuSuD__maHoa__4A8310C6");

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.LichSuSuDungPhongs)
                .HasForeignKey(d => d.MaKhachHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LichSuSuD__maKha__498EEC8D");

            entity.HasOne(d => d.MaPhongNavigation).WithMany(p => p.LichSuSuDungPhongs)
                .HasForeignKey(d => d.MaPhong)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LichSuSuD__maPho__489AC854");

            entity.HasOne(d => d.MaThuePhongNavigation).WithMany(p => p.LichSuSuDungPhongs)
                .HasForeignKey(d => d.MaThuePhong)
                .HasConstraintName("FK__LichSuSuD__maThu__4B7734FF");
        });

        modelBuilder.Entity<LoaiPhongHatKaraoke>(entity =>
        {
            entity.HasKey(e => e.MaLoaiPhong).HasName("PK__loaiPhon__106832AAF41DD649");

            entity.ToTable("loaiPhongHatKaraoke");

            entity.Property(e => e.MaLoaiPhong).HasColumnName("maLoaiPhong");
            entity.Property(e => e.SucChua).HasColumnName("sucChua");
            entity.Property(e => e.TenLoaiPhong)
                .HasMaxLength(50)
                .HasColumnName("tenLoaiPhong");
        });

        modelBuilder.Entity<MaOtp>(entity =>
        {
            entity.HasKey(e => e.IdOtp).HasName("PK__MaOTP__3CB844F21AE80C6C");

            entity.ToTable("MaOTP");

            entity.HasIndex(e => e.MaTaiKhoan, "IX_MaOTP_TaiKhoan");

            entity.Property(e => e.IdOtp).HasColumnName("idOTP");
            entity.Property(e => e.DaSuDung).HasColumnName("daSuDung");
            entity.Property(e => e.maOTP)
                .HasMaxLength(10)
                .HasColumnName("maOTP");
            entity.Property(e => e.MaTaiKhoan).HasColumnName("maTaiKhoan");
            entity.Property(e => e.NgayHetHan)
                .HasColumnType("datetime")
                .HasColumnName("ngayHetHan");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngayTao");

            entity.HasOne(d => d.MaTaiKhoanNavigation).WithMany(p => p.MaOtps)
                .HasForeignKey(d => d.MaTaiKhoan)
                .HasConstraintName("FK__MaOTP__maTaiKhoa__08B54D69");
        });

        modelBuilder.Entity<MonAn>(entity =>
        {
            entity.HasKey(e => e.MaMonAn).HasName("PK__MonAn__2D20DD221681F916");

            entity.ToTable("MonAn");

            entity.Property(e => e.MaMonAn).HasColumnName("maMonAn");
            entity.Property(e => e.MaSanPham).HasColumnName("maSanPham");
            entity.Property(e => e.SoLuongConLai).HasColumnName("soLuongConLai");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.MonAns)
                .HasForeignKey(d => d.MaSanPham)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MonAn__maSanPham__22751F6C");

            entity.HasOne(m => m.VatLieu)
                   .WithOne(m => m.MonAn)
                   .HasForeignKey<MonAn>(m => m.MaVatLieu)
                   .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<NhaCungCap>(entity =>
        {
            entity.HasKey(e => e.MaNhaCungCap).HasName("PK__NhaCungC__D0B4D6DE5A14D52C");

            entity.ToTable("NhaCungCap");

            entity.Property(e => e.MaNhaCungCap).HasColumnName("maNhaCungCap");
            entity.Property(e => e.DiaChi)
                .HasMaxLength(300)
                .HasColumnName("diaChi");
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(15)
                .HasColumnName("soDienThoai");
            entity.Property(e => e.TenNhaCungCap)
                .HasMaxLength(200)
                .HasColumnName("tenNhaCungCap");
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.MaNv).HasName("PK__NhanVien__7A3EC7D5D566A33A");

            entity.ToTable("NhanVien");

            entity.HasIndex(e => e.MaTaiKhoan, "IX_NhanVien_TaiKhoan");

            entity.HasIndex(e => e.Email, "UQ__NhanVien__AB6E61642A8F12D3").IsUnique();

            entity.Property(e => e.MaNv)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("maNV");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.HoTen)
                .HasMaxLength(100)
                .HasColumnName("hoTen");
            entity.Property(e => e.LoaiNhanVien)
                .HasMaxLength(30)
                .HasColumnName("loaiNhanVien");
            entity.Property(e => e.MaTaiKhoan).HasColumnName("maTaiKhoan");
            entity.Property(e => e.NgaySinh).HasColumnName("ngaySinh");
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(15)
                .HasColumnName("soDienThoai");

            entity.HasOne(d => d.MaTaiKhoanNavigation).WithMany(p => p.NhanViens)
                .HasForeignKey(d => d.MaTaiKhoan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NhanVien__maTaiK__0E6E26BF");
        });

        modelBuilder.Entity<PhieuHuyHang>(entity =>
        {
            entity.HasKey(e => e.MaPhieuHuyHang).HasName("PK__PhieuHuy__33F1711020CBF93C");

            entity.ToTable("PhieuHuyHang");

            entity.Property(e => e.MaPhieuHuyHang).HasColumnName("maPhieuHuyHang");
            entity.Property(e => e.MoTaPhieu)
                .HasMaxLength(500)
                .HasColumnName("moTaPhieu");
            entity.Property(e => e.NgayTaoPhieu)
                .HasDefaultValueSql("(CONVERT([date],getdate()))")
                .HasColumnName("ngayTaoPhieu");
            entity.Property(e => e.TenPhieu)
                .HasMaxLength(200)
                .HasColumnName("tenPhieu");
            entity.Property(e => e.TongTienThatThoat)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("tongTienThatThoat");
        });

        modelBuilder.Entity<PhieuKiemHang>(entity =>
        {
            entity.HasKey(e => e.MaPhieuKiemHang).HasName("PK__PhieuKie__483B07F2550A1DF0");

            entity.ToTable("PhieuKiemHang");

            entity.Property(e => e.MaPhieuKiemHang).HasColumnName("maPhieuKiemHang");
            entity.Property(e => e.MoTaPhieu)
                .HasMaxLength(500)
                .HasColumnName("moTaPhieu");
            entity.Property(e => e.NgayKiemKe)
                .HasDefaultValueSql("(CONVERT([date],getdate()))")
                .HasColumnName("ngayKiemKe");
        });

        modelBuilder.Entity<PhieuLuong>(entity =>
        {
            entity.HasKey(e => e.MaPhieuLuong).HasName("PK__PhieuLuo__8D6F261279E4BA60");

            entity.ToTable("PhieuLuong");

            entity.Property(e => e.MaPhieuLuong).HasColumnName("maPhieuLuong");
            entity.Property(e => e.HeSoLuong)
                .HasDefaultValue(1.0m)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("heSoLuong");
            entity.Property(e => e.MaNhanVien).HasColumnName("maNhanVien");
            entity.Property(e => e.NgayIn)
                .HasDefaultValueSql("(CONVERT([date],getdate()))")
                .HasColumnName("ngayIn");
            entity.Property(e => e.TongTienLuong)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("tongTienLuong");

            entity.HasOne(d => d.MaNhanVienNavigation).WithMany(p => p.PhieuLuongs)
                .HasForeignKey(d => d.MaNhanVien)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PhieuLuon__maNha__17F790F9");
        });

        modelBuilder.Entity<PhieuNhapHang>(entity =>
        {
            entity.HasKey(e => e.MaPhieuNhapHang).HasName("PK__PhieuNha__E3AF626E6E25EC46");

            entity.ToTable("PhieuNhapHang");

            entity.Property(e => e.MaPhieuNhapHang).HasColumnName("maPhieuNhapHang");
            entity.Property(e => e.MaNhaCungCap).HasColumnName("maNhaCungCap");
            entity.Property(e => e.MoTaPhieu)
                .HasMaxLength(500)
                .HasColumnName("moTaPhieu");
            entity.Property(e => e.NgayTaoPhieu)
                .HasDefaultValueSql("(CONVERT([date],getdate()))")
                .HasColumnName("ngayTaoPhieu");
            entity.Property(e => e.TenPhieu)
                .HasMaxLength(200)
                .HasColumnName("tenPhieu");
            entity.Property(e => e.TongTienNhap)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("tongTienNhap");

            entity.HasOne(d => d.MaNhaCungCapNavigation).WithMany(p => p.PhieuNhapHangs)
                .HasForeignKey(d => d.MaNhaCungCap)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PhieuNhap__maNha__5CA1C101");
        });

        modelBuilder.Entity<PhongHatKaraoke>(entity =>
        {
            entity.HasKey(e => e.MaPhong).HasName("PK__PhongHat__4CD55E1057E1C2E4");

            entity.ToTable("PhongHatKaraoke");

            entity.Property(e => e.MaPhong).HasColumnName("maPhong");
            entity.Property(e => e.DangSuDung).HasColumnName("dangSuDung");
            entity.Property(e => e.MaLoaiPhong).HasColumnName("maLoaiPhong");
            entity.Property(e => e.MaSanPham).HasColumnName("maSanPham");

            entity.HasOne(d => d.MaLoaiPhongNavigation).WithMany(p => p.PhongHatKaraokes)
                .HasForeignKey(d => d.MaLoaiPhong)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PhongHatK__maLoa__2A164134");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.PhongHatKaraokes)
                .HasForeignKey(d => d.MaSanPham)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PhongHatK__maSan__29221CFB");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.RefreshTokenId).HasName("PK__RefreshT__FEAC95282D4E28E7");

            entity.ToTable("RefreshToken");

            entity.HasIndex(e => e.MaTaiKhoan, "IX_RefreshToken_TaiKhoan");

            entity.Property(e => e.RefreshTokenId).HasColumnName("refreshTokenID");
            entity.Property(e => e.ChuoiRefreshToken)
                .HasMaxLength(500)
                .HasColumnName("chuoiRefreshToken");
            entity.Property(e => e.MaTaiKhoan).HasColumnName("maTaiKhoan");
            entity.Property(e => e.ThoiGianHetHan)
                .HasColumnType("datetime")
                .HasColumnName("thoiGianHetHan");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("thoiGianTao");

            entity.HasOne(d => d.MaTaiKhoanNavigation).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.MaTaiKhoan)
                .HasConstraintName("FK__RefreshTo__maTai__03F0984C");
        });

        modelBuilder.Entity<SanPhamDichVu>(entity =>
        {
            entity.HasKey(e => e.MaSanPham).HasName("PK__SanPhamD__5B439C439A4B08B7");

            entity.ToTable("SanPhamDichVu");

            entity.Property(e => e.MaSanPham).HasColumnName("maSanPham");
            entity.Property(e => e.HinhAnhSanPham)
                .HasMaxLength(500)
                .HasColumnName("hinhAnhSanPham");
            entity.Property(e => e.TenSanPham)
                .HasMaxLength(200)
                .HasColumnName("tenSanPham");
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.ToTable("TaiKhoan");
            entity.HasKey(e => e.Id); // Sử dụng Id từ IdentityUser<Guid>

            // Map các columns
            entity.Property(e => e.Id).HasColumnName("maTaiKhoan");
            entity.Property(e => e.UserName).HasColumnName("tenDangNhap");
            entity.Property(e => e.daKichHoat).HasColumnName("daKichHoat");
            entity.Property(e => e.daBiKhoa).HasColumnName("daBiKhoa");
            entity.Property(e => e.loaiTaiKhoan).HasColumnName("loaiTaiKhoan");
        });

        modelBuilder.Entity<VaiTro>(entity =>
        {
            entity.ToTable("VaiTro");
            entity.Property(e => e.moTa).HasColumnName("moTa");
        });


        modelBuilder.Entity<ThuePhong>(entity =>
        {
            entity.HasKey(e => e.MaThuePhong).HasName("PK__ThuePhon__99717FBF068C52BC");

            entity.ToTable("ThuePhong");

            entity.HasIndex(e => e.MaPhong, "IX_ThuePhong_Phong");
            entity.HasIndex(e => e.MaHoaDon, "IX_ThuePhong_HoaDon");
            entity.HasIndex(e => e.TrangThai, "IX_ThuePhong_TrangThai");

            entity.Property(e => e.MaThuePhong)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("maThuePhong");
            entity.Property(e => e.MaKhachHang).HasColumnName("maKhachHang");
            entity.Property(e => e.MaPhong).HasColumnName("maPhong");
            entity.Property(e => e.MaHoaDon).HasColumnName("maHoaDon");
            entity.Property(e => e.ThoiGianBatDau)
                .HasColumnType("datetime")
                .HasColumnName("thoiGianBatDau");
            entity.Property(e => e.ThoiGianKetThuc)
                .HasColumnType("datetime")
                .HasColumnName("thoiGianKetThuc");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasDefaultValue("DangSuDung")
                .HasColumnName("trangThai");

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.ThuePhongs)
                .HasForeignKey(d => d.MaKhachHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ThuePhong__maKha__44CA3770");

            entity.HasOne(d => d.MaPhongNavigation).WithMany(p => p.ThuePhongs)
                .HasForeignKey(d => d.MaPhong)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ThuePhong__maPho__43D61337");

            entity.HasOne(d => d.MaHoaDonNavigation).WithMany(p => p.ThuePhongs)
                .HasForeignKey(d => d.MaHoaDon)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__ThuePhong__maHoa__45BE5BA9");
        });

        modelBuilder.Entity<VatLieu>(entity =>
        {
            entity.HasKey(e => e.MaVatLieu).HasName("PK__VatLieu__1B9590FA537F995B");

            entity.ToTable("VatLieu");

            entity.Property(e => e.MaVatLieu).HasColumnName("maVatLieu");
            entity.Property(e => e.DonViTinh)
                .HasMaxLength(20)
                .HasColumnName("donViTinh");
            entity.Property(e => e.SoLuongTonKho).HasColumnName("soLuongTonKho");
            entity.Property(e => e.TenVatLieu)
                .HasMaxLength(200)
                .HasColumnName("tenVatLieu");
        });

        modelBuilder.Entity<LichLamViec>(entity =>
        {
            entity.HasKey(e => e.MaLichLamViec).HasName("PK_LichLamViec");
            entity.ToTable("LichLamViec");

            entity.Property(e => e.MaLichLamViec).HasColumnName("maLichLamViec");
            entity.Property(e => e.NgayLamViec).HasColumnName("ngayLamViec");
            entity.Property(e => e.MaNhanVien).HasColumnName("maNhanVien");
            entity.Property(e => e.MaCa).HasColumnName("maCa");

            entity.HasOne(d => d.NhanVien)
                .WithMany(p => p.LichLamViecs)
                .HasForeignKey(d => d.MaNhanVien)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.CaLamViec)
                .WithMany(p => p.LichLamViecs)
                .HasForeignKey(d => d.MaCa)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<LuongCaLamViec>()
    .ToTable("LuongCaLamViec")
    .HasOne(l => l.CaLamViec)
    .WithMany(c => c.LuongCaLamViecs)
    .HasForeignKey(l => l.MaCa);


        modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("AspNetUserClaims");
        modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("AspNetUserLogins");
        modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("AspNetUserTokens");
        modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("AspNetRoleClaims");
        modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("AspNetUserRoles");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
