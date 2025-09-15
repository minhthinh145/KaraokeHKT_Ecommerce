using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Booking;
using QLQuanKaraokeHKT.Infrastructure.Data;
using QLQuanKaraokeHKT.Infrastructure.Repositories.Base;

namespace QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.Booking
{
    public class LichSuSuDungPhongRepository : GenericRepository<LichSuSuDungPhong, int>,ILichSuSuDungPhongRepository
    {

        public LichSuSuDungPhongRepository(QlkaraokeHktContext context) : base(context)
        {

        }

        public async Task<List<LichSuSuDungPhong>> GetLichSuByKhachHangAsync(Guid maKhachHang)
        {
            return await _dbSet
                .Where(ls => ls.MaKhachHang == maKhachHang)
                .OrderByDescending(ls => ls.ThoiGianBatDau)
                .ToListAsync();
        }

        public async Task<List<LichSuSuDungPhong>> GetLichSuWithDetailsAsync(Guid maKhachHang)
        {
            return await _dbSet
                .Include(ls => ls.MaPhongNavigation)
                    .ThenInclude(p => p.MaSanPhamNavigation)
                .Include(ls => ls.MaHoaDonNavigation)
                .Include(ls => ls.MaKhachHangNavigation)
                .Where(ls => ls.MaKhachHang == maKhachHang)
                .OrderByDescending(ls => ls.ThoiGianBatDau)
                .ToListAsync();
        }

        public async Task<LichSuSuDungPhong> GetLichSuByIdAsync(int idLichSu)
        {
            return await _dbSet
                .Include(ls => ls.MaPhongNavigation)
                .Include(ls => ls.MaHoaDonNavigation)
                .Include(ls => ls.MaKhachHangNavigation)
                .Include(ls => ls.MaThuePhongNavigation)
                .FirstOrDefaultAsync(ls => ls.IdLichSu == idLichSu);
        }

        public async Task<LichSuSuDungPhong> CreateLichSuAsync(LichSuSuDungPhong lichSu)
        {
            _dbSet.Add(lichSu);
            await _context.SaveChangesAsync();
            return lichSu;
        }

        public async Task<LichSuSuDungPhong> GetLichSuByMaThuePhongAsync(Guid maThuePhong)
        {
           var LichSu = await _dbSet
                .Include(ls => ls.MaPhongNavigation)
                .Include(ls => ls.MaHoaDonNavigation)
                .Include(ls => ls.MaKhachHangNavigation)
                .FirstOrDefaultAsync(ls => ls.MaThuePhong == maThuePhong);
            return LichSu ?? throw new KeyNotFoundException("Lịch sử sử dụng phòng không tồn tại cho mã thuê phòng này.");
        }
    }
}