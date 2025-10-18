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

        private const string CheckConnectedFailureError = "Dome simulator is not connected";

        private ILogger Logger;
        private IProfile Profile;
        internal DomeHardware DomeHardware;

        public override string DeviceName { get => Name; }

        public Dome(int deviceNumber, ILogger logger, IProfile profile) : base(deviceNumber, logger, profile)
        {
            Logger = logger;
            Profile = profile;

            DomeHardware = new DomeHardware(Profile);

            LoadConfig();

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

        /// <summary>
        /// Gets the device type value for this driver.
        /// </summary>
        public override DeviceTypes DeviceType => DeviceTypes.Dome;

        #region IDomeV2 members

        /// <summary>
        /// Load the stored config for this driver.
        /// </summary>
        public void LoadConfig()
        {
            DomeHardware.LoadConfig();
        }

        /// <summary>
        /// Save the current config.
        /// </summary>
        public void SaveConfig()
        {
            DomeHardware.SaveConfig();
        }

        /// <summary>
        /// Reset settings to default.
        /// </summary>
        public new void ResetSettings()
        {
            Profile.Clear();
        }

        /// <summary>
        /// Gets the current profile in XML format. This is not guaranteed to be compatible across versions.
        /// </summary>
        /// <returns>An XML encoded copy of the profile.</returns>
        public new string GetXMLProfile()
        {
            return Profile.GetProfile();
        }

        private void check_Az(double Az)
        {
            if (Az == DomeHardware.InvalidCoordinate)
            {
                throw new InvalidValueException("Azimuth", "Invalid Coordinate", "0 to 360 degrees");
            }

            if (Az >= 360.0 | Az < 0.0)
            {
                throw new InvalidValueException("Azimuth", Az.ToString(), "0 to 360.0 degrees");
            }
        }

        public void AbortSlew()
        {
            this.ProcessCommand(
            () =>
            {
                CheckConnected(CheckConnectedFailureError);
                DomeHardware.Halt();
            }, DeviceType, MemberNames.AbortSlew, "Command");
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
                return this.ProcessCommand(
                () =>
                {
                    if (!DomeHardware.CanSetAltitude.Value)
                        throw new PropertyNotImplementedException("Altitude", false);

                    CheckConnected(CheckConnectedFailureError);

                    if (DomeHardware.ShutterState == ShutterState.Error)
                    {
                        Logger.LogWarning("Shutter in ErrorState");
                    }

                    if (DomeHardware.ShutterState != ShutterState.Open)
                    {
                        Logger.LogWarning("Shutter not Open");
                    }

                    return DomeHardware.DomeAltitude.Value;
                }, DeviceType, MemberNames.Altitude, "Get");
            }
        }

        public bool AtHome
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    CheckConnected(CheckConnectedFailureError);
                    return DomeHardware.AtHome;
                }, DeviceType, MemberNames.AtHome, "Get");
            }
        }

        public bool AtPark
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    CheckConnected(CheckConnectedFailureError);
                    return DomeHardware.AtPark;
                }, DeviceType, MemberNames.AtPark, "Get");
            }
        }

        public double Azimuth
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    if (!DomeHardware.CanSetAzimuth.Value)
                    {
                        throw new PropertyNotImplementedException("Azimuth", false);
                    }

                    CheckConnected(CheckConnectedFailureError);
                    return DomeHardware.DomeAzimuth.Value;
                }, DeviceType, MemberNames.Azimuth, "Get");
            }
        }

        public bool CanFindHome
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    return DomeHardware.CanFindHome.Value;
                }, DeviceType, MemberNames.CanFindHome, "Get");
            }
        }

        public bool CanPark
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    return DomeHardware.CanPark.Value;
                }, DeviceType, MemberNames.CanPark, "Get");
            }
        }

        public bool CanSetAltitude
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    return DomeHardware.CanSetAltitude.Value;
                }, DeviceType, MemberNames.CanSetAltitude, "Get");
            }
        }

        public bool CanSetAzimuth
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    return DomeHardware.CanSetAzimuth.Value;
                }, DeviceType, MemberNames.CanSetAzimuth, "Get");
            }
        }

        public bool CanSetPark
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    return DomeHardware.CanSetPark.Value;
                }, DeviceType, MemberNames.CanSetPark, "Get");
            }
        }

        public bool CanSetShutter
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    return DomeHardware.CanSetShutter.Value;
                }, DeviceType, MemberNames.CanSetShutter, "Get");
            }
        }

        public bool CanSlave
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    return false;
                }, DeviceType, MemberNames.CanSlave, "Get");
            }
        }

        public bool CanSyncAzimuth
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    return DomeHardware.CanSyncAzimuth.Value;
                }, DeviceType, MemberNames.CanSyncAzimuth, "Get");
            }
        }

        public void CloseShutter()
        {
            this.ProcessCommand(
                () =>
                {
                    if (!DomeHardware.CanSetShutter.Value)
                    {
                        throw new MethodNotImplementedException("CloseShutter");
                    }

                    CheckConnected(CheckConnectedFailureError);
                    DomeHardware.CloseShutter();
                }, DeviceType, MemberNames.CloseShutter, "Command");
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
            this.ProcessCommand(
                () =>
                {
                    if (!DomeHardware.CanFindHome.Value)
                    {
                        throw new MethodNotImplementedException("FindHome");
                    }

                    CheckConnected(CheckConnectedFailureError);
                    if (!DomeHardware.CommandAtHome)
                        DomeHardware.FindHome();
                }, DeviceType, MemberNames.FindHome, "Command");
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
            this.ProcessCommand(
                () =>
                {
                    if (!DomeHardware.CanSetShutter.Value)
                    {
                        throw new MethodNotImplementedException("OpenShutter");
                    }

                    CheckConnected(CheckConnectedFailureError);

                    if (DomeHardware.ShutterState == ShutterState.Error)
                        throw new InvalidOperationException("Shutter failed to open");

                    DomeHardware.OpenShutter();
                }, DeviceType, MemberNames.OpenShutter, "Command");
        }

        public void Park()
        {
            this.ProcessCommand(
                () =>
                {
                    if (!DomeHardware.CanPark.Value)
                    {
                        throw new MethodNotImplementedException("Park");
                    }

                    CheckConnected(CheckConnectedFailureError);
                    if (!DomeHardware.CommandAtPark)
                        DomeHardware.Park();
                }, DeviceType, MemberNames.Park, "Command");

        }

        public void SetPark()
        {
            this.ProcessCommand(
                () =>
                {
                    if (!DomeHardware.CanSetPark.Value)
                    {
                        throw new MethodNotImplementedException("Park");
                    }

                    CheckConnected(CheckConnectedFailureError);
                    DomeHardware.ParkPosition.Value = DomeHardware.DomeAzimuth.Value;

                    if (!DomeHardware.StandardAtPark.Value)
                    {
                        DomeHardware.CommandAtPark = true;
                    }
                }, DeviceType, MemberNames.SetPark, "Command");
        }

        public ShutterState ShutterStatus
        {
            get
            {

                return this.ProcessCommand(
                () =>
                {

                    if (!DomeHardware.CanSetShutter.Value)
                        throw new PropertyNotImplementedException("ShutterStatus", false);

                    CheckConnected(CheckConnectedFailureError);
                    return DomeHardware.ShutterState;
                }, DeviceType, MemberNames.ShutterStatus, "Get");
            }
        }

        public bool Slaved
        {
            get
            {

                return this.ProcessCommand(
                () =>
                {

                    return false;
                }, DeviceType, MemberNames.Slaved, "Get");
            }

            set
            {
                ProcessCommand(
                () =>
                {
                    if (value)
                    {
                        throw new PropertyNotImplementedException("Slaved", true);
                    }
                }, DeviceType, MemberNames.Slaved, "Set");
            }
        }

        public bool Slewing
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    CheckConnected(CheckConnectedFailureError);

                    return DomeHardware.IsSlewing;
                }, DeviceType, MemberNames.Slewing, "Command");
            }
        }

        public void SlewToAltitude(double Altitude)
        {
            this.ProcessCommand(
                () =>
                {
                    if (!DomeHardware.CanSetAltitude.Value)
                    {
                        throw new MethodNotImplementedException("SlewToAltitude");
                    }

                    if (DomeHardware.ShutterState == ShutterState.Error)
                    {
                        Logger.LogWarning("Shutter in ErrorState");
                    }

                    if (DomeHardware.ShutterState != ShutterState.Open)
                    {
                        Logger.LogWarning("Shutter not Open");
                    }

                    if (Altitude < DomeHardware.MinimumAltitude.Value | Altitude > DomeHardware.MaximumAltitude.Value)
                    {
                        throw new InvalidValueException("SlewToAltitude", Altitude.ToString(), DomeHardware.MinimumAltitude.Value.ToString() + " to " + DomeHardware.MaximumAltitude.Value.ToString());
                    }

                    CheckConnected(CheckConnectedFailureError);

                    DomeHardware.MoveShutter(Altitude);
                }, DeviceType, MemberNames.SlewToAltitude, "Command");
        }

        public void SlewToAzimuth(double Azimuth)
        {
            this.ProcessCommand(
                () =>
                {
                    if (!DomeHardware.CanSetAzimuth.Value)
                    {
                        throw new MethodNotImplementedException("SlewToAzimuth");
                    }

                    CheckConnected(CheckConnectedFailureError);
                    check_Az(Azimuth);
                    DomeHardware.Move(Azimuth);
                }, DeviceType, MemberNames.SlewToAzimuth, "Command");
        }

        public void SyncToAzimuth(double Azimuth)
        {
            this.ProcessCommand(
                () =>
                {

                    if (!DomeHardware.CanSyncAzimuth.Value)
                    {
                        throw new MethodNotImplementedException("SyncToAzimuth");
                    }

                    CheckConnected(CheckConnectedFailureError);
                    check_Az(Azimuth);
                    DomeHardware.Sync(Azimuth);
                }, DeviceType, MemberNames.SyncToAzimuth, "Command");
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
                return this.ProcessCommand(
                () =>
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
                }, DeviceType, MemberNames.DeviceState, "Get");
            }
        }
        #endregion IDomeV3 members
    }
}