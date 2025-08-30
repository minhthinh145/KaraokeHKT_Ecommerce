using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Base;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.Booking
{
    public interface IChiTietHoaDonDichVuRepository : IGenericRepository<ChiTietHoaDonDichVu, int>
    {
        /// <summary>
        /// Lấy chi tiết hóa đơn theo mã hóa đơn
        /// </summary>
        Task<List<ChiTietHoaDonDichVu>> GetChiTietByHoaDonAsync(Guid maHoaDon);

        /// <summary>
        /// Lấy chi tiết hóa đơn kèm sản phẩm navigation
        /// </summary>
        Task<List<ChiTietHoaDonDichVu>> GetChiTietWithSanPhamAsync(Guid maHoaDon);

        /// <summary>
        /// Xóa tất cả chi tiết theo mã hóa đơn
        /// </summary>
        Task<bool> DeleteAllByHoaDonAsync(Guid maHoaDon);

        /// <summary>
        /// Cập nhật số lượng chi tiết hóa đơn
        /// </summary>
        Task<bool> UpdateSoLuongAsync(int maChiTiet, int soLuongMoi);
    }
}