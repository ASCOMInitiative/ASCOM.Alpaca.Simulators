namespace ASCOM.Simulators
{
    using System;
    using System.Collections.Generic;

    using ASCOM.Common;
    using ASCOM.Common.DeviceInterfaces;
    using ASCOM.Common.Interfaces;

    /// <summary>
    /// An ASCOM FilterWheel Simulator.
    /// The original version was written in 2009 by Mark Crossly in VB.Net.
    /// This version supports Alpaca and interface versions 1-3.
    /// </summary>
    public class FilterWheel : OmniSim.BaseDriver.Driver, IFilterWheelV3, IAlpacaDevice, ISimulation // Early-bind interface implemented by this driver
    {
        private const string DriverName = "Alpaca Filter Wheel Simulator";

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterWheel"/> class.
        /// </summary>
        /// <param name="deviceNumber">The device number from the Alpaca API instance. Used for log files and settings.</param>
        /// <param name="logger">An ASCOM Logger for this to write calls to.</param>
        /// <param name="profile">An ASCOM Profile for this driver to store information to.</param>
        public FilterWheel(int deviceNumber, ILogger logger, IProfile profile)
            : base(deviceNumber, logger, profile)
        {
            this.DeviceNumber = deviceNumber;

            logger.LogInformation($"FilterWheel {deviceNumber} - Starting initialization");

            this.FilterWheelHardware = new FilterWheelHardware(logger, profile);
        }

        /// <summary>
        /// Gets what device this this driver exposes.
        /// </summary>
        public override DeviceTypes DeviceType { get; } = DeviceTypes.FilterWheel;

        /// <summary>
        /// Gets the underlying Hardware simulation.
        /// </summary>
        public FilterWheelHardware FilterWheelHardware
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets an interface version for V1 drivers that would throw on a InterfaceVersion Call.
        /// </summary>
        public override short SafeInterfaceVersion
        {
            get
            {
                return this.FilterWheelHardware.InterfaceVersion.Value;
            }
        }

        public override string DeviceName { get { return $"{DriverName} - {DeviceNumber}"; } }

        #region IFilterWheelV2 members

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
                    return "A simulator for the ASCOM FilterWheel API usable with Alpaca and COM";
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
                    return "ASCOM filter wheel driver simulator";
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
                    return this.FilterWheelHardware.InterfaceVersion.Value;
                }, DeviceType, MemberNames.InterfaceVersion, "Get");
            }
        }

        /// <summary>
        /// Gets the ASCOM Driver Name.
        /// </summary>
        public override string Name
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    return DriverName;
                }, DeviceType, MemberNames.Name, "Get");
            }
        }

        /// <summary>
        /// Gets or Sets the FilterWheel Position.
        /// </summary>
        public short Position
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    return this.FilterWheelHardware.Position;
                }, DeviceType, MemberNames.Position, "Get");
            }

            set
            {
                this.ProcessCommand(
                () =>
                {
                    this.FilterWheelHardware.Position = value;
                }, DeviceType, MemberNames.Position, "Set");
            }
        }

        /// <summary>
        /// Gets the FilterWheel Offsets.
        /// </summary>
        public int[] FocusOffsets
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    return this.FilterWheelHardware.FocusOffsets;
                }, DeviceType, MemberNames.FocusOffsets, "Get");
            }
        }

        /// <summary>
        /// Gets the Filter Names.
        /// </summary>
        public string[] Names
        {
            get
            {
                return this.ProcessCommand(
                () =>
                {
                    return this.FilterWheelHardware.FilterNames;
                }, DeviceType, MemberNames.Names, "Get");
            }
        }

        #endregion IFilterWheelV2 members

        #region IFilterWheelV3 members

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
                    List<StateValue> deviceState = [];

                    try
                    {
                        deviceState.Add(new StateValue(nameof(IFilterWheelV3.Position), this.Position));
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

        #endregion IFilterWheelV3 members

        /// <summary>
        /// Connects to the hardware.
        /// </summary>
        public override void Connect()
        {
            base.ConnectTimer.Interval = FilterWheelHardware.ConnectDelay.Value;
            base.Connect();
        }

        /// <summary>
        /// Load settings.
        /// </summary>
        public override void LoadSettings()
        {
            this.FilterWheelHardware.LoadSettings();
        }
    }
}