namespace QLQuanKaraokeHKT.Application.Services
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

    public class ServiceResult<T> : ServiceResult
    {
        public new T? Data { get; set; }

        public static ServiceResult<T> Success(string message = "", T? data = default)
        {
            return new ServiceResult<T>
            {
                IsSuccess = true,
                Message = message,
                Data = data
            };
        }

        public static new ServiceResult<T> Failure(string message = "", object? errorData = null)
        {
            return new ServiceResult<T>
            {
                IsSuccess = false,
                Message = message,
                Data = default
            };
        }
    }
}
