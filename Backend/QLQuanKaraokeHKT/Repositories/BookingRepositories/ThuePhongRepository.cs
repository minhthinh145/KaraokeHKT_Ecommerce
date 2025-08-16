using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Models;

namespace QLQuanKaraokeHKT.Repositories.BookingRepositories
{
    public class ThuePhongRepository : IThuePhongRepository
    {
        private readonly QlkaraokeHktContext _context;

        public ThuePhongRepository(QlkaraokeHktContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ThuePhong> CreateThuePhongAsync(ThuePhong thuePhong)
        {
            thuePhong.MaThuePhong = Guid.NewGuid();
            _context.ThuePhongs.Add(thuePhong);
            await _context.SaveChangesAsync();
            return thuePhong;
        }

        public async Task<ThuePhong?> GetThuePhongByIdAsync(Guid maThuePhong)
        {
            return await _context.ThuePhongs
                .Include(t => t.MaKhachHangNavigation)
                .Include(t => t.MaPhongNavigation)
                    .ThenInclude(p => p.MaSanPhamNavigation)
                .Include(t => t.MaPhongNavigation)
                    .ThenInclude(p => p.MaLoaiPhongNavigation)
                .FirstOrDefaultAsync(t => t.MaThuePhong == maThuePhong);
        }

        public async Task<List<ThuePhong>> GetThuePhongByKhachHangAsync(Guid maKhachHang)
        {
            return await _context.ThuePhongs
                .Include(t => t.MaPhongNavigation)
                    .ThenInclude(p => p.MaSanPhamNavigation)
                .Include(t => t.MaPhongNavigation)
                    .ThenInclude(p => p.MaLoaiPhongNavigation)
                .Where(t => t.MaKhachHang == maKhachHang)
                .OrderByDescending(t => t.ThoiGianBatDau)
                .ToListAsync();
        }

        public async Task<bool> UpdateTrangThaiAsync(Guid maThuePhong, string trangThai)
        {
            var thuePhong = await _context.ThuePhongs.FindAsync(maThuePhong);
            if (thuePhong == null) return false;

            thuePhong.TrangThai = trangThai;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateThoiGianKetThucAsync(Guid maThuePhong, DateTime thoiGianKetThuc)
        {
            var thuePhong = await _context.ThuePhongs.FindAsync(maThuePhong);
            if (thuePhong == null) return false;

            thuePhong.ThoiGianKetThuc = thoiGianKetThuc;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ThuePhong>> GetThuePhongExpiredAsync(DateTime timeLimit)
        {
            return await _context.ThuePhongs
                .Where(t => t.TrangThai == "Pending" && t.ThoiGianBatDau < timeLimit)
                .ToListAsync();
        }

        public async Task<bool> CheckPhongAvailableAsync(int maPhong, DateTime thoiGianBatDau, int soGio)
        {
            var thoiGianKetThuc = thoiGianBatDau.AddHours(soGio);

            var conflictBooking = await _context.ThuePhongs
                .Where(t => t.MaPhong == maPhong &&
                           (t.TrangThai == "DaThanhToan" || t.TrangThai == "DangSuDung") &&
                           ((t.ThoiGianBatDau < thoiGianKetThuc && t.ThoiGianKetThuc > thoiGianBatDau)))
                .AnyAsync();

            return !conflictBooking;
        }


        public async Task<LichSuSuDungPhong> CreateLichSuSuDungPhongAsync(LichSuSuDungPhong lichSu)
        {
            _context.LichSuSuDungPhongs.Add(lichSu);
            await _context.SaveChangesAsync();
            return lichSu;
        }

        public async Task<bool> UpdateLichSuSuDungPhongAsync(Guid maKhachHang, int maPhong, DateTime thoiGianKetThuc)
        {
            var lichSu = await _context.LichSuSuDungPhongs
                .FirstOrDefaultAsync(l => l.MaKhachHang == maKhachHang && l.MaPhong == maPhong);

            if (lichSu == null) return false;

            lichSu.ThoiGianKetThuc = thoiGianKetThuc;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ThuePhong?> GetThuePhongPendingByKhachHangAsync(Guid maKhachHang)
        {
            return await _context.ThuePhongs
                .Include(t => t.MaPhongNavigation)
                .FirstOrDefaultAsync(t => t.MaKhachHang == maKhachHang && t.TrangThai == "Pending");
        }

        public async Task<CaLamViec?> GetCurrentCaLamViecAsync()
        {
            var currentTime = TimeOnly.FromDateTime(DateTime.Now);
            return await _context.CaLamViecs
                .Where(c => c.GioBatDauCa <= currentTime && c.GioKetThucCa >= currentTime)
                .FirstOrDefaultAsync();
        }

        public async Task<List<ThuePhong>> GetThuePhongByKhachHangWithDetailsAsync(Guid maKhachHang)
        {
          var thuePhongs = await _context.ThuePhongs
                .Include(t => t.MaPhongNavigation)
                    .ThenInclude(p => p.MaSanPhamNavigation)
                .Include(t => t.MaPhongNavigation)
                    .ThenInclude(p => p.MaLoaiPhongNavigation)
                .Where(t => t.MaKhachHang == maKhachHang)
                .OrderByDescending(t => t.ThoiGianBatDau)
                .ToListAsync();
            return thuePhongs;
        }

        public async Task<bool> UpdateMaHoaDonAsync(Guid maThuePhong, Guid maHoaDon)
        {
            var thuePhong = await _context.ThuePhongs.FindAsync(maThuePhong);
            if (thuePhong == null) return false;
            thuePhong.MaHoaDon = maHoaDon;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}