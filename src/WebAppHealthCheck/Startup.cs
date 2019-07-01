using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

using WebAppHealthCheck.Extensions;
using WebAppHealthCheck.HealthCheck;


namespace WebAppHealthCheck
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApiVersioning( options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
            });

            services.ConfigureHealthProbe();

            services
                .AddMvcCore()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddApiExplorer()
                .AddAuthorization()
                //.AddCors()
                .AddJsonFormatters();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.ConfigureExceptionHandler(env);

            app.ConfigureHealthProbe();

            if (env.IsDevelopment())
            {
                //app.UseStaticFiles();
                //app.UseSwaggerDocumentation();
            }

            //app.UseAuthentication();
            app.UseMvc();
        }
    }
}
