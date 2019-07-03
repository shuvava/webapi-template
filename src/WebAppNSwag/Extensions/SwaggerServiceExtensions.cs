using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using WebAppNSwag.Models;


namespace WebAppNSwag.Extensions
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services
                .AddOpenApiDocument(doc =>
                {
                    doc.Title = "Version 1";
                    doc.DocumentName = $"v{ApiVersions.V1}";
                    doc.ApiGroupNames = new[] {ApiVersions.V1};
                })
                .AddOpenApiDocument(doc =>
                {
                    doc.Title = "Version 2";
                    doc.DocumentName = $"v{ApiVersions.V2}";
                    doc.ApiGroupNames = new[] {ApiVersions.V2};
                });
            return services;
        }


        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseOpenApi();
            app.UseSwaggerUi3();
//            app.UseReDoc();
            return app;
        }
    }
}
