using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Application.Repositories;

namespace QLQuanKaraokeHKT.Application.Repositories.Inventory
{
    public interface IGiaDichVuRepository : IGenericRepository<GiaDichVu, int>
    {
        Task<GiaDichVu?> GetCurrentPriceAsync(int maSanPham, int? maCa = null, DateOnly? ngayApDung = null);

        Task<List<GiaDichVu>> GetPricesByProductAsync(int maSanPham);

        Task<int> BulkUpdateStatusByProductAsync(int maSanPham, string newStatus, string currentStatus = "HieuLuc");

    }
}