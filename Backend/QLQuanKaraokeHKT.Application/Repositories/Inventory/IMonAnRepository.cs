using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Application.Repositories;

namespace QLQuanKaraokeHKT.Application.Repositories.Inventory
{
    public interface IMonAnRepository : IGenericRepository<MonAn,int>
    {
        Task<MonAn?> GetMonAnByMaVatLieuAsync(int maVatLieu);
        Task UpdateSoLuongByMaVatLieuAsync(int maVatLieu, int soLuongMoi);
    }
}