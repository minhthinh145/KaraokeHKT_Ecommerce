using QLQuanKaraokeHKT.Domain.Entities;
using System.Runtime.CompilerServices;

namespace QLQuanKaraokeHKT.Application.Helpers
{
    public static class EmailContentHelper
    {
        #region Employee Email Templates

        public static string CreateWelcomeEmployeeEmail(string hoTen, string email, string password)
        {
            return $@"
<div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
    <h2 style='color: #2c5aa0;'>Chào mừng {hoTen}!</h2>
    
    <p>Tài khoản nhân viên của bạn đã được tạo thành công tại <strong>Karaoke HKT</strong>.</p>
    
    <div style='background-color: #f8f9fa; padding: 15px; border-radius: 5px; margin: 20px 0;'>
        <h3 style='margin-top: 0;'>Thông tin đăng nhập:</h3>
        <p><strong>Email:</strong> {email}</p>
        <p><strong>Mật khẩu:</strong> {password}</p>
    </div>
    
    <p style='color: #e74c3c;'><strong>Lưu ý:</strong> Vui lòng đăng nhập và đổi mật khẩu ngay lần đầu tiên để bảo mật tài khoản.</p>
        
    <hr style='margin: 30px 0;'>
    <p style='color: #666; font-size: 12px;'>
        Trân trọng,<br>
        <strong>Hệ thống Quản lý Karaoke HKT</strong>
    </p>
</div>";
        }

        public static string CreateShiftChangeApprovalEmail(string hoTen, bool approved, string fromShift, string toShift, DateOnly fromDate, DateOnly toDate, string? note = null)
        {
            var statusColor = approved ? "#28a745" : "#dc3545";
            var statusText = approved ? "CHẤP NHẬN" : "TỪ CHỐI";

            return $@"
<div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
    <h2 style='color: #2c5aa0;'>Thông báo yêu cầu chuyển ca</h2>
    
    <p>Chào {hoTen},</p>
    
    <div style='background-color: {statusColor}; color: white; padding: 15px; text-align: center; border-radius: 5px; margin: 20px 0;'>
        <h3 style='margin: 0;'>YÊU CẦU CHUYỂN CA ĐÃ ĐƯỢC {statusText}</h3>
    </div>
    
    <div style='background-color: #f8f9fa; padding: 15px; border-radius: 5px; margin: 20px 0;'>
        <h3 style='margin-top: 0;'>Chi tiết yêu cầu:</h3>
        <p><strong>Từ:</strong> {fromShift} - {fromDate:dd/MM/yyyy}</p>
        <p><strong>Đến:</strong> {toShift} - {toDate:dd/MM/yyyy}</p>
        {(!string.IsNullOrWhiteSpace(note) ? $"<p><strong>Ghi chú:</strong> {note}</p>" : "")}
    </div>
    
    <p>Nếu có thắc mắc, vui lòng liên hệ bộ phận quản lý nhân sự.</p>
    
    <hr style='margin: 30px 0;'>
    <p style='color: #666; font-size: 12px;'>
        Trân trọng,<br>
        <strong>Bộ phận Quản lý Nhân sự - Karaoke HKT</strong>
    </p>
</div>";
        }

        public static string CreateEmployeeTerminationEmail(string hoTen, DateTime terminationDate)
        {
            return $@"
<div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
    <h2 style='color: #dc3545;'>Thông báo nghỉ việc</h2>
    
    <p>Chào {hoTen},</p>
     
    <p>Cảm ơn bạn đã đóng góp cho sự phát triển của Karaoke HKT. Chúc bạn thành công trong công việc mới!</p>
    
    <hr style='margin: 30px 0;'>
    <p style='color: #666; font-size: 12px;'>
        Trân trọng,<br>
        <strong>Bộ phận Quản lý Nhân sự - Karaoke HKT</strong>
    </p>
</div>";
        }

        #endregion

        #region OTP & Security Email Templates


        public static string CreateOtpEmail(string hoTen, string otpCode)
        {
            return $@"Chào {hoTen},

Mã OTP của bạn là: {otpCode}

Mã này có hiệu lực trong 5 phút.
Vui lòng không chia sẻ mã này với bất kỳ ai.

Nếu bạn không yêu cầu mã này, vui lòng bỏ qua email này.

Trân trọng,
Hệ thống Quản lý Karaoke HKT";
        }

        public static string CreatePasswordResetEmail(string hoTen, string newPassword)
        {
            return $@"Chào {hoTen},

Mật khẩu của bạn đã được đặt lại thành công!

Mật khẩu mới: {newPassword}

Vui lòng đăng nhập và đổi mật khẩu ngay lập tức để bảo mật tài khoản.

Nếu bạn không yêu cầu đặt lại mật khẩu, vui lòng liên hệ hỗ trợ ngay lập tức.

Trân trọng,
Hệ thống Quản lý Karaoke HKT";
        }

        #endregion

        #region System Notification Templates


        public static string CreateAccountLockedEmail(string hoTen, string lyDo)
        {
            return $@"Chào {hoTen},

Tài khoản của bạn đã bị tạm khóa.

Lý do: {lyDo}

Vui lòng liên hệ bộ phận hỗ trợ để được mở khóa tài khoản.

Hotline: 1900 1234

Trân trọng,
Hệ thống Quản lý Karaoke HKT";
        }


        public static string CreateAccountUnlockedEmail(string hoTen)
        {
            return $@"Chào {hoTen},

Tài khoản của bạn đã được mở khóa thành công!

Bạn có thể đăng nhập và sử dụng dịch vụ bình thường.

Chúc bạn có trải nghiệm tốt!

Trân trọng,
Hệ thống Quản lý Karaoke HKT";
        }

        #endregion

        #region Customer Email Templates
        public static string CreatePaymentReminderEmail(string hoTen, string maHoaDon, decimal tongTien, DateTime hanThanhToan)
        {
            return $@"
<div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
    <h2 style='color: #f39c12;'>Nhắc nhở thanh toán</h2>
    
    <p>Chào {hoTen},</p>
    
    <div style='background-color: #fff3cd; border: 1px solid #ffeaa7; padding: 15px; border-radius: 5px; margin: 20px 0;'>
        <h3 style='margin-top: 0;'>Thông tin hóa đơn cần thanh toán:</h3>
        <p><strong>Mã hóa đơn:</strong> {maHoaDon}</p>
        <p><strong>Tổng tiền:</strong> {tongTien:N0} VNĐ</p>
        <p><strong>Hạn thanh toán:</strong> {hanThanhToan:dd/MM/yyyy HH:mm}</p>
    </div>
    
    <p>Vui lòng thanh toán trước thời hạn để tránh bị hủy đặt phòng.</p>
    
    <hr style='margin: 30px 0;'>
    <p style='color: #666; font-size: 12px;'>
        Trân trọng,<br>
        <strong>Quán Karaoke HKT</strong>
    </p>
</div>";
        }

        #endregion

        #region Work Schedule Email Templates
        public static string CreateWorkScheduleNotificationEmail(string hoTen, DateOnly start, DateOnly end, List<LichLamViec> lichLamViecs)
        {
            var scheduleText = string.Join("\n", lichLamViecs.Select(lv =>
                $"{lv.NgayLamViec:dd/MM/yyyy} - Ca {lv.CaLamViec?.TenCa} ({lv.CaLamViec?.GioBatDauCa:hh\\:mm} - {lv.CaLamViec?.GioKetThucCa:hh\\:mm})"
            ));

            return $@"Chào {hoTen},

Lịch làm việc của bạn từ {start:dd/MM/yyyy} đến {end:dd/MM/yyyy}:

{scheduleText}

 Lưu ý quan trọng:
• Vui lòng có mặt đúng giờ theo lịch
• Thông báo trước nếu có vấn đề phát sinh
• Liên hệ quản lý nếu cần hỗ trợ

Đăng nhập hệ thống để xem chi tiết và cập nhật thông tin.

Trân trọng,
Phòng Nhân sự - Karaoke HKT";
        }

        #endregion       

        #region Email Subjects

        public static class Subjects
        {
            public const string WelcomeEmployee = "Chào mừng nhân viên mới - Karaoke HKT";
            public const string EmployeeAccountUpdate = "Cập nhật thông tin tài khoản - Karaoke HKT";
            public const string WelcomeCustomer = "Chào mừng đến với Karaoke HKT";
            public const string BookingConfirmation = "Xác nhận đặt phòng - Karaoke HKT";
            public const string WelcomeAdmin = "Tài khoản quản trị viên - Karaoke HKT";
            public const string OtpCode = "Mã OTP xác thực - Karaoke HKT";
            public const string PasswordReset = "Đặt lại mật khẩu - Karaoke HKT";
            public const string AccountLocked = "Thông báo khóa tài khoản - Karaoke HKT";
            public const string AccountUnlocked = "Thông báo mở khóa tài khoản - Karaoke HKT";
            public const string WorkScheduleNotification = "Thông báo lịch làm việc - Karaoke HKT";
        }

        #endregion
    }
}
