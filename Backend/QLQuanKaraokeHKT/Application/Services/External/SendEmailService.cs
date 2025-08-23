using MimeKit;
using MailKit.Net.Smtp;
using QLQuanKaraokeHKT.Core.Interfaces.Services.External;

namespace QLQuanKaraokeHKT.Application.Services.External
{
    public class SendEmailService : ISendEmailService
    {
        private readonly IConfiguration _configuration;

        public SendEmailService(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        public async Task SendEmailByContentAsync(string toEmail, string subject, string content)
        {
           var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Karaoke HKT", _configuration["EmailSettings:From"]));
            message.To.Add(new MailboxAddress("User", toEmail));
            message.Subject = subject;
            message.Body = new TextPart("plain")
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

        public async Task SendOtpEmailAsync(string toEmail, string otpCode)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Karaoke HKT", _configuration["EmailSettings:From"]));
            Console.Write($"{toEmail}");
            message.To.Add(new MailboxAddress("User", toEmail));
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
