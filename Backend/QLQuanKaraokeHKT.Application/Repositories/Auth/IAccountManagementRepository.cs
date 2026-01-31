using QLQuanKaraokeHKT.Domain.Entities;

namespace QLQuanKaraokeHKT.Application.Repositories.Auth
{
    public interface IAccountManagementRepository
    {
        Task<bool> LockAccountAsync(Guid maTaIkhoan);

        Task<bool> UnlockAccountAsync(Guid maTaIkhoan);

        Task<bool> DeleteUserAsync(TaiKhoan user);

        Task<bool> DeleteUserByIdAsync(Guid userId);
    }
}
