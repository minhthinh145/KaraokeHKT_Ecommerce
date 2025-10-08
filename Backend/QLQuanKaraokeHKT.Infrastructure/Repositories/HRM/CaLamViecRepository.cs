using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Application.Repositories.HRM;
using QLQuanKaraokeHKT.Infrastructure.Data;


namespace QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.HRM
{
    public class CaLamViecRepository : GenericRepository<CaLamViec,int>,ICaLamViecRepository
    {

        public CaLamViecRepository(QlkaraokeHktContext context) : base(context) { }

        public async Task<int> GetIdCaLamViecByTenCaAsync(string tenCa)
        {
           var caLamViecs = await _dbSet.FirstOrDefaultAsync(c => c.TenCa == tenCa);

            if (caLamViecs == null) {
                throw new KeyNotFoundException($"CaLamViec with TenCa {tenCa} not found.");
            }

            return caLamViecs.MaCa;
        }

        public async Task<List<CaLamViec>> GetCaLamViecByTenCaAsync(List<string> tenCaList)
        {
            return await _dbSet
                .Where(c => tenCaList.Contains(c.TenCa))
                .OrderBy(c => c.TenCa)
                .ToListAsync();
        }
    }
}
