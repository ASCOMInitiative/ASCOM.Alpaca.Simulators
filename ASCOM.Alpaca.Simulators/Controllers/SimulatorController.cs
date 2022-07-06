using ASCOM.Common.Alpaca;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace ASCOM.Alpaca.Simulators
{
    [ServiceFilter(typeof(AuthorizationFilter))]
    [ApiController]
    [Route("simulator/v1/")]
    public class SimulatorController : ProcessBaseController
    {
        public const string APIRoot = "simulator/v1/";

        private ASCOM.Simulators.Camera CameraAccess(uint InstanceID)
        {
                return DeviceManager.GetCamera((uint)InstanceID) as ASCOM.Simulators.Camera;
        }

        private ASCOM.Simulators.CoverCalibratorSimulator CoverCalibratorAccess(uint InstanceID)
        {
            return DeviceManager.GetCoverCalibrator((uint)InstanceID) as ASCOM.Simulators.CoverCalibratorSimulator;
        }

        private ASCOM.Simulators.Dome DomeAccess(uint InstanceID)
        {
            return DeviceManager.GetDome((uint)InstanceID) as ASCOM.Simulators.Dome;
        }

        /// <summary>
        /// OmniSim only API - Resets a device settings to the simulator default
        /// </summary>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("camera/{DeviceNumber}/reset")]
        public ActionResult<Response> ResetCamera(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {

            return ProcessRequest(() =>
            {
                CameraAccess(DeviceNumber).ClearProfile();
                CameraAccess(DeviceNumber).InitialiseSimulator();
            },
            DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Reseting Camera to default settings.");
        }

        /// <summary>
        /// OmniSim only API - Resets a device settings to the simulator default
        /// </summary>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("covercalibrator/{DeviceNumber}/reset")]
        public ActionResult<Response> ResetCoverCalibrator(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {

            return ProcessRequest(() =>
            {
                CoverCalibratorAccess(DeviceNumber).ResetSettings();
            },
            DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Reseting CoverCalibrator to default settings.");
        }

        /// <summary>
        /// OmniSim only API - Resets a device settings to the simulator default
        /// </summary>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("dome/{DeviceNumber}/reset")]
        public ActionResult<Response> ResetDome(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {

            return ProcessRequest(() =>
            {
                DomeAccess(DeviceNumber).ResetConfig();
                DomeAccess(DeviceNumber).LoadConfig();
            },
            DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Reseting Dome to default settings.");
        }
    }
}