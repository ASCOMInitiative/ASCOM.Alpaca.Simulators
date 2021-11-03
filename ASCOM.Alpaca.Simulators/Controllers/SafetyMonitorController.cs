using Alpaca;
using ASCOM.Common.Alpaca;
using ASCOM.Common.DeviceInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace ASCOM.Alpaca.Simulators
{
    [ServiceFilter(typeof(AuthorizationFilter))]
    [ApiController]
    [Route("api/v1/safetymonitor/")]
    public class SafetyMonitorController : AlpacaController
    {
        public const string APIRoot = "api/v1/safetymonitor/";

        [NonAction]
        public override IAscomDevice GetDevice(int DeviceNumber)
        {
            return DeviceManager.GetSafetyMonitor(DeviceNumber);
        }

        [HttpGet]
        [Route("{DeviceNumber}/issafe")]
        public ActionResult<BoolResponse> IsSafe([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)]uint ClientID = 0, 
 [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetSafetyMonitor(DeviceNumber).IsSafe, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }
    }
}