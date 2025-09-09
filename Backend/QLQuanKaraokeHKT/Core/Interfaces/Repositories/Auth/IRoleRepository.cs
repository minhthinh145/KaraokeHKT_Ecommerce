using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Base;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.Auth
{
    public interface IRoleRepository : IGenericRepository<VaiTro, Guid>
    {
        Task<bool> RoleExistsAsync(string roleName);

        Task CreateRoleAsync(string roleName);

        Task AddToRoleAsync(TaiKhoan user, string role);

        Task<bool> UpdateUserRoleAsync(TaiKhoan user, string newRoleName);

        Task RemoveFromRoleAsync(TaiKhoan user, string roleName);

        Task<string> GetRoleDescriptionAsync(string roleName);

        Task<string?> GetRoleCodeFromDescriptionAsync(string roleDescription);
    }
}
