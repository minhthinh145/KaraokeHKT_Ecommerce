using QLQuanKaraokeHKT.Domain.Entities;

namespace QLQuanKaraokeHKT.Application.Repositories.Auth
{
    public interface IRoleRepository
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
