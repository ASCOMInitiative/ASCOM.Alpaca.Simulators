using ASCOM;
using ASCOM.Alpaca.Responses;
using ASCOM.Standard.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace ASCOMSimulators
{
    /// <summary>
    /// ASCOM CoverCalibrator Driver for Simulator.
    /// </summary>

    public class CoverCalibratorSimulator : ICoverCalibratorV1, IAlpacaDevice
    {
        // Private simulator constants
        private const string DRIVER_DESCRIPTION = "ASCOM CoverCalibrator Simulator"; // Driver description that displays in the ASCOM Chooser.

        public const double SYNCHRONOUS_BEHAVIOUR_LIMIT = 0.5; // Threshold (seconds) above which state changes will be handled asynchronously

        private readonly int DeviceNumber = 0;

        /// <summary>
        /// Resets all stored device settings
        /// </summary>
        public void ResetSettings()
        {
            Profile?.Clear();
        }

        // Persistence constants
        private const string TRACE_STATE_PROFILE_NAME = "Trace State"; private const bool TRACE_STATE_DEFAULT = false;

        private const string MAX_BRIGHTNESS_PROFILE_NAME = "Maximum Brightness"; private const string MAX_BRIGHTNESS_DEFAULT = "100"; // Number of different brightness states
        private const string CALIBRATOR_STABILISATION_TIME_PROFILE_NAME = "Calibrator Stabilisation Time"; private const double CALIBRATOR_STABLISATION_TIME_DEFAULT = 2.0; // Seconds
        private const string CALIBRATOR_INITIALISATION_STATE_PROFILE_NAME = "Calibrator Initialisation State"; private const CalibratorStatus CALIBRATOR_INITIALISATION_STATE_DEFAULT = CalibratorStatus.Off;
        private const string COVER_OPENING_TIME_PROFILE_NAME = "Cover Opening Time"; private const double COVER_OPENING_TIME_DEFAULT = 5.0; // Seconds
        private const string COVER_INITIALISATION_STATE_PROFILE_NAME = "Cover Initialisation State"; private const CoverStatus COVER_INITIALISATION_STATE_DEFAULT = CoverStatus.Closed;
        private const string UNIQUE_ID_PROFILE_NAME = "UniqueID";

        // Simulator state variables
        private CoverStatus coverState; // The current cover status

        private CalibratorStatus calibratorState; // The current calibrator status
        private int brightnessValue; // The current brightness of the calibrator
        private CoverStatus targetCoverState; // The final cover status at the end of the current asynchronous command
        private CalibratorStatus targetCalibratorStatus; // The final calibrator status at the end of the current asynchronous command

        // User configuration variables
        public CalibratorStatus CalibratorStateInitialisationValue;

        public CoverStatus CoverStateInitialisationValue;
        public int MaxBrightnessValue;
        public double CoverOpeningTimeValue;
        public double CalibratorStablisationTimeValue;

        // Simulator components
        internal ILogger TL; // ASCOM Trace Logger component

        internal IProfile Profile; //Access to device settings

        private readonly System.Timers.Timer coverTimer;
        private readonly System.Timers.Timer calibratorTimer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Simulator"/> class.
        /// </summary>
        /// <param name="deviceNumber">The instance number of this driver. If there is only one this should be 0</param>
        /// <param name="logger">The logger instance to use</param>
        /// <param name="profile">A profile to store settings</param>
        public CoverCalibratorSimulator(int deviceNumber, ILogger logger, IProfile profile)
        {
            try
            {
                DeviceNumber = deviceNumber;

                // Initialise the driver trace logger
                TL = logger;
                Profile = profile;

                // Read device configuration from the ASCOM Profile store, this also sets the trace logger enabled state
                ReadProfile();
                TL.LogInformation($"CoverCalibrator {deviceNumber} - Starting initialisation");

                //This should be replaced by the next bit of code but is semi-unique as a default.
                string UniqueID = Name + deviceNumber.ToString();
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
                catch(Exception ex)
                {
                    TL.LogError($"CoverCalibrator {deviceNumber} - {ex.Message}");
                }

                TL.LogInformation($"CoverCalibrator {deviceNumber} - UUID of {UniqueID}");

                Configuration = new AlpacaConfiguredDevice(Name, "CoverCalibrator", deviceNumber, UniqueID);

                // Initialise remaining components
                calibratorTimer = new System.Timers.Timer();
                if (CalibratorStablisationTimeValue > 0.0)
                {
                    calibratorTimer.Interval = Convert.ToInt32(CalibratorStablisationTimeValue * 1000.0); // Set the timer interval in milliseconds from the stabilisation time in seconds
                }
                calibratorTimer.Elapsed += CalibratorTimer_Tick;
                TL.LogInformation($"CoverCalibrator {deviceNumber} - Set calibrator timer to: {calibratorTimer.Interval}ms.");

                coverTimer = new System.Timers.Timer();
                if (CoverOpeningTimeValue > 0.0)
                {
                    coverTimer.Interval = Convert.ToInt32(CoverOpeningTimeValue * 1000.0); // Set the timer interval in milliseconds from the opening time in seconds
                }
                coverTimer.Elapsed += CoverTimer_Tick;
                TL.LogInformation($"CoverCalibrator {deviceNumber} - Set cover timer to: {coverTimer.Interval}ms.");

                // Initialise internal start-up values
                IsConnected = false; // Initialise connected to false
                brightnessValue = 0; // Set calibrator brightness to 0 i.e. off
                coverState = CoverStateInitialisationValue; // Set the cover state as set by the user
                calibratorState = CalibratorStateInitialisationValue; // Set the calibration state as set by the user

                TL.LogInformation($"CoverCalibrator {deviceNumber} - Completed initialisation");
            }
            catch (Exception ex)
            {
                // Create a message to the user
                string message = $"Exception while creating CoverCalibrator simulator: \r\n{ex.Message}";

                // Attempt to log the message
                try
                {
                    TL.LogInformation($"CoverCalibrator {deviceNumber} - {message}");
                }
                catch { } // Ignore any errors while attempting to log the error

                // Display the error to the user
            }
        }

        private void CoverTimer_Tick(object sender, EventArgs e)
        {
            coverState = targetCoverState;
            coverTimer.Stop();
            TL.LogVerbose($"CoverCalibrator {DeviceNumber} - End of cover asynchronous event - cover state is now: {coverState}.");
        }

        private void CalibratorTimer_Tick(object sender, EventArgs e)
        {
            calibratorState = targetCalibratorStatus;
            calibratorTimer.Stop();
            TL.LogVerbose($"CoverCalibrator {DeviceNumber} - End of cover asynchronous event - cover state is now: {coverState}.");
        }

        #region Common properties and methods.

        public IList<string> SupportedActions
        {
            get
            {
                TL.LogVerbose($"CoverCalibrator {DeviceNumber} - SupportedActions - Returning empty list");
                return new List<string>();
            }
        }

        public string Action(string actionName, string actionParameters)
        {
            LogVerbose("", "Action {0}, parameters {1} not implemented", actionName, actionParameters);
            throw new ASCOM.ActionNotImplementedException("Action " + actionName + " is not implemented by this driver");
        }

        public void CommandBlind(string command, bool raw)
        {
            CheckConnected("CommandBlind");
            throw new ASCOM.MethodNotImplementedException("CommandBlind");
        }

        public bool CommandBool(string command, bool raw)
        {
            CheckConnected("CommandBool");
            throw new ASCOM.MethodNotImplementedException("CommandBool");
        }

        public string CommandString(string command, bool raw)
        {
            CheckConnected("CommandString");
            throw new ASCOM.MethodNotImplementedException("CommandString");
        }

        public void Dispose()
        {
            //Do not cleanup the tracelogger or profile. This is handled at a higher level.
        }

        public bool Connected
        {
            get
            {
                LogVerbose("Connected", "Get {0}", IsConnected);
                return IsConnected;
            }
            set
            {
                LogVerbose("Connected", "Set {0}", value);
                if (value == IsConnected) return; // We are already in the required state so ignore the request

                if (value)
                {
                    IsConnected = true;
                }
                else
                {
                    IsConnected = false;
                }
            }
        }

        public string Description
        {
            get
            {
                LogVerbose("Description Get", DRIVER_DESCRIPTION);
                return DRIVER_DESCRIPTION;
            }
        }

        public string DriverInfo
        {
            get
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

                string driverInfo = $"CoverCalibrator driver version: {fvi.FileVersion}.";
                LogVerbose("DriverInfo Get", driverInfo);
                return driverInfo;
            }
        }

        public string DriverVersion
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                string driverVersion = $"{version.Major}.{version.Minor}";
                LogVerbose("DriverVersion Get", driverVersion);
                return driverVersion;
            }
        }

        public short InterfaceVersion
        {
            // set by the driver wizard
            get
            {
                LogVerbose("InterfaceVersion Get", "1");
                return Convert.ToInt16("1");
            }
        }

        public string Name
        {
            get
            {
                string name = "CoverCalibrator Simulator";
                LogVerbose("Name Get", name);
                return name;
            }
        }

        #endregion Common properties and methods.

        #region ICoverCalibrator Implementation

        /// <summary>
        /// Returns the state of the device cover, if present, otherwise returns "NotPresent"
        /// </summary>
        public CoverStatus CoverState
        {
            get
            {
                if (IsConnected)
                {
                    LogVerbose("CoverState Get", coverState.ToString());
                    return coverState;
                }
                else
                {
                    LogVerbose("CoverState Get", $"Not connected, returning CoverStatus.Unknown");
                    return CoverStatus.Unknown;
                }
            }
        }

        /// <summary>
        /// Initiates cover opening if a cover is present
        /// </summary>
        public void OpenCover()
        {
            if (coverState == CoverStatus.NotPresent) throw new MethodNotImplementedException("This device has no cover capability.");

            if (!IsConnected) throw new NotConnectedException("The simulator is not connected, the OpenCover method is not available.");

            if (CoverOpeningTimeValue <= SYNCHRONOUS_BEHAVIOUR_LIMIT) // Synchronous behaviour
            {
                coverState = CoverStatus.Moving;
                WaitFor(CoverOpeningTimeValue);
                LogVerbose("OpenCover", $"Cover opened synchronously in {CoverOpeningTimeValue} seconds.");
                coverState = CoverStatus.Open;
            }
            else
            {
                coverState = CoverStatus.Moving;
                targetCoverState = CoverStatus.Open;
                coverTimer.Start();
                LogVerbose("OpenCover", $"Starting asynchronous cover opening for {CoverOpeningTimeValue} seconds.");
            }
        }

        /// <summary>
        /// Initiates cover closing if a cover is present
        /// </summary>
        public void CloseCover()
        {
            if (coverState == CoverStatus.NotPresent) throw new MethodNotImplementedException("This device has no cover capability.");

            if (!IsConnected) throw new NotConnectedException("The simulator is not connected, the CloseCover method is not available.");

            if (CoverOpeningTimeValue <= SYNCHRONOUS_BEHAVIOUR_LIMIT) // Synchronous behaviour
            {
                coverState = CoverStatus.Moving;
                WaitFor(CoverOpeningTimeValue);
                LogVerbose("CloseCover", $"Cover closed synchronously in {CoverOpeningTimeValue} seconds.");
                coverState = CoverStatus.Closed;
            }
            else
            {
                coverState = CoverStatus.Moving;
                targetCoverState = CoverStatus.Closed;
                coverTimer.Start();
                LogVerbose("CloseCover", $"Starting asynchronous cover closing for {CoverOpeningTimeValue} seconds.");
            }
        }

        /// <summary>
        /// Stops any cover movement that may be in progress if a cover is present and cover movement can be interrupted.
        /// </summary>
        public void HaltCover()
        {
            if (coverState == CoverStatus.NotPresent) throw new MethodNotImplementedException("This device has no cover capability.");

            if (!IsConnected) throw new NotConnectedException("The simulator is not connected, the HaltCover method is not available.");

            if (CoverOpeningTimeValue <= SYNCHRONOUS_BEHAVIOUR_LIMIT) throw new MethodNotImplementedException("Cover movement methods are synchronous and cannot be interrupted.");

            coverTimer.Stop();
            coverState = CoverStatus.Unknown;

            LogVerbose("HaltCover", $"Cover halted and cover state set to {CoverStatus.Unknown}");
        }

        /// <summary>
        /// Returns the state of the calibration device, if present, otherwise returns "NotPresent"
        /// </summary>
        public CalibratorStatus CalibratorState
        {
            get
            {
                if (IsConnected)
                {
                    LogVerbose("CalibratorState Get", calibratorState.ToString());
                    return calibratorState;
                }
                else
                {
                    LogVerbose("CalibratorState Get", $"Not connected, returning CalibratorState.Unknown");
                    return CalibratorStatus.Unknown;
                }
            }
        }

        /// <summary>
        /// Returns the current calibrator brightness in the range 0 (completely off) to <see cref="MaxBrightness"/> (fully on)
        /// </summary>
        public int Brightness
        {
            get
            {
                if (calibratorState == CalibratorStatus.NotPresent) throw new PropertyNotImplementedException("Brightness", false);

                if (!IsConnected) throw new NotConnectedException("The simulator is not connected, the Brightness property is not available.");

                LogVerbose("Brightness Get", brightnessValue.ToString());
                return brightnessValue;
            }
        }

        /// <summary>
        /// The Brightness value that makes the calibrator deliver its maximum illumination.
        /// </summary>
        public int MaxBrightness
        {
            get
            {
                if (calibratorState == CalibratorStatus.NotPresent) throw new PropertyNotImplementedException("MaxBrightness", false);

                if (!IsConnected) throw new NotConnectedException("The simulator is not connected, the MaxBrightness property is not available.");

                LogVerbose("MaxBrightness Get", MaxBrightnessValue.ToString());
                return MaxBrightnessValue;
            }
        }

        /// <summary>
        /// Turns the calibrator on at the specified brightness if the device has calibration capability
        /// </summary>
        /// <param name="Brightness"></param>
        public void CalibratorOn(int Brightness)
        {
            if (calibratorState == CalibratorStatus.NotPresent) throw new MethodNotImplementedException("This device has no calibrator capability.");

            if (!IsConnected) throw new NotConnectedException("The simulator is not connected, the CalibratorOn method is not available.");

            if ((Brightness < 0) | (Brightness > MaxBrightnessValue)) throw new InvalidValueException("CalibratorOn", Brightness.ToString(), $"0 to {MaxBrightnessValue}");

            brightnessValue = Brightness; // Set the assigned brightness

            if (CalibratorStablisationTimeValue <= SYNCHRONOUS_BEHAVIOUR_LIMIT) // Synchronous behaviour
            {
                calibratorState = CalibratorStatus.NotReady;
                WaitFor(CalibratorStablisationTimeValue);
                LogVerbose("CalibratorOn", $"Calibrator turned on synchronously in {CalibratorStablisationTimeValue} seconds.");
                calibratorState = CalibratorStatus.Ready;
            }
            else // Asynchronous behaviour
            {
                calibratorState = CalibratorStatus.NotReady;
                targetCalibratorStatus = CalibratorStatus.Ready;
                calibratorTimer.Start();
                LogVerbose("CalibratorOn", $"Starting asynchronous calibrator turn on for {CalibratorStablisationTimeValue} seconds.");
            }
        }

        /// <summary>
        /// Turns the calibrator off if the device has calibration capability
        /// </summary>
        public void CalibratorOff()
        {
            if (calibratorState == CalibratorStatus.NotPresent) throw new MethodNotImplementedException("This device has no calibrator capability.");

            if (!IsConnected) throw new NotConnectedException("The simulator is not connected, the CalibratorOff method is not available.");

            brightnessValue = 0; // Set the brightness to zero per the ASCOM specification

            if (CalibratorStablisationTimeValue <= SYNCHRONOUS_BEHAVIOUR_LIMIT) // Synchronous behaviour
            {
                calibratorState = CalibratorStatus.NotReady;
                WaitFor(CalibratorStablisationTimeValue);
                LogVerbose("CalibratorOff", $"Calibrator turned off synchronously in {CalibratorStablisationTimeValue} seconds.");
                calibratorState = CalibratorStatus.Off;
            }
            else // Asynchronous behaviour
            {
                calibratorState = CalibratorStatus.NotReady;
                targetCalibratorStatus = CalibratorStatus.Off;
                calibratorTimer.Start();
                LogVerbose("CalibratorOff", $"Starting asynchronous calibrator turn off for {CalibratorStablisationTimeValue} seconds.");
            }
        }

        #endregion ICoverCalibrator Implementation

        #region Alpaca Information
        public AlpacaConfiguredDevice Configuration
        {
            get;
            private set;
        }
        #endregion

        #region Private properties and methods

        // here are some useful properties and methods that can be used as required
        // to help with driver development

        /// <summary>
        /// Returns true if there is a valid connection to the driver hardware
        /// </summary>
        private bool IsConnected { get; set; }

        /// <summary>
        /// Use this function to throw an exception if we aren't connected to the hardware
        /// </summary>
        /// <param name="message"></param>
        private void CheckConnected(string message)
        {
            if (!IsConnected)
            {
                throw new NotConnectedException(message);
            }
        }

        /// <summary>
        /// Read the device configuration from the ASCOM Profile store
        /// </summary>
        internal void ReadProfile()
        {
            var temp = Profile.GetValue(TRACE_STATE_PROFILE_NAME, TRACE_STATE_DEFAULT.ToString());
            MaxBrightnessValue = Convert.ToInt32(Profile.GetValue(MAX_BRIGHTNESS_PROFILE_NAME, MAX_BRIGHTNESS_DEFAULT));
            CalibratorStablisationTimeValue = Convert.ToDouble(Profile.GetValue(CALIBRATOR_STABILISATION_TIME_PROFILE_NAME, CALIBRATOR_STABLISATION_TIME_DEFAULT.ToString()));
            if (!Enum.TryParse<CalibratorStatus>(Profile.GetValue(CALIBRATOR_INITIALISATION_STATE_PROFILE_NAME, CALIBRATOR_INITIALISATION_STATE_DEFAULT.ToString()), out CalibratorStateInitialisationValue))
            {
                CalibratorStateInitialisationValue = CALIBRATOR_INITIALISATION_STATE_DEFAULT;
            }
            CoverOpeningTimeValue = Convert.ToDouble(Profile.GetValue(COVER_OPENING_TIME_PROFILE_NAME, COVER_OPENING_TIME_DEFAULT.ToString()));
            if (!Enum.TryParse<CoverStatus>(Profile.GetValue(COVER_INITIALISATION_STATE_PROFILE_NAME, COVER_INITIALISATION_STATE_DEFAULT.ToString()), out CoverStateInitialisationValue))
            {
                CoverStateInitialisationValue = COVER_INITIALISATION_STATE_DEFAULT;
            }
        }

        /// <summary>
        /// Write the device configuration to the  ASCOM  Profile store
        /// </summary>
        public void WriteProfile()
        {
            Profile.WriteValue(MAX_BRIGHTNESS_PROFILE_NAME, MaxBrightnessValue.ToString());
            Profile.WriteValue(CALIBRATOR_STABILISATION_TIME_PROFILE_NAME, CalibratorStablisationTimeValue.ToString());
            Profile.WriteValue(CALIBRATOR_INITIALISATION_STATE_PROFILE_NAME, CalibratorStateInitialisationValue.ToString());
            Profile.WriteValue(COVER_OPENING_TIME_PROFILE_NAME, CoverOpeningTimeValue.ToString());
            Profile.WriteValue(COVER_INITIALISATION_STATE_PROFILE_NAME, CoverStateInitialisationValue.ToString());
        }

        /// <summary>
        /// Log helper function that takes formatted strings and arguments
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        internal void LogVerbose(string identifier, string message, params object[] args)
        {
            var msg = string.Format(message, args);

            TL.LogVerbose($"CoverCalibrator {DeviceNumber} - {identifier} - {msg}");
        }

        /// <summary>
        /// Wait for a given number of seconds while keeping the Windows message pump running
        /// </summary>
        /// <param name="duration">Wait duration (seconds)</param>
        private void WaitFor(double duration)
        {
            DateTime endTime = DateTime.Now.AddSeconds(duration); // Calculate the end time
            do
            {
                System.Threading.Thread.Sleep(20);
                //Application.DoEvents();
            } while (DateTime.Now < endTime);
        }

        #endregion Private properties and methods
    }
}