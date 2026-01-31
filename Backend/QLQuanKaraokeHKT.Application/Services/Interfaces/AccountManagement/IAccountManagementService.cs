
using QLQuanKaraokeHKT.Application.DTOs.AuthDTOs;

namespace QLQuanKaraokeHKT.Application.Services.Interfaces.AccountManagement
{
    public interface IAccountManagementService
    {
        Task<ServiceResult> GetAllLoaiTaiKhoanAsync();
        Task<ServiceResult> DeleteAccountAsync(Guid maTaiKhoan);
        Task<ServiceResult> UpdateAccountAsync(UpdateAccountDTO updateAccountDTO);

    }
}
