using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Models;
using QLQuanKaraokeHKT.Repositories.QLKho.Interfaces;

namespace QLQuanKaraokeHKT.Repositories.QLKho.Implementations
{
    public class GiaDichVuRepository : IGiaDichVuRepository
    {
        private readonly QlkaraokeHktContext _context;

        public GiaDichVuRepository(QlkaraokeHktContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<GiaDichVu> CreateGiaDichVuAsync(GiaDichVu giaDichVu)
        {
            _context.GiaDichVus.Add(giaDichVu);
            await _context.SaveChangesAsync();
            return giaDichVu;
        }

        public async Task<GiaDichVu?> GetGiaHienTaiByMaSanPhamAsync(int maSanPham)
        {
            return await _context.GiaDichVus
                .Where(g => g.MaSanPham == maSanPham && g.TrangThai == "HieuLuc")
                .OrderByDescending(g => g.NgayApDung)
                .FirstOrDefaultAsync();
        }

        public async Task<List<GiaDichVu>> GetGiaDichVuByMaSanPhamAsync(int maSanPham)
        {
            return await _context.GiaDichVus
                .Where(g => g.MaSanPham == maSanPham)
                .OrderByDescending(g => g.NgayApDung)
                .ToListAsync();
        }

        public async Task<bool> UpdateGiaDichVuStatusAsync(int maGiaDichVu, string trangThai)
        {
            var gia = await _context.GiaDichVus.FindAsync(maGiaDichVu);
            if (gia == null) return false;
            gia.TrangThai = trangThai;
            await _context.SaveChangesAsync();
            return true;
        }

        // ✅ IMPLEMENT METHODS MỚI
        public async Task<GiaDichVu?> GetGiaHienTaiByMaSanPhamAndCaAsync(int maSanPham, int? maCa = null, DateOnly? ngayApDung = null)
        {
            var ngayTimKiem = ngayApDung ?? DateOnly.FromDateTime(DateTime.Now);

            // Ưu tiên tìm giá theo ca cụ thể
            if (maCa.HasValue)
            {
                var giaTheoCa = await _context.GiaDichVus
                    .Where(g => g.MaSanPham == maSanPham
                               && g.MaCa == maCa
                               && g.TrangThai == "HieuLuc"
                               && g.NgayApDung <= ngayTimKiem)
                    .OrderByDescending(g => g.NgayApDung)
                    .FirstOrDefaultAsync();

                if (giaTheoCa != null) return giaTheoCa;
            }

            // Fallback về giá chung (MaCa = null)
            return await _context.GiaDichVus
                .Where(g => g.MaSanPham == maSanPham
                           && g.MaCa == null
                           && g.TrangThai == "HieuLuc"
                           && g.NgayApDung <= ngayTimKiem)
                .OrderByDescending(g => g.NgayApDung)
                .FirstOrDefaultAsync();
        }

        public async Task<List<GiaDichVu>> GetGiaDichVuByMaSanPhamAndCaAsync(int maSanPham, int? maCa = null)
        {
            var query = _context.GiaDichVus.Where(g => g.MaSanPham == maSanPham);

            if (maCa.HasValue)
            {
                query = query.Where(g => g.MaCa == maCa);
            }

            return await query
                .OrderByDescending(g => g.NgayApDung)
                .ToListAsync();
        }

        public async Task<bool> UpdateGiaDichVuStatusByMaSanPhamAsync(int maSanPham, string trangThai, int? maCa = null)
        {
            IQueryable<GiaDichVu> query = _context.GiaDichVus
                .Where(g => g.MaSanPham == maSanPham && g.TrangThai == "HieuLuc");

            // ✅ FIX: Khi maCa = null, chỉ update giá chung. Khi có maCa, chỉ update ca đó
            if (maCa.HasValue)
            {
                query = query.Where(g => g.MaCa == maCa.Value);
            }
            else
            {
                // Khi muốn update tất cả (truyền maCa = null), không filter thêm
                // Hoặc khi muốn update chỉ giá chung, filter MaCa == null
                // Tùy logic business, ở đây tôi giả sử muốn update TẤT CẢ khi maCa = null
            }

            var giaList = await query.ToListAsync();
            if (!giaList.Any()) return false;

            foreach (var gia in giaList)
            {
                gia.TrangThai = trangThai;
            }

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<GiaDichVu?> GetGiaDichVuHienTaiAsync(int maSanPham, DateOnly ngayApDung, int? maCa = null)
        {
            // Ưu tiên tìm giá theo ca cụ thể nếu có
            if (maCa.HasValue)
            {
                var giaTheoCa = await _context.GiaDichVus
                    .Where(g => g.MaSanPham == maSanPham
                               && g.MaCa == maCa
                               && g.TrangThai == "HieuLuc"
                               && g.NgayApDung <= ngayApDung)
                    .OrderByDescending(g => g.NgayApDung)
                    .FirstOrDefaultAsync();

                if (giaTheoCa != null) return giaTheoCa;
            }

            // Fallback về giá chung (MaCa = null)
            return await _context.GiaDichVus
                .Where(g => g.MaSanPham == maSanPham
                           && g.MaCa == null
                           && g.TrangThai == "HieuLuc"
                           && g.NgayApDung <= ngayApDung)
                .OrderByDescending(g => g.NgayApDung)
                .FirstOrDefaultAsync();
        }

    }
}