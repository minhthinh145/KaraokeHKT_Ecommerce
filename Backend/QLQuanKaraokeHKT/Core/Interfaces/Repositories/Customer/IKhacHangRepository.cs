using QLQuanKaraokeHKT.Core.Entities;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.Customer
{
    public interface IKhacHangRepository
    {
        /// <summary>
        /// Lấy danh sách tất cả khách hàng
        /// </summary>
        /// <returns>Danh sách khách hàng</returns>
        Task<IList<KhachHang>> GetAllAsync();

        /// <summary>
        /// Lấy khách hàng theo ID
            /// </summary>
        /// <param name="maKhachHang">ID khách hàng</param>
        /// <returns>Thông tin khách hàng hoặc null nếu không tìm thấy</returns>
        Task<KhachHang?> GetByIdAsync(Guid maKhachHang);

        /// <summary>
        /// Lấy khách hàng theo ID tàikhooản
        /// </summary>
        /// <param name="maKhachHang">ID khách hàng</param>
        /// <returns>Thông tin khách hàng hoặc null nếu không tìm thấy</returns>
        Task<KhachHang?> GetByAccountIdAsync(Guid maTaiKhoan);


        /// <summary>
        /// Tạo mới khách hàng với tài khoản liên kết
        /// </summary>
        /// <param name="khachHang">Thông tin khách hàng</param>
        /// <param name="maTaiKhoan">ID tài khoản liên kết</param>
        /// <returns>Khách hàng vừa được tạo</returns>
        Task<KhachHang> CreateWithAccountAsync(KhachHang khachHang, Guid maTaiKhoan);

        /// <summary>
        /// Tạo mới khách hàng với tài khoản liên kết
        /// </summary>
        /// <param name="khachHang">Thông tin khách hàng</param>
        /// <returns>Khách hàng vừa được tạo</returns>
        Task<KhachHang> CreateKhacHangAsync(KhachHang khachHang);

        /// <summary>
        /// Cập nhật thông tin khách hàng
        /// </summary>
        /// <param name="khachHang">Thông tin khách hàng cần cập nhật</param>
        /// <returns>True nếu cập nhật thành công, False nếu thất bại</returns>
        Task<bool> UpdateAsync(KhachHang khachHang);

        /// <summary>
        /// Xóa khách hàng theo ID tài khoản
        /// </summary>
        /// <param name="maTaiKhoan">ID tài khoản liên kết</param>
        /// <returns>True nếu xóa thành công, False nếu thất bại</returns>
        Task<bool> DeleteByAccountIdAsync(Guid maTaiKhoan);

        Task<List<KhachHang>> GetAllWithTaiKhoanAsync();



    }
}