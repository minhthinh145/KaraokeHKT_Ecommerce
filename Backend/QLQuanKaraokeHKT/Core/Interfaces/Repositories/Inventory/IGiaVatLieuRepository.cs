using QLQuanKaraokeHKT.Core.Entities;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.Inventory
{
    public interface IGiaVatLieuRepository
    {
        Task<GiaVatLieu> CreateGiaVatLieuAsync(GiaVatLieu giaVatLieu);
        Task<GiaVatLieu?> GetGiaHienTaiByMaVatLieuAsync(int maVatLieu);
        Task<List<GiaVatLieu>> GetGiaVatLieuByMaVatLieuAsync(int maVatLieu);
        Task<bool> UpdateGiaVatLieuStatusAsync(int maGiaVatLieu, string trangThai);
    }
}