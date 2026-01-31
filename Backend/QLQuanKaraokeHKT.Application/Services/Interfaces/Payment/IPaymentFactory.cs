using QLQuanKaraokeHKT.Application.DTOs.PaymentDTOs;

namespace QLQuanKaraokeHKT.Application.Services.Interfaces.Payment
{
    public interface IPaymentFactory
    {
        IPaymentService GetPaymentService(string paymentMethod);
        IEnumerable<string> GetSupportedPaymentMethods();
    }
}