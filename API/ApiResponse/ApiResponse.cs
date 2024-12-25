using System.Net;

namespace API.ApiResponse
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public T Data { get; set; }

        // Başarılı bir response için kullanılan constructor
        public ApiResponse(T? data, string? message = "İşlem başarılı.", HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            IsSuccess = true;
            Message = message;
            StatusCode = statusCode;
            Data = data;
        }

        // Hatalı bir response için kullanılan constructor
        public ApiResponse(string? errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            IsSuccess = false;
            Message = errorMessage;
            StatusCode = statusCode;
            Data = default;
        }
    }
}
