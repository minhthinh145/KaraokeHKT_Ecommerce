using AutoMapper;
using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs;
using QLQuanKaraokeHKT.Core.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Core.DTOs.BookingDTOs;
using QLQuanKaraokeHKT.Core.DTOs.QLHeThongDTOs;
using QLQuanKaraokeHKT.Core.DTOs.QLKhoDTOs;
using QLQuanKaraokeHKT.Core.DTOs.QLNhanSuDTOs;
using QLQuanKaraokeHKT.Core.DTOs.QLPhongDTOs;
using QLQuanKaraokeHKT.Core.Entities;

namespace QLQuanKaraokeHKT.Application.Mappings
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            // TaiKhoan to UserProfileDTO mapping
            CreateMap<TaiKhoan, UserProfileDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src =>
                    // Ưu tiên lấy từ KhachHang.TenKhachHang, nếu không có thì lấy TaiKhoan.FullName, cuối cùng là UserName
                    src.KhachHangs.FirstOrDefault() != null && !string.IsNullOrEmpty(src.KhachHangs.FirstOrDefault().TenKhachHang)
                        ? src.KhachHangs.FirstOrDefault().TenKhachHang
                        : !string.IsNullOrEmpty(src.FullName)
                            ? src.FullName
                            : src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.LoaiTaiKhoan, opt => opt.MapFrom(src => src.loaiTaiKhoan))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src =>
                    src.KhachHangs.FirstOrDefault() != null && src.KhachHangs.FirstOrDefault().NgaySinh.HasValue
                        ? src.KhachHangs.FirstOrDefault().NgaySinh.Value.ToDateTime(TimeOnly.MinValue)
                        : (DateTime?)null));

            // KhachHang to UserProfileDTO mapping
            CreateMap<KhachHang, UserProfileDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src =>
                    // Ưu tiên TenKhachHang, nếu không có thì lấy FullName từ TaiKhoan, cuối cùng là UserName
                    !string.IsNullOrEmpty(src.TenKhachHang)
                        ? src.TenKhachHang
                        : !string.IsNullOrEmpty(src.MaTaiKhoanNavigation.FullName)
                            ? src.MaTaiKhoanNavigation.FullName
                            : src.MaTaiKhoanNavigation.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email ?? src.MaTaiKhoanNavigation.Email))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.MaTaiKhoanNavigation.PhoneNumber))
                .ForMember(dest => dest.LoaiTaiKhoan, opt => opt.MapFrom(src => src.MaTaiKhoanNavigation.loaiTaiKhoan))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src =>
                    src.NgaySinh.HasValue ? src.NgaySinh.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null));

            // SignUpDTO to TaiKhoan mapping
            CreateMap<SignUpDTO, TaiKhoan>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.daKichHoat, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.daBiKhoa, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.loaiTaiKhoan, opt => opt.MapFrom(src => ApplicationRole.KhacHang))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
                .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());

            // SignUpDTO to KhachHang mapping
            CreateMap<SignUpDTO, KhachHang>()
                .ForMember(dest => dest.TenKhachHang, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.NgaySinh, opt => opt.MapFrom(src =>
                    src.DateOfBirth.HasValue ? DateOnly.FromDateTime(src.DateOfBirth.Value) : (DateOnly?)null))
                .ForMember(dest => dest.MaKhachHang, opt => opt.Ignore())
                .ForMember(dest => dest.MaTaiKhoan, opt => opt.Ignore());

            // CreateUserOtpDTO to MaOtp mapping
            CreateMap<CreateUserOtpDTO, MaOtp>()
                .ForMember(dest => dest.IdOtp, opt => opt.Ignore()) // Auto-generated
                .ForMember(dest => dest.maOTP, opt => opt.MapFrom(src => src.maOTP))
                .ForMember(dest => dest.MaTaiKhoan, opt => opt.MapFrom(src => src.MaTaiKhoan))
                .ForMember(dest => dest.NgayTao, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.NgayHetHan, opt => opt.MapFrom(src => src.ExpirationTime))
                .ForMember(dest => dest.DaSuDung, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.MaTaiKhoanNavigation, opt => opt.Ignore());

            CreateMap<UserProfileDTO, TaiKhoan>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.loaiTaiKhoan, opt => opt.MapFrom(src => src.LoaiTaiKhoan))
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
                .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore())
                .ForMember(dest => dest.EmailConfirmed, opt => opt.Ignore())
                .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.Ignore())
                .ForMember(dest => dest.TwoFactorEnabled, opt => opt.Ignore())
                .ForMember(dest => dest.LockoutEnd, opt => opt.Ignore())
                .ForMember(dest => dest.LockoutEnabled, opt => opt.Ignore())
                .ForMember(dest => dest.AccessFailedCount, opt => opt.Ignore())
                .ForMember(dest => dest.NormalizedUserName, opt => opt.Ignore())
                .ForMember(dest => dest.NormalizedEmail, opt => opt.Ignore())
                .ForMember(dest => dest.daKichHoat, opt => opt.Ignore())
                .ForMember(dest => dest.daBiKhoa, opt => opt.Ignore())
                .ForMember(dest => dest.KhachHangs, opt => opt.Ignore())
                .ForMember(dest => dest.NhanViens, opt => opt.Ignore())
                .ForMember(dest => dest.MaOtps, opt => opt.Ignore())
                .ForMember(dest => dest.RefreshTokens, opt => opt.Ignore());

            CreateMap<UserProfileDTO, KhachHang>()
                .ForMember(dest => dest.TenKhachHang, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.NgaySinh, opt => opt.MapFrom(src =>
                    src.BirthDate.HasValue ? DateOnly.FromDateTime(src.BirthDate.Value) : (DateOnly?)null))
                .ForMember(dest => dest.MaKhachHang, opt => opt.Ignore())
                .ForMember(dest => dest.MaTaiKhoan, opt => opt.Ignore())
                .ForMember(dest => dest.MaTaiKhoanNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.HoaDonDichVus, opt => opt.Ignore())
                .ForMember(dest => dest.LichSuSuDungPhongs, opt => opt.Ignore())
                .ForMember(dest => dest.ThuePhongs, opt => opt.Ignore());

            // TokenResponseDTO mappings (if needed for complex scenarios)
            CreateMap<(string AccessToken, string RefreshToken), LoginResponseDTO>()
                .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.AccessToken))
                .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.RefreshToken));

            //KhachHangdto
            CreateMap<KhachHang, KhachHangDTO>().ReverseMap();
            //TaiKhoan -> KhachHang reverse
            CreateMap<TaiKhoan, KhachHang>()
                  .ForMember(dest => dest.TenKhachHang, opt => opt.MapFrom(src => src.FullName)).ReverseMap();

            CreateMap<NhanVien, NhanVienDTO>().ReverseMap();

            // AddNhanVienDTO mappings
            CreateMap<AddNhanVienDTO, TaiKhoan>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.HoTen))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.SoDienThoai))
                .ForMember(dest => dest.loaiTaiKhoan, opt => opt.MapFrom(src => src.LoaiTaiKhoan))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
                .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());

            // AddNhanVienDTO to NhanVien mapping (bỏ LoaiNhanVien mapping)
            CreateMap<AddNhanVienDTO, NhanVien>()
                .ForMember(dest => dest.HoTen, opt => opt.MapFrom(src => src.HoTen))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.NgaySinh, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.NgaySinh)))
                .ForMember(dest => dest.SoDienThoai, opt => opt.MapFrom(src => src.SoDienThoai))
                .ForMember(dest => dest.LoaiNhanVien, opt => opt.Ignore()) // Sẽ set thủ công sau
                .ForMember(dest => dest.MaNv, opt => opt.Ignore())
                .ForMember(dest => dest.MaTaiKhoan, opt => opt.Ignore());

            // NhanVienDTO to TaiKhoan mapping
            CreateMap<NhanVienDTO, TaiKhoan>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.HoTen))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.SoDienThoai))
                 .ForMember(dest => dest.loaiTaiKhoan, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
                .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());

            // NhanVien + TaiKhoan mappings
            CreateMap<NhanVien, NhanVienTaiKhoanDTO>()
                .ForMember(dest => dest.MaNv, opt => opt.MapFrom(src => src.MaNv))
                .ForMember(dest => dest.HoTen, opt => opt.MapFrom(src => src.HoTen))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.NgaySinh, opt => opt.MapFrom(src => src.NgaySinh))
                .ForMember(dest => dest.SoDienThoai, opt => opt.MapFrom(src => src.SoDienThoai))
                .ForMember(dest => dest.LoaiNhanVien, opt => opt.MapFrom(src => src.LoaiNhanVien))
                .ForMember(dest => dest.MaTaiKhoan, opt => opt.MapFrom(src => src.MaTaiKhoan))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.MaTaiKhoanNavigation.UserName))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.MaTaiKhoanNavigation.FullName))
                .ForMember(dest => dest.DaKichHoat, opt => opt.MapFrom(src => src.MaTaiKhoanNavigation.daKichHoat))
                .ForMember(dest => dest.DaBiKhoa, opt => opt.MapFrom(src => src.MaTaiKhoanNavigation.daBiKhoa))
                .ForMember(dest => dest.LoaiTaiKhoan, opt => opt.MapFrom(src => src.MaTaiKhoanNavigation.loaiTaiKhoan))
                .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => src.MaTaiKhoanNavigation.EmailConfirmed));

            // KhachHang + TaiKhoan mappings
            CreateMap<KhachHang, KhachHangTaiKhoanDTO>()
                .ForMember(dest => dest.MaKhachHang, opt => opt.MapFrom(src => src.MaKhachHang))
                .ForMember(dest => dest.TenKhachHang, opt => opt.MapFrom(src => src.TenKhachHang))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.NgaySinh, opt => opt.MapFrom(src => src.NgaySinh))
                .ForMember(dest => dest.MaTaiKhoan, opt => opt.MapFrom(src => src.MaTaiKhoan))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.MaTaiKhoanNavigation.UserName))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.MaTaiKhoanNavigation.FullName))
                .ForMember(dest => dest.DaKichHoat, opt => opt.MapFrom(src => src.MaTaiKhoanNavigation.daKichHoat))
                .ForMember(dest => dest.DaBiKhoa, opt => opt.MapFrom(src => src.MaTaiKhoanNavigation.daBiKhoa))
                .ForMember(dest => dest.LoaiTaiKhoan, opt => opt.MapFrom(src => src.MaTaiKhoanNavigation.loaiTaiKhoan))
                .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => src.MaTaiKhoanNavigation.EmailConfirmed));

            CreateMap<TaiKhoanQuanLyDTO, TaiKhoan>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.loaiTaiKhoan, opt => opt.MapFrom(src => src.loaiTaiKhoan))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.MaTaiKhoan))
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
                .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<AddAccountForAdminDTO, TaiKhoan>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.loaiTaiKhoan, opt => opt.MapFrom(src => src.loaiTaiKhoan))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
                .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore())
            .ReverseMap();


            CreateMap<UpdateAccountDTO, TaiKhoan>()
    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.newUserName))
    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.newUserName))
    .ForMember(dest => dest.loaiTaiKhoan, opt => opt.MapFrom(src => src.newLoaiTaiKhoan))
    .ForMember(dest => dest.Id, opt => opt.Ignore())
    .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<CaLamViec, CaLamViecDTO>().ReverseMap();
            CreateMap<CaLamViec, AddCaLamViecDTO>().ReverseMap();
            CreateMap<LuongCaLamViec, AddLuongCaLamViecDTO>().ReverseMap();
            CreateMap<LuongCaLamViecDTO, LuongCaLamViec>()
              .ForMember(dest => dest.CaLamViec, opt => opt.Ignore())
              .ForMember(dest => dest.MaCa, opt => opt.MapFrom(src => src.MaCa))
              .ForMember(dest => dest.GiaCa, opt => opt.MapFrom(src => src.GiaCa))
              .ForMember(dest => dest.IsDefault, opt => opt.MapFrom(src => src.IsDefault))
              .ForMember(dest => dest.NgayApDung, opt => opt.MapFrom(src => src.NgayApDung))
              .ForMember(dest => dest.NgayKetThuc, opt => opt.MapFrom(src => src.NgayKetThuc))
              .ForMember(dest => dest.MaLuongCaLamViec, opt => opt.MapFrom(src => src.MaLuongCaLamViec))
            .ReverseMap();
            CreateMap<AddLichLamViecDTO, LichLamViec>()
                .ForMember(dest => dest.MaLichLamViec, opt => opt.Ignore())
                .ForMember(dest => dest.NhanVien, opt => opt.Ignore())
                .ForMember(dest => dest.CaLamViec, opt => opt.Ignore())
                            .ReverseMap();


            CreateMap<LichLamViec, LichLamViecDTO>()
                .ForMember(dest => dest.TenNhanVien, opt => opt.MapFrom(src => src.NhanVien.HoTen))
                .ForMember(dest => dest.LoaiNhanVien, opt => opt.MapFrom(src => src.NhanVien.LoaiNhanVien))
            .ReverseMap();


            CreateMap<VatLieu, VatLieuDetailDTO>()
            .ForMember(dest => dest.MaSanPham, opt => opt.MapFrom(src => src.MonAn != null ? src.MonAn.MaSanPham : (int?)null))
            .ForMember(dest => dest.TenSanPham, opt => opt.MapFrom(src => src.MonAn != null && src.MonAn.MaSanPhamNavigation != null ? src.MonAn.MaSanPhamNavigation.TenSanPham : null))
            .ForMember(dest => dest.HinhAnhSanPham, opt => opt.MapFrom(src => src.MonAn != null && src.MonAn.MaSanPhamNavigation != null ? src.MonAn.MaSanPhamNavigation.HinhAnhSanPham : null))

            // Giá nhập (GiaVatLieu)
            .ForMember(dest => dest.GiaNhapHienTai, opt => opt.MapFrom(src =>
                src.GiaVatLieus.Where(g => g.TrangThai == "HieuLuc").Select(g => (decimal?)g.DonGia).FirstOrDefault()))
            .ForMember(dest => dest.NgayApDungGiaNhap, opt => opt.MapFrom(src =>
                src.GiaVatLieus.Where(g => g.TrangThai == "HieuLuc").Select(g => (DateOnly?)g.NgayApDung).FirstOrDefault()))
            .ForMember(dest => dest.TrangThaiGiaNhap, opt => opt.MapFrom(src =>
                src.GiaVatLieus.Where(g => g.TrangThai == "HieuLuc").Select(g => g.TrangThai).FirstOrDefault()))

            // Giá bán (GiaDichVu) - đồng giá/riêng biệt từng ca
            .ForMember(dest => dest.DongGiaAllCa, opt => opt.MapFrom(src =>
                src.MonAn != null && src.MonAn.MaSanPhamNavigation != null
                    ? src.MonAn.MaSanPhamNavigation.GiaDichVus.Any(g => g.TrangThai == "HieuLuc" && g.MaCa == null)
                        ? true  // Có giá chung (MaCa = null)
                        : src.MonAn.MaSanPhamNavigation.GiaDichVus.Any(g => g.TrangThai == "HieuLuc" && g.MaCa != null)
                            ? false  // Có giá riêng biệt (MaCa != null)
                            : null
                    : (bool?)null))
.ForMember(dest => dest.GiaBanChung, opt => opt.MapFrom(src =>
    src.MonAn != null && src.MonAn.MaSanPhamNavigation != null
        ? src.MonAn.MaSanPhamNavigation.GiaDichVus
            .Where(g => g.TrangThai == "HieuLuc" && g.MaCa == null)  // Đồng giá
            .Select(g => (decimal?)g.DonGia)
            .FirstOrDefault()
        : null))
      .ForMember(dest => dest.GiaBanCa1, opt => opt.MapFrom(src =>
    src.MonAn != null && src.MonAn.MaSanPhamNavigation != null
        ? src.MonAn.MaSanPhamNavigation.GiaDichVus
            .Where(g => g.TrangThai == "HieuLuc" && g.MaCaNavigation != null && g.MaCaNavigation.TenCa == "Ca 1")
            .Select(g => (decimal?)g.DonGia)
            .FirstOrDefault()
        : null))
.ForMember(dest => dest.GiaBanCa2, opt => opt.MapFrom(src =>
    src.MonAn != null && src.MonAn.MaSanPhamNavigation != null
        ? src.MonAn.MaSanPhamNavigation.GiaDichVus
            .Where(g => g.TrangThai == "HieuLuc" && g.MaCaNavigation != null && g.MaCaNavigation.TenCa == "Ca 2")
            .Select(g => (decimal?)g.DonGia)
            .FirstOrDefault()
        : null))
.ForMember(dest => dest.GiaBanCa3, opt => opt.MapFrom(src =>
    src.MonAn != null && src.MonAn.MaSanPhamNavigation != null
        ? src.MonAn.MaSanPhamNavigation.GiaDichVus
            .Where(g => g.TrangThai == "HieuLuc" && g.MaCaNavigation != null && g.MaCaNavigation.TenCa == "Ca 3")
            .Select(g => (decimal?)g.DonGia)
            .FirstOrDefault()
        : null))
            // Giá bán hiện tại (ưu tiên theo ca, fallback đồng giá)
            .ForMember(dest => dest.GiaBanHienTai, opt => opt.MapFrom(src =>
                src.MonAn != null && src.MonAn.MaSanPhamNavigation != null
                    ? src.MonAn.MaSanPhamNavigation.GiaDichVus
                        .Where(g => g.TrangThai == "HieuLuc")
                        .OrderBy(g => g.MaCa ?? 0)
                        .Select(g => (decimal?)g.DonGia)
                        .FirstOrDefault()
                    : null))
            .ForMember(dest => dest.NgayApDungGiaBan, opt => opt.MapFrom(src =>
                src.MonAn != null && src.MonAn.MaSanPhamNavigation != null
                    ? src.MonAn.MaSanPhamNavigation.GiaDichVus
                        .Where(g => g.TrangThai == "HieuLuc")
                        .OrderBy(g => g.MaCa ?? 0)
                        .Select(g => (DateOnly?)g.NgayApDung)
                        .FirstOrDefault()
                    : null))
            .ForMember(dest => dest.TrangThaiGiaBan, opt => opt.MapFrom(src =>
                src.MonAn != null && src.MonAn.MaSanPhamNavigation != null
                    ? src.MonAn.MaSanPhamNavigation.GiaDichVus
                        .Where(g => g.TrangThai == "HieuLuc")
                        .OrderBy(g => g.MaCa ?? 0)
                        .Select(g => g.TrangThai)
                        .FirstOrDefault()
                    : null))
            .ForMember(dest => dest.MaMonAn, opt => opt.MapFrom(src => src.MonAn != null ? src.MonAn.MaMonAn : (int?)null))
            .ForMember(dest => dest.SoLuongConLai, opt => opt.MapFrom(src => src.MonAn != null ? src.MonAn.SoLuongConLai : (int?)null));


            // Thêm vào constructor ApplicationMapper
            CreateMap<AddVatLieuDTO, SanPhamDichVu>()
                .ForMember(dest => dest.MaSanPham, opt => opt.Ignore())
                .ForMember(dest => dest.TenSanPham, opt => opt.MapFrom(src => src.TenSanPham))
                .ForMember(dest => dest.HinhAnhSanPham, opt => opt.MapFrom(src => src.HinhAnhSanPham))
                .ForMember(dest => dest.ChiTietHoaDonDichVus, opt => opt.Ignore())
                .ForMember(dest => dest.GiaDichVus, opt => opt.Ignore())
                .ForMember(dest => dest.MonAns, opt => opt.Ignore())
                .ForMember(dest => dest.PhongHatKaraokes, opt => opt.Ignore());

            CreateMap<AddVatLieuDTO, VatLieu>()
                .ForMember(dest => dest.MaVatLieu, opt => opt.Ignore())
                .ForMember(dest => dest.TenVatLieu, opt => opt.MapFrom(src => src.TenVatLieu))
                .ForMember(dest => dest.DonViTinh, opt => opt.MapFrom(src => src.DonViTinh))
                .ForMember(dest => dest.SoLuongTonKho, opt => opt.MapFrom(src => src.SoLuongTonKho))
                .ForMember(dest => dest.NgungCungCap, opt => opt.Ignore()) // Set thủ công
                .ForMember(dest => dest.MonAn, opt => opt.Ignore())
                .ForMember(dest => dest.ChiTietPhieuHuyHangs, opt => opt.Ignore())
                .ForMember(dest => dest.ChiTietPhieuKiemHangs, opt => opt.Ignore())
                .ForMember(dest => dest.ChiTietPhieuNhapHangs, opt => opt.Ignore())
                .ForMember(dest => dest.GiaVatLieus, opt => opt.Ignore());

            CreateMap<AddVatLieuDTO, GiaVatLieu>()
                .ForMember(dest => dest.MaGiaVatLieu, opt => opt.Ignore())
                .ForMember(dest => dest.MaVatLieu, opt => opt.Ignore()) // Set thủ công
                .ForMember(dest => dest.DonGia, opt => opt.MapFrom(src => src.GiaNhap))
                .ForMember(dest => dest.NgayApDung, opt => opt.MapFrom(src => src.NgayApDungGiaNhap))
                .ForMember(dest => dest.TrangThai, opt => opt.MapFrom(src => src.TrangThaiGiaNhap))
                .ForMember(dest => dest.MaVatLieuNavigation, opt => opt.Ignore());

            // Thêm mapping mới cho GiaDichVu - chỉ dùng cho mapping cơ bản, logic tạo theo ca sẽ handle trong service
            CreateMap<AddVatLieuDTO, GiaDichVu>()
                .ForMember(dest => dest.MaGiaDichVu, opt => opt.Ignore())
                .ForMember(dest => dest.MaSanPham, opt => opt.Ignore()) // Set thủ công trong service
                .ForMember(dest => dest.MaCa, opt => opt.Ignore()) // Set thủ công trong service
                .ForMember(dest => dest.DonGia, opt => opt.Ignore()) // Set thủ công trong service
                .ForMember(dest => dest.NgayApDung, opt => opt.MapFrom(src => src.NgayApDungGiaBan))
                .ForMember(dest => dest.TrangThai, opt => opt.MapFrom(src => src.TrangThaiGiaBan))
                .ForMember(dest => dest.MaSanPhamNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.MaCaNavigation, opt => opt.Ignore());
            // Mapping UpdateVatLieuDTO -> VatLieu
            CreateMap<UpdateVatLieuDTO, VatLieu>()
                .ForMember(dest => dest.MaVatLieu, opt => opt.MapFrom(src => src.MaVatLieu))
                .ForMember(dest => dest.TenVatLieu, opt => opt.MapFrom(src => src.TenVatLieu))
                .ForMember(dest => dest.DonViTinh, opt => opt.MapFrom(src => src.DonViTinh))
                .ForMember(dest => dest.SoLuongTonKho, opt => opt.Ignore())
                .ForMember(dest => dest.NgungCungCap, opt => opt.Ignore())
                .ForMember(dest => dest.MonAn, opt => opt.Ignore())
                .ForMember(dest => dest.ChiTietPhieuHuyHangs, opt => opt.Ignore())
                .ForMember(dest => dest.ChiTietPhieuKiemHangs, opt => opt.Ignore())
                .ForMember(dest => dest.ChiTietPhieuNhapHangs, opt => opt.Ignore())
                .ForMember(dest => dest.GiaVatLieus, opt => opt.Ignore());

            // Mapping UpdateVatLieuDTO -> SanPhamDichVu
            CreateMap<UpdateVatLieuDTO, SanPhamDichVu>()
                .ForMember(dest => dest.TenSanPham, opt => opt.MapFrom(src => src.TenSanPham))
                .ForMember(dest => dest.HinhAnhSanPham, opt => opt.MapFrom(src => src.HinhAnhSanPham))
                .ForMember(dest => dest.MaSanPham, opt => opt.Ignore())
                .ForMember(dest => dest.ChiTietHoaDonDichVus, opt => opt.Ignore())
                .ForMember(dest => dest.GiaDichVus, opt => opt.Ignore())
                .ForMember(dest => dest.MonAns, opt => opt.Ignore())
                .ForMember(dest => dest.PhongHatKaraokes, opt => opt.Ignore());

            // Mapping UpdateVatLieuDTO -> GiaVatLieu
            CreateMap<UpdateVatLieuDTO, GiaVatLieu>()
                .ForMember(dest => dest.MaGiaVatLieu, opt => opt.Ignore())
                .ForMember(dest => dest.MaVatLieu, opt => opt.Ignore()) // Set thủ công khi dùng
                .ForMember(dest => dest.DonGia, opt => opt.MapFrom(src => src.GiaNhapMoi ?? 0))
                .ForMember(dest => dest.NgayApDung, opt => opt.MapFrom(src => src.NgayApDungGiaNhap ?? DateOnly.FromDateTime(DateTime.Now)))
                .ForMember(dest => dest.TrangThai, opt => opt.MapFrom(src => src.TrangThaiGiaNhap ?? "HieuLuc"))
                .ForMember(dest => dest.MaVatLieuNavigation, opt => opt.Ignore());


            CreateMap<UpdateVatLieuDTO, GiaDichVu>()
                .ForMember(dest => dest.MaGiaDichVu, opt => opt.Ignore())
                .ForMember(dest => dest.MaSanPham, opt => opt.Ignore()) // Set thủ công trong service
                .ForMember(dest => dest.MaCa, opt => opt.Ignore()) // Set thủ công trong service
                .ForMember(dest => dest.DonGia, opt => opt.Ignore()) // Set thủ công trong service
                .ForMember(dest => dest.NgayApDung, opt => opt.MapFrom(src => src.NgayApDungGiaBan ?? DateOnly.FromDateTime(DateTime.Now)))
                .ForMember(dest => dest.TrangThai, opt => opt.MapFrom(src => src.TrangThaiGiaBan ?? "HieuLuc"))
                .ForMember(dest => dest.MaSanPhamNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.MaCaNavigation, opt => opt.Ignore());

            // Thêm vào constructor ApplicationMapper
            CreateMap<LoaiPhongHatKaraoke, LoaiPhongDTO>().ReverseMap();
            CreateMap<AddLoaiPhongDTO, LoaiPhongHatKaraoke>()
                .ForMember(dest => dest.MaLoaiPhong, opt => opt.Ignore())
                .ForMember(dest => dest.PhongHatKaraokes, opt => opt.Ignore());

            // Thêm vào constructor ApplicationMapper
            CreateMap<PhongHatKaraoke, PhongHatDetailDTO>()
                .ForMember(dest => dest.TenLoaiPhong, opt => opt.MapFrom(src => src.MaLoaiPhongNavigation.TenLoaiPhong))
                .ForMember(dest => dest.SucChua, opt => opt.MapFrom(src => src.MaLoaiPhongNavigation.SucChua))
                .ForMember(dest => dest.TenSanPham, opt => opt.MapFrom(src => src.MaSanPhamNavigation.TenSanPham))
                .ForMember(dest => dest.HinhAnhSanPham, opt => opt.MapFrom(src => src.MaSanPhamNavigation.HinhAnhSanPham))
                .ForMember(dest => dest.DongGiaAllCa, opt => opt.MapFrom(src =>
                    src.MaSanPhamNavigation != null
                        ? src.MaSanPhamNavigation.GiaDichVus.Any(g => g.TrangThai == "HieuLuc" && g.MaCa == null)
                            ? true  // Có giá chung (MaCa = null)
                            : src.MaSanPhamNavigation.GiaDichVus.Any(g => g.TrangThai == "HieuLuc" && g.MaCa != null)
                                ? false  // Có giá riêng biệt (MaCa != null)
                                : null
                        : (bool?)null))
                .ForMember(dest => dest.GiaThueChung, opt => opt.MapFrom(src =>
                    src.MaSanPhamNavigation != null
                        ? src.MaSanPhamNavigation.GiaDichVus
                            .Where(g => g.TrangThai == "HieuLuc" && g.MaCa == null)
                            .Select(g => (decimal?)g.DonGia)
                            .FirstOrDefault()
                        : null))
                .ForMember(dest => dest.GiaThueCa1, opt => opt.MapFrom(src =>
                    src.MaSanPhamNavigation != null
                        ? src.MaSanPhamNavigation.GiaDichVus
                            .Where(g => g.TrangThai == "HieuLuc" && g.MaCaNavigation != null && g.MaCaNavigation.TenCa == "Ca 1")
                            .Select(g => (decimal?)g.DonGia)
                            .FirstOrDefault()
                        : null))
                .ForMember(dest => dest.GiaThueCa2, opt => opt.MapFrom(src =>
                    src.MaSanPhamNavigation != null
                        ? src.MaSanPhamNavigation.GiaDichVus
                            .Where(g => g.TrangThai == "HieuLuc" && g.MaCaNavigation != null && g.MaCaNavigation.TenCa == "Ca 2")
                            .Select(g => (decimal?)g.DonGia)
                            .FirstOrDefault()
                        : null))
                .ForMember(dest => dest.GiaThueCa3, opt => opt.MapFrom(src =>
                    src.MaSanPhamNavigation != null
                        ? src.MaSanPhamNavigation.GiaDichVus
                            .Where(g => g.TrangThai == "HieuLuc" && g.MaCaNavigation != null && g.MaCaNavigation.TenCa == "Ca 3")
                            .Select(g => (decimal?)g.DonGia)
                            .FirstOrDefault()
                        : null))
                .ForMember(dest => dest.GiaThueHienTai, opt => opt.MapFrom(src =>
                    src.MaSanPhamNavigation != null
                        ? src.MaSanPhamNavigation.GiaDichVus
                            .Where(g => g.TrangThai == "HieuLuc")
                            .OrderBy(g => g.MaCa ?? 0)
                            .Select(g => (decimal?)g.DonGia)
                            .FirstOrDefault()
                        : null))
                .ForMember(dest => dest.NgayApDungGia, opt => opt.MapFrom(src =>
                    src.MaSanPhamNavigation != null
                        ? src.MaSanPhamNavigation.GiaDichVus
                            .Where(g => g.TrangThai == "HieuLuc")
                            .OrderBy(g => g.MaCa ?? 0)
                            .Select(g => (DateOnly?)g.NgayApDung)
                            .FirstOrDefault()
                        : null))
                .ForMember(dest => dest.TrangThaiGia, opt => opt.MapFrom(src =>
                    src.MaSanPhamNavigation != null
                        ? src.MaSanPhamNavigation.GiaDichVus
                            .Where(g => g.TrangThai == "HieuLuc")
                            .OrderBy(g => g.MaCa ?? 0)
                            .Select(g => g.TrangThai)
                            .FirstOrDefault()
                        : null));

            CreateMap<AddPhongHatDTO, SanPhamDichVu>()
                .ForMember(dest => dest.MaSanPham, opt => opt.Ignore())
                .ForMember(dest => dest.TenSanPham, opt => opt.MapFrom(src => src.TenPhong))
                .ForMember(dest => dest.HinhAnhSanPham, opt => opt.MapFrom(src => src.HinhAnhPhong))
                .ForMember(dest => dest.ChiTietHoaDonDichVus, opt => opt.Ignore())
                .ForMember(dest => dest.GiaDichVus, opt => opt.Ignore())
                .ForMember(dest => dest.MonAns, opt => opt.Ignore())
                .ForMember(dest => dest.PhongHatKaraokes, opt => opt.Ignore());

            CreateMap<AddPhongHatDTO, PhongHatKaraoke>()
                .ForMember(dest => dest.MaPhong, opt => opt.Ignore())
                .ForMember(dest => dest.MaSanPham, opt => opt.Ignore()) // Set thủ công
                .ForMember(dest => dest.DangSuDung, opt => opt.Ignore()) // Set thủ công
                .ForMember(dest => dest.NgungHoatDong, opt => opt.Ignore()) // Set thủ công
                .ForMember(dest => dest.MaLoaiPhongNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.MaSanPhamNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.LichSuSuDungPhongs, opt => opt.Ignore())
                .ForMember(dest => dest.ThuePhongs, opt => opt.Ignore());

            CreateMap<AddPhongHatDTO, GiaDichVu>()
                .ForMember(dest => dest.MaGiaDichVu, opt => opt.Ignore())
                .ForMember(dest => dest.MaSanPham, opt => opt.Ignore()) // Set thủ công
                .ForMember(dest => dest.MaCa, opt => opt.Ignore()) // Set thủ công  
                .ForMember(dest => dest.DonGia, opt => opt.Ignore()) // Set thủ công
                .ForMember(dest => dest.NgayApDung, opt => opt.MapFrom(src => src.NgayApDungGia))
                .ForMember(dest => dest.TrangThai, opt => opt.MapFrom(src => src.TrangThaiGia))
                .ForMember(dest => dest.MaSanPhamNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.MaCaNavigation, opt => opt.Ignore());

            CreateMap<UpdatePhongHatDTO, SanPhamDichVu>()
                .ForMember(dest => dest.TenSanPham, opt => opt.MapFrom(src => src.TenPhong))
                .ForMember(dest => dest.HinhAnhSanPham, opt => opt.MapFrom(src => src.HinhAnhPhong))
                .ForMember(dest => dest.MaSanPham, opt => opt.Ignore())
                .ForMember(dest => dest.ChiTietHoaDonDichVus, opt => opt.Ignore())
                .ForMember(dest => dest.GiaDichVus, opt => opt.Ignore())
                .ForMember(dest => dest.MonAns, opt => opt.Ignore())
                .ForMember(dest => dest.PhongHatKaraokes, opt => opt.Ignore());

            CreateMap<UpdatePhongHatDTO, GiaDichVu>()
                .ForMember(dest => dest.MaGiaDichVu, opt => opt.Ignore())
                .ForMember(dest => dest.MaSanPham, opt => opt.Ignore()) // Set thủ công
                .ForMember(dest => dest.MaCa, opt => opt.Ignore()) // Set thủ công
                .ForMember(dest => dest.DonGia, opt => opt.Ignore()) // Set thủ công
                .ForMember(dest => dest.NgayApDung, opt => opt.MapFrom(src => src.NgayApDungGia ?? DateOnly.FromDateTime(DateTime.Now)))
                .ForMember(dest => dest.TrangThai, opt => opt.MapFrom(src => src.TrangThaiGia ?? "HieuLuc"))
                .ForMember(dest => dest.MaSanPhamNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.MaCaNavigation, opt => opt.Ignore());

            CreateMap<UpdatePhongHatDTO, PhongHatKaraoke>()
    .ForMember(dest => dest.MaPhong, opt => opt.Ignore())
    .ForMember(dest => dest.MaSanPham, opt => opt.Ignore())
    .ForMember(dest => dest.DangSuDung, opt => opt.Ignore())
    .ForMember(dest => dest.NgungHoatDong, opt => opt.Ignore())
    // ✅ Map MaLoaiPhong nếu có giá trị
    .ForMember(dest => dest.MaLoaiPhong, opt => opt.MapFrom(src => src.MaLoaiPhong ?? 0))
    .ForMember(dest => dest.MaLoaiPhongNavigation, opt => opt.Ignore())
    .ForMember(dest => dest.MaSanPhamNavigation, opt => opt.Ignore())
    .ForMember(dest => dest.LichSuSuDungPhongs, opt => opt.Ignore())
    .ForMember(dest => dest.ThuePhongs, opt => opt.Ignore());

            // Thêm vào constructor ApplicationMapper

            CreateMap<AddYeuCauChuyenCaDTO, YeuCauChuyenCa>()
                .ForMember(dest => dest.MaYeuCau, opt => opt.Ignore())
                .ForMember(dest => dest.DaPheDuyet, opt => opt.Ignore())
                .ForMember(dest => dest.KetQuaPheDuyet, opt => opt.Ignore())
                .ForMember(dest => dest.GhiChuPheDuyet, opt => opt.Ignore())
                .ForMember(dest => dest.NgayTaoYeuCau, opt => opt.Ignore())
                .ForMember(dest => dest.NgayPheDuyet, opt => opt.Ignore())
                .ForMember(dest => dest.LichLamViecGoc, opt => opt.Ignore())
                .ForMember(dest => dest.CaMoi, opt => opt.Ignore());

            CreateMap<YeuCauChuyenCa, YeuCauChuyenCaDTO>()
                .ForMember(dest => dest.TenNhanVien, opt => opt.MapFrom(src => src.LichLamViecGoc.NhanVien.HoTen))
                .ForMember(dest => dest.NgayLamViecGoc, opt => opt.MapFrom(src => src.LichLamViecGoc.NgayLamViec))
                .ForMember(dest => dest.TenCaGoc, opt => opt.MapFrom(src => src.LichLamViecGoc.CaLamViec.TenCa))
                .ForMember(dest => dest.TenCaMoi, opt => opt.MapFrom(src => src.CaMoi.TenCa));

            CreateMap<PhongHatKaraoke, PhongHatForCustomerDTO>()
          .ForMember(dest => dest.TenPhong, opt => opt.MapFrom(src => src.MaSanPhamNavigation.TenSanPham))
          .ForMember(dest => dest.TenLoaiPhong, opt => opt.MapFrom(src => src.MaLoaiPhongNavigation.TenLoaiPhong))
          .ForMember(dest => dest.SucChua, opt => opt.MapFrom(src => src.MaLoaiPhongNavigation.SucChua))
          .ForMember(dest => dest.HinhAnhPhong, opt => opt.MapFrom(src => src.MaSanPhamNavigation.HinhAnhSanPham))
        .ForMember(dest => dest.GiaThueHienTai, opt => opt.Ignore()) // Set thủ công trong service
          .ForMember(dest => dest.Available, opt => opt.MapFrom(src => !src.DangSuDung && !src.NgungHoatDong));

            // DatPhongDTO -> ThuePhong
            CreateMap<DatPhongDTO, ThuePhong>()
                .ForMember(dest => dest.MaThuePhong, opt => opt.Ignore()) // Generated
                .ForMember(dest => dest.ThoiGianKetThuc, opt => opt.Ignore()) // Calculated in service
                .ForMember(dest => dest.TrangThai, opt => opt.Ignore()) // Set in service
                .ForMember(dest => dest.MaKhachHangNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.MaPhongNavigation, opt => opt.Ignore());

            // ThuePhong -> LichSuDatPhongDTO
            CreateMap<ThuePhong, LichSuDatPhongDTO>()
                .ForMember(dest => dest.TenPhong, opt => opt.MapFrom(src => src.MaPhongNavigation.MaSanPhamNavigation.TenSanPham))
                .ForMember(dest => dest.NgayTao, opt => opt.MapFrom(src => src.ThoiGianBatDau))
                .ForMember(dest => dest.NgayThanhToan, opt => opt.Ignore()) // Can be set from HoaDonDichVu if needed
                .ForMember(dest => dest.TongTien, opt => opt.Ignore()); // Calculate from related data

            // ThuePhong -> DatPhongResponseDTO
            CreateMap<ThuePhong, DatPhongResponseDTO>()
                .ForMember(dest => dest.TenPhong, opt => opt.MapFrom(src => src.MaPhongNavigation.MaSanPhamNavigation.TenSanPham))
                .ForMember(dest => dest.ThoiGianKetThucDuKien, opt => opt.MapFrom(src => src.ThoiGianKetThuc))
                .ForMember(dest => dest.NgayTao, opt => opt.MapFrom(src => src.ThoiGianBatDau))
                .ForMember(dest => dest.MaHoaDon, opt => opt.Ignore()) // Set manually in service
                .ForMember(dest => dest.TongTien, opt => opt.Ignore()) // Calculate in service
                .ForMember(dest => dest.HanThanhToan, opt => opt.Ignore()) // Set in service
                .ForMember(dest => dest.UrlThanhToan, opt => opt.Ignore()); // Set in service

            // VNPay mappings

            // KhachHang specific mappings for booking
            CreateMap<KhachHang, string>()
                .ConvertUsing(src => src.TenKhachHang); // For customer name in VNPay

            CreateMap<KhachHang, string>()
                .ConvertUsing(src => src.Email); // For customer email in VNPay

            // Reverse mappings
            CreateMap<PhongHatForCustomerDTO, PhongHatKaraoke>()
                .ForMember(dest => dest.MaSanPhamNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.MaLoaiPhongNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.DangSuDung, opt => opt.MapFrom(src => !src.Available))
                .ForMember(dest => dest.NgungHoatDong, opt => opt.Ignore())
                .ForMember(dest => dest.LichSuSuDungPhongs, opt => opt.Ignore())
                .ForMember(dest => dest.ThuePhongs, opt => opt.Ignore());

            // HoaDonDichVu mapping for booking
            CreateMap<HoaDonDichVu, decimal>()
                .ConvertUsing(src => src.TongTienHoaDon);



            // TaiKhoan to UserProfileDTO mapping
            CreateMap<TaiKhoan, UserProfileDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src =>
                    // Ưu tiên lấy từ KhachHang.TenKhachHang, nếu không có thì lấy TaiKhoan.FullName, cuối cùng là UserName
                    src.KhachHangs.FirstOrDefault() != null && !string.IsNullOrEmpty(src.KhachHangs.FirstOrDefault().TenKhachHang)
                        ? src.KhachHangs.FirstOrDefault().TenKhachHang
                        : !string.IsNullOrEmpty(src.FullName)
                            ? src.FullName
                            : src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.LoaiTaiKhoan, opt => opt.MapFrom(src => src.loaiTaiKhoan))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src =>
                    src.KhachHangs.FirstOrDefault() != null && src.KhachHangs.FirstOrDefault().NgaySinh.HasValue
                        ? src.KhachHangs.FirstOrDefault().NgaySinh.Value.ToDateTime(TimeOnly.MinValue)
                        : (DateTime?)null));

            CreateMap<LichSuSuDungPhong, LichSuDatPhongDTO>()
            .ForMember(dest => dest.TenPhong, opt => opt.MapFrom(src => src.MaPhongNavigation.MaSanPhamNavigation.TenSanPham))
            .ForMember(dest => dest.TongTien, opt => opt.MapFrom(src => src.MaHoaDonNavigation.TongTienHoaDon))
            .ForMember(dest => dest.NgayThanhToan, opt => opt.MapFrom(src => src.MaHoaDonNavigation.NgayTao))
            .ForMember(dest => dest.TrangThai, opt => opt.MapFrom(src => src.MaThuePhongNavigation.TrangThai))
            .ForMember(dest => dest.MaThuePhong, opt => opt.MapFrom(src => src.MaThuePhong)) // Assuming MaThuePhong is available in LichSuSuDungPhong
            .ForMember(dest => dest.NgayTao, opt => opt.MapFrom(src => src.ThoiGianBatDau))
            .ReverseMap();

            // ✅ THÊM MAPPING TỪ ThuePhong SANG LichSuDatPhongDTO
            CreateMap<ThuePhong, LichSuDatPhongDTO>()
                .ForMember(dest => dest.TenPhong, opt => opt.MapFrom(src =>
                    src.MaPhongNavigation.MaSanPhamNavigation.TenSanPham))
                .ForMember(dest => dest.NgayTao, opt => opt.MapFrom(src => src.ThoiGianBatDau))
                .ForMember(dest => dest.TongTien, opt => opt.Ignore()) // Sẽ set trong service
                .ForMember(dest => dest.NgayThanhToan, opt => opt.Ignore()) // Sẽ set trong service
                .ForMember(dest => dest.TrangThai, opt => opt.Ignore()); // Sẽ set trong service
        }
    }
}

