using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NopClient.Services.Repositories.Models;
using NopClient.Services.Repositories.Nop;


namespace NopClient.Services.WebApp.Extensions
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection AddNopServicesRepository(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<NopDocumentRepositoryOptions>(configuration.GetSection("NopDocumentRepository"));
            services.AddSingleton<INopRepository, NopRepositorySql>();

            return services;
        }
    }
}
