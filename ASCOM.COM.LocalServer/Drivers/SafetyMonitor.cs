using ASCOM.Common.DeviceInterfaces;
using System.Collections;
using System.Runtime.InteropServices;

namespace OmniSim.LocalServer.Drivers
{

    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class SafetyMonitor : BaseDriver, ASCOM.DeviceInterface.ISafetyMonitorV3, IDisposable
    {

        public bool IsSafe => (base.DeviceV2 as ISafetyMonitorV3).IsSafe;

        public static Func<ASCOM.Common.DeviceInterfaces.IAscomDeviceV2> DeviceAccess;

        public SafetyMonitor()
        {
            base.GetDevice = DeviceAccess;
        }
    }
}
