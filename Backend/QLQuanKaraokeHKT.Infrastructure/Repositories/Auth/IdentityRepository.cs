using Microsoft.AspNetCore.Identity;
using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Application.Repositories.Auth;

namespace QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.Auth
{
    public class IdentityRepository : IIdentityRepository
    {
        private UserManager<TaiKhoan> _userManager;

        public IdentityRepository(UserManager<TaiKhoan> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }   

        public async Task<IdentityResult> CreateUserAsync(TaiKhoan user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<TaiKhoan?> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<TaiKhoan?> FindByUserIDAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<bool> CheckPasswordAsync(TaiKhoan user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IList<string>> GetUserRolesAsync(TaiKhoan user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<IdentityResult> UpdateUserAsync(TaiKhoan user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<(bool Success, string[] Errors)> UpdatePasswordAsync(TaiKhoan user, string newPassword)
        {
            try
            {
                if (user == null)
                    return (false, new[] { "User không tồn tại." });

                if (string.IsNullOrWhiteSpace(newPassword))
                    return (false, new[] { "Mật khẩu mới không được để trống." });

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

                if (result.Succeeded)
                {
                    return (true, Array.Empty<string>());
                }
                else
                {
                    var errors = result.Errors.Select(e => e.Description).ToArray();
                    return (false, errors);
                }
            }
            catch (Exception ex)
            {
                return (false, new[] { $"Lỗi hệ thống: {ex.Message}" });
            }
        }
      
        public async Task<bool> CheckEmailExitsAsync(string email)
        {
           return await _userManager.FindByEmailAsync(email) != null;
        }
    }
}
