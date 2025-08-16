using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Models;
using QLQuanKaraokeHKT.Repositories.QLKho.Interfaces;

namespace QLQuanKaraokeHKT.Repositories.QLKho.Implementations
{
    public class MonAnRepository : IMonAnRepository
    {
        private readonly QlkaraokeHktContext _context;

        public MonAnRepository(QlkaraokeHktContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<MonAn> CreateMonAnAsync(MonAn monAn)
        {
            _context.MonAns.Add(monAn);
            await _context.SaveChangesAsync();
            return monAn;
        }

        public async Task<MonAn?> GetMonAnByMaVatLieuAsync(int maVatLieu)
        {
            return await _context.MonAns
                .Include(m => m.MaSanPhamNavigation)
                .FirstOrDefaultAsync(m => m.MaVatLieu == maVatLieu);
        }

        public async Task<bool> UpdateSoLuongMonAnAsync(int maVatLieu, int soLuongMoi)
        {
            var monAn = await _context.MonAns
                .FirstOrDefaultAsync(m => m.MaVatLieu == maVatLieu);

            if (monAn == null) return false;

            monAn.SoLuongConLai = soLuongMoi;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}