namespace Boilerplate.Helper
{
    public class ApiResponse<T> {
        public string? Error { get; set; }
        public T? Data {get; set;}

        public static ApiResponse<T> SuccessResponse(T data, string? error = null) => new() { Error = null, Data = data};

        public static ApiResponse<T> ErrorResponse(T data, string? error ) => new() {Error = error, Data = data};
    }
}