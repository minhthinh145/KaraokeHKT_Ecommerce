using AutoMapper;
using QLQuanKaraokeHKT.Application.DTOs.QLPhongDTOs;
using QLQuanKaraokeHKT.Domain.Entities;

namespace QLQuanKaraokeHKT.Application.Mappings.Room
{
    public class RoomMappingProfile : Profile
    {
        public RoomMappingProfile()
        {
            CreateRoomTypeMappings();
            CreateRoomMappings();
            CreateRoomPricingMappings();
        }

        private void CreateRoomTypeMappings()
        {
            CreateMap<LoaiPhongHatKaraoke, LoaiPhongDTO>().ReverseMap();
            CreateMap<AddLoaiPhongDTO, LoaiPhongHatKaraoke>();

        }

        private void CreateRoomMappings()
        {
            CreateMap<AddPhongHatDTO, SanPhamDichVu>()
                .ForMember(dest => dest.TenSanPham, opt => opt.MapFrom(src => src.TenPhong))
                .ForMember(dest => dest.HinhAnhSanPham, opt => opt.MapFrom(src => src.HinhAnhPhong));

            CreateMap<AddPhongHatDTO, PhongHatKaraoke>();


            CreateMap<UpdatePhongHatDTO, SanPhamDichVu>()
                .ForMember(dest => dest.TenSanPham, opt => opt.MapFrom(src => src.TenPhong))
                .ForMember(dest => dest.HinhAnhSanPham, opt => opt.MapFrom(src => src.HinhAnhPhong));

            CreateMap<UpdatePhongHatDTO, PhongHatKaraoke>()
                .ForMember(dest => dest.MaLoaiPhong, opt => opt.MapFrom(src => src.MaLoaiPhong ?? 0));
        }

        private void CreateRoomPricingMappings()
        {
            CreateMap<AddPhongHatDTO, GiaDichVu>()
                .ForMember(dest => dest.NgayApDung, opt => opt.MapFrom(src => src.NgayApDungGia))
                .ForMember(dest => dest.TrangThai, opt => opt.MapFrom(src => src.TrangThaiGia));

            CreateMap<UpdatePhongHatDTO, GiaDichVu>()
                .ForMember(dest => dest.NgayApDung, opt => opt.MapFrom(src => src.NgayApDungGia ?? DateOnly.FromDateTime(DateTime.Now)))
                .ForMember(dest => dest.TrangThai, opt => opt.MapFrom(src => src.TrangThaiGia ?? "HieuLuc"));
        }
    }
}