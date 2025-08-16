using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Models;
using QLQuanKaraokeHKT.Repositories.QLPhong.Interfaces;

namespace QLQuanKaraokeHKT.Repositories.QLPhong.Implementations
{
    public class LoaiPhongRepository : ILoaiPhongRepository
    {
        private readonly QlkaraokeHktContext _context;

        public LoaiPhongRepository(QlkaraokeHktContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<LoaiPhongHatKaraoke>> GetAllLoaiPhongAsync()
        {
            return await _context.LoaiPhongHatKaraokes
                .OrderByDescending(lp => lp.SucChua)
                .ToListAsync();
        }

        public async Task<LoaiPhongHatKaraoke?> GetLoaiPhongByIdAsync(int maLoaiPhong)
        {
            return await _context.LoaiPhongHatKaraokes.FindAsync(maLoaiPhong);
        }

        public async Task<LoaiPhongHatKaraoke> CreateLoaiPhongAsync(LoaiPhongHatKaraoke loaiPhong)
        {
            _context.LoaiPhongHatKaraokes.Add(loaiPhong);
            await _context.SaveChangesAsync();
            return loaiPhong;
        }

        public async Task<bool> UpdateLoaiPhongAsync(LoaiPhongHatKaraoke loaiPhong)
        {
            var existing = await _context.LoaiPhongHatKaraokes.FindAsync(loaiPhong.MaLoaiPhong);
            if (existing == null) return false;

            existing.TenLoaiPhong = loaiPhong.TenLoaiPhong;
            existing.SucChua = loaiPhong.SucChua;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteLoaiPhongAsync(int maLoaiPhong)
        {
            var loaiPhong = await _context.LoaiPhongHatKaraokes.FindAsync(maLoaiPhong);
            if (loaiPhong == null) return false;

            _context.LoaiPhongHatKaraokes.Remove(loaiPhong);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HasPhongHatKaraokeAsync(int maLoaiPhong)
        {
            return await _context.PhongHatKaraokes
                .AnyAsync(p => p.MaLoaiPhong == maLoaiPhong);
        }
    }
}