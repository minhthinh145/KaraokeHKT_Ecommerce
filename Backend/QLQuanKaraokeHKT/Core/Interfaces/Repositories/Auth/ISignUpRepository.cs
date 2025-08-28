using Microsoft.AspNetCore.Identity;
using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Base;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.Auth
{
    public interface ISignUpRepository : IGenericRepository<TaiKhoan, Guid>
    {

        /// <summary>
        /// Tìm kiếm người dùng theo email - Chuyên dụng cho quá trình đăng ký
        /// Sử dụng FirstOrDefaultAsync để tránh lỗi duplicate records
        /// </summary>
        /// <param name="email">Địa chỉ email cần kiểm tra</param>
        /// <returns>Trả về đối tượng TaiKhoan nếu tìm thấy, ngược lại trả về null</returns>
        Task<TaiKhoan?> FindByEmailSignUpAsync(string email);

        /// <summary>
        /// Kiểm tra email có tồn tại trong hệ thống không - Chuyên dụng cho đăng ký
        /// </summary>
        /// <param name="email">Email cần kiểm tra</param>
        /// <returns>True nếu email đã tồn tại, False nếu chưa</returns>
        Task<bool> IsEmailExistsForSignUpAsync(string email);

        /// <summary>
        /// Tạo tài khoản khách hàng hoàn chỉnh trong một transaction - Chuyên dụng cho đăng ký
        /// </summary>
        /// <param name="user">Thông tin tài khoản</param>
        /// <param name="password">Mật khẩu</param>
        /// <param name="khachHang">Thông tin khách hàng</param>
        /// <returns>Tuple chứa kết quả và user đã tạo</returns>
        Task<(IdentityResult Result, TaiKhoan? User)> CreateCustomerAccountForSignUpAsync(
            TaiKhoan user,
            string password,
            KhachHang khachHang);
    }
}
