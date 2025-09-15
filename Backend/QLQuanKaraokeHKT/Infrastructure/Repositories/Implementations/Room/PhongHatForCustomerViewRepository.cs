using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Core.Entities.Views;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Room;
using QLQuanKaraokeHKT.Infrastructure.Data;
using QLQuanKaraokeHKT.Infrastructure.Repositories.Base;
using QLQuanKaraokeHKT.Shared.Models;

namespace QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.Room
{
    public class PhongHatForCustomerViewRepository : GenericRepository<PhongHatForCustomerView, int>, IPhongHatForCustomerViewRepository
    {
        public PhongHatForCustomerViewRepository(QlkaraokeHktContext context) : base(context)
        {
        }

        public async Task<Pagination<PhongHatForCustomerView>> GetCustomerRoomsAsync(int pageIndex, int pageSize)
        {
            try
            {
                var query = _context.PhongHatForCustomerViews.AsNoTracking();
                return await Pagination<PhongHatForCustomerView>.ToPagedList(query, pageIndex, pageSize);
            }
            catch
            {
                return new Pagination<PhongHatForCustomerView>(new List<PhongHatForCustomerView>(), 0, pageIndex, pageSize);
            }
        }

  
        public async Task<PhongHatForCustomerView?> GetCustomerRoomByIdAsync(int maPhong)
        {
            try
            {
                return await _dbSet
                    .AsNoTracking()
                    .FirstOrDefaultAsync(v => v.MaPhong == maPhong);
            }
            catch
            {
                return null;
            }
        }

    }
}
