using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Application.Repositories;

namespace QLQuanKaraokeHKT.Application.Repositories.Booking
{
    public interface IHoaDonRepository : IGenericRepository<HoaDonDichVu, Guid>
    {
        Task<HoaDonDichVu?> GetHoaDonWithKhachHangAsync(Guid maHoaDon);

        Task<HoaDonDichVu?> GetHoaDonWithDetailsAsync(Guid maHoaDon);

        Task<HoaDonDichVu?> GetHoaDonByThuePhongAsync(Guid maKhachHang, DateTime thoiGianBatDau, int toleranceMinutes = 5);

        Task<bool> UpdateHoaDonStatusAsync(Guid maHoaDon, string trangThai);
    }
}