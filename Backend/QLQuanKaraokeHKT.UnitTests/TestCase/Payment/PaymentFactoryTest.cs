using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using QLQuanKaraokeHKT.Application.Services.Payment;
using QLQuanKaraokeHKT.Core.Enums;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Payment;
using QLQuanKaraokeHKT.Shared.Constants;
using QLQuanKaraokeHKT.UnitTests.Extension;
using Xunit;

namespace QLQuanKaraokeHKT.UnitTests.TestCase.Payment
{
    public class PaymentFactoryTest : SetupTest
    {
        private readonly Mock<IServiceProvider> _mockServiceProvider;
        private readonly PaymentFactory _paymentFactory;

        public PaymentFactoryTest()
        {
            _mockServiceProvider = new Mock<IServiceProvider>();
            var mockLogger = MockService.GetMockLogger<PaymentFactory>();
            _paymentFactory = new PaymentFactory(_mockServiceProvider.Object, mockLogger);
        }

        [Fact]
        public void GetPaymentService_WithVNPay_ShouldReturnVNPayService()
        {
            // Arrange
            var mockVNPayService = MockService.CreateMockPaymentService(PaymentMethod.VNPay);
            
            _mockServiceProvider
                .Setup(x => x.GetService(typeof(VNPayPaymentService)))
                .Returns(mockVNPayService.Object);

            // Act
            var result = _paymentFactory.GetPaymentService("VNPay");

            // Assert
            result.Should().NotBeNull();
            result.PaymentMethod.Should().Be(PaymentMethod.VNPay);
        }

        [Fact]
        public void GetPaymentService_WithUnsupportedMethod_ShouldThrowNotSupportedException()
        {
            // Act & Assert
            var act = () => _paymentFactory.GetPaymentService("Momo");
            act.Should().Throw<NotSupportedException>()
                .WithMessage(PaymentErrorMessage.PaymentMethodNotSupported);
        }

        [Fact]
        public void GetSupportedPaymentMethods_ShouldReturnAvailableMethods()
        {
            // Act
            var result = _paymentFactory.GetSupportedPaymentMethods();

            // Assert
            result.Should().NotBeEmpty();
            result.Should().Contain("VNPay");
        }

        [Theory]
        [InlineData("VNPay")]
        [InlineData("vnpay")]
        [InlineData("VNPAY")]
        public void GetPaymentService_WithStringMethod_ShouldBeCaseInsensitive(string paymentMethodName)
        {
            // Arrange
            var mockVNPayService = MockService.CreateMockPaymentService(PaymentMethod.VNPay);
            
            _mockServiceProvider
                .Setup(x => x.GetService(typeof(VNPayPaymentService)))
                .Returns(mockVNPayService.Object);

            // Act
            var result = _paymentFactory.GetPaymentService(paymentMethodName);

            // Assert
            result.Should().NotBeNull();
            result.PaymentMethod.Should().Be(PaymentMethod.VNPay);
        }

        [Fact]
        public void GetPaymentService_WithEmptyString_ShouldThrowArgumentException()
        {
            // Act & Assert
            var act = () => _paymentFactory.GetPaymentService("");
            act.Should().Throw<NotSupportedException>()
                .WithMessage(PaymentErrorMessage.PaymentMethodEmpty);
        }
    }
}