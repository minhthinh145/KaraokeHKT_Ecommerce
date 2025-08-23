using QLQuanKaraokeHKT.Core.Entities;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.Room
{
    public interface ILoaiPhongRepository
    {
        Task<List<LoaiPhongHatKaraoke>> GetAllLoaiPhongAsync();
        Task<LoaiPhongHatKaraoke?> GetLoaiPhongByIdAsync(int maLoaiPhong);
        Task<LoaiPhongHatKaraoke> CreateLoaiPhongAsync(LoaiPhongHatKaraoke loaiPhong);
        Task<bool> UpdateLoaiPhongAsync(LoaiPhongHatKaraoke loaiPhong);
        Task<bool> DeleteLoaiPhongAsync(int maLoaiPhong);
        Task<bool> HasPhongHatKaraokeAsync(int maLoaiPhong);
    }
}
