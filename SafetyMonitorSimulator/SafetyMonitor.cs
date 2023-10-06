using ASCOM.Common;
using ASCOM.Common.DeviceInterfaces;
using ASCOM.Common.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;

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

#if ASCOM_7_PREVIEW
    public class SafetyMonitor : ISafetyMonitorV3, IDisposable, IAlpacaDevice, ISimulation
#else
    public class SafetyMonitor : ISafetyMonitor, IDisposable, IAlpacaDevice, ISimulation
#endif
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

        /// <summary>
        /// Driver interface version
        /// </summary>
#if ASCOM_7_PREVIEW
        private const short interfaceVersion = 3;
#else
        private const short interfaceVersion = 2;
#endif

        /// <summary>
        /// ASCOM DeviceID (COM ProgID) for this driver.
        /// The DeviceID is used by ASCOM applications to load the driver at runtime.
        /// </summary>
        private const string sCsDriverId = "ASCOM.Simulator.SafetyMonitor";

        /// <summary>
        /// Driver description that displays in the ASCOM Chooser.
        /// </summary>
        private const string sCsDriverDescription = "ASCOM Simulator SafetyMonitor Driver";

        private static bool _isSafe;

        private ILogger Logger;
        private IProfile Profile;

#endregion Constants

        #region ISafetyMonitor Public Members

        //
        // PUBLIC COM INTERFACE ISafetyMonitor IMPLEMENTATION
        //

        /// <summary>
        /// Initializes a new instance of the <see cref="SafetyMonitor"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public SafetyMonitor(int deviceNumber, ILogger logger, IProfile profile)
        {
            Profile = profile;
            Logger = logger;

            DeviceNumber = deviceNumber;

            if (CheckSafetyMonitorKeyValue())
            {
                GetProfileSetting();
            }

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

        public string DeviceName { get => Name; }
        public int DeviceNumber { get; private set; }
        public string UniqueID { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SafetyMonitor"/> is connected.
        /// </summary>
        /// <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
        public bool Connected { get; set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get { return description; }
        }

        /// <summary>
        /// Gets the driver info.
        /// </summary>
        /// <value>The driver info.</value>
        public string DriverInfo
        {
            get { return driverInfo; }
        }

        /// <summary>
        /// Gets the driver version.
        /// </summary>
        /// <value>The driver version.</value>
        public string DriverVersion
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                return $"{version.Major}.{version.Minor}";
            }
        }

        /// <summary>
        /// Gets the interface version.
        /// </summary>
        /// <value>The interface version.</value>
        public short InterfaceVersion
        {
            get { return interfaceVersion; }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return name; }
        }

        public void Dispose()
        {
        }

        void IDisposable.Dispose()
        {
            Dispose();
        }

        /// <summary>
        /// Return the condition of the SafetyMonitor
        /// </summary>
        /// <value>State of the Monitor</value>
        public bool IsSafe
        {
            get
            {
                if (!Connected)
                {
                    return false;
                }
                return _isSafe;
            }
        }

        /// <summary>
        /// Invokes the specified device-specific action.
        /// </summary>
        public string Action(string actionName, string actionParameters)
        {
            throw new MethodNotImplementedException("Action");
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and does
        /// not wait for a response. Optionally, protocol framing
        /// characters may be added to the string before transmission.
        /// </summary>
        public void CommandBlind(string command, bool raw)
        {
            throw new MethodNotImplementedException("CommandBlind");
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits
        /// for a boolean response. Optionally, protocol framing
        /// characters may be added to the string before transmission.
        /// </summary>
        public bool CommandBool(string command, bool raw)
        {
            throw new MethodNotImplementedException("CommandBool");
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits
        /// for a string response. Optionally, protocol framing
        /// characters may be added to the string before transmission.
        /// </summary>
        public string CommandString(string command, bool raw)
        {
            throw new MethodNotImplementedException("CommandString");
        }

        /// <summary>
        /// Gets the supported actions.
        /// </summary>
        public IList<string> SupportedActions
        {
            // no supported actions, return empty array
            get { return new List<string>(); }
        }

        #endregion ISafetyMonitor Public Members

#region ISafetyMonitorV3 members
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

        public IList<IStateValue> DeviceState
        {
            get
            {
                List<IStateValue> deviceState = new List<IStateValue>();
                try { deviceState.Add(new StateValue(nameof(ISafetyMonitorV3.IsSafe), IsSafe)); } catch { }
                try { deviceState.Add(new StateValue(DateTime.Now)); } catch { }

                return deviceState;
            }
        }
#endif
#endregion

        #region SafetyMonitor Private Members

        /// <summary>
        /// Validates the profile key and the value
        /// </summary>
        private bool CheckSafetyMonitorKeyValue()
        {
            string s = Profile.GetValue("SafetyMonitor", "false").ToUpperInvariant();
            if (s != "TRUE" & s != "FALSE")
            {
                //found something wrong, delete evertyhing
                DeleteProfileSettings();
            }
            return true;
        }

        /// <summary>
        /// Loads a specific setting from the profile
        /// </summary>
        private void GetProfileSetting()
        {
            string s = Profile.GetValue("SafetyMonitor", false.ToString());

            if (bool.TryParse(s, out bool res))
            {
                _isSafe = res;
            }
            else
            {
                // A bad value is not very safe now is it?
                _isSafe = false;
            }
        }

        /// <summary>
        /// Used to set the IsSafe value in the profile. Public so the settings UI has access.
        /// </summary>
        /// <param name="value">If the driver should report safe</param>
        public void SetIsSafeProfile(bool value)
        {
            Profile.WriteValue("SafetyMonitor", value.ToString());

            _isSafe = value;
        }

        /// <summary>
        /// Reset settings to default.
        /// </summary>
        public void ResetSettings()
        {
            Profile.Clear();
        }

        public string GetXMLProfile()
        {
            return Profile.GetProfile();
        }

        /// <summary>
        /// Delete all settings io the profile for this driver ID
        /// </summary>
        private void DeleteProfileSettings()
        {
            Profile.Clear();
        }

        #endregion SafetyMonitor Private Members
    }
}