using ASCOM.Tools;
using System;

namespace ASCOM.Alpaca.Simulators
{
    public class DriverManager
    {
        internal static void LoadCamera(int DeviceID)
        {
            var dev = new ASCOM.Simulators.Camera(DeviceID, Logging.Log, new XMLProfile(ServerSettings.SettingsFolderName, DeviceManager.Camera, (uint)DeviceID));

            DeviceManager.LoadCamera(DeviceID, dev, dev.DeviceName, dev.UniqueID);
        }

        internal static void LoadCoverCalibrator(int DeviceID)
        {
            var dev = new ASCOM.Simulators.CoverCalibratorSimulator(DeviceID, Logging.Log, new XMLProfile(ServerSettings.SettingsFolderName, DeviceManager.CoverCalibrator, (uint)DeviceID));
            DeviceManager.LoadCoverCalibrator(DeviceID, dev, dev.DeviceName, dev.UniqueID);
        }

        internal static void LoadDome(int DeviceID)
        {
            var dev = new ASCOM.Simulators.Dome(DeviceID, Logging.Log, new XMLProfile(ServerSettings.SettingsFolderName, DeviceManager.Dome, (uint)DeviceID));
            DeviceManager.LoadDome(DeviceID, dev, dev.DeviceName, dev.UniqueID);
        }

        internal static void LoadFilterWheel(int DeviceID)
        {
            var dev = new ASCOM.Simulators.FilterWheel(DeviceID, new OmniSim.Tools.DualLogger(ServerSettings.LogFileNameDevice("FilterWheel", DeviceID), Logging.Log), new XMLProfile(ServerSettings.SettingsFolderName, DeviceManager.FilterWheel, (uint)DeviceID));
            DeviceManager.LoadFilterWheel(DeviceID, dev, dev.DeviceName, dev.UniqueID);
        }

        internal static void LoadFocuser(int DeviceID)
        {
            var dev = new ASCOM.Simulators.Focuser(DeviceID, new OmniSim.Tools.DualLogger(ServerSettings.LogFileNameDevice("Focuser", DeviceID), Logging.Log), new XMLProfile(ServerSettings.SettingsFolderName, DeviceManager.Focuser, (uint)DeviceID));
            DeviceManager.LoadFocuser(DeviceID, dev, dev.DeviceName, dev.UniqueID);
        }

        internal static void LoadObservingConditions(int DeviceID)
        {
            var dev = new ASCOM.Simulators.ObservingConditions(DeviceID, Logging.Log, new XMLProfile(ServerSettings.SettingsFolderName, DeviceManager.ObservingCondition, (uint)DeviceID));
            DeviceManager.LoadObservingConditions(DeviceID, dev, dev.DeviceName, dev.UniqueID);
        }

        internal static void LoadRotator(int DeviceID)
        {
            var dev = new ASCOM.Simulators.Rotator(DeviceID, new OmniSim.Tools.DualLogger(ServerSettings.LogFileNameDevice("Rotator", DeviceID), Logging.Log), new XMLProfile(ServerSettings.SettingsFolderName, DeviceManager.Rotator, (uint)DeviceID));
            DeviceManager.LoadRotator(DeviceID, dev, dev.DeviceName, dev.UniqueID);
        }

        internal static void LoadSafetyMonitor(int DeviceID)
        {
            var dev = new ASCOM.Simulators.SafetyMonitor(DeviceID, Logging.Log, new XMLProfile(ServerSettings.SettingsFolderName, DeviceManager.SafetyMonitor, (uint)DeviceID));
            DeviceManager.LoadSafetyMonitor(DeviceID, dev, dev.DeviceName, dev.UniqueID);
        }

        internal static void LoadSwitch(int DeviceID)
        {
            var dev = new ASCOM.Simulators.Switch(DeviceID, Logging.Log, new XMLProfile(ServerSettings.SettingsFolderName, DeviceManager.Switch, (uint)DeviceID));
            DeviceManager.LoadSwitch(DeviceID, dev, dev.DeviceName, dev.UniqueID);
        }

        internal static void LoadTelescope(int DeviceID)
        {
            var dev = new ASCOM.Simulators.Telescope(DeviceID, Logging.Log, new XMLProfile(ServerSettings.SettingsFolderName, DeviceManager.Telescope, (uint)DeviceID));
            DeviceManager.LoadTelescope(DeviceID, dev, dev.DeviceName, dev.UniqueID);
        }

        /// <summary>
        /// Reset all device settings profiles.
        /// </summary>
        internal static void Reset()
        {
            foreach (var dev in DeviceManager.Cameras.Values)
            {
                try
                {
                    (dev as ASCOM.Simulators.Camera)?.ResetSettings();
                }
                catch (Exception ex)
                {
                    Logging.LogError($"Failed to reset Camera settings with error: {ex.Message}");
                }
            }

            foreach (var dev in DeviceManager.CoverCalibrators.Values)
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

            foreach (var dev in DeviceManager.Domes.Values)
            {
                try
                {
                    (dev as ASCOM.Simulators.Dome)?.ResetSettings();
                }
                catch (Exception ex)
                {
                    Logging.LogError($"Failed to reset Dome settings with error: {ex.Message}");
                }
            }

            foreach (var dev in DeviceManager.FilterWheels.Values)
            {
                try
                {
                    (dev as ASCOM.Simulators.FilterWheel)?.ResetSettings();
                }
                catch (Exception ex)
                {
                    Logging.LogError($"Failed to reset Filter Wheel settings with error: {ex.Message}");
                }
            }

            foreach (var dev in DeviceManager.Focusers.Values)
            {
                try
                {
                    (dev as ASCOM.Simulators.Focuser)?.ResetSettings();
                }
                catch (Exception ex)
                {
                    Logging.LogError($"Failed to reset Focuser settings with error: {ex.Message}");
                }
            }

            foreach (var dev in DeviceManager.ObservingConditions.Values)
            {
                try
                {
                    (dev as ASCOM.Simulators.ObservingConditions)?.ResetSettings();
                }
                catch (Exception ex)
                {
                    Logging.LogError($"Failed to reset Observing Conditions settings with error: {ex.Message}");
                }
            }

            foreach (var dev in DeviceManager.Rotators.Values)
            {
                try
                {
                    (dev as ASCOM.Simulators.Rotator).RotatorHardware.ResetProfile();
                }
                catch (Exception ex)
                {
                    Logging.LogError($"Failed to reset Rotator settings with error: {ex.Message}");
                }
            }

            foreach (var dev in DeviceManager.SafetyMonitors.Values)
            {
                try
                {
                    (dev as ASCOM.Simulators.SafetyMonitor)?.ResetSettings();
                }
                catch (Exception ex)
                {
                    Logging.LogError($"Failed to reset SafetyMonitor settings with error: {ex.Message}");
                }
            }

            foreach (var dev in DeviceManager.Switches.Values)
            {
                try
                {
                    (dev as ASCOM.Simulators.Switch)?.ResetSettings();
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
            catch (Exception ex)
            {
                Logging.LogError($"Failed to reset Telescope settings with error: {ex.Message}");
            }
        }
    }
}