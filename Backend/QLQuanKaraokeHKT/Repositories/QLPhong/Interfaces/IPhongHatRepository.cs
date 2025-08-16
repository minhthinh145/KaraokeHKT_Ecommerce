using QLQuanKaraokeHKT.Models;

namespace QLQuanKaraokeHKT.Repositories.QLPhong.Interfaces
{
    public interface IPhongHatRepository
    {
        Task<List<PhongHatKaraoke>> GetAllPhongHatWithDetailsAsync();
        Task<PhongHatKaraoke?> GetPhongHatByIdAsync(int maPhong);
        Task<PhongHatKaraoke?> GetPhongHatWithDetailsByIdAsync(int maPhong);
        Task<PhongHatKaraoke> CreatePhongHatAsync(PhongHatKaraoke phongHat);
        Task<bool> UpdatePhongHatAsync(PhongHatKaraoke phongHat);
        Task<bool> UpdateNgungHoatDongAsync(int maPhong, bool ngungHoatDong);
        Task<bool> UpdateDangSuDungAsync(int maPhong, bool dangSuDung);
        Task<List<PhongHatKaraoke>> GetPhongHatByLoaiPhongAsync(int maLoaiPhong);
    }
}