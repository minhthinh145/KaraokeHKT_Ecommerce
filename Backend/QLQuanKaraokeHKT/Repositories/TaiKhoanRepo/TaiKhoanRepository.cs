using Microsoft.AspNetCore.Identity;
using QLQuanKaraokeHKT.Models;

namespace QLQuanKaraokeHKT.Repositories.TaiKhoanRepo
{
    public class TaiKhoanRepository : ITaiKhoanRepository
    {
        private readonly RoleManager<VaiTro> _roleManager;
        private readonly UserManager<TaiKhoan> _userManager;

        public TaiKhoanRepository(UserManager<TaiKhoan> userManager, RoleManager<VaiTro> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task AddToRoleAsync(TaiKhoan user, string role)
        {
            await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<bool> CheckPasswordAsync(TaiKhoan user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task CreateRoleAsync(string roleName)
        {
            await _roleManager.CreateAsync(new VaiTro { Name = roleName, NormalizedName = roleName.ToUpper() });
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

        public async Task<IList<string>> GetUserRolesAsync(TaiKhoan user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
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
        public async Task<IdentityResult> UpdateUserAsync(TaiKhoan user)
        {
            return await _userManager.UpdateAsync(user);
        }
    }
}