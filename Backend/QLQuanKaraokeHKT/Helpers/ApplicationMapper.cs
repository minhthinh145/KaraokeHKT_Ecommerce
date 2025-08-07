using AutoMapper;
using QLQuanKaraokeHKT.DTOs;
using QLQuanKaraokeHKT.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.DTOs.QLHeThongDTOs;
using QLQuanKaraokeHKT.DTOs.QLNhanSuDTOs;
using QLQuanKaraokeHKT.Models;

namespace QLQuanKaraokeHKT.Helpers
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
            CreateMap<(string AccessToken, string RefreshToken), TokenResponseDTO>()
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

            CreateMap<TaiKhoanQuanLyDTO , TaiKhoan>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.loaiTaiKhoan, opt => opt.MapFrom(src => src.loaiTaiKhoan))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.MaTaiKhoan))
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
                .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}