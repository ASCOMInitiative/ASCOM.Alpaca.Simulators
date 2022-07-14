using ASCOM.Common.Alpaca;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace ASCOM.Alpaca.Simulators
{
    /// <summary>
    /// OmniSim only, not part of Alpaca
    /// </summary>
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

        private ASCOM.Simulators.FilterWheel FilterWheelAccess(uint InstanceID)
        {
            return DeviceManager.GetFilterWheel((uint)InstanceID) as ASCOM.Simulators.FilterWheel;
        }
        
        private ASCOM.Simulators.Focuser FocuserAccess(uint InstanceID)
        {
            return DeviceManager.GetFocuser((uint)InstanceID) as ASCOM.Simulators.Focuser;
        }

        private ASCOM.Simulators.ObservingConditions ObservingConditionsAccess(uint InstanceID)
        {
            return DeviceManager.GetObservingConditions((uint)InstanceID) as ASCOM.Simulators.ObservingConditions;
        }

        private ASCOM.Simulators.SafetyMonitor SafetyMonitorAccess(uint InstanceID)
        {
            return DeviceManager.GetSafetyMonitor((uint)InstanceID) as ASCOM.Simulators.SafetyMonitor;
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
        [Route("filterwheel/{DeviceNumber}/reset")]
        public ActionResult<Response> ResetFilterWheel(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {

            return ProcessRequest(() =>
            {
                ASCOM.Simulators.FilterWheelHardware.ResetProfile();
                ASCOM.Simulators.FilterWheelHardware.Initialize();
            },
            DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Reseting FilterWheel to default settings.");
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
        [Route("focuser/{DeviceNumber}/reset")]
        public ActionResult<Response> ResetFocuser(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {


            return ProcessRequest(() =>
            {
                FocuserAccess(DeviceNumber).Reset();
            },
            DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Reseting Focuser to default settings.");
        }

        /// <summary>
        /// OmniSim only API - Resets a device settings to the simulator default ObservingConditions 
        /// </summary>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("observingconditions/{DeviceNumber}/reset")]
        public ActionResult<Response> ResetObservingConditions(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {


            return ProcessRequest(() =>
            {
                ASCOM.Simulators.OCSimulator.ClearProfile();
                ASCOM.Simulators.OCSimulator.Init();
            },
            DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Reseting Focuser to default settings.");
        }

        /// <summary>
        /// OmniSim only API - Resets a device settings to the simulator default ObservingConditions 
        /// </summary>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("rotator/{DeviceNumber}/reset")]
        public ActionResult<Response> ResetRotator(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {


            return ProcessRequest(() =>
            {
                ASCOM.Simulators.RotatorHardware.ResetProfile();
            },
            DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Reseting Focuser to default settings.");
        }

        /// <summary>
        /// OmniSim only API - Resets a device settings to the simulator default ObservingConditions 
        /// </summary>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("safetymonitor/{DeviceNumber}/reset")]
        public ActionResult<Response> ResetSafetyMonitor(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {


            return ProcessRequest(() =>
            {
                SafetyMonitorAccess(DeviceNumber).ResetProfile();
            },
            DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Reseting Focuser to default settings.");
        }
    }
}