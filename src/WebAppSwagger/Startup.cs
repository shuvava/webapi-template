using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using WebAppSwagger.Extensions;
using WebAppSwagger.HealthCheck;


namespace WebAppSwagger
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
            //services.AddAuthentication(sharedOptions =>
            //    {
            //        sharedOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //        sharedOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    })
            //    .AddAzureAd(options => { Configuration.Bind("AzureAd", options); });
            //services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            //{
            //    builder
            //        .AllowAnyOrigin()
            //        .AllowAnyMethod()
            //        .AllowAnyHeader();
            //}));

            //services.AddApplicationInsightsTelemetry();
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new HeaderApiVersionReader("X-App-Version");
            });

            services.AddSwaggerDocumentation();

            //services.AddNopServicesRepository(Configuration);

            //services.AddMemoryCache();

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
            //loggerFactory
            //    .AddApplicationInsights(app.ApplicationServices, LogLevel.Information);

            //app.UseMiddleware<ExceptionMiddleware>();

            //app.UseCors("MyPolicy");
            //app.UseStaticFiles();

            app.ConfigureHealthProbe();

            if (env.IsDevelopment())
            {
                app.UseSwaggerDocumentation();
            }


            //app.UseAuthentication();
            app.UseMvc();
        }
    }
}
