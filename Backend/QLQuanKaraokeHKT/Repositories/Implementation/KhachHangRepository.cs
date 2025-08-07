using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Models;
using QLQuanKaraokeHKT.Repositories.Interfaces;
using QLQuanKaraokeHKT.Repositories.TaiKhoanRepo;

namespace QLQuanKaraokeHKT.Repositories.Implementation
{
    public class KhacHangRepository : IKhacHangRepository
    {
        private readonly QlkaraokeHktContext _context;
        private readonly ITaiKhoanRepository _taiKhoanRepo;

        public KhacHangRepository(QlkaraokeHktContext context, ITaiKhoanRepository taiKhoanRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _taiKhoanRepo = taiKhoanRepository ?? throw new ArgumentNullException(nameof(taiKhoanRepository));
        }

        /// <summary>
        /// Lấy danh sách tất cả khách hàng
        /// </summary>
        /// <returns>Danh sách khách hàng</returns>
        public async Task<IList<KhachHang>> GetAllAsync()
        {
            try
            {
                return await _context.KhachHangs
                    .Include(k => k.MaTaiKhoanNavigation)
                    .OrderBy(k => k.TenKhachHang)
                    .ToListAsync();
            }
            catch (Exception)
            {
                return new List<KhachHang>();
            }
        }

        /// <summary>
        /// Lấy khách hàng theo ID
        /// </summary>
        /// <param name="maKhachHang">ID khách hàng</param>
        /// <returns>Thông tin khách hàng hoặc null nếu không tìm thấy</returns>
        public async Task<KhachHang?> GetByIdAsync(Guid maKhachHang)
        {
            try
            {
                return await _context.KhachHangs
                    .Include(k => k.MaTaiKhoanNavigation)
                    .FirstOrDefaultAsync(k => k.MaKhachHang == maKhachHang);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Tạo mới khách hàng với tài khoản liên kết
        /// </summary>
        /// <param name="khachHang">Thông tin khách hàng</param>
        /// <param name="maTaiKhoan">ID tài khoản liên kết</param>
        /// <returns>Khách hàng vừa được tạo</returns>
        public async Task<KhachHang> CreateWithAccountAsync(KhachHang khachHang, Guid maTaiKhoan)
        {
            try
            {
                if (khachHang == null)
                    throw new ArgumentNullException(nameof(khachHang));

                // Kiểm tra tài khoản có tồn tại không
                var checkTaiKhoan = await _taiKhoanRepo.FindByUserIDAsync(maTaiKhoan.ToString());

                if (checkTaiKhoan == null)
                    throw new InvalidOperationException($"Tài khoản với ID {maTaiKhoan} không tồn tại.");

                // Kiểm tra tài khoản đã có khách hàng chưa
                var existingCustomer = await _context.KhachHangs
                    .FirstOrDefaultAsync(k => k.MaTaiKhoan == maTaiKhoan);

                if (existingCustomer != null)
                    throw new InvalidOperationException($"Tài khoản với ID {maTaiKhoan} đã có khách hàng liên kết.");

                khachHang.MaKhachHang = Guid.NewGuid();
                khachHang.MaTaiKhoan = maTaiKhoan;

                // Validate required fields
                if (string.IsNullOrWhiteSpace(khachHang.TenKhachHang))
                    throw new ArgumentException("Tên khách hàng không được để trống.");

                await _context.KhachHangs.AddAsync(khachHang);
                await _context.SaveChangesAsync();

                // Load lại với navigation properties
                return await _context.KhachHangs
                    .Include(k => k.MaTaiKhoanNavigation)
                    .FirstAsync(k => k.MaKhachHang == khachHang.MaKhachHang);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Cập nhật thông tin khách hàng
        /// </summary>
        /// <param name="khachHang">Thông tin khách hàng cần cập nhật</param>
        /// <returns>True nếu cập nhật thành công, False nếu thất bại</returns>
        public async Task<bool> UpdateAsync(KhachHang khachHang)
        {
            try
            {
                if (khachHang == null)
                    return false;

                // Kiểm tra khách hàng có tồn tại không
                var existingCustomer = await _context.KhachHangs
                    .FirstOrDefaultAsync(k => k.MaKhachHang == khachHang.MaKhachHang);

                if (existingCustomer == null)
                    return false;

                // Validate required fields
                if (string.IsNullOrWhiteSpace(khachHang.TenKhachHang))
                    return false;

                // Update thông tin
                existingCustomer.TenKhachHang = khachHang.TenKhachHang;
                existingCustomer.Email = khachHang.Email;
                existingCustomer.NgaySinh = khachHang.NgaySinh;

                // Không cho phép thay đổi MaTaiKhoan
                // existingCustomer.MaTaiKhoan = khachHang.MaTaiKhoan;

                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Xóa khách hàng theo ID tài khoản
        /// </summary>
        /// <param name="maTaiKhoan">ID tài khoản liên kết</param>
        /// <returns>True nếu xóa thành công, False nếu thất bại</returns>
        public async Task<bool> DeleteByAccountIdAsync(Guid maTaiKhoan)
        {
            try
            {
                var khachHang = await _context.KhachHangs
                    .FirstOrDefaultAsync(k => k.MaTaiKhoan == maTaiKhoan);

                if (khachHang == null)
                    return false;

                // Kiểm tra có dữ liệu liên quan không (hóa đơn, thuê phòng, lịch sử)
                var hasRelatedData = await _context.HoaDonDichVus
                    .AnyAsync(h => h.MaKhachHang == khachHang.MaKhachHang) ||
                    await _context.ThuePhongs
                    .AnyAsync(t => t.MaKhachHang == khachHang.MaKhachHang) ||
                    await _context.LichSuSuDungPhongs
                    .AnyAsync(l => l.MaKhachHang == khachHang.MaKhachHang);

                if (hasRelatedData)
                {
                    // Không cho phép xóa nếu có dữ liệu liên quan
                    // Có thể implement soft delete hoặc throw exception
                    throw new InvalidOperationException("Không thể xóa khách hàng vì có dữ liệu liên quan (hóa đơn, thuê phòng, lịch sử).");
                }

                _context.KhachHangs.Remove(khachHang);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #region Helper Methods

        /// <summary>
        /// Lấy khách hàng theo ID tài khoản (Helper method)
        /// </summary>
        /// <param name="maTaiKhoan">ID tài khoản</param>
        /// <returns>Khách hàng hoặc null</returns>
        public async Task<KhachHang?> GetByAccountIdAsync(Guid maTaiKhoan)
        {
            try
            {
                return await _context.KhachHangs
                    .Include(k => k.MaTaiKhoanNavigation)
                    .FirstOrDefaultAsync(k => k.MaTaiKhoan == maTaiKhoan);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Kiểm tra khách hàng có tồn tại không
        /// </summary>
        /// <param name="maKhachHang">ID khách hàng</param>
        /// <returns>True nếu tồn tại</returns>
        public async Task<bool> ExistsAsync(Guid maKhachHang)
        {
            try
            {
                return await _context.KhachHangs
                    .AnyAsync(k => k.MaKhachHang == maKhachHang);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Đếm tổng số khách hàng
        /// </summary>
        /// <returns>Số lượng khách hàng</returns>
        public async Task<int> GetTotalCountAsync()
        {
            try
            {
                return await _context.KhachHangs.CountAsync();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<KhachHang> CreateKhacHangAsync(KhachHang khachHang)
        {
            try
            {
                if (khachHang == null)
                    throw new ArgumentNullException(nameof(khachHang));
                khachHang.MaKhachHang = Guid.NewGuid();
                await _context.KhachHangs.AddAsync(khachHang);
                await _context.SaveChangesAsync();
                return khachHang;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Không thể tạo khách hàng.", ex);
            }

        }

        public async Task<List<KhachHang>> GetAllWithTaiKhoanAsync()
        {
            var khachHangs = await _context.KhachHangs
                .Include(k => k.MaTaiKhoanNavigation)
                .ToListAsync();
            return khachHangs;
        }
        #endregion
    }
}