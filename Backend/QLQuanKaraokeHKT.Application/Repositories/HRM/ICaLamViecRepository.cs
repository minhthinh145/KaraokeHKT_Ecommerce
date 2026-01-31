using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Application.Repositories;

namespace QLQuanKaraokeHKT.Application.Repositories.HRM
{
    public interface ICaLamViecRepository : IGenericRepository<CaLamViec,int>
    {
   
        Task<int> GetIdCaLamViecByTenCaAsync(string tenCa);

        Task<List<CaLamViec>> GetCaLamViecByTenCaAsync(List<string> tenCaList);

    }
}