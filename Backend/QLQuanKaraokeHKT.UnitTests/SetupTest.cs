using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using QLQuanKaraokeHKT.Application.Services.Payment;
using QLQuanKaraokeHKT.Core.Interfaces;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Common;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Payment;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Room;
using QLQuanKaraokeHKT.Infrastructure.Data;

namespace QLQuanKaraokeHKT.UnitTests
{
    [ExcludeFromCodeCoverage]
    public class SetupTest : IDisposable
    {
        protected readonly IMapper _mapperConfig;
        protected readonly Fixture _fixture;
        protected readonly Mock<IUnitOfWork> _unitOfWorkMock;
        protected readonly QlkaraokeHktContext _dbContext;
        protected readonly IServiceProvider _serviceProvider;
        
        // Payment related mocks
        protected readonly Mock<IPaymentFactory> _paymentFactoryMock;
        protected readonly Mock<IPaymentService> _paymentServiceMock;
        protected readonly Mock<IOptions<VNPayConfig>> _vnpayConfigMock;
        protected readonly VNPayConfig _vnpayConfig; // Config từ appsettings
        
        // Room related mocks
        protected readonly Mock<IRoomOrchestrator> _roomOrchestratorMock;
        protected readonly Mock<IPricingService> _pricingServiceMock;
        protected readonly Mock<IRoomQueryService> _roomQueryServiceMock;
        
        // Common mocks
        protected readonly Mock<ILogger<VNPayPaymentService>> _loggerMock;

        public SetupTest()
        {
            // ✅ TẠO SERVICE COLLECTION VÀ ĐỌC CONFIG TỪ APPSETTINGS
            var services = new ServiceCollection();

            // Build configuration từ appsettings.json
            var configuration = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: false) // Đọc file ngay tại output
             .AddEnvironmentVariables()
             .Build();

            // Configure VNPay từ appsettings - KHÔNG hardcode!
            services.Configure<VNPayConfig>(configuration.GetSection("VNPay"));
            
            // Build service provider
            _serviceProvider = services.BuildServiceProvider();
            
            // Lấy VNPayConfig từ DI container
            var vnpayOptions = _serviceProvider.GetRequiredService<IOptions<VNPayConfig>>();
            _vnpayConfig = vnpayOptions.Value;
            
            // AutoMapper configuration
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new QLQuanKaraokeHKT.Application.Mappings.Room.RoomMappingProfile());
                mc.AddProfile(new QLQuanKaraokeHKT.Application.Mappings.Room.RoomQueryMappingProfile());
            });
            _mapperConfig = mappingConfig.CreateMapper();
            
            // AutoFixture for test data generation
            _fixture = new Fixture();
            
            // Mock setup
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _paymentFactoryMock = new Mock<IPaymentFactory>();
            _paymentServiceMock = new Mock<IPaymentService>();
            _roomOrchestratorMock = new Mock<IRoomOrchestrator>();
            _pricingServiceMock = new Mock<IPricingService>();
            _roomQueryServiceMock = new Mock<IRoomQueryService>();
            _loggerMock = new Mock<ILogger<VNPayPaymentService>>();
            
            // Setup VNPay config mock với config THẬT
            _vnpayConfigMock = new Mock<IOptions<VNPayConfig>>();
            _vnpayConfigMock.Setup(x => x.Value).Returns(_vnpayConfig);

            // Validate config - đảm bảo không bị null
            if (string.IsNullOrEmpty(_vnpayConfig.HashSecret))
                throw new InvalidOperationException("VNPay HashSecret không được cấu hình trong appsettings.json!");

            // In-Memory Database for integration tests
            var options = new DbContextOptionsBuilder<QlkaraokeHktContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _dbContext = new QlkaraokeHktContext(options);
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}