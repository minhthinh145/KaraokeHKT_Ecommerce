using QLQuanKaraokeHKT.DTOs.QLPhongDTOs;
using QLQuanKaraokeHKT.Helpers;

namespace QLQuanKaraokeHKT.Services.QLPhongServices
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