using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Models;

namespace QLQuanKaraokeHKT.Repositories.TaiKhoanRepo
{
    public class TaiKhoanRepository : ITaiKhoanRepository
    {
        private readonly RoleManager<VaiTro> _roleManager;
        private readonly UserManager<TaiKhoan> _userManager;
        private readonly QlkaraokeHktContext _context;

        public TaiKhoanRepository(UserManager<TaiKhoan> userManager, RoleManager<VaiTro> roleManager, QlkaraokeHktContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context ?? throw new ArgumentNullException(nameof(context));
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

        public async Task<string> GetRoleDescriptionAsync(string roleName)
        {
            try
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                // Giả sử VaiTro có property Description
                return role?.moTa ?? roleName;
            }
            catch
            {
                return roleName; // Fallback về tên vai trò nếu có lỗi
            }
        }

        public async Task<bool> UpdateUserRoleAsync(TaiKhoan user, string newRoleName)
        {
            try
            {
                var currentRoles = await _userManager.GetRolesAsync(user);

                // Xóa tất cả role hiện tại
                if (currentRoles.Any())
                {
                    var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    if (!removeResult.Succeeded)
                        return false;
                }

                var addResult = await _userManager.AddToRoleAsync(user, newRoleName);
                return addResult.Succeeded;
            }
            catch
            {
                return false;
            }
        }

        public async Task RemoveFromRoleAsync(TaiKhoan user, string roleName)
        {
            await _userManager.RemoveFromRoleAsync(user, roleName);
        }

        // Thêm methods này vào class TaiKhoanRepository:

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

        public async Task<string?> GetRoleCodeFromDescriptionAsync(string roleDescription)
        {
            var role = await _roleManager.Roles
                .FirstOrDefaultAsync(r => r.moTa == roleDescription || r.Name == roleDescription);
            if (role != null)
                {
                return role.Name;
            }
            else
            {
                return null;
            }

        }

        public async Task<bool> LockAccountAsync(Guid maTaiKhoan)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(maTaiKhoan.ToString());
                if (user == null)
                {
                    return false; // Tài khoản không tồn tại
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

                // Unlock tài khoản
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

        /// <summary>
        /// Tìm kiếm người dùng theo email - Chuyên dụng cho quá trình đăng ký
        /// Sử dụng FirstOrDefaultAsync để tránh lỗi "Sequence contains more than one element"
        /// </summary>
        /// <param name="email">Địa chỉ email cần kiểm tra</param>
        /// <returns>
        /// Trả về đối tượng <see cref="TaiKhoan"/> nếu tìm thấy, ngược lại trả về <c>null</c>.
        /// </returns>
        public async Task<TaiKhoan?> FindByEmailSignUpAsync(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    return null;

                var normalizedEmail = email.ToUpper();

                // ✅ Sử dụng FirstOrDefaultAsync để tránh lỗi duplicate records
                // Ưu tiên query trực tiếp database để có kết quả realtime nhất
                var user = await _context.Users
                    .Where(u => u.NormalizedEmail == normalizedEmail)
                    .FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                // ✅ Fallback strategy nếu có lỗi với direct context query
                try
                {
                    // Thử với UserManager nếu context query fail
                    var userFromManager = await _userManager.FindByEmailAsync(email);
                    return userFromManager;
                }
                catch
                {
                    // ✅ Final fallback - query manual với exception handling
                    try
                    {
                        var users = await _context.Users
                            .Where(u => u.NormalizedEmail == email.ToUpper())
                            .ToListAsync();

                        // Trả về user đầu tiên nếu có, hoặc null
                        return users.FirstOrDefault();
                    }
                    catch
                    {
                        // Log error nếu cần thiết
                        // _logger?.LogError(ex, "Error in FindByEmailSignUpAsync for email: {Email}", email);
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// Kiểm tra email có tồn tại trong hệ thống không - Chuyên dụng cho đăng ký
        /// </summary>
        /// <param name="email">Email cần kiểm tra</param>
        /// <returns>True nếu email đã tồn tại, False nếu chưa</returns>
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
                // Trong trường hợp có lỗi, return true để an toàn (ngăn tạo duplicate)
                return true;
            }
        }

        /// <summary>
        /// Tạo tài khoản khách hàng hoàn chỉnh trong một transaction - Chuyên dụng cho đăng ký
        /// </summary>
        /// <param name="user">Thông tin tài khoản</param>
        /// <param name="password">Mật khẩu</param>
        /// <param name="khachHang">Thông tin khách hàng</param>
        /// <returns>Tuple chứa kết quả và user đã tạo</returns>
        public async Task<(IdentityResult Result, TaiKhoan? User)> CreateCustomerAccountForSignUpAsync(
            TaiKhoan user,
            string password,
            KhachHang khachHang)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // ✅ Kiểm tra email tồn tại một lần nữa trong transaction
                var existingUser = await FindByEmailSignUpAsync(user.Email);
                if (existingUser != null)
                {
                    await transaction.RollbackAsync();
                    return (IdentityResult.Failed(new IdentityError { Description = "Email đã được sử dụng." }), null);
                }

                // 1. Tạo tài khoản user
                var createResult = await _userManager.CreateAsync(user, password);
                if (!createResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return (createResult, null);
                }

                // 2. ✅ Flush để đảm bảo user được tạo trong database
                await _context.SaveChangesAsync();

                // 3. Assign role KhachHang
                var roleResult = await _userManager.AddToRoleAsync(user, "KhachHang");
                if (!roleResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return (roleResult, null);
                }

                // 4. ✅ Set foreign key và tạo KhachHang record
                khachHang.MaTaiKhoan = user.Id;
                _context.KhachHangs.Add(khachHang);

                // 5. Final save
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return (IdentityResult.Success, user);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (IdentityResult.Failed(new IdentityError { Description = $"Lỗi hệ thống: {ex.Message}" }), null);
            }
        }
    }
}