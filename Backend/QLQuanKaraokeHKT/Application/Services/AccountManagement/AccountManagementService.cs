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
        public async Task<ServiceResult> LockAccountByMaTaiKhoanAsync(Guid maTaiKhoan)
        {
            var result = await _unitOfWork.AccountManagementRepository.LockAccountAsync(maTaiKhoan);
            if (result)
            {
                return ServiceResult.Success("Tài khoản đã bị khóa thành công.");
            }
            return ServiceResult.Failure("Không thể khóa tài khoản. Vui lòng kiểm tra lại mã tài khoản hoặc trạng thái tài khoản.");

        }

        public async Task<ServiceResult> UnlockAccountByMaTaiKhoanAsync(Guid maTaiKhoan)
        {
            var result = await _unitOfWork.AccountManagementRepository.UnlockAccountAsync(maTaiKhoan);
            if (result)
            {
                return ServiceResult.Success("Tài khoản đã được mở khóa thành công.");
            }
            return ServiceResult.Failure("Không thể mở khóa tài khoản. Vui lòng kiểm tra lại mã tài khoản hoặc trạng thái tài khoản.");
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
                // 1. Validate input
                if (updateAccountDTO == null || string.IsNullOrWhiteSpace(updateAccountDTO.newUserName))
                {
                    return ServiceResult.Failure("Thông tin cập nhật không hợp lệ.");
                }

                // 2. Tìm tài khoản theo ID
                var existingUser = await _unitOfWork.IdentityRepository.FindByUserIDAsync(updateAccountDTO.maTaiKhoan.ToString());
                if (existingUser == null)
                {
                    return ServiceResult.Failure("Không tìm thấy tài khoản.");
                }

                // 3. Lưu role cũ để so sánh
                string oldRole = existingUser.loaiTaiKhoan;

                // 4. Map thông tin mới vào user (AutoMapper sẽ chỉ map các field có giá trị)
                _mapper.Map(updateAccountDTO, existingUser);

                // 5. Cập nhật thông tin tài khoản
                var updateResult = await _unitOfWork.IdentityRepository.UpdateUserAsync(existingUser);
                if (!updateResult.Succeeded)
                {
                    var errors = updateResult.Errors.Select(e => e.Description).ToList();
                    return ServiceResult.Failure("Cập nhật thông tin tài khoản thất bại.", errors);
                }

                // 6. Cập nhật mật khẩu nếu có
                if (!string.IsNullOrWhiteSpace(updateAccountDTO.newPassword))
                {
                    var passwordResult = await _unitOfWork.IdentityRepository.UpdatePasswordAsync(existingUser, updateAccountDTO.newPassword);
                    if (!passwordResult.Success)
                    {
                        return ServiceResult.Failure("Cập nhật mật khẩu thất bại.", passwordResult.Errors);
                    }
                }

                // 7. Cập nhật role nếu có thay đổi
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
