using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;


namespace WebAppHealthCheck.HealthCheck
{
    /// <summary>
    /// links:
    ///     https://github.com/xabaril/AspNetCore.Diagnostics.HealthChecks
    ///     https://github.com/aspnet/AspNetCore.Docs/tree/master/aspnetcore/host-and-deploy/health-checks
    /// </summary>
    public static class HealthProbeStartup
    {
        private const string HealthCheckServiceAssembly =
            "Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckPublisherHostedService";


        public static IServiceCollection ConfigureHealthProbe(this IServiceCollection services)
        {
            services.AddHostedService<StartupHostedService>();
            services.AddSingleton<StartupHostedServiceHealthCheck>();



            services.AddHealthChecks()
                .AddCheck("Example", () =>
                    HealthCheckResult.Healthy("Example is OK!"), tags: new[] {"example"})
                .AddCheck("Example Unhealthy", () =>
                    HealthCheckResult.Unhealthy("Example is OK!"), tags: new[] {"example"})
                .AddCheck<ExampleHealthCheck>(
                    "example_health_check",
                    failureStatus: HealthStatus.Degraded,
                    tags: new[] {"example"})
                .AddCheck<StartupHostedServiceHealthCheck>(
                    "hosted_service_startup",
                    HealthStatus.Degraded,
                    new[] {"ready"});

            services.Configure<HealthCheckPublisherOptions>(options =>
            {
                options.Delay = TimeSpan.FromSeconds(2);
                options.Predicate = check => check.Tags.Contains("ready");
            });

            // The following workaround permits adding an IHealthCheckPublisher
            // instance to the service container when one or more other hosted
            // services have already been added to the app. This workaround
            // won't be required with the release of ASP.NET Core 3.0. For more
            // information, see: https://github.com/aspnet/Extensions/issues/639.
            services.TryAddEnumerable(
                ServiceDescriptor.Singleton(typeof(IHostedService),
                    typeof(HealthCheckPublisherOptions).Assembly
                        .GetType(HealthCheckServiceAssembly)));

            services.AddSingleton<IHealthCheckPublisher, ReadinessPublisher>();

            return services;
        }


        public static IApplicationBuilder ConfigureHealthProbe(this IApplicationBuilder app)
        {

            // from https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-2.2
            app.UseHealthChecks("/health/example", new HealthCheckOptions()
            {
                // WriteResponse is a delegate used to write the response.
                ResponseWriter = HealthResponseWriter.WriteReadinessResponse,
                // The readiness check uses all registered checks with the 'ready' tag.
                Predicate = check => check.Tags.Contains("example")
            });
            app.UseHealthChecks("/health/ready", new HealthCheckOptions()
            {
                // WriteResponse is a delegate used to write the response.
                ResponseWriter = HealthResponseWriter.WriteReadinessResponse,
                // The readiness check uses all registered checks with the 'ready' tag.
                Predicate = check => check.Tags.Contains("ready")
            });
            app.UseHealthChecks("/health/live", new HealthCheckOptions()
            {
                // WriteResponse is a delegate used to write the response.
                ResponseWriter = HealthResponseWriter.WriteLivenessResponse,
                // Exclude all checks and return a 200-Ok.
                Predicate = (_) => false,
            });

            return app;
        }
    }
}
