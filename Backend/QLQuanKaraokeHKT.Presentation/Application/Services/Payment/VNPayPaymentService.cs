using Microsoft.Extensions.Options;
using QLQuanKaraokeHKT.Shared.Constants;
using QLQuanKaraokeHKT.Application.Helpers;
using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.PaymentDTOs;
using QLQuanKaraokeHKT.Core.Enums;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Payment;

namespace QLQuanKaraokeHKT.Application.Services.Payment
{
    public class VNPayPaymentService : IPaymentService
    {
        private readonly VNPayConfig _vnPayConfig;
        private readonly ILogger<VNPayPaymentService> _logger;

        public PaymentMethod PaymentMethod => PaymentMethod.VNPay;

        public VNPayPaymentService(IOptions<VNPayConfig> vnPayConfig, ILogger<VNPayPaymentService> logger)
        {
            _vnPayConfig = vnPayConfig.Value;
            _logger = logger;
        }

        public async Task<ServiceResult> CreatePaymentUrlAsync(PaymentRequestDTO request, HttpContext httpContext)
        {
            try
            {
                if (request.Amount <= 0)
                    return ServiceResult.Failure(PaymentErrorMessage.InvalidAmount);

                if (request.OrderId == Guid.Empty)
                    return ServiceResult.Failure(PaymentErrorMessage.InvalidOrderId);

                var vnpay = new VNPayHelper();
                var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);

                var returnUrl = string.IsNullOrEmpty(request.ReturnUrl) ? _vnPayConfig.ReturnUrl : request.ReturnUrl;

                vnpay.AddRequestData("vnp_Version", _vnPayConfig.Version);
                vnpay.AddRequestData("vnp_Command", _vnPayConfig.Command);
                vnpay.AddRequestData("vnp_TmnCode", _vnPayConfig.TmnCode);
                vnpay.AddRequestData("vnp_Amount", ((long)(request.Amount * 100)).ToString());
                vnpay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
                vnpay.AddRequestData("vnp_CurrCode", request.Currency);
                vnpay.AddRequestData("vnp_IpAddr", VNPayHelper.Utils.GetIpAddress(httpContext));
                vnpay.AddRequestData("vnp_Locale", request.Locale);
                vnpay.AddRequestData("vnp_OrderInfo", request.OrderDescription);
                vnpay.AddRequestData("vnp_OrderType", "billpayment");

                var scheme = httpContext.Request.IsHttps ? "https" : "http";
                var host = httpContext.Request.Host.ToString();
                var callbackUrl = $"{scheme}://{host}/api/Payment/vnpay-callback";
                
                vnpay.AddRequestData("vnp_ReturnUrl", callbackUrl);
                vnpay.AddRequestData("vnp_TxnRef", request.OrderId.ToString());

                var paymentUrl = vnpay.CreatePaymentUrl(_vnPayConfig.BaseUrl, _vnPayConfig.HashSecret);

                var response = new PaymentUrlResponseDTO
                {
                    PaymentUrl = paymentUrl,
                    OrderId = request.OrderId,
                    PaymentMethod = PaymentMethod,
                    ExpiryTime = DateTime.Now.AddMinutes(request.ExpiryMinutes)
                };

                return ServiceResult.Success(PaymentErrorMessage.Success.UrlCreated, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi tạo URL thanh toán VNPay cho đơn hàng {OrderId}", request.OrderId);
                return ServiceResult.Failure(PaymentErrorMessage.CreateUrlFailed);
            }
        }

        public async Task<ServiceResult> ProcessPaymentResponseAsync(IQueryCollection responseData)
        {
            try
            {
                if (!responseData.Any())
                    return ServiceResult.Failure(PaymentErrorMessage.NoResponseFromGateway);

                var vnpay = new VNPayHelper();

                foreach (var (key, value) in responseData)
                {
                    if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(key, value);
                    }
                }

                var orderIdStr = vnpay.GetResponseData("vnp_TxnRef");
                if (string.IsNullOrEmpty(orderIdStr) || !Guid.TryParse(orderIdStr, out Guid orderId))
                    return ServiceResult.Failure(PaymentErrorMessage.InvalidOrderId);

                var vnpTransactionId = vnpay.GetResponseData("vnp_TransactionNo");
                var vnpResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                var vnpOrderInfo = vnpay.GetResponseData("vnp_OrderInfo");
                var vnpAmountStr = vnpay.GetResponseData("vnp_Amount");
                var vnpPayDateStr = vnpay.GetResponseData("vnp_PayDate");

                if (!decimal.TryParse(vnpAmountStr, out decimal vnpAmountRaw))
                    return ServiceResult.Failure(PaymentErrorMessage.InvalidTransactionAmount);

                if (!DateTime.TryParseExact(vnpPayDateStr, "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out DateTime vnpPayDate))
                    return ServiceResult.Failure(PaymentErrorMessage.InvalidPaymentDate);

                var vnpAmount = vnpAmountRaw / 100;

                bool checkSignature = ValidatePaymentResponse(responseData);
                if (!checkSignature)
                {
                    return ServiceResult.Failure(PaymentErrorMessage.InvalidPaymentResponse);
                }

                var response = new PaymentResponseDTO
                {
                    Success = vnpResponseCode == "00",
                    PaymentMethod = PaymentMethod,
                    OrderDescription = vnpOrderInfo,
                    OrderId = orderId,
                    TransactionId = vnpTransactionId,
                    ResponseCode = vnpResponseCode,
                    Amount = vnpAmount,
                    PaymentDate = vnpPayDate,
                    SignatureValid = checkSignature
                };

                if (!response.Success)
                {
                    response.ErrorMessage = VNPayErrorCodeMessage.GetErrorMessage(vnpResponseCode);
                }

                return ServiceResult.Success(PaymentErrorMessage.Success.ResponseProcessed, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi xử lý phản hồi VNPay");
                return ServiceResult.Failure("Không thể xử lý kết quả thanh toán, vui lòng liên hệ hỗ trợ");
            }
        }

        public bool ValidatePaymentResponse(IQueryCollection responseData)
        {
            try
            {
                var vnpay = new VNPayHelper();

                foreach (var (key, value) in responseData)
                {
                    if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(key, value);
                    }
                }

                var vnpSecureHash = responseData["vnp_SecureHash"];
                return vnpay.ValidateSignature(vnpSecureHash, _vnPayConfig.HashSecret);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi xác thực chữ ký VNPay");
                return false;
            }
        }

        public async Task<ServiceResult> GetPaymentStatusAsync(string transactionId)
        {
            return ServiceResult.Failure(PaymentErrorMessage.StatusQueryNotSupported);
        }


    }

    public class VNPayConfig
    {
        public string TmnCode { get; set; } = null!;
        public string HashSecret { get; set; } = null!;
        public string BaseUrl { get; set; } = null!;
        public string Command { get; set; } = "pay";
        public string CurrCode { get; set; } = "VND";
        public string Version { get; set; } = "2.1.0";
        public string Locale { get; set; } = "vn";
        public string ReturnUrl { get; set; } = null!;
        public string IPNUrl { get; set; } = null!;
    }
}