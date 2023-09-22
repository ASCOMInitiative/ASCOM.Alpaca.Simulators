using ASCOM.Common.DeviceInterfaces;

namespace ASCOM.Simulators.LocalServer.Drivers
{
    public class SafetyMonitor : BaseDriver, ISafetyMonitorV3
    {
        internal override ISafetyMonitorV3 DeviceV2 => ASCOM.Alpaca.DeviceManager.GetSafetyMonitor(0);

        public bool IsSafe => DeviceV2.IsSafe;
    }
}
