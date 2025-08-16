using QLQuanKaraokeHKT.DTOs.VNPayDTOs;
using QLQuanKaraokeHKT.Helpers;

namespace QLQuanKaraokeHKT.Services.VNPayServices
{
    public interface IVNPayService
    {
        /// <summary>
        /// Tạo URL thanh toán VNPay
        /// </summary>
        Task<ServiceResult> CreatePaymentUrlAsync(VNPayRequestDTO request, HttpContext httpContext);

        /// <summary>
        /// Xử lý phản hồi từ VNPay
        /// </summary>
        Task<ServiceResult> ProcessVNPayResponseAsync(IQueryCollection vnpayData);

        /// <summary>
        /// Xác thực chữ ký từ VNPay
        /// </summary>
        bool ValidateSignature(IQueryCollection vnpayData);
    }
}