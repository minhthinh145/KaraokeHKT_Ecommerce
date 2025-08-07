namespace QLQuanKaraokeHKT.ExternalService.Interfaces
{
    public interface ISendEmailService
    {
        /// <summary>
        /// Send OTP to User
        /// </summary>
        /// <param name="toEmail">Email người nhận</param>
        /// <param name="otpCode">Mã OTP cần gửi</param>
        Task SendOtpEmailAsync(string toEmail, string otpCode);

        Task SendEmailByContentAsync(string toEmail, string subject, string content);

    }
}
