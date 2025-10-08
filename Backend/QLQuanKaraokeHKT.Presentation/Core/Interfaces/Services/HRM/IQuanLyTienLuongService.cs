using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.QLNhanSuDTOs;

namespace QLQuanKaraokeHKT.Core.Interfaces.Services.HRM
{
    public interface IQuanLyTienLuongService
    {
        Task<ServiceResult> GetAllLuongCaLamViecsAsync();
        Task<ServiceResult> GetLuongCaLamViecByIdAsync(int maLuongCaLamViec);
        Task<ServiceResult> GetLuongCaLamViecByMaCaAsync(int maCa);
        Task<ServiceResult> CreateLuongCaLamViecAsync(AddLuongCaLamViecDTO addLuongCaLamViecDto);
        Task<ServiceResult> UpdateLuongCaLamViecAsync(LuongCaLamViecDTO luongCaLamViecDto);
        Task<ServiceResult> DeleteLuongCaLamViecAsync(int maLuongCaLamViec);    
    }
}
