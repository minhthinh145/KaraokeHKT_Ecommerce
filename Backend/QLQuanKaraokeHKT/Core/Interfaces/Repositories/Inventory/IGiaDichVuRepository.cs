using QLQuanKaraokeHKT.Core.Entities;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.Inventory
{
    public interface IGiaDichVuRepository
    {
        Task<GiaDichVu> CreateGiaDichVuAsync(GiaDichVu giaDichVu);
        Task<GiaDichVu?> GetGiaHienTaiByMaSanPhamAsync(int maSanPham);
        Task<List<GiaDichVu>> GetGiaDichVuByMaSanPhamAsync(int maSanPham);
        Task<bool> UpdateGiaDichVuStatusAsync(int maGiaDichVu, string trangThai);

        /// <summary>
        /// Lấy giá hiện tại theo sản phẩm và ca (ưu tiên giá theo ca, fallback về giá chung)
        /// </summary>
        Task<GiaDichVu?> GetGiaHienTaiByMaSanPhamAndCaAsync(int maSanPham, int? maCa = null, DateOnly? ngayApDung = null);

        /// <summary>
        /// Lấy tất cả giá theo sản phẩm, có thể filter theo ca
        /// </summary>
        Task<List<GiaDichVu>> GetGiaDichVuByMaSanPhamAndCaAsync(int maSanPham, int? maCa = null);

        /// <summary>
        /// Vô hiệu hóa tất cả giá cũ của sản phẩm (có thể filter theo ca)
        /// </summary>
        Task<bool> UpdateGiaDichVuStatusByMaSanPhamAsync(int maSanPham, string trangThai, int? maCa = null);

        Task<GiaDichVu?> GetGiaDichVuHienTaiAsync(int maSanPham, DateOnly ngayApDung, int? maCa = null);
    }
}