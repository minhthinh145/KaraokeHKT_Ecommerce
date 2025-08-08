using QLQuanKaraokeHKT.Helpers;
using QLQuanKaraokeHKT.Repositories.TaiKhoanRepo;
using QLQuanKaraokeHKT.Services.QLHeThongServices.Interface;

namespace QLQuanKaraokeHKT.Services.QLHeThongServices.Implementation
{
    public class AccountManagementService : IAccountManagementService
    {
        private readonly ITaiKhoanRepository _taiKhoanRepository;

        public AccountManagementService(ITaiKhoanRepository taiKhoanRepository)
        {
            _taiKhoanRepository = taiKhoanRepository;
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
    }
}
