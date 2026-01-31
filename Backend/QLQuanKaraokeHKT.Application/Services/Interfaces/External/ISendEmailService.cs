namespace QLQuanKaraokeHKT.Application.Services.Interfaces.External
{
    public interface ISendEmailService
    {
        Task SendEmailByContentAsync(string toEmail, string subject, string content);
        Task SendOtpEmailAsync(string toEmail, string otpCode);

        #region HRM
        Task SendWelcomeEmployeeEmailAsync(string toEmail, string hoTen, string password);
        Task SendEmployeeAccountUpdateEmailAsync(string toEmail, string hoTen, string? newPassword = null);
        Task SendShiftChangeApprovalEmailAsync(string toEmail, string hoTen, bool approved, string fromShift, string toShift, DateOnly fromDate, DateOnly toDate, string? note = null);
        Task SendEmployeeTerminationEmailAsync(string toEmail, string hoTen, DateTime terminationDate);
        #endregion

        #region Customer
        #endregion

        #region Auth/System
        Task SendPasswordResetEmailAsync(string toEmail, string hoTen, string resetToken);
        Task SendAccountLockedEmailAsync(string toEmail, string hoTen, string reason);
        #endregion
    }
}