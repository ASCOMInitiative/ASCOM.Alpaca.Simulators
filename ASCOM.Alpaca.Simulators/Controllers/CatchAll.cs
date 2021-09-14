using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ASCOM.Alpaca.Simulators
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
            return BadRequest("Alpaca API route was not found on this driver");
        }

        [HttpPut]
        [Route("{**catchAll}")]
        public IActionResult CatchAllApiPuts()
        {
            return BadRequest("Alpaca API route was not found on this driver");
        }
    }
}