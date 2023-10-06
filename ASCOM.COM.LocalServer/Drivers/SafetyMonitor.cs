using ASCOM.Common.DeviceInterfaces;
using ASCOM.LocalServer;
using System.Collections;
using System.Runtime.InteropServices;

namespace ASCOM.Simulators.LocalServer.Drivers
{

    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class SafetyMonitor : BaseDriver, ASCOM.DeviceInterface.ISafetyMonitor, IDisposable
    {

        public bool IsSafe => (base.DeviceV2 as ISafetyMonitor).IsSafe;

#if ASCOM_7_PREVIEW
        public static Func<ASCOM.Common.DeviceInterfaces.IAscomDeviceV2> DeviceAccess;
#else
        public static Func<ASCOM.Common.DeviceInterfaces.IAscomDevice> DeviceAccess;
#endif

        public SafetyMonitor()
        {
            base.GetDevice = DeviceAccess;
        }
    }
}
