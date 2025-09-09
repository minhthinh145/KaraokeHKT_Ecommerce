using AutoMapper;
using QLQuanKaraokeHKT.Core.DTOs;
using QLQuanKaraokeHKT.Core.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Core.DTOs.Core;
using QLQuanKaraokeHKT.Core.DTOs.QLNhanSuDTOs;
using QLQuanKaraokeHKT.Core.Entities;

namespace QLQuanKaraokeHKT.Application.Mappings.Account
{
    public class AccountMappingProfile : Profile
    {
        public AccountMappingProfile()
        {
            CreateAccountMappings();
            UpdateAccountMappings();
        }


        private void CreateAccountMappings()
        {
            CreateMap<CreateAccountRequest, TaiKhoan>()
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
               .ForMember(dest => dest.loaiTaiKhoan, opt => opt.MapFrom(src => src.RoleCode))
               .ForMember(dest => dest.daKichHoat, opt => opt.MapFrom(src => src.IsActive))
               .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => src.IsEmailConfirmed));

            CreateMap<TaiKhoan, CreateAccountRequest>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.RoleCode, opt => opt.MapFrom(src => src.loaiTaiKhoan))
                .ForMember(dest => dest.IsEmailConfirmed, opt => opt.MapFrom(src => src.EmailConfirmed))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.daKichHoat))
                .ForMember(dest => dest.RequireEmailVerification, opt => opt.MapFrom(src => !src.EmailConfirmed));

            CreateMap<CreateAccountRequest, AddNhanVienDTO>()
                .ForMember(dest => dest.SoDienThoai, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.HoTen, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.LoaiTaiKhoan, opt => opt.MapFrom(src => src.RoleCode))
                .ReverseMap();

        }

        private void UpdateAccountMappings()
        {
            CreateMap<UpdateAccountRequest, TaiKhoan>()
                 .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                 .ForMember(dest => dest.loaiTaiKhoan, opt => opt.MapFrom(src => src.RoleCode));

            CreateMap<TaiKhoan, UpdateAccountRequest>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.RoleCode, opt => opt.MapFrom(src => src.loaiTaiKhoan));

            CreateMap<UpdateAccountRequest, UpdateAccountDTO>()
                .ForMember(dest => dest.maTaiKhoan, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.newLoaiTaiKhoan, opt => opt.MapFrom(src => src.RoleCode))
                .ForMember(dest => dest.newUserName, opt => opt.MapFrom(src => src.FullName));

            CreateMap<UpdateAccountRequest, NhanVienDTO>()
               .ForMember(dest => dest.SoDienThoai, opt => opt.MapFrom(src => src.PhoneNumber))
               .ForMember(dest => dest.LoaiNhanVien, opt => opt.MapFrom(src => src.RoleCode))
               .ForMember(dest => dest.HoTen, opt => opt.MapFrom(src => src.FullName))
               .ReverseMap();

        }
    }
}
