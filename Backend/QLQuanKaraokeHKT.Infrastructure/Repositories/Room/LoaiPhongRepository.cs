using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Application.Repositories.Room;
using QLQuanKaraokeHKT.Infrastructure.Data;

namespace QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.Room
{
    public class LoaiPhongRepository : GenericRepository<LoaiPhongHatKaraoke,int>,ILoaiPhongRepository
    {

        public LoaiPhongRepository(QlkaraokeHktContext context) : base(context) { }

        public override async Task<List<LoaiPhongHatKaraoke>> GetAllAsync()
        {
            return await _dbSet
                .OrderByDescending(lp => lp.SucChua)
                .ToListAsync();
        }

        public async Task<bool> HasPhongHatKaraokeAsync(int maLoaiPhong)
        {
            return await _dbSet
                .AnyAsync(p => p.MaLoaiPhong == maLoaiPhong);
        }
    }
}