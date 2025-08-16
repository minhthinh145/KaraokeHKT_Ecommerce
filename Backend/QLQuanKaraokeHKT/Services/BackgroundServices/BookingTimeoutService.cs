using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Models;
using QLQuanKaraokeHKT.Repositories.QLPhong.Interfaces;

namespace QLQuanKaraokeHKT.Services.BackgroundServices
{
    public class AutoBookingService : IHostedService, IDisposable
    {
        private readonly ILogger<AutoBookingService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private Timer _timeoutTimer;
        private Timer _checkoutTimer;

        public AutoBookingService(ILogger<AutoBookingService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Auto Booking Service đã khởi động.");

            // Timer 1: Xử lý timeout booking (mỗi 5 phút) - Hủy booking chưa thanh toán sau 15 phút
            _timeoutTimer = new Timer(CheckExpiredBookings, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));

            // Timer 2: Tự động check-out phòng hết hạn (mỗi 2 phút)
            _checkoutTimer = new Timer(AutoCheckoutExpiredRooms, null, TimeSpan.Zero, TimeSpan.FromMinutes(2));

            return Task.CompletedTask;
        }

        #region Xử lý booking timeout (15 phút chưa thanh toán)
        private async void CheckExpiredBookings(object state)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<QlkaraokeHktContext>();
            var phongHatRepository = scope.ServiceProvider.GetRequiredService<IPhongHatRepository>();

            try
            {
                var timeLimit = DateTime.Now.AddMinutes(-15);

                // Tìm các booking chưa thanh toán và đã quá 15 phút
                var expiredBookings = await context.ThuePhongs
                    .Include(t => t.MaKhachHangNavigation)
                    .Include(t => t.MaPhongNavigation)
                    .Where(t => t.TrangThai == "Pending" && t.ThoiGianBatDau < timeLimit)
                    .ToListAsync();

                foreach (var booking in expiredBookings)
                {
                    // Cập nhật trạng thái thuê phòng
                    booking.TrangThai = "DaHuy";

                    // Trả lại phòng
                    await phongHatRepository.UpdateDangSuDungAsync(booking.MaPhong, false);

                    // Hủy hóa đơn tương ứng
                    var minTime = booking.ThoiGianBatDau.AddMinutes(-10);
                    var maxTime = booking.ThoiGianBatDau.AddMinutes(10);

                    var hoaDon = await context.HoaDonDichVus
                        .FirstOrDefaultAsync(h =>
                            h.MaKhachHang == booking.MaKhachHang &&
                            h.TrangThai == "ChuaThanhToan" &&
                            h.NgayTao >= minTime &&
                            h.NgayTao <= maxTime);
                    if (hoaDon != null)
                    {
                        hoaDon.TrangThai = "DaHuy";
                    }

                    _logger.LogInformation($"Đã hủy booking timeout: ThuePhong {booking.MaThuePhong} - Phòng {booking.MaPhong}");
                }

                await context.SaveChangesAsync();

                if (expiredBookings.Any())
                {
                    _logger.LogInformation($"Đã xử lý {expiredBookings.Count} booking bị timeout.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xử lý booking timeout");
            }
        }
        #endregion

        #region Tự động check-out phòng hết hạn
        private async void AutoCheckoutExpiredRooms(object state)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<QlkaraokeHktContext>();
            var phongHatRepository = scope.ServiceProvider.GetRequiredService<IPhongHatRepository>();

            try
            {
                var currentTime = DateTime.Now;

                // ✅ TÌM CÁC PHÒNG ĐÃ HẾT THỜI GIAN THUÊ
                var expiredRooms = await context.ThuePhongs
                    .Include(t => t.MaPhongNavigation)
                    .Include(t => t.MaKhachHangNavigation)
                    .Where(t => t.TrangThai == "DangSuDung" &&
                               t.ThoiGianKetThuc.HasValue &&
                               t.ThoiGianKetThuc.Value <= currentTime)
                    .ToListAsync();

                foreach (var thuePhong in expiredRooms)
                {
                    await AutoCheckoutRoom(context, phongHatRepository, thuePhong);
                }

                if (expiredRooms.Any())
                {
                    await context.SaveChangesAsync();
                    _logger.LogInformation($"Đã tự động check-out {expiredRooms.Count} phòng hết hạn.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tự động check-out phòng");
            }
        }

        private async Task AutoCheckoutRoom(QlkaraokeHktContext context, IPhongHatRepository phongHatRepository, ThuePhong thuePhong)
        {
            try
            {
                var currentTime = DateTime.Now;

                // 1. ✅ CẬP NHẬT TRẠNG THÁI THUÊ PHÒNG
                thuePhong.TrangThai = "DaKetThuc";
                thuePhong.ThoiGianKetThuc = currentTime;

                // 2. ✅ TRẢ PHÒNG (DangSuDung = false)
                await phongHatRepository.UpdateDangSuDungAsync(thuePhong.MaPhong, false);

                // 3. ✅ CẬP NHẬT LỊCH SỬ SỬ DỤNG PHÒNG - BỔ SUNG MaThuePhong
                var lichSuSuDung = await context.LichSuSuDungPhongs
                    .FirstOrDefaultAsync(l => l.MaThuePhong == thuePhong.MaThuePhong); // ✅ TÌM THEO MaThuePhong

                if (lichSuSuDung != null)
                {
                    lichSuSuDung.ThoiGianKetThuc = currentTime;
                }

                _logger.LogInformation($"✅ Tự động check-out phòng {thuePhong.MaPhong} cho khách hàng {thuePhong.MaKhachHangNavigation?.TenKhachHang}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi tự động check-out phòng {thuePhong.MaPhong}");
            }
        }
        #endregion

        #region Cleanup và monitoring methods (Optional - để mở rộng sau)

        /// <summary>
        /// Dọn dẹp các bản ghi cũ (có thể chạy hàng ngày)
        /// </summary>
        private async Task CleanupOldRecords(QlkaraokeHktContext context)
        {
            try
            {
                var cutoffDate = DateTime.Now.AddDays(-30); // Xóa dữ liệu cũ hơn 30 ngày

                // Xóa các booking đã hủy quá 30 ngày
                var oldCancelledBookings = await context.ThuePhongs
                    .Where(t => t.TrangThai == "DaHuy" && t.ThoiGianBatDau < cutoffDate)
                    .ToListAsync();

                if (oldCancelledBookings.Any())
                {
                    context.ThuePhongs.RemoveRange(oldCancelledBookings);
                    _logger.LogInformation($"Đã dọn dẹp {oldCancelledBookings.Count} booking cũ.");
                }

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi dọn dẹp dữ liệu cũ");
            }
        }

        /// <summary>
        /// Thống kê tình trạng phòng (có thể dùng cho monitoring)
        /// </summary>
        private async Task LogRoomStatistics(QlkaraokeHktContext context)
        {
            try
            {
                var totalRooms = await context.PhongHatKaraokes.CountAsync();
                var busyRooms = await context.PhongHatKaraokes.CountAsync(p => p.DangSuDung);
                var availableRooms = totalRooms - busyRooms;

                _logger.LogInformation($"📊 Thống kê phòng: Tổng {totalRooms}, Đang sử dụng {busyRooms}, Trống {availableRooms}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thống kê phòng");
            }
        }

        #endregion

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Auto Booking Service đã dừng.");
            _timeoutTimer?.Change(Timeout.Infinite, 0);
            _checkoutTimer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timeoutTimer?.Dispose();
            _checkoutTimer?.Dispose();
        }
    }
}