using QLQuanKaraokeHKT.Core.Entities.Views;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Base;
using QLQuanKaraokeHKT.Shared.Models;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.Room
{
    public interface IPhongHatForCustomerViewRepository : IGenericRepository<PhongHatForCustomerView, int>
    { 
        Task<Pagination<PhongHatForCustomerView>> GetCustomerRoomsAsync(int pageIndex, int pageSize);
        Task<PhongHatForCustomerView?> GetCustomerRoomByIdAsync(int maPhong);
    }
}
