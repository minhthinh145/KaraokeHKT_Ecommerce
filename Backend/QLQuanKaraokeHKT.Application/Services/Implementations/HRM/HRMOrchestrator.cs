﻿using AutoMapper;
using Org.BouncyCastle.Ocsp;
using QLQuanKaraokeHKT.Application.Helpers;
using QLQuanKaraokeHKT.Application.DTOs;
using QLQuanKaraokeHKT.Application.DTOs.Core;
using QLQuanKaraokeHKT.Application.DTOs.QLNhanSuDTOs;
using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Application.Services.Interfaces.Core;
using QLQuanKaraokeHKT.Application.Services.Interfaces.External;
using QLQuanKaraokeHKT.Application.Services.Interfaces.HRM;

namespace QLQuanKaraokeHKT.Application.Services.Implementations.HRM
{
    public class HRMOrchestrator : IHRMOrchestrator
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISendEmailService _emailService;
        private readonly ICoreAccountService _coreAccountService;

        public HRMOrchestrator(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ISendEmailService emailService,
            ICoreAccountService coreAccountService
            )
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _coreAccountService = coreAccountService ?? throw new ArgumentNullException(nameof(coreAccountService));
        }

        public async Task<ServiceResult> GetAllNhanVienAsync()
        {
            try
            {
                var nhanViens = await _unitOfWork.NhanVienRepository.GetAllAsync();
                if (nhanViens == null || !nhanViens.Any())
                {
                    return ServiceResult.Failure("Không có nhân viên nào trong hệ thống.");
                }

                var nhanVienDTOs = _mapper.Map<IEnumerable<NhanVienDTO>>(nhanViens);
                return ServiceResult.Success("Lấy danh sách nhân viên thành công.", nhanVienDTOs);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi hệ thống khi lấy danh sách nhân viên: {ex.Message}");
            }
        }

        public async Task<ServiceResult> AddNhanVienWithAccountWorkFlowAsync(AddNhanVienDTO addNhanVienDto)
        {
            try
            {
                var password = PasswordHelper.GenerateEmployeePassword(addNhanVienDto.LoaiTaiKhoan, addNhanVienDto.SoDienThoai);

                var accountResult = await CreateTaiKhoanForNhanVienAsync(addNhanVienDto);
                if (!accountResult.IsSuccess)
                    return accountResult;

                var taiKhoan = (TaiKhoan)accountResult.Data!;

                var addNhanVienResult = await _unitOfWork.ExecuteTransactionAsync(async () =>
                {

                    var employeeResult = await CreateNhanVienAsync(addNhanVienDto, taiKhoan.Id);

                    await _emailService.SendWelcomeEmployeeEmailAsync(addNhanVienDto.Email, addNhanVienDto.HoTen, password);

                    return (NhanVienDTO)employeeResult.Data!;

                });

                return ServiceResult.Success("Tạo nhân viên và tài khoản thành công.", addNhanVienResult);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi hệ thống khi tạo nhân viên: {ex.Message}");
            }
        }

        public async Task<ServiceResult> UpdateNhanVienAsyncAndAccountAsync(NhanVienDTO nhanVienDto)
        {
            try
            {
                if (nhanVienDto == null)
                    return ServiceResult.Failure("Thông tin cập nhật không hợp lệ.");

                var result = await _unitOfWork.ExecuteTransactionAsync(async () =>
                {
                    var updateAccountResult = await UpdateTaiKhoanAsync(nhanVienDto);
                    if (!updateAccountResult.IsSuccess)
                        throw new InvalidOperationException(updateAccountResult.Message);

                    var existingNhanVien = await _unitOfWork.NhanVienRepository.GetByIdAsync(nhanVienDto.MaNv);
                    if (existingNhanVien == null)
                        throw new InvalidOperationException("Không tìm thấy nhân viên.");

                    _mapper.Map(nhanVienDto, existingNhanVien);

                    var updateEmployeeResult = await _unitOfWork.NhanVienRepository.UpdateAsync(existingNhanVien);
                    if (!updateEmployeeResult)
                        throw new InvalidOperationException("Cập nhật nhân viên thất bại.");

                    return _mapper.Map<NhanVienDTO>(existingNhanVien);
                });

                return ServiceResult.Success("Cập nhật nhân viên và tài khoản thành công.", result);
            }
            catch (InvalidOperationException ex)
            {
                return ServiceResult.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi hệ thống khi cập nhật nhân viên: {ex.Message}");
            }
        }

        public async Task<ServiceResult> UpdateNhanVienDaNghiViecAsync(Guid maNhanVien, bool daNghiViec)
        {
            try
            {
                return await _unitOfWork.ExecuteTransactionAsync(async () => {
                    var existingNhanVien = await _unitOfWork.NhanVienRepository.GetByIdAsync(maNhanVien);
                    if (existingNhanVien == null)
                        return ServiceResult.Failure("Không tìm thấy nhân viên.");

                    var updateResult = await _unitOfWork.NhanVienRepository.UpdateNhanVienDaNghiViecAsync(maNhanVien, daNghiViec);
                    if (!updateResult)
                        return ServiceResult.Failure("Cập nhật trạng thái nhân viên thất bại.");

                    await _coreAccountService.MarkAccountAsLockOrUnlockAsync(existingNhanVien.MaTaiKhoan, daNghiViec);

                     return ServiceResult.Success("Cập nhật trạng thái nhân viên thành công.");

                 });

            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi hệ thống khi cập nhật trạng thái nhân viên: {ex.Message}");
            }
        }

        #region Private Helper Methods
        private async Task<ServiceResult<TaiKhoan>> CreateTaiKhoanForNhanVienAsync(AddNhanVienDTO addNhanVienDto)
        {
            string password = PasswordHelper.GenerateEmployeePassword(addNhanVienDto.LoaiTaiKhoan, addNhanVienDto.SoDienThoai);

            var taiKhoan = _mapper.Map<CreateAccountRequest>(addNhanVienDto);
            taiKhoan.IsEmailConfirmed = true;
            taiKhoan.RequireEmailVerification = false;
            taiKhoan.Password = password;

            return await _coreAccountService.CreateAccountAsync(taiKhoan);
        }

        private async Task<ServiceResult> CreateNhanVienAsync(AddNhanVienDTO addNhanVienDto, Guid maTaiKhoan)
        {
            var nhanVien = _mapper.Map<NhanVien>(addNhanVienDto);

            nhanVien.MaTaiKhoan = maTaiKhoan;

            var createNhanVienResult = await _unitOfWork.NhanVienRepository.CreateAsync(nhanVien);
            if (createNhanVienResult == null)
            {
                return ServiceResult.Failure("Tạo nhân viên thất bại.");
            }

            var nhanVienDTO = _mapper.Map<NhanVienDTO>(nhanVien);

            return ServiceResult.Success("Tạo nhân viên thành công.", nhanVienDTO);
        }

        private async Task<ServiceResult> UpdateTaiKhoanAsync(NhanVienDTO nhanVienDTO)
        {

            var updateRequest = _mapper.Map<UpdateAccountRequest>(nhanVienDTO);
            var taiKhoan = await _unitOfWork.IdentityRepository.FindByEmailAsync(nhanVienDTO.Email);

            if (taiKhoan == null)
            {
                return ServiceResult.Failure("Không tìm thấy tài khoản liên kết.");
            }

            updateRequest.Id = taiKhoan.Id;

            var updateUserResult = await _coreAccountService.UpdateAccountAsync(updateRequest);

            if (!updateUserResult.IsSuccess)
            {

                return ServiceResult.Failure($"Cập nhật tài khoản thất bại: {updateUserResult.Message}");
            }

            return ServiceResult.Success("Cập nhật tài khoản thành công.");
        }
        #endregion
    }
}