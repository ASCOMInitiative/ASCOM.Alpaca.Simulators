using ASCOM.Alpaca.Simulators;
using ASCOM.Common.Alpaca;
using ASCOM.Common.DeviceInterfaces;
using ASCOM.Common.Helpers;
using ASCOM.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mime;

namespace Alpaca
{
    /// <summary>
    /// A Custom REST MVC controller with additions to help handle Alpaca responses and error responses.
    /// This contains the Alpaca endpoints that are common to all devices.
    /// </summary>
    public abstract class AlpacaController : Controller
    {
        public abstract IAscomDevice GetDevice(uint DeviceNumber);

        #region Common Methods

        /// <summary>
        /// Invokes the specified device-specific action.
        /// </summary>
        /// <remarks>
        /// <para>Actions and SupportedActions are a standardised means for drivers to extend functionality beyond the built-in capabilities of the ASCOM device interfaces.</para>
        /// <br/>
        /// <para>The key advantage of using Actions is that drivers can expose any device specific functionality required. The downside is that, in order to use these unique features, every application author would need to create bespoke code to present or exploit them.</para>
        /// <br/>
        /// <para>The Action parameter and return strings are deceptively simple, but can support transmission of arbitrarily complex data structures, for example through JSON encoding.</para>
        /// <br/>
        /// <para>This capability will be of primary value to</para>
        /// <br/>
        /// <para>  * bespoke software and hardware configurations where a single entity controls both the consuming application software and the hardware / driver environment</para>
        /// <para>  * a group of application and device authors to quickly formulate and try out new interface capabilities without requiring an immediate change to the ASCOM device interface, which will take a lot longer than just agreeing a name, input parameters and a standard response for an Action command.</para>
        /// <br/>
        /// <para>The list of Action commands supported by a driver can be discovered through the SupportedActions property.</para>
        /// <br/>
        /// <para>This method should return an error message and NotImplementedException error number (0x400) if the driver just implements the standard ASCOM device methods and has no bespoke, unique, functionality.</para>
        /// </remarks>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="Action">A well known name that represents the action to be carried out.</param>
        /// <param name="Parameters">List of required parameters or an Empty String if none are required</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/action")]
        public ActionResult<StringResponse> Action(
            [Required][DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [Required][FromForm][SwaggerSchema("A well known name that represents the action to be carried out.")] string Action,
            [FromForm][SwaggerSchema("List of required parameters or an Empty String if none are required.")] string Parameters = "",
            [FromForm][SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [FromForm][SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => GetDevice(DeviceNumber).Action(Action, Parameters), DeviceManager.ServerTransactionID, (uint)ClientID, (uint)ClientTransactionID, $"Action: {Action}, Parameters {Parameters}");
        }

        /// <summary>
        /// Transmits an arbitrary string to the device
        /// </summary>
        /// <remarks>
        /// <para>Transmits an arbitrary string to the device and does not wait for a response. Optionally, protocol framing characters may be added to the string before transmission.</para>
        /// </remarks>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="Command">A well known name that represents the action to be carried out.</param>
        /// <param name="Raw">If set to true the string is transmitted 'as-is', if set to false then protocol framing characters may be added prior to transmission.</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/commandblind")]
        public ActionResult<VoidResponse> CommandBlind(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [Required][FromForm][SwaggerSchema("The literal command string to be transmitted.")] string Command,
            [FromForm][SwaggerSchema("If set to true the string is transmitted 'as-is', if set to false then protocol framing characters may be added prior to transmission.")] bool Raw = false,
            [FromForm][SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [FromForm][SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => GetDevice(DeviceNumber).CommandBlind(Command, Raw), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Command {Command}, Raw {Raw}");
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and returns a boolean value from the device.
        /// </summary>
        /// <remarks>
        /// <para>Transmits an arbitrary string to the device and waits for a boolean response. Optionally, protocol framing characters may be added to the string before transmission.</para>
        /// </remarks>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="Command">A well known name that represents the action to be carried out.</param>
        /// <param name="Raw">If set to true the string is transmitted 'as-is', if set to false then protocol framing characters may be added prior to transmission.</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/commandbool")]
        public ActionResult<BoolResponse> CommandBool(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [Required][FromForm][SwaggerSchema("The literal command string to be transmitted.")] string Command,
            [FromForm][SwaggerSchema("If set to true the string is transmitted 'as-is', if set to false then protocol framing characters may be added prior to transmission.")] bool Raw = false,
            [FromForm][SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [FromForm][SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => GetDevice(DeviceNumber).CommandBool(Command, Raw), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Command={Command}, Raw={Raw}");
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and returns a string value from the device.
        /// </summary>
        /// <remarks>
        /// <para>Transmits an arbitrary string to the device and waits for a string response. Optionally, protocol framing characters may be added to the string before transmission.</para>
        /// </remarks>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="Command">A well known name that represents the action to be carried out.</param>
        /// <param name="Raw">If set to true the string is transmitted 'as-is', if set to false then protocol framing characters may be added prior to transmission.</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/commandstring")]
        public ActionResult<StringResponse> CommandString(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [Required][FromForm] string Command,
            [FromForm] bool Raw = false,
            [FromForm][SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [FromForm][SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => GetDevice(DeviceNumber).CommandString(Command, Raw), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Command={Command}, Raw={Raw}");
        }

        /// <summary>
        /// Retrieves the connected state of the device
        /// </summary>
        /// <remarks>
        /// <para>Retrieves the connected state of the device</para>
        /// </remarks>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/connected")]
        public ActionResult<BoolResponse> Connected(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => GetDevice(DeviceNumber).Connected, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        /// <summary>
        /// Sets the connected state of the device
        /// </summary>
        /// <remarks>
        /// <para>Sets the connected state of the device</para>
        /// </remarks>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="Connected">Set True to connect to the device hardware, set False to disconnect from the device hardware.</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/connected")]
        public ActionResult<VoidResponse> Connected(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [Required][FromForm][SwaggerSchema("Set True to connect to the device hardware, set False to disconnect from the device hardware.")] bool Connected,
            [FromForm][SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [FromForm][SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            if (Connected || !ServerSettings.PreventRemoteDisconnects)
            {
                return ProcessRequest(() => { GetDevice(DeviceNumber).Connected = Connected; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Connected={Connected}");
            }
            return ProcessRequest(() => { }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        /// <summary>
        /// Device description
        /// </summary>
        /// <remarks>
        /// <para>The description of the device</para>
        /// </remarks>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/description")]
        public ActionResult<StringResponse> Description(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => GetDevice(DeviceNumber).Description, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        /// <summary>
        /// Device driver description
        /// </summary>
        /// <remarks>
        /// <para>The description of the driver</para>
        /// </remarks>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/driverinfo")]
        public ActionResult<StringResponse> DriverInfo(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => GetDevice(DeviceNumber).DriverInfo, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        /// <summary>
        /// Driver Version
        /// </summary>
        /// <remarks>
        /// <para>A string containing only the major and minor version of the driver.</para>
        /// </remarks>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/driverversion")]
        public ActionResult<StringResponse> DriverVersion(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => GetDevice(DeviceNumber).DriverVersion, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        /// <summary>
        /// The ASCOM Device interface version number that this device supports.
        /// </summary>
        /// <remarks>
        /// <para>This method returns the version of the ASCOM device interface contract to which this device complies. Only one interface version is current at a moment in time and all new devices should be built to the latest interface version. Applications can choose which device interface versions they support and it is in their interest to support previous versions as well as the current version to ensure they can use the largest number of devices.</para>
        /// </remarks>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/interfaceversion")]
        public ActionResult<IntResponse> InterfaceVersion(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => GetDevice(DeviceNumber).InterfaceVersion, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        /// <summary>
        /// Device name
        /// </summary>
        /// <remarks>
        /// <para>The name of the device</para>
        /// </remarks>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/name")]
        public ActionResult<StringResponse> Name(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => GetDevice(DeviceNumber).Name, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        /// <summary>
        /// Returns the list of action names supported by this driver.
        /// </summary>
        /// <remarks>
        /// <para>Returns the list of action names supported by this driver.</para>
        /// </remarks>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/supportedactions")]
        public ActionResult<StringListResponse> SupportedActions(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => GetDevice(DeviceNumber).SupportedActions, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        #region IDisposable Members

        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/dispose")]
        public ActionResult<VoidResponse> Dispose(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [FromForm][SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [FromForm][SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            if (!ServerSettings.PreventRemoteDisposes)
            {
                return ProcessRequest(() => { GetDevice(DeviceNumber).Dispose(); }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
            }
            return ProcessRequest(() => { }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        #endregion IDisposable Members

        #endregion Common Methods

        /// <summary>
        /// This function logs the incoming API call then executes the passed function
        /// By executing the function this can catch any errors
        /// If it completes successfully a bool response is returned with an http 200
        /// If no device is available an HTTP 400 is returned
        /// If the device call fails an Alpaca JSON error is returned
        /// </summary>
        /// <param name="operation">The operation to preform on the device. Often this is just a lambda that returns a property. By passing it in as a function it can be executed inside a try catch and handle the exception.</param>
        /// <param name="TransactionID">The current server transaction id</param>
        /// <param name="ClientID">The client id</param>
        /// <param name="ClientTransactionID">The client transaction id</param>
        /// <param name="payload">Any payload values, optional, only used for logging</param>
        /// <returns></returns>
        internal ActionResult<BoolResponse> ProcessRequest(Func<bool> operation, uint TransactionID, uint ClientID, uint ClientTransactionID, string payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, payload);

                return Ok(new BoolResponse()
                {
                    ClientTransactionID = ClientTransactionID,
                    ServerTransactionID = TransactionID,
                    Value = operation.Invoke()
                });
            }
            catch (DeviceNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Ok(ResponseHelpers.ExceptionResponseBuilder<BoolResponse>(ex, ClientTransactionID, TransactionID));
            }
        }

        internal ActionResult<DriveRatesResponse> ProcessRequest(Func<ITrackingRates> p, uint TransactionID, [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
, string payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, payload);

                var rates = p.Invoke();

                IList<DriveRate> res = new List<DriveRate>();

                foreach (DriveRate rate in rates)
                {
                    res.Add(rate);
                }

                return Ok(new DriveRatesResponse()
                {
                    ClientTransactionID = ClientTransactionID,
                    ServerTransactionID = TransactionID,
                    Value = res
                });
            }
            catch (DeviceNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Ok(ResponseHelpers.ExceptionResponseBuilder<DriveRatesResponse>(ex, ClientTransactionID, TransactionID));
            }
        }

        internal ActionResult<DoubleResponse> ProcessRequest(Func<double> p, uint TransactionID, [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
, string payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, payload);

                return Ok(new DoubleResponse()
                {
                    ClientTransactionID = ClientTransactionID,
                    ServerTransactionID = TransactionID,
                    Value = p.Invoke()
                });
            }
            catch (DeviceNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Ok(ResponseHelpers.ExceptionResponseBuilder<DoubleResponse>(ex, ClientTransactionID, TransactionID));
            }
        }

        internal ActionResult<AxisRatesResponse> ProcessRequest(Func<IAxisRates> p, uint TransactionID, [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
, string payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, payload);

                var rates = p.Invoke();

                IList<AxisRate> res = new List<AxisRate>();

                foreach (IRate rate in rates)
                {
                    res.Add(new AxisRate(rate.Minimum, rate.Maximum));
                }

                return Ok(new AxisRatesResponse()
                {
                    ClientTransactionID = ClientTransactionID,
                    ServerTransactionID = TransactionID,
                    Value = res
                });
            }
            catch (DeviceNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Ok(ResponseHelpers.ExceptionResponseBuilder<AxisRatesResponse>(ex, ClientTransactionID, TransactionID));
            }
        }

        internal ActionResult<IntArray2DResponse> ProcessRequest(Func<int[,]> p, uint TransactionID, [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
, string payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, payload);

                return Ok(new IntArray2DResponse()
                {
                    ClientTransactionID = ClientTransactionID,
                    ServerTransactionID = TransactionID,
                    Value = p.Invoke()
                });
            }
            catch (DeviceNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Ok(ResponseHelpers.ExceptionResponseBuilder<IntArray2DResponse>(ex, ClientTransactionID, TransactionID));
            }
        }

        internal ActionResult<StringListResponse> ProcessRequest(Func<IList<string>> p, uint TransactionID, [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
, string payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, payload);

                return Ok(new StringListResponse()
                {
                    ClientTransactionID = ClientTransactionID,
                    ServerTransactionID = TransactionID,
                    Value = p.Invoke()
                });
            }
            catch (DeviceNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Ok(ResponseHelpers.ExceptionResponseBuilder<StringListResponse>(ex, ClientTransactionID, TransactionID));
            }
        }

        internal ActionResult<IntListResponse> ProcessRequest(Func<IList<int>> p, uint TransactionID, [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
, string payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, payload);

                return Ok(new IntListResponse()
                {
                    ClientTransactionID = ClientTransactionID,
                    ServerTransactionID = TransactionID,
                    Value = p.Invoke()
                });
            }
            catch (DeviceNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Ok(ResponseHelpers.ExceptionResponseBuilder<IntListResponse>(ex, ClientTransactionID, TransactionID));
            }
        }

        internal ActionResult<IntResponse> ProcessRequest(Func<int> p, uint TransactionID, [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
, string payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, payload);

                return Ok(new IntResponse()
                {
                    ClientTransactionID = ClientTransactionID,
                    ServerTransactionID = TransactionID,
                    Value = p.Invoke()
                });
            }
            catch (DeviceNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Ok(ResponseHelpers.ExceptionResponseBuilder<IntResponse>(ex, ClientTransactionID, TransactionID));
            }
        }

        internal ActionResult<StringResponse> ProcessRequest(Func<string> p, uint TransactionID, [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
, string payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, payload);

                return Ok(new StringResponse()
                {
                    ClientTransactionID = ClientTransactionID,
                    ServerTransactionID = TransactionID,
                    Value = p.Invoke()
                });
            }
            catch (DeviceNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Ok(ResponseHelpers.ExceptionResponseBuilder<StringResponse>(ex, ClientTransactionID, TransactionID));
            }
        }

        internal ActionResult<VoidResponse> ProcessRequest(Action p, uint TransactionID, [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
, string payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, payload);
                p.Invoke();
                return Ok(new Response() { ClientTransactionID = ClientTransactionID, ServerTransactionID = TransactionID });
            }
            catch (DeviceNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Ok(ResponseHelpers.ExceptionResponseBuilder<Response>(ex, ClientTransactionID, TransactionID));
            }
        }

        /// <summary>
        /// Log out an API request to the ASCOM Standard Logger Instance. This logs at a level of Verbose.
        /// </summary>
        /// <param name="remoteIpAddress">The IP Address of the remote computer</param>
        /// <param name="request">The requested API</param>
        /// <param name="clientID">The Client ID</param>
        /// <param name="clientTransactionID">The Client Transaction ID</param>
        /// <param name="transactionID">The Server Transaction ID</param>
        /// <param name="payload">The function payload if any exists</param>
        private static void LogAPICall(IPAddress remoteIpAddress, string request, uint clientID, uint clientTransactionID, uint transactionID, string payload = "")
        {
            if (payload == null || payload == string.Empty)
            {
                Logger.LogVerbose($"Transaction: {transactionID} - {remoteIpAddress} ({clientID}, {clientTransactionID}) requested {request}");
            }
            else
            {
                Logger.LogVerbose($"Transaction: {transactionID} - {remoteIpAddress} ({clientID}, {clientTransactionID}) requested {request} with payload {payload}");
            }
        }
    }

    internal class DeviceNotFoundException : Exception
    {
        internal DeviceNotFoundException(string message) : base(message)
        {
        }
    }
}