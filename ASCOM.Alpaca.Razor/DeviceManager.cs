using ASCOM.Alpaca.Discovery;
using ASCOM.Common.DeviceInterfaces;
using ASCOM.Simulators;
using ASCOM.Tools;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ASCOM.Alpaca.Simulators
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

        // These store the actual instance of the device drivers. They are keyed to the Device Number
        public static readonly Dictionary<int, ICameraV3> cameraV3s = new Dictionary<int, ICameraV3>();

        public static readonly Dictionary<int, ICoverCalibratorV1> coverCalibratorV1s = new Dictionary<int, ICoverCalibratorV1>();
        public static readonly Dictionary<int, IDomeV2> domeV2s = new Dictionary<int, IDomeV2>();
        public static readonly Dictionary<int, IFilterWheelV2> filterWheelV2s = new Dictionary<int, IFilterWheelV2>();
        public static readonly Dictionary<int, IFocuserV3> focuserV3s = new Dictionary<int, IFocuserV3>();
        public static readonly Dictionary<int, IObservingConditions> observingConditions = new Dictionary<int, IObservingConditions>();
        public static readonly Dictionary<int, IRotatorV3> rotatorV3s = new Dictionary<int, IRotatorV3>();
        public static readonly Dictionary<int, ISafetyMonitor> safetyMonitors = new Dictionary<int, ISafetyMonitor>();
        public static readonly Dictionary<int, ISwitchV2> switchV2s = new Dictionary<int, ISwitchV2>();
        public static readonly Dictionary<int, ITelescopeV3> telescopeV3s = new Dictionary<int, ITelescopeV3>();

        private static string ServerFolderName
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return "Server";
                }
                else
                {
                    return "Server".ToLowerInvariant();
                }
            }
        }

        public static void LoadCamera(int DeviceID, ICameraV3 Device)
        {
            //Remove if the simulated instance already exists
            cameraV3s.Remove(DeviceID);
            //Add the new instance
            cameraV3s.Add(DeviceID, Device);
        }

        public static void LoadCoverCalibrator(int DeviceID, ICoverCalibratorV1 Device)
        {
            //Remove if the simulated instance already exists
            coverCalibratorV1s.Remove(DeviceID);
            //Add the new instance
            coverCalibratorV1s.Add(0, Device);
        }

        public static void LoadDome(int DeviceID, IDomeV2 Device)
        {
            //Remove if the simulated instance already exists
            domeV2s.Remove(DeviceID);
            //Add the new instance
            domeV2s.Add(DeviceID, Device);
        }

        public static void LoadFilterWheel(int DeviceID, IFilterWheelV2 Device)
        {
            //Remove if the simulated instance already exists
            filterWheelV2s.Remove(DeviceID);
            //Add the new instance
            filterWheelV2s.Add(DeviceID, Device);
        }

        public static void LoadFocuser(int DeviceID, IFocuserV3 Device)
        {
            //Remove if the simulated instance already exists
            focuserV3s.Remove(DeviceID);
            //Add the new instance
            focuserV3s.Add(DeviceID, Device);
        }

        public static void LoadObservingConditions(int DeviceID, IObservingConditions Device)
        {
            //Remove if the simulated instance already exists
            observingConditions.Remove(DeviceID);
            //Add the new instance
            observingConditions.Add(DeviceID, Device);
        }

        public static void LoadRotator(int DeviceID, IRotatorV3 Device)
        {
            //Remove if the simulated instance already exists
            rotatorV3s.Remove(DeviceID);
            //Add the new instance
            rotatorV3s.Add(DeviceID, Device);
        }

        public static void LoadSafetyMonitor(int DeviceID, ISafetyMonitor Device)
        {
            //Remove if the simulated instance already exists
            safetyMonitors.Remove(DeviceID);
            //Add the new instance
            safetyMonitors.Add(DeviceID, Device);
        }

        public static void LoadSwitch(int DeviceID, ISwitchV2 Device)
        {
            //Remove if the simulated instance already exists
            switchV2s.Remove(DeviceID);
            //Add the new instance
            switchV2s.Add(DeviceID, Device);
        }

        public static void LoadTelescope(int DeviceID, ITelescopeV3 Device)
        {
            //Remove if the simulated instance already exists
            telescopeV3s.Remove(DeviceID);
            //Add the new instance
            telescopeV3s.Add(DeviceID, Device);
        }

        //Returns a list of every single device type for the Management API
        internal static List<AlpacaConfiguredDevice> GetDevices()
        {
            List<AlpacaConfiguredDevice> devices = new List<AlpacaConfiguredDevice>();

            foreach (var dev in cameraV3s)
            {
                devices.Add(new AlpacaConfiguredDevice((dev.Value as IAlpacaDevice).DeviceName, "Camera", (dev.Value as IAlpacaDevice).DeviceNumber, (dev.Value as IAlpacaDevice).UniqueID));
            }

            foreach (var dev in coverCalibratorV1s)
            {
                devices.Add(new AlpacaConfiguredDevice((dev.Value as IAlpacaDevice).DeviceName, "CoverCalibrator", (dev.Value as IAlpacaDevice).DeviceNumber, (dev.Value as IAlpacaDevice).UniqueID));
            }

            foreach (var dev in domeV2s)
            {
                devices.Add(new AlpacaConfiguredDevice((dev.Value as IAlpacaDevice).DeviceName, "Dome", (dev.Value as IAlpacaDevice).DeviceNumber, (dev.Value as IAlpacaDevice).UniqueID));
            }

            foreach (var dev in filterWheelV2s)
            {
                devices.Add(new AlpacaConfiguredDevice((dev.Value as IAlpacaDevice).DeviceName, "FilterWheel", (dev.Value as IAlpacaDevice).DeviceNumber, (dev.Value as IAlpacaDevice).UniqueID));
            }

            foreach (var dev in focuserV3s)
            {
                devices.Add(new AlpacaConfiguredDevice((dev.Value as IAlpacaDevice).DeviceName, "Focuser", (dev.Value as IAlpacaDevice).DeviceNumber, (dev.Value as IAlpacaDevice).UniqueID));
            }

            foreach (var dev in observingConditions)
            {
                devices.Add(new AlpacaConfiguredDevice((dev.Value as IAlpacaDevice).DeviceName, "ObservingConditions", (dev.Value as IAlpacaDevice).DeviceNumber, (dev.Value as IAlpacaDevice).UniqueID));
            }

            foreach (var dev in rotatorV3s)
            {
                devices.Add(new AlpacaConfiguredDevice((dev.Value as IAlpacaDevice).DeviceName, "Rotator", (dev.Value as IAlpacaDevice).DeviceNumber, (dev.Value as IAlpacaDevice).UniqueID));
            }

            foreach (var dev in safetyMonitors)
            {
                devices.Add(new AlpacaConfiguredDevice((dev.Value as IAlpacaDevice).DeviceName, "SafetyMonitor", (dev.Value as IAlpacaDevice).DeviceNumber, (dev.Value as IAlpacaDevice).UniqueID));
            }

            foreach (var dev in switchV2s)
            {
                devices.Add(new AlpacaConfiguredDevice((dev.Value as IAlpacaDevice).DeviceName, "Switch", (dev.Value as IAlpacaDevice).DeviceNumber, (dev.Value as IAlpacaDevice).UniqueID));
            }

            foreach (var dev in telescopeV3s)
            {
                devices.Add(new AlpacaConfiguredDevice((dev.Value as IAlpacaDevice).DeviceName, "Telescope", (dev.Value as IAlpacaDevice).DeviceNumber, (dev.Value as IAlpacaDevice).UniqueID));
            }

            return devices;
        }

        //These methods allow access to specific devices for the API controllers and the device Blazor UI Pages

        public static ICameraV3 GetCamera(uint DeviceID)
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

        public static ICoverCalibratorV1 GetCoverCalibrator(uint DeviceID)
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

        public static IDomeV2 GetDome(uint DeviceID)
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

        public static IFilterWheelV2 GetFilterWheel(uint DeviceID)
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

        public static IFocuserV3 GetFocuser(uint DeviceID)
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

        public static IObservingConditions GetObservingConditions(uint DeviceID)
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

        public static IRotatorV3 GetRotator(uint DeviceID)
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

        public static ISafetyMonitor GetSafetyMonitor(uint DeviceID)
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

        public static ISwitchV2 GetSwitch(uint DeviceID)
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

        public static ITelescopeV3 GetTelescope(uint DeviceID)
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