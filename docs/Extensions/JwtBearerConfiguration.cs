using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace WebAppBase.Extensions
{
    public class JwtBearerConfiguration : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly AzureAdOptions _azureOptions;
        private readonly ILogger _logger;


        public JwtBearerConfiguration(
            IOptions<AzureAdOptions> azureOptions,
            ILogger<JwtBearerConfiguration> logger)
        {
            _azureOptions = azureOptions.Value;
            _logger = logger;
        }


        public void Configure(JwtBearerOptions options)
        {
            Configure(Options.DefaultName, options);
        }


        public void Configure(string name, JwtBearerOptions options)
        {
            options.Audience = _azureOptions.Audiences;
            options.Authority = _azureOptions.IssuerUrl;
            options.RequireHttpsMetadata = true;
            options.SaveToken = true;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudiences = new List<string>
                {
                    _azureOptions.Audiences,
                    _azureOptions.ClientId
                },
                ValidateIssuer = true,
                ValidIssuer = _azureOptions.IssuerUrl,
                RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = OnAuthenticationFailed
            };
        }


        /// <summary>
        ///     this method is invoked if exceptions are thrown during request processing
        /// </summary>
        private Task OnAuthenticationFailed(AuthenticationFailedContext context)
        {
            _logger.LogError($"Authentication Failed(OnAuthenticationFailed): {context.Exception}");

            return Task.FromResult(0);
        }
    }
}
