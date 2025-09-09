using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Base;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.HRM
{
    public interface INhanVienRepository : IGenericRepository<NhanVien, Guid>
    {
        Task<NhanVien?> GetNhanVienByTaiKhoanIdAsync(Guid maTaiKhoan);
        Task<NhanVien> GetByIdWithTaiKhoanAsync(Guid maNhanVien);
        Task<List<NhanVien>> GetAllByLoaiTaiKhoanWithTaiKhoanAsync(string loaiTaiKhoan);
        Task<List<NhanVien>> GetAllNhanVienWithTaiKhoanAsync();
        Task<bool> UpdateNhanVienDaNghiViecAsync(Guid maNhanVien, bool daNghiViec);
    }
}
