using System.Collections.Frozen;

namespace QLQuanKaraokeHKT.Shared.Constants
{
    public static class VNPayErrorCodeMessage
    {
        public const string SuccessCode = "00";
        public const string SuspiciousTransactionCode = "07";
        public const string InternetBankingNotRegisteredCode = "09";
        public const string AuthenticationFailedCode = "10";
        public const string TransactionExpiredCode = "11";
        public const string AccountLockedCode = "12";
        public const string InvalidOtpCode = "13";
        public const string TransactionCancelledCode = "24";
        public const string InsufficientBalanceCode = "51";
        public const string DailyLimitExceededCode = "65";
        public const string BankMaintenanceCode = "75";
        public const string PasswordFailedCode = "79";

        public static readonly FrozenDictionary<string, string> ErrorMessages = new Dictionary<string, string>
        {
            [SuccessCode] = "Giao dịch thành công",
            [SuspiciousTransactionCode] = "Giao dịch có dấu hiệu bất thường, vui lòng liên hệ ngân hàng",
            [InternetBankingNotRegisteredCode] = "Tài khoản chưa đăng ký dịch vụ Internet Banking",
            [AuthenticationFailedCode] = "Thông tin xác thực không chính xác quá 3 lần",
            [TransactionExpiredCode] = "Giao dịch đã hết thời gian thanh toán",
            [AccountLockedCode] = "Tài khoản đã bị khóa",
            [InvalidOtpCode] = "Mã OTP không chính xác",
            [TransactionCancelledCode] = "Giao dịch đã bị hủy",
            [InsufficientBalanceCode] = "Tài khoản không đủ số dư",
            [DailyLimitExceededCode] = "Đã vượt quá hạn mức giao dịch trong ngày",
            [BankMaintenanceCode] = "Ngân hàng đang bảo trì, vui lòng thử lại sau",
            [PasswordFailedCode] = "Nhập sai mật khẩu quá nhiều lần"
        }.ToFrozenDictionary();

        public const string DefaultErrorMessage = "Giao dịch không thành công, vui lòng thử lại";

        public static string GetErrorMessage(string responseCode)
        {
            return ErrorMessages.TryGetValue(responseCode, out var message) 
                ? message 
                : DefaultErrorMessage;
        }

        public static bool IsSuccess(string responseCode)
        {
            return responseCode == SuccessCode;
        }
    }
}