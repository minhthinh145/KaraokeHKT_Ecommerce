using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Base;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.HRM
{
    public interface ILuongCaLamViecRepository : IGenericRepository<LuongCaLamViec, int>
    {

        Task<LuongCaLamViec?> GetLuongCaLamViecByIdAsync(int maLuongCaLamViec);

        Task<LuongCaLamViec?> GetLuongCaLamViecByMaCaAsync(int maCa);

        Task<LuongCaLamViec?> GetLuongCaLamViecByMaCaAndDateAsync(int maCa, DateOnly ngay);
    }
}