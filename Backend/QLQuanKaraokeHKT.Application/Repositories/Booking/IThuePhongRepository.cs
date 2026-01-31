using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Application.Repositories;

namespace QLQuanKaraokeHKT.Application.Repositories.Booking
{
    public interface IThuePhongRepository : IGenericRepository<ThuePhong, Guid>
    {
        Task<List<ThuePhong>> GetThuePhongByKhachHangAsync(Guid maKhachHang);
        Task<List<ThuePhong>> GetThuePhongByKhachHangWithDetailsAsync(Guid maKhachHang);
        Task<bool> UpdateTrangThaiAsync(Guid maThuePhong, string trangThai);
        Task<bool> UpdateThoiGianKetThucAsync(Guid maThuePhong, DateTime thoiGianKetThuc);
        Task<List<ThuePhong>> GetThuePhongExpiredAsync(DateTime timeLimit);
        Task<bool> CheckPhongAvailableAsync(int maPhong, DateTime thoiGianBatDau, int soGio);

        Task<CaLamViec?> GetCurrentCaLamViecAsync();

        Task<bool> UpdateMaHoaDonAsync(Guid maThuePhong, Guid maHoaDon);
    }
}