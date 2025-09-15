using QLQuanKaraokeHKT.Application.Mappings.Account;
using QLQuanKaraokeHKT.Application.Mappings.Auth;
using QLQuanKaraokeHKT.Application.Mappings.Booking;
using QLQuanKaraokeHKT.Application.Mappings.Customer;
using QLQuanKaraokeHKT.Application.Mappings.HRM;
using QLQuanKaraokeHKT.Application.Mappings.Inventory;
using QLQuanKaraokeHKT.Application.Mappings.Room;
using QLQuanKaraokeHKT.Application.Services.AccountManagement;
using QLQuanKaraokeHKT.Application.Services.Auth;
using QLQuanKaraokeHKT.Application.Services.Common;
using QLQuanKaraokeHKT.Application.Services.Core;
using QLQuanKaraokeHKT.Application.Services.Customer;
using QLQuanKaraokeHKT.Application.Services.External;
using QLQuanKaraokeHKT.Application.Services.HRM;
using QLQuanKaraokeHKT.Application.Services.Inventory;
using QLQuanKaraokeHKT.Application.Services.Payment;
using QLQuanKaraokeHKT.Application.Services.Room;
using QLQuanKaraokeHKT.Core.Interfaces;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Auth;
using QLQuanKaraokeHKT.Core.Interfaces.Services.AccountManagement;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Auth;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Common;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Core;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Customer;
using QLQuanKaraokeHKT.Core.Interfaces.Services.External;
using QLQuanKaraokeHKT.Core.Interfaces.Services.HRM;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Inventory;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Payment;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Room;
using QLQuanKaraokeHKT.Infrastructure;
using QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.Auth;

namespace QLQuanKaraokeHKT.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(
                cfg =>
                {
                    cfg.AddProfile<AuthMappingProfile>();

                    cfg.AddProfile<ActorMappingProfile>();

                    cfg.AddProfile<RoomMappingProfile>();
                    cfg.AddProfile<RoomQueryMappingProfile>();

                    cfg.AddProfile<InventoryMappingProfile>();
                    cfg.AddProfile<InventoryQueryMappingProfile>();

                    cfg.AddProfile<HRMMappingProfile>();

                    cfg.AddProfile<BookingMappingProfile>();
                    cfg.AddProfile<AccountMappingProfile>();
                    
                });
            //add repo missing (temp)
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();


            // VNPay Configuration
            services.Configure<VNPayConfig>(configuration.GetSection("VNPay"));
            services.AddScoped<IPaymentService, VNPayPaymentService>();
            services.AddScoped<IPaymentFactory, PaymentFactory>();


            // UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IPricingService, PricingService>();
            services.AddScoped<ICoreAccountService, CoreAccountService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPaymentService>(provider => provider.GetService<VNPayPaymentService>()!);
            services.AddScoped<IMaOtpService, MaOtpService>();
            services.AddScoped<ISendEmailService, SendEmailService>();

            // Authentication Services
            services.AddScoped<IVerifyAuthService, VerifyAuthService>();
            services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
            services.AddScoped<IUserRegistrationService, UserRegistrationService>();
            services.AddScoped<IPasswordManagementService, PasswordManagementService>();
            services.AddScoped<IUserProfileService, UserProfileService>();
            services.AddScoped<IAuthOrchestrator, AuthOrchestrator>();

            // Account Management Services
            services.AddScoped<IAdminAccountService, AdminAccountService>();
            services.AddScoped<INhanVienAccountService, NhanVienAccountService>();
            services.AddScoped<IKhachHangAccountService, KhachHangAccountService>();
            services.AddScoped<IAccountManagementService, AccountManagementService>();

            // Business Services
            services.AddScoped<IKhachHangService, KhachHangService>();
            services.AddScoped<IRoomQueryService, RoomQueryService>();

            // HRM Services
            services.AddScoped<IHRMOrchestrator, HRMOrchestrator>();
            services.AddScoped<IQLCaLamViecService, QLCaLamViecService>();
            services.AddScoped<IQuanLyTienLuongService, QuanLyTienLuongService>();
            services.AddScoped<IQLLichLamViecService, QLLichLamViecService>();
            services.AddScoped<IQLYeuCauChuyenCaService, QLYeuCauChuyenCaService>();

            // Inventory Services
            services.AddScoped<IQLVatLieuService, QLVatLieuService>();

            // Room Services
            services.AddScoped<IQLLoaiPhongService, QLLoaiPhongService>();
            services.AddScoped<IRoomOrchestrator, RoomOrchestrator>();
            // Payment Services

            // External Services
      
            //services.AddScoped<IKhachHangDatPhongService, KhachHangDatPhongService>();

            // Background Services
            //   services.AddHostedService<AutoBookingService>();

            return services;
        }
    }
}