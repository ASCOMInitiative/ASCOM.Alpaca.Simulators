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
            : base(deviceNumber, logger, profile, RotatorName, 4, 2)
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
                },
                nameof(IRotatorV4.Description),
                "Get",
                2);
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
                },
                nameof(IRotatorV4.DriverInfo),
                "Get",
                2);
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
                },
                nameof(IRotatorV4.InterfaceVersion),
                "Get",
                2);
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
                },
                nameof(IRotatorV4.Name),
                "Get",
                2);
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
                },
                nameof(IRotatorV4.CanReverse),
                "Get",
                2);
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
                },
                nameof(IRotatorV4.IsMoving),
                "Get",
                1);
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
                },
                nameof(IRotatorV4.MechanicalPosition),
                "Get",
                3);
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
                },
                nameof(IRotatorV4.Reverse),
                "Get",
                1);
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
                },
                nameof(IRotatorV4.Reverse),
                "Set",
                1);
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
                },
                nameof(IRotatorV4.StepSize),
                "Get",
                1);
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
                },
                nameof(IRotatorV4.TargetPosition),
                "Get",
                1);
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
                },
                nameof(IRotatorV4.Position),
                "Get",
                1);
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
                },
                nameof(IRotatorV4.MoveMechanical),
                "Get",
                4);
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
                },
                nameof(IRotatorV4.Halt),
                "Command",
                1);
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
                },
                nameof(IRotatorV4.Move),
                $"Command to {position}",
                1);
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
                },
                nameof(IRotatorV4.MoveMechanical),
                $"Command to {position}",
                3);
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
                },
                nameof(IRotatorV4.MoveAbsolute),
                $"Command to {position}",
                1);
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
                },
                nameof(IRotatorV4.MoveMechanical),
                $"Command to {position}",
                3);
        }

        /// <summary>
        /// Connects to the hardware.
        /// </summary>
        public override void Connect()
        {
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