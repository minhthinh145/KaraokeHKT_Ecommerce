using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Auth;
using QLQuanKaraokeHKT.Infrastructure.Data;
using QLQuanKaraokeHKT.Infrastructure.Repositories.Base;

namespace QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.Auth
{
    public class SignUpRepository : GenericRepository<TaiKhoan, Guid>, ISignUpRepository
    {
        private readonly UserManager<TaiKhoan> _userManager;

        public SignUpRepository(QlkaraokeHktContext context, UserManager<TaiKhoan> userManager) : base(context)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }
        public async Task<TaiKhoan?> FindByEmailSignUpAsync(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    return null;

                var normalizedEmail = email.ToUpper();

                var user = await _context.Users
                    .Where(u => u.NormalizedEmail == normalizedEmail)
                    .FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                try
                {
                    var userFromManager = await _userManager.FindByEmailAsync(email);
                    return userFromManager;
                }
                catch
                {
                    try
                    {
                        var users = await _context.Users
                            .Where(u => u.NormalizedEmail == email.ToUpper())
                            .ToListAsync();

                        return users.FirstOrDefault();
                    }
                    catch
                    {

                        return null;
                    }
                }
            }
        }

        public async Task<bool> IsEmailExistsForSignUpAsync(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    return false;

                var user = await FindByEmailSignUpAsync(email);
                return user != null;
            }
            catch
            {
                return true;
            }
        }

        public async Task<(IdentityResult Result, TaiKhoan? User)> CreateCustomerAccountForSignUpAsync(
            TaiKhoan user,
            string password,
            KhachHang khachHang)
        {
            try
            {
                var existingUser = await FindByEmailSignUpAsync(user.Email);
                if (existingUser != null)
                {
                    return (IdentityResult.Failed(new IdentityError { Description = "Email đã được sử dụng." }), null);
                }

                var createResult = await _userManager.CreateAsync(user, password);
                if (!createResult.Succeeded)
                {
                    return (createResult, null);
                }


                // 3. Assign role KhachHang
                var roleResult = await _userManager.AddToRoleAsync(user, "KhachHang");
                if (!roleResult.Succeeded)
                {
                    return (roleResult, null);
                }

                khachHang.MaTaiKhoan = user.Id;
                _context.KhachHangs.Add(khachHang);


                return (IdentityResult.Success, user);
            }
            catch (Exception ex)
            {
                return (IdentityResult.Failed(new IdentityError { Description = $"Lỗi hệ thống: {ex.Message}" }), null);
            }
        }
    }
}
