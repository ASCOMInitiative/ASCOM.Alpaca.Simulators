using ASCOM.Alpaca.Discovery;
using ASCOM.Alpaca.Razor;
using ASCOM.Common.Alpaca;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace ASCOM.Alpaca
{
    [ServiceFilter(typeof(AuthorizationFilter))]
    [ApiExplorerSettings(GroupName = "Alpaca")]
    [ApiController]
    public class ManagementController : Controller
    {
        /// <summary>
        /// Supported Alpaca API versions
        /// </summary>
        /// <remarks>
        /// <para>Returns an integer array of supported Alpaca API version numbers.</para>
        /// </remarks>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [AllowAnonymous]
        [Route("management/apiversions")]
        public IntListResponse ApiVersions(
            [SwaggerSchema(Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            var TransactionID = DeviceManager.ServerTransactionID;
            Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
            return new IntListResponse(ClientTransactionID, TransactionID, new int[] {1});
        }

        /// <summary>
        /// Summary information about this device as a whole
        /// </summary>
        /// <remarks>
        /// <para>Summary information about each available ASCOM device</para>
        /// </remarks>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [AllowAnonymous]
        [Route("management/v1/description")]
        public AlpacaDescriptionResponse Description(
            [SwaggerSchema(Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            var TransactionID = DeviceManager.ServerTransactionID;
            Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);

            return new AlpacaDescriptionResponse(ClientTransactionID, TransactionID, new AlpacaDeviceDescription(DeviceManager.Configuration.ServerName, DeviceManager.Configuration.Manufacturer, DeviceManager.Configuration.ServerVersion, DeviceManager.Configuration.Location));
        }

        /// <summary>
        /// Summary information about each available ASCOM device
        /// </summary>
        /// <remarks>
        /// <para>Returns an array of device description objects, providing unique information for each served device, enabling them to be accessed through the Alpaca Device API.</para>
        /// </remarks>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpGet]
        [AllowAnonymous]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("management/v1/configureddevices")]
        public AlpacaConfiguredDevicesResponse ConfiguredDevices(
            [SwaggerSchema(Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            var TransactionID = DeviceManager.ServerTransactionID;
            Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);

            return new AlpacaConfiguredDevicesResponse(ClientTransactionID, TransactionID, DeviceManager.GetDevices());
        }
    }
}