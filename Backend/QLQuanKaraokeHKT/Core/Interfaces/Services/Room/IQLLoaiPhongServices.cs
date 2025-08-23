using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.QLPhongDTOs;

namespace QLQuanKaraokeHKT.Core.Interfaces.Services.Room
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