using ASCOM.Common;
using ASCOM.Common.DeviceInterfaces;
using ASCOM.Common.Interfaces;
using OmniSim.BaseDriver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace ASCOM.Simulators
{
    /// <summary>
    /// ASCOM CoverCalibrator Driver for Simulator.
    /// </summary>
    public class CoverCalibratorSimulator : OmniSim.BaseDriver.Driver, ICoverCalibratorV2, IAlpacaDevice, ISimulation
    {
        // Private simulator constants
        private const string SafeName = "Alpaca CoverCalibrator Simulator"; // Driver description that displays in the ASCOM Chooser.

        public const double SYNCHRONOUS_BEHAVIOUR_LIMIT = 0.5; // Threshold (seconds) above which state changes will be handled asynchronously

        public int DeviceNumber
        {
            get;
            private set;
        }

        /// <summary>
        /// Resets all stored device settings
        /// </summary>
        public void ResetSettings()
        {
            Profile?.Clear();
            ReadProfile();
        }

        public string GetXMLProfile()
        {
            return Profile.GetProfile();
        }

        /// <summary>
        /// Name of the Driver.
        /// </summary>
        public override string DeviceName { get { return $"{SafeName} - {DeviceNumber}"; } }

        /// <summary>
        /// Gets what device this this driver exposes.
        /// </summary>
        public override DeviceTypes DeviceType { get; } = DeviceTypes.CoverCalibrator;

        /// <summary>
        /// Gets the stored interface version to use.
        /// </summary>
        public Setting<short> InterfaceVersionSetting { get; } = new Setting<short>("InterfaceVersion", "The ASCOM Interface Version, allowed values are 1-2", 2);

        // Persistence constants
        private const string TRACE_STATE_PROFILE_NAME = "Trace State"; private const bool TRACE_STATE_DEFAULT = false;

        private const string UNIQUE_ID_PROFILE_NAME = "UniqueID";

        // Simulator state variables
        private CoverStatus coverState; // The current cover status

        private CalibratorStatus calibratorState; // The current calibrator status
        private int brightnessValue; // The current brightness of the calibrator
        private CoverStatus targetCoverState; // The final cover status at the end of the current asynchronous command
        private CalibratorStatus targetCalibratorStatus; // The final calibrator status at the end of the current asynchronous command

        // User configuration variables
        /*public CalibratorStatus CalibratorStateInitialisationValue;

        public CoverStatus CoverStateInitialisationValue;
        public double CoverOpeningTimeValue;
        public double CalibratorStablisationTimeValue;*/

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
                UniqueID = SafeName + deviceNumber.ToString();
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
                    TL.LogError($"CoverCalibrator {deviceNumber} - {ex.Message}");
                }

                TL.LogInformation($"CoverCalibrator {deviceNumber} - UUID of {UniqueID}");

                // Initialise remaining components
                calibratorTimer = new System.Timers.Timer();
                if (CalibratorStablisationTime.Value > 0.0)
                {
                    calibratorTimer.Interval = Convert.ToInt32(CalibratorStablisationTime.Value * 1000.0); // Set the timer interval in milliseconds from the stabilisation time in seconds
                }
                calibratorTimer.Elapsed += CalibratorTimer_Tick;
                TL.LogInformation($"CoverCalibrator {deviceNumber} - Set calibrator timer to: {calibratorTimer.Interval}ms.");

                coverTimer = new System.Timers.Timer();
                if (CoverOpeningTime.Value > 0.0)
                {
                    coverTimer.Interval = Convert.ToInt32(CoverOpeningTime.Value * 1000.0); // Set the timer interval in milliseconds from the opening time in seconds
                }
                coverTimer.Elapsed += CoverTimer_Tick;
                TL.LogInformation($"CoverCalibrator {deviceNumber} - Set cover timer to: {coverTimer.Interval}ms.");

                // Initialise internal start-up values
                IsConnected = false; // Initialise connected to false
                brightnessValue = 0; // Set calibrator brightness to 0 i.e. off

                calibratorState = CalibratorStatus.Off;
                if (Enum.TryParse<CalibratorStatus>(CalibratorStateInitialisation.Value, out CalibratorStatus state))
                {
                    calibratorState = state;
                }

                coverState = CoverStatus.Closed;
                if (Enum.TryParse<CoverStatus>(CoverStateInitialisation.Value, out CoverStatus coverstatus))
                {
                    coverState = coverstatus;
                }

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

        #region Common properties and methods.

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
                    return "A simulator for the ASCOM Focuser API usable with Alpaca and COM";
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
                    return "ASCOM focuser simulator";
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
                    return SafeName;
                }, DeviceType, MemberNames.Name, "Get");
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

            if (CoverOpeningTime.Value <= SYNCHRONOUS_BEHAVIOUR_LIMIT) // Synchronous behaviour
            {
                coverState = CoverStatus.Moving;
                WaitFor(CoverOpeningTime.Value);
                LogVerbose("OpenCover", $"Cover opened synchronously in {CoverOpeningTime.Value} seconds.");
                coverState = CoverStatus.Open;
            }
            else
            {
                coverState = CoverStatus.Moving;
                targetCoverState = CoverStatus.Open;
                coverTimer.Start();
                LogVerbose("OpenCover", $"Starting asynchronous cover opening for {CoverOpeningTime.Value} seconds.");
            }
        }

        /// <summary>
        /// Initiates cover closing if a cover is present
        /// </summary>
        public void CloseCover()
        {
            if (coverState == CoverStatus.NotPresent) throw new MethodNotImplementedException("This device has no cover capability.");

            if (!IsConnected) throw new NotConnectedException("The simulator is not connected, the CloseCover method is not available.");

            if (CoverOpeningTime.Value <= SYNCHRONOUS_BEHAVIOUR_LIMIT) // Synchronous behaviour
            {
                coverState = CoverStatus.Moving;
                WaitFor(CoverOpeningTime.Value);
                LogVerbose("CloseCover", $"Cover closed synchronously in {CoverOpeningTime.Value} seconds.");
                coverState = CoverStatus.Closed;
            }
            else
            {
                coverState = CoverStatus.Moving;
                targetCoverState = CoverStatus.Closed;
                coverTimer.Start();
                LogVerbose("CloseCover", $"Starting asynchronous cover closing for {CoverOpeningTime.Value} seconds.");
            }
        }

        /// <summary>
        /// Stops any cover movement that may be in progress if a cover is present and cover movement can be interrupted.
        /// </summary>
        public void HaltCover()
        {
            if (coverState == CoverStatus.NotPresent) throw new MethodNotImplementedException("This device has no cover capability.");

            if (!IsConnected) throw new NotConnectedException("The simulator is not connected, the HaltCover method is not available.");

            if (CoverOpeningTime.Value <= SYNCHRONOUS_BEHAVIOUR_LIMIT) throw new MethodNotImplementedException("Cover movement methods are synchronous and cannot be interrupted.");

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

                LogVerbose("MaxBrightness Get", MaximumBrightness.Value.ToString());
                return MaximumBrightness.Value;
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

            if ((Brightness < 0) | (Brightness > MaximumBrightness.Value)) throw new InvalidValueException("CalibratorOn", Brightness.ToString(), $"0 to {MaximumBrightness.Value}");

            brightnessValue = Brightness; // Set the assigned brightness

            if (CalibratorStablisationTime.Value <= SYNCHRONOUS_BEHAVIOUR_LIMIT) // Synchronous behaviour
            {
                calibratorState = CalibratorStatus.NotReady;
                WaitFor(CalibratorStablisationTime.Value);
                LogVerbose("CalibratorOn", $"Calibrator turned on synchronously in {CalibratorStablisationTime} seconds.");
                calibratorState = CalibratorStatus.Ready;
            }
            else // Asynchronous behaviour
            {
                calibratorState = CalibratorStatus.NotReady;
                targetCalibratorStatus = CalibratorStatus.Ready;
                calibratorTimer.Start();
                LogVerbose("CalibratorOn", $"Starting asynchronous calibrator turn on for {CalibratorStablisationTime} seconds.");
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

            if (CalibratorStablisationTime.Value <= SYNCHRONOUS_BEHAVIOUR_LIMIT) // Synchronous behaviour
            {
                calibratorState = CalibratorStatus.NotReady;
                WaitFor(CalibratorStablisationTime.Value);
                LogVerbose("CalibratorOff", $"Calibrator turned off synchronously in {CalibratorStablisationTime} seconds.");
                calibratorState = CalibratorStatus.Off;
            }
            else // Asynchronous behaviour
            {
                calibratorState = CalibratorStatus.NotReady;
                targetCalibratorStatus = CalibratorStatus.Off;
                calibratorTimer.Start();
                LogVerbose("CalibratorOff", $"Starting asynchronous calibrator turn off for {CalibratorStablisationTime} seconds.");
            }
        }

        #endregion ICoverCalibrator Implementation

        #region ICoverCalibratorV2 implementation
        /// <summary>
        /// Connects to the hardware.
        /// </summary>
        public override void Connect()
        {
            base.ConnectTimer.Interval = ConnectDelay.Value;
            base.Connect();
        }

        /// <summary>
        /// Return the device's operational state in one call
        /// </summary>
        public List<StateValue> DeviceState
        {
            get
            {
                // Create an array list to hold the IStateValue entries
                List<StateValue> deviceState = new List<StateValue>();

                try { deviceState.Add(new StateValue(nameof(ICoverCalibratorV2.Brightness), Brightness)); } catch { }
                try { deviceState.Add(new StateValue(nameof(ICoverCalibratorV2.CalibratorState), CalibratorState)); } catch { }
                try { deviceState.Add(new StateValue(nameof(ICoverCalibratorV2.CalibratorChanging), CalibratorChanging)); } catch { }
                try { deviceState.Add(new StateValue(nameof(ICoverCalibratorV2.CoverState), CoverState)); } catch { }
                try { deviceState.Add(new StateValue(nameof(ICoverCalibratorV2.CoverMoving), CoverMoving)); } catch { }
                try { deviceState.Add(new StateValue(DateTime.Now)); } catch { }

                return deviceState;
            }
        }

        public bool CalibratorChanging
        {
            get
            {
                return CalibratorState == CalibratorStatus.NotReady;
            }
        }

        public bool CoverMoving
        {
            get
            {
                return CoverState == CoverStatus.Moving;
            }
        }
        #endregion

        #region Alpaca Information

        public string UniqueID { get; private set; }

        #endregion Alpaca Information

        #region Private properties and methods

        /// <summary>
        /// Gets the delay for the connect timer.
        /// </summary>
        public Setting<short> ConnectDelay { get; } = new Setting<short>("ConnectDelay", "The delay to be used for Connect() in milliseconds, allowed values are 1-30000", 1500);

        /// <summary>
        /// Gets a value.
        /// </summary>
        public Setting<int> MaximumBrightness { get; } = new Setting<int>("Maximum Brightness", "Max Brightness Range 1-2147483647 default (100)", 100);

        /// <summary>
        /// Gets a value.
        /// </summary>
        public Setting<double> CalibratorStablisationTime { get; } = new Setting<double>("Calibrator Stabilisation Time", "Max Brightness Range 1-100.0 default (2.0)", 2.0);

        /// <summary>
        /// Gets a value.
        /// </summary>
        public Setting<double> CoverOpeningTime { get; } = new Setting<double>("Cover Opening Time", "Cover Opening Time 1-100.0 default (5.0)", 5.0);

        /// <summary>
        /// Gets a value.
        /// </summary>
        public Setting<string> CalibratorStateInitialisation { get; } = new Setting<string>("Calibrator Initialisation State", "Calibrator Initialisation State, allowed values (NotPresent = 0, Off = 1, NotReady = 2, Ready = 3, Unknown = 4, Error = 5) default Off", CalibratorStatus.Off.ToString());

        /// <summary>
        /// Gets a value.
        /// </summary>
        public Setting<string> CoverStateInitialisation { get; } = new Setting<string>("Cover Initialisation State", "Cover Initialisation State, allowed values (NotPresent = 0, Closed = 1, Moving = 2, Open = 3, Unknown = 4, Error = 5) default Off", CoverStatus.Closed.ToString());

        /// <summary>
        /// Read the device configuration from the ASCOM Profile store.
        /// </summary>
        internal void ReadProfile()
        {
            var temp = Profile.GetValue(TRACE_STATE_PROFILE_NAME, TRACE_STATE_DEFAULT.ToString());
            this.MaximumBrightness.Value = this.Profile.GetSettingReturningDefault(this.MaximumBrightness);
            this.CalibratorStablisationTime.Value = this.Profile.GetSettingReturningDefault(this.CalibratorStablisationTime);
            this.CoverOpeningTime.Value = this.Profile.GetSettingReturningDefault(this.CoverOpeningTime);
            this.CalibratorStateInitialisation.Value = this.Profile.GetSettingReturningDefault(this.CalibratorStateInitialisation);
            this.CoverStateInitialisation.Value = this.Profile.GetSettingReturningDefault(this.CoverStateInitialisation);
        }

        /// <summary>
        /// Write the device configuration to the  ASCOM  Profile store
        /// </summary>
        public void WriteProfile()
        {
            Profile.SetSetting(ConnectDelay);
            Profile.SetSetting(MaximumBrightness);
            Profile.SetSetting(CalibratorStablisationTime);
            Profile.SetSetting(CoverOpeningTime);
            Profile.SetSetting(CoverStateInitialisation);
            Profile.SetSetting(CalibratorStateInitialisation);
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