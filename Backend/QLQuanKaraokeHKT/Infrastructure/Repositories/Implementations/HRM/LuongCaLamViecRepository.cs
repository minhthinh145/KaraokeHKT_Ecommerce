using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.HRM;
using QLQuanKaraokeHKT.Infrastructure.Data;
using QLQuanKaraokeHKT.Infrastructure.Repositories.Base;

namespace QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.HRM
{
    public class LuongCaLamViecRepository : GenericRepository<LuongCaLamViec,int>,ILuongCaLamViecRepository
    {

        public LuongCaLamViecRepository(QlkaraokeHktContext context) : base(context) { }

        public override async Task<List<LuongCaLamViec>> GetAllAsync()
        {
            return await _dbSet
                .Include(l => l.CaLamViec)
                .ToListAsync();
        }

        public async Task<LuongCaLamViec?> GetLuongCaLamViecByIdAsync(int maLuongCaLamViec)
        {
            return await _dbSet
                .Include(l => l.CaLamViec)
                .FirstOrDefaultAsync(l => l.MaLuongCaLamViec == maLuongCaLamViec);
        }

        public async Task<LuongCaLamViec?> GetLuongCaLamViecByMaCaAsync(int maCa)
        {

            return await _dbSet
                .Include(l => l.CaLamViec)
                .FirstOrDefaultAsync(l => l.MaCa == maCa && l.IsDefault);
        }

        public async Task<LuongCaLamViec?> GetLuongCaLamViecByMaCaAndDateAsync(int maCa, DateOnly ngay)
        {
            // Ưu tiên lương event
            var eventLuong = await _dbSet
                .Where(l => l.MaCa == maCa && !l.IsDefault
                    && (l.NgayApDung == null || l.NgayApDung <= ngay)
                    && (l.NgayKetThuc == null || l.NgayKetThuc >= ngay))
                .OrderByDescending(l => l.NgayApDung)
                .FirstOrDefaultAsync();

            if (eventLuong != null) return eventLuong;

            // Nếu không có event, lấy default
            return await _dbSet
                .FirstOrDefaultAsync(l => l.MaCa == maCa && l.IsDefault);
        }
    }
}
