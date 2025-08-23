using AutoMapper;
using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Auth;
using QLQuanKaraokeHKT.Core.Interfaces.Services.AccountManagement;
using QLQuanKaraokeHKT.Core.Interfaces.Services.External;

namespace QLQuanKaraokeHKT.Application.Services.AccountManagement
{
    public class AccountBaseService : IAccountBaseService
    {
        public readonly ITaiKhoanRepository _taiKhoanRepository;
        public readonly ISendEmailService _emailService;
        private readonly IMapper _mapper;

        public AccountBaseService(ITaiKhoanRepository taiKhoanRepository, ISendEmailService emailService, IMapper mapper)
        {
            _taiKhoanRepository = taiKhoanRepository;
            _emailService = emailService;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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