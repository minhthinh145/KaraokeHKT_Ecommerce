using QLQuanKaraokeHKT.Models;

namespace QLQuanKaraokeHKT.Repositories.BookingRepositories
{
    public interface ILichSuSuDungPhongRepository
    {
        Task<List<LichSuSuDungPhong>> GetLichSuByKhachHangAsync(Guid maKhachHang);
        Task<LichSuSuDungPhong> GetLichSuByIdAsync(int idLichSu);
        Task<LichSuSuDungPhong> CreateLichSuAsync(LichSuSuDungPhong lichSu);
        Task<List<LichSuSuDungPhong>> GetLichSuWithDetailsAsync(Guid maKhachHang);
        Task<LichSuSuDungPhong> GetLichSuByMaThuePhongAsync(Guid maThuePhong);
    }
}