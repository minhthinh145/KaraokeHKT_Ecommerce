using QLQuanKaraokeHKT.Models;

namespace QLQuanKaraokeHKT.Repositories.QLKho.Interfaces
{
    public interface ISanPhamDichVuRepository
    {
        Task<SanPhamDichVu> CreateSanPhamDichVuAsync(SanPhamDichVu sanPham);
        Task<SanPhamDichVu?> GetSanPhamDichVuByIdAsync(int maSanPham);
        Task<bool> UpdateSanPhamDichVuAsync(SanPhamDichVu sanPham);
    }
}