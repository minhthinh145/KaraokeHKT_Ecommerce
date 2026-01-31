﻿using AutoFixture;
using FluentAssertions;
using QLQuanKaraokeHKT.Application.Services.Implementations.Payment;
using QLQuanKaraokeHKT.Application.DTOs.PaymentDTOs;
using QLQuanKaraokeHKT.Shared.Enums;
using QLQuanKaraokeHKT.Shared.Constants;
using QLQuanKaraokeHKT.UnitTests.Extension;
using Xunit;

namespace QLQuanKaraokeHKT.UnitTests.TestCase.Payment
{
    public class VNPayPaymentServiceTest : SetupTest
    {
        private readonly VNPayPaymentService _vnPayService;

        public VNPayPaymentServiceTest()
        {
            _vnPayService = new VNPayPaymentService(_vnpayConfigMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void PaymentMethod_ShouldReturnVNPay()
        {
            // Act
            var result = _vnPayService.PaymentMethod;

            // Assert
            result.Should().Be(PaymentMethod.VNPay);
        }

        [Fact]
        public async Task CreatePaymentUrlAsync_WithValidRequest_ShouldReturnSuccessResult()
        {
            // Arrange
            var request = _fixture.Build<PaymentRequestDTO>()
                .With(x => x.OrderId, Guid.NewGuid())
                .With(x => x.Amount, 100000m)
                .With(x => x.OrderDescription, "Test payment")
                .With(x => x.PaymentMethod, PaymentMethod.VNPay)
                .With(x => x.Currency, _vnpayConfig.CurrCode) //
                .With(x => x.Locale, _vnpayConfig.Locale)  
                .With(x => x.ExpiryMinutes, 15)
                .Create();

            var mockHttpContext = MockService.CreateMockHttpContext();

            // Act
            var result = await _vnPayService.CreatePaymentUrlAsync(request, mockHttpContext);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().BeOfType<PaymentUrlResponseDTO>();

            var response = (PaymentUrlResponseDTO)result.Data!;
            response.PaymentUrl.Should().NotBeNullOrEmpty();
            response.PaymentUrl.Should().Contain(_vnpayConfig.BaseUrl); // Verify URL contains real base URL
            response.OrderId.Should().Be(request.OrderId);
            response.PaymentMethod.Should().Be(PaymentMethod.VNPay);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1000)]
        public async Task CreatePaymentUrlAsync_WithInvalidAmount_ShouldReturnFailure(decimal amount)
        {
            // Arrange
            var request = _fixture.Build<PaymentRequestDTO>()
                .With(x => x.Amount, amount)
                .With(x => x.OrderId, Guid.NewGuid())
                .With(x => x.OrderDescription, "Test payment")
                .With(x => x.PaymentMethod, PaymentMethod.VNPay)
                .With(x => x.Currency, "VND")
                .With(x => x.Locale, "vn")
                .With(x => x.ExpiryMinutes, 15)
                .Create();

            var mockHttpContext = MockService.CreateMockHttpContext();

            // Act
            var result = await _vnPayService.CreatePaymentUrlAsync(request, mockHttpContext);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be(PaymentErrorMessage.InvalidAmount);
        }

        [Fact]
        public async Task CreatePaymentUrlAsync_WithEmptyOrderId_ShouldReturnFailure()
        {
            // Arrange
            var request = _fixture.Build<PaymentRequestDTO>()
                .With(x => x.Amount, 100000m)
                .With(x => x.OrderId, Guid.Empty)
                .With(x => x.OrderDescription, "Test payment")
                .With(x => x.PaymentMethod, PaymentMethod.VNPay)
                .With(x => x.Currency, "VND")
                .With(x => x.Locale, "vn")
                .With(x => x.ExpiryMinutes, 15)
                .Create();

            var mockHttpContext = MockService.CreateMockHttpContext();

            // Act
            var result = await _vnPayService.CreatePaymentUrlAsync(request, mockHttpContext);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be(PaymentErrorMessage.InvalidOrderId);
        }

        [Fact]
        public async Task ProcessPaymentResponseAsync_WithSuccessResponse_ShouldReturnSuccess()
        {
            // Arrange - SỬ DỤNG CONFIG THẬT từ DI, KHÔNG hardcode!
            var testOrderId = Guid.NewGuid();
            var queryCollection = MockService.CreateMockVNPayResponseWithRealConfig(
                _vnpayConfig, // Config từ appsettings qua DI
                isSuccess: true, 
                orderId: testOrderId
            );

            // Act
            var result = await _vnPayService.ProcessPaymentResponseAsync(queryCollection);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().BeOfType<PaymentResponseDTO>();

            var response = (PaymentResponseDTO)result.Data!;
            response.PaymentMethod.Should().Be(PaymentMethod.VNPay);
            response.Success.Should().BeTrue();
            response.OrderId.Should().Be(testOrderId);
            response.SignatureValid.Should().BeTrue(); // Signature valid với config thật
        }

        [Fact]
        public async Task ProcessPaymentResponseAsync_WithFailureResponse_ShouldReturnFailureWithMessage()
        {
            // Arrange - Sử dụng config thực
            var queryCollection = MockService.CreateMockVNPayResponseWithRealConfig(
                _vnpayConfig, 
                isSuccess: false
            );

            // Act
            var result = await _vnPayService.ProcessPaymentResponseAsync(queryCollection);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue(); // Processing successful
            result.Data.Should().BeOfType<PaymentResponseDTO>();

            var response = (PaymentResponseDTO)result.Data!;
            response.Success.Should().BeFalse(); // Payment failed
            response.PaymentMethod.Should().Be(PaymentMethod.VNPay);
            response.ErrorMessage.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void ValidatePaymentResponse_WithRealConfigData_ShouldValidateCorrectly()
        {
            // Arrange - Sử dụng config thực để tạo signature
            var queryCollection = MockService.CreateMockVNPayResponseWithRealConfig(_vnpayConfig);

            // Act
            var result = _vnPayService.ValidatePaymentResponse(queryCollection);

            // Assert
            result.Should().BeTrue(); // Bây giờ signature validation sẽ pass!
        }
    }
}