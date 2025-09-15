using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Auth;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Customer;
using QLQuanKaraokeHKT.Infrastructure.Data;
using QLQuanKaraokeHKT.Infrastructure.Repositories.Base;

namespace QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.Customer
{
    public class KhachHangRepository : GenericRepository<KhachHang,Guid>,IKhachHangRepository
    {

        public KhachHangRepository(QlkaraokeHktContext context) : base(context) { }

        public override async Task<List<KhachHang>> GetAllAsync()
        {
            try
            {
                return await _context.KhachHangs
                    .Include(k => k.MaTaiKhoanNavigation)
                    .OrderBy(k => k.TenKhachHang)
                    .ToListAsync();
            }
            catch (Exception)
            {
                return new List<KhachHang>();
            }
        }


        public override async Task<KhachHang?> GetByIdAsync(Guid maKhachHang)
        {
            try
            {
                return await _context.KhachHangs
                    .Include(k => k.MaTaiKhoanNavigation)
                    .FirstOrDefaultAsync(k => k.MaKhachHang == maKhachHang);
            }
            catch (Exception)
            {
                return null;
            }
        }

    
        public async Task<KhachHang?> GetByAccountIdAsync(Guid maTaiKhoan)
        {
            try
            {
                return await _context.KhachHangs
                    .Include(k => k.MaTaiKhoanNavigation)
                    .FirstOrDefaultAsync(k => k.MaTaiKhoan == maTaiKhoan);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<KhachHang>> GetAllWithTaiKhoanAsync()
        {
            var khachHangs = await _context.KhachHangs
                .Include(k => k.MaTaiKhoanNavigation)
                .ToListAsync();
            return khachHangs;
        }

        #region Helper Methods
        private async Task<bool> ExistsAsync(Guid maKhachHang)
        {
            try
            {
                return await _context.KhachHangs
                    .AnyAsync(k => k.MaKhachHang == maKhachHang);
            }
            catch (Exception)
            {
                return false;
            }
        }
   
        #endregion
    }
}