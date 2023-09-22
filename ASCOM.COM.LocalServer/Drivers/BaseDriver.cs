using ASCOM.Common.DeviceInterfaces;
using Newtonsoft.Json.Linq;
using System.Security.Policy;

namespace ASCOM.Simulators.LocalServer.Drivers
{
    public abstract class BaseDriver : IAscomDeviceV2
    {
        internal virtual IAscomDeviceV2 DeviceV2 { get; }

        public bool Connecting => DeviceV2.Connecting;

        public IList<IStateValue> DeviceState => DeviceV2.DeviceState;

        public bool Connected { get => DeviceV2.Connected; set => DeviceV2.Connected = value; }

        public string Description => DeviceV2.Description;

        public string DriverInfo => DeviceV2.DriverInfo;

        public string DriverVersion => DeviceV2.DriverVersion;

        public short InterfaceVersion => DeviceV2.InterfaceVersion;

        public string Name => DeviceV2.Name;

        public IList<string> SupportedActions => DeviceV2.SupportedActions;

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

        public void Connect()
        {
            DeviceV2.Connect();
        }

        public void Disconnect()
        {
            DeviceV2.Disconnect();
        }

        public void Dispose()
        {
            DeviceV2.Dispose();
        }
    }
}
