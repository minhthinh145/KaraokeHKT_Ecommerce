using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Room;
using QLQuanKaraokeHKT.Infrastructure.Data;

namespace QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.Room
{
    public class PhongHatRepository : IPhongHatRepository
    {
        private readonly QlkaraokeHktContext _context;

        public PhongHatRepository(QlkaraokeHktContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<PhongHatKaraoke>> GetAllPhongHatWithDetailsAsync()
        {
            return await _context.PhongHatKaraokes
                .Include(p => p.MaLoaiPhongNavigation)
                .Include(p => p.MaSanPhamNavigation)
                    .ThenInclude(sp => sp.GiaDichVus)
                        .ThenInclude(gdv => gdv.MaCaNavigation)
                .OrderByDescending(p => p.MaPhong) 
                .ToListAsync();
        }

        public async Task<PhongHatKaraoke?> GetPhongHatByIdAsync(int maPhong)
        {
            return await _context.PhongHatKaraokes.FindAsync(maPhong);
        }

        public async Task<PhongHatKaraoke?> GetPhongHatWithDetailsByIdAsync(int maPhong)
        {
            return await _context.PhongHatKaraokes
                .Include(p => p.MaLoaiPhongNavigation)
                .Include(p => p.MaSanPhamNavigation)
                    .ThenInclude(sp => sp.GiaDichVus)
                        .ThenInclude(gdv => gdv.MaCaNavigation) // ✅ THÊM INCLUDE CHO MaCaNavigation
                .FirstOrDefaultAsync(p => p.MaPhong == maPhong);
        }

        public async Task<PhongHatKaraoke> CreatePhongHatAsync(PhongHatKaraoke phongHat)
        {
            _context.PhongHatKaraokes.Add(phongHat);
            await _context.SaveChangesAsync();
            return phongHat;
        }

        public async Task<bool> UpdatePhongHatAsync(PhongHatKaraoke phongHat)
        {
            var existing = await _context.PhongHatKaraokes.FindAsync(phongHat.MaPhong);
            if (existing == null) return false;

            existing.MaLoaiPhong = phongHat.MaLoaiPhong;
            existing.MaSanPham = phongHat.MaSanPham;
            existing.DangSuDung = phongHat.DangSuDung;
            existing.NgungHoatDong = phongHat.NgungHoatDong;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateNgungHoatDongAsync(int maPhong, bool ngungHoatDong)
        {
            var phongHat = await _context.PhongHatKaraokes.FindAsync(maPhong);
            if (phongHat == null) return false;

            phongHat.NgungHoatDong = ngungHoatDong;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateDangSuDungAsync(int maPhong, bool dangSuDung)
        {
            var phongHat = await _context.PhongHatKaraokes.FindAsync(maPhong);
            if (phongHat == null) return false;

            phongHat.DangSuDung = dangSuDung;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<PhongHatKaraoke>> GetPhongHatByLoaiPhongAsync(int maLoaiPhong)
        {
            return await _context.PhongHatKaraokes
                .Include(p => p.MaLoaiPhongNavigation)
                .Include(p => p.MaSanPhamNavigation)
                    .ThenInclude(sp => sp.GiaDichVus)
                        .ThenInclude(gdv => gdv.MaCaNavigation) // ✅ THÊM INCLUDE CHO MaCaNavigation
                .Where(p => p.MaLoaiPhong == maLoaiPhong)
                .OrderBy(p => p.MaPhong)
                .ToListAsync();
        }
    }
}