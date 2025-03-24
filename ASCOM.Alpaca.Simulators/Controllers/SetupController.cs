using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Net.Mime;
using Microsoft.FeatureManagement.Mvc;
using ASCOM.Common;
using Octokit;
using System.Xml;
using OmniSim.BaseDriver;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;

namespace ASCOM.Alpaca.Simulators.Controllers
{
    [ApiExplorerSettings(GroupName = "AlpacaSetup")]
    public class SetupController : Controller
    {
        /// <summary>
        /// Primary browser web page for the overall collection of devices and the Alpaca service
        /// </summary>
        /// <remarks>
        /// This is a single, well-known, API version-independent, browser interface that provides a consistent user experience and well known "new user" starting point for all Alpaca devices. The web page must describe the overall device, including name, manufacturer and version number.
        /// This page must also enable the user to set cross cutting information that applies to the whole device such as the operating IP address and port number.
        /// This URL could also provide links to configuration pages of available devices, at the developer's discretion.
        /// </remarks>
        /// <response code="200">Alpaca device primary HTML page</response>
        /// <response code="403" examples="Error message describing why the command cannot be processed">The provided path specifies an unsupported device or method, return an error message to display in the browser</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpGet]
        [Produces(MediaTypeNames.Text.Html)]
        [FeatureGate("HideAlpacaUI")]
        [Route("/setup")]
        public ActionResult<string> ServerSetup([Required][DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber)
        {
            return Ok(string.Empty);
        }

        /// <summary>
        /// Configuration web page for the specified device
        /// </summary>
        /// <remarks>
        /// <para>Web page user interface that enables device specific configuration to be set for each available device. This must be implemented, even if the response to the user is that the device is not configurable.
        /// User relevance is increased by providing a unique configuration page for each device.This enables deep linking directly to the device's individual configuration page and avoids displaying non-relevant information about other devices.</para>
        /// </remarks>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <response code="200">Alpaca individual ASCOM device HTML configuration page.This must be implemented, even if the response to the user is that the device is not configurable</response>
        /// <response code="403" examples="Error message describing why the command cannot be processed">The provided path specifies an unsupported device or method, return an error message to display in the browser</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpGet]
        [Produces(MediaTypeNames.Text.Html)]
        [FeatureGate("HideAlpacaUI")]
        [Route("/setup/v1/camera/{DeviceNumber}/setup")]
        public ActionResult<string> CameraSetup([Required][DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber)
        {
            return Ok(string.Empty);
        }

        /// <summary>
        /// Configuration web page for the specified device
        /// </summary>
        /// <remarks>
        /// <para>Web page user interface that enables device specific configuration to be set for each available device. This must be implemented, even if the response to the user is that the device is not configurable.
        /// User relevance is increased by providing a unique configuration page for each device.This enables deep linking directly to the device's individual configuration page and avoids displaying non-relevant information about other devices.</para>
        /// </remarks>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <response code="200">Alpaca individual ASCOM device HTML configuration page.This must be implemented, even if the response to the user is that the device is not configurable</response>
        /// <response code="403" examples="Error message describing why the command cannot be processed">The provided path specifies an unsupported device or method, return an error message to display in the browser</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpGet]
        [Produces(MediaTypeNames.Text.Html)]
        [FeatureGate("HideAlpacaUI")]
        [Route("/setup/v1/covercalibrator/{DeviceNumber}/setup")]
        public ActionResult<string> CoverCalibratorSetup([Required][DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber)
        {
            return Ok(string.Empty);
        }

        /// <summary>
        /// Configuration web page for the specified device
        /// </summary>
        /// <remarks>
        /// <para>Web page user interface that enables device specific configuration to be set for each available device. This must be implemented, even if the response to the user is that the device is not configurable.
        /// User relevance is increased by providing a unique configuration page for each device.This enables deep linking directly to the device's individual configuration page and avoids displaying non-relevant information about other devices.</para>
        /// </remarks>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <response code="200">Alpaca individual ASCOM device HTML configuration page.This must be implemented, even if the response to the user is that the device is not configurable</response>
        /// <response code="403" examples="Error message describing why the command cannot be processed">The provided path specifies an unsupported device or method, return an error message to display in the browser</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpGet]
        [Produces(MediaTypeNames.Text.Html)]
        [FeatureGate("HideAlpacaUI")]
        [Route("/setup/v1/dome/{DeviceNumber}/setup")]
        public ActionResult<string> DomeSetup([Required][DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber)
        {
            return Ok(string.Empty);
        }

        /// <summary>
        /// Configuration web page for the specified device
        /// </summary>
        /// <remarks>
        /// <para>Web page user interface that enables device specific configuration to be set for each available device. This must be implemented, even if the response to the user is that the device is not configurable.
        /// User relevance is increased by providing a unique configuration page for each device.This enables deep linking directly to the device's individual configuration page and avoids displaying non-relevant information about other devices.</para>
        /// </remarks>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <response code="200">Alpaca individual ASCOM device HTML configuration page.This must be implemented, even if the response to the user is that the device is not configurable</response>
        /// <response code="403" examples="Error message describing why the command cannot be processed">The provided path specifies an unsupported device or method, return an error message to display in the browser</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpGet]
        [Produces(MediaTypeNames.Text.Html)]
        [FeatureGate("HideAlpacaUI")]
        [Route("/setup/v1/filterwheel/{DeviceNumber}/setup")]
        public ActionResult<string> FilterWheelSetup([Required][DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber)
        {
            return Ok(string.Empty);
        }

        /// <summary>
        /// Configuration web page for the specified device
        /// </summary>
        /// <remarks>
        /// <para>Web page user interface that enables device specific configuration to be set for each available device. This must be implemented, even if the response to the user is that the device is not configurable.
        /// User relevance is increased by providing a unique configuration page for each device.This enables deep linking directly to the device's individual configuration page and avoids displaying non-relevant information about other devices.</para>
        /// </remarks>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <response code="200">Alpaca individual ASCOM device HTML configuration page.This must be implemented, even if the response to the user is that the device is not configurable</response>
        /// <response code="403" examples="Error message describing why the command cannot be processed">The provided path specifies an unsupported device or method, return an error message to display in the browser</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpGet]
        [Produces(MediaTypeNames.Text.Html)]
        [FeatureGate("HideAlpacaUI")]
        [Route("/setup/v1/focuser/{DeviceNumber}/setup")]
        public ActionResult<string> FocuserSetup([Required][DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber)
        {
            return Ok(string.Empty);
        }

        /// <summary>
        /// Configuration web page for the specified device
        /// </summary>
        /// <remarks>
        /// <para>Web page user interface that enables device specific configuration to be set for each available device. This must be implemented, even if the response to the user is that the device is not configurable.
        /// User relevance is increased by providing a unique configuration page for each device.This enables deep linking directly to the device's individual configuration page and avoids displaying non-relevant information about other devices.</para>
        /// </remarks>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <response code="200">Alpaca individual ASCOM device HTML configuration page.This must be implemented, even if the response to the user is that the device is not configurable</response>
        /// <response code="403" examples="Error message describing why the command cannot be processed">The provided path specifies an unsupported device or method, return an error message to display in the browser</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpGet]
        [Produces(MediaTypeNames.Text.Html)]
        [FeatureGate("HideAlpacaUI")]
        [Route("/setup/v1/observingconditions/{DeviceNumber}/setup")]
        public ActionResult<string> ObservingConditionsSetup([Required][DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber)
        {
            return Ok(string.Empty);
        }

        /// <summary>
        /// Configuration web page for the specified device
        /// </summary>
        /// <remarks>
        /// <para>Web page user interface that enables device specific configuration to be set for each available device. This must be implemented, even if the response to the user is that the device is not configurable.
        /// User relevance is increased by providing a unique configuration page for each device.This enables deep linking directly to the device's individual configuration page and avoids displaying non-relevant information about other devices.</para>
        /// </remarks>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <response code="200">Alpaca individual ASCOM device HTML configuration page.This must be implemented, even if the response to the user is that the device is not configurable</response>
        /// <response code="403" examples="Error message describing why the command cannot be processed">The provided path specifies an unsupported device or method, return an error message to display in the browser</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpGet]
        [Produces(MediaTypeNames.Text.Html)]
        [FeatureGate("HideAlpacaUI")]
        [Route("/setup/v1/rotator/{DeviceNumber}/setup")]
        public ActionResult<string> RotatorSetup([Required][DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber)
        {
            return Ok(string.Empty);
        }

        /// <summary>
        /// Configuration web page for the specified device
        /// </summary>
        /// <remarks>
        /// <para>Web page user interface that enables device specific configuration to be set for each available device. This must be implemented, even if the response to the user is that the device is not configurable.
        /// User relevance is increased by providing a unique configuration page for each device.This enables deep linking directly to the device's individual configuration page and avoids displaying non-relevant information about other devices.</para>
        /// </remarks>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <response code="200">Alpaca individual ASCOM device HTML configuration page.This must be implemented, even if the response to the user is that the device is not configurable</response>
        /// <response code="403" examples="Error message describing why the command cannot be processed">The provided path specifies an unsupported device or method, return an error message to display in the browser</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpGet]
        [Produces(MediaTypeNames.Text.Html)]
        [FeatureGate("HideAlpacaUI")]
        [Route("/setup/v1/safetymonitor/{DeviceNumber}/setup")]
        public ActionResult<string> SafetyMonitorSetup([Required][DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber)
        {
            return Ok(string.Empty);
        }

        /// <summary>
        /// Configuration web page for the specified device
        /// </summary>
        /// <remarks>
        /// <para>Web page user interface that enables device specific configuration to be set for each available device. This must be implemented, even if the response to the user is that the device is not configurable.
        /// User relevance is increased by providing a unique configuration page for each device.This enables deep linking directly to the device's individual configuration page and avoids displaying non-relevant information about other devices.</para>
        /// </remarks>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <response code="200">Alpaca individual ASCOM device HTML configuration page.This must be implemented, even if the response to the user is that the device is not configurable</response>
        /// <response code="403" examples="Error message describing why the command cannot be processed">The provided path specifies an unsupported device or method, return an error message to display in the browser</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpGet]
        [Produces(MediaTypeNames.Text.Html)]
        [FeatureGate("HideAlpacaUI")]
        [Route("/setup/v1/switch/{DeviceNumber}/setup")]
        public ActionResult<string> SwitchSetup([Required][DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber)
        {
            return Ok(string.Empty);
        }

        /// <summary>
        /// Configuration web page for the specified device
        /// </summary>
        /// <remarks>
        /// <para>Web page user interface that enables device specific configuration to be set for each available device. This must be implemented, even if the response to the user is that the device is not configurable.
        /// User relevance is increased by providing a unique configuration page for each device.This enables deep linking directly to the device's individual configuration page and avoids displaying non-relevant information about other devices.</para>
        /// </remarks>
        /// <param name="DeviceNumber">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>
        /// <response code="200">Alpaca individual ASCOM device HTML configuration page.This must be implemented, even if the response to the user is that the device is not configurable</response>
        /// <response code="403" examples="Error message describing why the command cannot be processed">The provided path specifies an unsupported device or method, return an error message to display in the browser</response>
        /// <response code="500" examples="Error message describing why the command cannot be processed">Server internal error, check error message</response>
        [HttpGet]
        [Produces(MediaTypeNames.Text.Html)]
        [FeatureGate("HideAlpacaUI")]
        [Route("/setup/v1/telescope/{DeviceNumber}/setup")]
        public ActionResult<string> TelescopeSetup([Required][DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = "uint32")][Range(0, 4294967295)] uint DeviceNumber)
        {
            return Ok(string.Empty);
        }
    }
}
