using QLQuanKaraokeHKT.Core.Entities;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.Inventory
{
    public interface IMonAnRepository
    {
        Task<MonAn> CreateMonAnAsync(MonAn monAn);
        Task<MonAn?> GetMonAnByMaVatLieuAsync(int maVatLieu);
        Task<bool> UpdateSoLuongMonAnAsync(int maVatLieu, int soLuongMoi);
    }
}