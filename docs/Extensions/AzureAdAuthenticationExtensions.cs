using System;

using Microsoft.Extensions.Options;

using NopClient.Services.WebApp.Extensions;


namespace WebAppBase.Extensions
{
    public static class AzureAdAuthenticationExtensions
    {
        public static AuthenticationBuilder AddAzureAd(this AuthenticationBuilder builder,
            Action<AzureAdOptions> configureOptions)
        {
            builder.Services.Configure(configureOptions);
            builder.Services.AddSingleton<IConfigureOptions<JwtBearerOptions>, JwtBearerConfiguration>();
            builder.AddJwtBearer();

            return builder;
        }
    }
}
