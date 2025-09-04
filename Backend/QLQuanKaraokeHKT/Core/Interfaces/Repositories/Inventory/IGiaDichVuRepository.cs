using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Base;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.Inventory
{
    public interface IGiaDichVuRepository : IGenericRepository<GiaDichVu, int>
    {
        Task<GiaDichVu?> GetCurrentPriceAsync(int maSanPham, int? maCa = null, DateOnly? ngayApDung = null);

        Task<List<GiaDichVu>> GetPricesByProductAsync(int maSanPham);

        Task<int> BulkUpdateStatusByProductAsync(int maSanPham, string newStatus, string currentStatus = "HieuLuc");

    }
}