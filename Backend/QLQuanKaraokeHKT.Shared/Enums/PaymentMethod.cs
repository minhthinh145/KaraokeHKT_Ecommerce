namespace QLQuanKaraokeHKT.Shared.Enums
{
    public enum PaymentMethod
    {
        VNPay = 1,
        Momo = 2,
        ZaloPay = 3,
        ShopeePay = 4
    }

    public static class PaymentMethodExtensions
    {
        public static string GetDisplayName(this PaymentMethod paymentMethod)
        {
            return paymentMethod switch
            {
                PaymentMethod.VNPay => "VNPay",
                PaymentMethod.Momo => "Momo",
                PaymentMethod.ZaloPay => "ZaloPay",
                PaymentMethod.ShopeePay => "ShopeePay",
                _ => throw new ArgumentOutOfRangeException(nameof(paymentMethod))
            };
        }

        public static PaymentMethod FromString(string paymentMethodString)
        {
            return paymentMethodString?.ToLower() switch
            {
                "vnpay" => PaymentMethod.VNPay,
                "momo" => PaymentMethod.Momo,
                "zalopay" => PaymentMethod.ZaloPay,
                "shopeepay" => PaymentMethod.ShopeePay,
                _ => throw new ArgumentException($"Payment method '{paymentMethodString}' is not supported")
            };
        }
    }
}