using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.QLPhongDTOs;

namespace QLQuanKaraokeHKT.Core.Interfaces.Services.Room
{
    public interface IQLPhongHatService
    {
        Task<ServiceResult> GetAllPhongHatWithDetailsAsync();
        Task<ServiceResult> GetPhongHatDetailByIdAsync(int maPhong);
        Task<ServiceResult> CreatePhongHatWithFullDetailsAsync(AddPhongHatDTO addPhongHatDto);
        Task<ServiceResult> UpdatePhongHatWithDetailsAsync(UpdatePhongHatDTO updatePhongHatDto);
        Task<ServiceResult> UpdateNgungHoatDongAsync(int maPhong, bool ngungHoatDong);
        Task<ServiceResult> UpdateDangSuDungAsync(int maPhong, bool dangSuDung);
        Task<ServiceResult> GetPhongHatByLoaiPhongAsync(int maLoaiPhong);
    }
}