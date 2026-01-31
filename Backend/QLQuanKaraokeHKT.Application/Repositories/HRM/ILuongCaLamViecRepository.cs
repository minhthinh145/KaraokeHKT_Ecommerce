using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Application.Repositories;

namespace QLQuanKaraokeHKT.Application.Repositories.HRM
{
    public interface ILuongCaLamViecRepository : IGenericRepository<LuongCaLamViec, int>
    {

        Task<LuongCaLamViec?> GetLuongCaLamViecByIdAsync(int maLuongCaLamViec);

        Task<LuongCaLamViec?> GetLuongCaLamViecByMaCaAsync(int maCa);

        Task<LuongCaLamViec?> GetLuongCaLamViecByMaCaAndDateAsync(int maCa, DateOnly ngay);
    }
}