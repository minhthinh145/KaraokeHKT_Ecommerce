﻿using AutoMapper;
using QLQuanKaraokeHKT.Shared.Constants;
using QLQuanKaraokeHKT.Application.DTOs;
using QLQuanKaraokeHKT.Application.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Application.DTOs.QLHeThongDTOs;
using QLQuanKaraokeHKT.Domain.Entities;

namespace QLQuanKaraokeHKT.Application.Mappings.Auth
{
    public class AuthMappingProfile : Profile
    {
        public AuthMappingProfile()
        {
            CreateSignUpMappings();
            CreateSignInMappings();
            CreateUserProfileMappings();
            CreateOtpMappings();
            CreateAccountManagementMappings();
        }

        private void CreateSignUpMappings()
        {
            CreateMap<SignUpDTO, TaiKhoan>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.loaiTaiKhoan, opt => opt.MapFrom(src => ApplicationRole.KhacHang))
                .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.daKichHoat, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.daBiKhoa, opt => opt.MapFrom(src => false));


            CreateMap<SignUpDTO, KhachHang>()
                .ForMember(dest => dest.TenKhachHang, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.NgaySinh, opt => opt.MapFrom(src =>
                    src.DateOfBirth.HasValue ? DateOnly.FromDateTime(src.DateOfBirth.Value) : (DateOnly?)null));
       
        }

        private void CreateSignInMappings()
        {
            CreateMap<(string AccessToken, string RefreshToken), LoginResponseDTO>()
                .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.AccessToken))
                .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.RefreshToken));
        }

        private void CreateUserProfileMappings()
        {
            CreateMap<TaiKhoan, UserProfileDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => GetDisplayName(src)))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.LoaiTaiKhoan, opt => opt.MapFrom(src => src.loaiTaiKhoan))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => GetBirthDate(src)));

            CreateMap<KhachHang, UserProfileDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => GetKhachHangDisplayName(src)))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email ?? src.MaTaiKhoanNavigation.Email))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.MaTaiKhoanNavigation.PhoneNumber))
                .ForMember(dest => dest.LoaiTaiKhoan, opt => opt.MapFrom(src => src.MaTaiKhoanNavigation.loaiTaiKhoan))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src =>
                    src.NgaySinh.HasValue ? src.NgaySinh.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null));

            CreateMap<UserProfileDTO, TaiKhoan>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone));


            CreateMap<UserProfileDTO, KhachHang>()
                .ForMember(dest => dest.TenKhachHang, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.NgaySinh, opt => opt.MapFrom(src =>
                    src.BirthDate.HasValue ? DateOnly.FromDateTime(src.BirthDate.Value) : (DateOnly?)null));

        }

        private void CreateOtpMappings()
        {
            CreateMap<CreateUserOtpDTO, MaOtp>()
                .ForMember(dest => dest.NgayTao, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.NgayHetHan, opt => opt.MapFrom(src => src.ExpirationTime))
                .ForMember(dest => dest.DaSuDung, opt => opt.MapFrom(src => false));

        }

        private void CreateAccountManagementMappings()
        {
            CreateMap<TaiKhoanQuanLyDTO, TaiKhoan>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.MaTaiKhoan))
                .ReverseMap();

            CreateMap<AddAccountForAdminDTO, TaiKhoan>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.UserName))
                .ReverseMap();

            CreateMap<UpdateAccountDTO, TaiKhoan>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.newUserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.newUserName))
                .ForMember(dest => dest.loaiTaiKhoan, opt => opt.MapFrom(src => src.newLoaiTaiKhoan));
        }

        #region Helper Methods
        private static string GetDisplayName(TaiKhoan taiKhoan)
        {
            var khachHang = taiKhoan.KhachHangs.FirstOrDefault();
            return !string.IsNullOrEmpty(khachHang?.TenKhachHang)
                ? khachHang.TenKhachHang
                : !string.IsNullOrEmpty(taiKhoan.FullName)
                    ? taiKhoan.FullName
                    : taiKhoan.UserName;
        }

        private static DateTime? GetBirthDate(TaiKhoan taiKhoan)
        {
            var khachHang = taiKhoan.KhachHangs.FirstOrDefault();
            return khachHang?.NgaySinh?.ToDateTime(TimeOnly.MinValue);
        }

        private static string GetKhachHangDisplayName(KhachHang khachHang)
        {
            return !string.IsNullOrEmpty(khachHang.TenKhachHang)
                ? khachHang.TenKhachHang
                : !string.IsNullOrEmpty(khachHang.MaTaiKhoanNavigation.FullName)
                    ? khachHang.MaTaiKhoanNavigation.FullName
                    : khachHang.MaTaiKhoanNavigation.UserName;
        }
        #endregion
    }
}