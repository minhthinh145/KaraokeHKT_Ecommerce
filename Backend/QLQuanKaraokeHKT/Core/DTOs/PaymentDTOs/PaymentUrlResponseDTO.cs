using QLQuanKaraokeHKT.Core.Enums;

namespace QLQuanKaraokeHKT.Core.DTOs.PaymentDTOs
{
    public class PaymentUrlResponseDTO
    {
        public string PaymentUrl { get; set; } = null!;
        public Guid OrderId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public DateTime ExpiryTime { get; set; }
        public Dictionary<string, object>? ProviderData { get; set; }
    }
}