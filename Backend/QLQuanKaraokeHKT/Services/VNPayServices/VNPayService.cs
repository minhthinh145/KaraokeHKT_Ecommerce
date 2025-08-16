using Microsoft.Extensions.Options;
using QLQuanKaraokeHKT.DTOs.VNPayDTOs;
using QLQuanKaraokeHKT.Helpers;

namespace QLQuanKaraokeHKT.Services.VNPayServices
{
    public class VNPayService : IVNPayService
    {
        private readonly VNPayConfig _vnPayConfig;
        private readonly ILogger<VNPayService> _logger;

        public VNPayService(IOptions<VNPayConfig> vnPayConfig, ILogger<VNPayService> logger)
        {
            _vnPayConfig = vnPayConfig.Value;
            _logger = logger;
        }

        public async Task<ServiceResult> CreatePaymentUrlAsync(VNPayRequestDTO request, HttpContext httpContext)
        {
            try
            {
                var vnpay = new VNPayHelper();
                var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);

                // ✅ SỬ DỤNG ReturnUrl TỪ CONFIG
                var returnUrl = string.IsNullOrEmpty(request.ReturnUrl) ? _vnPayConfig.ReturnUrl : request.ReturnUrl;

                vnpay.AddRequestData("vnp_Version", _vnPayConfig.Version);
                vnpay.AddRequestData("vnp_Command", _vnPayConfig.Command);
                vnpay.AddRequestData("vnp_TmnCode", _vnPayConfig.TmnCode);
                vnpay.AddRequestData("vnp_Amount", ((long)(request.Amount * 100)).ToString());
                vnpay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
                vnpay.AddRequestData("vnp_CurrCode", _vnPayConfig.CurrCode);
                vnpay.AddRequestData("vnp_IpAddr", VNPayHelper.Utils.GetIpAddress(httpContext));
                vnpay.AddRequestData("vnp_Locale", _vnPayConfig.Locale);
                vnpay.AddRequestData("vnp_OrderInfo", request.OrderDescription);
                vnpay.AddRequestData("vnp_OrderType", "billpayment");

                // ✅ KIỂM TRA VÀ SỬ DỤNG ĐÚNG PROTOCOL
                var scheme = httpContext.Request.IsHttps ? "https" : "http";
                var host = httpContext.Request.Host.ToString();
                
                // Nếu frontend và backend khác port
                var callbackUrl = $"{scheme}://{host}/api/KhachHangBooking/vnpay-callback";
                
                vnpay.AddRequestData("vnp_ReturnUrl", callbackUrl);

                vnpay.AddRequestData("vnp_TxnRef", request.OrderId.ToString());

                var paymentUrl = vnpay.CreatePaymentUrl(_vnPayConfig.BaseUrl, _vnPayConfig.HashSecret);

                return ServiceResult.Success("Tạo URL thanh toán thành công.", paymentUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo URL thanh toán VNPay");
                return ServiceResult.Failure($"Lỗi khi tạo URL thanh toán: {ex.Message}");
            }
        }

        public async Task<ServiceResult> ProcessVNPayResponseAsync(IQueryCollection vnpayData)
        {
            try
            {
                var vnpay = new VNPayHelper();

                foreach (var (key, value) in vnpayData)
                {
                    if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(key, value);
                    }
                }

                var orderId = Guid.Parse(vnpay.GetResponseData("vnp_TxnRef"));
                var vnpTransactionId = vnpay.GetResponseData("vnp_TransactionNo");
                var vnpSecureHash = vnpayData["vnp_SecureHash"];
                var vnpResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                var vnpOrderInfo = vnpay.GetResponseData("vnp_OrderInfo");
                var vnpAmount = Convert.ToDecimal(vnpay.GetResponseData("vnp_Amount")) / 100;
                var vnpPayDate = DateTime.ParseExact(vnpay.GetResponseData("vnp_PayDate"), "yyyyMMddHHmmss", null);

                bool checkSignature = ValidateSignature(vnpayData);
                if (!checkSignature)
                {
                    return ServiceResult.Failure("Chữ ký không hợp lệ");
                }

                var response = new VNPayResponseDTO
                {
                    Success = vnpResponseCode == "00",
                    PaymentMethod = "VNPay",
                    OrderDescription = vnpOrderInfo,
                    OrderId = orderId,
                    TransactionId = vnpTransactionId,
                    VnPayResponseCode = vnpResponseCode,
                    Amount = vnpAmount,
                    PaymentDate = vnpPayDate
                };

                if (!response.Success)
                {
                    response.ErrorMessage = GetVNPayResponseMessage(vnpResponseCode);
                }

                return ServiceResult.Success("Xử lý phản hồi VNPay thành công.", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xử lý phản hồi VNPay");
                return ServiceResult.Failure($"Lỗi khi xử lý phản hồi thanh toán: {ex.Message}");
            }
        }

        public bool ValidateSignature(IQueryCollection vnpayData)
        {
            try
            {
                var vnpay = new VNPayHelper();

                foreach (var (key, value) in vnpayData)
                {
                    if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(key, value);
                    }
                }

                var vnpSecureHash = vnpayData["vnp_SecureHash"];
                return vnpay.ValidateSignature(vnpSecureHash, _vnPayConfig.HashSecret);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xác thực chữ ký VNPay");
                return false;
            }
        }

        private string GetVNPayResponseMessage(string responseCode)
        {
            return responseCode switch
            {
                "00" => "Giao dịch thành công",
                "07" => "Trừ tiền thành công. Giao dịch bị nghi ngờ (liên quan tới lừa đảo, giao dịch bất thường)",
                "09" => "Giao dịch không thành công do: Thẻ/Tài khoản của khách hàng chưa đăng ký dịch vụ InternetBanking tại ngân hàng",
                "10" => "Giao dịch không thành công do: Khách hàng xác thực thông tin thẻ/tài khoản không đúng quá 3 lần",
                "11" => "Giao dịch không thành công do: Đã hết hạn chờ thanh toán",
                "12" => "Giao dịch không thành công do: Thẻ/Tài khoản của khách hàng bị khóa",
                "13" => "Giao dịch không thành công do Quý khách nhập sai mật khẩu xác thực giao dịch (OTP)",
                "24" => "Giao dịch không thành công do: Khách hàng hủy giao dịch",
                "51" => "Giao dịch không thành công do: Tài khoản của quý khách không đủ số dư để thực hiện giao dịch",
                "65" => "Giao dịch không thành công do: Tài khoản của Quý khách đã vượt quá hạn mức giao dịch trong ngày",
                "75" => "Ngân hàng thanh toán đang bảo trì",
                "79" => "Giao dịch không thành công do: KH nhập sai mật khẩu thanh toán quá số lần quy định",
                _ => "Giao dịch không thành công"
            };
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