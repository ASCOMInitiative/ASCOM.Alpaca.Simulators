using ASCOM.Alpaca.Discovery;
using ASCOM.Alpaca.Razor;
using ASCOM.Common.DeviceInterfaces;
using ASCOM.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace ASCOM.Alpaca
{
    public static class DeviceManager
    {
        public static uint RawTransactionID
        {
            get;
            set;
        } = 1;

        /// <summary>
        /// A server wide Transaction ID Counter
        /// </summary>
        public static uint ServerTransactionID
        {
            get
            {
                return RawTransactionID++;
            }
        }

        internal static IAlpacaConfiguration Configuration { get; private set; }

        // These store the actual instance of the device drivers. They are keyed to the Device Number
        public static readonly Dictionary<int, ICameraV4> cameraV3s = new Dictionary<int, ICameraV4>();

        public static readonly Dictionary<int, ICoverCalibratorV2> coverCalibratorV1s = new Dictionary<int, ICoverCalibratorV2>();
        public static readonly Dictionary<int, IDomeV3> domeV2s = new Dictionary<int, IDomeV3>();
        public static readonly Dictionary<int, IFilterWheelV3> filterWheelV2s = new Dictionary<int, IFilterWheelV3>();
        public static readonly Dictionary<int, IFocuserV4> focuserV3s = new Dictionary<int, IFocuserV4>();
        public static readonly Dictionary<int, IObservingConditionsV2> observingConditions = new Dictionary<int, IObservingConditionsV2>();
        public static readonly Dictionary<int, IRotatorV4> rotatorV3s = new Dictionary<int, IRotatorV4>();
        public static readonly Dictionary<int, ISafetyMonitorV3> safetyMonitors = new Dictionary<int, ISafetyMonitorV3>();
        public static readonly Dictionary<int, ISwitchV3> switchV2s = new Dictionary<int, ISwitchV3>();
        public static readonly Dictionary<int, ITelescopeV4> telescopeV3s = new Dictionary<int, ITelescopeV4>();

        static List<AlpacaConfiguredDevice> AlpacaDevices = new List<AlpacaConfiguredDevice>();

        public static void LoadConfiguration(IAlpacaConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static void LoadCamera(int DeviceID, ICameraV4 Device, string AlpacaName, string UniqueID)
        {
            //Remove if the simulated instance already exists
            cameraV3s.Remove(DeviceID);
            //Add the new instance
            cameraV3s.Add(DeviceID, Device);

            AlpacaDevices.Remove(AlpacaDevices.FirstOrDefault(a => a.UniqueID == UniqueID));
            AlpacaDevices.Add(new AlpacaConfiguredDevice(AlpacaName, "Camera", DeviceID, UniqueID));
        }

        public static void LoadCoverCalibrator(int DeviceID, ICoverCalibratorV2 Device, string AlpacaName, string UniqueID)
        {
            //Remove if the simulated instance already exists
            coverCalibratorV1s.Remove(DeviceID);
            //Add the new instance
            coverCalibratorV1s.Add(0, Device);

            AlpacaDevices.Remove(AlpacaDevices.FirstOrDefault(a => a.UniqueID == UniqueID));
            AlpacaDevices.Add(new AlpacaConfiguredDevice(AlpacaName, "CoverCalibrator", DeviceID, UniqueID));
        }

        public static void LoadDome(int DeviceID, IDomeV3 Device, string AlpacaName, string UniqueID)
        {
            //Remove if the simulated instance already exists
            domeV2s.Remove(DeviceID);
            //Add the new instance
            domeV2s.Add(DeviceID, Device);

            AlpacaDevices.Remove(AlpacaDevices.FirstOrDefault(a => a.UniqueID == UniqueID));
            AlpacaDevices.Add(new AlpacaConfiguredDevice(AlpacaName, "Dome", DeviceID, UniqueID));
        }

        public static void LoadFilterWheel(int DeviceID, IFilterWheelV3 Device, string AlpacaName, string UniqueID)
        {
            //Remove if the simulated instance already exists
            filterWheelV2s.Remove(DeviceID);
            //Add the new instance
            filterWheelV2s.Add(DeviceID, Device);

            AlpacaDevices.Remove(AlpacaDevices.FirstOrDefault(a => a.UniqueID == UniqueID));
            AlpacaDevices.Add(new AlpacaConfiguredDevice(AlpacaName, "FilterWheel", DeviceID, UniqueID));
        }

        public static void LoadFocuser(int DeviceID, IFocuserV4 Device, string AlpacaName, string UniqueID)
        {
            //Remove if the simulated instance already exists
            focuserV3s.Remove(DeviceID);
            //Add the new instance
            focuserV3s.Add(DeviceID, Device);

            AlpacaDevices.Remove(AlpacaDevices.FirstOrDefault(a => a.UniqueID == UniqueID));
            AlpacaDevices.Add(new AlpacaConfiguredDevice(AlpacaName, "Focuser", DeviceID, UniqueID));
        }

        public static void LoadObservingConditions(int DeviceID, IObservingConditionsV2 Device, string AlpacaName, string UniqueID)
        {
            //Remove if the simulated instance already exists
            observingConditions.Remove(DeviceID);
            //Add the new instance
            observingConditions.Add(DeviceID, Device);

            AlpacaDevices.Remove(AlpacaDevices.FirstOrDefault(a => a.UniqueID == UniqueID));
            AlpacaDevices.Add(new AlpacaConfiguredDevice(AlpacaName, "ObservingConditions", DeviceID, UniqueID));
        }

        public static void LoadRotator(int DeviceID, IRotatorV4 Device, string AlpacaName, string UniqueID)
        {
            //Remove if the simulated instance already exists
            rotatorV3s.Remove(DeviceID);
            //Add the new instance
            rotatorV3s.Add(DeviceID, Device);

            AlpacaDevices.Remove(AlpacaDevices.FirstOrDefault(a => a.UniqueID == UniqueID));
            AlpacaDevices.Add(new AlpacaConfiguredDevice(AlpacaName, "Rotator", DeviceID, UniqueID));
        }

        public static void LoadSafetyMonitor(int DeviceID, ISafetyMonitorV3 Device, string AlpacaName, string UniqueID)
        {
            //Remove if the simulated instance already exists
            safetyMonitors.Remove(DeviceID);
            //Add the new instance
            safetyMonitors.Add(DeviceID, Device);

            AlpacaDevices.Remove(AlpacaDevices.FirstOrDefault(a => a.UniqueID == UniqueID));
            AlpacaDevices.Add(new AlpacaConfiguredDevice(AlpacaName, "SafetyMonitor", DeviceID, UniqueID));
        }

        public static void LoadSwitch(int DeviceID, ISwitchV3 Device, string AlpacaName, string UniqueID)
        {
            //Remove if the simulated instance already exists
            switchV2s.Remove(DeviceID);
            //Add the new instance
            switchV2s.Add(DeviceID, Device);

            AlpacaDevices.Remove(AlpacaDevices.FirstOrDefault(a => a.UniqueID == UniqueID));
            AlpacaDevices.Add(new AlpacaConfiguredDevice(AlpacaName, "Switch", DeviceID, UniqueID));
        }

        public static void LoadTelescope(int DeviceID, ITelescopeV4 Device, string AlpacaName, string UniqueID)
        {
            //Remove if the simulated instance already exists
            telescopeV3s.Remove(DeviceID);
            //Add the new instance
            telescopeV3s.Add(DeviceID, Device);

            AlpacaDevices.Remove(AlpacaDevices.FirstOrDefault(a => a.UniqueID == UniqueID));
            AlpacaDevices.Add(new AlpacaConfiguredDevice(AlpacaName, "Telescope", DeviceID, UniqueID));
        }

        //Returns a list of every single device type for the Management API
        internal static List<AlpacaConfiguredDevice> GetDevices()
        {
            return AlpacaDevices;
        }

        //These methods allow access to specific devices for the API controllers and the device Blazor UI Pages

        public static ICameraV4 GetCamera(uint DeviceID)
        {
            if (cameraV3s.ContainsKey((int)DeviceID))
            {
                return cameraV3s[(int)DeviceID];
            }
            else
            {
                throw new DeviceNotFoundException(string.Format("Instance {0} does not exist in this server.", DeviceID));
            }
        }

        public static ICoverCalibratorV2 GetCoverCalibrator(uint DeviceID)
        {
            if (coverCalibratorV1s.ContainsKey((int)DeviceID))
            {
                return coverCalibratorV1s[(int)DeviceID];
            }
            else
            {
                throw new DeviceNotFoundException(string.Format("Instance {0} does not exist in this server.", DeviceID));
            }
        }

        public static IDomeV3 GetDome(uint DeviceID)
        {
            if (domeV2s.ContainsKey((int)DeviceID))
            {
                return domeV2s[(int)DeviceID];
            }
            else
            {
                throw new DeviceNotFoundException(string.Format("Instance {0} does not exist in this server.", DeviceID));
            }
        }

        public static IFilterWheelV3 GetFilterWheel(uint DeviceID)
        {
            if (filterWheelV2s.ContainsKey((int)DeviceID))
            {
                return filterWheelV2s[(int)DeviceID];
            }
            else
            {
                throw new DeviceNotFoundException(string.Format("Instance {0} does not exist in this server.", DeviceID));
            }
        }

        public static IFocuserV4 GetFocuser(uint DeviceID)
        {
            if (focuserV3s.ContainsKey((int)DeviceID))
            {
                return focuserV3s[(int)DeviceID];
            }
            else
            {
                throw new DeviceNotFoundException(string.Format("Instance {0} does not exist in this server.", DeviceID));
            }
        }

        public static IObservingConditionsV2 GetObservingConditions(uint DeviceID)
        {
            if (observingConditions.ContainsKey((int)DeviceID))
            {
                return observingConditions[(int)DeviceID];
            }
            else
            {
                throw new DeviceNotFoundException(string.Format("Instance {0} does not exist in this server.", DeviceID));
            }
        }

        public static IRotatorV4 GetRotator(uint DeviceID)
        {
            if (rotatorV3s.ContainsKey((int)DeviceID))
            {
                return rotatorV3s[(int)DeviceID];
            }
            else
            {
                throw new DeviceNotFoundException(string.Format("Instance {0} does not exist in this server.", DeviceID));
            }
        }

        public static ISafetyMonitorV3 GetSafetyMonitor(uint DeviceID)
        {
            if (safetyMonitors.ContainsKey((int)DeviceID))
            {
                return safetyMonitors[(int)DeviceID];
            }
            else
            {
                throw new DeviceNotFoundException(string.Format("Instance {0} does not exist in this server.", DeviceID));
            }
        }

        public static ISwitchV3 GetSwitch(uint DeviceID)
        {
            if (switchV2s.ContainsKey((int)DeviceID))
            {
                return switchV2s[(int)DeviceID];
            }
            else
            {
                throw new DeviceNotFoundException(string.Format("Instance {0} does not exist in this server.", DeviceID));
            }
        }

        public static ITelescopeV4 GetTelescope(uint DeviceID)
        {
            if (telescopeV3s.ContainsKey((int)DeviceID))
            {
                return telescopeV3s[(int)DeviceID];
            }
            else
            {
                throw new DeviceNotFoundException(string.Format("Instance {0} does not exist in this server.", DeviceID));
            }
        }

        //Use only lowercase for case sensitive OSes

        #region Settings Folder Names

        public static string Telescope
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return "Telescope";
                }
                else
                {
                    return "Telescope".ToLowerInvariant();
                }
            }
        }

        public static string Camera
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return "Camera";
                }
                else
                {
                    return "Camera".ToLowerInvariant();
                }
            }
        }

        public static string Dome
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return "Dome";
                }
                else
                {
                    return "Dome".ToLowerInvariant();
                }
            }
        }

        public static string FilterWheel
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return "FilterWheel";
                }
                else
                {
                    return "FilterWheel".ToLowerInvariant();
                }
            }
        }

        public static string Focuser
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return "Focuser";
                }
                else
                {
                    return "Focuser".ToLowerInvariant();
                }
            }
        }

        public static string ObservingConditions
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return "ObservingConditions";
                }
                else
                {
                    return "ObservingConditions".ToLowerInvariant();
                }
            }
        }

        public static string Rotator
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return "Rotator";
                }
                else
                {
                    return "Rotator".ToLowerInvariant();
                }
            }
        }

        public static string SafetyMonitor
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return "SafetyMonitor";
                }
                else
                {
                    return "SafetyMonitor".ToLowerInvariant();
                }
            }
        }

        public static string Switch
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return "Switch";
                }
                else
                {
                    return "Switch".ToLowerInvariant();
                }
            }
        }

        public static string CoverCalibrator
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return "CoverCalibrator";
                }
                else
                {
                    return "CoverCalibrator".ToLowerInvariant();
                }
            }
        }

        #endregion Settings Folder Names
    }
}