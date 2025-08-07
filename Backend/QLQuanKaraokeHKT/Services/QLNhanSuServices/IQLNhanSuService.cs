using QLQuanKaraokeHKT.DTOs;
using QLQuanKaraokeHKT.DTOs.QLNhanSuDTOs;
using QLQuanKaraokeHKT.Helpers;

namespace QLQuanKaraokeHKT.Services.QLHeThongServices
{
    public interface IQLNhanSuService
    {
        Task<ServiceResult> GetAllNhanVienAsync();
        Task<ServiceResult> AddNhanVienAndAccounAsync(AddNhanVienDTO addNhanVienDto, string password);
        Task<ServiceResult> UpdateNhanVienAsyncAndAccountAsync(NhanVienDTO nhanVienDto);
    }
}