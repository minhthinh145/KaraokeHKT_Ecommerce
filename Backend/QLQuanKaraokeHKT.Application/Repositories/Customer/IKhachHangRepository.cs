using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Application.Repositories;

namespace QLQuanKaraokeHKT.Application.Repositories.Customer
{
    public interface IKhachHangRepository : IGenericRepository<KhachHang,Guid>
    {

        Task<KhachHang?> GetByAccountIdAsync(Guid maTaiKhoan);

        Task<List<KhachHang>> GetAllWithTaiKhoanAsync();
    }
}