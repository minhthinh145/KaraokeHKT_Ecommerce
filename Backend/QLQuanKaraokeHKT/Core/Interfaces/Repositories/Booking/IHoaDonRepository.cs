using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Base;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.Booking
{
    public interface IHoaDonRepository : IGenericRepository<HoaDonDichVu, Guid>
    {
        Task<HoaDonDichVu?> GetHoaDonWithKhachHangAsync(Guid maHoaDon);

        Task<HoaDonDichVu?> GetHoaDonWithDetailsAsync(Guid maHoaDon);

        Task<HoaDonDichVu?> GetHoaDonByThuePhongAsync(Guid maKhachHang, DateTime thoiGianBatDau, int toleranceMinutes = 5);

        Task<bool> UpdateHoaDonStatusAsync(Guid maHoaDon, string trangThai);
    }
}