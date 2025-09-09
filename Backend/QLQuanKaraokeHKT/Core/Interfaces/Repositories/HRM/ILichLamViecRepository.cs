using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Base;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.HRM
{
    public interface ILichLamViecRepository : IGenericRepository<LichLamViec, int>
    {
        Task<List<LichLamViec>> GetLichLamViecByNhanVienAsync(Guid maNhanVien);

        Task<List<LichLamViec>> GetLichLamViecByRangeAsync(DateOnly start, DateOnly end);

        Task<bool> UpdateLichLamViecForYeuCauChuyenCaAsync(int maLichLamViec, DateOnly ngayLamViecMoi, int maCaMoi);

    }
}