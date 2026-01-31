namespace QLQuanKaraokeHKT.Application.DTOs.VNPayDTOs
{
    public class VNPayResponseDTO
    {
        public bool Success { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public string OrderDescription { get; set; } = null!;
        public Guid OrderId { get; set; }
        public string PaymentId { get; set; } = null!;
        public string TransactionId { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string VnPayResponseCode { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? ErrorMessage { get; set; }
    }
}