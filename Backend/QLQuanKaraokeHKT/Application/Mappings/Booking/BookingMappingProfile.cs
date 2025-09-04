using AutoMapper;
using QLQuanKaraokeHKT.Core.DTOs.BookingDTOs;
using QLQuanKaraokeHKT.Core.Entities;

namespace QLQuanKaraokeHKT.Application.Mappings.Booking
{
    public class BookingMappingProfile : Profile
    {
        public BookingMappingProfile()
        {
            CreateBookingMappings();
            CreateBookingHistoryMappings();
            CreateVNPayMappings();
        }

        private void CreateBookingMappings()
        {
            CreateMap<DatPhongDTO, ThuePhong>();

            CreateMap<ThuePhong, DatPhongResponseDTO>()
                .ForMember(dest => dest.TenPhong, opt => opt.MapFrom(src => src.MaPhongNavigation.MaSanPhamNavigation.TenSanPham))
                .ForMember(dest => dest.ThoiGianKetThucDuKien, opt => opt.MapFrom(src => src.ThoiGianKetThuc))
                .ForMember(dest => dest.NgayTao, opt => opt.MapFrom(src => src.ThoiGianBatDau))
                .ForMember(dest => dest.MaHoaDon, opt => opt.Ignore())
                .ForMember(dest => dest.TongTien, opt => opt.Ignore())
                .ForMember(dest => dest.HanThanhToan, opt => opt.Ignore())
                .ForMember(dest => dest.UrlThanhToan, opt => opt.Ignore());
        }

        private void CreateBookingHistoryMappings()
        {
            CreateMap<ThuePhong, LichSuDatPhongDTO>()
                .ForMember(dest => dest.TenPhong, opt => opt.MapFrom(src => src.MaPhongNavigation.MaSanPhamNavigation.TenSanPham))
                .ForMember(dest => dest.NgayTao, opt => opt.MapFrom(src => src.ThoiGianBatDau))
                .ForMember(dest => dest.TongTien, opt => opt.Ignore())
                .ForMember(dest => dest.NgayThanhToan, opt => opt.Ignore())
                .ForMember(dest => dest.TrangThai, opt => opt.Ignore());

            CreateMap<LichSuSuDungPhong, LichSuDatPhongDTO>()
                .ForMember(dest => dest.TenPhong, opt => opt.MapFrom(src => src.MaPhongNavigation.MaSanPhamNavigation.TenSanPham))
                .ForMember(dest => dest.TongTien, opt => opt.MapFrom(src => src.MaHoaDonNavigation!.TongTienHoaDon))
                .ForMember(dest => dest.NgayThanhToan, opt => opt.MapFrom(src => src.MaHoaDonNavigation!.NgayTao))
                .ForMember(dest => dest.TrangThai, opt => opt.MapFrom(src => src.MaThuePhongNavigation!.TrangThai))
                .ForMember(dest => dest.NgayTao, opt => opt.MapFrom(src => src.ThoiGianBatDau))
                .ReverseMap();
        }

        private void CreateVNPayMappings()
        {
            CreateMap<KhachHang, string>()
                .ConvertUsing(src => src.TenKhachHang);

            CreateMap<HoaDonDichVu, decimal>()
                .ConvertUsing(src => src.TongTienHoaDon);
        }
    }
}