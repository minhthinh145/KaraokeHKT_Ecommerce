using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Inventory;
using QLQuanKaraokeHKT.Infrastructure.Data;
using QLQuanKaraokeHKT.Infrastructure.Repositories.Base;

namespace QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.Inventory
{
    public class MonAnRepository : GenericRepository<MonAn,int>,IMonAnRepository
    {

        public MonAnRepository(QlkaraokeHktContext context) : base(context) { }

        public async Task<MonAn?> GetMonAnByMaVatLieuAsync(int maVatLieu)
        {
            return await _context.MonAns
                .Include(m => m.MaSanPhamNavigation)
                .FirstOrDefaultAsync(m => m.MaVatLieu == maVatLieu);
        }

        public async Task UpdateSoLuongByMaVatLieuAsync(int maVatLieu, int soLuongMoi)
        {
            var monAn = await GetMonAnByMaVatLieuAsync(maVatLieu);  
            monAn!.SoLuongConLai = soLuongMoi;
            await UpdateAsync(monAn);
        }
    }
}