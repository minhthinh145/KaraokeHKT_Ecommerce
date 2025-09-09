using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Base;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.Auth
{
    public interface IAccountManagementRepository : IGenericRepository<TaiKhoan,Guid>   
    {
        Task<bool> LockAccountAsync(Guid maTaIkhoan);

        Task<bool> UnlockAccountAsync(Guid maTaIkhoan);

        Task<bool> DeleteUserAsync(TaiKhoan user);

        Task<bool> DeleteUserByIdAsync(Guid userId);

        Task<bool> DeleteUserWithRelatedDataAsync(TaiKhoan user);

    }
}
