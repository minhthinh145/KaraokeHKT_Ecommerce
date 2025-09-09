using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Base;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.Inventory
{
    public interface IVatLieuRepository : IGenericRepository<VatLieu,int>
    {
        Task<List<VatLieu>> GetAllVatLieuWithDetailsAsync(bool includePricing = false);
        Task<VatLieu?> GetVatLieuWithDetailsByIdAsync(int maVatLieu, bool includePricing = false);
        Task<VatLieu?> GetVatLieuForUpdateAsync(int maVatLieu);

        Task<bool> UpdateSoLuongVatLieuAsync(int maVatLieu, int soLuongMoi);
        Task<bool> UpdateNgungCungCapAsync(int maVatLieu, bool ngungCungCap);
    }
}