namespace Blog.Application.Responses
{
    public class Response<T>
    {
        public Response(bool isSuccess, string message, bool isFound = true, T data = default)
        {
            IsSuccess = isSuccess;
            IsFound = isFound;
            Message = message;
            Data = data;
        }

        public bool IsSuccess { get; set; }
        public bool IsFound { get; set; }
        public string Message { get; set; }
        public T Data { get; }

        public static Response<T> Success(T data, string message = "Successful Operation.")
        {
            return new Response<T>(true, message, data: data);
        }

        public static Response<T> Failure(string message = "Operation Failed.")
        {
            return new Response<T>(false, message);
        }

        public static Response<T> NotFound(string message = "Not Found.")
        {
            return new Response<T>(false, message, false);
        }
    }
}
