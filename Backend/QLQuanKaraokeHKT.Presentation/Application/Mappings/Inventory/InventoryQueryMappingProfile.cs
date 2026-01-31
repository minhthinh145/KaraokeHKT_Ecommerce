using AutoMapper;
using QLQuanKaraokeHKT.Application.DTOs.QLKhoDTOs;
using QLQuanKaraokeHKT.Domain.Entities;

namespace QLQuanKaraokeHKT.Application.Mappings.Inventory
{
    public class InventoryQueryMappingProfile : Profile
    {
        public InventoryQueryMappingProfile()
        {
            CreateVatLieuDetailMappings();
        }

        private void CreateVatLieuDetailMappings()
        {
            CreateMap<VatLieu, VatLieuDetailDTO>()
                .ForMember(dest => dest.MaSanPham, opt => opt.MapFrom(src => src.MonAn != null ? src.MonAn.MaSanPham : (int?)null))
                .ForMember(dest => dest.TenSanPham, opt => opt.MapFrom(src => src.MonAn != null && src.MonAn.MaSanPhamNavigation != null ? src.MonAn.MaSanPhamNavigation.TenSanPham : null))
                .ForMember(dest => dest.HinhAnhSanPham, opt => opt.MapFrom(src => src.MonAn != null && src.MonAn.MaSanPhamNavigation != null ? src.MonAn.MaSanPhamNavigation.HinhAnhSanPham : null))
                .ForMember(dest => dest.MaMonAn, opt => opt.MapFrom(src => src.MonAn != null ? src.MonAn.MaMonAn : (int?)null))
                .ForMember(dest => dest.SoLuongConLai, opt => opt.MapFrom(src => src.MonAn != null ? src.MonAn.SoLuongConLai : (int?)null))

                .ForMember(dest => dest.DongGiaAllCa, opt => opt.Ignore())
                .ForMember(dest => dest.GiaNhapHienTai, opt => opt.Ignore())
                .ForMember(dest => dest.NgayApDungGiaNhap, opt => opt.Ignore())
                .ForMember(dest => dest.TrangThaiGiaNhap, opt => opt.Ignore())
                .ForMember(dest => dest.GiaBanChung, opt => opt.Ignore())
                .ForMember(dest => dest.GiaBanCa1, opt => opt.Ignore())
                .ForMember(dest => dest.GiaBanCa2, opt => opt.Ignore())
                .ForMember(dest => dest.GiaBanCa3, opt => opt.Ignore())
                .ForMember(dest => dest.GiaBanHienTai, opt => opt.Ignore())
                .ForMember(dest => dest.NgayApDungGiaBan, opt => opt.Ignore())
                .ForMember(dest => dest.TrangThaiGiaBan, opt => opt.Ignore());
        }
    }
}