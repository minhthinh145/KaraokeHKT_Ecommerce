using QLQuanKaraokeHKT.Core.Enums;

namespace QLQuanKaraokeHKT.Core.DTOs.PaymentDTOs
{
    public class PaymentResponseDTO
    {
        public bool Success { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string OrderDescription { get; set; } = null!;
        public Guid OrderId { get; set; }
        public string TransactionId { get; set; } = null!;
        public string? PaymentId { get; set; }
        public string ResponseCode { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? ErrorMessage { get; set; }
        public Dictionary<string, object>? AdditionalData { get; set; }
        public bool SignatureValid { get; set; } = true;
    }
}