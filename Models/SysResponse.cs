namespace ProductManagement.Models
{
    public class SysResponse
    {
        public bool? Status { get; set; }
        public string? Message { get; set; }
        public string[]? Errors { get; set; }
        public object? Data { get; set; }


        public static SysResponse Success(object? data = null , string? message = null)
        {
            return new SysResponse { Status = true, Data = data, Message = message };
        }

        public static SysResponse Error(object? data = null, string ? message = null, params string[] errors)
        {
            return new SysResponse { Status = false, Errors = errors, Data = data, Message = message};
        }
    }

    public class SysResponse<T>
    {
        public bool? Status { get; set; }
        public string? Message { get; set; }
        public string[]? Errors { get; set; }
        public T? Data { get; set; }


        public static SysResponse<T> Success(T data, string? message = null)
        {
            return new SysResponse<T> { Status = true, Data = data, Message = message };
        }

        public static SysResponse<T> Error(T data, string? message = null, params string[] errors)
        {
            return new SysResponse<T> { Status = false, Errors = errors, Data = data, Message = message };
        }
    }
}
