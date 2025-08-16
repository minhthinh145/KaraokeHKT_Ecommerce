using QLQuanKaraokeHKT.Models;

namespace QLQuanKaraokeHKT.Repositories.BookingRepositories
{
    public interface IHoaDonRepository
    {
        /// <summary>
        /// Tạo hóa đơn dịch vụ
        /// </summary>
        Task<HoaDonDichVu> CreateHoaDonAsync(HoaDonDichVu hoaDon);

        /// <summary>
        /// Tạo chi tiết hóa đơn dịch vụ
        /// </summary>
        Task<ChiTietHoaDonDichVu> CreateChiTietHoaDonAsync(ChiTietHoaDonDichVu chiTiet);

        /// <summary>
        /// Lấy hóa đơn theo ID
        /// </summary>
        Task<HoaDonDichVu?> GetHoaDonByIdAsync(Guid maHoaDon);

 
        /// <summary>
        /// Cập nhật trạng thái hóa đơn
        /// </summary>
        Task<bool> UpdateHoaDonStatusAsync(Guid maHoaDon, string trangThai);

        /// <summary>
        /// Lấy chi tiết hóa đơn theo mã hóa đơn
        /// </summary>
        Task<List<ChiTietHoaDonDichVu>> GetChiTietHoaDonAsync(Guid maHoaDon);

        /// <summary>
        /// Lấy hóa đơn kèm chi tiết theo ID
        /// </summary>
        Task<HoaDonDichVu?> GetHoaDonWithDetailsAsync(Guid maHoaDon);

        /// <summary>
        /// Xóa chi tiết hóa đơn theo mã hóa đơn
        /// </summary>
        Task<bool> DeleteChiTietHoaDonByMaHoaDonAsync(Guid maHoaDon);
    }
}