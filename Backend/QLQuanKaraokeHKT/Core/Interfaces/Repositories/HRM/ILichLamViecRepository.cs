using QLQuanKaraokeHKT.Core.Entities;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.HRM
{
    public interface ILichLamViecRepository
    {
        Task<LichLamViec> CreateLichLamViecWithNhanVienAsync(LichLamViec lichLamViec);
        Task<LichLamViec?> GetLichLamViecByIdAsync(int maLichLamviec);
        Task<List<LichLamViec>> GetAllLichLamViecAsync();
        Task<List<LichLamViec>> GetLichLamViecByNhanVienAsync(Guid maNhanVien);
        /// <summary>
        /// Rettrives all work schedules in a field of time
        /// </summary>
        /// <param name="start">TimeStart</param>
        /// <param name="end">TimeEnd</param>
        /// <returns></returns>
        Task<List<LichLamViec>> GetLichLamViecByRangeAsync(DateOnly start, DateOnly end);
        Task<List<NhanVien>> GetNhanVienByLichLamViecRangeAsync(DateOnly start, DateOnly end);
        Task<bool> UpdateLichLamViecAsync(LichLamViec lichLamViec);
        /// <summary>
        /// Cập nhật lịch làm việc cho yêu cầu chuyển ca đã được duyệt
        /// </summary>
        /// <param name="maLichLamViec">Mã lịch làm việc cần cập nhật</param>
        /// <param name="ngayLamViecMoi">Ngày làm việc mới</param>
        /// <param name="maCaMoi">Mã ca mới</param>
        /// <returns>True nếu cập nhật thành công</returns>
        Task<bool> UpdateLichLamViecForYeuCauChuyenCaAsync(int maLichLamViec, DateOnly ngayLamViecMoi, int maCaMoi);
        Task<bool> DeleteLichLamViecByIdAsync(int maLichLamviec);
    }
}