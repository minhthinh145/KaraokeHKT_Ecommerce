using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Base;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.Booking
{
    public interface IChiTietHoaDonDichVuRepository : IGenericRepository<ChiTietHoaDonDichVu, int>
    {

        Task<List<ChiTietHoaDonDichVu>> GetChiTietByHoaDonAsync(Guid maHoaDon);

        Task<List<ChiTietHoaDonDichVu>> GetChiTietWithSanPhamAsync(Guid maHoaDon);

        Task<bool> DeleteAllByHoaDonAsync(Guid maHoaDon);

        Task<bool> UpdateSoLuongAsync(int maChiTiet, int soLuongMoi);
    }
}