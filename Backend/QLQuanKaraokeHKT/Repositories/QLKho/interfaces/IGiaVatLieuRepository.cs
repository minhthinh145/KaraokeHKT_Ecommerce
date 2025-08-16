using QLQuanKaraokeHKT.Models;

namespace QLQuanKaraokeHKT.Repositories.QLKho.Interfaces
{
    public interface IGiaVatLieuRepository
    {
        Task<GiaVatLieu> CreateGiaVatLieuAsync(GiaVatLieu giaVatLieu);
        Task<GiaVatLieu?> GetGiaHienTaiByMaVatLieuAsync(int maVatLieu);
        Task<List<GiaVatLieu>> GetGiaVatLieuByMaVatLieuAsync(int maVatLieu);
        Task<bool> UpdateGiaVatLieuStatusAsync(int maGiaVatLieu, string trangThai);
    }
}