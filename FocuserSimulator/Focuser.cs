[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("ASCOM.Alpaca.Simulators")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("OmniSim.SettingsAPIGenerator")]

namespace ASCOM.Simulators
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using ASCOM.Common;
    using ASCOM.Common.DeviceInterfaces;
    using ASCOM.Common.Interfaces;
    using OmniSim.BaseDriver;

    /// <summary>
    /// Motor states.
    /// </summary>
    internal enum MotorState
    {
        /// <summary>
        /// Idle state.
        /// </summary>
        Idle,

        /// <summary>
        /// Moving State.
        /// </summary>
        Moving,

        /// <summary>
        /// Settling State.
        /// </summary>
        Settling,
    }

    /// <summary>
    /// ASCOM Focuser Driver for a Focuser.
    /// This class is the implementation of the public ASCOM interface.
    /// </summary>
    public class Focuser : OmniSim.BaseDriver.Driver, IFocuserV4, IAlpacaDevice, ISimulation
    {
        private const string UniqueIDProfileName = "UniqueID";

        /// <summary>
        /// Name of the Driver.
        /// </summary>
        private const string SafeName = "Alpaca Focuser Simulator";

        // Shared tracelogger between this instances classes
        private readonly ILogger traceLogger;

        // drives the position and temperature changers
        private readonly System.Timers.Timer moveTimer;
        private readonly Random randomGenerator;
        private readonly int lastOffset;
        private readonly bool keepMoving;
        private DateTime lastTempUpdate;
        private MotorState motorState = MotorState.Idle;
        private DateTime settleFinishTime;

        private int rateOfChange;
        private double lastTemp = 0;

        private int target = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Focuser"/> class.
        /// This is not safe to use, it is used for auto generating API settings calls
        /// </summary>
        public Focuser()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Focuser"/> class.
        /// </summary>
        /// <param name="deviceNumber">The device number from the Alpaca API instance. Used for log files and settings.</param>
        /// <param name="logger">An ASCOM Logger for this to write calls to.</param>
        /// <param name="profile">An ASCOM Profile for this driver to store information to.</param>
        public Focuser(int deviceNumber, ILogger logger, IProfile profile)
            : base(deviceNumber, logger, profile)
        {
            try
            {
                this.traceLogger = logger;
                this.Profile = profile;
                this.DeviceNumber = deviceNumber;
                this.traceLogger.LogInformation($"New Focuser {deviceNumber} Started");

                this.keepMoving = false;
                this.lastOffset = 0;
                this.rateOfChange = 1;

                // Temperature fluctuation random generator
                this.randomGenerator = new Random();
                this.LoadSettings();

                // start a timer that monitors and moves the focuser
                this.moveTimer = new System.Timers.Timer();
                this.moveTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.MoveTimer_Tick);
                this.moveTimer.Interval = 100;
                this.moveTimer.Enabled = true;

                this.UniqueID = DeviceName + deviceNumber.ToString();

                // Create a Unique ID if it does not exist
                try
                {
                    if (!profile.ContainsKey(UniqueIDProfileName))
                    {
                        var uniqueid = Guid.NewGuid().ToString();
                        profile.WriteValue(UniqueIDProfileName, uniqueid);
                    }

                    this.UniqueID = profile.GetValue(UniqueIDProfileName);
                }
                catch (Exception ex)
                {
                    logger.LogError($"Focuser {deviceNumber} - {ex.Message}");
                }

                logger.LogInformation($"Focuser {deviceNumber} - UUID of {this.UniqueID}");
            }
            catch (Exception ex)
            {
                this.traceLogger.LogError(ex.Message.ToString());
                throw;
            }
        }

        /// <summary>
        /// Name of the Driver.
        /// </summary>
        public override string DeviceName { get { return $"{SafeName} - {DeviceNumber}"; } }

        /// <summary>
        /// Gets what device this this driver exposes.
        /// </summary>
        public override DeviceTypes DeviceType { get; } = DeviceTypes.Focuser;

        /// <summary>
        /// Gets the stored interface version to use.
        /// </summary>
        public Setting<short> InterfaceVersionSetting { get; } = new Setting<short>("InterfaceVersion", "The ASCOM Interface Version, allowed values are 1-4", 4);

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
        /// Gets the delay for the connect timer
        /// </summary>
        public Setting<short> ConnectDelay { get; } = new Setting<short>("ConnectDelay", "The delay to be used for Connect() in milliseconds, allowed values are 1-30000", 1500);

        /// <summary>
        /// Gets a value indicating whether the focuser can halt.
        /// </summary>
        public Setting<bool> CanHalt { get; } = new Setting<bool>("CanHalt", "True if the focuser can halt", true);

        /// <summary>
        /// Gets a value indicating whether the focuser has a temperature probe.
        /// </summary>
        public Setting<bool> TempProbe { get; } = new Setting<bool>("TempProbe", "True if the driver has a temperature probe", true);

        /// <summary>
        /// Gets a value indicating whether the setting is true.
        /// </summary>
        public Setting<bool> Synchronous { get; } = new Setting<bool>("Synchronous", "True if the focuser moves are synchronous", true);

        /// <summary>
        /// Gets a value indicating whether the setting is true.
        /// </summary>
        public Setting<bool> CanStepSize { get; } = new Setting<bool>("CanStepSize", "True if the driver can report step size", true);

        /// <summary>
        /// Gets a value.
        /// </summary>
        public Setting<double> TempMax { get; } = new Setting<double>("TempMax", "Maximum simulated temperature", 50);

        /// <summary>
        /// Gets a value.
        /// </summary>
        public Setting<double> TempMin { get; } = new Setting<double>("TempMin", "Minimum simulated temperature", -50);

        /// <summary>
        /// Gets a value.
        /// </summary>
        public Setting<double> TempPeriod { get; } = new Setting<double>("TempPeriod", "Period to use for temperature changes (seconds)", 3);

        /// <summary>
        /// Gets a value.
        /// </summary>
        public Setting<int> TempSteps { get; } = new Setting<int>("TempSteps", "How many steps per temp comp action", 10);

        /// <summary>
        /// Gets a value.
        /// </summary>
        public Setting<int> SettleTime { get; } = new Setting<int>("SettleTime", "Move settle time", 500);

        /// <summary>
        /// Gets a value indicating whether the setting is true.
        /// </summary>
        public Setting<bool> AbsoluteSetting { get; } = new Setting<bool>("Absolute", "True if the focuser is an absolute focuser", true);

        /// <summary>
        /// Gets a value.
        /// </summary>
        public Setting<int> MaxIncrementSetting { get; } = new Setting<int>("MaxIncrement", "The Maximum Increment for moves", 50000);

        /// <summary>
        /// Gets a value.
        /// </summary>
        public Setting<int> MaxStepSetting { get; } = new Setting<int>("MaxStep", "The Max Step for moves", 50000);

        /// <summary>
        /// Gets a value.
        /// </summary>
        public Setting<int> PositionSetting { get; } = new Setting<int>("Position", "The starting position", 25000);

        /// <summary>
        /// Gets a value.
        /// </summary>
        public Setting<int> StepSizeSetting { get; } = new Setting<int>("StepSize", "The focuser step size (microns)", 20);

        /// <summary>
        /// Gets a value indicating whether the setting is true.
        /// </summary>
        public Setting<bool> TempCompSetting { get; } = new Setting<bool>("TempComp", "Temp Comp State", false);

        /// <summary>
        /// Gets a value indicating whether the setting is true.
        /// </summary>
        public Setting<bool> TempCompAvailableSetting { get; } = new Setting<bool>("TempCompAvailable", "True if the driver supports temp comp", true);

        /// <summary>
        /// Gets a value.
        /// </summary>
        public Setting<double> TemperatureSetting { get; } = new Setting<double>("Temperature", "Starting Temperature", 5);

        /// <summary>
        /// Gets a value indicating whether the focuser is capable of absolute position;
        /// that is, being commanded to a specific step location.
        /// </summary>
        public bool Absolute
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    return this.AbsoluteSetting.Value;
                }, DeviceType, MemberNames.Absolute, "Get");
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
        /// Gets a value indicating whether the focuser is currently moving to a new position. False if the focuser is stationary.
        /// </summary>
        /// <value><c>true</c> if moving; otherwise, <c>false</c>.</value>
        public bool IsMoving
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    return this.motorState != MotorState.Idle;
                }, DeviceType, MemberNames.IsMoving, "Get");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the focuser is connected. Set True to start the link to the focuser;
        /// set False to terminate the link. The current link status can also be read
        /// back as this property. An exception will be raised if the link fails to
        /// change state for any reason.
        /// </summary>
        /// <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
        public bool Link
        {
            get { return this.Connected; }
            set { this.Connected = value; }
        }

        /// <summary>
        /// Gets a value indicating the Maximum increment size allowed by the focuser; i.e. the maximum number
        /// of steps allowed in one move operation. For most focusers this is the
        /// same as the MaxStep property. This is normally used to limit the
        /// Increment display in the host software.
        /// </summary>
        public int MaxIncrement
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    return this.MaxIncrementSetting.Value;
                }, DeviceType, MemberNames.MaxIncrement, "Get");
            }
        }

        /// <summary>
        /// Gets a value of theMaximum step position permitted. The focuser can step between 0 and MaxStep.
        /// If an attempt is made to move the focuser beyond these limits,
        /// it will automatically stop at the limit.
        /// </summary>
        public int MaxStep
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    return this.MaxStepSetting.Value;
                }, DeviceType, MemberNames.MaxStep, "Get");
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

        /// <summary>
        /// Current focuser position, in steps. Valid only for absolute positioning
        /// focusers (see the Absolute property). An exception will be raised for
        /// relative positioning focusers.
        /// </summary>
        public int Position
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    if (!this.AbsoluteSetting.Value)
                    {
                        throw new PropertyNotImplementedException("Position", "Position cannot be read for a relative focuser");
                    }

                    return this.PositionSetting.Value;
                }, DeviceType, MemberNames.Position, "Get");
            }
        }

        /// <summary>
        /// Gets the Step size (microns) for the focuser. Raises an exception if the focuser
        /// does not intrinsically know what the step size is.
        /// </summary>
        public double StepSize
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    if (this.CanStepSize.Value)
                    {
                        return this.StepSizeSetting.Value;
                    }

                    throw new PropertyNotImplementedException("Property StepSize is not implemented");
                }, DeviceType, MemberNames.StepSize, "Get");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the state of temperature compensation mode (if available), else always
        /// False. If the TempCompAvailable property is True, then setting TempComp
        /// to True puts the focuser into temperature tracking mode. While in
        /// temperature tracking mode, Move commands will be rejected by the
        /// focuser. Set to False to turn off temperature tracking. An exception
        /// will be raised if TempCompAvailable is False and an attempt is made
        /// to set TempComp to true.
        /// </summary>
        public bool TempComp
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    if (!this.TempCompAvailableSetting.Value)
                    {
                        return false;
                    }

                    return this.TempCompSetting.Value;
                }, DeviceType, MemberNames.TempComp, "Get");
            }

            set
            {
                this.ProcessCommand(
                () =>
                {
                    if (!this.TempCompAvailableSetting.Value)
                    {
                        throw new PropertyNotImplementedException("TempComp");
                    }

                    this.TempCompSetting.Value = value;
                }, DeviceType, MemberNames.TempComp, "Set");
            }
        }

        /// <summary>
        /// Gets a value indicating whether the focuser has temperature compensation available. Will be True
        /// only if the focuser's temperature compensation can be turned on and
        /// off via the TempComp property.
        /// </summary>
        public bool TempCompAvailable
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    return this.TempCompAvailableSetting.Value;
                }, DeviceType, MemberNames.TempCompAvailable, "Get");
            }
        }

        /// <summary>
        /// Gets the Current ambient temperature as measured by the focuser. Raises an
        /// exception if ambient temperature is not available. Commonly
        /// available on focusers with a built-in temperature compensation
        /// mode.
        /// </summary>
        public double Temperature
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    this.CheckConnected("Temperature");
                    return this.TemperatureSetting.Value;
                }, DeviceType, MemberNames.Temperature, "Get");
            }
        }

        #region IFocuserV4 members

        /// <summary>
        /// Gets and Returns the device's operational state in one call.
        /// </summary>
        public override List<StateValue> DeviceState
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    // Create an array list to hold the IStateValue entries
                    List<StateValue> deviceState = [];

                    try
                    {
                        deviceState.Add(new StateValue(nameof(IFocuserV4.IsMoving), this.IsMoving));
                    }
                    catch
                    {
                    }

                    try
                    {
                        deviceState.Add(new StateValue(nameof(IFocuserV4.Position), this.Position));
                    }
                    catch
                    {
                    }

                    try
                    {
                        deviceState.Add(new StateValue(nameof(IFocuserV4.Temperature), this.Temperature));
                    }
                    catch
                    {
                    }

                    try
                    {
                        deviceState.Add(new StateValue(DateTime.Now));
                    }
                    catch
                    {
                    }

                    return deviceState;
                }, DeviceType, MemberNames.DeviceState, "Get");
            }
        }

        #endregion IFocuserV4 members

        /// <summary>
        /// Immediately stop any focuser motion due to a previous Move()
        /// method call. Some focusers may not support this function, in
        /// which case an exception will be raised. Recommendation: Host
        /// software should call this method upon initialization and, if
        /// it fails, disable the Halt button in the user interface.
        /// </summary>
        public void Halt()
        {
            this.ProcessCommand(
                () =>
                {
                    if (!this.CanHalt.Value)
                    {
                        throw new MethodNotImplementedException("Halt");
                    }

                    this.CheckConnected("Halt");
                    if (this.AbsoluteSetting.Value)
                    {
                        this.target = this.PositionSetting.Value;
                    }
                    else
                    {
                        this.PositionSetting.Value = 0;
                    }
                }, DeviceType, MemberNames.Halt, "Command");
        }

        /// <summary>
        /// Step size (microns) for the focuser. Raises an exception if
        /// the focuser does not intrinsically know what the step size is.
        /// </summary>
        /// <param name="value">Target position.</param>
        /// <exception cref="InvalidOperationException">Raised for 1-2 version focusers.</exception>
        public void Move(int value)
        {
            this.ProcessCommand(
                () =>
                {
                    this.CheckConnected("Move");

                    // Next two lines removed to implement IFocuserV3 requirement
                    if (this.TempCompSetting.Value && this.InterfaceVersionSetting.Value < 3)
                    {
                        throw new InvalidOperationException("Move not allowed when temperature compensation is active");
                    }

                    if (this.AbsoluteSetting.Value)
                    {
                        this.TraceLogger.LogVerbose($"Move Absolute to: {value}");
                        this.target = this.Truncate(0, value, this.MaxStepSetting.Value);
                        this.rateOfChange = 40;
                    }
                    else
                    {
                        this.TraceLogger.LogVerbose($"Move Relative by: {value}");
                        this.target = 0;
                        this.PositionSetting.Value = this.Truncate(-this.MaxStepSetting.Value, value, this.MaxStepSetting.Value);
                        this.rateOfChange = 40;
                    }

                    this.motorState = MotorState.Moving;
                }, DeviceType, MemberNames.Move, "Command");
        }

        /// <summary>
        /// Connects to the hardware.
        /// </summary>
        public override void Connect()
        {
            base.ConnectTimer.Interval = ConnectDelay.Value;
            base.Connect();
        }

        /// <summary>
        /// Ticks 10 times a second, updating the focuser position and IsMoving properties.
        /// </summary>
        private void MoveTimer_Tick(object source, System.Timers.ElapsedEventArgs e)
        {
            // Change at introduction of IFocuserV3 - only allow random temperature induced changes when the motor is in the idle state
            // This is because IFocuser V3 allows moves when temperature compensation is active
            if (this.motorState == MotorState.Idle)
            {
                // Create random temperature change
                if (DateTime.Now.Subtract(this.lastTempUpdate).TotalSeconds > this.TempPeriod.Value)
                {
                    this.lastTempUpdate = DateTime.Now;

                    // apply a random change to the temperature
                    double tempOffset = this.randomGenerator.NextDouble() - 0.5;
                    this.TemperatureSetting.Value = Math.Round(this.TemperatureSetting.Value + tempOffset, 2);

                    // move the focuser target to track the temperature if required
                    if (this.TempCompSetting.Value)
                    {
                        var dt = (int)((this.TemperatureSetting.Value - this.lastTemp) * this.TempSteps.Value);
                        if (dt != 0)// return;
                        {
                            this.target += dt;
                            this.lastTemp = this.TemperatureSetting.Value;
                        }
                    }
                }
            }

            // Condition target within the acceptable range
            if (this.target > this.MaxStepSetting.Value)
            {
                this.target = this.MaxStepSetting.Value;
            }

            if (this.target < 0)
            {
                this.target = 0;
            }

            // Actually move the focuser if necessary
            if (this.PositionSetting.Value != this.target)
            {
                if (Math.Abs(this.PositionSetting.Value - this.target) <= this.rateOfChange)
                {
                    this.PositionSetting.Value = this.target;
                }
                else
                {
                    this.PositionSetting.Value += (this.PositionSetting.Value > this.target) ? -this.rateOfChange : this.rateOfChange;
                }
            }

            if (this.keepMoving)
            {
                this.target = (Math.Sign(this.lastOffset) > 0) ? this.MaxStepSetting.Value : 0;
                if (this.rateOfChange < 100)
                {
                    this.rateOfChange = (int)Math.Ceiling((double)this.rateOfChange * 1.2);
                }
            }

            // handle MotorState
            switch (this.motorState)
            {
                case MotorState.Moving:
                    if (this.PositionSetting.Value == this.target)
                    {
                        this.motorState = MotorState.Settling;
                        this.settleFinishTime = DateTime.Now + TimeSpan.FromMilliseconds(this.SettleTime.Value);
                    }

                    return;

                case MotorState.Settling:
                    if (this.settleFinishTime < DateTime.Now)
                    {
                        this.motorState = MotorState.Idle;
                    }

                    return;
            }
        }

        /// <summary>
        /// Truncate val to be between min and max.
        /// </summary>
        /// <param name="min">Min Value.</param>
        /// <param name="val">Current Value.</param>
        /// <param name="max">Max Value.</param>
        /// <returns>Truncated Value.</returns>
        private int Truncate(int min, int val, int max)
        {
            return Math.Max(Math.Min(max, val), min);
        }

        /// <summary>
        /// Load the profile values.
        /// </summary>
        public override void LoadSettings()
        {
            this.AbsoluteSetting.Value = this.Profile.GetSettingReturningDefault(this.AbsoluteSetting);
            this.MaxIncrementSetting.Value = this.Profile.GetSettingReturningDefault(this.MaxIncrementSetting);
            this.MaxStepSetting.Value = this.Profile.GetSettingReturningDefault(this.MaxStepSetting);
            this.PositionSetting.Value = this.Profile.GetSettingReturningDefault(this.PositionSetting);
            this.target = this.PositionSetting.Value;
            this.StepSizeSetting.Value = this.Profile.GetSettingReturningDefault(this.StepSizeSetting);
            this.TempCompSetting.Value = this.Profile.GetSettingReturningDefault(this.TempCompSetting);
            this.TempCompAvailableSetting.Value = this.Profile.GetSettingReturningDefault(this.TempCompAvailableSetting);
            this.TemperatureSetting.Value = this.Profile.GetSettingReturningDefault(this.TemperatureSetting);
            this.CanHalt.Value = this.Profile.GetSettingReturningDefault(this.CanHalt);
            this.CanStepSize.Value = this.Profile.GetSettingReturningDefault(this.CanStepSize);
            this.Synchronous.Value = this.Profile.GetSettingReturningDefault(this.Synchronous);
            this.TempMax.Value = this.Profile.GetSettingReturningDefault(this.TempMax);
            this.TempMin.Value = this.Profile.GetSettingReturningDefault(this.TempMin);
            this.TempPeriod.Value = this.Profile.GetSettingReturningDefault(this.TempPeriod);
            this.TempProbe.Value = this.Profile.GetSettingReturningDefault(this.TempProbe);
            this.TempSteps.Value = this.Profile.GetSettingReturningDefault(this.TempSteps);
            this.SettleTime.Value = this.Profile.GetSettingReturningDefault(this.SettleTime);
            this.InterfaceVersionSetting.Value =this.Profile.GetSettingReturningDefault(this.InterfaceVersionSetting);
        }

        /// <summary>
        /// Save profile values.
        /// </summary>
        public void SaveProfileSettings()
        {
            if (this.TemperatureSetting.Value > this.TempMax.Value)
            {
                this.TemperatureSetting.Value = this.TempMax.Value;
            }

            if (this.TemperatureSetting.Value < this.TempMin.Value)
            {
                this.TemperatureSetting.Value = this.TempMin.Value;
            }

            if (this.PositionSetting.Value > this.MaxStepSetting.Value)
            {
                this.PositionSetting.Value = this.MaxStepSetting.Value;
            }

            this.Profile.SetSetting(this.AbsoluteSetting);
            this.Profile.SetSetting(this.MaxIncrementSetting);
            this.Profile.SetSetting(this.MaxStepSetting);
            this.Profile.SetSetting(this.PositionSetting);
            this.Profile.SetSetting(this.StepSizeSetting);
            this.Profile.SetSetting(this.TempCompSetting);
            this.Profile.SetSetting(this.TempCompAvailableSetting);
            this.Profile.SetSetting(this.TemperatureSetting);
            this.Profile.SetSetting(this.CanHalt);
            this.Profile.SetSetting(this.CanStepSize);
            this.Profile.SetSetting(this.CanStepSize);
            this.Profile.SetSetting(this.TempMax);
            this.Profile.SetSetting(this.TempMin);
            this.Profile.SetSetting(this.TempPeriod);
            this.Profile.SetSetting(this.TempProbe);
            this.Profile.SetSetting(this.TempSteps);
            this.Profile.SetSetting(this.SettleTime);
            this.Profile.SetSetting(this.InterfaceVersionSetting);
        }
    }
}