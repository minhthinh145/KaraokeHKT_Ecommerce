using QLQuanKaraokeHKT.DTOs.QLPhongDTOs;
using QLQuanKaraokeHKT.Helpers;

namespace QLQuanKaraokeHKT.Services.QLPhongServices
{
    public interface IQLLoaiPhongService
    {
        Task<ServiceResult> GetAllLoaiPhongAsync();
        Task<ServiceResult> GetLoaiPhongByIdAsync(int maLoaiPhong);
        Task<ServiceResult> CreateLoaiPhongAsync(AddLoaiPhongDTO addLoaiPhongDto);
        Task<ServiceResult> UpdateLoaiPhongAsync(LoaiPhongDTO loaiPhongDto);
        Task<ServiceResult> DeleteLoaiPhongAsync(int maLoaiPhong);
    }
}