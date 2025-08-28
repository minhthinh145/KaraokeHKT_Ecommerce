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
                var existingUser = await FindByEmailSignUpAsync(user.Email);
                if (existingUser != null)
                {
                    await transaction.RollbackAsync();
                    return (IdentityResult.Failed(new IdentityError { Description = "Email đã được sử dụng." }), null);
                }

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
