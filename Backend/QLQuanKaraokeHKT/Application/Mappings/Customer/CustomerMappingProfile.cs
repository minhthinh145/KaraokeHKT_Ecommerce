using AutoMapper;
using QLQuanKaraokeHKT.Core.DTOs;
using QLQuanKaraokeHKT.Core.DTOs.QLHeThongDTOs;
using QLQuanKaraokeHKT.Core.DTOs.QLNhanSuDTOs;
using QLQuanKaraokeHKT.Core.Entities;

namespace QLQuanKaraokeHKT.Application.Mappings.Customer
{
    public class CustomerMappingProfile : Profile
    {
        public CustomerMappingProfile()
        {
            CreateCustomerMappings();
            CreateEmployeeMappings();
        }

        private void CreateCustomerMappings()
        {
            CreateMap<KhachHang, KhachHangDTO>().ReverseMap();
            
            CreateMap<TaiKhoan, KhachHang>()
                .ForMember(dest => dest.TenKhachHang, opt => opt.MapFrom(src => src.FullName))
                .ReverseMap();

            CreateMap<KhachHang, KhachHangTaiKhoanDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.MaTaiKhoanNavigation.UserName))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.MaTaiKhoanNavigation.FullName))
                .ForMember(dest => dest.DaKichHoat, opt => opt.MapFrom(src => src.MaTaiKhoanNavigation.daKichHoat))
                .ForMember(dest => dest.DaBiKhoa, opt => opt.MapFrom(src => src.MaTaiKhoanNavigation.daBiKhoa))
                .ForMember(dest => dest.LoaiTaiKhoan, opt => opt.MapFrom(src => src.MaTaiKhoanNavigation.loaiTaiKhoan))
                .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => src.MaTaiKhoanNavigation.EmailConfirmed));
        }

        private void CreateEmployeeMappings()
        {
            CreateMap<NhanVien, NhanVienDTO>().ReverseMap();

            CreateMap<AddNhanVienDTO, TaiKhoan>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.HoTen))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.SoDienThoai));


            CreateMap<AddNhanVienDTO, NhanVien>()
                .ForMember(dest => dest.NgaySinh, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.NgaySinh)));
 

            CreateMap<NhanVienDTO, TaiKhoan>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.HoTen))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.SoDienThoai));


            CreateMap<NhanVien, NhanVienTaiKhoanDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.MaTaiKhoanNavigation.UserName))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.MaTaiKhoanNavigation.FullName))
                .ForMember(dest => dest.DaKichHoat, opt => opt.MapFrom(src => src.MaTaiKhoanNavigation.daKichHoat))
                .ForMember(dest => dest.DaBiKhoa, opt => opt.MapFrom(src => src.MaTaiKhoanNavigation.daBiKhoa))
                .ForMember(dest => dest.LoaiTaiKhoan, opt => opt.MapFrom(src => src.MaTaiKhoanNavigation.loaiTaiKhoan))
                .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => src.MaTaiKhoanNavigation.EmailConfirmed));
        }
    }
}