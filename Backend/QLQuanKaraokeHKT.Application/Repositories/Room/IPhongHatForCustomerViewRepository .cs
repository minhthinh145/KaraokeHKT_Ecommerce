using QLQuanKaraokeHKT.Domain.Entities.Views;
using QLQuanKaraokeHKT.Application;
using QLQuanKaraokeHKT.Application.Repositories;
using QLQuanKaraokeHKT.Shared.Models;

namespace QLQuanKaraokeHKT.Application
{
    public interface IPhongHatForCustomerViewRepository : IGenericRepository<PhongHatForCustomerView, int>
    { 
        Task<Pagination<PhongHatForCustomerView>> GetCustomerRoomsAsync(int pageIndex, int pageSize);
        Task<PhongHatForCustomerView?> GetCustomerRoomByIdAsync(int maPhong);
    }
}
