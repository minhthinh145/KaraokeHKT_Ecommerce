using AutoMapper;
using QLQuanKaraokeHKT.Application.DTOs.QLKhoDTOs;
using QLQuanKaraokeHKT.Domain.Entities;

namespace QLQuanKaraokeHKT.Application.Mappings.Inventory
{
    public class InventoryMappingProfile : Profile
    {
        public InventoryMappingProfile()
        {
            CreateVatLieuMappings();
            CreatePricingMappings();
        }

        private void CreateVatLieuMappings()
        {
            CreateMap<AddVatLieuDTO, SanPhamDichVu>()
                .ForMember(dest => dest.TenSanPham, opt => opt.MapFrom(src => src.TenSanPham));

            CreateMap<AddVatLieuDTO, VatLieu>();


            CreateMap<UpdateVatLieuDTO, VatLieu>();


            CreateMap<UpdateVatLieuDTO, SanPhamDichVu>();
        }

        private void CreatePricingMappings()
        {
            CreateMap<AddVatLieuDTO, GiaVatLieu>()
                .ForMember(dest => dest.DonGia, opt => opt.MapFrom(src => src.GiaNhap))
                .ForMember(dest => dest.NgayApDung, opt => opt.MapFrom(src => src.NgayApDungGiaNhap))
                .ForMember(dest => dest.TrangThai, opt => opt.MapFrom(src => src.TrangThaiGiaNhap));

            CreateMap<AddVatLieuDTO, GiaDichVu>()
                .ForMember(dest => dest.NgayApDung, opt => opt.MapFrom(src => src.NgayApDungGiaBan))
                .ForMember(dest => dest.TrangThai, opt => opt.MapFrom(src => src.TrangThaiGiaBan));

            CreateMap<UpdateVatLieuDTO, GiaVatLieu>()
                .ForMember(dest => dest.DonGia, opt => opt.MapFrom(src => src.GiaNhapMoi ?? 0))
                .ForMember(dest => dest.NgayApDung, opt => opt.MapFrom(src => src.NgayApDungGiaNhap ?? DateOnly.FromDateTime(DateTime.Now)))
                .ForMember(dest => dest.TrangThai, opt => opt.MapFrom(src => src.TrangThaiGiaNhap ?? "HieuLuc"));

            CreateMap<UpdateVatLieuDTO, GiaDichVu>()
                .ForMember(dest => dest.NgayApDung, opt => opt.MapFrom(src => src.NgayApDungGiaBan ?? DateOnly.FromDateTime(DateTime.Now)))
                .ForMember(dest => dest.TrangThai, opt => opt.MapFrom(src => src.TrangThaiGiaBan ?? "HieuLuc"));
        }
    }
}