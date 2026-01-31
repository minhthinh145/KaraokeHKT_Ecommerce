﻿using AutoMapper;
using QLQuanKaraokeHKT.Application.Helpers;
using QLQuanKaraokeHKT.Shared.Constants;
using QLQuanKaraokeHKT.Application.DTOs.QLHeThongDTOs;
using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Application.Services.Interfaces.AccountManagement;

namespace QLQuanKaraokeHKT.Application.Services.Implementations.AccountManagement
{
    public class AdminAccountService : IAdminAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AdminAccountService( IMapper mapper,IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResult> GetAllAdminAccountAsync()
        {
            {
                var taiKhoanQuanLyList = await _unitOfWork.TaiKhoanQuanLyRepository.GettAllAdminAccount();
                if (taiKhoanQuanLyList == null || !taiKhoanQuanLyList.Any())
                {
                    return ServiceResult.Failure("Không có tài khoản quản lý nào trong hệ thống.");
                }
                var taiKhoanQuanLyDTOs = _mapper.Map<IEnumerable<TaiKhoanQuanLyDTO>>(taiKhoanQuanLyList);
                return ServiceResult.Success("Lấy danh sách tài khoản quản lý thành công.", taiKhoanQuanLyDTOs);

            }
        }

        public async Task<ServiceResult> AddAdminAccountAsync(AddAccountForAdminDTO adminDTO)
        {
            if (adminDTO == null)
            {
                return ServiceResult.Failure("Thông tin tài khoản quản lý không hợp lệ.");
            }
            var TaiKhoanQuanLy = _mapper.Map<TaiKhoan>(adminDTO);
            TaiKhoanQuanLy.daKichHoat = true;
            TaiKhoanQuanLy.EmailConfirmed = true;
            TaiKhoanQuanLy.daBiKhoa = false;
            string password = PasswordHelper.GenerateAdminPassword(TaiKhoanQuanLy.loaiTaiKhoan);

            var result = await _unitOfWork.IdentityRepository.CreateUserAsync(TaiKhoanQuanLy, password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return ServiceResult.Failure("Tạo tài khoản quản lý thất bại.", errors);
            }

            await _unitOfWork.RoleRepository.AddToRoleAsync(TaiKhoanQuanLy, TaiKhoanQuanLy.loaiTaiKhoan);

            return ServiceResult.Success("Tạo tài khoản quản lý thành công.");
        }

     
    }
}
