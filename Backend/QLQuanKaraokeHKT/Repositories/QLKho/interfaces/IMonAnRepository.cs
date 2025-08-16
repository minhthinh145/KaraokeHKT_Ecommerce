using QLQuanKaraokeHKT.Models;

namespace QLQuanKaraokeHKT.Repositories.QLKho.Interfaces
{
    public interface IMonAnRepository
    {
        Task<MonAn> CreateMonAnAsync(MonAn monAn);
        Task<MonAn?> GetMonAnByMaVatLieuAsync(int maVatLieu);
        Task<bool> UpdateSoLuongMonAnAsync(int maVatLieu, int soLuongMoi);
    }
}