﻿using ASCOM.Common;
using ASCOM.Common.DeviceInterfaces;
using ASCOM;
using System.Collections;
using System.Security.Policy;

namespace OmniSim.LocalServer.Drivers
{
    public class BaseDriver : ReferenceCountedObjectBase, IDisposable
    {
        internal Func<IAscomDeviceV2> GetDevice;

        internal IAscomDeviceV2 DeviceV2 { get => GetDevice(); }

        public bool Connecting => DeviceV2.Connecting;

        public ASCOM.DeviceInterface.IStateValueCollection DeviceState
        {
            get
            {
                // The StateValue class used by the ASCOM library is different to the COM visible StateValue class which is installed by the ASCOM Platform only on the Windows OS.
                // The following code converts the OmniSim / ASCOM Library version of StateValue into the COM visible Windows version for return to COM clients by the OmniSim local server.

                // Iterate over the simulator's list of ASCOM.Common.DeviceInterfaces.StateValue response instances, convert each to an ASCOM.DeviceInterface.StateValue instance and add it to the response ArrayList
                List<ASCOM.DeviceInterface.IStateValue> stateValues = new List<ASCOM.DeviceInterface.IStateValue>();
                foreach(StateValue value in DeviceV2.DeviceState)
                {
                    ASCOM.DeviceInterface.StateValue stateValue=new ASCOM.DeviceInterface.StateValue(value.Name,value.Value);
                    stateValues.Add(stateValue);
                }

                return new ASCOM.DeviceInterface.StateValueCollection(stateValues);
            }
        }

        public void Connect()
        {
            DeviceV2.Connect();
        }

        public void Disconnect()
        {
            DeviceV2.Disconnect();
        }

        public ArrayList SupportedActions => new ArrayList(DeviceV2.SupportedActions.ToArray());

        public bool Connected { get => DeviceV2.Connected; set => DeviceV2.Connected = value; }

        public string Description => DeviceV2.Description;

        public string DriverInfo => DeviceV2.DriverInfo;

        public string DriverVersion => DeviceV2.DriverVersion;

        public short InterfaceVersion => DeviceV2.InterfaceVersion;

        public string Name => DeviceV2.Name;

        public string Action(string ActionName, string ActionParameters)
        {
            return DeviceV2.Action(ActionName, ActionParameters);
        }

        public void CommandBlind(string Command, bool Raw = false)
        {
            DeviceV2.CommandBlind(Command, Raw);
        }

        public bool CommandBool(string Command, bool Raw = false)
        {
            return DeviceV2.CommandBool(Command, Raw);
        }

        public string CommandString(string Command, bool Raw = false)
        {
            return DeviceV2.CommandString(Command, Raw);
        }

        public void Dispose()
        {
            DeviceV2.Dispose();
        }

        public void SetupDialog()
        {
            try
            {
                NativeMethods.MessageBox(System.IntPtr.Zero, $"The Device Simulator can be configured through the Alpaca Web UI. This will block until dismissed allowing changes while the client is waiting.", "Setup Dialog", 0);
            }
            catch
            {
            }
        }
    }
}
