using ASCOM.Alpaca;
using ASCOM.Common.Alpaca;
using OmniSim.BaseDriver;
using System.Text;
using System.Text.Json;

namespace OmniSim.SettingsAPIGenerator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var filterwheelAPI = BuildSettingsAPI(typeof(ASCOM.Simulators.FilterWheelHardware), "FilterWheel", "(DeviceManager.GetFilterWheel(DeviceNumber) as ASCOM.Simulators.FilterWheel).FilterWheelHardware");

            using (StreamWriter writetext = File.CreateText("../../../../ASCOM.Alpaca.Simulators/Controllers/FilterWheelSettingsController.cs"))
            {
                writetext.Write(filterwheelAPI);
            }

            var focuserAPI = BuildSettingsAPI(typeof(ASCOM.Simulators.Focuser), "Focuser", "(DeviceManager.GetFocuser(DeviceNumber) as ASCOM.Simulators.Focuser)");

            using (StreamWriter writetext = File.CreateText("../../../../ASCOM.Alpaca.Simulators/Controllers/FocuserSettingsController.cs"))
            {
                writetext.Write(focuserAPI);
            }

            var rotatorAPI = BuildSettingsAPI(typeof(ASCOM.Simulators.RotatorHardware), "Rotator", "((DeviceManager.GetRotator(DeviceNumber) as ASCOM.Simulators.Rotator)).RotatorHardware");

            using (StreamWriter writetext = File.CreateText("../../../../ASCOM.Alpaca.Simulators/Controllers/RotatorSettingsController.cs"))
            {
                writetext.Write(rotatorAPI);
            }

            var domeAPI = BuildSettingsAPI(typeof(ASCOM.Simulators.DomeHardware), "Dome", "((DeviceManager.GetDome(DeviceNumber) as ASCOM.Simulators.Dome)).DomeHardware");

            using (StreamWriter writetext = File.CreateText("../../../../ASCOM.Alpaca.Simulators/Controllers/DomeSettingsController.cs"))
            {
                writetext.Write(domeAPI);
            }

            var smAPI = BuildSettingsAPI(typeof(ASCOM.Simulators.SafetyMonitor), "SafetyMonitor", "((DeviceManager.GetSafetyMonitor(DeviceNumber) as ASCOM.Simulators.SafetyMonitor))");

            using (StreamWriter writetext = File.CreateText("../../../../ASCOM.Alpaca.Simulators/Controllers/SafetyMonitorSettingsController.cs"))
            {
                writetext.Write(smAPI);
            }
        }

        private static string BuildSettingsAPI(Type DriverType, string DeviceType, string AccessString)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(Usings);
            builder.AppendLine(NameSpace);
            builder.AppendLine("{");

            builder.AppendLine(ClassHeader);
            builder.AppendLine($"    public class {DeviceType}SettingsController : ProcessBaseController");

            builder.AppendLine("    {");

            // Generate API for FilterWheel
            foreach (var prop in SettingsHelpers.GetSettingsProperties(DriverType))
            {
                dynamic setting = prop.GetValue(Activator.CreateInstance(DriverType));

                builder.AppendLine(GetSettingRaw(DeviceType, setting.Key, setting.Description, setting.Value.GetType().ToString(), GetResponseType(setting.Value.GetType()), $"{AccessString}.{prop.Name}.Value"));
                builder.AppendLine(PutSettingRaw(DeviceType, setting.Key, setting.Description, setting.Value.GetType().ToString(), $"{AccessString}.{prop.Name}.Value = {setting.Key};"));
            }

            builder.AppendLine("}");

            builder.AppendLine("}");

            var res = builder.ToString();

            return res;
        }

        private static string GetResponseType(Type t)
        {
            if (t == typeof(bool))
            {
                return typeof(BoolResponse).ToString();
            }
            else if (t == typeof(DateTime))
            {
                return typeof(DateTimeResponse).ToString();
            }
            else if (t == typeof(double))
            {
                return typeof(DoubleResponse).ToString();
            }
            else if (t == typeof(float))
            {
                return typeof(DoubleResponse).ToString();
            }
            else if (t == typeof(int))
            {
                return typeof(IntResponse).ToString();
            }
            else if (t == typeof(string))
            {
                return typeof(StringResponse).ToString();
            }
            else if (t == typeof(short))
            {
                return typeof(IntResponse).ToString();
            }
            throw new Exception($"Unknown Type {t}");
            
        }

        private static string Usings = @"//DO NOT EDIT, AUTO-GENORATED
using ASCOM.Common.Alpaca;
using ASCOM.Simulators;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
";

        private static string NameSpace = "namespace ASCOM.Alpaca.Simulators";

        private static string ClassHeader = @"    /// <summary>
    /// Autogenerated DO NOT EDIT
    /// OmniSim only, not part of Alpaca
    /// </summary>
    [ServiceFilter(typeof(AuthorizationFilter))]
    [ApiController]
    [Route(""simulator/v1/"")]";

        private static string GetSettingRaw(string device, string key, string description, string type, string responsetype, string command)
        {
            return 
                $"        /// <summary>\r\n" +
                $"        /// OmniSim Only - {description}\r\n" +
                $"        /// </summary>\r\n" +
                $"        /// <remarks>\r\n" +
                $"        /// <para>{description}</para>\r\n" +
                $"        /// </remarks>\r\n" +
                $"        /// <param name=\"DeviceNumber\">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>\r\n" +
                $"        /// <param name=\"ClientID\">Client's unique ID.</param>\r\n" +
                $"        /// <param name=\"ClientTransactionID\">Client's transaction ID.</param>\r\n" +
                $"        /// <response code=\"200\">Transaction complete or exception</response>\r\n" +
                $"        /// <response code=\"400\" examples=\"Error message describing why the command cannot be processed\">Method or parameter value error, check error message</response>\r\n" +
                $"        /// <response code=\"500\" examples=\"Error message describing why the command cannot be processed\">Server internal error, check error message</response>\r\n" +
                $"        [HttpGet]\r\n" +
                $"        [Produces(MediaTypeNames.Application.Json)]\r\n" +
                $"        [ApiExplorerSettings(GroupName = \"OmniSim\")]\r\n" +
                $"        [Route(\"{device.ToLower()}/{{DeviceNumber}}/{key.ToLower()}\")]\r\n" +
                $"        public ActionResult<{responsetype}> {key}(\r\n" +
                $"            [Required][DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = \"uint32\")][Range(0, 4294967295)] uint DeviceNumber,\r\n" +
                $"            [SwaggerSchema(Strings.ClientIDDescription, Format = \"uint32\")][Range(0, 4294967295)] uint ClientID = 0,\r\n" +
                $"            [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = \"uint32\")][Range(0, 4294967295)] uint ClientTransactionID = 0)\r\n" +
                $"        {{\r\n" +
                $"            return ProcessRequest(() => {command}, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);\r\n" +
                $"        }}";
        }

        private static string PutSettingRaw(string device, string key, string description, string type, string command)
        {
            return
                $"        /// <summary>\r\n" +
                $"        /// OmniSim Only - {description}\r\n" +
                $"        /// </summary>\r\n" +
                $"        /// <remarks>\r\n" +
                $"        /// <para>{description}</para>\r\n" +
                $"        /// </remarks>\r\n" +
                $"        /// <param name=\"DeviceNumber\">Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)</param>\r\n" +
                $"        /// <param name=\"{key}\">{description}</param>\r\n" +
                $"        /// <param name=\"ClientID\">Client's unique ID.</param>\r\n" +
                $"        /// <param name=\"ClientTransactionID\">Client's transaction ID.</param>\r\n" +
                $"        /// <response code=\"200\">Transaction complete or exception</response>\r\n" +
                $"        /// <response code=\"400\" examples=\"Error message describing why the command cannot be processed\">Method or parameter value error, check error message</response>\r\n" +
                $"        /// <response code=\"500\" examples=\"Error message describing why the command cannot be processed\">Server internal error, check error message</response>\r\n" +
                $"        [HttpPut]\r\n" +
                $"        [Produces(MediaTypeNames.Application.Json)]\r\n" +
                $"        [ApiExplorerSettings(GroupName = \"OmniSim\")]\r\n" +
                $"        [Route(\"{device.ToLower()}/{{DeviceNumber}}/{key.ToLower()}\")]\r\n" +
                $"        public ActionResult<Response> {key}(\r\n" +
                $"            [Required][DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription, Format = \"uint32\")][Range(0, 4294967295)] uint DeviceNumber,\r\n" +
                $"            [Required][FromForm][SwaggerSchema(\"{description}\")] {type} {key},\r\n" +
                $"            [FromForm][SwaggerSchema(Strings.ClientIDDescription, Format = \"uint32\")][Range(0, 4294967295)] uint ClientID = 0,\r\n" +
                $"            [FromForm][SwaggerSchema(Strings.ClientTransactionIDDescription, Format = \"uint32\")][Range(0, 4294967295)] uint ClientTransactionID = 0)\r\n" +
                $"        {{\r\n" +
                $"            return ProcessRequest(() => {{ {command} }}, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $\"{key}={{{key}}}\");\r\n" +
                $"        }}";
        }
    }
}