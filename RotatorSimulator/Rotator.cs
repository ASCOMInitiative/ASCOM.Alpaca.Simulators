namespace ASCOM.Simulators
{
    using ASCOM.Common;
    using ASCOM.Common.DeviceInterfaces;
    using ASCOM.Common.Interfaces;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// ASCOM Rotator simulator.
    /// </summary>
    public class Rotator : OmniSim.BaseDriver.Driver, IRotatorV4, IAlpacaDevice, ISimulation
    {
        private const string UniqueIDProfileKey = "UniqueID";

        private const string RotatorName = "Alpaca Rotator Simulator";
        private const string RotatorDescription = "ASCOM Rotator Driver for RotatorSimulator";
        private const string RotatorDriverInfo = "ASCOM.Simulator.Rotator";

        /// <summary>
        /// Initializes a new instance of the <see cref="Rotator"/> class.
        /// </summary>
        /// <param name="deviceNumber">Alpaca device number.</param>
        /// <param name="logger">Tracelogger.</param>
        /// <param name="profile">Profile.</param>
        public Rotator(int deviceNumber, ILogger logger, IProfile profile)
            : base(deviceNumber, logger, profile)
        {
            try
            {
                this.RotatorHardware = new RotatorHardware();

                this.RotatorHardware.Initialize(profile);

                this.DeviceNumber = deviceNumber;

                // This should be replaced by the next bit of code but is semi-unique as a default.
                this.UniqueID = RotatorName + deviceNumber.ToString();
                // Create a Unique ID if it does not exist
                try
                {
                    if (!profile.ContainsKey(UniqueIDProfileKey))
                    {
                        var uniqueid = Guid.NewGuid().ToString();
                        profile.WriteValue(UniqueIDProfileKey, uniqueid);
                    }

                    this.UniqueID = profile.GetValue(UniqueIDProfileKey);
                }
                catch (Exception ex)
                {
                    logger.LogError($"Rotator {deviceNumber} - {ex.Message}");
                }

                logger.LogInformation($"Rotator {deviceNumber} - UUID of {this.UniqueID}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message.ToString());
                throw;
            }
        }

        /// <summary>
        /// Name of the Driver.
        /// </summary>
        public override string DeviceName { get { return $"{RotatorName} - {DeviceNumber}"; } }

        /// <summary>
        /// Gets what device this this driver exposes.
        /// </summary>
        public override DeviceTypes DeviceType { get; } = DeviceTypes.Rotator;

        /// <summary>
        /// Gets access to the underlying simulation.
        /// </summary>
        public RotatorHardware RotatorHardware { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the Rotator is connected.
        /// </summary>
        public override bool Connected
        {
            get
            {
                return base.Connected && this.RotatorHardware.Connected;
            }

            set
            {
                base.Connected = value;
                this.RotatorHardware.Connected = value;
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
                    return RotatorDescription;
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
                    return RotatorDriverInfo;
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
                    return this.RotatorHardware.InterfaceVersionSetting.Value;
                }, DeviceType, MemberNames.InterfaceVersion, "Get");
            }
        }

        /// <summary>
        /// Gets the safe interface version with no exception for V1.
        /// </summary>
        public override short SafeInterfaceVersion
        {
            get => this.RotatorHardware.InterfaceVersionSetting.Value;
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
                    return RotatorName;
                }, DeviceType, MemberNames.Name, "Get");
            }
        }

        /// <summary>
        /// Gets a value indicating whether the rotator can reverse.
        /// </summary>
        public bool CanReverse
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    return this.RotatorHardware.CanReverse.Value;
                }, DeviceType, MemberNames.CanReverse, "Get");
            }
        }

        /// <summary>
        /// Gets a value indicating whether the rotator is moving.
        /// </summary>
        public bool IsMoving
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    return this.RotatorHardware.Moving;
                }, DeviceType, MemberNames.IsMoving, "Get");
            }
        }

        /// <summary>
        /// Gets the current mechanical position.
        /// </summary>
        public float MechanicalPosition
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    return this.RotatorHardware.Position.Value;
                }, DeviceType, MemberNames.MechanicalPosition, "Get");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the rotator is reversed.
        /// </summary>
        public bool Reverse
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    if (this.RotatorHardware.InterfaceVersionSetting.Value < 3 && !this.RotatorHardware.CanReverse.Value)
                    {
                        throw new PropertyNotImplementedException();
                    }

                    return this.RotatorHardware.Reverse.Value;
                }, DeviceType, MemberNames.Reverse, "Get");
            }

            set
            {
                this.ProcessCommand(
                () =>
                {
                    if (this.RotatorHardware.InterfaceVersionSetting.Value < 3 && !this.RotatorHardware.CanReverse.Value)
                    {
                        throw new PropertyNotImplementedException();
                    }

                    this.RotatorHardware.Reverse.Value = value;
                }, DeviceType, MemberNames.Reverse, "Set");
            }
        }

        /// <summary>
        /// Gets the rotator step size.
        /// </summary>
        public float StepSize
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    return this.RotatorHardware.StepSize;
                }, DeviceType, MemberNames.StepSize, "Get");
            }
        }

        /// <summary>
        /// Gets the rotator Target Position.
        /// </summary>
        public float TargetPosition
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    return this.RotatorHardware.TargetPosition;
                }, DeviceType, MemberNames.TargetPosition, "Get");
            }
        }

        /// <summary>
        /// Gets the Rotators position with any Sync Offsets applied.
        /// </summary>
        public float Position
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    return (this.RotatorHardware.Position.Value + this.RotatorHardware.SyncOffset.Value + 360) % 360;
                }, DeviceType, MemberNames.Position, "Get");
            }
        }

        /// <summary>
        /// Gets the device's operational state in one call.
        /// </summary>
        public override List<StateValue> DeviceState
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    // Create an array list to hold the IStateValue entries
                    List<StateValue> deviceState = new List<StateValue>();

                    try
                    {
                        deviceState.Add(new StateValue(nameof(IRotatorV4.IsMoving), this.IsMoving));
                    }
                    catch
                    {
                    }

                    try
                    {
                        deviceState.Add(new StateValue(nameof(IRotatorV4.MechanicalPosition), this.MechanicalPosition));
                    }
                    catch
                    {
                    }

                    try
                    {
                        deviceState.Add(new StateValue(nameof(IRotatorV4.Position), this.Position));
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

        /// <summary>
        /// Halts the rotator.
        /// </summary>
        public void Halt()
        {
            this.ProcessCommand(
                () =>
                {
                    this.RotatorHardware.Halt();
                }, DeviceType, MemberNames.Halt, "Command");
        }

        /// <summary>
        /// Moves the rotator.
        /// </summary>
        /// <param name="position">Target position.</param>
        public void Move(float position)
        {
            this.ProcessCommand(
                () =>
                {
                    this.RotatorHardware.Move(position);
                }, DeviceType, MemberNames.Move, $"Command to {position}");
        }

        /// <summary>
        /// Moves the rotator.
        /// </summary>
        /// <param name="position">Target position.</param>
        public void MoveMechanical(float position)
        {
            this.ProcessCommand(
                () =>
                {
                    this.RotatorHardware.MoveAbsolute(position);
                }, DeviceType, MemberNames.MoveMechanical, $"Command to {position}");
        }

        /// <summary>
        /// Moves the rotator to target absolute location.
        /// </summary>
        /// <param name="position">The target position.</param>
        public void MoveAbsolute(float position)
        {
            this.ProcessCommand(
                () =>
                {
                    this.RotatorHardware.MoveAbsolute((position - this.RotatorHardware.SyncOffset.Value + 36000) % 360);
                }, DeviceType, MemberNames.MoveAbsolute, $"Command to {position}");
        }

        /// <summary>
        /// Syncs the rotator to a target location without moving it.
        /// </summary>
        /// <param name="position">The position to sync to.</param>
        public void Sync(float position)
        {
            this.ProcessCommand(
                () =>
                {
                    this.RotatorHardware.SyncOffset.Value = position - this.RotatorHardware.Position.Value;
                }, DeviceType, MemberNames.Sync, $"Command to {position}");
        }

        /// <summary>
        /// Connects to the hardware.
        /// </summary>
        public override void Connect()
        {
            base.ConnectTimer.Interval = RotatorHardware.ConnectDelay.Value;
            this.RotatorHardware.Connected = true;
            base.Connect();
        }

        /// <summary>
        /// Disconnects from the hardware.
        /// </summary>
        public override void Disconnect()
        {
            this.RotatorHardware.Connected = false;
            base.Disconnect();
        }
    }
}