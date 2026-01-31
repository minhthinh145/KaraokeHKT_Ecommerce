﻿using System.ComponentModel.DataAnnotations;
using QLQuanKaraokeHKT.Shared.Enums;

namespace QLQuanKaraokeHKT.Application.DTOs.PaymentDTOs
{
    public class PaymentRequestDTO
    {
        [Required]
        public Guid OrderId { get; set; }

        [Required]
        [Range(1000, double.MaxValue, ErrorMessage = "Số tiền thanh toán phải lớn hơn 1.000 VNĐ")]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Mô tả đơn hàng không được vượt quá 255 ký tự")]
        public string OrderDescription { get; set; } = null!;

        public string? CustomerName { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string? CustomerEmail { get; set; }

        [RegularExpression(@"^(\+84|0)[0-9]{9,10}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string? CustomerPhone { get; set; }

        public string? ReturnUrl { get; set; }

        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.VNPay;

        public Dictionary<string, object>? Metadata { get; set; }

        public string Locale { get; set; } = "vn";

        public string Currency { get; set; } = "VND";

        public int ExpiryMinutes { get; set; } = 15;
    }
}