using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;



namespace WebAppSerilog.Controllers
{
    [ApiController]
    [Route( "api/v1/[controller]" )]
    [Route( "api/[controller]" )]
    public class ValuesController : ControllerBase
    {
        /// <summary>
        /// Get Values
        /// </summary>
        /// <response code="200" nullable="true">string</response>
        [HttpGet]
        [ProducesResponseType(typeof(string), 200)]
        public IActionResult Get([FromServices] ILogger<ValuesController> logger)
        {
            logger.LogInformation("LogInformation");
            logger.LogWarning("LogWarning");
            logger.LogError("LogError");
            logger.LogCritical("LogCritical");
            return Ok($"Hello world! {HttpContext.TraceIdentifier}");
        }
    }
}
