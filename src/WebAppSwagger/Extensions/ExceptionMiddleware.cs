using System;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;


namespace WebAppSwagger.Extensions
{
    public class ExceptionMiddleware
    {
        private const string ContentType = "application/json";
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;


        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }


        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext).ConfigureAwait(false);
            }
        }


        private Task HandleExceptionAsync(HttpContext context)
        {
            context.Response.ContentType = ContentType;
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;


            return context.Response.WriteAsync("Internal Server Error from the custom middleware.");
        }
    }
}
