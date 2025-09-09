using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Inventory;
using QLQuanKaraokeHKT.Infrastructure.Data;
using QLQuanKaraokeHKT.Infrastructure.Repositories.Base;

namespace QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.Inventory
{
    public class GiaVatLieuRepository : GenericRepository<GiaVatLieu,int>,IGiaVatLieuRepository
    {

        public GiaVatLieuRepository(QlkaraokeHktContext context) : base(context) { }

        public async Task DisableCurrentPricesAsync(int maVatLieu, string newTrangThai)
        {
            var giaHienTai = await GetGiaHienTaiByMaVatLieuAsync(maVatLieu);

            giaHienTai!.TrangThai = newTrangThai;

            await UpdateAsync(giaHienTai);
        }

        public async Task<GiaVatLieu?> GetGiaHienTaiByMaVatLieuAsync(int maVatLieu)
        {
            return await _dbSet
                .Where(g => g.MaVatLieu == maVatLieu && g.TrangThai == "HieuLuc")
                .FirstOrDefaultAsync();
        }

        public async Task<List<GiaVatLieu>> GetGiaVatLieuByMaVatLieuAsync(int maVatLieu)
        {
            return await _dbSet
                .Where(g => g.MaVatLieu == maVatLieu)
                .OrderByDescending(g => g.NgayApDung)
                .ToListAsync();
        }     
    }
}