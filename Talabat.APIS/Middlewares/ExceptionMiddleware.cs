using System.Net;
using System.Text.Json;
using Talabat.APIS.Errors;

namespace Talabat.APIS.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next ,ILogger<ExceptionMiddleware> logger,IHostEnvironment env) 
        {
            _next = next;
            _logger = logger;
            _env = env;
        }
        
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
               await _next.Invoke(context);

            }catch (Exception ex)
            {
                _logger.LogError(ex,ex.Message);
                //log exception Database (production)
                context.Response.ContentType = "appliction/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment() ?
                    new ApiExceptionResponse(500, ex.Message, ex.StackTrace.ToString())
                    : new ApiExceptionResponse(500);

                var json =JsonSerializer.Serialize(response);

                await context.Response.WriteAsync(json);
            }
        }
    }
}
