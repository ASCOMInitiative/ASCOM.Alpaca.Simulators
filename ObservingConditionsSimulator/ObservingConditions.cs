using ASCOM.Common;
using ASCOM.Common.DeviceInterfaces;
using ASCOM.Common.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ASCOM.Simulators
{
    /// <summary>
    /// ASCOM ObservingConditions Driver for Observing Conditions OCSimulator.
    /// </summary>
#if ASCOM_7_PREVIEW
    public class ObservingConditions : IObservingConditionsV2, IAlpacaDevice, ISimulation
#else
    public class ObservingConditions : IObservingConditions, IAlpacaDevice, ISimulation
#endif
    {
        #region Variables and Constants

        internal static ILogger TL; // Private variable to hold the trace logger object (creates a diagnostic log file with information that you specify)
        private bool clientIsConnected;

        private const string UNIQUE_ID_PROFILE_NAME = "UniqueID";

        #endregion Variables and Constants

        #region Class initialiser

        /// <summary>
        /// Initializes a new instance of the <see cref="OCSimulator"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public ObservingConditions(int deviceNumber, ILogger logger, IProfile profile)
        {
            try
            {
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

                OCSimulator.LogMessage("ObservingConditions", "Completed initialisation");
            }
            catch (Exception ex)
            {
                OCSimulator.LogMessage("ObservingConditions", ex.ToString());
            }
        }

        public string DeviceName { get => Name; }
        public int DeviceNumber { get; private set; }
        public string UniqueID { get; private set; }

        #endregion Class initialiser

        #region Common properties and methods.

        /// <summary>
        /// Displays the Setup Dialogue form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
        /// </summary>
        public void SetupDialog()
        {
        }

        public IList<string> SupportedActions
        {
            get { return OCSimulator.SupportedActions(); }
        }

        public string Action(string actionName, string actionParameters)
        { return OCSimulator.Action(actionName, actionParameters); }

        public void CommandBlind(string command, bool raw)
        {
            OCSimulator.CommandBlind(command, raw);
        }

        public bool CommandBool(string command, bool raw)
        {
            return OCSimulator.CommandBool(command, raw);
        }

        public string CommandString(string command, bool raw)
        {
            return OCSimulator.CommandString(command, raw);
        }

        public void Dispose()
        {
        }

        public bool Connected
        {
            get
            {
                //return OCSimulator.IsConnected();
                return clientIsConnected;
            }
            set
            {
                clientIsConnected = value;
                if (value) OCSimulator.Connect();
                else OCSimulator.Disconnect();
            }
        }

        public string Description
        {
            get { return OCSimulator.Description(); }
        }

        public string DriverInfo
        {
            get { return OCSimulator.DriverInfo(); }
        }

        public string DriverVersion
        {
            get { return OCSimulator.DriverVersion(); }
        }

        public short InterfaceVersion
        {
            get { return OCSimulator.InterfaceVersion(); }
        }

        public string Name
        {
            get { return OCSimulator.Name(); }
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

#if ASCOM_7_PREVIEW

        public void Connect()
        {
            Connected = true;
        }

        public void Disconnect()
        {
            Connected = false;
        }

        public bool Connecting
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Return the device's operational state in one call
        /// </summary>
        public IList<IStateValue> DeviceState
        {
            get
            {
                // Create an array list to hold the IStateValue entries
                List<IStateValue> deviceState = new List<IStateValue>();

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
#endif
#endregion




        #region ISimulation

        public void ResetSettings()
        {
            OCSimulator.ClearProfile();
        }

        public string GetXMLProfile()
        {
            return OCSimulator.driverProfile.GetProfile();
        }

        #endregion ISimulation
    }
}