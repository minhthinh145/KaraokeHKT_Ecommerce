using MimeKit;
using MailKit.Net.Smtp;
using QLQuanKaraokeHKT.Application.Helpers;
using QLQuanKaraokeHKT.Application.Services.Interfaces.External;

namespace QLQuanKaraokeHKT.Application.Services.Implementations.External
{
    public class SendEmailService : ISendEmailService
    {
        private readonly IConfiguration _configuration;

        public SendEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region HRM Specific Methods

        public async Task SendWelcomeEmployeeEmailAsync(string toEmail, string hoTen, string password)
        {
            var subject = "Tạo tài khoản nhân viên - Karaoke HKT";
            var content = EmailContentHelper.CreateWelcomeEmployeeEmail(hoTen, toEmail, password);
            await SendEmailInternalAsync(toEmail, subject, content);
        }

        public async Task SendEmployeeAccountUpdateEmailAsync(string toEmail, string hoTen, string? newPassword = null)
        {
            var subject = "Cập nhật tài khoản nhân viên - Karaoke HKT";
            var content = EmailContentHelper.CreateWelcomeEmployeeEmail(hoTen, toEmail, newPassword!);
            await SendEmailInternalAsync(toEmail, subject, content);
        }

        public async Task SendShiftChangeApprovalEmailAsync(string toEmail, string hoTen, bool approved, string fromShift, string toShift, DateOnly fromDate, DateOnly toDate, string? note = null)
        {
            var subject = $"Thông báo {(approved ? "chấp nhận" : "từ chối")} yêu cầu chuyển ca - Karaoke HKT";
            var content = EmailContentHelper.CreateShiftChangeApprovalEmail(hoTen, approved, fromShift, toShift, fromDate, toDate, note);
            await SendEmailInternalAsync(toEmail, subject, content);
        }

        public async Task SendEmployeeTerminationEmailAsync(string toEmail, string hoTen, DateTime terminationDate)
        {
            var subject = "Thông báo nghỉ việc - Karaoke HKT";
            var content = EmailContentHelper.CreateEmployeeTerminationEmail(hoTen, terminationDate);
            await SendEmailInternalAsync(toEmail, subject, content);
        }

        #endregion

        #region Customer Specific Methods

     

        #endregion

        #region System Specific Methods

        public async Task SendPasswordResetEmailAsync(string toEmail, string hoTen, string resetToken)
        {
            var subject = "Đặt lại mật khẩu - Karaoke HKT";
            var content = EmailContentHelper.CreatePasswordResetEmail(hoTen, resetToken);
            await SendEmailInternalAsync(toEmail, subject, content);
        }

        public async Task SendAccountLockedEmailAsync(string toEmail, string hoTen, string reason)
        {
            var subject = "Thông báo khóa tài khoản - Karaoke HKT";
            var content = EmailContentHelper.CreateAccountLockedEmail(hoTen, reason);
            await SendEmailInternalAsync(toEmail, subject, content);
        }
        #endregion

        #region Legacy Methods (Keep for backward compatibility)

        public async Task SendEmailByContentAsync(string toEmail, string subject, string content)
        {
            await SendEmailInternalAsync(toEmail, subject, content);
        }

        public async Task SendOtpEmailAsync(string toEmail, string otpCode)
        {
            var subject = "Xác thực OTP - Quán Karaoke HKT";
            var content = $"Mã OTP của bạn là: {otpCode}. Vui lòng không chia sẻ mã này với bất kỳ ai.";
            await SendEmailInternalAsync(toEmail, subject, content);
        }

        #endregion

        #region Private Email Sending Core

        private async Task SendEmailInternalAsync(string toEmail, string subject, string content)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Karaoke HKT", _configuration["EmailSettings:From"]));
                message.To.Add(new MailboxAddress("User", toEmail));
                message.Subject = subject;
                message.Body = new TextPart("html") 
                {
                    Text = content
                };

                using var client = new SmtpClient();
                await client.ConnectAsync(
                    _configuration["EmailSettings:SmtpServer"],
                    int.Parse(_configuration["EmailSettings:Port"]),
                    true
                );
                await client.AuthenticateAsync(
                    _configuration["EmailSettings:Username"],
                    _configuration["EmailSettings:Password"]
                );
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                // Log error if needed
                throw new InvalidOperationException($"Failed to send email to {toEmail}: {ex.Message}", ex);
            }
        }

        #endregion
    }
}