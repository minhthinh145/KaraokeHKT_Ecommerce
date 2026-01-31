using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Application.Repositories;

namespace QLQuanKaraokeHKT.Application.Repositories.Inventory
{
    public interface IGiaVatLieuRepository : IGenericRepository<GiaVatLieu,int>
    {
        Task<GiaVatLieu?> GetGiaHienTaiByMaVatLieuAsync(int maVatLieu);
        Task<List<GiaVatLieu>> GetGiaVatLieuByMaVatLieuAsync(int maVatLieu);
        Task DisableCurrentPricesAsync(int maVatLieu, string newTrangThai);
    }
}