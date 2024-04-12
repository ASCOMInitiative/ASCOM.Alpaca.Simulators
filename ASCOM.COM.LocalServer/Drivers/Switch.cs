using System.Runtime.InteropServices;

namespace OmniSim.LocalServer.Drivers
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class Switch : BaseDriver, ASCOM.DeviceInterface.ISwitchV3, IDisposable
    {
        public ASCOM.Common.DeviceInterfaces.ISwitchV3 Device => (base.DeviceV2 as ASCOM.Common.DeviceInterfaces.ISwitchV3);

        public short MaxSwitch => Device.MaxSwitch;
        
        public static Func<ASCOM.Common.DeviceInterfaces.ISwitchV3> DeviceAccess;

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

        public void SetAsync(short id, bool state)
        {
            Device.SetAsync(id, state);
        }

        public void SetAsyncValue(short id, double value)
        {
            Device.SetAsyncValue(id, value);
        }

        public bool CanAsync(short id)
        {
            return Device.CanAsync(id);
        }

        public bool StateChangeComplete(short id)
        {
            return Device.StateChangeComplete(id);
        }

        public void CancelAsync(short id)
        {
            Device.CancelAsync(id);
        }
    }
}
