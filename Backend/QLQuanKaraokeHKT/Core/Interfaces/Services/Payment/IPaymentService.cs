using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.PaymentDTOs;
using QLQuanKaraokeHKT.Core.Enums;

namespace QLQuanKaraokeHKT.Core.Interfaces.Services.Payment
{
    public interface IPaymentService
    {
        PaymentMethod PaymentMethod { get; }
        
        Task<ServiceResult> CreatePaymentUrlAsync(PaymentRequestDTO request, HttpContext httpContext);
        
        Task<ServiceResult> ProcessPaymentResponseAsync(IQueryCollection responseData);
        
        bool ValidatePaymentResponse(IQueryCollection responseData);
        
        Task<ServiceResult> GetPaymentStatusAsync(string transactionId);
    }
}