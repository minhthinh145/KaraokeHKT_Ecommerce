using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Models;
using QLQuanKaraokeHKT.Repositories.QLKho.Interfaces;

namespace QLQuanKaraokeHKT.Repositories.QLKho.Implementations
{
    public class VatLieuRepository : IVatLieuRepository
    {
        private readonly QlkaraokeHktContext _context;

        public VatLieuRepository(QlkaraokeHktContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<VatLieu>> GetAllVatLieuWithDetailsAsync()
        {
            return await _context.VatLieus
                .Include(v => v.MonAn)
                    .ThenInclude(m => m.MaSanPhamNavigation)
                        .ThenInclude(sp => sp.GiaDichVus) // Lấy tất cả giá bán, không filter trạng thái
                                        .ThenInclude(gdv => gdv.MaCaNavigation)
                .Include(v => v.GiaVatLieus) // Lấy tất cả giá nhập, không filter trạng thái

                .OrderBy(v => v.TenVatLieu)
                .ToListAsync();
        }
        public async Task<VatLieu?> GetVatLieuByIdAsync(int maVatLieu)
        {
            return await _context.VatLieus.FindAsync(maVatLieu);
        }

        public async Task<VatLieu?> GetVatLieuWithDetailsByIdAsync(int maVatLieu)
        {
            return await _context.VatLieus
                .Include(v => v.MonAn)
                    .ThenInclude(m => m.MaSanPhamNavigation)
                        .ThenInclude(sp => sp.GiaDichVus)
                                        .ThenInclude(gdv => gdv.MaCaNavigation)
                .Include(v => v.GiaVatLieus)
                .FirstOrDefaultAsync(v => v.MaVatLieu == maVatLieu);
        }
        public async Task<VatLieu> CreateVatLieuAsync(VatLieu vatLieu)
        {
            _context.VatLieus.Add(vatLieu);
            await _context.SaveChangesAsync();
            return vatLieu;
        }

        public async Task<bool> UpdateVatLieuAsync(VatLieu vatLieu)
        {
            var existing = await _context.VatLieus.FindAsync(vatLieu.MaVatLieu);
            if (existing == null) return false;

            existing.TenVatLieu = vatLieu.TenVatLieu;
            existing.DonViTinh = vatLieu.DonViTinh;
            existing.SoLuongTonKho = vatLieu.SoLuongTonKho;
            existing.NgungCungCap = vatLieu.NgungCungCap;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateSoLuongVatLieuAsync(int maVatLieu, int soLuongMoi)
        {
            var vatLieu = await _context.VatLieus.FindAsync(maVatLieu);
            if (vatLieu == null) return false;

            vatLieu.SoLuongTonKho = soLuongMoi;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateNgungCungCapAsync(int maVatLieu, bool ngungCungCap)
        {
            var vatLieu = await _context.VatLieus.FindAsync(maVatLieu);
            if (vatLieu == null) return false;

            vatLieu.NgungCungCap = ngungCungCap;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}