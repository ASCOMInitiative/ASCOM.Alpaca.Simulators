using System.Runtime.InteropServices;

namespace ASCOM.Simulators.LocalServer.Drivers
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class Switch : BaseDriver, ASCOM.DeviceInterface.ISwitchV2, IDisposable
    {
        public ASCOM.Common.DeviceInterfaces.ISwitchV2 Device => (base.DeviceV2 as ASCOM.Common.DeviceInterfaces.ISwitchV2);

        public short MaxSwitch => Device.MaxSwitch;

        public static Func<ASCOM.Common.DeviceInterfaces.IAscomDeviceV2> DeviceAccess;

        public Switch()
        {
            base.GetDevice = DeviceAccess;
        }

        public string GetSwitchName(short id)
        {
            return Device.GetSwitchDescription(id);
        }

        public void SetSwitchName(short id, string name)
        {
            Device.SetSwitchName(id, name);
        }

        public string GetSwitchDescription(short id)
        {
            return Device.GetSwitchDescription(id);
        }

        public bool CanWrite(short id)
        {
            return Device.CanWrite(id);
        }

        public bool GetSwitch(short id)
        {
            return Device.GetSwitch(id);
        }

        public void SetSwitch(short id, bool state)
        {
            Device.SetSwitch(id, state);
        }

        public double MaxSwitchValue(short id)
        {
            return Device.MaxSwitchValue(id);
        }

        public double MinSwitchValue(short id)
        {
            return Device.MinSwitchValue(id);
        }

        public double SwitchStep(short id)
        {
            return Device.SwitchStep(id);
        }

        public double GetSwitchValue(short id)
        {
            return Device.GetSwitchValue(id);
        }

        public void SetSwitchValue(short id, double value)
        {
            Device.SetSwitchValue(id, value);
        }
    }
}
