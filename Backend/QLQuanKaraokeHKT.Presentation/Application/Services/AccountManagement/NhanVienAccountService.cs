using AutoMapper;
using QLQuanKaraokeHKT.Application.Helpers;
using QLQuanKaraokeHKT.Application.Services.Auth;
using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs;
using QLQuanKaraokeHKT.Core.DTOs.QLHeThongDTOs;
using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Core.Interfaces;

using QLQuanKaraokeHKT.Core.Interfaces.Services.AccountManagement;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Auth;
using QLQuanKaraokeHKT.Core.Interfaces.Services.External;

namespace QLQuanKaraokeHKT.Application.Services.AccountManagement
{
    public class NhanVienAccountService : INhanVienAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISendEmailService _sendEmailService ;

        public NhanVienAccountService(
            IUnitOfWork unitOfWork,
            ISendEmailService sendEmailService,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sendEmailService = sendEmailService;
        }

        public async Task<ServiceResult> GetAllTaiKhoanNhanVienAsync()
        {
            try
            {
                var nhanViens = await _unitOfWork.NhanVienRepository.GetAllNhanVienWithTaiKhoanAsync(); 
                if (nhanViens == null || !nhanViens.Any())
                {
                    return ServiceResult.Failure("Không có nhân viên nào trong hệ thống.");
                }

                var nhanVienTaiKhoanDTOs = _mapper.Map<IEnumerable<NhanVienTaiKhoanDTO>>(nhanViens);
                return ServiceResult.Success("Lấy danh sách tài khoản nhân viên thành công.", nhanVienTaiKhoanDTOs);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi hệ thống khi lấy danh sách tài khoản nhân viên: {ex.Message}");
            }
        }

        public async Task<ServiceResult> GetAllTaiKhoanNhanVienByLoaiTaiKhoanAsync(string loaiTaiKhoan)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(loaiTaiKhoan))
                {
                    return ServiceResult.Failure("Loại tài khoản không được để trống.");
                }

                var nhanViens = await _unitOfWork.NhanVienRepository.GetAllByLoaiTaiKhoanWithTaiKhoanAsync(loaiTaiKhoan); 
                if (nhanViens == null || !nhanViens.Any())
                {
                    return ServiceResult.Failure($"Không có nhân viên nào với loại tài khoản '{loaiTaiKhoan}'.");
                }

                var nhanVienTaiKhoanDTOs = _mapper.Map<IEnumerable<NhanVienTaiKhoanDTO>>(nhanViens);
                return ServiceResult.Success($"Lấy danh sách nhân viên loại '{loaiTaiKhoan}' thành công.", nhanVienTaiKhoanDTOs);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi hệ thống khi lấy danh sách nhân viên theo loại tài khoản: {ex.Message}");
            }
        }

        public async Task<ServiceResult> ChangeTaiKhoanForNhanVienAsync(AddTaiKhoanForNhanVienDTO request)
        {

            try
            {
                var validationResult = ValidationHelper.ValidateAddTaiKhoanForNhanVien(request);
                if (!validationResult.IsSuccess)
                    return validationResult;

                var nhanVien = await _unitOfWork.NhanVienRepository.GetByIdWithTaiKhoanAsync(request.MaNhanVien);
                if (nhanVien?.MaTaiKhoan == Guid.Empty)
                {
                    return ServiceResult.Failure("Nhân viên không có tài khoản hoặc không tồn tại.");
                }

                var taiKhoan = await _unitOfWork.IdentityRepository.FindByUserIDAsync(nhanVien.MaTaiKhoan.ToString());
                if (taiKhoan == null)
                {
                    return ServiceResult.Failure("Không tìm thấy tài khoản liên kết.");
                }

                await UpdateAccountInfoAsync(taiKhoan, request);

                await UpdateEmployeeEmailAsync(nhanVien, request.Email);

                if (!string.IsNullOrWhiteSpace(request.Password))
                {
                    var formattedPassword = PasswordHelper.FormatPassword(request.Password);
                    await _sendEmailService.SendPasswordResetEmailAsync(request.Email, nhanVien.HoTen, formattedPassword);
                }

                var result = _mapper.Map<NhanVienDTO>(nhanVien);
                return ServiceResult.Success("Cập nhật email tài khoản thành công.", result);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi hệ thống: {ex.Message}");
            }
        }

        public async Task<ServiceResult> GetProfileByUserIdAsync(Guid userId)
        {
            var taiKhoan = await _unitOfWork.IdentityRepository.FindByUserIDAsync(userId.ToString());

            if (taiKhoan == null) return ServiceResult.Failure("Không tìm thấy tài khoản.");

            var nhanVien = await _unitOfWork.NhanVienRepository.GetNhanVienByTaiKhoanIdAsync(taiKhoan.Id);

            if (nhanVien == null) return ServiceResult.Failure("Không tìm thấy nhân viên.");

            var dto = _mapper.Map<NhanVienTaiKhoanDTO>(nhanVien);
            return ServiceResult.Success("Lấy profile thành công.", dto);
        }

        #region helper Method
        private async Task UpdateAccountInfoAsync(TaiKhoan taiKhoan, AddTaiKhoanForNhanVienDTO request)
        {
            taiKhoan.Email = request.Email;
            taiKhoan.UserName = request.Email;
            taiKhoan.NormalizedEmail = request.Email.ToUpper();
            taiKhoan.NormalizedUserName = request.Email.ToUpper();

            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                var passwordResult = await _unitOfWork.IdentityRepository.UpdatePasswordAsync(taiKhoan, PasswordHelper.FormatPassword(request.Password));
                if (!passwordResult.Success)
                {
                    throw new InvalidOperationException($"Cập nhật mật khẩu thất bại: {string.Join(", ", passwordResult.Errors)}");
                }
            }

            var updateResult = await _unitOfWork.IdentityRepository.UpdateUserAsync(taiKhoan);
            if (!updateResult.Succeeded)
            {
                var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Cập nhật tài khoản thất bại: {errors}");
            }
        }

        private async Task UpdateEmployeeEmailAsync(NhanVien nhanVien, string newEmail)
        {
            if (nhanVien.Email != newEmail)
            {
                nhanVien.Email = newEmail;
                var updateResult = await _unitOfWork.NhanVienRepository.UpdateAsync(nhanVien);
                if (!updateResult)
                {
                    throw new InvalidOperationException("Cập nhật email nhân viên thất bại.");
                }
            }
        }
        #endregion
    }
}
