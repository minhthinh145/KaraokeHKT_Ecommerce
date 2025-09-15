using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Booking;
using QLQuanKaraokeHKT.Infrastructure.Data;
using QLQuanKaraokeHKT.Infrastructure.Repositories.Base;

namespace QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.Booking
{
    public class ChiTietHoaDonDichVuRepository : GenericRepository<ChiTietHoaDonDichVu, int>, IChiTietHoaDonDichVuRepository
    {
        public ChiTietHoaDonDichVuRepository(QlkaraokeHktContext context) : base(context) { }

        public async Task<List<ChiTietHoaDonDichVu>> GetChiTietByHoaDonAsync(Guid maHoaDon)
        {
            return await _context.ChiTietHoaDonDichVus
                .Where(ct => ct.MaHoaDon == maHoaDon)
                .ToListAsync();
        }

        public async Task<List<ChiTietHoaDonDichVu>> GetChiTietWithSanPhamAsync(Guid maHoaDon)
        {
            return await _context.ChiTietHoaDonDichVus
                .Include(ct => ct.MaSanPhamNavigation)
                .Where(ct => ct.MaHoaDon == maHoaDon)
                .ToListAsync();
        }

        public async Task<bool> DeleteAllByHoaDonAsync(Guid maHoaDon)
        {
            var chiTiets = await _context.ChiTietHoaDonDichVus
                .Where(ct => ct.MaHoaDon == maHoaDon)
                .ToListAsync();

            if (chiTiets.Any())
            {
                _context.ChiTietHoaDonDichVus.RemoveRange(chiTiets);
            }

            return true;
        }

        public async Task<bool> UpdateSoLuongAsync(int maChiTiet, int soLuongMoi)
        {
            var chiTiet = await GetByIdAsync(maChiTiet);
            if (chiTiet == null) return false;

            chiTiet.SoLuong = soLuongMoi;
            return await UpdateAsync(chiTiet);
        }

    }
}
