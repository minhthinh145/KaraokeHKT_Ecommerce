using QLQuanKaraokeHKT.Core.DTOs.PaymentDTOs;

namespace QLQuanKaraokeHKT.Core.Interfaces.Services.Payment
{
    public interface IPaymentFactory
    {
        IPaymentService GetPaymentService(string paymentMethod);
        IEnumerable<string> GetSupportedPaymentMethods();
    }
}