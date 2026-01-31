using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Application.Repositories;

namespace QLQuanKaraokeHKT.Application.Repositories.HRM
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