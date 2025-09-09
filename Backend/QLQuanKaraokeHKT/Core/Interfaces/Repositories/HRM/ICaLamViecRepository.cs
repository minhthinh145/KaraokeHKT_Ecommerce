using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Base;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.HRM
{
    public interface ICaLamViecRepository : IGenericRepository<CaLamViec,int>
    {
   
        Task<int> GetIdCaLamViecByTenCaAsync(string tenCa);

        Task<List<CaLamViec>> GetCaLamViecByTenCaAsync(List<string> tenCaList);

    }
}