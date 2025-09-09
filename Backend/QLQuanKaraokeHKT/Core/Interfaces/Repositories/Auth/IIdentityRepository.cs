using Microsoft.AspNetCore.Identity;
using QLQuanKaraokeHKT.Core.Entities;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.Auth
{
    public interface IIdentityRepository
    {
        Task<IdentityResult> CreateUserAsync(TaiKhoan user, string password);

        Task<TaiKhoan?> FindByEmailAsync(string email);

        Task<TaiKhoan?> FindByUserIDAsync(string userId);

        Task<bool> CheckPasswordAsync(TaiKhoan user, string password);

        Task<IList<string>> GetUserRolesAsync(TaiKhoan user);

        Task<IdentityResult> UpdateUserAsync(TaiKhoan user);

        Task<(bool Success, string[] Errors)> UpdatePasswordAsync(TaiKhoan user, string newPassword);

        Task<bool> CheckEmailExitsAsync(string email);

    }
}
