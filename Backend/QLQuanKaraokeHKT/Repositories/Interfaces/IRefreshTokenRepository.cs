using QLQuanKaraokeHKT.Models;

namespace QLQuanKaraokeHKT.Repositories.Interfaces
{
    public interface IRefreshTokenRepository
    {
        /// <summary>
        /// Lưu refresh token mới vào database
        /// </summary>
        /// <param name="userId">ID của user</param>
        /// <param name="token">Refresh token string</param>
        /// <param name="created">Thời gian tạo</param>
        /// <param name="expires">Thời gian hết hạn</param>
        /// <returns></returns>
        Task SaveAsync(Guid userId, string token, DateTime created, DateTime expires);

        /// <summary>
        /// Tìm refresh token theo token string
        /// </summary>
        /// <param name="token">Refresh token string</param>
        /// <returns>RefreshToken object hoặc null nếu không tìm thấy</returns>
        Task<RefreshToken?> FindByTokenAsync(string token);

        /// <summary>
        /// Thu hồi (revoke) một refresh token cụ thể
        /// </summary>
        /// <param name="token">Refresh token string cần thu hồi</param>
        /// <returns></returns>
        Task RevokeAsync(string token);

        /// <summary>
        /// Thu hồi tất cả refresh token của một user cụ thể
        /// </summary>
        /// <param name="userId">ID của user</param>
        /// <returns></returns>
        Task RevokeAllByUserIdAsync(Guid userId);

        /// <summary>
        /// Lấy tất cả refresh token còn hiệu lực của một user
        /// </summary>
        /// <param name="userId">ID của user</param>
        /// <returns>Danh sách refresh token còn hiệu lực</returns>
        Task<IList<RefreshToken>> GetActiveTokensByUserIdAsync(Guid userId);

        /// <summary>
        /// Xóa các refresh token đã hết hạn khỏi database
        /// </summary>
        /// <returns>Số lượng token đã được xóa</returns>
        Task<int> CleanupExpiredTokensAsync();

        /// <summary>
        /// Kiểm tra xem refresh token có còn hiệu lực không
        /// </summary>
        /// <param name="token">Refresh token string</param>
        /// <returns>True nếu token còn hiệu lực, false nếu không</returns>
        Task<bool> IsTokenValidAsync(string token);

        /// <summary>
        /// Đếm số lượng refresh token còn hiệu lực của một user
        /// </summary>
        /// <param name="userId">ID của user</param>
        /// <returns>Số lượng token còn hiệu lực</returns>
        Task<int> CountActiveTokensByUserIdAsync(Guid userId);
    }
}