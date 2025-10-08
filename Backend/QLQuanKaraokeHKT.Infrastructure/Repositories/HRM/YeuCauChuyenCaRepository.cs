using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Application.Repositories.HRM;
using QLQuanKaraokeHKT.Infrastructure.Data;


namespace QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.HRM
{
    public class YeuCauChuyenCaRepository : GenericRepository<YeuCauChuyenCa, int>, IYeuCauChuyenCaRepository
    {

        public YeuCauChuyenCaRepository(QlkaraokeHktContext context) : base(context) { }

        #region Query Builder 
        private IQueryable<YeuCauChuyenCa> BuidBaseQuery()
        {
            return _dbSet
                .Include(y => y.LichLamViecGoc)
                    .ThenInclude(l => l.NhanVien)
                .Include(y => y.LichLamViecGoc)
                    .ThenInclude(l => l.CaLamViec)
                .Include(y => y.CaMoi)
                .AsQueryable();

        }
        #endregion


        #region Read-only Queries
        public override async Task<List<YeuCauChuyenCa>> GetAllAsync()
        {
            return await BuidBaseQuery()
                .OrderByDescending(y => y.NgayTaoYeuCau)
                .ToListAsync();
        }

        public async Task<List<YeuCauChuyenCa>> GetYeuCauChuyenCaByNhanVienAsync(Guid maNhanVien)
        {
            return await BuidBaseQuery()
                .Where(y => y.LichLamViecGoc.MaNhanVien == maNhanVien)
                .OrderByDescending(y => y.NgayTaoYeuCau)
                .ToListAsync();
        }

        public async Task<List<YeuCauChuyenCa>> GetYeuCauChuyenCaChuaPheDuyetAsync()
        {
            return await BuidBaseQuery()
                .Where(y => !y.DaPheDuyet)
                .OrderByDescending(y => y.NgayTaoYeuCau)
                .ToListAsync();
        }

        public async Task<List<YeuCauChuyenCa>> GetYeuCauChuyenCaDaPheDuyetAsync()
        {
            return await BuidBaseQuery()
                .Where(y => y.DaPheDuyet)
                .OrderByDescending(y => y.NgayPheDuyet)
                .ToListAsync();
        }

        public async override Task<YeuCauChuyenCa?> GetByIdAsync(int maYeuCau)
        {
            return await BuidBaseQuery()
                .Include(y => y.CaMoi)
                .FirstOrDefaultAsync(y => y.MaYeuCau == maYeuCau);
        }
        #endregion
        public async Task<bool> PheDuyetYeuCauChuyenCaAsync(int maYeuCau, bool ketQuaPheDuyet, string? ghiChuPheDuyet)
        {
            var yeuCau = await _dbSet
                .FirstOrDefaultAsync(y => y.MaYeuCau == maYeuCau);

            if (yeuCau == null || yeuCau.DaPheDuyet) return false;

            yeuCau.DaPheDuyet = true;
            yeuCau.KetQuaPheDuyet = ketQuaPheDuyet;
            yeuCau.GhiChuPheDuyet = ghiChuPheDuyet;
            yeuCau.NgayPheDuyet = DateTime.Now;

            return true;
        }

        public async Task<bool> CheckConflictLichLamViecAsync(Guid maNhanVien, DateOnly ngayLamViec, int maCa)
        {
            return await _context.LichLamViecs
                .AnyAsync(l => l.MaNhanVien == maNhanVien &&
                              l.NgayLamViec == ngayLamViec &&
                              l.MaCa == maCa);
        }
    }

}