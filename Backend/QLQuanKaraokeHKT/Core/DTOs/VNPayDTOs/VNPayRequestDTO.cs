using System.ComponentModel.DataAnnotations;

namespace QLQuanKaraokeHKT.Core.DTOs.VNPayDTOs
{
    public class VNPayRequestDTO
    {
        [Required]
        public Guid OrderId { get; set; } // MaHoaDon

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string OrderDescription { get; set; } = null!;

        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerPhone { get; set; }

        public string? ReturnUrl { get; set; }
    }
}