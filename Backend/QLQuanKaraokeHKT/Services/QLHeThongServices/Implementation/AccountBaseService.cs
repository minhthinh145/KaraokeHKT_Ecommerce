using QLQuanKaraokeHKT.ExternalService.Interfaces;
using QLQuanKaraokeHKT.Helpers;
using QLQuanKaraokeHKT.Repositories.TaiKhoanRepo;
using QLQuanKaraokeHKT.Services.QLHeThongServices.Interface;

namespace QLQuanKaraokeHKT.Services.QLHeThongServices.Implementation
{
    public class AccountBaseService : IAccountBaseService
    {
        public readonly ITaiKhoanRepository _taiKhoanRepository;
        public readonly ISendEmailService _emailService;
        public AccountBaseService(ITaiKhoanRepository taiKhoanRepository, ISendEmailService emailService)
        {
            _taiKhoanRepository = taiKhoanRepository;
            _emailService = emailService;
        }
        public async Task<ServiceResult> CheckEmailExistsAsync(string email)
        {
            var existingUser = await _taiKhoanRepository.FindByEmailAsync(email);
            if (existingUser != null)
            {
                return ServiceResult.Failure("Email đã được sử dụng bởi tài khoản khác.");
            }
            return ServiceResult.Success();
        }


        public async Task<ServiceResult> SendWelcomeEmailAsync(string email, string hoTen, string password)
        {
            try
            {
                var emailContent = $"Chào nhân viên {hoTen},\n\n" +
                                   "Tài khoản của bạn đã được tạo thành công. Vui lòng đăng nhập với thông tin sau:\n" +
                                   $"Email: {email}\n" +
                                   $"Mật khẩu: {password}";

                await _emailService.SendEmailByContentAsync(email, "Tạo tài khoản nhân viên", emailContent);
                return ServiceResult.Success("Đã gửi email chào mừng thành công.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi khi gửi email chào mừng: {ex.Message}");

            }
        }

     

    }
}
