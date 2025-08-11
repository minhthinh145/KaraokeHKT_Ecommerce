using QLQuanKaraokeHKT.DTOs.QLNhanSuDTOs;
using QLQuanKaraokeHKT.Helpers;

namespace QLQuanKaraokeHKT.Services.QLNhanSuServices.QLCaLamViecServices
{
    public interface IQLCaLamViecService
    {
        Task<ServiceResult> CreateCaLamViecAsync(AddCaLamViecDTO addCaLamViecDto);
        Task<ServiceResult> GetAllCaLamViecsAsync();
        Task<ServiceResult> GetCaLamViecByIdAsync(int maCa);
    }
}
