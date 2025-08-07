using Microsoft.AspNetCore.Identity;
using QLQuanKaraokeHKT.Models;

namespace QLQuanKaraokeHKT.Repositories.TaiKhoanRepo
{
    public interface ITaiKhoanRepository
    {
        /// <summary>
        /// Tìm kiếm người dùng dựa trên địa chỉ email.
        /// </summary>
        /// <param name="email">Địa chỉ email của người dùng.</param>
        /// <returns>
        /// Trả về đối tượng <see cref="TaiKhoan"/> nếu tìm thấy, ngược lại trả về <c>null</c>.
        /// </returns>
        Task<TaiKhoan?> FindByEmailAsync(string email);

        /// <summary>
        /// Find a user by UserID
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>
        ///  Return TaiKhoan if find success , null if not
        /// </returns>
        Task<TaiKhoan?> FindByUserIDAsync(string userId);

        /// <summary>
        /// Kiểm tra tính hợp lệ của mật khẩu người dùng.
        /// </summary>
        /// <param name="user">Đối tượng người dùng cần kiểm tra.</param>
        /// <param name="password">Mật khẩu cần xác thực.</param>
        /// <returns>
        /// <c>true</c> nếu mật khẩu hợp lệ, ngược lại trả về <c>false</c>.
        /// </returns>
        Task<bool> CheckPasswordAsync(TaiKhoan user, string password);

        /// <summary>
        /// Lấy danh sách vai trò của một người dùng.
        /// </summary>
        /// <param name="user">Đối tượng người dùng.</param>
        /// <returns>
        /// Danh sách các vai trò của người dùng.
        /// </returns>
        Task<IList<string>> GetUserRolesAsync(TaiKhoan user);

        /// <summary>
        /// Tạo một người dùng mới trong cơ sở dữ liệu.
        /// </summary>
        /// <param name="user">Đối tượng người dùng cần tạo.</param>
        /// <param name="password">Mật khẩu của người dùng.</param>
        /// <returns>
        /// Đối tượng <see cref="IdentityResult"/> chứa thông tin về kết quả tạo người dùng.
        /// </returns>
        Task<IdentityResult> CreateUserAsync(TaiKhoan user, string password);

        /// <summary>
        /// Kiểm tra xem một vai trò có tồn tại trong hệ thống không.
        /// </summary>
        /// <param name="roleName">Tên vai trò cần kiểm tra.</param>
        /// <returns>
        /// <c>true</c> nếu vai trò tồn tại, ngược lại trả về <c>false</c>.
        /// </returns>
        Task<bool> RoleExistsAsync(string roleName);

        /// <summary>
        /// Tạo một vai trò mới trong hệ thống.
        /// </summary>
        /// <param name="roleName">Tên vai trò cần tạo.</param>
        /// <returns></returns>
        Task CreateRoleAsync(string roleName);

        /// <summary>
        /// Thêm người dùng vào một vai trò cụ thể.
        /// </summary>
        /// <param name="user">Đối tượng người dùng.</param>
        /// <param name="role">Tên vai trò.</param>
        /// <returns></returns>
        Task AddToRoleAsync(TaiKhoan user, string role);

        /// <summary>
        /// Cập nhật thông tin người dùng.
        /// </summary>
        /// <param name="user">Đối tượng người dùng cần cập nhật.</param>
        /// <returns>
        /// Đối tượng <see cref="IdentityResult"/> chứa thông tin về kết quả cập nhật.
        /// </returns>
        Task<IdentityResult> UpdateUserAsync(TaiKhoan user);

        /// <summary>
        /// Cập nhật mật khẩu người dùng.
        /// </summary>
        /// <param name="user">Đối tượng người dùng cần cập nhật mật khẩu.</param>
        /// <param name="newPassword">Mật khẩu mới.</param>
        /// <returns>
        /// Tuple chứa thông tin thành công/thất bại và danh sách lỗi nếu có.
        /// </returns>
        Task<(bool Success, string[] Errors)> UpdatePasswordAsync(TaiKhoan user, string newPassword);

        /// <summary>
        /// Lấy mô tả vai trò từ database
        /// </summary>
        /// <param name="roleName">Tên vai trò</param>
        /// <returns>Mô tả vai trò hoặc tên vai trò nếu không tìm thấy</returns>
        Task<string> GetRoleDescriptionAsync(string roleName);


        /// <summary>
        /// Cập nhật role của user - xóa role cũ và thêm role mới
        /// </summary>
        /// <param name="user">User cần cập nhật role</param>
        /// <param name="newRoleName">Tên role mới</param>
        /// <returns>True nếu thành công</returns>
        Task<bool> UpdateUserRoleAsync(TaiKhoan user, string newRoleName);

        /// <summary>
        /// Xóa user khỏi role
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="roleName">Tên role cần xóa</param>
        /// <returns></returns>
        Task RemoveFromRoleAsync(TaiKhoan user, string roleName);

        // Thêm method này vào interface:

        /// <summary>
        /// Xóa tài khoản người dùng khỏi hệ thống
        /// </summary>
        /// <param name="user">Tài khoản cần xóa</param>
        /// <returns>True nếu xóa thành công</returns>
        Task<bool> DeleteUserAsync(TaiKhoan user);

        /// <summary>
        /// Xóa tài khoản theo ID
        /// </summary>
        /// <param name="userId">ID tài khoản cần xóa</param>
        /// <returns>True nếu xóa thành công</returns>
        Task<bool> DeleteUserByIdAsync(Guid userId);

        /// <summary>
        /// Xóa tài khoản và tất cả dữ liệu liên quan (roles, refresh tokens, OTP...)
        /// </summary>
        /// <param name="user">Tài khoản cần xóa</param>
        /// <returns>True nếu xóa thành công</returns>
        Task<bool> DeleteUserWithRelatedDataAsync(TaiKhoan user);

        // Thêm method này:

        /// <summary>
        /// Lấy mã role từ mô tả vai trò
        /// </summary>
        /// <param name="roleDescription">Mô tả vai trò (VD: "Nhân viên kho")</param>
        /// <returns>Mã role (VD: "NhanVienKho") hoặc null nếu không tìm thấy</returns>
        Task<string?> GetRoleCodeFromDescriptionAsync(string roleDescription);
        /// <summary>
        /// Lock tài khoản người dùng theo ID tài khoản
        /// </summary>
        /// <param name="maTaIkhoan"></param>
        /// <returns></returns>
        Task<bool> LockAccountAsync(Guid maTaIkhoan);
        /// <summary>
        /// Unlock tài khoản người dùng theo ID tài khoản
        /// </summary>
        /// <param name="maTaIkhoan"></param>
        /// <returns></returns>
        Task<bool> UnlockAccountAsync(Guid maTaIkhoan);
    }
}