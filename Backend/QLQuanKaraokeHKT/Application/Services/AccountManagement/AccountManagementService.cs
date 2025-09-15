using AutoMapper;
using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Core.Interfaces;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Auth;
using QLQuanKaraokeHKT.Core.Interfaces.Services.AccountManagement;

namespace QLQuanKaraokeHKT.Application.Services.AccountManagement
{
    public class AccountManagementService : IAccountManagementService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AccountManagementService(
            IMapper mapper,
            IUnitOfWork unitOfWork
            )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }  
        public async Task<ServiceResult> GetAllLoaiTaiKhoanAsync()
        {
            var result = RoleHelper.GetAllRoleCodes();
            if (result == null || !result.Any())
            {
                return ServiceResult.Failure("Không có loại tài khoản nào trong hệ thống.");
            }
            return ServiceResult.Success("Lấy danh sách loại tài khoản thành công.", result);

        }


        public async Task<ServiceResult> DeleteAccountAsync(Guid maTaiKhoan)
        {
            if (maTaiKhoan == Guid.Empty)
            {
                return ServiceResult.Failure("Mã tài khoản không hợp lệ.");
            }
            var result = await _unitOfWork.AccountManagementRepository.DeleteUserByIdAsync(maTaiKhoan);
            if (result)
            {
                return ServiceResult.Success("Tài khoản đã được xóa thành công.");
            }
            return ServiceResult.Failure("Không thể xóa tài khoản. Vui lòng thử lại sau.");
        }

        public async Task<ServiceResult> UpdateAccountAsync(UpdateAccountDTO updateAccountDTO)
        {
            try
            {
                if (updateAccountDTO == null || string.IsNullOrWhiteSpace(updateAccountDTO.newUserName))
                {
                    return ServiceResult.Failure("Thông tin cập nhật không hợp lệ.");
                }


                var existingUser = await _unitOfWork.IdentityRepository.FindByUserIDAsync(updateAccountDTO.maTaiKhoan.ToString());
                if (existingUser == null)
                {
                    return ServiceResult.Failure("Không tìm thấy tài khoản.");
                }

                string oldRole = existingUser.loaiTaiKhoan;

                _mapper.Map(updateAccountDTO, existingUser);

                var updateResult = await _unitOfWork.IdentityRepository.UpdateUserAsync(existingUser);
                if (!updateResult.Succeeded)
                {
                    var errors = updateResult.Errors.Select(e => e.Description).ToList();
                    return ServiceResult.Failure("Cập nhật thông tin tài khoản thất bại.", errors);
                }

                if (!string.IsNullOrWhiteSpace(updateAccountDTO.newPassword))
                {
                    var passwordResult = await _unitOfWork.IdentityRepository.UpdatePasswordAsync(existingUser, updateAccountDTO.newPassword);
                    if (!passwordResult.Success)
                    {
                        return ServiceResult.Failure("Cập nhật mật khẩu thất bại.", passwordResult.Errors);
                    }
                }

                if (!string.IsNullOrWhiteSpace(updateAccountDTO.newLoaiTaiKhoan) &&
                    oldRole != updateAccountDTO.newLoaiTaiKhoan)
                {
                    var roleUpdateResult = await _unitOfWork.RoleRepository.UpdateUserRoleAsync(existingUser, updateAccountDTO.newLoaiTaiKhoan);
                    if (!roleUpdateResult)
                    {
                        return ServiceResult.Failure("Cập nhật vai trò thất bại.");
                    }
                }


                return ServiceResult.Success("Cập nhật tài khoản thành công.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi hệ thống khi cập nhật tài khoản: {ex.Message}");
            }
        }

    }
}
