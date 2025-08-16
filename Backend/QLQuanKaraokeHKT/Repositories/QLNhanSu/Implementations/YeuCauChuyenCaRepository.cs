using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Models;
using QLQuanKaraokeHKT.Repositories.QLNhanSu.Interfaces;

namespace QLQuanKaraokeHKT.Repositories.QLNhanSu.Implementations
{
    public class YeuCauChuyenCaRepository : IYeuCauChuyenCaRepository
    {
        private readonly QlkaraokeHktContext _context;

        public YeuCauChuyenCaRepository(QlkaraokeHktContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<YeuCauChuyenCa> CreateYeuCauChuyenCaAsync(YeuCauChuyenCa yeuCauChuyenCa)
        {
            _context.YeuCauChuyenCas.Add(yeuCauChuyenCa);
            await _context.SaveChangesAsync();
            return yeuCauChuyenCa;
        }

        public async Task<List<YeuCauChuyenCa>> GetAllYeuCauChuyenCaAsync()
        {
            return await _context.YeuCauChuyenCas
                .Include(y => y.LichLamViecGoc)
                    .ThenInclude(l => l.NhanVien)
                .Include(y => y.LichLamViecGoc)
                    .ThenInclude(l => l.CaLamViec)
                .Include(y => y.CaMoi)
                .OrderByDescending(y => y.NgayTaoYeuCau)
                .ToListAsync();
        }

        public async Task<List<YeuCauChuyenCa>> GetYeuCauChuyenCaByNhanVienAsync(Guid maNhanVien)
        {
            return await _context.YeuCauChuyenCas
                .Include(y => y.LichLamViecGoc)
                    .ThenInclude(l => l.NhanVien)
                .Include(y => y.LichLamViecGoc)
                    .ThenInclude(l => l.CaLamViec)
                .Include(y => y.CaMoi)
                .Where(y => y.LichLamViecGoc.MaNhanVien == maNhanVien)
                .OrderByDescending(y => y.NgayTaoYeuCau)
                .ToListAsync();
        }

        public async Task<List<YeuCauChuyenCa>> GetYeuCauChuyenCaChuaPheDuyetAsync()
        {
            return await _context.YeuCauChuyenCas
                .Include(y => y.LichLamViecGoc)
                    .ThenInclude(l => l.NhanVien)
                .Include(y => y.LichLamViecGoc)
                    .ThenInclude(l => l.CaLamViec)
                .Include(y => y.CaMoi)
                .Where(y => !y.DaPheDuyet)
                .OrderBy(y => y.NgayTaoYeuCau)
                .ToListAsync();
        }

        public async Task<List<YeuCauChuyenCa>> GetYeuCauChuyenCaDaPheDuyetAsync()
        {
            return await _context.YeuCauChuyenCas
                .Include(y => y.LichLamViecGoc)
                    .ThenInclude(l => l.NhanVien)
                .Include(y => y.LichLamViecGoc)
                    .ThenInclude(l => l.CaLamViec)
                .Include(y => y.CaMoi)
                .Where(y => y.DaPheDuyet)
                .OrderByDescending(y => y.NgayPheDuyet)
                .ToListAsync();
        }

        public async Task<YeuCauChuyenCa?> GetYeuCauChuyenCaByIdAsync(int maYeuCau)
        {
            return await _context.YeuCauChuyenCas
                .Include(y => y.LichLamViecGoc)
                    .ThenInclude(l => l.NhanVien)
                .Include(y => y.LichLamViecGoc)
                    .ThenInclude(l => l.CaLamViec)
                .Include(y => y.CaMoi)
                .FirstOrDefaultAsync(y => y.MaYeuCau == maYeuCau);
        }

        public async Task<bool> UpdateYeuCauChuyenCaAsync(YeuCauChuyenCa yeuCauChuyenCa)
        {
            var existing = await _context.YeuCauChuyenCas.FindAsync(yeuCauChuyenCa.MaYeuCau);
            if (existing == null) return false;

            existing.NgayLamViecMoi = yeuCauChuyenCa.NgayLamViecMoi;
            existing.MaCaMoi = yeuCauChuyenCa.MaCaMoi;
            existing.LyDoChuyenCa = yeuCauChuyenCa.LyDoChuyenCa;
            existing.DaPheDuyet = yeuCauChuyenCa.DaPheDuyet;
            existing.KetQuaPheDuyet = yeuCauChuyenCa.KetQuaPheDuyet;
            existing.GhiChuPheDuyet = yeuCauChuyenCa.GhiChuPheDuyet;
            existing.NgayPheDuyet = yeuCauChuyenCa.NgayPheDuyet;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> PheDuyetYeuCauChuyenCaAsync(int maYeuCau, bool ketQuaPheDuyet, string? ghiChuPheDuyet)
        {
            var yeuCau = await _context.YeuCauChuyenCas
                .FirstOrDefaultAsync(y => y.MaYeuCau == maYeuCau);

            if (yeuCau == null || yeuCau.DaPheDuyet) return false;

            yeuCau.DaPheDuyet = true;
            yeuCau.KetQuaPheDuyet = ketQuaPheDuyet;
            yeuCau.GhiChuPheDuyet = ghiChuPheDuyet;
            yeuCau.NgayPheDuyet = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CheckConflictLichLamViecAsync(Guid maNhanVien, DateOnly ngayLamViec, int maCa)
        {
            return await _context.LichLamViecs
                .AnyAsync(l => l.MaNhanVien == maNhanVien &&
                              l.NgayLamViec == ngayLamViec &&
                              l.MaCa == maCa);
        }

        public async Task<bool> DeleteYeuCauChuyenCaAsync(int maYeuCau)
        {
            var yeuCau = await _context.YeuCauChuyenCas.FindAsync(maYeuCau);
            if (yeuCau == null) return false;

            _context.YeuCauChuyenCas.Remove(yeuCau);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}