using Microsoft.AspNetCore.Identity;
using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Application.Repositories.Auth;


namespace QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.Auth
{
    public class AccountManagementRepository : IAccountManagementRepository
    {
        private readonly UserManager<TaiKhoan> _userManager;
        public AccountManagementRepository(UserManager<TaiKhoan> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<bool> LockAccountAsync(Guid maTaiKhoan)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(maTaiKhoan.ToString());
                if (user == null)
                {
                    return false; 
                }

                user.LockoutEnabled = true;
                user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(100);
                user.daBiKhoa = true;

                var result = await _userManager.UpdateAsync(user);
                return result.Succeeded;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UnlockAccountAsync(Guid maTaiKhoan)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(maTaiKhoan.ToString());
                if (user == null)
                {
                    return false;
                }

                user.LockoutEnabled = false;
                user.LockoutEnd = null;
                user.daBiKhoa = false;

                var result = await _userManager.UpdateAsync(user);
                return result.Succeeded;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(TaiKhoan user)
        {
            try
            {
                if (user == null)
                    return false;

                var result = await _userManager.DeleteAsync(user);
                return result.Succeeded;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteUserByIdAsync(Guid userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                    return false;

                return await DeleteUserAsync(user);
            }
            catch
            {
                return false;
            }
        }
    }
}
