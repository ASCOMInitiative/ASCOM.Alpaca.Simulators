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
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterWheel"/> class.
        /// </summary>
        /// <param name="deviceNumber">The device number from the Alpaca API instance. Used for log files and settings.</param>
        /// <param name="logger">An ASCOM Logger for this to write calls to.</param>
        /// <param name="profile">An ASCOM Profile for this driver to store information to.</param>
        public FilterWheel(int deviceNumber, ILogger logger, IProfile profile)
            : base(deviceNumber, logger, profile, 3, 2)
        {
            this.DeviceNumber = deviceNumber;

            logger.LogInformation($"FilterWheel {deviceNumber} - Starting initialization");

            this.FilterWheelHardware = new FilterWheelHardware(logger, profile);
        }

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
                },
                nameof(IFilterWheelV3.Description),
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
                    return "ASCOM filter wheel driver simulator";
                },
                nameof(IFilterWheelV3.DriverInfo),
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
                    return this.FilterWheelHardware.InterfaceVersion.Value;
                },
                nameof(IFilterWheelV3.InterfaceVersion),
                "Get",
                2);
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
                    return "Alpaca Filter Wheel Simulator";
                },
                nameof(IFilterWheelV3.Name),
                "Get",
                2);
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
                },
                nameof(IFilterWheelV3.Position),
                "Get",
                1);
            }

            set
            {
                this.ProcessCommand(
                () =>
                {
                    this.FilterWheelHardware.Position = value;
                },
                nameof(IFilterWheelV3.Position),
                "Set",
                1);
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
                },
                nameof(IFilterWheelV3.FocusOffsets),
                "Set",
                1);
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
                },
                nameof(IFilterWheelV3.Names),
                "Get",
                1);
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
                },
                nameof(IFilterWheelV3.DeviceState),
                "Get",
                3);
            }
        }

        #endregion IFilterWheelV3 members
    }
}