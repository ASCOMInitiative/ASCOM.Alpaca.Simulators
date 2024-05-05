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
    public class FilterWheel : BaseDriver, ASCOM.DeviceInterface.IFilterWheelV3, IDisposable
    {
        ASCOM.Common.DeviceInterfaces.IFilterWheelV3 Device = new ASCOM.Com.DriverAccess.FilterWheel("OmniSim.FilterWheel");

        public FilterWheel()
        {
            base.GetDevice = () => (ASCOM.Common.DeviceInterfaces.IAscomDeviceV2)Device;
        }

		public int[] FocusOffsets => Device.FocusOffsets;

		public string[] Names => Device.Names;

		public short Position { get => Device.Position; set => Device.Position = value; }
	}
}
