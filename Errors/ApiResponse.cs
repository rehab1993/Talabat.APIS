
namespace Talabat.APIS.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode,string? message = null) {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private string? GetDefaultMessageForStatusCode(int? statusCode)
        {
            //500=>Internal server error
            //400=>Bad request
            //401=>Unauthorized
            //404=>not found
            return statusCode switch
            {
                
                400 => "Bad request",
                401 => "You Are Not authorized",
                404 => "Resource not found",
                500 => "Internal server error",
                _ => null,

            };
        }

        public int StatusCode { get; set; }
        public string? Message { get; set; }
    }
}
