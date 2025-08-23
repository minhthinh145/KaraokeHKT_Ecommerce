using QLQuanKaraokeHKT.Core.Entities;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.Inventory
{
    public interface ISanPhamDichVuRepository
    {
        Task<SanPhamDichVu> CreateSanPhamDichVuAsync(SanPhamDichVu sanPham);
        Task<SanPhamDichVu?> GetSanPhamDichVuByIdAsync(int maSanPham);
        Task<bool> UpdateSanPhamDichVuAsync(SanPhamDichVu sanPham);
    }
}