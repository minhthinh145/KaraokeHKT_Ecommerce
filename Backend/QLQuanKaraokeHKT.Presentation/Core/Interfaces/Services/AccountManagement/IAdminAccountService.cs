using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.QLHeThongDTOs;

namespace QLQuanKaraokeHKT.Core.Interfaces.Services.AccountManagement
{
    public interface IAdminAccountService
    {
        Task<ServiceResult> GetAllAdminAccountAsync();
        Task<ServiceResult> AddAdminAccountAsync(AddAccountForAdminDTO adminDTO);
    }
}