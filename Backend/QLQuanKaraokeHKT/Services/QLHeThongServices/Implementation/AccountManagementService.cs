using AutoMapper;
using QLQuanKaraokeHKT.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Helpers;
using QLQuanKaraokeHKT.Repositories.TaiKhoanRepo;
using QLQuanKaraokeHKT.Services.QLHeThongServices.Interface;

namespace QLQuanKaraokeHKT.Services.QLHeThongServices.Implementation
{
    public class AccountManagementService : IAccountManagementService
    {
        private readonly ITaiKhoanRepository _taiKhoanRepository;
        private readonly IMapper _mapper;

        public AccountManagementService(ITaiKhoanRepository taiKhoanRepository, IMapper mapper)
        {
            _taiKhoanRepository = taiKhoanRepository;
            _mapper = mapper;
        }   
        public async Task<ServiceResult> LockAccountByMaTaiKhoanAsync(Guid maTaiKhoan)
        {
            var result = await _taiKhoanRepository.LockAccountAsync(maTaiKhoan);
            if (result)
            {
                return ServiceResult.Success("Tài khoản đã bị khóa thành công.");
            }
            return ServiceResult.Failure("Không thể khóa tài khoản. Vui lòng kiểm tra lại mã tài khoản hoặc trạng thái tài khoản.");

        }

        public async Task<ServiceResult> UnlockAccountByMaTaiKhoanAsync(Guid maTaiKhoan)
        {
            var result = await _taiKhoanRepository.UnlockAccountAsync(maTaiKhoan);
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
            var result = await _taiKhoanRepository.DeleteUserByIdAsync(maTaiKhoan);
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
                var existingUser = await _taiKhoanRepository.FindByUserIDAsync(updateAccountDTO.maTaiKhoan.ToString());
                if (existingUser == null)
                {
                    return ServiceResult.Failure("Không tìm thấy tài khoản.");
                }

                // 3. Lưu role cũ để so sánh
                string oldRole = existingUser.loaiTaiKhoan;

                // 4. Map thông tin mới vào user (AutoMapper sẽ chỉ map các field có giá trị)
                _mapper.Map(updateAccountDTO, existingUser);

                // 5. Cập nhật thông tin tài khoản
                var updateResult = await _taiKhoanRepository.UpdateUserAsync(existingUser);
                if (!updateResult.Succeeded)
                {
                    var errors = updateResult.Errors.Select(e => e.Description).ToList();
                    return ServiceResult.Failure("Cập nhật thông tin tài khoản thất bại.", errors);
                }

                // 6. Cập nhật mật khẩu nếu có
                if (!string.IsNullOrWhiteSpace(updateAccountDTO.newPassword))
                {
                    var passwordResult = await _taiKhoanRepository.UpdatePasswordAsync(existingUser, updateAccountDTO.newPassword);
                    if (!passwordResult.Success)
                    {
                        return ServiceResult.Failure("Cập nhật mật khẩu thất bại.", passwordResult.Errors);
                    }
                }

                // 7. Cập nhật role nếu có thay đổi
                if (!string.IsNullOrWhiteSpace(updateAccountDTO.newLoaiTaiKhoan) &&
                    oldRole != updateAccountDTO.newLoaiTaiKhoan)
                {
                    var roleUpdateResult = await _taiKhoanRepository.UpdateUserRoleAsync(existingUser, updateAccountDTO.newLoaiTaiKhoan);
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
