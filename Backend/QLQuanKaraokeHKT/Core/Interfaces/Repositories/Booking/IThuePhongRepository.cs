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

        /// <summary>
        /// Tạo lịch sử sử dụng phòng
        /// </summary>
        Task<LichSuSuDungPhong> CreateLichSuSuDungPhongAsync(LichSuSuDungPhong lichSu);

        /// <summary>
        /// Cập nhật lịch sử sử dụng phòng
        /// </summary>
        Task<bool> UpdateLichSuSuDungPhongAsync(Guid maKhachHang, int maPhong, DateTime thoiGianKetThuc);

        /// <summary>
        /// Lấy ThuePhong pending theo khách hàng
        /// </summary>
        Task<ThuePhong?> GetThuePhongPendingByKhachHangAsync(Guid maKhachHang);

        /// <summary>
        /// Lấy ca làm việc hiện tại
        /// </summary>
        Task<CaLamViec?> GetCurrentCaLamViecAsync();

        Task<bool> UpdateMaHoaDonAsync(Guid maThuePhong, Guid maHoaDon);
    }
}