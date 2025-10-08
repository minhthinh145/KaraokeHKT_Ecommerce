using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;
using Moq.Protected;

namespace QLQuanKaraokeHKT.UnitTests.Extension
{
    public static class MockService
    {
        public static ILogger<T> GetMockLogger<T>()
        {
            var mockLogger = new Mock<ILogger<T>>();
            mockLogger
                .Setup(logger => logger.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception?>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()))
                .Verifiable();

            return mockLogger.Object;
        }

        public static IHttpClientFactory GetMockHttpFactory(HttpStatusCode statusCode, string content = "")
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            var httpResponse = new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            };

            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);

            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://mockedurl.com")
            };

            var mockFactory = new Mock<IHttpClientFactory>();
            mockFactory
                .Setup(factory => factory.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            return mockFactory.Object;
        }

        public static HttpContext CreateMockHttpContext()
        {
            var context = new DefaultHttpContext();
            context.Request.Scheme = "https";
            context.Request.Host = new HostString("localhost:5066");
            context.Request.IsHttps = true;
            context.Connection.RemoteIpAddress = System.Net.IPAddress.Parse("127.0.0.1");
            return context;
        }

        /// <summary>
        /// Tạo VNPay response mock - PHẢI TRUYỀN VÀO CONFIG THẬT từ DI container
        /// KHÔNG hardcode hash secret!
        /// </summary>
        public static IQueryCollection CreateMockVNPayResponseWithRealConfig(VNPayConfig config, bool isSuccess = true, Guid? orderId = null)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config), "VNPayConfig không được null - phải inject từ DI container!");

            if (string.IsNullOrEmpty(config.HashSecret))
                throw new ArgumentException("HashSecret không được rỗng!", nameof(config));

            var testOrderId = orderId?.ToString() ?? Guid.NewGuid().ToString();
            var amount = "10000000"; // 100,000 VND * 100
            var bankCode = "NCB";
            var orderInfo = "Test payment for unit testing";
            var payDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            var responseCode = isSuccess ? "00" : "24";
            var transactionNo = Random.Shared.Next(10000000, 99999999).ToString();
            
            // SỬ DỤNG CONFIG THẬT từ appsettings
            var secureHash = GenerateRealVNPaySignature(
                amount, bankCode, orderInfo, payDate, responseCode, 
                config.TmnCode, transactionNo, testOrderId, config.HashSecret);
            
            var queryDict = new Dictionary<string, StringValues>
            {
                ["vnp_Amount"] = amount,
                ["vnp_BankCode"] = bankCode,
                ["vnp_OrderInfo"] = orderInfo,
                ["vnp_PayDate"] = payDate,
                ["vnp_ResponseCode"] = responseCode,
                ["vnp_TmnCode"] = config.TmnCode, // Từ config
                ["vnp_TransactionNo"] = transactionNo,
                ["vnp_TxnRef"] = testOrderId,
                ["vnp_SecureHash"] = secureHash
            };
            
            return new QueryCollection(queryDict);
        }

        /// <summary>
        /// DEPRECATED: Không dùng method này vì không an toàn
        /// Sử dụng CreateMockVNPayResponseWithRealConfig thay thế
        /// </summary>
        [Obsolete("Không an toàn - sử dụng CreateMockVNPayResponseWithRealConfig thay thế")]
        public static IQueryCollection CreateMockVNPayResponse(bool isSuccess = true, Guid? orderId = null, VNPayConfig? config = null)
        {
            throw new InvalidOperationException("Method này không an toàn vì hardcode sensitive data. Sử dụng CreateMockVNPayResponseWithRealConfig với config từ DI container.");
        }

        public static IQueryCollection CreateEmptyQueryCollection()
        {
            return new QueryCollection();
        }

        public static Mock<IPaymentService> CreateMockPaymentService(PaymentMethod method)
        {
            var mockService = new Mock<IPaymentService>();
            mockService.Setup(x => x.PaymentMethod).Returns(method);
            return mockService;
        }

        /// <summary>
        /// Tạo VNPay signature THẬT sử dụng VNPayHelper - SỬ DỤNG CONFIG THẬT
        /// </summary>
        private static string GenerateRealVNPaySignature(
            string amount, string bankCode, string orderInfo, string payDate, 
            string responseCode, string tmnCode, string transactionNo, 
            string txnRef, string hashSecret)
        {
            // Validate inputs - KHÔNG để lộ sensitive data
            if (string.IsNullOrEmpty(hashSecret))
                throw new ArgumentException("HashSecret không được rỗng!", nameof(hashSecret));

            // Tạo signature string theo cách VNPay làm (không bao gồm vnp_SecureHash)
            var signDataBuilder = new StringBuilder();
            var sortedData = new Dictionary<string, string>
            {
                {"vnp_Amount", amount},
                {"vnp_BankCode", bankCode},
                {"vnp_OrderInfo", orderInfo},
                {"vnp_PayDate", payDate},
                {"vnp_ResponseCode", responseCode},
                {"vnp_TmnCode", tmnCode},
                {"vnp_TransactionNo", transactionNo},
                {"vnp_TxnRef", txnRef}
            }.OrderBy(x => x.Key);

            foreach (var kvp in sortedData)
            {
                if (!string.IsNullOrEmpty(kvp.Value))
                {
                    signDataBuilder.Append($"{System.Net.WebUtility.UrlEncode(kvp.Key)}={System.Net.WebUtility.UrlEncode(kvp.Value)}&");
                }
            }

            // Remove last '&'
            var signData = signDataBuilder.ToString();
            if (signData.Length > 0)
            {
                signData = signData.Remove(signData.Length - 1, 1);
            }

            // Sử dụng VNPayHelper THẬT để tạo HMAC-SHA512 hash
            return VNPayHelper.HmacSHA512(hashSecret, signData);
        }
    }
}