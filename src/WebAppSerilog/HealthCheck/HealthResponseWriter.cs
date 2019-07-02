using System.Reflection;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;


namespace WebAppSerilog.HealthCheck
{
    public static class HealthResponseWriter
    {
        private static readonly JsonSerializerSettings Settings;
        private static readonly string Version;
        static HealthResponseWriter()
        {
            Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                Formatting = Formatting.Indented
            };

            Settings.Converters.Add(new StringEnumConverter {NamingStrategy = new CamelCaseNamingStrategy()});
        }
        public static Task WriteLivenessResponse(HttpContext httpContext,
            HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";

            var health = new ReadinessHealthReport
            {
                Version = Version,
                Status = result.Status,
            };
            var json = JsonConvert.SerializeObject(health, Settings);

            return httpContext.Response.WriteAsync(json);
        }

        public static Task WriteReadinessResponse(HttpContext httpContext,
            HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";
            var json = JsonConvert.SerializeObject(result, Settings);

            return httpContext.Response.WriteAsync(json);
        }
    }


    public struct ReadinessHealthReport
    {
        public string Version { get; set; }
        public HealthStatus Status { get; set;  }
    }
}
