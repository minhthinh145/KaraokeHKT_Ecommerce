using Microsoft.AspNetCore.Identity;
using QLQuanKaraokeHKT.Core.Entities;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.Auth
{
    public interface IIdentityRepository
    {
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

    }
}
