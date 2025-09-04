using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Base;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.Customer
{
    public interface IKhachHangRepository : IGenericRepository<KhachHang,Guid>
    {

        Task<KhachHang?> GetByAccountIdAsync(Guid maTaiKhoan);

        Task<List<KhachHang>> GetAllWithTaiKhoanAsync();
    }
}