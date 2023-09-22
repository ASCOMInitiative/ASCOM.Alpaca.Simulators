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

        public bool IsSafe => (base.DeviceV2 as ISafetyMonitorV3).IsSafe;

        public SafetyMonitor()
        {
            base.GetDevice = () => ASCOM.Alpaca.DeviceManager.GetSafetyMonitor(0);
        }
    }
}
