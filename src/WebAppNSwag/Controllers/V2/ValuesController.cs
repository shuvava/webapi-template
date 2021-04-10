using Microsoft.AspNetCore.Mvc;

using NSwag.Annotations;

using WebAppNSwag.Models;


namespace WebAppNSwag.Controllers.V2
{
    [ApiController]
    [ApiVersion(ApiVersions.V2)]
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
        //[OpenApiTag("V3VersionedValues", Description = "New operations that should be only visible for version 3")]
        public ActionResult<string> Get() => "Hello world!(v2)";
    }
}
