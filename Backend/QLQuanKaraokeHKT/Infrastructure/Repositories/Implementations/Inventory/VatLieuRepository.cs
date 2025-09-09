using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Inventory;
using QLQuanKaraokeHKT.Infrastructure.Data;
using QLQuanKaraokeHKT.Infrastructure.Repositories.Base;
using System.Reflection.Metadata.Ecma335;

namespace QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.Inventory
{
    public class VatLieuRepository : GenericRepository<VatLieu, int>, IVatLieuRepository
    {

        public VatLieuRepository(QlkaraokeHktContext context) : base(context) { }

        #region Query Builder
        private IQueryable<VatLieu> BuildBaseQuery(bool includePricing = false)
        {
            var query = _dbSet
                .Include(v => v.MonAn)
                    .ThenInclude(m => m.MaSanPhamNavigation)
                .Include(v => v.GiaVatLieus.Where(g => g.TrangThai == "HieuLuc")) 
                .AsQueryable();

            if (includePricing)
            {
                query = query
                    .Include(v => v.MonAn)
                        .ThenInclude(m => m.MaSanPhamNavigation)
                            .ThenInclude(sp => sp.GiaDichVus
                                .Where(g => g.TrangThai == "HieuLuc")) 
                                    .ThenInclude(g => g.MaCaNavigation);
            }

            return query;
        }
        #endregion


        #region Read-Only Queries (NoTracking)
        public async Task<List<VatLieu>> GetAllVatLieuWithDetailsAsync(bool includePricing = false)
        {
            return await BuildBaseQuery(includePricing)
                .AsNoTracking()
                .OrderBy(v => v.TenVatLieu)
                .ToListAsync();
        }

        public async Task<VatLieu?> GetVatLieuWithDetailsByIdAsync(int maVatLieu, bool includePricing = false)
        {
            return await BuildBaseQuery(includePricing)
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.MaVatLieu == maVatLieu);
        }
        #endregion

        public async Task<VatLieu?> GetVatLieuWithDetailsByIdAsync(int maVatLieu)
        {
            return await BuildBaseQuery().FirstOrDefaultAsync(v => v.MaVatLieu == maVatLieu);
        }

        public async Task<VatLieu?> GetVatLieuForUpdateAsync(int maVatLieu)
        {
            return await _dbSet
                .Include(v => v.MonAn)
                    .ThenInclude(m => m.MaSanPhamNavigation)
                .Include(v => v.GiaVatLieus)
                .FirstOrDefaultAsync(v => v.MaVatLieu == maVatLieu);
        }

        public async Task<bool> UpdateSoLuongVatLieuAsync(int maVatLieu, int soLuongMoi)
        {
            var vatLieu = await _context.VatLieus.FindAsync(maVatLieu);
            if (vatLieu == null) return false;

            vatLieu.SoLuongTonKho = soLuongMoi;
            return true;
        }

        public async Task<bool> UpdateNgungCungCapAsync(int maVatLieu, bool ngungCungCap)
        {
            var vatLieu = await _context.VatLieus.FindAsync(maVatLieu);
            if (vatLieu == null) return false;

            vatLieu.NgungCungCap = ngungCungCap;
            return true;
        }
    }
}