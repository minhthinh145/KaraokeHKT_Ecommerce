using QLQuanKaraokeHKT.Models;

namespace QLQuanKaraokeHKT.Repositories.QLKho.Interfaces
{
    public interface IVatLieuRepository
    {
        Task<List<VatLieu>> GetAllVatLieuWithDetailsAsync();
        Task<VatLieu?> GetVatLieuByIdAsync(int maVatLieu);
        Task<VatLieu?> GetVatLieuWithDetailsByIdAsync(int maVatLieu);
        Task<VatLieu> CreateVatLieuAsync(VatLieu vatLieu);
        Task<bool> UpdateVatLieuAsync(VatLieu vatLieu);
        Task<bool> UpdateSoLuongVatLieuAsync(int maVatLieu, int soLuongMoi);
        Task<bool> UpdateNgungCungCapAsync(int maVatLieu, bool ngungCungCap);
    }
}