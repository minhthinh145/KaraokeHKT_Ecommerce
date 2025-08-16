using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Models;
using QLQuanKaraokeHKT.Repositories.QLNhanSu.Interfaces;

namespace QLQuanKaraokeHKT.Repositories.QLNhanSu.Implementations
{
    public class CaLamViecRepository : ICaLamViecRepository
    {
        private readonly QlkaraokeHktContext _context;

        public CaLamViecRepository(QlkaraokeHktContext context)
        {
            _context = context?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<CaLamViec> CreateCaLamViecAsync(CaLamViec caLamViec)
        {
            var result = await _context.CaLamViecs.AddAsync(caLamViec);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public Task<bool> DeleteCaLamViecAsync(int maCa)
        {
            var caLamViec = _context.CaLamViecs.Find(maCa);
            if (caLamViec == null)
            {
                return Task.FromResult(false);
            }
            _context.CaLamViecs.Remove(caLamViec);
            return _context.SaveChangesAsync().ContinueWith(t => t.Result > 0);
        }

        public async Task<List<CaLamViec>> GetAllCaLamViecsAsync()
        {
            var caLamViecs = await _context.CaLamViecs.ToListAsync();
            return caLamViecs ?? new List<CaLamViec>();
        }

        public async Task<CaLamViec> GetCaLamViecByIdAsync(int maCa)
        {
            var caLamViec = await _context.CaLamViecs.FindAsync(maCa);
            return caLamViec ?? throw new KeyNotFoundException($"CaLamViec with ID {maCa} not found.");

        }

        public async Task<int> GetIdCaLamViecByTenCaAsync(string tenCa)
        {
           var caLamViecs = await _context.CaLamViecs.FirstOrDefaultAsync(c => c.TenCa == tenCa);
            return caLamViecs.MaCa;
        }

        public async Task<CaLamViec> UpdateCaLamViecAsync(CaLamViec caLamViec)
        {
            var existingCaLamViec = await GetCaLamViecByIdAsync(caLamViec.MaCa);
            if (existingCaLamViec == null)
            {
                throw new KeyNotFoundException($"CaLamViec with ID {caLamViec.MaCa} not found.");
            }
            _context.CaLamViecs.Update(caLamViec);
            await _context.SaveChangesAsync();
            return caLamViec;
        }


        public async Task<List<CaLamViec>> GetCaLamViecByTenCaAsync(List<string> tenCaList)
        {
            return await _context.CaLamViecs
                .Where(c => tenCaList.Contains(c.TenCa))
                .OrderBy(c => c.TenCa)
                .ToListAsync();
        }
    }
}
