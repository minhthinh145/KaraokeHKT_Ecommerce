using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Base;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.Booking
{
    public interface IHoaDonRepository : IGenericRepository<HoaDonDichVu, Guid>
    {
        /// <summary>
        /// Lấy hóa đơn kèm khách hàng + tài khoản navigation 
        /// </summary>
        Task<HoaDonDichVu?> GetHoaDonWithKhachHangAsync(Guid maHoaDon);

        /// <summary>
        /// Lấy hóa đơn kèm chi tiết + navigation properties (full details)
        /// </summary>
        Task<HoaDonDichVu?> GetHoaDonWithDetailsAsync(Guid maHoaDon);

        /// <summary>
        /// Lấy hóa đơn theo thuê phòng 
        /// </summary>
        Task<HoaDonDichVu?> GetHoaDonByThuePhongAsync(Guid maKhachHang, DateTime thoiGianBatDau, int toleranceMinutes = 5);

        /// <summary>
        /// Cập nhật trạng thái hóa đơn
        /// </summary>
        Task<bool> UpdateHoaDonStatusAsync(Guid maHoaDon, string trangThai);
    }
}