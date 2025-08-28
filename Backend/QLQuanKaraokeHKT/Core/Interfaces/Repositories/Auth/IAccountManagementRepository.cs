using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Base;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.Auth
{
    public interface IAccountManagementRepository : IGenericRepository<TaiKhoan,Guid>   
    {

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

    }
}
