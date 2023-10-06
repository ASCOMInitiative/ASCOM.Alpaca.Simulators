using ASCOM.Common.Alpaca;
using ASCOM.Common.DeviceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace ASCOM.Alpaca
{
    [ServiceFilter(typeof(AuthorizationFilter))]
    [ApiController]
    [Route("api/v1/safetymonitor/")]
    public class SafetyMonitorController : AlpacaController
    {
        public const string APIRoot = "api/v1/safetymonitor/";

        [NonAction]
#if ASCOM_7_PREVIEW
        public override IAscomDeviceV2 GetDevice(uint DeviceNumber)
#else
        public override IAscomDevice GetDevice(uint DeviceNumber)
#endif
        {
            return DeviceManager.GetSafetyMonitor(DeviceNumber);
        }

        /// <summary>
        /// Indicates whether the monitored state is safe for use.
        /// </summary>
        /// <remarks>
        /// <para>Indicates whether the monitored state is safe for use. True if the state is safe, False if it is unsafe.</para>
        /// </remarks>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/issafe")]
        public ActionResult<BoolResponse> IsSafe(
            [Required][DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [SwaggerSchema(Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetSafetyMonitor(DeviceNumber).IsSafe, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }
    }
}