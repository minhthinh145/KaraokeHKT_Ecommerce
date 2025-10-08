using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Application.Repositories.Inventory;
using QLQuanKaraokeHKT.Infrastructure.Data;


namespace QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.Inventory
{

    public class GiaDichVuRepository : GenericRepository<GiaDichVu, int>, IGiaDichVuRepository
    {
        public GiaDichVuRepository(QlkaraokeHktContext context) : base(context)
        {
        }

        #region Override Generic Methods (Enhanced with Navigation Properties)


        public override async Task<List<GiaDichVu>> GetAllAsync()
        {
            try
            {
                return await _dbSet
                    .OrderByDescending(g => g.NgayApDung)
                    .ToListAsync();
            }
            catch (Exception)
            {
                return new List<GiaDichVu>();
            }
        }

        public override async Task<GiaDichVu?> GetByIdAsync(int id)
        {
            try
            {
                return await _dbSet
                    .FirstOrDefaultAsync(g => g.MaGiaDichVu == id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region Specialized Pricing Methods


        public async Task<GiaDichVu?> GetCurrentPriceAsync(int maSanPham, int? maCa = null, DateOnly? ngayApDung = null)
        {
            try
            {
                var ngayTimKiem = ngayApDung ?? DateOnly.FromDateTime(DateTime.Now);

                IQueryable<GiaDichVu> query = _dbSet
                    .Where(g => g.MaSanPham == maSanPham
                               && g.TrangThai == "HieuLuc"
                               && g.NgayApDung <= ngayTimKiem);

                if (maCa.HasValue)
                {
                    var giaTheoCa = await query
                        .Where(g => g.MaCa == maCa)
                        .OrderByDescending(g => g.NgayApDung)
                        .FirstOrDefaultAsync();

                    if (giaTheoCa != null) return giaTheoCa;
                }

                return await query
                    .Where(g => g.MaCa == null)
                    .OrderByDescending(g => g.NgayApDung)
                    .FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<GiaDichVu>> GetPricesByProductAsync(int maSanPham)
        {
            try
            {
                return await _dbSet
                  .Where(g => g.MaSanPham == maSanPham)
                  .OrderByDescending(g => g.NgayApDung)
                  .ThenBy(g => g.MaCa ?? 0)
                  .ToListAsync();
            }
            catch (Exception)
            {
                return new List<GiaDichVu>();
            }
        }


        public async Task<int> BulkUpdateStatusByProductAsync(int maSanPham, string newStatus, string currentStatus = "HieuLuc")
        {
            try
            {
                return await _dbSet
                    .Where(g => g.MaSanPham == maSanPham && g.TrangThai == currentStatus)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(g => g.TrangThai, newStatus));
            }
            catch (Exception)
            {
                return 0;
            }
        }

        #endregion
    }
}