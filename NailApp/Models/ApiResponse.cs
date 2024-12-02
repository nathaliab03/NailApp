using System.Net;

namespace NailApp.Models
{
    public class ApiResponse
    {
        public bool IsSuccess { get; set; }
        public List<string> Errors { get; set; }
        public Object Content { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public ApiResponse() { }
        public ApiResponse(bool isSuccess, object content = null, List<string> errors = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            IsSuccess = isSuccess;
            Content = content;
            Errors = errors ?? new List<string>();
            StatusCode = statusCode;
        }

        public static ApiResponse Success(object content, HttpStatusCode statusCode = HttpStatusCode.OK) =>
        new ApiResponse(true, content, null, statusCode);

        public static ApiResponse Error(List<string> errors, HttpStatusCode statusCode = HttpStatusCode.BadRequest) =>
            new ApiResponse(false, null, errors, statusCode);

        public static ApiResponse Error(string error, HttpStatusCode statusCode = HttpStatusCode.BadRequest) =>
            Error(new List<string> { error }, statusCode);
    }
}
