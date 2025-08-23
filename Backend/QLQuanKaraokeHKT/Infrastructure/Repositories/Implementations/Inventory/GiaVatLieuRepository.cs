using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Inventory;
using QLQuanKaraokeHKT.Infrastructure.Data;

namespace QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.Inventory
{
    public class GiaVatLieuRepository : IGiaVatLieuRepository
    {
        private readonly QlkaraokeHktContext _context;

        public GiaVatLieuRepository(QlkaraokeHktContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<GiaVatLieu> CreateGiaVatLieuAsync(GiaVatLieu giaVatLieu)
        {
            _context.GiaVatLieus.Add(giaVatLieu);
            await _context.SaveChangesAsync();
            return giaVatLieu;
        }

        public async Task<GiaVatLieu?> GetGiaHienTaiByMaVatLieuAsync(int maVatLieu)
        {
            return await _context.GiaVatLieus
                .Where(g => g.MaVatLieu == maVatLieu && g.TrangThai == "HieuLuc")
                .OrderByDescending(g => g.NgayApDung)
                .FirstOrDefaultAsync();
        }

        public async Task<List<GiaVatLieu>> GetGiaVatLieuByMaVatLieuAsync(int maVatLieu)
        {
            return await _context.GiaVatLieus
                .Where(g => g.MaVatLieu == maVatLieu)
                .OrderByDescending(g => g.NgayApDung)
                .ToListAsync();
        }

        public async Task<bool> UpdateGiaVatLieuStatusAsync(int maGiaVatLieu, string trangThai)
        {
            var gia = await _context.GiaVatLieus.FindAsync(maGiaVatLieu);
            if (gia == null) return false;
            gia.TrangThai = trangThai;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}