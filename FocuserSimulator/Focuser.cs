using ASCOM.Common;
using ASCOM.Common.DeviceInterfaces;
using ASCOM.Common.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ASCOM.Alpaca.Simulators")]

namespace ASCOM.Simulators
{
    //
    // Your driver's DeviceID is ASCOM.Simulator.Focuser
    //
    // The Guid attribute sets the CLSID for ASCOM.Simulator.Focuser
    // The ClassInterface/None attributed prevents an empty interface called
    // _Conceptual from being created and used as the [default] interface
    //

    /// <summary>
    /// ASCOM Focuser Driver for a Focuser.
    /// This class is the implementation of the public ASCOM interface.
    /// </summary>
    public class Focuser : IFocuserV4, IDisposable, IAlpacaDevice, ISimulation
    {
        #region Constants

        private const string UNIQUE_ID_PROFILE_NAME = "UniqueID";

        /// <summary>
        /// Name of the Driver
        /// </summary>
        private const string name = "Alpaca Focuser Simulator";

        /// <summary>
        /// Description of the driver
        /// </summary>
        private const string description = "ASCOM Focuser Simulator Driver";

        /// <summary>
        /// Driver information
        /// </summary>
        private const string driverInfo = "Focuser Simulator Driver";

        /// <summary>
        /// Driver interface version
        /// </summary>
        /// 
        private const short interfaceVersion = 4;

        /// <summary>
        /// ASCOM DeviceID (COM ProgID) for this driver.
        /// The DeviceID is used by ASCOM applications to load the driver at runtime.
        /// </summary>
        private const string sCsDriverId = "ASCOM.Simulator.Focuser";

        /// <summary>
        /// Driver description that displays in the ASCOM Chooser.
        /// </summary>
        private const string sCsDriverDescription = "ASCOM Simulator Focuser Driver";

        /// <summary>
        /// Sets up the permanent store for saved settings
        /// </summary>
        private readonly IProfile Profile;

#endregion Constants

        internal ILogger TL;// Shared tracelogger between this instances classes

        #region local parameters

        private bool _isConnected;
        private System.Timers.Timer _moveTimer; // drives the position and temperature changers
        private int _position;
        internal int Target;
        private double _lastTemp;
        private DateTime lastTempUpdate;
        private Random RandomGenerator;
        internal double InternalStepSize;
        internal bool tempComp;

        private enum MotorState
        {
            idle,
            moving,
            settling
        }

        private MotorState motorState = MotorState.idle;
        private DateTime settleFinishTime;

        #endregion local parameters

        #region Constructor and dispose

        /// <summary>
        /// Initializes a new instance of the <see cref="Focuser"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public Focuser(int deviceNumber, ILogger logger, IProfile profile)
        {
            try
            {
                TL = logger;
                Profile = profile;
                LogMessage($"New Focuser {deviceNumber}", "Started");

                DeviceNumber = deviceNumber;

                //check to see if the profile is ok

                LogMessage("New", "Validated OK");
                KeepMoving = false;
                LastOffset = 0;
                RateOfChange = 1;
                MouseDownTime = DateTime.MaxValue; //Initialise to "don't accelerate" value
                RandomGenerator = new Random(); //Temperature fluctuation random generator
                LoadFocuserKeyValues();
                LogMessage("New", "Loaded Key Values");
                LogMessage("FocusSettingsForm", "Created Handbox");

                // start a timer that monitors and moves the focuser
                _moveTimer = new System.Timers.Timer();
                _moveTimer.Elapsed += new System.Timers.ElapsedEventHandler(MoveTimer_Tick);
                _moveTimer.Interval = 100;
                _moveTimer.Enabled = true;
                _lastTemp = Temperature;
                Target = _position;

                LogMessage("New", "Started Simulation");

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
                    logger.LogError($"Focuser {deviceNumber} - {ex.Message}");
                }

                logger.LogInformation($"Focuser {deviceNumber} - UUID of {UniqueID}");

                LogMessage("New", "Completed");
            }
            catch (Exception ex)
            {
                TL.LogError(ex.Message.ToString());
                throw ex;
            }
        }

        public string DeviceName { get => Name; }
        public int DeviceNumber { get; private set; }
        public string UniqueID { get; private set; }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //try { LogMessage("Dispose", "Dispose called: " + disposing.ToString()); } catch { }
                //try { _moveTimer.Stop(); } catch { }
                //try { _moveTimer.Close(); } catch { }
                //try { _moveTimer.Dispose(); } catch { }
            }
        }

        public void Dispose()
        {
            try { LogMessage("Dispose", "Dispose called."); } catch { }
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion Constructor and dispose

        #region Private Properties

        internal bool CanHalt { get; set; }
        internal bool TempProbe { get; set; }
        internal bool Synchronous { get; set; }
        internal bool CanStepSize { get; set; }
        internal bool KeepMoving { get; set; }
        internal int LastOffset { get; set; }
        internal double TempMax { get; set; }
        internal double TempMin { get; set; }
        internal double TempPeriod { get; set; }
        internal int TempSteps { get; set; }
        internal int RateOfChange { get; set; }
        internal DateTime MouseDownTime { get; set; }
        internal int SettleTime { get; set; }

        #endregion Private Properties

        #region IFocuserV3 Members

        /// <summary>
        /// True if the focuser is capable of absolute position;
        /// that is, being commanded to a specific step location.
        /// </summary>
        public bool Absolute { get; set; }

        /// <summary>
        /// Invokes the specified device-specific action.
        /// </summary>
        /// <exception cref="MethodNotImplementedException"></exception>
        public string Action(string actionName, string actionParameters)
        {
            throw new MethodNotImplementedException("Action");
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and does not
        /// wait for a response. Optionally, protocol framing characters
        /// may be added to the string before transmission.
        /// mode.
        /// </summary>
        /// <exception cref="MethodNotImplementedException"></exception>
        public void CommandBlind(string command, bool raw)
        {
            throw new MethodNotImplementedException("CommandBlind");
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits
        /// for a boolean response. Optionally, protocol framing
        /// characters may be added to the string before transmission.
        /// </summary>
        /// <exception cref="MethodNotImplementedException"></exception>
        public bool CommandBool(string command, bool raw)
        {
            throw new MethodNotImplementedException("CommandBool");
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits
        /// for a string response. Optionally, protocol framing
        /// characters may be added to the string before transmission.
        /// </summary>
        /// <exception cref="MethodNotImplementedException"></exception>
        public string CommandString(string command, bool raw)
        {
            throw new MethodNotImplementedException("CommandString");
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Focuser"/> is connected.
        /// </summary>
        /// <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
        public bool Connected
        {
            get
            {
                LogMessage("Connected Get", _isConnected.ToString());
                return _isConnected;
            }
            set
            {
                if (_isConnected == value)
                {
                    LogMessage("Connected Set", "Connected is already :" + _isConnected.ToString() + ", doing nothing.");
                    return;
                }
                if (value)
                {
                    LogMessage("Connected Set", "Connecting driver.");
                    if (_moveTimer == null)
                    {
                        LogMessage("Connected Set", "Move timer is null so creating new timer.");
                        _moveTimer = new System.Timers.Timer();
                    }
                    LogMessage("Connected Set", "Adding move timer handler.");
                    _moveTimer.Elapsed += new System.Timers.ElapsedEventHandler(MoveTimer_Tick);
                    _moveTimer.Interval = 100;
                    LogMessage("Connected Set", "Enabling move timer.");
                    _moveTimer.Enabled = true;
                    LogMessage("Connected Set", "Showing handbox.");
                }
                else
                {
                    LogMessage("Connected Set", "Disconnecting driver.");
                    LogMessage("Connected Set", "Disabling move timer.");
                    _moveTimer.Enabled = false;
                    LogMessage("Connected Set", "Removing move timer handler.");
                    _moveTimer.Elapsed -= MoveTimer_Tick;
                    LogMessage("Connected Set", "Hiding handbox.");
                }
                _isConnected = value;
            }
        }

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
        /// Immediately stop any focuser motion due to a previous Move()
        /// method call. Some focusers may not support this function, in
        /// which case an exception will be raised. Recommendation: Host
        /// software should call this method upon initialization and, if
        /// it fails, disable the Halt button in the user interface.
        /// </summary>
        public void Halt()
        {
            if (!CanHalt)
                throw new MethodNotImplementedException("Halt");

            CheckConnected("Halt");
            if (Absolute)
            {
                Target = _position;
            }
            else
            {
                _position = 0;
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
        /// True if the focuser is currently moving to a new position. False if the focuser is stationary.
        /// </summary>
        /// <value><c>true</c> if moving; otherwise, <c>false</c>.</value>
        public bool IsMoving
        {
            //get { return (_position != Target); }
            get
            {
                LogMessage("IsMoving", "MotorState " + motorState.ToString());
                return (motorState != MotorState.idle);
            }
        }

        /// <summary>
        /// State of the connection to the focuser. et True to start the link to the focuser;
        /// set False to terminate the link. The current link status can also be read
        /// back as this property. An exception will be raised if the link fails to
        /// change state for any reason.
        /// </summary>
        /// <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
        public bool Link
        {
            get { return Connected; }
            set { Connected = value; }
        }

        /// <summary>
        /// Maximum increment size allowed by the focuser; i.e. the maximum number
        /// of steps allowed in one move operation. For most focusers this is the
        /// same as the MaxStep property. This is normally used to limit the
        /// Increment display in the host software.
        /// </summary>
        public int MaxIncrement { get; internal set; }

        /// <summary>
        /// Maximum step position permitted. The focuser can step between 0 and MaxStep.
        /// If an attempt is made to move the focuser beyond these limits,
        /// it will automatically stop at the limit.
        /// </summary>
        public int MaxStep { get; internal set; }

        /// <summary>
        /// Step size (microns) for the focuser. Raises an exception if
        /// the focuser does not intrinsically know what the step size is.
        /// </summary>
        public void Move(int value)
        {
            CheckConnected("Move");
            // Next two lines removed to implement IFocuserV3 requirement
            // if (tempComp)
            // throw new InvalidOperationException("Move not allowed when temperature compensation is active");
            if (Absolute)
            {
                LogMessage("Move Absolute", value.ToString());
                Target = Truncate(0, value, MaxStep);
                RateOfChange = 40;
            }
            else
            {
                LogMessage("Move Relative", value.ToString());
                Target = 0;
                _position = Truncate(-MaxStep, value, MaxStep);
                RateOfChange = 40;
            }
            motorState = MotorState.moving;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Current focuser position, in steps. Valid only for absolute positioning
        /// focusers (see the Absolute property). An exception will be raised for
        /// relative positioning focusers.
        /// </summary>
        public int Position
        {
            get
            {
                if (!Absolute)
                {
                    LogMessage("Position", "Position cannot be read for a relative focuser");
                    throw new PropertyNotImplementedException("Position", "Position cannot be read for a relative focuser");
                }
                if (!(TL == null)) LogMessage("Position", _position.ToString());
                return _position;
            }
        }

        /// <summary>
        /// Displays the Setup Dialog form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// </summary>
        public void SetupDialog()
        {
        }

        /// <summary>
        /// Step size (microns) for the focuser. Raises an exception if the focuser
        /// does not intrinsically know what the step size is.
        /// </summary>
        public double StepSize
        {
            get
            {
                if (CanStepSize)
                {
                    return InternalStepSize;
                }
                throw new PropertyNotImplementedException("Property StepSize is not implemented");
            }
            internal set
            {
                InternalStepSize = value;
            }
        }

        //public double StepSize { get; internal set; }

        /// <summary>
        /// Gets the supported actions.
        /// </summary>
        public IList<string> SupportedActions
        {
            // no supported actions, return empty array
            get
            {
                return new List<string>();
            }
        }

        /// <summary>
        /// The state of temperature compensation mode (if available), else always
        /// False. If the TempCompAvailable property is True, then setting TempComp
        /// to True puts the focuser into temperature tracking mode. While in
        /// temperature tracking mode, Move commands will be rejected by the
        /// focuser. Set to False to turn off temperature tracking. An exception
        /// will be raised if TempCompAvailable is False and an attempt is made
        /// to set TempComp to true.
        /// </summary>
        public bool TempComp
        {
            get { return tempComp; }
            set
            {
                if (!TempCompAvailable)
                    throw new PropertyNotImplementedException("TempComp");
                tempComp = value;
            }
        }

        /// <summary>
        /// True if focuser has temperature compensation available. Will be True
        /// only if the focuser's temperature compensation can be turned on and
        /// off via the TempComp property.
        /// </summary>
        public bool TempCompAvailable { get; internal set; }

        /// <summary>
        /// Current ambient temperature as measured by the focuser. Raises an
        /// exception if ambient temperature is not available. Commonly
        /// available on focusers with a built-in temperature compensation
        /// mode.
        /// </summary>
        public double Temperature { get; internal set; }

        #endregion IFocuserV3 Members

#region IFocuserV4 members
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
        public List<StateValue> DeviceState
        {
            get
            {
                // Create an array list to hold the IStateValue entries
                List<StateValue> deviceState = new List<StateValue>();

                try { deviceState.Add(new StateValue(nameof(IFocuserV4.IsMoving), IsMoving)); } catch { }
                try { deviceState.Add(new StateValue(nameof(IFocuserV4.Position), Position)); } catch { }
                try { deviceState.Add(new StateValue(nameof(IFocuserV4.Temperature), Temperature)); } catch { }
                try { deviceState.Add(new StateValue(DateTime.Now)); } catch { }

                return deviceState;
            }
        }
#endregion

        #region Private Members

        /// <summary>
        /// Ticks 10 times a second, updating the focuser position and IsMoving properties
        /// </summary>
        private void MoveTimer_Tick(object source, System.Timers.ElapsedEventArgs e)
        {
            // Change at introduction of IFocuserV3 - only allow random temperature induced changes when the motor is in the idle state
            // This is because IFocuser V3 allows moves when temperature compensation is active
            if (motorState == MotorState.idle)
            {
                //Create random temperature change
                if (DateTime.Now.Subtract(lastTempUpdate).TotalSeconds > TempPeriod)
                {
                    lastTempUpdate = DateTime.Now;
                    // apply a random change to the temperature
                    double tempOffset = (RandomGenerator.NextDouble() - 0.5);// / 10.0;
                    Temperature = Math.Round(Temperature + tempOffset, 2);

                    // move the focuser target to track the temperature if required
                    if (tempComp)
                    {
                        var dt = (int)((Temperature - _lastTemp) * TempSteps);
                        if (dt != 0)// return;
                        {
                            Target += dt;
                            _lastTemp = Temperature;
                        }
                    }
                }
            }

            if (Target > MaxStep) Target = MaxStep; // Condition target within the acceptable range
            if (Target < 0) Target = 0;

            if (_position != Target) //Actually move the focuser if necessary
            {
                LogMessage("Moving", "LastOffset, Position, Target RateOfChange " + LastOffset + " " + _position + " " + Target + " " + RateOfChange);

                if (Math.Abs(_position - Target) <= RateOfChange)
                {
                    _position = Target;
                    LogMessage("Moving", "  Set position = target");
                }
                else
                {
                    _position += (_position > Target) ? -RateOfChange : RateOfChange;
                    LogMessage("Moving", "  Updated position = " + _position);
                }
                LogMessage("Moving", "  New position = " + _position);
            }
            if (KeepMoving & (DateTime.Now.Subtract(MouseDownTime).TotalSeconds > 0.5))
            {
                Target = (Math.Sign(LastOffset) > 0) ? MaxStep : 0;
                MouseDownTime = DateTime.Now;
                if (RateOfChange < 100)
                {
                    RateOfChange = (int)Math.Ceiling((double)RateOfChange * 1.2);
                }
                LogMessage("KeepMoving", "LastOffset, Position, Target, RateOfChange MouseDownTime " + LastOffset + " " + _position + " " + Target + " " + RateOfChange + " " + MouseDownTime.ToLongTimeString());
            }

            // handle MotorState
            switch (motorState)
            {
                case MotorState.moving:
                    if (_position == Target)
                    {
                        motorState = MotorState.settling;
                        settleFinishTime = DateTime.Now + TimeSpan.FromMilliseconds(SettleTime);
                        LogMessage("MoveTimer", "Settle start, time " + SettleTime.ToString());
                    }
                    return;

                case MotorState.settling:
                    if (settleFinishTime < DateTime.Now)
                    {
                        motorState = MotorState.idle;
                        LogMessage("MoveTimer", "settle finished");
                    }
                    return;
            }
        }

        private void CheckConnected(string property)
        {
            if (!_isConnected)
                throw new NotConnectedException(property);
        }

        /// <summary>
        /// Truncate val to be between min and max
        /// </summary>
        /// <param name="min"></param>
        /// <param name="val"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private static int Truncate(int min, int val, int max)
        {
            return Math.Max(Math.Min(max, val), min);
        }

        /// <summary>
        /// Load the profile values
        /// </summary>
        private void LoadFocuserKeyValues()
        {
            Absolute = Convert.ToBoolean(Profile.GetValue("Absolute", true.ToString()), CultureInfo.InvariantCulture);
            MaxIncrement = Convert.ToInt32(Profile.GetValue("MaxIncrement", "50000"), CultureInfo.InvariantCulture);
            MaxStep = Convert.ToInt32(Profile.GetValue("MaxStep", "50000"), CultureInfo.InvariantCulture);
            _position = Convert.ToInt32(Profile.GetValue("Position", "25000"), CultureInfo.InvariantCulture);
            InternalStepSize = Convert.ToDouble(Profile.GetValue("StepSize", "20"), CultureInfo.InvariantCulture);
            tempComp = Convert.ToBoolean(Profile.GetValue("TempComp", false.ToString()), CultureInfo.InvariantCulture);
            TempCompAvailable = Convert.ToBoolean(Profile.GetValue("TempCompAvailable", true.ToString()), CultureInfo.InvariantCulture);
            Temperature = Convert.ToDouble(Profile.GetValue("Temperature", "5"), CultureInfo.InvariantCulture);
            //extended focuser items
            CanHalt = Convert.ToBoolean(Profile.GetValue("CanHalt", true.ToString()), CultureInfo.InvariantCulture);
            CanStepSize = Convert.ToBoolean(Profile.GetValue("CanStepSize", true.ToString()), CultureInfo.InvariantCulture);
            Synchronous = Convert.ToBoolean(Profile.GetValue("Synchronous", false.ToString()), CultureInfo.InvariantCulture);
            TempMax = Convert.ToDouble(Profile.GetValue("TempMax", "50"), CultureInfo.InvariantCulture);
            TempMin = Convert.ToDouble(Profile.GetValue("TempMin", "-50"), CultureInfo.InvariantCulture);
            TempPeriod = Convert.ToDouble(Profile.GetValue("TempPeriod", "3"), CultureInfo.InvariantCulture);
            TempProbe = Convert.ToBoolean(Profile.GetValue("TempProbe", true.ToString()), CultureInfo.InvariantCulture);
            TempSteps = Convert.ToInt32(Profile.GetValue("TempSteps", "10"), CultureInfo.InvariantCulture);
            SettleTime = Convert.ToInt32(Profile.GetValue("SettleTime", "500"), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Save profile values
        /// </summary>
        public void SaveProfileSettings()
        {
            if (Temperature > TempMax) Temperature = TempMax;
            if (Temperature < TempMin) Temperature = TempMin;
            if (_position > MaxStep) _position = MaxStep;

            //ascom items
            Profile.WriteValue("Absolute", Absolute.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue("MaxIncrement", MaxIncrement.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue("MaxStep", MaxStep.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue("Position", _position.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue("StepSize", InternalStepSize.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue("TempComp", tempComp.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue("TempCompAvailable", TempCompAvailable.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue("Temperature", Temperature.ToString(CultureInfo.InvariantCulture));
            //extended focuser items
            Profile.WriteValue("CanHalt", CanHalt.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue("CanStepSize", CanStepSize.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue("Synchronous", Synchronous.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue("TempMax", TempMax.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue("TempMin", TempMin.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue("TempPeriod", TempPeriod.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue("TempProbe", TempProbe.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue("TempSteps", TempSteps.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue("SettleTime", SettleTime.ToString(CultureInfo.InvariantCulture));
        }

        #region ISimulator

        public void ResetSettings()
        {
            Profile.Clear();
            LoadFocuserKeyValues();
        }

        public string GetXMLProfile()
        {
            return Profile.GetProfile();
        }

        #endregion ISimulator

        /// <summary>
        /// Log a message making sure that the TraceLogger exists.
        /// This is to work round an issue with the form timers that seem to fire after the form and trace logger have been disposed
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        private void LogMessage(string source, string message)
        {
            try
            {
                if (!(TL == null)) TL.LogVerbose(source + " - " + message);
            }
            catch { } // Ignore errors here
        }

        #endregion Private Members
    }
}