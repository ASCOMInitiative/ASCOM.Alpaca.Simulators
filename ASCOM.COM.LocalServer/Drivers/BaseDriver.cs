using ASCOM.Common;
using ASCOM.Common.DeviceInterfaces;
using ASCOM.LocalServer;
using System.Collections;
using System.Security.Policy;

namespace ASCOM.Simulators.LocalServer.Drivers
{
    public class BaseDriver : ReferenceCountedObjectBase, IDisposable
    {
#if ASCOM_7_PREVIEW
        internal Func<IAscomDeviceV2> GetDevice;
#else
        internal Func<IAscomDevice> GetDevice;

#endif

#if ASCOM_7_PREVIEW
        internal IAscomDeviceV2 DeviceV2 { get => GetDevice(); }

        public bool Connecting => DeviceV2.Connecting;

        public IList<IStateValue> DeviceState => DeviceV2.DeviceState;

        public void Connect()
        {
            DeviceV2.Connect();
        }

        public void Disconnect()
        {
            DeviceV2.Disconnect();
        }
#else
        internal IAscomDevice DeviceV2 { get => GetDevice(); }
#endif

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
