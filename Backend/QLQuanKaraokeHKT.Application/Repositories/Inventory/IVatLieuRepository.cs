using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Application.Repositories;

namespace QLQuanKaraokeHKT.Application.Repositories.Inventory
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