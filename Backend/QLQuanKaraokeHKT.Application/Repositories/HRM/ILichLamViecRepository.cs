using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Application.Repositories;

namespace QLQuanKaraokeHKT.Application.Repositories.HRM
{
    public interface ILichLamViecRepository : IGenericRepository<LichLamViec, int>
    {
        Task<List<LichLamViec>> GetLichLamViecByNhanVienAsync(Guid maNhanVien);

        Task<List<LichLamViec>> GetLichLamViecByRangeAsync(DateOnly start, DateOnly end);

        Task<bool> UpdateLichLamViecForYeuCauChuyenCaAsync(int maLichLamViec, DateOnly ngayLamViecMoi, int maCaMoi);

    }
}