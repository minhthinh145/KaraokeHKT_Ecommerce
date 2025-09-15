using AutoMapper;
using QLQuanKaraokeHKT.Core.DTOs.QLNhanSuDTOs;
using QLQuanKaraokeHKT.Core.Entities;

namespace QLQuanKaraokeHKT.Application.Mappings.HRM
{
    public class HRMMappingProfile : Profile
    {
        public HRMMappingProfile()
        {
            CreateCaLamViecMappings();
            CreateLichLamViecMappings();
            CreateLuongMappings();
            CreateYeuCauChuyenCaMappings();
        }

        private void CreateCaLamViecMappings()
        {
            CreateMap<CaLamViec, CaLamViecDTO>().ReverseMap();
            CreateMap<CaLamViec, AddCaLamViecDTO>().ReverseMap();
        }

        private void CreateLichLamViecMappings()
        {
            CreateMap<AddLichLamViecDTO, LichLamViec>();


            CreateMap<LichLamViec, LichLamViecDTO>()
                .ForMember(dest => dest.TenNhanVien, opt => opt.MapFrom(src => src.NhanVien.HoTen))
                .ForMember(dest => dest.LoaiNhanVien, opt => opt.MapFrom(src => src.NhanVien.LoaiNhanVien))
                .ReverseMap();
    
        }

        private void CreateLuongMappings()
        {
            CreateMap<LuongCaLamViec, AddLuongCaLamViecDTO>().ReverseMap();

            CreateMap<LuongCaLamViecDTO, LuongCaLamViec>()
                .ReverseMap();
        }

        private void CreateYeuCauChuyenCaMappings()
        {
            CreateMap<AddYeuCauChuyenCaDTO, YeuCauChuyenCa>();

            CreateMap<YeuCauChuyenCa, YeuCauChuyenCaDTO>()
                .ForMember(dest => dest.TenNhanVien, opt => opt.MapFrom(src => src.LichLamViecGoc.NhanVien.HoTen))
                .ForMember(dest => dest.NgayLamViecGoc, opt => opt.MapFrom(src => src.LichLamViecGoc.NgayLamViec))
                .ForMember(dest => dest.TenCaGoc, opt => opt.MapFrom(src => src.LichLamViecGoc.CaLamViec.TenCa))
                .ForMember(dest => dest.TenCaMoi, opt => opt.MapFrom(src => src.CaMoi.TenCa));
        }
    }
}