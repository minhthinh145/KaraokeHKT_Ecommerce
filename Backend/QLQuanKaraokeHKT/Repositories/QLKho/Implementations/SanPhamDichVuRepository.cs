using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Models;
using QLQuanKaraokeHKT.Repositories.QLKho.Interfaces;

namespace QLQuanKaraokeHKT.Repositories.QLKho.Implementations
{
    public class SanPhamDichVuRepository : ISanPhamDichVuRepository
    {
        private readonly QlkaraokeHktContext _context;

        public SanPhamDichVuRepository(QlkaraokeHktContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<SanPhamDichVu> CreateSanPhamDichVuAsync(SanPhamDichVu sanPham)
        {
            _context.SanPhamDichVus.Add(sanPham);
            await _context.SaveChangesAsync();
            return sanPham;
        }

        public async Task<SanPhamDichVu?> GetSanPhamDichVuByIdAsync(int maSanPham)
        {
            return await _context.SanPhamDichVus.FindAsync(maSanPham);
        }

        public async Task<bool> UpdateSanPhamDichVuAsync(SanPhamDichVu sanPham)
        {
            var existing = await _context.SanPhamDichVus.FindAsync(sanPham.MaSanPham);
            if (existing == null) return false;

            existing.TenSanPham = sanPham.TenSanPham;
            existing.HinhAnhSanPham = sanPham.HinhAnhSanPham;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}