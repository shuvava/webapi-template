using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using NSwag.Generation.Processors;

using WebAppNSwag.Extensions.Swagger;
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
                    doc.OperationProcessors.Insert(0, new ApiVersionProcessor1()); // I do not know why built-in implementation remove header attribute
                })
                .AddOpenApiDocument(doc =>
                {
                    doc.Title = "Version 2";
                    doc.DocumentName = $"v{ApiVersions.V2}";
                    doc.ApiGroupNames = new[] {ApiVersions.V2};
                    doc.OperationProcessors.Insert(0, new ApiVersionProcessor());
                });
            return services;
        }


        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            // Add OpenAPI/Swagger middlewares
            app.UseOpenApi(); // Serves the registered OpenAPI/Swagger documents by default on `/swagger/{documentName}/swagger.json`
            app.UseSwaggerUi3();// Serves the Swagger UI 3 web ui to view the OpenAPI/Swagger documents by default on `/swagger`
//            app.UseReDoc();
            return app;
        }
    }
}
