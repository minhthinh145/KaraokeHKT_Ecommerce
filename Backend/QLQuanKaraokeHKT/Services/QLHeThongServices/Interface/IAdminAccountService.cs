using QLQuanKaraokeHKT.DTOs.QLHeThongDTOs;
using QLQuanKaraokeHKT.Helpers;

namespace QLQuanKaraokeHKT.Services.QLHeThongServices.Interface
{
    public interface IAdminAccountService
    {
        Task<ServiceResult> GetAllAdminAccountAsync();
        Task<ServiceResult> AddAdminAccountAsync(AddAccountForAdminDTO adminDTO);
    }
}