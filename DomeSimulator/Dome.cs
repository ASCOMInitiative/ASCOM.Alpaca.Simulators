using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using ASCOM.Common;
using ASCOM.Common.DeviceInterfaces;
using ASCOM.Common.Interfaces;

[assembly: InternalsVisibleTo("ASCOM.Alpaca.Simulators")]

namespace ASCOM.Simulators
{
    public class Dome : OmniSim.BaseDriver.Driver, IDomeV3, IAlpacaDevice, ISimulation
    {
        private const string UNIQUE_ID_PROFILE_NAME = "UniqueID";

        private ILogger Logger;
        private IProfile Profile;
        internal DomeHardware DomeHardware;

        public Dome(int deviceNumber, ILogger logger, IProfile profile) : base(deviceNumber, logger, profile)
        {
            Logger = logger;
            Profile = profile;

            DomeHardware = new DomeHardware(Profile);

            LoadConfig();

            LogMessage($"New dome {deviceNumber}", "Log started");

            DeviceNumber = deviceNumber;

            //This should be replaced by the next bit of code but is semi-unique as a default.
            UniqueID = Name + deviceNumber.ToString();
            //Create a Unique ID if it does not exist
            try
            {
                if (!profile.ContainsKey(UNIQUE_ID_PROFILE_NAME))
                {
                    var uniqueid = Guid.NewGuid().ToString();
                    profile.WriteValue(UNIQUE_ID_PROFILE_NAME, uniqueid);
                }
                UniqueID = profile.GetValue(UNIQUE_ID_PROFILE_NAME);
            }
            catch (Exception ex)
            {
                logger.LogError($"Dome {deviceNumber} - {ex.Message}");
            }

            logger.LogInformation($"Dome {deviceNumber} - UUID of {UniqueID}");
        }

        #region IDomeV2 members

        public void LoadConfig()
        {
            DomeHardware.LoadConfig();
        }

        public void SaveConfig()
        {
            DomeHardware.SaveConfig();
        }

        public override string DeviceName { get => Name; }

        public void ResetSettings()
        {
            Profile.Clear();
        }

        public string GetXMLProfile()
        {
            return Profile.GetProfile();
        }

        private void check_Az(double Az)
        {
            if (Az == DomeHardware.INVALID_COORDINATE)
                throw new InvalidValueException("Azimuth", "Invalid Coordinate", "0 to 360 degrees");
            if (Az >= 360.0 | Az < 0.0)
            {
                throw new InvalidValueException("Azimuth", Az.ToString(), "0 to 360.0 degrees");
            }
        }

        public void AbortSlew()
        {
            CheckConnected("Dome simulator is not connected");
            DomeHardware.Halt();
        }

        /// <summary>
        /// Connects to the hardware.
        /// </summary>
        public override void Connect()
        {
            base.ConnectTimer.Interval = DomeHardware.ConnectDelay.Value;
            base.Connect();
        }

        public double Altitude
        {
            get
            {
                if (!DomeHardware.CanSetAltitude.Value)
                    throw new PropertyNotImplementedException("Altitude", false);

                CheckConnected("Dome simulator is not connected");

                if (DomeHardware.ShutterState == ShutterState.Error)
                    LogMessage("Altitude", "Shutter in ErrorState");
                if (DomeHardware.ShutterState != ShutterState.Open)
                    LogMessage("Altitude", "Shutter not Open");

                return DomeHardware.DomeAltitude.Value;
            }
        }

        public bool AtHome
        {
            get
            {
                CheckConnected("Dome simulator is not connected");
                return DomeHardware.AtHome;
            }
        }

        public bool AtPark
        {
            get
            {
                CheckConnected("Dome simulator is not connected");
                return DomeHardware.AtPark;
            }
        }

        public double Azimuth
        {
            get
            {
                if (!DomeHardware.CanSetAzimuth.Value)
                    throw new PropertyNotImplementedException("Azimuth", false);

                CheckConnected("Dome simulator is not connected");
                return DomeHardware.DomeAzimuth.Value;
            }
        }

        public bool CanFindHome
        {
            get
            {
                return DomeHardware.CanFindHome.Value;
            }
        }

        public bool CanPark
        {
            get
            {
                return DomeHardware.CanPark.Value;
            }
        }

        public bool CanSetAltitude
        {
            get
            {
                return DomeHardware.CanSetAltitude.Value;
            }
        }

        public bool CanSetAzimuth
        {
            get
            {
                return DomeHardware.CanSetAzimuth.Value;
            }
        }

        public bool CanSetPark
        {
            get
            {
                return DomeHardware.CanSetPark.Value;
            }
        }

        public bool CanSetShutter
        {
            get
            {
                return DomeHardware.CanSetShutter.Value;
            }
        }

        public bool CanSlave
        {
            get
            {
                return false;
            }
        }

        public bool CanSyncAzimuth
        {
            get
            {
                return DomeHardware.CanSyncAzimuth.Value;
            }
        }

        public void CloseShutter()
        {
            if (!DomeHardware.CanSetShutter.Value)
            {
                throw new MethodNotImplementedException("CloseShutter");
            }

            CheckConnected("Dome simulator is not connected");
            DomeHardware.CloseShutter();
        }

        /// <summary>
        /// Gets the ASCOM Driver Description.
        /// </summary>
        public override string Description
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    return "ASCOM Dome Simulator .NET";
                }, DeviceType, MemberNames.Description, "Get");
            }
        }

        /// <summary>
        /// Gets the ASCOM Driver DriverInfo.
        /// </summary>
        public override string DriverInfo
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    return "ASCOM OmniSim Dome simulation";
                }, DeviceType, MemberNames.DriverInfo, "Get");
            }
        }

        /// <summary>
        /// Gets the ASCOM Driver Interface Version.
        /// </summary>
        public override short InterfaceVersion
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    return this.DomeHardware.InterfaceVersionSetting.Value;
                }, DeviceType, MemberNames.InterfaceVersion, "Get");
            }
        }

        public override short SafeInterfaceVersion
        {
            get
            {
                return this.DomeHardware.InterfaceVersionSetting.Value;
            }
        }

        public void FindHome()
        {
            if (!DomeHardware.CanFindHome.Value)
            {
                throw new MethodNotImplementedException("FindHome");
            }

            CheckConnected("Dome simulator is not connected");
            if (!DomeHardware.g_bAtHome)
                DomeHardware.FindHome();
        }

        public string Name
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    return "Alpaca Dome Simulator";
                }, DeviceType, MemberNames.Name, "Get");
            }
        }

        public void OpenShutter()
        {
            if (!DomeHardware.CanSetShutter.Value)
            {
                throw new MethodNotImplementedException("OpenShutter");
            }

            CheckConnected("Dome simulator is not connected");

            if (DomeHardware.ShutterState == ShutterState.Error)
                throw new InvalidOperationException("Shutter failed to open");

            DomeHardware.OpenShutter();
        }

        public void Park()
        {
            if (!DomeHardware.CanPark.Value)
            {
                throw new MethodNotImplementedException("Park");
            }

            CheckConnected("Dome simulator is not connected");
            if (!DomeHardware.g_bAtPark)
                DomeHardware.Park();
        }

        public void SetPark()
        {
            if (!DomeHardware.CanSetPark.Value)
            {
                throw new MethodNotImplementedException("Park");
            }

            CheckConnected("Dome simulator is not connected");
            DomeHardware.ParkPosition.Value = DomeHardware.DomeAzimuth.Value;

            if (!DomeHardware.StandardAtPark.Value)
            {
                DomeHardware.g_bAtPark = true;
            }
        }

        public ShutterState ShutterStatus
        {
            get
            {
                if (!DomeHardware.CanSetShutter.Value)
                    throw new PropertyNotImplementedException("ShutterStatus", false);

                CheckConnected("Dome simulator is not connected");
                return DomeHardware.ShutterState;
            }
        }

        public bool Slaved
        {
            get
            {
                return false;
            }
            set
            {
                if (value)
                    throw new PropertyNotImplementedException("Slaved", true);
            }
        }

        public bool Slewing
        {
            get
            {
                CheckConnected("Dome simulator is not connected");

                return DomeHardware.IsSlewing;
            }
        }

        public void SlewToAltitude(double Altitude)
        {
            if (!DomeHardware.CanSetAltitude.Value)
            {
                throw new MethodNotImplementedException("SlewToAltitude");
            }

            CheckConnected("Dome simulator is not connected");

            if (DomeHardware.ShutterState == ShutterState.Error)
                LogMessage("Altitude", "Shutter in ErrorState");
            if (DomeHardware.ShutterState != ShutterState.Open)
                LogMessage("Altitude", "Shutter not Open");
            if (Altitude < DomeHardware.MinimumAltitude.Value | Altitude > DomeHardware.MaximumAltitude.Value)
                throw new InvalidValueException("SlewToAltitude", Altitude.ToString(), DomeHardware.MinimumAltitude.ToString() + " to " + DomeHardware.MaximumAltitude.ToString());

            DomeHardware.MoveShutter(Altitude);
        }

        public void SlewToAzimuth(double Azimuth)
        {
            if (!DomeHardware.CanSetAzimuth.Value)
            {
                throw new MethodNotImplementedException("SlewToAzimuth");
            }

            CheckConnected("Dome simulator is not connected");
            check_Az(Azimuth);
            DomeHardware.Move(Azimuth);
        }

        public void SyncToAzimuth(double Azimuth)
        {
            if (!DomeHardware.CanSyncAzimuth.Value)
            {
                throw new MethodNotImplementedException("SyncToAzimuth");
            }

            CheckConnected("Dome simulator is not connected");
            check_Az(Azimuth);
            DomeHardware.Sync(Azimuth);
        }

        #endregion IDomeV2 members

        #region IDomeV3 members

        /// <summary>
        /// Return the device's operational state in one call
        /// </summary>
        public List<StateValue> DeviceState
        {
            get
            {
                // Create an array list to hold the IStateValue entries
                List<StateValue> deviceState = new List<StateValue>();

                try { deviceState.Add(new StateValue(nameof(IDomeV3.Altitude), Altitude)); } catch { }
                try { deviceState.Add(new StateValue(nameof(IDomeV3.AtHome), AtHome)); } catch { }
                try { deviceState.Add(new StateValue(nameof(IDomeV3.AtPark), AtPark)); } catch { }
                try { deviceState.Add(new StateValue(nameof(IDomeV3.Azimuth), Azimuth)); } catch { }
                try { deviceState.Add(new StateValue(nameof(IDomeV3.ShutterStatus), ShutterStatus)); } catch { }
                try { deviceState.Add(new StateValue(nameof(IDomeV3.Slewing), Slewing)); } catch { }
                try { deviceState.Add(new StateValue(DateTime.Now)); } catch { }

                return deviceState;
            }
        }

        public override DeviceTypes DeviceType => DeviceTypes.Dome;

        #endregion IDomeV3 members

        private void LogMessage(string message, string details)
        {
            Logger.LogVerbose(message + " - " + details);
        }
    }
}