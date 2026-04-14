using ASCOM.Common;
using ASCOM.Common.DeviceInterfaces;
using ASCOM.Common.Interfaces;
using OmniSim.BaseDriver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ASCOM.Alpaca.Simulators")]
namespace ASCOM.Simulators
{
    /// <summary>
    /// ASCOM ObservingConditions Driver for Observing Conditions OCSimulator.
    /// </summary>
    public class ObservingConditions : OmniSim.BaseDriver.Driver, IObservingConditionsV2, IAlpacaDevice, ISimulation
    {
        #region Variables and Constants

        internal static ILogger TL; // Private variable to hold the trace logger object (creates a diagnostic log file with information that you specify)

        private const string UNIQUE_ID_PROFILE_NAME = "UniqueID";

        internal OCSimulator OCSimulator;

        #endregion Variables and Constants

        #region Class initialiser

        /// <summary>
        /// Initializes a new instance of the <see cref="OCSimulator"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public ObservingConditions(int deviceNumber, ILogger logger, IProfile profile) : base(deviceNumber, logger, profile)
        {
            try
            {
                OCSimulator = new OCSimulator();
                OCSimulator.driverProfile = profile;
                OCSimulator.TL = logger;
                OCSimulator.Init();
                TL = logger;

                DeviceNumber = deviceNumber;

                OCSimulator.LogMessage($"New ObservingConditions {deviceNumber}", "Starting initialisation");

                // This should be replaced by the next bit of code but is semi - unique as a default.
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
                    logger.LogError($"ObservingConditions {deviceNumber} - {ex.Message}");
                }

                logger.LogInformation($"ObservingConditions {deviceNumber} - UUID of {UniqueID}");

                if (DeviceCapabilities.LatestInterface[DeviceType] < InterfaceVersionSetting.Value)
                {
                    InterfaceVersionSetting.Value = DeviceCapabilities.LatestInterface[DeviceType];
                }

                OCSimulator.LogMessage("ObservingConditions", "Completed initialisation");
            }
            catch (Exception ex)
            {
                OCSimulator.LogMessage("ObservingConditions", ex.ToString());
            }
        }

        /// <summary>
        /// Name of the Driver.
        /// </summary>
        public override string DeviceName
        { get { return $"{OCSimulator.Name()} - {DeviceNumber}"; } }

        /// <summary>
        /// Gets what device this this driver exposes.
        /// </summary>
        public override DeviceTypes DeviceType { get; } = DeviceTypes.ObservingConditions;

        /// <summary>
        /// Gets the stored interface version to use.
        /// </summary>
        public Setting<short> InterfaceVersionSetting { get; } = new Setting<short>("InterfaceVersion", "The ASCOM Interface Version, allowed values are 1-2", 2);

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

        #endregion Class initialiser

        #region Common properties and methods.

        public override void Connect()
        {
            if(!IsConnected)
            {
                OCSimulator.Connect();
            }
            base.Connect();
        }

        public override void Disconnect()
        {
            if (IsConnected)
            {
                OCSimulator.Disconnect();
            }
            base.Disconnect();
        }

        public override bool Connected
        {
            get
            {
                return base.Connected;
            }
            set
            {
                if (value)
                {
                    if (!IsConnected)
                    {
                        OCSimulator.Connect();
                    }
                }
                else
                {
                    if (!IsConnected)
                    {
                        OCSimulator.Disconnect();
                    }
                }
                base.Connected = value;
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
                    return OCSimulator.Description();
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
                    return OCSimulator.DriverInfo();
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
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public override string Name
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    return OCSimulator.Name();
                }, DeviceType, MemberNames.Name, "Get");
            }
        }

        #endregion Common properties and methods.

        #region IObservingConditionsV1 Implementation

        public double AveragePeriod
        {
            get { return OCSimulator.AveragePeriodGet(); }
            set { OCSimulator.AveragePeriodSet(value); }
        }

        public double CloudCover
        {
            get { return OCSimulator.CloudCover(); }
        }

        public double DewPoint
        {
            get { return OCSimulator.DewPoint(); }
        }

        public double Humidity
        {
            get { return OCSimulator.Humidity(); }
        }

        public double Pressure
        {
            get { return OCSimulator.Pressure(); }
        }

        public double RainRate
        {
            get { return OCSimulator.RainRate(); }
        }

        public void Refresh()
        {
            OCSimulator.Refresh();
        }

        public string SensorDescription(string PropertyName)
        {
            return OCSimulator.SensorDescription(PropertyName);
        }

        public double SkyBrightness
        {
            get { return OCSimulator.SkyBrightness(); }
        }

        public double SkyQuality
        {
            get { return OCSimulator.SkyQuality(); }
        }

        public double StarFWHM
        {
            get { return OCSimulator.StarFWHM(); }
        }

        public double SkyTemperature
        {
            get { return OCSimulator.SkyTemperature(); }
        }

        public double Temperature
        {
            get { return OCSimulator.Temperature(); }
        }

        public double TimeSinceLastUpdate(string PropertyName)
        {
            return OCSimulator.TimeSinceLastUpdate(PropertyName);
        }

        public double WindDirection
        {
            get { return OCSimulator.WindDirection(); }
        }

        public double WindGust
        {
            get { return OCSimulator.WindGust(); }
        }

        public double WindSpeed
        {
            get { return OCSimulator.WindSpeed(); }
        }

        #endregion ObservingConditions Implementation

        #region IObservingConditionsV2 implementation

        /// <summary>
        /// Return the device's operational state in one call
        /// </summary>
        public List<StateValue> DeviceState
        {
            get
            {
                // Create an array list to hold the IStateValue entries
                List<StateValue> deviceState = new List<StateValue>();

                try { deviceState.Add(new StateValue(nameof(IObservingConditionsV2.CloudCover), CloudCover)); } catch { }
                try { deviceState.Add(new StateValue(nameof(IObservingConditionsV2.DewPoint), DewPoint)); } catch { }
                try { deviceState.Add(new StateValue(nameof(IObservingConditionsV2.Humidity), Humidity)); } catch { }
                try { deviceState.Add(new StateValue(nameof(IObservingConditionsV2.Pressure), Pressure)); } catch { }
                try { deviceState.Add(new StateValue(nameof(IObservingConditionsV2.RainRate), RainRate)); } catch { }
                try { deviceState.Add(new StateValue(nameof(IObservingConditionsV2.SkyBrightness), SkyBrightness)); } catch { }
                try { deviceState.Add(new StateValue(nameof(IObservingConditionsV2.SkyQuality), SkyQuality)); } catch { }
                try { deviceState.Add(new StateValue(nameof(IObservingConditionsV2.SkyTemperature), SkyTemperature)); } catch { }
                try { deviceState.Add(new StateValue(nameof(IObservingConditionsV2.StarFWHM), StarFWHM)); } catch { }
                try { deviceState.Add(new StateValue(nameof(IObservingConditionsV2.Temperature), Temperature)); } catch { }
                try { deviceState.Add(new StateValue(nameof(IObservingConditionsV2.WindDirection), WindDirection)); } catch { }
                try { deviceState.Add(new StateValue(nameof(IObservingConditionsV2.WindSpeed), WindSpeed)); } catch { }
                try { deviceState.Add(new StateValue(nameof(IObservingConditionsV2.WindGust), WindGust)); } catch { }
                try { deviceState.Add(new StateValue(DateTime.Now)); } catch { }

                return deviceState;
            }
        }
#endregion
    }
}