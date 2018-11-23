using System.Reflection;

using Microsoft.AspNetCore.Mvc;


namespace WebAppSwagger.Controllers
{
    [Route("/app")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        private readonly string _version;


        public VersionController()
        {
            _version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }


        [HttpGet("version")]
        [ProducesResponseType(typeof(string), 200)]
        public ActionResult<string> Get()
        {
            return _version;
        }
    }
}
