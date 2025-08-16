using QLQuanKaraokeHKT.DTOs;
using QLQuanKaraokeHKT.DTOs.QLNhanSuDTOs;
using QLQuanKaraokeHKT.Helpers;

namespace QLQuanKaraokeHKT.Services.QLHeThongServices
{
    public interface IQLNhanSuService
    {
        Task<ServiceResult> GetAllNhanVienAsync();
        Task<ServiceResult> AddNhanVienAndAccountAsync(AddNhanVienDTO addNhanVienDto, string password);
        Task<ServiceResult> UpdateNhanVienAsyncAndAccountAsync(NhanVienDTO nhanVienDto);

        Task<ServiceResult> UpdateNhanVienDaNghiViecAsync(Guid maNhanVien, bool daNghiViec);
    }
}