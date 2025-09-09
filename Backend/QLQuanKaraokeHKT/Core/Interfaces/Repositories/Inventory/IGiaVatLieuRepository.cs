using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Base;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.Inventory
{
    public interface IGiaVatLieuRepository : IGenericRepository<GiaVatLieu,int>
    {
        Task<GiaVatLieu?> GetGiaHienTaiByMaVatLieuAsync(int maVatLieu);
        Task<List<GiaVatLieu>> GetGiaVatLieuByMaVatLieuAsync(int maVatLieu);
        Task DisableCurrentPricesAsync(int maVatLieu, string newTrangThai);
    }
}