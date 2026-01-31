using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLQuanKaraokeHKT.Migrations
{
    /// <inheritdoc />
    public partial class Inital : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "loaiPhongHatKaraoke",
                columns: table => new
                {
                    maLoaiPhong = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenLoaiPhong = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    sucChua = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__loaiPhon__106832AAF41DD649", x => x.maLoaiPhong);
                });

            migrationBuilder.CreateTable(
                name: "NhaCungCap",
                columns: table => new
                {
                    maNhaCungCap = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenNhaCungCap = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    diaChi = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    soDienThoai = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NhaCungC__D0B4D6DE5A14D52C", x => x.maNhaCungCap);
                });

            migrationBuilder.CreateTable(
                name: "PhieuHuyHang",
                columns: table => new
                {
                    maPhieuHuyHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenPhieu = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    moTaPhieu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ngayTaoPhieu = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "(CONVERT([date],getdate()))"),
                    tongTienThatThoat = table.Column<decimal>(type: "decimal(15,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PhieuHuy__33F1711020CBF93C", x => x.maPhieuHuyHang);
                });

            migrationBuilder.CreateTable(
                name: "PhieuKiemHang",
                columns: table => new
                {
                    maPhieuKiemHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    moTaPhieu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ngayKiemKe = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "(CONVERT([date],getdate()))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PhieuKie__483B07F2550A1DF0", x => x.maPhieuKiemHang);
                });

            migrationBuilder.CreateTable(
                name: "SanPhamDichVu",
                columns: table => new
                {
                    maSanPham = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenSanPham = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    hinhAnhSanPham = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SanPhamD__5B439C439A4B08B7", x => x.maSanPham);
                });

            migrationBuilder.CreateTable(
                name: "TaiKhoan",
                columns: table => new
                {
                    maTaiKhoan = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    tenDangNhap = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    daKichHoat = table.Column<bool>(type: "bit", nullable: false),
                    daBiKhoa = table.Column<bool>(type: "bit", nullable: false),
                    loaiTaiKhoan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiKhoan", x => x.maTaiKhoan);
                });

            migrationBuilder.CreateTable(
                name: "VaiTro",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    moTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaiTro", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VatLieu",
                columns: table => new
                {
                    maVatLieu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenVatLieu = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    donViTinh = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    soLuongTonKho = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__VatLieu__1B9590FA537F995B", x => x.maVatLieu);
                });

            migrationBuilder.CreateTable(
                name: "PhieuNhapHang",
                columns: table => new
                {
                    maPhieuNhapHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenPhieu = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    moTaPhieu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ngayTaoPhieu = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "(CONVERT([date],getdate()))"),
                    tongTienNhap = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    maNhaCungCap = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PhieuNha__E3AF626E6E25EC46", x => x.maPhieuNhapHang);
                    table.ForeignKey(
                        name: "FK__PhieuNhap__maNha__5CA1C101",
                        column: x => x.maNhaCungCap,
                        principalTable: "NhaCungCap",
                        principalColumn: "maNhaCungCap");
                });

            migrationBuilder.CreateTable(
                name: "GiaDichVu",
                columns: table => new
                {
                    maGiaDichVu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    donGia = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    maSanPham = table.Column<int>(type: "int", nullable: false),
                    ngayApDung = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "(CONVERT([date],getdate()))"),
                    trangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "HieuLuc")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__GiaDichV__E5242F374B0CF2DF", x => x.maGiaDichVu);
                    table.ForeignKey(
                        name: "FK__GiaDichVu__maSan__30C33EC3",
                        column: x => x.maSanPham,
                        principalTable: "SanPhamDichVu",
                        principalColumn: "maSanPham");
                });

            migrationBuilder.CreateTable(
                name: "MonAn",
                columns: table => new
                {
                    maMonAn = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    maSanPham = table.Column<int>(type: "int", nullable: false),
                    soLuongConLai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MonAn__2D20DD221681F916", x => x.maMonAn);
                    table.ForeignKey(
                        name: "FK__MonAn__maSanPham__22751F6C",
                        column: x => x.maSanPham,
                        principalTable: "SanPhamDichVu",
                        principalColumn: "maSanPham");
                });

            migrationBuilder.CreateTable(
                name: "PhongHatKaraoke",
                columns: table => new
                {
                    maPhong = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    maSanPham = table.Column<int>(type: "int", nullable: false),
                    maLoaiPhong = table.Column<int>(type: "int", nullable: false),
                    dangSuDung = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PhongHat__4CD55E1057E1C2E4", x => x.maPhong);
                    table.ForeignKey(
                        name: "FK__PhongHatK__maLoa__2A164134",
                        column: x => x.maLoaiPhong,
                        principalTable: "loaiPhongHatKaraoke",
                        principalColumn: "maLoaiPhong");
                    table.ForeignKey(
                        name: "FK__PhongHatK__maSan__29221CFB",
                        column: x => x.maSanPham,
                        principalTable: "SanPhamDichVu",
                        principalColumn: "maSanPham");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_TaiKhoan_UserId",
                        column: x => x.UserId,
                        principalTable: "TaiKhoan",
                        principalColumn: "maTaiKhoan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_TaiKhoan_UserId",
                        column: x => x.UserId,
                        principalTable: "TaiKhoan",
                        principalColumn: "maTaiKhoan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_TaiKhoan_UserId",
                        column: x => x.UserId,
                        principalTable: "TaiKhoan",
                        principalColumn: "maTaiKhoan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KhachHang",
                columns: table => new
                {
                    maKhachHang = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    tenKhachHang = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ngaySinh = table.Column<DateOnly>(type: "date", nullable: true),
                    maTaiKhoan = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__KhachHan__0CCB3D497F337DE2", x => x.maKhachHang);
                    table.ForeignKey(
                        name: "FK__KhachHang__maTai__123EB7A3",
                        column: x => x.maTaiKhoan,
                        principalTable: "TaiKhoan",
                        principalColumn: "maTaiKhoan");
                });

            migrationBuilder.CreateTable(
                name: "MaOTP",
                columns: table => new
                {
                    idOTP = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    maOTP = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ngayTao = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ngayHetHan = table.Column<DateTime>(type: "datetime", nullable: false),
                    daSuDung = table.Column<bool>(type: "bit", nullable: false),
                    maTaiKhoan = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MaOTP__3CB844F21AE80C6C", x => x.idOTP);
                    table.ForeignKey(
                        name: "FK__MaOTP__maTaiKhoa__08B54D69",
                        column: x => x.maTaiKhoan,
                        principalTable: "TaiKhoan",
                        principalColumn: "maTaiKhoan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NhanVien",
                columns: table => new
                {
                    maNV = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    hoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ngaySinh = table.Column<DateOnly>(type: "date", nullable: true),
                    soDienThoai = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    loaiNhanVien = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    maTaiKhoan = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NhanVien__7A3EC7D5D566A33A", x => x.maNV);
                    table.ForeignKey(
                        name: "FK__NhanVien__maTaiK__0E6E26BF",
                        column: x => x.maTaiKhoan,
                        principalTable: "TaiKhoan",
                        principalColumn: "maTaiKhoan");
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    refreshTokenID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    chuoiRefreshToken = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    maTaiKhoan = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    thoiGianTao = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    thoiGianHetHan = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__RefreshT__FEAC95282D4E28E7", x => x.refreshTokenID);
                    table.ForeignKey(
                        name: "FK__RefreshTo__maTai__03F0984C",
                        column: x => x.maTaiKhoan,
                        principalTable: "TaiKhoan",
                        principalColumn: "maTaiKhoan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_VaiTro_RoleId",
                        column: x => x.RoleId,
                        principalTable: "VaiTro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_TaiKhoan_UserId",
                        column: x => x.UserId,
                        principalTable: "TaiKhoan",
                        principalColumn: "maTaiKhoan",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_VaiTro_RoleId",
                        column: x => x.RoleId,
                        principalTable: "VaiTro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietPhieuHuyHang",
                columns: table => new
                {
                    maChiTietPhieu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    maPhieuHuyHang = table.Column<int>(type: "int", nullable: false),
                    soLuongVatLieuHuy = table.Column<int>(type: "int", nullable: false),
                    maVatLieu = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ChiTietP__D8405232CEEF559A", x => x.maChiTietPhieu);
                    table.ForeignKey(
                        name: "FK__ChiTietPh__maPhi__690797E6",
                        column: x => x.maPhieuHuyHang,
                        principalTable: "PhieuHuyHang",
                        principalColumn: "maPhieuHuyHang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__ChiTietPh__maVat__69FBBC1F",
                        column: x => x.maVatLieu,
                        principalTable: "VatLieu",
                        principalColumn: "maVatLieu");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietPhieuKiemHang",
                columns: table => new
                {
                    maChiTietPhieu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    maPhieuKiemHang = table.Column<int>(type: "int", nullable: false),
                    soLuongTheoSo = table.Column<int>(type: "int", nullable: false),
                    soLuongThucTe = table.Column<int>(type: "int", nullable: false),
                    chenhLech = table.Column<int>(type: "int", nullable: true, computedColumnSql: "([soLuongThucTe]-[soLuongTheoSo])", stored: false),
                    ghiChu = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    maVatLieu = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ChiTietP__D8405232A82E4C91", x => x.maChiTietPhieu);
                    table.ForeignKey(
                        name: "FK__ChiTietPh__maPhi__719CDDE7",
                        column: x => x.maPhieuKiemHang,
                        principalTable: "PhieuKiemHang",
                        principalColumn: "maPhieuKiemHang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__ChiTietPh__maVat__72910220",
                        column: x => x.maVatLieu,
                        principalTable: "VatLieu",
                        principalColumn: "maVatLieu");
                });

            migrationBuilder.CreateTable(
                name: "GiaVatLieu",
                columns: table => new
                {
                    maGiaVatLieu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    donGia = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    maVatLieu = table.Column<int>(type: "int", nullable: false),
                    ngayApDung = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "(CONVERT([date],getdate()))"),
                    trangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "HieuLuc")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__GiaVatLi__7CD9F14AF5AAD9C1", x => x.maGiaVatLieu);
                    table.ForeignKey(
                        name: "FK__GiaVatLie__maVat__57DD0BE4",
                        column: x => x.maVatLieu,
                        principalTable: "VatLieu",
                        principalColumn: "maVatLieu");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietPhieuNhapHang",
                columns: table => new
                {
                    maChiTietPhieu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    maPhieuNhapHang = table.Column<int>(type: "int", nullable: false),
                    soLuongVatLieuNhap = table.Column<int>(type: "int", nullable: false),
                    maVatLieu = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ChiTietP__D8405232B9AC7D2C", x => x.maChiTietPhieu);
                    table.ForeignKey(
                        name: "FK__ChiTietPh__maPhi__607251E5",
                        column: x => x.maPhieuNhapHang,
                        principalTable: "PhieuNhapHang",
                        principalColumn: "maPhieuNhapHang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__ChiTietPh__maVat__6166761E",
                        column: x => x.maVatLieu,
                        principalTable: "VatLieu",
                        principalColumn: "maVatLieu");
                });

            migrationBuilder.CreateTable(
                name: "HoaDonDichVu",
                columns: table => new
                {
                    maHoaDon = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    tenHoaDon = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    moTaHoaDon = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    maKhachHang = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    tongTienHoaDon = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    ngayTao = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    trangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "ChoThanhToan")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__HoaDonDi__026B4D9A25DB1AE2", x => x.maHoaDon);
                    table.ForeignKey(
                        name: "FK__HoaDonDic__maKha__3864608B",
                        column: x => x.maKhachHang,
                        principalTable: "KhachHang",
                        principalColumn: "maKhachHang");
                });

            migrationBuilder.CreateTable(
                name: "ThuePhong",
                columns: table => new
                {
                    maThuePhong = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    maPhong = table.Column<int>(type: "int", nullable: false),
                    maKhachHang = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    thoiGianBatDau = table.Column<DateTime>(type: "datetime", nullable: false),
                    thoiGianKetThuc = table.Column<DateTime>(type: "datetime", nullable: true),
                    trangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "DangSuDung")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ThuePhon__99717FBF068C52BC", x => x.maThuePhong);
                    table.ForeignKey(
                        name: "FK__ThuePhong__maKha__44CA3770",
                        column: x => x.maKhachHang,
                        principalTable: "KhachHang",
                        principalColumn: "maKhachHang");
                    table.ForeignKey(
                        name: "FK__ThuePhong__maPho__43D61337",
                        column: x => x.maPhong,
                        principalTable: "PhongHatKaraoke",
                        principalColumn: "maPhong");
                });

            migrationBuilder.CreateTable(
                name: "CaLamViec",
                columns: table => new
                {
                    maCaLamViec = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ngayLamViec = table.Column<DateOnly>(type: "date", nullable: false),
                    gioBatDauCa = table.Column<TimeOnly>(type: "time", nullable: false),
                    gioKetThucCa = table.Column<TimeOnly>(type: "time", nullable: false),
                    maNhanVien = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CaLamVie__84019BCC8B786A53", x => x.maCaLamViec);
                    table.ForeignKey(
                        name: "FK__CaLamViec__maNha__1AD3FDA4",
                        column: x => x.maNhanVien,
                        principalTable: "NhanVien",
                        principalColumn: "maNV");
                });

            migrationBuilder.CreateTable(
                name: "PhieuLuong",
                columns: table => new
                {
                    maPhieuLuong = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    maNhanVien = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ngayIn = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "(CONVERT([date],getdate()))"),
                    heSoLuong = table.Column<decimal>(type: "decimal(10,2)", nullable: false, defaultValue: 1.0m),
                    tongTienLuong = table.Column<decimal>(type: "decimal(15,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PhieuLuo__8D6F261279E4BA60", x => x.maPhieuLuong);
                    table.ForeignKey(
                        name: "FK__PhieuLuon__maNha__17F790F9",
                        column: x => x.maNhanVien,
                        principalTable: "NhanVien",
                        principalColumn: "maNV");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietHoaDonDichVu",
                columns: table => new
                {
                    maCTHoaDon = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    maHoaDon = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    maSanPham = table.Column<int>(type: "int", nullable: false),
                    soLuong = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ChiTietH__22113E7CA9E09973", x => x.maCTHoaDon);
                    table.ForeignKey(
                        name: "FK__ChiTietHo__maHoa__3D2915A8",
                        column: x => x.maHoaDon,
                        principalTable: "HoaDonDichVu",
                        principalColumn: "maHoaDon",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__ChiTietHo__maSan__3E1D39E1",
                        column: x => x.maSanPham,
                        principalTable: "SanPhamDichVu",
                        principalColumn: "maSanPham");
                });

            migrationBuilder.CreateTable(
                name: "LichSuSuDungPhong",
                columns: table => new
                {
                    idLichSu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    maPhong = table.Column<int>(type: "int", nullable: false),
                    thoiGianBatDau = table.Column<DateTime>(type: "datetime", nullable: false),
                    thoiGianKetThuc = table.Column<DateTime>(type: "datetime", nullable: false),
                    maKhachHang = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    maHoaDon = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LichSuSu__C6FF38160F0FFDA4", x => x.idLichSu);
                    table.ForeignKey(
                        name: "FK__LichSuSuD__maHoa__4A8310C6",
                        column: x => x.maHoaDon,
                        principalTable: "HoaDonDichVu",
                        principalColumn: "maHoaDon");
                    table.ForeignKey(
                        name: "FK__LichSuSuD__maKha__498EEC8D",
                        column: x => x.maKhachHang,
                        principalTable: "KhachHang",
                        principalColumn: "maKhachHang");
                    table.ForeignKey(
                        name: "FK__LichSuSuD__maPho__489AC854",
                        column: x => x.maPhong,
                        principalTable: "PhongHatKaraoke",
                        principalColumn: "maPhong");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_CaLamViec_NgayLam",
                table: "CaLamViec",
                column: "ngayLamViec");

            migrationBuilder.CreateIndex(
                name: "IX_CaLamViec_NhanVien",
                table: "CaLamViec",
                column: "maNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDonDichVu_maHoaDon",
                table: "ChiTietHoaDonDichVu",
                column: "maHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDonDichVu_maSanPham",
                table: "ChiTietHoaDonDichVu",
                column: "maSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietPhieuHuyHang_maPhieuHuyHang",
                table: "ChiTietPhieuHuyHang",
                column: "maPhieuHuyHang");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietPhieuHuyHang_maVatLieu",
                table: "ChiTietPhieuHuyHang",
                column: "maVatLieu");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietPhieuKiemHang_maPhieuKiemHang",
                table: "ChiTietPhieuKiemHang",
                column: "maPhieuKiemHang");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietPhieuKiemHang_maVatLieu",
                table: "ChiTietPhieuKiemHang",
                column: "maVatLieu");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietPhieuNhapHang_maPhieuNhapHang",
                table: "ChiTietPhieuNhapHang",
                column: "maPhieuNhapHang");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietPhieuNhapHang_maVatLieu",
                table: "ChiTietPhieuNhapHang",
                column: "maVatLieu");

            migrationBuilder.CreateIndex(
                name: "IX_GiaDichVu_SanPham_NgayApDung",
                table: "GiaDichVu",
                columns: new[] { "maSanPham", "ngayApDung" });

            migrationBuilder.CreateIndex(
                name: "IX_GiaVatLieu_VatLieu_NgayApDung",
                table: "GiaVatLieu",
                columns: new[] { "maVatLieu", "ngayApDung" });

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_KhachHang",
                table: "HoaDonDichVu",
                column: "maKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_NgayTao",
                table: "HoaDonDichVu",
                column: "ngayTao");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_TrangThai",
                table: "HoaDonDichVu",
                column: "trangThai");

            migrationBuilder.CreateIndex(
                name: "IX_KhachHang_TaiKhoan",
                table: "KhachHang",
                column: "maTaiKhoan");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuSuDungPhong_maHoaDon",
                table: "LichSuSuDungPhong",
                column: "maHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuSuDungPhong_maKhachHang",
                table: "LichSuSuDungPhong",
                column: "maKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuSuDungPhong_maPhong",
                table: "LichSuSuDungPhong",
                column: "maPhong");

            migrationBuilder.CreateIndex(
                name: "IX_MaOTP_TaiKhoan",
                table: "MaOTP",
                column: "maTaiKhoan");

            migrationBuilder.CreateIndex(
                name: "IX_MonAn_maSanPham",
                table: "MonAn",
                column: "maSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_NhanVien_TaiKhoan",
                table: "NhanVien",
                column: "maTaiKhoan");

            migrationBuilder.CreateIndex(
                name: "UQ__NhanVien__AB6E61642A8F12D3",
                table: "NhanVien",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhieuLuong_maNhanVien",
                table: "PhieuLuong",
                column: "maNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuNhapHang_maNhaCungCap",
                table: "PhieuNhapHang",
                column: "maNhaCungCap");

            migrationBuilder.CreateIndex(
                name: "IX_PhongHatKaraoke_maLoaiPhong",
                table: "PhongHatKaraoke",
                column: "maLoaiPhong");

            migrationBuilder.CreateIndex(
                name: "IX_PhongHatKaraoke_maSanPham",
                table: "PhongHatKaraoke",
                column: "maSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_TaiKhoan",
                table: "RefreshToken",
                column: "maTaiKhoan");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "TaiKhoan",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "TaiKhoan",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ThuePhong_maKhachHang",
                table: "ThuePhong",
                column: "maKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_ThuePhong_Phong",
                table: "ThuePhong",
                column: "maPhong");

            migrationBuilder.CreateIndex(
                name: "IX_ThuePhong_TrangThai",
                table: "ThuePhong",
                column: "trangThai");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "VaiTro",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CaLamViec");

            migrationBuilder.DropTable(
                name: "ChiTietHoaDonDichVu");

            migrationBuilder.DropTable(
                name: "ChiTietPhieuHuyHang");

            migrationBuilder.DropTable(
                name: "ChiTietPhieuKiemHang");

            migrationBuilder.DropTable(
                name: "ChiTietPhieuNhapHang");

            migrationBuilder.DropTable(
                name: "GiaDichVu");

            migrationBuilder.DropTable(
                name: "GiaVatLieu");

            migrationBuilder.DropTable(
                name: "LichSuSuDungPhong");

            migrationBuilder.DropTable(
                name: "MaOTP");

            migrationBuilder.DropTable(
                name: "MonAn");

            migrationBuilder.DropTable(
                name: "PhieuLuong");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "ThuePhong");

            migrationBuilder.DropTable(
                name: "VaiTro");

            migrationBuilder.DropTable(
                name: "PhieuHuyHang");

            migrationBuilder.DropTable(
                name: "PhieuKiemHang");

            migrationBuilder.DropTable(
                name: "PhieuNhapHang");

            migrationBuilder.DropTable(
                name: "VatLieu");

            migrationBuilder.DropTable(
                name: "HoaDonDichVu");

            migrationBuilder.DropTable(
                name: "NhanVien");

            migrationBuilder.DropTable(
                name: "PhongHatKaraoke");

            migrationBuilder.DropTable(
                name: "NhaCungCap");

            migrationBuilder.DropTable(
                name: "KhachHang");

            migrationBuilder.DropTable(
                name: "loaiPhongHatKaraoke");

            migrationBuilder.DropTable(
                name: "SanPhamDichVu");

            migrationBuilder.DropTable(
                name: "TaiKhoan");
        }
    }
}
