using AutoMapper;
using QLQuanKaraokeHKT.Application.DTOs.QLPhongDTOs;
using QLQuanKaraokeHKT.Application.DTOs.BookingDTOs;
using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Domain.Entities.Views;

namespace QLQuanKaraokeHKT.Application.Mappings.Room
{
    public class RoomQueryMappingProfile : Profile
    {
        public RoomQueryMappingProfile()
        {
            CreateRoomDetailMappings();
            CreateCustomerViewMappings();
        }

        private void CreateRoomDetailMappings()
        {
            CreateMap<PhongHatKaraoke, PhongHatDetailDTO>()
                .ForMember(dest => dest.TenLoaiPhong, opt => opt.MapFrom(src => src.MaLoaiPhongNavigation.TenLoaiPhong))
                .ForMember(dest => dest.SucChua, opt => opt.MapFrom(src => src.MaLoaiPhongNavigation.SucChua))
                .ForMember(dest => dest.TenSanPham, opt => opt.MapFrom(src => src.MaSanPhamNavigation.TenSanPham))
                .ForMember(dest => dest.HinhAnhSanPham, opt => opt.MapFrom(src => src.MaSanPhamNavigation.HinhAnhSanPham))

                .ForMember(dest => dest.DongGiaAllCa, opt => opt.MapFrom(src =>
                    src.MaSanPhamNavigation.GiaDichVus.Any(g => g.MaCa == null)))

                .ForMember(dest => dest.GiaThueChung, opt => opt.MapFrom(src =>
                    src.MaSanPhamNavigation.GiaDichVus
                        .FirstOrDefault(g => g.MaCa == null)
                        .DonGia))

                .ForMember(dest => dest.GiaThueCa1, opt => opt.MapFrom(src =>
                    src.MaSanPhamNavigation.GiaDichVus
                        .FirstOrDefault(g => g.MaCaNavigation != null && g.MaCaNavigation.TenCa == "Ca 1")
                        .DonGia))

                .ForMember(dest => dest.GiaThueCa2, opt => opt.MapFrom(src =>
                    src.MaSanPhamNavigation.GiaDichVus
                        .FirstOrDefault(g => g.MaCaNavigation != null && g.MaCaNavigation.TenCa == "Ca 2")
                        .DonGia))

                .ForMember(dest => dest.GiaThueCa3, opt => opt.MapFrom(src =>
                    src.MaSanPhamNavigation.GiaDichVus
                        .FirstOrDefault(g => g.MaCaNavigation != null && g.MaCaNavigation.TenCa == "Ca 3")
                        .DonGia))

                .ForMember(dest => dest.GiaThueHienTai, opt => opt.MapFrom(src =>
                    GetCurrentPriceSimple(src.MaSanPhamNavigation.GiaDichVus)))

                .ForMember(dest => dest.NgayApDungGia, opt => opt.MapFrom(src =>
                    src.MaSanPhamNavigation.GiaDichVus
                        .OrderByDescending(g => g.NgayApDung)
                        .FirstOrDefault()
                        .NgayApDung))

                .ForMember(dest => dest.TrangThaiGia, opt => opt.MapFrom(src => "HieuLuc"));

            CreateMap<PhongHatForCustomerView, PhongHatForCustomerDTO>().ReverseMap();
        }

        private void CreateCustomerViewMappings()
        {
            CreateMap<PhongHatKaraoke, PhongHatForCustomerDTO>()
                .ForMember(dest => dest.TenPhong, opt => opt.MapFrom(src => src.MaSanPhamNavigation.TenSanPham))
                .ForMember(dest => dest.TenLoaiPhong, opt => opt.MapFrom(src => src.MaLoaiPhongNavigation.TenLoaiPhong))
                .ForMember(dest => dest.SucChua, opt => opt.MapFrom(src => src.MaLoaiPhongNavigation.SucChua))
                .ForMember(dest => dest.HinhAnhPhong, opt => opt.MapFrom(src => src.MaSanPhamNavigation.HinhAnhSanPham))
                .ForMember(dest => dest.GiaThueHienTai, opt => opt.MapFrom(src =>
                    GetCurrentPriceSimple(src.MaSanPhamNavigation.GiaDichVus)));
        }

        private static decimal? GetCurrentPriceSimple(ICollection<GiaDichVu> giaDichVus)
        {
            if (!giaDichVus?.Any() == true) return null;

            var flatRate = giaDichVus.FirstOrDefault(g => g.MaCa == null);
            if (flatRate != null) return flatRate.DonGia;

            var currentTime = TimeOnly.FromDateTime(DateTime.Now);

            var currentShift = giaDichVus.FirstOrDefault(g =>
                g.MaCaNavigation != null &&
                IsCurrentShift(currentTime, g.MaCaNavigation));

            if (currentShift != null) return currentShift.DonGia;

            return giaDichVus.FirstOrDefault()?.DonGia;
        }

        private static bool IsCurrentShift(TimeOnly currentTime, CaLamViec caLamViec)
        {
            var startTime = caLamViec.GioBatDauCa;
            var endTime = caLamViec.GioKetThucCa;

            if (startTime <= endTime)
            {
                return currentTime >= startTime && currentTime < endTime;
            }

            else
            {
                return currentTime >= startTime || currentTime < endTime;
            }
        }
    }
}