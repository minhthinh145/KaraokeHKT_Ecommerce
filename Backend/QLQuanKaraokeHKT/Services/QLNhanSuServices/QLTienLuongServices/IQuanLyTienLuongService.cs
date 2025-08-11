using QLQuanKaraokeHKT.DTOs.QLNhanSuDTOs;
using QLQuanKaraokeHKT.Helpers;

namespace QLQuanKaraokeHKT.Services.QLNhanSuServices.QLTienLuongServices
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
