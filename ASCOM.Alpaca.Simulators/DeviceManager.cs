using Alpaca;
using ASCOM.Alpaca.Discovery;
using ASCOM.Common.Alpaca;
using ASCOM.Common.DeviceInterfaces;
using ASCOM.Simulators;
using ASCOM.Tools;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ASCOM.Alpaca.Simulators
{
    internal static class DeviceManager
    {
        internal static uint RawTransactionID
        {
            get;
            set;
        } = 0;

        /// <summary>
        /// A server wide Transaction ID Counter
        /// </summary>
        internal static uint ServerTransactionID
        {
            get
            {
                return RawTransactionID++;
            }
        }

        // These store the actual instance of the device drivers. They are keyed to the Device Number
        private readonly static Dictionary<int, ICameraV3> cameraV3s = new Dictionary<int, ICameraV3>();
        private readonly static Dictionary<int, ICoverCalibratorV1> coverCalibratorV1s = new Dictionary<int, ICoverCalibratorV1>();
        private readonly static Dictionary<int, IDomeV2> domeV2s = new Dictionary<int, IDomeV2>();
        private readonly static Dictionary<int, IFilterWheelV2> filterWheelV2s = new Dictionary<int, IFilterWheelV2>();
        private readonly static Dictionary<int, IFocuserV3> focuserV3s = new Dictionary<int, IFocuserV3>();
        private readonly static Dictionary<int, IObservingConditions> observingConditions = new Dictionary<int, IObservingConditions>();
        private readonly static Dictionary<int, IRotatorV3> rotatorV3s = new Dictionary<int, IRotatorV3>();
        private readonly static Dictionary<int, ISafetyMonitor> safetyMonitors = new Dictionary<int, ISafetyMonitor>();
        private readonly static Dictionary<int, ISwitchV2> switchV2s = new Dictionary<int, ISwitchV2>();
        private readonly static Dictionary<int, ITelescopeV3> telescopeV3s = new Dictionary<int, ITelescopeV3>();

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

        static DeviceManager()
        {
            //Only one instance of each in this simulator

            rotatorV3s.Add(0, new ASCOM.Simulators.Rotator(0, Logging.Log,
                new XMLProfile(ServerSettings.SettingsFolderName, Rotator, 0)));

            safetyMonitors.Add(0, new ASCOM.Simulators.SafetyMonitor(0, Logging.Log,
                new XMLProfile(ServerSettings.SettingsFolderName, SafetyMonitor, 0)));

            switchV2s.Add(0, new ASCOM.Simulators.Switch(0, Logging.Log,
                new XMLProfile(ServerSettings.SettingsFolderName, Switch, 0)));

            telescopeV3s.Add(0, new ASCOM.Simulators.Telescope(0, Logging.Log,
                new XMLProfile(ServerSettings.SettingsFolderName, Telescope, 0)));

            LoadCamera(0);

            LoadCoverCalibrator(0);

            LoadDome(0);

            LoadFilterWheel(0);

            LoadFocuser(0);

            LoadObservingConditions(0);
        }

        internal static void LoadCamera(int DeviceID)
        {
            //Remove if the simulated instance already exists
            cameraV3s.Remove(DeviceID);
            //Add the new instance
            cameraV3s.Add(DeviceID, new ASCOM.Simulators.Camera(DeviceID, Logging.Log, new XMLProfile(ServerSettings.SettingsFolderName, Camera, (uint)DeviceID)));
        }

        internal static void LoadCoverCalibrator(int DeviceID)
        {
            //Remove if the simulated instance already exists
            coverCalibratorV1s.Remove(DeviceID);
            //Add the new instance
            coverCalibratorV1s.Add(0, new ASCOM.Simulators.CoverCalibratorSimulator(DeviceID, Logging.Log, new XMLProfile(ServerSettings.SettingsFolderName, CoverCalibrator, (uint)DeviceID)));
        }

        internal static void LoadDome(int DeviceID)
        {
            //Remove if the simulated instance already exists
            domeV2s.Remove(DeviceID);
            //Add the new instance
            domeV2s.Add(DeviceID, new ASCOM.Simulators.Dome(DeviceID, Logging.Log,
                new XMLProfile(ServerSettings.SettingsFolderName, Dome, (uint)DeviceID)));
        }

        internal static void LoadFilterWheel(int DeviceID)
        {
            //Remove if the simulated instance already exists
            filterWheelV2s.Remove(DeviceID);
            //Add the new instance
            filterWheelV2s.Add(DeviceID, new ASCOM.Simulators.FilterWheel(DeviceID, Logging.Log,
                new XMLProfile(ServerSettings.SettingsFolderName, FilterWheel, (uint)DeviceID)));
        }

        internal static void LoadFocuser(int DeviceID)
        {
            //Remove if the simulated instance already exists
            focuserV3s.Remove(DeviceID);
            //Add the new instance
            focuserV3s.Add(DeviceID, new ASCOM.Simulators.Focuser(DeviceID, Logging.Log,
                new XMLProfile(ServerSettings.SettingsFolderName, Focuser, (uint)DeviceID)));
        }

        internal static void LoadObservingConditions(int DeviceID)
        {
            //Remove if the simulated instance already exists
            observingConditions.Remove(DeviceID);
            //Add the new instance
            observingConditions.Add(DeviceID, new ASCOM.Simulators.ObservingConditions(DeviceID, Logging.Log,
                new XMLProfile(ServerSettings.SettingsFolderName, ObservingConditions, (uint)DeviceID)));
        }

        /// <summary>
        /// Reset all device settings profiles.
        /// </summary>
        internal static void Reset()
        {
            foreach (var dev in cameraV3s.Values)
            {
                try
                {
                    (dev as ASCOM.Simulators.Camera)?.ClearProfile();
                }
                catch (Exception ex)
                {
                    Logging.LogError($"Failed to reset Camera settings with error: {ex.Message}");
                }
            }

            foreach (var dev in coverCalibratorV1s.Values)
            {
                try
                {
                    (dev as ASCOM.Simulators.CoverCalibratorSimulator)?.ResetSettings();
                }
                catch (Exception ex)
                {
                    Logging.LogError($"Failed to reset CoverCalibrator settings with error: {ex.Message}");
                }
            }

            foreach (var dev in domeV2s.Values)
            {
                try
                {
                    (dev as ASCOM.Simulators.Dome)?.ResetConfig();
                }
                catch (Exception ex)
                {
                    Logging.LogError($"Failed to reset Dome settings with error: {ex.Message}");
                }
            }

            try
            {
                ASCOM.Simulators.FilterWheelHardware.ResetProfile();
            }
            catch (Exception ex)
            {
                Logging.LogError($"Failed to reset Filter Wheel settings with error: {ex.Message}");
            }

            foreach (var dev in focuserV3s.Values)
            {
                try
                {
                    (dev as ASCOM.Simulators.Focuser)?.Reset();
                }
                catch (Exception ex)
                {
                    Logging.LogError($"Failed to reset Focuser settings with error: {ex.Message}");
                }
            }

            try
            {
                ASCOM.Simulators.OCSimulator.ClearProfile();
            }
            catch (Exception ex)
            {
                Logging.LogError($"Failed to reset Observing Conditions settings with error: {ex.Message}");
            }

            try
            {
                ASCOM.Simulators.RotatorHardware.ResetProfile();
            }
            catch (Exception ex)
            {
                Logging.LogError($"Failed to reset Rotator settings with error: {ex.Message}");
            }

            foreach (var dev in safetyMonitors.Values)
            {
                try
                {
                    (dev as ASCOM.Simulators.SafetyMonitor)?.ResetProfile();
                }
                catch (Exception ex)
                {
                    Logging.LogError($"Failed to reset SafetyMonitor settings with error: {ex.Message}");
                }
            }

            foreach (var dev in switchV2s.Values)
            {
                try
                {
                    (dev as ASCOM.Simulators.Switch)?.ResetProfile();
                }
                catch (Exception ex)
                {
                    Logging.LogError($"Failed to reset Switch settings with error: {ex.Message}");
                }
            }

            try
            {
                ASCOM.Simulators.TelescopeHardware.ClearProfile();
            }
            catch(Exception ex)
            {
                Logging.LogError($"Failed to reset Telescope settings with error: {ex.Message}");
            }
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

        internal static ICameraV3 GetCamera(uint DeviceID)
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

        internal static ICoverCalibratorV1 GetCoverCalibrator(uint DeviceID)
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

        internal static IDomeV2 GetDome(uint DeviceID)
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

        internal static IFilterWheelV2 GetFilterWheel(uint DeviceID)
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

        internal static IFocuserV3 GetFocuser(uint DeviceID)
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

        internal static IObservingConditions GetObservingConditions(uint DeviceID)
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

        internal static IRotatorV3 GetRotator(uint DeviceID)
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

        internal static ISafetyMonitor GetSafetyMonitor(uint DeviceID)
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

        internal static ISwitchV2 GetSwitch(uint DeviceID)
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

        internal static ITelescopeV3 GetTelescope(uint DeviceID)
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

        private static string Telescope
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

        private static string Camera
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

        private static string Dome
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

        private static string FilterWheel
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

        private static string Focuser
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

        private static string ObservingConditions
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

        private static string Rotator
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

        private static string SafetyMonitor
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

        private static string Switch
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

        private static string CoverCalibrator
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
        #endregion
    }
}