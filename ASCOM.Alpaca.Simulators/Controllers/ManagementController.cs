using ASCOM.Alpaca.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASCOM.Alpaca.Simulators
{
    [ServiceFilter(typeof(AuthorizationFilter))]
    [ApiController]
    public class ManagementController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("management/apiversions")]
        public IntListResponse ApiVersions(uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = DeviceManager.ServerTransactionID;
            Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
            return new IntListResponse(ClientTransactionID, TransactionID, ServerSettings.APIVersions);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("management/v1/description")]
        public AlpacaDescriptionResponse Description(uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = DeviceManager.ServerTransactionID;
            Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);

            return new AlpacaDescriptionResponse(ClientTransactionID, TransactionID, new AlpacaDeviceDescription(ServerSettings.ServerName, ServerSettings.Manufacturer, ServerSettings.Version, ServerSettings.Location));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("management/v1/configureddevices")]
        public AlpacaConfiguredDevicesResponse ConfiguredDevices(uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = DeviceManager.ServerTransactionID;
            Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);

            return new AlpacaConfiguredDevicesResponse(ClientTransactionID, TransactionID, DeviceManager.GetDevices());
        }
    }
}