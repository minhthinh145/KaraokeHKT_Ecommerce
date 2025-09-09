using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.HRM;
using QLQuanKaraokeHKT.Infrastructure.Data;
using QLQuanKaraokeHKT.Infrastructure.Repositories.Base;

namespace QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.HRM
{
    public class LichLamViecRepository : GenericRepository<LichLamViec,int>,ILichLamViecRepository
    {

        public LichLamViecRepository(QlkaraokeHktContext context) : base(context) { }

        public override async Task<LichLamViec?> GetByIdAsync(int maLichLamviec)
        {
            var lichLamViec = await _dbSet
                .Include(llv => llv.NhanVien)
                .Include(llv => llv.CaLamViec)
                .FirstOrDefaultAsync(llv => llv.MaLichLamViec == maLichLamviec);
            if (lichLamViec == null)
            {
                throw new ArgumentException("LichLamViec does not exist.");
            }
            return lichLamViec;
        }

        public async Task<List<LichLamViec>> GetLichLamViecByNhanVienAsync(Guid maNhanVien)
        {
            var lichLamViecs = await _dbSet
                .Where(llv => llv.MaNhanVien == maNhanVien)
                .ToListAsync();
            if (lichLamViecs == null || lichLamViecs.Count == 0)
            {
                throw new InvalidOperationException("No LichLamViec found for the specified NhanVien.");
            }
            return lichLamViecs;
        }

        public async Task<List<LichLamViec>> GetLichLamViecByRangeAsync(DateOnly start, DateOnly end)
        {
            return await _dbSet
                .Include(lv => lv.NhanVien)
                .Include(lv => lv.CaLamViec)
                .Where(lv => lv.NgayLamViec >= start && lv.NgayLamViec <= end)
                .ToListAsync();
        }

        public async Task<bool> UpdateLichLamViecForYeuCauChuyenCaAsync(int maLichLamViec, DateOnly ngayLamViecMoi, int maCaMoi)
        {
            try
            {
                var lichLamViec = await _context.LichLamViecs.FindAsync(maLichLamViec);
                if (lichLamViec == null)
                    return false;

                // Kiểm tra xung đột lịch làm việc mới
                var hasConflict = await _dbSet
                    .AnyAsync(l => l.MaNhanVien == lichLamViec.MaNhanVien &&
                                  l.NgayLamViec == ngayLamViecMoi &&
                                  l.MaCa == maCaMoi &&
                                  l.MaLichLamViec != maLichLamViec);

                if (hasConflict)
                    return false;

                lichLamViec.NgayLamViec = ngayLamViecMoi;
                lichLamViec.MaCa = maCaMoi;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}