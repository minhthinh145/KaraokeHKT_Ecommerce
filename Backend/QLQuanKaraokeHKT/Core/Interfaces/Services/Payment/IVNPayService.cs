using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.VNPayDTOs;

namespace QLQuanKaraokeHKT.Core.Interfaces.Services.Payment
{
    public interface IVNPayService
    {

        Task<ServiceResult> CreatePaymentUrlAsync(VNPayRequestDTO request, HttpContext httpContext);

        Task<ServiceResult> ProcessVNPayResponseAsync(IQueryCollection vnpayData);

        bool ValidateSignature(IQueryCollection vnpayData);
    }
}