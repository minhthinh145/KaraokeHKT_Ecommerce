using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.QLNhanSuDTOs;

namespace QLQuanKaraokeHKT.Core.Interfaces.Services.HRM
{
    public interface IQLYeuCauChuyenCaService
    {
        Task<ServiceResult> CreateYeuCauChuyenCaAsync(AddYeuCauChuyenCaDTO addYeuCauChuyenCaDto);
        Task<ServiceResult> GetAllYeuCauChuyenCaAsync();
        Task<ServiceResult> GetYeuCauChuyenCaByNhanVienAsync(Guid maNhanVien);
        Task<ServiceResult> GetYeuCauChuyenCaChuaPheDuyetAsync();
        Task<ServiceResult> GetYeuCauChuyenCaDaPheDuyetAsync();
        Task<ServiceResult> GetYeuCauChuyenCaByIdAsync(int maYeuCau);
        Task<ServiceResult> PheDuyetYeuCauChuyenCaAsync(PheDuyetYeuCauChuyenCaDTO pheDuyetDto);
        Task<ServiceResult> DeleteYeuCauChuyenCaAsync(int maYeuCau);
    }
}