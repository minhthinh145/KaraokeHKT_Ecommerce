using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Booking;
using QLQuanKaraokeHKT.Infrastructure.Data;
using QLQuanKaraokeHKT.Infrastructure.Repositories.Base;

namespace QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.Booking
{
    public class HoaDonRepository : GenericRepository<HoaDonDichVu, Guid>, IHoaDonRepository
    {
        public HoaDonRepository(QlkaraokeHktContext context) : base(context)
        {
        }

        public override async Task<HoaDonDichVu?> GetByIdAsync(Guid id)
        {
            return await _context.HoaDonDichVus
                .Include(h => h.MaKhachHangNavigation)
                .FirstOrDefaultAsync(h => h.MaHoaDon == id);
        }

        public async Task<HoaDonDichVu?> GetHoaDonWithKhachHangAsync(Guid maHoaDon)
        {
            return await _context.HoaDonDichVus
                .Include(h => h.MaKhachHangNavigation)
                    .ThenInclude(k => k.MaTaiKhoanNavigation)
                .FirstOrDefaultAsync(h => h.MaHoaDon == maHoaDon);
        }

        public async Task<HoaDonDichVu?> GetHoaDonWithDetailsAsync(Guid maHoaDon)
        {
            return await _context.HoaDonDichVus
                .Include(h => h.MaKhachHangNavigation)
                    .ThenInclude(k => k.MaTaiKhoanNavigation)
                .Include(h => h.ChiTietHoaDonDichVus)
                    .ThenInclude(ct => ct.MaSanPhamNavigation)
                .FirstOrDefaultAsync(h => h.MaHoaDon == maHoaDon);
        }

        public async Task<HoaDonDichVu?> GetHoaDonByThuePhongAsync(Guid maKhachHang, DateTime thoiGianBatDau, int toleranceMinutes = 5)
        {
            var startTime = thoiGianBatDau.AddMinutes(-toleranceMinutes);
            var endTime = thoiGianBatDau.AddMinutes(toleranceMinutes);

            return await _context.HoaDonDichVus
                .Include(h => h.MaKhachHangNavigation)
                .Where(h => h.MaKhachHang == maKhachHang &&
                           h.NgayTao >= startTime &&
                           h.NgayTao <= endTime)
                .OrderByDescending(h => h.NgayTao)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateHoaDonStatusAsync(Guid maHoaDon, string trangThai)
        {
            var hoaDon = await _dbSet.FindAsync(maHoaDon);
            if (hoaDon == null) return false;

            hoaDon.TrangThai = trangThai;
            return await UpdateAsync(hoaDon);
        }
    }
}