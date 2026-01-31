using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Application.Repositories.HRM;
using QLQuanKaraokeHKT.Infrastructure.Data;


namespace QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.HRM
{
    public class NhanVienRepository : GenericRepository<NhanVien, Guid>,INhanVienRepository
    {

        public NhanVienRepository(QlkaraokeHktContext context) : base(context) { }

        #region Build Base Query
        private IQueryable<NhanVien> BuildBaseQuery()
        {
            return _dbSet
                .Include(nv => nv.MaTaiKhoanNavigation)
                .AsQueryable();
                
        }
        #endregion

        #region Read-only Operations
        public async Task<List<NhanVien>> GetAllByLoaiTaiKhoanWithTaiKhoanAsync(string loaiTaiKhoan)
        {
            var nhanVienList = await BuildBaseQuery()
                .Where(nv => nv.MaTaiKhoanNavigation.loaiTaiKhoan == loaiTaiKhoan)
                .OrderBy(nv => nv.HoTen)
                .AsNoTracking()
                .ToListAsync();
            return nhanVienList;
        }

        public override async Task<List<NhanVien>> GetAllAsync()
        {
            var nhanVienList = await BuildBaseQuery()
                .OrderBy(nv => nv.HoTen)
                .AsNoTracking()
                .ToListAsync();
            return nhanVienList;
        }

        public async Task<List<NhanVien>> GetAllNhanVienWithTaiKhoanAsync()
        {
            var nhanVienList = await _context.NhanViens
                .Include(nv => nv.MaTaiKhoanNavigation)
                .OrderBy(nv => nv.HoTen)
                .AsNoTracking()
                .ToListAsync();
            return nhanVienList;
        }

        public async Task<NhanVien> GetByIdWithTaiKhoanAsync(Guid maNhanVien)
        {
            var nhanVien = await BuildBaseQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(nv => nv.MaNv == maNhanVien);
                
            if (nhanVien == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy nhân viên với ID {maNhanVien}");
            }
            return nhanVien;
        }

        public async Task<NhanVien?> GetNhanVienByTaiKhoanIdAsync(Guid maTaiKhoan)
        {
            var nhanVien = await _context.NhanViens
                .Include(nv => nv.MaTaiKhoanNavigation)
                .AsNoTracking()
                .FirstOrDefaultAsync(nv => nv.MaTaiKhoan == maTaiKhoan);
            return nhanVien;
        }
        #endregion
        public async Task<bool> UpdateNhanVienDaNghiViecAsync(Guid maNhanVien, bool daNghiViec)
        {
            var nhanVien = await GetByIdAsync(maNhanVien);
            if (nhanVien == null)
            {
                return false;
            }
            nhanVien.DaNghiViec = daNghiViec;
            return true;

        }
    }
}
