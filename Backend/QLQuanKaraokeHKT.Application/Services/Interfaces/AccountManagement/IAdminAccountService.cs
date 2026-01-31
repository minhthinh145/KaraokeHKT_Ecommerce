using QLQuanKaraokeHKT.Application.DTOs.QLHeThongDTOs;

namespace QLQuanKaraokeHKT.Application.Services.Interfaces.AccountManagement
{
    public interface IAdminAccountService
    {
        Task<ServiceResult> GetAllAdminAccountAsync();
        Task<ServiceResult> AddAdminAccountAsync(AddAccountForAdminDTO adminDTO);
    }
}