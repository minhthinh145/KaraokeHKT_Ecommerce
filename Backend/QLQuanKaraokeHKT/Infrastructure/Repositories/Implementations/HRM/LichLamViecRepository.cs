using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.HRM;
using QLQuanKaraokeHKT.Infrastructure.Data;

namespace QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.HRM
{
    public class LichLamViecRepository : ILichLamViecRepository
    {
        private readonly QlkaraokeHktContext _context;
        private readonly INhanVienRepository _nhanVienRepository;

        public LichLamViecRepository(QlkaraokeHktContext context, INhanVienRepository nhanVienRepository)
        {
            _context = context;
            _nhanVienRepository = nhanVienRepository;

        }
        public async Task<LichLamViec> CreateLichLamViecWithNhanVienAsync(LichLamViec lichLamViec)
        {
            var existingNhanVien = await _nhanVienRepository.GetNhanVienByIdAsync(lichLamViec.MaNhanVien);
            if (existingNhanVien == null)
            {
                throw new ArgumentException("NhanVien does not exist.");
            }
            _context.LichLamViecs.Add(lichLamViec);
            await _context.SaveChangesAsync();
            return lichLamViec;
        }

        public async Task<bool> DeleteLichLamViecByIdAsync(int maLichLamviec)
        {
            var lichLamViec = await _context.LichLamViecs.FindAsync(maLichLamviec);
            if (lichLamViec == null)
            {
                throw new ArgumentException("LichLamViec does not exist.");
            }
            _context.LichLamViecs.Remove(lichLamViec);
            var result = await _context.SaveChangesAsync();
            if (result <= 0)
            {
                throw new InvalidOperationException("Failed to delete LichLamViec.");
            }
            return true;
        }

        public async Task<List<LichLamViec>> GetAllLichLamViecAsync()
        {
            var lichLamViecs = await _context.LichLamViecs.ToListAsync();
            if (lichLamViecs == null || lichLamViecs.Count == 0)
            {
                throw new InvalidOperationException("No LichLamViec found.");
            }
            return lichLamViecs;
        }

        public async Task<LichLamViec?> GetLichLamViecByIdAsync(int maLichLamviec)
        {
            var lichLamViec = await _context.LichLamViecs
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
            var nhanVien = await _nhanVienRepository.GetNhanVienByIdAsync(maNhanVien);
            if (nhanVien == null)
            {
                throw new ArgumentException("NhanVien does not exist.");
            }
            var lichLamViecs = await _context.LichLamViecs
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
            return await _context.LichLamViecs
                .Include(lv => lv.NhanVien)
                .Include(lv => lv.CaLamViec)
                .Where(lv => lv.NgayLamViec >= start && lv.NgayLamViec <= end)
                .ToListAsync();
        }

        public async Task<List<NhanVien>> GetNhanVienByLichLamViecRangeAsync(DateOnly start, DateOnly end)
        {
            return await _context.NhanViens
                .Include(nv => nv.LichLamViecs.Where(lv => lv.NgayLamViec >= start && lv.NgayLamViec <= end))
                    .ThenInclude(lv => lv.CaLamViec)
                .Where(nv => nv.LichLamViecs.Any(lv => lv.NgayLamViec >= start && lv.NgayLamViec <= end))
                .ToListAsync();
        }

        public async Task<bool> UpdateLichLamViecAsync(LichLamViec lichLamViec)
        {
            try
            {
                var existing = await _context.LichLamViecs.FindAsync(lichLamViec.MaLichLamViec);
                if (existing == null)
                    return false;

                // Cập nhật các trường
                existing.NgayLamViec = lichLamViec.NgayLamViec;
                existing.MaNhanVien = lichLamViec.MaNhanVien;
                existing.MaCa = lichLamViec.MaCa;

                // Lưu thay đổi
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Cập nhật lịch làm việc cho yêu cầu chuyển ca đã được duyệt
        /// </summary>
        /// <param name="maLichLamViec">Mã lịch làm việc cần cập nhật</param>
        /// <param name="ngayLamViecMoi">Ngày làm việc mới</param>
        /// <param name="maCaMoi">Mã ca mới</param>
        /// <returns>True nếu cập nhật thành công</returns>
        public async Task<bool> UpdateLichLamViecForYeuCauChuyenCaAsync(int maLichLamViec, DateOnly ngayLamViecMoi, int maCaMoi)
        {
            try
            {
                var lichLamViec = await _context.LichLamViecs.FindAsync(maLichLamViec);
                if (lichLamViec == null)
                    return false;

                // Kiểm tra xung đột lịch làm việc mới
                var hasConflict = await _context.LichLamViecs
                    .AnyAsync(l => l.MaNhanVien == lichLamViec.MaNhanVien &&
                                  l.NgayLamViec == ngayLamViecMoi &&
                                  l.MaCa == maCaMoi &&
                                  l.MaLichLamViec != maLichLamViec);

                if (hasConflict)
                    return false;

                // Cập nhật thông tin
                lichLamViec.NgayLamViec = ngayLamViecMoi;
                lichLamViec.MaCa = maCaMoi;

                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}