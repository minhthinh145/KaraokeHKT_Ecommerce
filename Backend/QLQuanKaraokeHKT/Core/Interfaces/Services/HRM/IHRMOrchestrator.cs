using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs;
using QLQuanKaraokeHKT.Core.DTOs.QLNhanSuDTOs;

namespace QLQuanKaraokeHKT.Core.Interfaces.Services.HRM
{
    public interface IHRMOrchestrator
    {
        Task<ServiceResult> GetAllNhanVienAsync();

        Task<ServiceResult> AddNhanVienWithAccountWorkFlowAsync(AddNhanVienDTO addNhanVienDto);

        Task<ServiceResult> UpdateNhanVienAsyncAndAccountAsync(NhanVienDTO nhanVienDto);

        Task<ServiceResult> UpdateNhanVienDaNghiViecAsync(Guid maNhanVien, bool daNghiViec);
    }
}