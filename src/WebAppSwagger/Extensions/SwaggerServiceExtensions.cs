using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;

using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

using WebAppSwagger.Extensions.Swagger;
using WebAppSwagger.Models;


namespace WebAppSwagger.Extensions
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc($"v{ApiVersions.V1}", new Info { Title = "API", Version = $"v{ApiVersions.V1}" });
                    options.SwaggerDoc($"v{ApiVersions.V2}", new Info { Title = "API", Version = $"v{ApiVersions.V2}" });

                    options.DocInclusionPredicate((version, apiDescription) =>
                    {
                        var actionVersions = apiDescription.ActionAttributes().OfType<MapToApiVersionAttribute>().SelectMany(attr => attr.Versions);
                        var controllerVersions = apiDescription.ControllerAttributes().OfType<ApiVersionAttribute>().SelectMany(attr => attr.Versions);
                        var controllerAndActionVersionsOverlap = controllerVersions.Intersect(actionVersions).Any();
                        if (controllerAndActionVersionsOverlap)
                        {
                            return actionVersions.Any(v => $"v{v.ToString()}" == version);
                        }
                        if (!controllerVersions.Any() && version == $"v{ApiVersions.V1}")
                        {
                            return true;
                        }
                        return controllerVersions.Any(v => $"v{v.ToString()}" == version);
                    });
                    options.OperationFilter<RemoveVersionParameters>();
                    options.DocumentFilter<SetVersionInPaths>();

                    //var security = new Dictionary<string, IEnumerable<string>>
                    //{
                    //    {"Bearer", new string[] { }}
                    //};

                    //options.AddSecurityDefinition("Bearer", new ApiKeyScheme
                    //{
                    //    Description =
                    //        "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    //    Name = "Authorization",
                    //    In = "header",
                    //    Type = "apiKey"
                    //});

                    //options.AddSecurityRequirement(security);

                    // integrate xml comments
                    options.IncludeXmlComments( XmlCommentsFilePath );
                });

            return services;
        }


        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(
                options =>
                {
                    options.SwaggerEndpoint($"/swagger/v{ApiVersions.V1}/swagger.json", $"Versioned API v{ApiVersions.V1}");
                    options.SwaggerEndpoint($"/swagger/v{ApiVersions.V2}/swagger.json", $"Versioned API v{ApiVersions.V2}");
                    options.DocumentTitle = "Documentation Example";
                    options.DocExpansion(DocExpansion.None);
                });

            return app;
        }

        static string XmlCommentsFilePath
        {
            get
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = typeof( Startup ).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine( basePath, fileName );
            }
        }
    }
}
