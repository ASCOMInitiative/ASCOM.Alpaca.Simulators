using Alpaca;
using ASCOM.Common.Alpaca;
using ASCOM.Common.DeviceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace ASCOM.Alpaca.Simulators
{
    [ServiceFilter(typeof(AuthorizationFilter))]
    [ApiController]
    [Route("api/v1/switch/")]
    public class SwitchController : AlpacaController
    {
        public const string APIRoot = "api/v1/switch/";

        [NonAction]
        public override IAscomDevice GetDevice(uint DeviceNumber)
        {
            return DeviceManager.GetSwitch(DeviceNumber);
        }

        /// <summary>
        /// The number of switch devices managed by this driver
        /// </summary>
        /// <remarks>
        /// <para>Returns the number of switch devices managed by this driver. Devices are numbered from 0 to MaxSwitch - 1</para>
        /// </remarks>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/maxswitch")]
        public ActionResult<IntResponse> MaxSwitch(
            [Required][DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [SwaggerSchema(Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).MaxSwitch, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        /// <summary>
        /// Indicates whether the specified switch device can be written to
        /// </summary>
        /// <remarks>
        /// <para>Reports if the specified switch device can be written to, default true. This is false if the device cannot be written to, for example a limit switch or a sensor. Devices are numbered from 0 to MaxSwitch - 1</para>
        /// </remarks>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="ID">The device number (0 to MaxSwitch - 1)</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/canwrite")]
        public ActionResult<BoolResponse> CanWrite(
            [Required][DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [Required][DefaultValue(0)] short ID,
            [SwaggerSchema(Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).CanWrite(ID), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        /// <summary>
        /// Return the state of switch device id as a boolean
        /// </summary>
        /// <remarks>
        /// <para>Return the state of switch device id as a boolean. Devices are numbered from 0 to MaxSwitch - 1</para>
        /// </remarks>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="ID">The device number (0 to MaxSwitch - 1)</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/getswitch")]
        public ActionResult<BoolResponse> GetSwitch(
            [Required][DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [Required][DefaultValue(0)] short ID,
            [SwaggerSchema(Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).GetSwitch(ID), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        /// <summary>
        /// Gets the description of the specified switch device
        /// </summary>
        /// <remarks>
        /// <para>Gets the description of the specified switch device. This is to allow a fuller description of the device to be returned, for example for a tool tip. Devices are numbered from 0 to MaxSwitch - 1</para>
        /// </remarks>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="ID">The device number (0 to MaxSwitch - 1)</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/getswitchdescription")]
        public ActionResult<StringResponse> GetSwitchDescription(
            [Required][DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [Required][DefaultValue(0)] short ID, [SwaggerSchema(Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).GetSwitchDescription(ID), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        /// <summary>
        /// Gets the name of the specified switch device
        /// </summary>
        /// <remarks>
        /// <para>Gets the name of the specified switch device. Devices are numbered from 0 to MaxSwitch - 1</para>
        /// </remarks>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="ID">The device number (0 to MaxSwitch - 1)</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/getswitchname")]
        public ActionResult<StringResponse> GetSwitchName(
            [Required][DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [Required][DefaultValue(0)] short ID,
            [SwaggerSchema(Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).GetSwitchName(ID), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        /// <summary>
        /// Gets the value of the specified switch device as a double
        /// </summary>
        /// <remarks>
        /// <para>Gets the value of the specified switch device as a double. Devices are numbered from 0 to MaxSwitch - 1, The value of this switch is expected to be between MinSwitchValue and MaxSwitchValue.</para>
        /// </remarks>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="ID">The device number (0 to MaxSwitch - 1)</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/getswitchvalue")]
        public ActionResult<DoubleResponse> GetSwitchValue(
            [Required][DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [Required][DefaultValue(0)] short ID,
            [SwaggerSchema(Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).GetSwitchValue(ID), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        /// <summary>
        /// Gets the minimum value of the specified switch device as a double
        /// </summary>
        /// <remarks>
        /// <para>Gets the minimum value of the specified switch device as a double. Devices are numbered from 0 to MaxSwitch - 1.</para>
        /// </remarks>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="ID">The device number (0 to MaxSwitch - 1)</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/minswitchvalue")]
        public ActionResult<DoubleResponse> MinSwitchValue(
            [Required][DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [Required][DefaultValue(0)] short ID,
            [SwaggerSchema(Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).MinSwitchValue(ID), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        /// <summary>
        /// Gets the maximum value of the specified switch device as a double
        /// </summary>
        /// <remarks>
        /// <para>Gets the maximum value of the specified switch device as a double. Devices are numbered from 0 to MaxSwitch - 1.</para>
        /// </remarks>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="ID">The device number (0 to MaxSwitch - 1)</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/maxswitchvalue")]
        public ActionResult<DoubleResponse> MaxSwitchValue(
            [Required][DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [Required][DefaultValue(0)] short ID, [SwaggerSchema(Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).MaxSwitchValue(ID), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        /// <summary>
        /// Returns the step size that this device supports (the difference between successive values of the device).
        /// </summary>
        /// <remarks>
        /// <para>Returns the step size that this device supports (the difference between successive values of the device). Devices are numbered from 0 to MaxSwitch - 1.</para>
        /// </remarks>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="ID">The device number (0 to MaxSwitch - 1)</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/switchstep")]
        public ActionResult<DoubleResponse> SwitchStep(
            [Required][DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [Required][DefaultValue(0)] short ID,
            [SwaggerSchema(Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).SwitchStep(ID), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        /// <summary>
        /// Sets a switch controller device to the specified state, true or false
        /// </summary>
        /// <remarks>
        /// <para>Sets a switch controller device to the specified state, true or false.</para>
        /// </remarks>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="ID">The device number (0 to MaxSwitch - 1)</param>
        /// <param name="State">The required control state(True or False)</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/setswitch")]
        public ActionResult<Response> SetSwitch(
            [Required][DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [FromForm][Required][DefaultValue(0)][SwaggerSchema("The device number (0 to MaxSwitch - 1)")] short ID,
            [FromForm][Required][SwaggerSchema("The required control state(True or False)")] bool State, [FromForm][SwaggerSchema(Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [FromForm][SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).SetSwitch(ID, State), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        /// <summary>
        /// Sets a switch device name to the specified value
        /// </summary>
        /// <remarks>
        /// <para>Sets a switch device name to the specified value.</para>
        /// </remarks>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="ID">The device number (0 to MaxSwitch - 1)</param>
        /// <param name="Name">The name of the device</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/setswitchname")]
        public ActionResult<Response> SetSwitchName(
            [Required][DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [FromForm][Required][DefaultValue(0)][SwaggerSchema("The device number (0 to MaxSwitch - 1)")] short ID,
            [Required][FromForm][SwaggerSchema("The name of the device")] string Name, [FromForm][SwaggerSchema(Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [FromForm][SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).SetSwitchName(ID, Name), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        /// <summary>
        /// Sets a switch device value to the specified value
        /// </summary>
        /// <remarks>
        /// <para>Sets a switch device value to the specified value.</para>
        /// </remarks>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="ID">The device number (0 to MaxSwitch - 1)</param>
        /// <param name="Value ">The value to be set, between MinSwitchValue and MaxSwitchValue</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/setswitchvalue")]
        public ActionResult<Response> SetSwitchValue(
            [Required][DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [FromForm][Required][DefaultValue(0)][SwaggerSchema("The device number (0 to MaxSwitch - 1)")] short ID,
            [Required][FromForm][SwaggerSchema("The value to be set, between MinSwitchValue and MaxSwitchValue")] double Value, [FromForm][SwaggerSchema(Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [FromForm][SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).SetSwitchValue(ID, Value), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }
    }
}