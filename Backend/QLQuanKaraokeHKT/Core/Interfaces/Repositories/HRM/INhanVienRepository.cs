using QLQuanKaraokeHKT.Core.Entities;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.HRM
{
    public interface INhanVienRepository
    {
        Task<List<NhanVien>> GetAllNhanVienAsync();
        Task<NhanVien?> GetNhanVienByIdAsync(Guid maNhanVien);
        Task<NhanVien?> GetNhanVienByTaiKhoanIdAsync(Guid maTaiKhoan);
        Task<NhanVien> CreateNhanVienAsync(NhanVien nhanVien);
        Task<bool> UpdateNhanVienAsync(NhanVien nhanVien);
        Task<bool> DeleteNhanVienByTaiKhoanIdAsync(Guid maTaiKhoan);
        Task<bool> DeleteNhanVienByIdAsync(Guid maNhanVien);
        Task<NhanVien> GetByIdWithTaiKhoanAsync(Guid maNhanVien);
        Task<List<NhanVien>> GetAllByLoaiTaiKhoanWithTaiKhoanAsync(string loaiTaiKhoan);
        Task<List<NhanVien>> GetAllNhanVienWithTaiKhoanAsync();
        Task<bool> UpdateNhanVienDaNghiViecAsync(Guid maNhanVien, bool daNghiViec);
    }
}
