using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs;
using QLQuanKaraokeHKT.Core.DTOs.QLNhanSuDTOs;

namespace QLQuanKaraokeHKT.Core.Interfaces.Services.HRM
{
    public interface IQLNhanSuService
    {
        Task<ServiceResult> GetAllNhanVienAsync();
        Task<ServiceResult> AddNhanVienAndAccountAsync(AddNhanVienDTO addNhanVienDto, string password);
        Task<ServiceResult> UpdateNhanVienAsyncAndAccountAsync(NhanVienDTO nhanVienDto);

        Task<ServiceResult> UpdateNhanVienDaNghiViecAsync(Guid maNhanVien, bool daNghiViec);
    }
}