using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Models;
using QLQuanKaraokeHKT.Repositories.QLNhanSu.Interfaces;

namespace QLQuanKaraokeHKT.Repositories.QLNhanSu.Implementations
{
    public class LuongCaLamViecRepository : ILuongCaLamViecRepository
    {
        private readonly QlkaraokeHktContext _context;

        public LuongCaLamViecRepository(QlkaraokeHktContext context)
        {
            _context = context;
        }

        public async Task<LuongCaLamViec> CreateLuongCaLamViecAsync(LuongCaLamViec luongCaLamViec)
        {
            _context.LuongCaLamViecs.Add(luongCaLamViec);
            await _context.SaveChangesAsync();
            return luongCaLamViec;
        }

        public async Task<List<LuongCaLamViec>> GetAllLuongCaLamViecsAsync()
        {
            return await _context.LuongCaLamViecs
                .Include(l => l.CaLamViec)
                .ToListAsync();
        }

        public async Task<LuongCaLamViec?> GetLuongCaLamViecByIdAsync(int maLuongCaLamViec)
        {
            return await _context.LuongCaLamViecs
                .Include(l => l.CaLamViec)
                .FirstOrDefaultAsync(l => l.MaLuongCaLamViec == maLuongCaLamViec);
        }

        public async Task<LuongCaLamViec?> GetLuongCaLamViecByMaCaAsync(int maCa)
        {
            // Lấy lương default của ca này
            return await _context.LuongCaLamViecs
                .Include(l => l.CaLamViec)
                .FirstOrDefaultAsync(l => l.MaCa == maCa && l.IsDefault);
        }

        public async Task<LuongCaLamViec> UpdateLuongCaLamViecAsync(LuongCaLamViec luongCaLamViec)
        {
            _context.LuongCaLamViecs.Update(luongCaLamViec);
            await _context.SaveChangesAsync();
            return luongCaLamViec;
        }

        public async Task<bool> DeleteLuongCaLamViecAsync(int maLuongCaLamViec)
        {
            var entity = await _context.LuongCaLamViecs.FindAsync(maLuongCaLamViec);
            if (entity == null) return false;
            _context.LuongCaLamViecs.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<LuongCaLamViec?> GetLuongCaLamViecByMaCaAndDateAsync(int maCa, DateOnly ngay)
        {
            // Ưu tiên lương event
            var eventLuong = await _context.LuongCaLamViecs
                .Where(l => l.MaCa == maCa && !l.IsDefault
                    && (l.NgayApDung == null || l.NgayApDung <= ngay)
                    && (l.NgayKetThuc == null || l.NgayKetThuc >= ngay))
                .OrderByDescending(l => l.NgayApDung)
                .FirstOrDefaultAsync();

            if (eventLuong != null) return eventLuong;

            // Nếu không có event, lấy default
            return await _context.LuongCaLamViecs
                .FirstOrDefaultAsync(l => l.MaCa == maCa && l.IsDefault);
        }
    }
}
