using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.QLNhanSuDTOs;

namespace QLQuanKaraokeHKT.Core.Interfaces.Services.HRM
{

    public interface IQLLichLamViecService
    {

        Task<ServiceResult> CreateLichLamViecAsync(AddLichLamViecDTO addLichLamViecDto);

        Task<ServiceResult> GetAllLichLamViecAsync();

        Task<ServiceResult> GetLichLamViecByNhanVienAsync(Guid maNhanVien);

        Task<ServiceResult> GetLichLamViecByRangeAsync(DateOnly start, DateOnly end);

        Task<ServiceResult> UpdateLichLamViecAsync(LichLamViecDTO lichLamViecDto);

        Task<ServiceResult> DeleteLichLamViecAsync(int maLichLamViec);

        Task<ServiceResult> SendNotiWorkSchedulesAsync(DateOnly start, DateOnly end);

        Task<ServiceResult> GetLichLamViecByNhanVienAndRangeAsync(Guid maNhanVien, DateOnly start, DateOnly end);
    }
}