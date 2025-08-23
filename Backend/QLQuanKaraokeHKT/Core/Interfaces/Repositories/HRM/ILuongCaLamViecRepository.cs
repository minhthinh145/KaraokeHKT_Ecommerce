using QLQuanKaraokeHKT.Core.Entities;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.HRM
{
    public interface ILuongCaLamViecRepository
    {
        Task<LuongCaLamViec> CreateLuongCaLamViecAsync(LuongCaLamViec luongCaLamViec);
        Task<List<LuongCaLamViec>> GetAllLuongCaLamViecsAsync();

        Task<LuongCaLamViec?> GetLuongCaLamViecByIdAsync(int maLuongCaLamViec);

        Task<LuongCaLamViec?> GetLuongCaLamViecByMaCaAsync(int maCa);

        Task<LuongCaLamViec> UpdateLuongCaLamViecAsync(LuongCaLamViec luongCaLamViec);

        Task<bool> DeleteLuongCaLamViecAsync(int maLuongCaLamViec);



    }
}
