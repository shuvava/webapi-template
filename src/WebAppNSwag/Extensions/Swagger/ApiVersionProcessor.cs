using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Namotion.Reflection;

using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;


namespace WebAppNSwag.Extensions.Swagger
{
    public class ApiVersionProcessor1 : IOperationProcessor
    {
        /// <summary>
        ///     Gets or sets the list of versions which should be included in the generated document (leave empty to include
        ///     all versions).
        /// </summary>
        public string[] IncludedVersions { get; set; }


        /// <summary>
        ///     Gets or sets a value indicating whether to ignore the global API version parameter (ASP.NET Core only,
        ///     default: true).
        /// </summary>
        public bool IgnoreParameter { get; set; } = true;


        /// <summary>Processes the specified method information.</summary>
        /// <param name="context">The processor context.</param>
        /// <returns>true if the operation should be added to the Swagger specification.</returns>
        public bool Process(OperationProcessorContext context)
        {
            if (UseVersionedApiExplorer(context))
            {
                var dynamicContext = (dynamic) context;
                var version = (string) dynamicContext.ApiDescription.GroupName;

                var isIncluded = IncludedVersions == null || IncludedVersions.Contains(version);

                if (isIncluded)
                {
                    RemoveApiVersionPathParameter(context, version);
                }

                return isIncluded;
            }

            var versions = GetVersions(context, "ApiVersionAttribute");

            if (versions.Any())
            {
                if (versions.Any(v =>
                    IncludedVersions == null || IncludedVersions.Length == 0 || IncludedVersions.Contains(v)))
                {
                    var mappedVersions = GetVersions(context, "MapToApiVersionAttribute");

                    var version = mappedVersions.FirstOrDefault(v =>
                        IncludedVersions == null || IncludedVersions.Length == 0 || IncludedVersions.Contains(v));

                    if (version == null && mappedVersions.Length == 0)
                    {
                        version = IncludedVersions != null && IncludedVersions.Any()
                            ? IncludedVersions[0]
                            : versions[0];
                    }

                    if (version != null)
                    {
                        RemoveApiVersionPathParameter(context, version);

                        return true;
                    }

                    return false;
                }

                return false;
            }

            return true;
        }


        private bool UseVersionedApiExplorer(OperationProcessorContext context)
        {
            if (context.HasProperty("ApiDescription"))
            {
                var dynamicContext = (dynamic) context;

                var usesVersionedApiExplorer = ((IDictionary<object, object>) dynamicContext.ApiDescription.Properties)
                    .Any(tuple => ((dynamic) tuple.Key).FullName == "Microsoft.AspNetCore.Mvc.ApiVersion");

                return usesVersionedApiExplorer;
            }

            return false;
        }


        private void RemoveApiVersionPathParameter(OperationProcessorContext context, string version)
        {
            var operationDescription = context.OperationDescription;
            operationDescription.Path = operationDescription.Path.Replace("{version:apiVersion}", version);
            operationDescription.Path = operationDescription.Path.Replace("{version}", version);
        }


        private string[] GetVersions(OperationProcessorContext context, string attributeType)
        {
            var versionAttributes = context.MethodInfo.GetCustomAttributes()
                .Concat(context.MethodInfo.DeclaringType.GetTypeInfo().GetCustomAttributes())
                .Concat(context.ControllerType.GetTypeInfo().GetCustomAttributes(true))
                .GetAssignableToTypeName(attributeType, TypeNameStyle.Name)
                .Where(a => a.HasProperty("Versions"))
                .SelectMany((dynamic a) => ((IEnumerable) a.Versions).OfType<object>().Select(v => v.ToString()))
                .ToArray();

            return versionAttributes;
        }
    }
}
