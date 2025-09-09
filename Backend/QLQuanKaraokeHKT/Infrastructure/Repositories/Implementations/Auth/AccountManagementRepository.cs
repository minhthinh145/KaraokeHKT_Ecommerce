using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Auth;
using QLQuanKaraokeHKT.Infrastructure.Data;
using QLQuanKaraokeHKT.Infrastructure.Repositories.Base;

namespace QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.Auth
{
    public class AccountManagementRepository : GenericRepository<TaiKhoan, Guid>,IAccountManagementRepository
    {
        private readonly UserManager<TaiKhoan> _userManager;

        public AccountManagementRepository(QlkaraokeHktContext context, UserManager<TaiKhoan> userManager) : base(context) 
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

        public async Task<bool> DeleteUserWithRelatedDataAsync(TaiKhoan user)
        {
            try
            {
                if (user == null)
                    return false;

                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    // 1. Xóa refresh tokens
                    var refreshTokens = await _context.RefreshTokens
                        .Where(rt => rt.MaTaiKhoan == user.Id)
                        .ToListAsync();
                    if (refreshTokens.Any())
                    {
                        _context.RefreshTokens.RemoveRange(refreshTokens);
                    }

                    // 2. Xóa OTP codes
                    var otpCodes = await _context.MaOtps
                        .Where(otp => otp.MaTaiKhoan == user.Id)
                        .ToListAsync();
                    if (otpCodes.Any())
                    {
                        _context.MaOtps.RemoveRange(otpCodes);
                    }

                    // 3. Xóa user roles (Identity tự handle)
                    var userRoles = await _userManager.GetRolesAsync(user);
                    if (userRoles.Any())
                    {
                        await _userManager.RemoveFromRolesAsync(user, userRoles);
                    }

                    // 4. Save changes cho related data trước
                    await _context.SaveChangesAsync();

                    // 5. Xóa user account
                    var deleteResult = await _userManager.DeleteAsync(user);
                    if (!deleteResult.Succeeded)
                    {
                        await transaction.RollbackAsync();
                        return false;
                    }

                    await transaction.CommitAsync();
                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
