using Alpaca;
using ASCOM.Alpaca.Responses;
using ASCOM.Standard.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ASCOM.Alpaca.Simulators
{
    [ServiceFilter(typeof(AuthorizationFilter))]
    [ApiController]
    [Route("api/v1/safetymonitor/")]
    public class SafetyMonitorController : AlpacaController
    {
        public new const string APIRoot = "api/v1/safetymonitor/";

        [NonAction]
        public override IAscomDevice GetDevice(int DeviceNumber)
        {
            return DeviceManager.GetSafetyMonitor(DeviceNumber);
        }

        [HttpGet]
        [Route("{DeviceNumber}/issafe")]
        public ActionResult<BoolResponse> IsSafe([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetSafetyMonitor(DeviceNumber).IsSafe, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }
    }
}