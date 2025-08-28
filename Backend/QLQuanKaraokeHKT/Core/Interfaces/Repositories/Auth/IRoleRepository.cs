using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Base;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.Auth
{
    public interface IRoleRepository : IGenericRepository<VaiTro, Guid>
    {
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


        /// <summary>
        /// Lấy mô tả vai trò từ database
        /// </summary>
        /// <param name="roleName">Tên vai trò</param>
        /// <returns>Mô tả vai trò hoặc tên vai trò nếu không tìm thấy</returns>
        Task<string> GetRoleDescriptionAsync(string roleName);


        /// <summary>
        /// Lấy mã role từ mô tả vai trò
        /// </summary>
        /// <param name="roleDescription">Mô tả vai trò (VD: "Nhân viên kho")</param>
        /// <returns>Mã role (VD: "NhanVienKho") hoặc null nếu không tìm thấy</returns>
        Task<string?> GetRoleCodeFromDescriptionAsync(string roleDescription);


    }
}
