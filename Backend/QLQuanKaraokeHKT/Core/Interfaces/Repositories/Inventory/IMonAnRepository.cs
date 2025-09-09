using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Base;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.Inventory
{
    public interface IMonAnRepository : IGenericRepository<MonAn,int>
    {
        Task<MonAn?> GetMonAnByMaVatLieuAsync(int maVatLieu);
        Task UpdateSoLuongByMaVatLieuAsync(int maVatLieu, int soLuongMoi);
    }
}