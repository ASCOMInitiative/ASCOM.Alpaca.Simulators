using ASCOM.Common.Alpaca;
using ASCOM.Simulators;
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

        private ASCOM.Simulators.Rotator RotatorAccess(uint InstanceID)
        {
            return DeviceManager.GetRotator((uint)InstanceID) as ASCOM.Simulators.Rotator;
        }

        private ASCOM.Simulators.SafetyMonitor SafetyMonitorAccess(uint InstanceID)
        {
            return DeviceManager.GetSafetyMonitor((uint)InstanceID) as ASCOM.Simulators.SafetyMonitor;
        }

        private ASCOM.Simulators.Switch SwitchAccess(uint InstanceID)
        {
            return DeviceManager.GetSwitch((uint)InstanceID) as ASCOM.Simulators.Switch;
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
                CameraAccess(DeviceNumber).ResetSettings();
                CameraAccess(DeviceNumber).InitialiseSimulator();
            },
            DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Reseting Camera {DeviceNumber} to default settings.");
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
            DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Reseting CoverCalibrator {DeviceNumber} to default settings.");
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
                DomeAccess(DeviceNumber).ResetSettings();
                DomeAccess(DeviceNumber).LoadConfig();
            },
            DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Reseting Dome {DeviceNumber} to default settings.");
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
                FilterWheelAccess(DeviceNumber).ResetSettings();
                ASCOM.Simulators.FilterWheelHardware.Initialize();
            },
            DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Reseting FilterWheel {DeviceNumber} to default settings.");
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
                FocuserAccess(DeviceNumber).ResetSettings();
            },
            DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Reseting Focuser {DeviceNumber} to default settings.");
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
        [Route("observingconditions/{DeviceNumber}/reset")]
        public ActionResult<Response> ResetObservingConditions(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() =>
            {
                ObservingConditionsAccess(DeviceNumber).ResetSettings();
                ASCOM.Simulators.OCSimulator.Init();
            },
            DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Reseting ObservingConditions {DeviceNumber} to default settings.");
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
        [Route("rotator/{DeviceNumber}/reset")]
        public ActionResult<Response> ResetRotator(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() =>
            {
                RotatorAccess(DeviceNumber).ResetSettings();
            },
            DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Reseting Rotator {DeviceNumber} to default settings.");
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
            DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Reseting SafetyMonitor {DeviceNumber} to default settings.");
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
        [Route("switch/{DeviceNumber}/reset")]
        public ActionResult<Response> ResetSwitch(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() =>
            {
                SwitchAccess(DeviceNumber).ResetProfile();
                SwitchAccess(DeviceNumber).ReadProfile();
            },
            DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Reseting Switch {DeviceNumber} to default settings.");
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
        [Route("telescope/{DeviceNumber}/reset")]
        public ActionResult<Response> ResetTelescope(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() =>
            {
                ASCOM.Simulators.TelescopeHardware.ClearProfile();
                ASCOM.Simulators.TelescopeHardware.Init();
            },
            DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Reseting Telescope {DeviceNumber} to default settings.");
        }

        #region Restart devices
        /// <summary>
        /// OmniSim only API - Restarts a device simulator to the simulator stored settings and a clean state. This can be used to restart a device so it behaves like the OmniSim server was just freshly started, without restarting the whole OmniSim.
        /// </summary>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("camera/{DeviceNumber}/restart")]
        public ActionResult<Response> RestartCamera(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() =>
            {
                //Only supports 1 camera right now, in the future use DeviceNumber instead.
                DeviceManager.LoadCamera(0);
            },
            DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Restarting Camera {DeviceNumber} to a clean state.");
        }

        /// <summary>
        /// OmniSim only API - Restarts a device simulator to the simulator stored settings and a clean state. This can be used to restart a device so it behaves like the OmniSim server was just freshly started, without restarting the whole OmniSim.
        /// </summary>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("covercalibrator/{DeviceNumber}/restart")]
        public ActionResult<Response> RestartCoverCalibrator(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() =>
            {
                //Only supports 1 right now, in the future use DeviceNumber instead.
                DeviceManager.LoadCoverCalibrator(0);
            },
            DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Restarting CoverCalibrator {DeviceNumber} to a clean state.");
        }

        /// <summary>
        /// OmniSim only API - Restarts a device simulator to the simulator stored settings and a clean state. This can be used to restart a device so it behaves like the OmniSim server was just freshly started, without restarting the whole OmniSim.
        /// </summary>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("dome/{DeviceNumber}/restart")]
        public ActionResult<Response> RestartDome(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() =>
            {
                //Only supports 1 right now, in the future use DeviceNumber instead.
                DeviceManager.LoadDome(0);
            },
            DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Restarting Dome {DeviceNumber} to a clean state.");
        }

        /// <summary>
        /// OmniSim only API - Restarts a device simulator to the simulator stored settings and a clean state. This can be used to restart a device so it behaves like the OmniSim server was just freshly started, without restarting the whole OmniSim.
        /// </summary>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("filterwheel/{DeviceNumber}/restart")]
        public ActionResult<Response> RestartFilterWheel(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() =>
            {
                //Only supports 1 right now, in the future use DeviceNumber instead.
                DeviceManager.LoadFilterWheel(0);
            },
            DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Restarting FilterWheel {DeviceNumber} to a clean state.");
        }

        /// <summary>
        /// OmniSim only API - Restarts a device simulator to the simulator stored settings and a clean state. This can be used to restart a device so it behaves like the OmniSim server was just freshly started, without restarting the whole OmniSim.
        /// </summary>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("focuser/{DeviceNumber}/restart")]
        public ActionResult<Response> RestartFocuser(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() =>
            {
                //Only supports 1 right now, in the future use DeviceNumber instead.
                DeviceManager.LoadFocuser(0);
            },
            DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Restarting Focuser {DeviceNumber} to a clean state.");
        }

        /// <summary>
        /// OmniSim only API - Restarts a device simulator to the simulator stored settings and a clean state. This can be used to restart a device so it behaves like the OmniSim server was just freshly started, without restarting the whole OmniSim.
        /// </summary>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("observingconditions/{DeviceNumber}/restart")]
        public ActionResult<Response> RestartObservingConditions(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() =>
            {
                //Only supports 1 right now, in the future use DeviceNumber instead.
                DeviceManager.LoadObservingConditions(0);
            },
            DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Restarting ObservingConditions {DeviceNumber} to a clean state.");
        }

        /// <summary>
        /// OmniSim only API - Restarts a device simulator to the simulator stored settings and a clean state. This can be used to restart a device so it behaves like the OmniSim server was just freshly started, without restarting the whole OmniSim.
        /// </summary>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("rotator/{DeviceNumber}/restart")]
        public ActionResult<Response> RestartRotator(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() =>
            {
                //Only supports 1 right now, in the future use DeviceNumber instead.
                DeviceManager.LoadRotator(0);
            },
            DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Restarting Rotator {DeviceNumber} to a clean state.");
        }

        /// <summary>
        /// OmniSim only API - Restarts a device simulator to the simulator stored settings and a clean state. This can be used to restart a device so it behaves like the OmniSim server was just freshly started, without restarting the whole OmniSim.
        /// </summary>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("safetymonitor/{DeviceNumber}/restart")]
        public ActionResult<Response> RestartSafetyMonitor(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() =>
            {
                //Only supports 1 right now, in the future use DeviceNumber instead.
                DeviceManager.LoadSafetyMonitor(0);
            },
            DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Restarting SafetyMonitor {DeviceNumber} to a clean state.");
        }

        /// <summary>
        /// OmniSim only API - Restarts a device simulator to the simulator stored settings and a clean state. This can be used to restart a device so it behaves like the OmniSim server was just freshly started, without restarting the whole OmniSim.
        /// </summary>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("switch/{DeviceNumber}/restart")]
        public ActionResult<Response> RestartSwitch(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() =>
            {
                //Only supports 1 right now, in the future use DeviceNumber instead.
                DeviceManager.LoadSwitch(0);
            },
            DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Restarting Switch {DeviceNumber} to a clean state.");
        }

        /// <summary>
        /// OmniSim only API - Restarts a device simulator to the simulator stored settings and a clean state. This can be used to restart a device so it behaves like the OmniSim server was just freshly started, without restarting the whole OmniSim.
        /// </summary>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <param name="ClientID">Client's unique ID.</param>
        /// <param name="ClientTransactionID">Client's transaction ID.</param>
        /// <response code="200">Transaction complete or exception</response>
        /// <response code="400" examples="Error message describing why the command cannot be processed">Method or parameter value error, check error message</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("telescope/{DeviceNumber}/restart")]
        public ActionResult<Response> RestartTelescope(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() =>
            {
                //Only supports 1 right now, in the future use DeviceNumber instead.
                DeviceManager.LoadTelescope(0);
            },
            DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Restarting Telescope {DeviceNumber} to a clean state.");
        }
        #endregion

        #region XMLProfile
        /// <summary>
        /// Gets a copy of the profile. This is returned as a string value in the standard Response Value field.
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
        [Route("camera/{DeviceNumber}/xmlprofile")]
        public ActionResult<StringResponse> GetXMLProfileCamera(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => (CameraAccess(DeviceNumber) as ISimulation).GetXMLProfile(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        /// <summary>
        /// Gets a copy of the profile. This is returned as a string value in the standard Response Value field.
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
        [Route("covercalibrator/{DeviceNumber}/xmlprofile")]
        public ActionResult<StringResponse> GetXMLCoverCalibrator(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => (CoverCalibratorAccess(DeviceNumber) as ISimulation).GetXMLProfile(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        /// <summary>
        /// Gets a copy of the profile. This is returned as a string value in the standard Response Value field.
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
        [Route("dome/{DeviceNumber}/xmlprofile")]
        public ActionResult<StringResponse> GetXMLDome(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => (DomeAccess(DeviceNumber) as ISimulation).GetXMLProfile(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        /// <summary>
        /// Gets a copy of the profile. This is returned as a string value in the standard Response Value field.
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
        [Route("filterwheel/{DeviceNumber}/xmlprofile")]
        public ActionResult<StringResponse> GetXMLFilterWheel(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => (FilterWheelAccess(DeviceNumber) as ISimulation).GetXMLProfile(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        /// <summary>
        /// Gets a copy of the profile. This is returned as a string value in the standard Response Value field.
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
        [Route("focuser/{DeviceNumber}/xmlprofile")]
        public ActionResult<StringResponse> GetXMLFocuser(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => (FocuserAccess(DeviceNumber) as ISimulation).GetXMLProfile(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        /// <summary>
        /// Gets a copy of the profile. This is returned as a string value in the standard Response Value field.
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
        [Route("observingconditions/{DeviceNumber}/xmlprofile")]
        public ActionResult<StringResponse> GetXMLObservingConditions(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => (ObservingConditionsAccess(DeviceNumber) as ISimulation).GetXMLProfile(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        /// <summary>
        /// Gets a copy of the profile. This is returned as a string value in the standard Response Value field.
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
        [Route("rotator/{DeviceNumber}/xmlprofile")]
        public ActionResult<StringResponse> GetXMLRotator(
            [DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber,
            [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => (RotatorAccess(DeviceNumber) as ISimulation).GetXMLProfile(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }
        #endregion
    }
}