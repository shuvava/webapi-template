using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;


namespace WebAppSerilog.Extensions
{
    /// <summary>
    /// by default AspNetCore add Header Request-Id to the logging scope with value "ParentId"
    /// <example>
    /// GET /api/values HTTP/1.1
    /// Host: localhost:5000
    /// Content-Type: application/json
    /// Request-Id: 0HLR8KN4T872T:00000002
    /// >>>>>>> some.log <<<<<<<<<<
    ///     {RequestId}            {ParentId}
    /// ... 0HLR8M0BN0LP8:00000001 0HLR8KN4T872T:00000002 ....
    /// >>>>>>> some.log <<<<<<<<<<
    /// </example>
    /// </summary>
    public class CorrelationIdMiddleware
    {
        private static readonly string CorrelationIdHeaderName = "Request-Id";
        private readonly RequestDelegate _next;


        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task InvokeAsync(HttpContext httpContext)
        {
            httpContext
                .Response
                .Headers
                .Add(CorrelationIdHeaderName, httpContext.TraceIdentifier);

            await _next(httpContext);
        }
    }


    public static class CorrelationIdMiddlewareExtension
    {
        public static IApplicationBuilder UseCorrelationIdMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CorrelationIdMiddleware>();
        }
    }
}
