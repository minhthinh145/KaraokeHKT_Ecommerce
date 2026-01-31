using QLQuanKaraokeHKT.Application.DTOs.PaymentDTOs;
using QLQuanKaraokeHKT.Shared.Enums;

namespace QLQuanKaraokeHKT.Application.Services.Interfaces.Payment
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