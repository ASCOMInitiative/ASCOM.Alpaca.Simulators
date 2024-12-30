// tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM FilterWheel driver for FilterWheelSimulator
//
// Description:	A port of the VB6 ASCOM Filterwheel simulator to VB.Net.
// Converted and built in Visual Studio 2008.
// The port leaves some messy code - it could really do with
// a ground up re-write!
//
// Implements:	ASCOM FilterWheel interface version: 5.1.0
// Author:		Mark Crossley <mark@markcrossley.co.uk>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 06-Jun-2009	mpc	1.0.0	Initial edit, from FilterWheel template
// --------------------------------------------------------------------------------
//
// Your driver's ID is ASCOM.FilterWheelSim.FilterWheel  ???
//
// The Guid attribute sets the CLSID for ASCOM.FilterWheelSim.FilterWheel
// The ClassInterface/None addribute prevents an empty interface called
// _FilterWheel from being created and used as the [default] interface
//

using ASCOM.Common;
using ASCOM.Common.DeviceInterfaces;
using ASCOM.Common.Interfaces;
using System;
using System.Collections.Generic;

namespace ASCOM.Simulators
{
    public class FilterWheel : OmniSim.BaseDriver.Driver, IFilterWheelV3, IAlpacaDevice, ISimulation // Early-bind interface implemented by this driver
    {
        //
        // Constructor - Must be public for COM registration!
        //

        public FilterWheelHardware FilterWheelHardware
        {
            get;
            private set;
        }

        public FilterWheel(int deviceNumber, ILogger logger, IProfile profile) : base(deviceNumber, logger, profile)
        {
            DeviceNumber = deviceNumber;

            logger.LogInformation($"FilterWheel {deviceNumber} - Starting initialization");

            FilterWheelHardware = new FilterWheelHardware(logger, profile);

            FilterWheelHardware.Initialize();
        }

        #region IFilterWheelV2 members

        public override string Description
        {
            get
            {
                return "A simulator for the ASCOM FilterWheel API usable with Alpaca and COM";
            }
        }

        public override string DriverInfo
        {
            get
            {
                return "ASCOM filter wheel driver simulator";
            }
        }

        public override short InterfaceVersion
        {
            get
            {
                return 3;
            }
        }

        public override string Name
        {
            get
            {
                return "Alpaca Filter Wheel Simulator";
            }
        }

        public short Position
        {
            get
            {
                return FilterWheelHardware.Position;
            }
            set
            {
                FilterWheelHardware.Position = value;
            }
        }

        public int[] FocusOffsets
        {
            get
            {
                return FilterWheelHardware.FocusOffsets;
            }
        }

        public string[] Names
        {
            get
            {
                return FilterWheelHardware.FilterNames;
            }
        }

        public void SetupDialog()
        {
        }

        #endregion IFilterWheelV2 members

        #region IFilterWheelV3 members

        /// <summary>
        /// Return the device's operational state in one call
        /// </summary>
        public override List<StateValue> DeviceState
        {
            get
            {
                // Create an array list to hold the IStateValue entries
                List<StateValue> deviceState = new List<StateValue>();

                try { deviceState.Add(new StateValue(nameof(IFilterWheelV3.Position), Position)); } catch { }
                try { deviceState.Add(new StateValue(DateTime.Now)); } catch { }

                return deviceState;
            }
        }

        #endregion IFilterWheelV3 members
    }
}