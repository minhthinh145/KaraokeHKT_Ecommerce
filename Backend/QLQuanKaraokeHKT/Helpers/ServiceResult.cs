namespace QLQuanKaraokeHKT.Helpers
{
    public class ServiceResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;

        public object? Data { get; set; }
        public static ServiceResult Success(string message = "", object data = null)
        {
            return new ServiceResult { IsSuccess = true, Message = message, Data = data };
        }

        public static ServiceResult Failure(string message = "", object data = null)
        {
            return new ServiceResult { IsSuccess = false, Message = message, Data = data };
        }

    }
}
