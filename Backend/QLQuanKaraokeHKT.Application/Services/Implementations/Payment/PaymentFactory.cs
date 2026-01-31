using QLQuanKaraokeHKT.Application.Services.Interfaces.Payment;
using QLQuanKaraokeHKT.Shared.Constants;

namespace QLQuanKaraokeHKT.Application.Services.Implementations.Payment
{
    public class PaymentFactory : IPaymentFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<PaymentFactory> _logger;
        private readonly Dictionary<string, Type> _paymentServices;

        public PaymentFactory(IServiceProvider serviceProvider, ILogger<PaymentFactory> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _paymentServices = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase)
            {
                { "VNPay", typeof(VNPayPaymentService) }
            };
        }

        public IPaymentService GetPaymentService(string paymentMethod)
        {
            if (string.IsNullOrWhiteSpace(paymentMethod))
            {
                throw new NotSupportedException(PaymentErrorMessage.PaymentMethodEmpty);
            }
            if (!_paymentServices.ContainsKey(paymentMethod))
            {
                throw new NotSupportedException(PaymentErrorMessage.PaymentMethodNotSupported);
            }

            var serviceType = _paymentServices[paymentMethod];
            var service = _serviceProvider.GetService(serviceType) as IPaymentService;
            
            if (service == null)
            {
                throw new InvalidOperationException($"Không thể khởi tạo dịch vụ thanh toán '{paymentMethod}'");
            }

            return service;
        }

        public IEnumerable<string> GetSupportedPaymentMethods()
        {
            return _paymentServices.Keys;
        }
    }
}