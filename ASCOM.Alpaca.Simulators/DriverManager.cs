using ASCOM.Tools;
using System;

namespace ASCOM.Alpaca.Simulators
{
    public class DriverManager
    {
        internal static void LoadCamera(int DeviceID)
        {
            DeviceManager.LoadCamera(DeviceID, new ASCOM.Simulators.Camera(DeviceID, Logging.Log, new XMLProfile(ServerSettings.SettingsFolderName, DeviceManager.Camera, (uint)DeviceID)));
        }

        internal static void LoadCoverCalibrator(int DeviceID)
        {
            DeviceManager.LoadCoverCalibrator(DeviceID, new ASCOM.Simulators.CoverCalibratorSimulator(DeviceID, Logging.Log, new XMLProfile(ServerSettings.SettingsFolderName, DeviceManager.CoverCalibrator, (uint)DeviceID)));
        }

        internal static void LoadDome(int DeviceID)
        {
            DeviceManager.LoadDome(DeviceID, new ASCOM.Simulators.Dome(DeviceID, Logging.Log, new XMLProfile(ServerSettings.SettingsFolderName, DeviceManager.Dome, (uint)DeviceID)));
        }

        internal static void LoadFilterWheel(int DeviceID)
        {
            DeviceManager.LoadFilterWheel(DeviceID, new ASCOM.Simulators.FilterWheel(DeviceID, Logging.Log, new XMLProfile(ServerSettings.SettingsFolderName, DeviceManager.FilterWheel, (uint)DeviceID)));
        }

        internal static void LoadFocuser(int DeviceID)
        {
            DeviceManager.LoadFocuser(DeviceID, new ASCOM.Simulators.Focuser(DeviceID, Logging.Log, new XMLProfile(ServerSettings.SettingsFolderName, DeviceManager.Focuser, (uint)DeviceID)));
        }

        internal static void LoadObservingConditions(int DeviceID)
        {
            DeviceManager.LoadObservingConditions(DeviceID, new ASCOM.Simulators.ObservingConditions(DeviceID, Logging.Log, new XMLProfile(ServerSettings.SettingsFolderName, DeviceManager.ObservingConditions, (uint)DeviceID)));
        }

        internal static void LoadRotator(int DeviceID) 
        {
            DeviceManager.LoadRotator(DeviceID, new ASCOM.Simulators.Rotator(DeviceID, Logging.Log, new XMLProfile(ServerSettings.SettingsFolderName, DeviceManager.Rotator, (uint)DeviceID)));
        }

        internal static void LoadSafetyMonitor(int DeviceID)
        {
            DeviceManager.LoadSafetyMonitor(DeviceID, new ASCOM.Simulators.SafetyMonitor(DeviceID, Logging.Log, new XMLProfile(ServerSettings.SettingsFolderName, DeviceManager.SafetyMonitor, (uint)DeviceID )));
        }

        internal static void LoadSwitch(int DeviceID)
        {
            DeviceManager.LoadSwitch(DeviceID, new ASCOM.Simulators.Switch(DeviceID, Logging.Log, new XMLProfile(ServerSettings.SettingsFolderName, DeviceManager.Switch, (uint)DeviceID)));
        }

        internal static void LoadTelescope(int DeviceID)
        {
            DeviceManager.LoadTelescope(DeviceID, new ASCOM.Simulators.Telescope(DeviceID, Logging.Log, new XMLProfile(ServerSettings.SettingsFolderName, DeviceManager.Telescope, (uint)DeviceID)));
        }

        /// <summary>
        /// Reset all device settings profiles.
        /// </summary>
        internal static void Reset()
        {
            foreach (var dev in DeviceManager.cameraV3s.Values)
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

            foreach (var dev in DeviceManager.coverCalibratorV1s.Values)
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

            foreach (var dev in DeviceManager.domeV2s.Values)
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

            foreach (var dev in DeviceManager.filterWheelV2s.Values)
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

            foreach (var dev in DeviceManager.focuserV3s.Values)
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

            foreach (var dev in DeviceManager.observingConditions.Values)
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

            try
            {
                ASCOM.Simulators.RotatorHardware.ResetProfile();
            }
            catch (Exception ex)
            {
                Logging.LogError($"Failed to reset Rotator settings with error: {ex.Message}");
            }

            foreach (var dev in DeviceManager.safetyMonitors.Values)
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

            foreach (var dev in DeviceManager.switchV2s.Values)
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