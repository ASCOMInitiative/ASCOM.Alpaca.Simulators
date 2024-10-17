using ASCOM.Common;
using ASCOM.Common.DeviceInterfaces;
using ASCOM.LocalServer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;

namespace ASCOM.Simulators.LocalServer.Drivers
{
    public class BaseDriver : ReferenceCountedObjectBase, IDisposable
    {
        internal Func<IAscomDeviceV2> GetDevice;

        internal IAscomDeviceV2 DeviceV2 { get => GetDevice(); }

        public bool Connecting => DeviceV2.Connecting;

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

        public DeviceInterface.IStateValueCollection DeviceState
        {
            get
            {
                // The StateValue class used by the ASCOM library is different to the COM visible StateValue class which is installed by the ASCOM Platform only on the Windows OS.
                // The following code converts the OmniSim / ASCOM Library version of StateValue into the COM visible Windows version for return to COM clients by the OmniSim local server.

                // Iterate over the simulator's list of ASCOM.Common.DeviceInterfaces.StateValue response instances, convert each to an ASCOM.DeviceInterface.StateValue instance and add it to the response ArrayList
                List<DeviceInterface.IStateValue> stateValues = new List<DeviceInterface.IStateValue>();
                foreach (StateValue value in DeviceV2.DeviceState)
                {
                    DeviceInterface.StateValue stateValue = new DeviceInterface.StateValue(value.Name, value.Value);
                    stateValues.Add(stateValue);
                }

                return new DeviceInterface.StateValueCollection(stateValues);
            }
        }

        public void SetupDialog()
        {
            try
            {
                System.Windows.Forms.MessageBox.Show($"The Device Simulator is configured through the Alpaca Web UI by starting a browser and navigating to: http://localhost:32323\r\n\r\n" +
                    $"Press the OK button when you have finished making changes through the Web UI.", "Setup Dialog", 0);
            }
            catch
            {
            }
        }
    }
}
