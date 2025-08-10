using QLQuanKaraokeHKT.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Helpers;

namespace QLQuanKaraokeHKT.Services.QLHeThongServices.Interface
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
