using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.AuthDTOs;

namespace QLQuanKaraokeHKT.Core.Interfaces.Services.AccountManagement
{
    public interface IAccountManagementService
    {
        Task<ServiceResult> LockAccountByMaTaiKhoanAsync(Guid maTaiKhoan);
        Task<ServiceResult> UnlockAccountByMaTaiKhoanAsync(Guid maTaiKhoan);
        Task<ServiceResult> GetAllLoaiTaiKhoanAsync();
        Task<ServiceResult> DeleteAccountAsync(Guid maTaiKhoan);
        Task<ServiceResult> UpdateAccountAsync(UpdateAccountDTO updateAccountDTO);


    }
}
