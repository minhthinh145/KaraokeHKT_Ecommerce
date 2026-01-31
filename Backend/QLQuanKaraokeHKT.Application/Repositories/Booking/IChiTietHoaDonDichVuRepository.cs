using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Application.Repositories;

namespace QLQuanKaraokeHKT.Application.Repositories.Booking
{
    public interface IChiTietHoaDonDichVuRepository : IGenericRepository<ChiTietHoaDonDichVu, int>
    {

        Task<List<ChiTietHoaDonDichVu>> GetChiTietByHoaDonAsync(Guid maHoaDon);

        Task<List<ChiTietHoaDonDichVu>> GetChiTietWithSanPhamAsync(Guid maHoaDon);

        Task<bool> DeleteAllByHoaDonAsync(Guid maHoaDon);

        Task<bool> UpdateSoLuongAsync(int maChiTiet, int soLuongMoi);
    }
}