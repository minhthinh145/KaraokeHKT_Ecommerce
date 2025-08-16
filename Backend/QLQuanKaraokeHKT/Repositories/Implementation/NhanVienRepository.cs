using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using QLQuanKaraokeHKT.Models;
using QLQuanKaraokeHKT.Repositories.Interfaces;

namespace QLQuanKaraokeHKT.Repositories.Implementation
{
    public class NhanVienRepository : INhanVienRepository
    {
        private readonly QlkaraokeHktContext _context;

        public NhanVienRepository(QlkaraokeHktContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<NhanVien> CreateNhanVienAsync(NhanVien nhanVien)
        {
            await _context.NhanViens.AddAsync(nhanVien);
            await _context.SaveChangesAsync();
            return nhanVien;
        }

        public async Task<bool> DeleteNhanVienByIdAsync(Guid maNhanVien)
        {
            var nhanVien = await _context.NhanViens.FindAsync(maNhanVien);
            if (nhanVien == null) return false;
            await _context.NhanViens.Remove(nhanVien).Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteNhanVienByTaiKhoanIdAsync(Guid maTaiKhoan)
        {
            var nhanVien = await _context.NhanViens
                .FirstOrDefaultAsync(nv => nv.MaTaiKhoan == maTaiKhoan);
            if (nhanVien == null) return false;
            await _context.NhanViens.Remove(nhanVien).Context.SaveChangesAsync();
            return true;
        }

        public async Task<List<NhanVien>> GetAllByLoaiTaiKhoanWithTaiKhoanAsync(string loaiTaiKhoan)
        {
            var nhanVienList = await _context.NhanViens
                .Include(nv => nv.MaTaiKhoanNavigation)
                .Where(nv => nv.MaTaiKhoanNavigation.loaiTaiKhoan == loaiTaiKhoan)
                .OrderBy(nv => nv.HoTen)
                .ToListAsync();
            return nhanVienList;
        }

        public async Task<List<NhanVien>> GetAllNhanVienAsync()
        {
            var nhanVienList = await _context.NhanViens
                .Include(nv => nv.MaTaiKhoanNavigation)
                .OrderBy(nv => nv.HoTen)
                .ToListAsync();
            return nhanVienList;
        }

        public async Task<List<NhanVien>> GetAllNhanVienWithTaiKhoanAsync()
        {
            var nhanVienList = await _context.NhanViens
                .Include(nv => nv.MaTaiKhoanNavigation)
                .OrderBy(nv => nv.HoTen)
                .ToListAsync();
            return nhanVienList;
        }

        public async Task<NhanVien> GetByIdWithTaiKhoanAsync(Guid maNhanVien)
        {
            var nhanVien = await _context.NhanViens
                .Include(nv => nv.MaTaiKhoanNavigation)
                .FirstOrDefaultAsync(nv => nv.MaNv == maNhanVien);
            if (nhanVien == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy nhân viên với ID {maNhanVien}");
            }
            return nhanVien;
        }

        public async Task<NhanVien?> GetNhanVienByIdAsync(Guid maNhanVien)
        {
            var nhanVien = await _context.NhanViens
                .Include(nv => nv.MaTaiKhoanNavigation)
                .FirstOrDefaultAsync(nv => nv.MaNv == maNhanVien);
            return nhanVien;
        }

        public async Task<NhanVien?> GetNhanVienByTaiKhoanIdAsync(Guid maTaiKhoan)
        {
            var nhanVien = await _context.NhanViens
                .Include(nv => nv.MaTaiKhoanNavigation)
                .FirstOrDefaultAsync(nv => nv.MaTaiKhoan == maTaiKhoan);
            return nhanVien;
        }

        public async Task<bool> UpdateNhanVienAsync(NhanVien nhanVien)
        {
            var existingNhanVien = await GetNhanVienByIdAsync(nhanVien.MaNv);
            if (existingNhanVien == null)
            {
                return false;
            }
            _context.NhanViens.Update(nhanVien);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> UpdateNhanVienDaNghiViecAsync(Guid maNhanVien, bool daNghiViec)
        {
            var nhanVien = await _context.NhanViens.FindAsync(maNhanVien);
            if (nhanVien == null)
            {
                return false;
            }
            nhanVien.DaNghiViec = daNghiViec;
            await _context.SaveChangesAsync();
            return true;

        }
    }
}
