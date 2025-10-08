using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Application.Repositories;

namespace QLQuanKaraokeHKT.Application.Repositories.Booking
{
    public interface ILichSuSuDungPhongRepository : IGenericRepository<LichSuSuDungPhong, int>
    {
        Task<List<LichSuSuDungPhong>> GetLichSuByKhachHangAsync(Guid maKhachHang);
        Task<LichSuSuDungPhong> GetLichSuByIdAsync(int idLichSu);
        Task<LichSuSuDungPhong> CreateLichSuAsync(LichSuSuDungPhong lichSu);
        Task<List<LichSuSuDungPhong>> GetLichSuWithDetailsAsync(Guid maKhachHang);
        Task<LichSuSuDungPhong> GetLichSuByMaThuePhongAsync(Guid maThuePhong);
    }
}