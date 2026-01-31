namespace QLQuanKaraokeHKT.Shared.Constants
{
    public static class PaymentErrorMessage
    {
        public const string InvalidAmount = "Số tiền thanh toán phải lớn hơn 0";
        public const string InvalidOrderId = "Mã đơn hàng không hợp lệ";
        public const string PaymentServiceNotFound = "Không thể khởi tạo dịch vụ thanh toán";
        public const string PaymentMethodNotSupported = "Phương thức thanh toán chưa được hỗ trợ";
        public const string PaymentMethodEmpty = "Phương thức thanh toán không được để trống";
        public const string CreateUrlFailed = "Không thể tạo liên kết thanh toán, vui lòng thử lại sau";
        public const string NoResponseFromGateway = "Không nhận được phản hồi từ cổng thanh toán";
        public const string InvalidPaymentResponse = "Phản hồi thanh toán không hợp lệ";
        public const string InvalidSignature = "Chữ ký thanh toán không hợp lệ";
        public const string InvalidTransactionAmount = "Số tiền giao dịch không hợp lệ";
        public const string InvalidPaymentDate = "Thời gian thanh toán không hợp lệ";
        public const string ProcessingFailed = "Không thể xử lý kết quả thanh toán, vui lòng liên hệ hỗ trợ";
        public const string StatusQueryNotSupported = "Tính năng truy vấn trạng thái chưa được hỗ trợ";
        public const string InvalidQueryParams = "Thông tin truy vấn không hợp lệ";
        public const string QueryStatusFailed = "Không thể truy vấn trạng thái thanh toán";
        public const string PaymentProcessingError = "Có lỗi xảy ra trong quá trình xử lý thanh toán";
        public const string InvalidRequestData = "Thông tin yêu cầu không hợp lệ";
        public static class Success
        {
            public const string UrlCreated = "Tạo liên kết thanh toán thành công";
            public const string ResponseProcessed = "Xử lý phản hồi thanh toán thành công";
            public const string MethodsRetrieved = "Lấy danh sách phương thức thanh toán thành công";
        }
    }
}