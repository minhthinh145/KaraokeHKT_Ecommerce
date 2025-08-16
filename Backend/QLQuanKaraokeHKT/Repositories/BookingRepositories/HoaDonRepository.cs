using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Models;

namespace QLQuanKaraokeHKT.Repositories.BookingRepositories
{
    public class HoaDonRepository : IHoaDonRepository
    {
        private readonly QlkaraokeHktContext _context;

        public HoaDonRepository(QlkaraokeHktContext context)
        {
            _context = context;
        }

        public async Task<HoaDonDichVu> CreateHoaDonAsync(HoaDonDichVu hoaDon)
        {
            _context.HoaDonDichVus.Add(hoaDon);
            await _context.SaveChangesAsync();
            return hoaDon;
        }

        public async Task<ChiTietHoaDonDichVu> CreateChiTietHoaDonAsync(ChiTietHoaDonDichVu chiTiet)
        {
            _context.ChiTietHoaDonDichVus.Add(chiTiet);
            await _context.SaveChangesAsync();
            return chiTiet;
        }

        public async Task<HoaDonDichVu?> GetHoaDonByIdAsync(Guid maHoaDon)
        {
            return await _context.HoaDonDichVus
                .Include(h => h.MaKhachHangNavigation)
                    .ThenInclude(k => k.MaTaiKhoanNavigation)
                .FirstOrDefaultAsync(h => h.MaHoaDon == maHoaDon);
        }

        public async Task<HoaDonDichVu?> GetHoaDonByThuePhongAsync(Guid maKhachHang, DateTime thoiGianBatDau, int toleranceMinutes = 5)
        {
            var startTime = thoiGianBatDau.AddMinutes(-toleranceMinutes);
            var endTime = thoiGianBatDau.AddMinutes(toleranceMinutes);

            return await _context.HoaDonDichVus
                .Where(h => h.MaKhachHang == maKhachHang &&
                           h.NgayTao >= startTime &&
                           h.NgayTao <= endTime)
                .OrderByDescending(h => h.NgayTao)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateHoaDonStatusAsync(Guid maHoaDon, string trangThai)
        {
            var hoaDon = await _context.HoaDonDichVus.FindAsync(maHoaDon);
            if (hoaDon == null) return false;

            hoaDon.TrangThai = trangThai;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ChiTietHoaDonDichVu>> GetChiTietHoaDonAsync(Guid maHoaDon)
        {
            return await _context.ChiTietHoaDonDichVus
                .Include(ct => ct.MaSanPhamNavigation)
                .Where(ct => ct.MaHoaDon == maHoaDon)
                .ToListAsync();
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

        public async Task<bool> DeleteChiTietHoaDonByMaHoaDonAsync(Guid maHoaDon)
        {
            var chiTiets = await _context.ChiTietHoaDonDichVus
                .Where(ct => ct.MaHoaDon == maHoaDon)
                .ToListAsync();

            if (chiTiets.Any())
            {
                _context.ChiTietHoaDonDichVus.RemoveRange(chiTiets);
                await _context.SaveChangesAsync();
            }

            return true;
        }
    }
}