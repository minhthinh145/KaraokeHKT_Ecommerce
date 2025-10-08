using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Application.Repositories.Room;
using QLQuanKaraokeHKT.Infrastructure.Data;

namespace QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.Room
{
    public class PhongHatKaraokeRepository : GenericRepository<PhongHatKaraoke, int>, IPhongHatKaraokeRepository
    {
        public PhongHatKaraokeRepository(QlkaraokeHktContext context) : base(context)
        {
        }

        #region Private Query Builder
        private IQueryable<PhongHatKaraoke> BuildBaseQuery(bool includePricing)
        {
            var query = _dbSet
                .Include(p => p.MaLoaiPhongNavigation)
                .Include(p => p.MaSanPhamNavigation)
                .AsQueryable();
            if (includePricing)
            {
                query = query
                    .Include(p => p.MaSanPhamNavigation)
                        .ThenInclude(sp => sp.GiaDichVus
                            .Where(g => g.TrangThai == "HieuLuc"))
                                .ThenInclude(g => g.MaCaNavigation);
            }

            return query;
        }
        #endregion

        #region Read-Only Queries
        public override async Task<List<PhongHatKaraoke>> GetAllAsync()
        {
            try
            {
                return await _dbSet
                    .AsNoTracking()
                    .OrderBy(p => p.MaPhong)
                    .ToListAsync();
            }
            catch
            {
                return new List<PhongHatKaraoke>();
            }
        }

        public override async Task<PhongHatKaraoke?> GetByIdAsync(int id)
        {
            try
            {
                return await _dbSet
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.MaPhong == id);
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<PhongHatKaraoke>> GetAllWithDetailsAsync(bool includePricing = false)
        {
            try
            {
                return await BuildBaseQuery(includePricing)
                    .AsNoTracking()
                    .OrderBy(p => p.MaPhong)
                    .ToListAsync();
            }
            catch
            {
                return new List<PhongHatKaraoke>();
            }
        }

        public async Task<PhongHatKaraoke?> GetByIdWithDetailsAsync(int maPhong, bool includePricing = false)
        {
            try
            {
                return await BuildBaseQuery(includePricing)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.MaPhong == maPhong);
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<PhongHatKaraoke>> GetByLoaiPhongAsync(int maLoaiPhong, bool includePricing = false)
        {
            try
            {
                return await BuildBaseQuery(includePricing)
                    .AsNoTracking()
                    .Where(p => p.MaLoaiPhong == maLoaiPhong)
                    .OrderBy(p => p.MaPhong)
                    .ToListAsync();
            }
            catch
            {
                return new List<PhongHatKaraoke>();
            }
        }

        public async Task<List<PhongHatKaraoke>> GetAvailableAsync()
        {
            try
            {
                return await _dbSet
                    .Where(p => !p.NgungHoatDong && !p.DangSuDung)
                    .AsNoTracking()
                    .OrderBy(p => p.MaPhong)
                    .ToListAsync();
            }
            catch
            {
                return new List<PhongHatKaraoke>();
            }
        }

        public async Task<List<PhongHatKaraoke>> GetOccupiedAsync()
        {
            try
            {
                return await _dbSet
                    .Where(p => !p.NgungHoatDong && p.DangSuDung)
                    .AsNoTracking()
                    .OrderBy(p => p.MaPhong)
                    .ToListAsync();
            }
            catch
            {
                return new List<PhongHatKaraoke>();
            }
        }

        public async Task<List<PhongHatKaraoke>> GetOutOfServiceAsync()
        {
            try
            {
                return await _dbSet
                    .Where(p => p.NgungHoatDong)
                    .AsNoTracking()
                    .OrderBy(p => p.MaPhong)
                    .ToListAsync();
            }
            catch
            {
                return new List<PhongHatKaraoke>();
            }
        }
        #endregion

        #region Tracked Queries (For Updates)
        public async Task<PhongHatKaraoke?> GetByIdForUpdateAsync(int maPhong)
        {
            try
            {
                return await _dbSet
                    .Include(p => p.MaLoaiPhongNavigation)
                    .Include(p => p.MaSanPhamNavigation)
                    .FirstOrDefaultAsync(p => p.MaPhong == maPhong);
            }
            catch
            {
                return null;
            }
        }
        #endregion
    }
}