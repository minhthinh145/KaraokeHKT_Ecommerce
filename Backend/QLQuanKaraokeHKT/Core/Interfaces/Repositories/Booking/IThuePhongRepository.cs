using QLQuanKaraokeHKT.Core.Entities;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.Booking
{
    public interface IThuePhongRepository
    {
        Task<ThuePhong> CreateThuePhongAsync(ThuePhong thuePhong);
        Task<ThuePhong?> GetThuePhongByIdAsync(Guid maThuePhong);
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