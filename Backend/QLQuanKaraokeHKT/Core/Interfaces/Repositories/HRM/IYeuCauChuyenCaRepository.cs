using QLQuanKaraokeHKT.Core.Entities;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.HRM
{
    public interface IYeuCauChuyenCaRepository
    {
        Task<YeuCauChuyenCa> CreateYeuCauChuyenCaAsync(YeuCauChuyenCa yeuCauChuyenCa);
        Task<List<YeuCauChuyenCa>> GetAllYeuCauChuyenCaAsync();
        Task<List<YeuCauChuyenCa>> GetYeuCauChuyenCaByNhanVienAsync(Guid maNhanVien);
        Task<List<YeuCauChuyenCa>> GetYeuCauChuyenCaChuaPheDuyetAsync();
        Task<List<YeuCauChuyenCa>> GetYeuCauChuyenCaDaPheDuyetAsync();
        Task<YeuCauChuyenCa?> GetYeuCauChuyenCaByIdAsync(int maYeuCau);
        Task<bool> UpdateYeuCauChuyenCaAsync(YeuCauChuyenCa yeuCauChuyenCa);
        Task<bool> PheDuyetYeuCauChuyenCaAsync(int maYeuCau, bool ketQuaPheDuyet, string? ghiChuPheDuyet);
        Task<bool> CheckConflictLichLamViecAsync(Guid maNhanVien, DateOnly ngayLamViec, int maCa);
        Task<bool> DeleteYeuCauChuyenCaAsync(int maYeuCau);
    }
}