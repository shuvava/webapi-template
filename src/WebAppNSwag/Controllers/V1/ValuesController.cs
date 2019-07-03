using Microsoft.AspNetCore.Mvc;

using NSwag.Annotations;

using WebAppNSwag.Models;


namespace WebAppNSwag.Controllers.V1
{
    [ApiController]
    [ApiVersion(ApiVersions.V1)]
    [Route( "api/v{version:apiVersion}/[controller]" )]
    [Route( "api/[controller]" )]
    public class ValuesController : ControllerBase
    {
        /// <summary>
        /// Get Values
        /// </summary>
        /// <response code="200" nullable="true">string</response>
        [HttpGet]
        [ProducesResponseType(typeof(string), 200)]
        [OpenApiTag("VersionedValues", Description = "Old operations")]
        public ActionResult<string> Get() => "Hello world!";
    }
}
