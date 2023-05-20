using Microsoft.AspNetCore.Mvc;

namespace ASCOM.Alpaca
{
    /// <summary>
    /// This controller catches all requests on the /api that do not have a controller and return an HTTP 400.
    /// To have this not catch something create the route.
    /// </summary>
    [ServiceFilter(typeof(AuthorizationFilter))]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api", Order = 999)]
    public class CatchAll : Controller
    {
        [HttpGet]
        [Route("{**catchAll}")]
        public IActionResult CatchAllApiGets()
        {
            string error = $"Alpaca API route {HttpContext.Request.Path} was not found on this driver";
            Logging.LogError(error);
            return BadRequest(error);
        }

        [HttpPut]
        [Route("{**catchAll}")]
        public IActionResult CatchAllApiPuts()
        {
            string error = $"Alpaca API route {HttpContext.Request.Path} was not found on this driver";
            Logging.LogError(error);
            return BadRequest(error);
        }
    }
}