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

        public static readonly Dictionary<int, ICameraV3> Cameras = new Dictionary<int, ICameraV3>();
        public static readonly Dictionary<int, ICoverCalibratorV1> CoverCalibrators = new Dictionary<int, ICoverCalibratorV1>();
        public static readonly Dictionary<int, IDomeV2> Domes = new Dictionary<int, IDomeV2>();
        public static readonly Dictionary<int, IFilterWheelV2> FilterWheels = new Dictionary<int, IFilterWheelV2>();
        public static readonly Dictionary<int, IFocuserV3> Focusers = new Dictionary<int, IFocuserV3>();
        public static readonly Dictionary<int, IObservingConditions> ObservingConditions = new Dictionary<int, IObservingConditions>();
        public static readonly Dictionary<int, IRotatorV3> Rotators = new Dictionary<int, IRotatorV3>();
        public static readonly Dictionary<int, ISafetyMonitor> SafetyMonitors = new Dictionary<int, ISafetyMonitor>();
        public static readonly Dictionary<int, ISwitchV2> Switches = new Dictionary<int, ISwitchV2>();
        public static readonly Dictionary<int, ITelescopeV3> Telescopes = new Dictionary<int, ITelescopeV3>();

        static List<AlpacaConfiguredDevice> AlpacaDevices = new List<AlpacaConfiguredDevice>();

        public static void LoadConfiguration(IAlpacaConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static void LoadCamera(int DeviceID, ICameraV3 Device, string AlpacaName, string UniqueID)
        {
            //Remove if the simulated instance already exists
            Cameras.Remove(DeviceID);
            //Add the new instance
            Cameras.Add(DeviceID, Device);

            AlpacaDevices.Remove(AlpacaDevices.FirstOrDefault(a => a.UniqueID == UniqueID));
            AlpacaDevices.Add(new AlpacaConfiguredDevice(AlpacaName, "Camera", DeviceID, UniqueID));
        }

        public static void LoadCoverCalibrator(int DeviceID, ICoverCalibratorV1 Device, string AlpacaName, string UniqueID)
        {
            //Remove if the simulated instance already exists
            CoverCalibrators.Remove(DeviceID);
            //Add the new instance
            CoverCalibrators.Add(0, Device);

            AlpacaDevices.Remove(AlpacaDevices.FirstOrDefault(a => a.UniqueID == UniqueID));
            AlpacaDevices.Add(new AlpacaConfiguredDevice(AlpacaName, "CoverCalibrator", DeviceID, UniqueID));
        }

        public static void LoadDome(int DeviceID, IDomeV2 Device, string AlpacaName, string UniqueID)
        {
            //Remove if the simulated instance already exists
            Domes.Remove(DeviceID);
            //Add the new instance
            Domes.Add(DeviceID, Device);

            AlpacaDevices.Remove(AlpacaDevices.FirstOrDefault(a => a.UniqueID == UniqueID));
            AlpacaDevices.Add(new AlpacaConfiguredDevice(AlpacaName, "Dome", DeviceID, UniqueID));
        }

        public static void LoadFilterWheel(int DeviceID, IFilterWheelV2 Device, string AlpacaName, string UniqueID)
        {
            //Remove if the simulated instance already exists
            FilterWheels.Remove(DeviceID);
            //Add the new instance
            FilterWheels.Add(DeviceID, Device);

            AlpacaDevices.Remove(AlpacaDevices.FirstOrDefault(a => a.UniqueID == UniqueID));
            AlpacaDevices.Add(new AlpacaConfiguredDevice(AlpacaName, "FilterWheel", DeviceID, UniqueID));
        }

        public static void LoadFocuser(int DeviceID, IFocuserV3 Device, string AlpacaName, string UniqueID)
        {
            //Remove if the simulated instance already exists
            Focusers.Remove(DeviceID);
            //Add the new instance
            Focusers.Add(DeviceID, Device);

            AlpacaDevices.Remove(AlpacaDevices.FirstOrDefault(a => a.UniqueID == UniqueID));
            AlpacaDevices.Add(new AlpacaConfiguredDevice(AlpacaName, "Focuser", DeviceID, UniqueID));
        }

        public static void LoadObservingConditions(int DeviceID, IObservingConditions Device, string AlpacaName, string UniqueID)
        {
            //Remove if the simulated instance already exists
            ObservingConditions.Remove(DeviceID);
            //Add the new instance
            ObservingConditions.Add(DeviceID, Device);

            AlpacaDevices.Remove(AlpacaDevices.FirstOrDefault(a => a.UniqueID == UniqueID));
            AlpacaDevices.Add(new AlpacaConfiguredDevice(AlpacaName, "ObservingConditions", DeviceID, UniqueID));
        }

        public static void LoadRotator(int DeviceID, IRotatorV3 Device, string AlpacaName, string UniqueID)
        {
            //Remove if the simulated instance already exists
            Rotators.Remove(DeviceID);
            //Add the new instance
            Rotators.Add(DeviceID, Device);

            AlpacaDevices.Remove(AlpacaDevices.FirstOrDefault(a => a.UniqueID == UniqueID));
            AlpacaDevices.Add(new AlpacaConfiguredDevice(AlpacaName, "Rotator", DeviceID, UniqueID));
        }

        public static void LoadSafetyMonitor(int DeviceID, ISafetyMonitor Device, string AlpacaName, string UniqueID)
        {
            //Remove if the simulated instance already exists
            SafetyMonitors.Remove(DeviceID);
            //Add the new instance
            SafetyMonitors.Add(DeviceID, Device);

            AlpacaDevices.Remove(AlpacaDevices.FirstOrDefault(a => a.UniqueID == UniqueID));
            AlpacaDevices.Add(new AlpacaConfiguredDevice(AlpacaName, "SafetyMonitor", DeviceID, UniqueID));
        }

        public static void LoadSwitch(int DeviceID, ISwitchV2 Device, string AlpacaName, string UniqueID)
        {
            //Remove if the simulated instance already exists
            Switches.Remove(DeviceID);
            //Add the new instance
            Switches.Add(DeviceID, Device);

            AlpacaDevices.Remove(AlpacaDevices.FirstOrDefault(a => a.UniqueID == UniqueID));
            AlpacaDevices.Add(new AlpacaConfiguredDevice(AlpacaName, "Switch", DeviceID, UniqueID));
        }

        public static void LoadTelescope(int DeviceID, ITelescopeV3 Device, string AlpacaName, string UniqueID)
        {
            //Remove if the simulated instance already exists
            Telescopes.Remove(DeviceID);
            //Add the new instance
            Telescopes.Add(DeviceID, Device);

            AlpacaDevices.Remove(AlpacaDevices.FirstOrDefault(a => a.UniqueID == UniqueID));
            AlpacaDevices.Add(new AlpacaConfiguredDevice(AlpacaName, "Telescope", DeviceID, UniqueID));
        }

        //Returns a list of every single device type for the Management API
        internal static List<AlpacaConfiguredDevice> GetDevices()
        {
            return AlpacaDevices;
        }

        //These methods allow access to specific devices for the API controllers and the device Blazor UI Pages

        public static ICameraV3 GetCamera(uint DeviceID)
        {
            if (Cameras.ContainsKey((int)DeviceID))
            {
                return Cameras[(int)DeviceID];
            }
            else
            {
                throw new DeviceNotFoundException(string.Format("Instance {0} does not exist in this server.", DeviceID));
            }
        }

        public static ICoverCalibratorV1 GetCoverCalibrator(uint DeviceID)
        {
            if (CoverCalibrators.ContainsKey((int)DeviceID))
            {
                return CoverCalibrators[(int)DeviceID];
            }
            else
            {
                throw new DeviceNotFoundException(string.Format("Instance {0} does not exist in this server.", DeviceID));
            }
        }

        public static IDomeV2 GetDome(uint DeviceID)
        {
            if (Domes.ContainsKey((int)DeviceID))
            {
                return Domes[(int)DeviceID];
            }
            else
            {
                throw new DeviceNotFoundException(string.Format("Instance {0} does not exist in this server.", DeviceID));
            }
        }

        public static IFilterWheelV2 GetFilterWheel(uint DeviceID)
        {
            if (FilterWheels.ContainsKey((int)DeviceID))
            {
                return FilterWheels[(int)DeviceID];
            }
            else
            {
                throw new DeviceNotFoundException(string.Format("Instance {0} does not exist in this server.", DeviceID));
            }
        }

        public static IFocuserV3 GetFocuser(uint DeviceID)
        {
            if (Focusers.ContainsKey((int)DeviceID))
            {
                return Focusers[(int)DeviceID];
            }
            else
            {
                throw new DeviceNotFoundException(string.Format("Instance {0} does not exist in this server.", DeviceID));
            }
        }

        public static IObservingConditions GetObservingConditions(uint DeviceID)
        {
            if (ObservingConditions.ContainsKey((int)DeviceID))
            {
                return ObservingConditions[(int)DeviceID];
            }
            else
            {
                throw new DeviceNotFoundException(string.Format("Instance {0} does not exist in this server.", DeviceID));
            }
        }

        public static IRotatorV3 GetRotator(uint DeviceID)
        {
            if (Rotators.ContainsKey((int)DeviceID))
            {
                return Rotators[(int)DeviceID];
            }
            else
            {
                throw new DeviceNotFoundException(string.Format("Instance {0} does not exist in this server.", DeviceID));
            }
        }

        public static ISafetyMonitor GetSafetyMonitor(uint DeviceID)
        {
            if (SafetyMonitors.ContainsKey((int)DeviceID))
            {
                return SafetyMonitors[(int)DeviceID];
            }
            else
            {
                throw new DeviceNotFoundException(string.Format("Instance {0} does not exist in this server.", DeviceID));
            }
        }

        public static ISwitchV2 GetSwitch(uint DeviceID)
        {
            if (Switches.ContainsKey((int)DeviceID))
            {
                return Switches[(int)DeviceID];
            }
            else
            {
                throw new DeviceNotFoundException(string.Format("Instance {0} does not exist in this server.", DeviceID));
            }
        }

        public static ITelescopeV3 GetTelescope(uint DeviceID)
        {
            if (Telescopes.ContainsKey((int)DeviceID))
            {
                return Telescopes[(int)DeviceID];
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

        public static string ObservingCondition
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