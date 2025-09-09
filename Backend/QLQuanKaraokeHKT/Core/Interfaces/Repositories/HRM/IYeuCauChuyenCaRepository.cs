using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Base;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.HRM
{
    public interface IYeuCauChuyenCaRepository : IGenericRepository<YeuCauChuyenCa, int>
    {
        Task<List<YeuCauChuyenCa>> GetYeuCauChuyenCaByNhanVienAsync(Guid maNhanVien);
        Task<List<YeuCauChuyenCa>> GetYeuCauChuyenCaChuaPheDuyetAsync();
        Task<List<YeuCauChuyenCa>> GetYeuCauChuyenCaDaPheDuyetAsync();
        Task<bool> PheDuyetYeuCauChuyenCaAsync(int maYeuCau, bool ketQuaPheDuyet, string? ghiChuPheDuyet);
        Task<bool> CheckConflictLichLamViecAsync(Guid maNhanVien, DateOnly ngayLamViec, int maCa);
    }
}