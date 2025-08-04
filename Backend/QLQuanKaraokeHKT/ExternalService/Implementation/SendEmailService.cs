using QLQuanKaraokeHKT.ExternalService.Interfaces;
using MimeKit;
using MailKit.Net.Smtp;

namespace QLQuanKaraokeHKT.ExternalService.Implementation
{
    public class SendEmailService : ISendEmailService
    {
        private readonly IConfiguration _configuration;

        public SendEmailService(IConfiguration configuration) 
        {
            _configuration = configuration;
        }
        public async Task SendOtpEmailAsync(string toEmail, string otpCode)
        {
            var message = new MimeKit.MimeMessage();
            message.From.Add(new MimeKit.MailboxAddress("Karaoke HKT", _configuration["EmailSettings:From"]));
            Console.Write($"{toEmail}");
            message.To.Add(new MimeKit.MailboxAddress("User", toEmail));
            message.Subject = "Xác thực OTP - Quán Karaoke HKT";
            message.Body = new TextPart("plain")
            {
                Text = $"Mã OTP của bạn là: {otpCode}. Vui lòng không chia sẻ mã này với bất kỳ ai."
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
    }
}
