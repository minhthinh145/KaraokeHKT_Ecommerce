using QLQuanKaraokeHKT.Shared.Enums;

namespace QLQuanKaraokeHKT.Shared.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this BookingStatus status)
        {
            return status switch
            {
                BookingStatus.Pending => "Chờ thanh toán",
                BookingStatus.DangSuDung => "Đang sử dụng",
                BookingStatus.DaKetThuc => "Đã kết thúc",
                BookingStatus.DaHuy => "Đã hủy",
                _ => "Không xác định"
            };
        }

        public static string GetDisplayName(this InvoiceStatus status)
        {
            return status switch
            {
                InvoiceStatus.ChuaThanhToan => "Chưa thanh toán",
                InvoiceStatus.DaThanhToan => "Đã thanh toán",
                InvoiceStatus.DaHuy => "Đã hủy",
                InvoiceStatus.HetHanThanhToan => "Hết hạn thanh toán",
                _ => "Không xác định"
            };
        }

        public static string GetDisplayName(this PaymentMethod method)
        {
            return method switch
            {
                PaymentMethod.VNPay => "VNPay",
                PaymentMethod.Momo => "Momo",
                PaymentMethod.ZaloPay => "ZaloPay",
                PaymentMethod.ShopeePay => "ShopeePay",
                _ => throw new ArgumentOutOfRangeException(nameof(method))
            };
        }
    }
}