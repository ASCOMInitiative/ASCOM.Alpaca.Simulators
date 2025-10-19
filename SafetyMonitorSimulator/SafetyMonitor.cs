using ASCOM.Common;
using ASCOM.Common.DeviceInterfaces;
using ASCOM.Common.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;

using OmniSim.BaseDriver;

namespace ASCOM.Simulators
{
    //
    // Your driver's DeviceID is ASCOM.Simulator.SafetyMonitor
    //
    // The Guid attribute sets the CLSID for ASCOM.Simulator.SafetyMonitor
    // The ClassInterface/None addribute prevents an empty interface called
    // _Conceptual from being created and used as the [default] interface
    //

    /// <summary>
    /// ASCOM SafetyMonitor Driver for a SafetyMonitor.
    /// This class is the implementation of the public ASCOM interface.
    /// </summary>

    public class SafetyMonitor : OmniSim.BaseDriver.Driver, ISafetyMonitorV3, IDisposable, IAlpacaDevice, ISimulation
    {
        #region Constants

        private const string UNIQUE_ID_PROFILE_NAME = "UniqueID";

        /// <summary>
        /// Name of the Driver
        /// </summary>
        private const string name = "Alpaca Safety Monitor Simulator";

        /// <summary>
        /// Description of the driver
        /// </summary>
        private const string description = "ASCOM SafetyMonitor Simulator Driver";

        /// <summary>
        /// Driver information
        /// </summary>
        private const string driverInfo = "SafetyMonitor Simulator Drivers";

        #endregion Constants

        #region ISafetyMonitor Public Members

        //
        // PUBLIC COM INTERFACE ISafetyMonitor IMPLEMENTATION
        //
        public SafetyMonitor()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SafetyMonitor"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public SafetyMonitor(int deviceNumber, ILogger logger, IProfile profile)
            : base(deviceNumber, logger, profile)
        {
            Profile = profile;

            DeviceNumber = deviceNumber;

            LoadSettings();

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
                logger.LogError($"SafetyMonitor {deviceNumber} - {ex.Message}");
            }

            logger.LogInformation($"SafetyMonitor {deviceNumber} - UUID of {UniqueID}");
        }

        public override string DeviceName { get => Name; }

        /// <summary>
        /// Gets the delay for the connect timer
        /// </summary>
        public Setting<short> ConnectDelay { get; } = new Setting<short>("ConnectDelay", "The delay to be used for Connect() in milliseconds, allowed values are 1-30000", 1500);

        /// <summary>
        /// Gets the stored interface version to use.
        /// </summary>
        public Setting<short> InterfaceVersionSetting { get; } = new Setting<short>("InterfaceVersion", "The ASCOM Interface Version, allowed values are 1-3", 3);

        /// <summary>
        /// Gets the stored interface version to use.
        /// </summary>
        public Setting<bool> IsSafeSetting { get; } = new Setting<bool>("IsSafeSetting", "If the Safety Monitor returns IsSafe as true or false", true);

        /// <summary>
        /// Gets the ASCOM Driver Description.
        /// </summary>
        public override string Name
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    return name;
                }, DeviceType, MemberNames.Name, "Get");
            }
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
                    return description;
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
                    return driverInfo;
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
                    return this.InterfaceVersionSetting.Value;
                }, DeviceType, MemberNames.InterfaceVersion, "Get");
            }
        }


        /// <summary>
        /// Gets an interface version for V1 drivers that would throw on a InterfaceVersion Call.
        /// </summary>
        public override short SafeInterfaceVersion
        {
            get
            {
                return this.InterfaceVersionSetting.Value;
            }
        }

        /// <summary>
        /// Return the condition of the SafetyMonitor
        /// </summary>
        /// <value>State of the Monitor</value>
        public bool IsSafe
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    if (!Connected)
                    {
                        throw new NotConnectedException("As of 2025/02 Safety Monitors should throw when not connected.");
                    }
                    return IsSafeSetting.Value;
                }, DeviceType, MemberNames.IsSafe, "Get");
            }
        }

        
        #endregion ISafetyMonitor Public Members

        #region ISafetyMonitorV3 members

        public override List<StateValue> DeviceState
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    List<StateValue> deviceState = new List<StateValue>();
                                try { deviceState.Add(new StateValue(nameof(ISafetyMonitorV3.IsSafe), IsSafe)); } catch { }
                                try { deviceState.Add(new StateValue(DateTime.Now)); } catch { }

                                return deviceState;
                }, DeviceType, MemberNames.DeviceState, "Get");
            }
        }

        public override DeviceTypes DeviceType => DeviceTypes.SafetyMonitor;

        /// <summary>
        /// Connects to the hardware.
        /// </summary>
        public override void Connect()
        {
            base.ConnectTimer.Interval = ConnectDelay.Value;
            base.Connect();
        }

        #endregion ISafetyMonitorV3 members

        #region SafetyMonitor Private Members

        /// <summary>
        /// Load the profile values.
        /// </summary>
        public override void LoadSettings()
        {
            this.IsSafeSetting.Value = this.Profile.GetSettingReturningDefault(this.IsSafeSetting);
            this.InterfaceVersionSetting.Value = this.Profile.GetSettingReturningDefault(this.InterfaceVersionSetting);
        }

        /// <summary>
        /// Save profile values.
        /// </summary>
        public void SaveProfileSettings()
        {
            this.Profile.SetSetting(this.IsSafeSetting);
            this.Profile.SetSetting(this.InterfaceVersionSetting);
        }

        #endregion SafetyMonitor Private Members
    }
}