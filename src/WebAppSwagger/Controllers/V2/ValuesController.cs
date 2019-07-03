using Microsoft.AspNetCore.Mvc;

using WebAppSwagger.Models;


namespace WebAppSwagger.Controllers.V2
{
    [ApiVersion(ApiVersions.V2)]
    [Route( "api/v{version:apiVersion}/[controller]" )]
    [Route( "api/[controller]" )]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(string), 200)]
        public ActionResult<string> Get() => "Hello world!(v2)";
    }
}
