using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;


namespace WebAppNSwag.HealthCheck
{
    public static class HealthProbeStartup
    {
        public static IServiceCollection ConfigureHealthProbe(this IServiceCollection services)
        {
            services.AddHealthChecks();

            return services;
        }


        public static IApplicationBuilder ConfigureHealthProbe(this IApplicationBuilder app)
        {
            // from https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-2.2
            app.UseHealthChecks("/health/ready", new HealthCheckOptions
            {
                // WriteResponse is a delegate used to write the response.
                ResponseWriter = HealthResponseWriter.WriteReadinessResponse,
                // The readiness check uses all registered checks with the 'ready' tag.
                Predicate = check => check.Tags.Contains("ready")
            });

            app.UseHealthChecks("/health/live", new HealthCheckOptions
            {
                // WriteResponse is a delegate used to write the response.
                ResponseWriter = HealthResponseWriter.WriteLivenessResponse,
                // Exclude all checks and return a 200-Ok.
                Predicate = _ => false
            });

            return app;
        }
    }
}
