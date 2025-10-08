using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.QLNhanSuDTOs;

namespace QLQuanKaraokeHKT.Core.Interfaces.Services.HRM
{
    public interface IQLCaLamViecService
    {
        Task<ServiceResult> CreateCaLamViecAsync(AddCaLamViecDTO addCaLamViecDto);
        Task<ServiceResult> GetAllCaLamViecsAsync();
        Task<ServiceResult> GetCaLamViecByIdAsync(int maCa);
    }
}
