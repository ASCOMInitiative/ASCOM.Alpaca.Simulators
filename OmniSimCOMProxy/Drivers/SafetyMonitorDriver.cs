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
    public class SafetyMonitor : BaseDriver, ASCOM.DeviceInterface.ISafetyMonitorV3, IDisposable
    {
        ASCOM.Common.DeviceInterfaces.ISafetyMonitorV3 Device = new ASCOM.Com.DriverAccess.SafetyMonitor("OmniSim.SafetyMonitor");

        public SafetyMonitor()
        {
            base.GetDevice = () => (ASCOM.Common.DeviceInterfaces.IAscomDeviceV2)Device;
        }

        public bool IsSafe => Device.IsSafe;
    }
}
