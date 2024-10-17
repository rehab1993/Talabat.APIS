namespace Talabat.APIS.Errors
{
    public class ApiExceptionResponse:ApiResponse
    {
        public string? Details { get; set; }
        public ApiExceptionResponse(int StatusCode,string? Message=null,string? _details=null):base(StatusCode,Message)
        {
            Details = _details;

        }
      

        
    }
}
