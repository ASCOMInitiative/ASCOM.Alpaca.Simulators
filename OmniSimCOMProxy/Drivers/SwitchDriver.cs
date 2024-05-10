using ASCOM.DeviceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ASCOM.Simulators.LocalServer.Drivers
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class Switch : BaseDriver, ASCOM.DeviceInterface.ISwitchV3, IDisposable
    {
        ASCOM.Common.DeviceInterfaces.ISwitchV3 Device = new ASCOM.Com.DriverAccess.Switch("OmniSim.Switch");

        public Switch()
        {
            base.GetDevice = () => (ASCOM.Common.DeviceInterfaces.IAscomDeviceV2)Device;
        }

		public short MaxSwitch => Device.MaxSwitch;

		public bool CanAsync(short id)
		{
			return Device.CanAsync(id);
		}

		public void CancelAsync(short id)
		{
			Device.CancelAsync(id);
		}

		public bool CanWrite(short id)
		{
			return Device.CanWrite(id);
		}

		public bool GetSwitch(short id)
		{
			return Device.GetSwitch(id);
		}

		public string GetSwitchDescription(short id)
		{
			return Device.GetSwitchDescription(id);
		}

		public string GetSwitchName(short id)
		{
			return Device.GetSwitchName(id);
		}

		public double GetSwitchValue(short id)
		{
			return Device.GetSwitchValue(id);
		}

		public double MaxSwitchValue(short id)
		{
			return Device.MaxSwitchValue(id);
		}

		public double MinSwitchValue(short id)
		{
			return Device.MinSwitchValue(id);
		}

		public void SetAsync(short id, bool state)
		{
			Device.SetAsync(id, state);
		}

		public void SetAsyncValue(short id, double value)
		{
			Device.SetAsyncValue(id, value);
		}

		public void SetSwitch(short id, bool state)
		{
			Device.SetSwitch(id, state);
		}

		public void SetSwitchName(short id, string name)
		{
			Device.SetSwitchName(id, name);
		}

		public void SetSwitchValue(short id, double value)
		{
			Device.SetSwitchValue(id, value);
		}

		public bool StateChangeComplete(short id)
		{
			return Device.StateChangeComplete(id);
		}

		public double SwitchStep(short id)
		{
			return Device.SwitchStep(id);
		}
	}
}
