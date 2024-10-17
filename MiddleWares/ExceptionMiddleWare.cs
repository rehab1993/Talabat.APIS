using System.Net;
using System.Text.Json;
using Talabat.APIS.Errors;

namespace Talabat.APIS.MiddleWares
{
    public class ExceptionMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleWare> _logger;
        private readonly IHostEnvironment _environment;

        public ExceptionMiddleWare(RequestDelegate Next, ILogger<ExceptionMiddleWare> Logger, IHostEnvironment environment)
        {
            _next = Next;
            _logger = Logger;
            _environment = environment;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
              await _next.Invoke(context);
            }
            catch (Exception ex) {
                _logger.LogError(ex,ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                //if (_environment.IsDevelopment())
                //{
                //    var Response = new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString());

                //}
                //else {
                //    var response = new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);
                //}
                var Response = _environment.IsDevelopment() ? new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString()) : new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);
                var options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                var JsonResponse=JsonSerializer.Serialize(Response,options);
                context.Response.WriteAsync(JsonResponse);



            }

            
        }
    } 
}

