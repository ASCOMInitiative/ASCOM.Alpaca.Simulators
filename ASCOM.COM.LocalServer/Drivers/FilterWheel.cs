using System.Runtime.InteropServices;

namespace ASCOM.Simulators.LocalServer.Drivers
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class FilterWheel : BaseDriver, ASCOM.DeviceInterface.IFilterWheelV2, IDisposable
    {
        public ASCOM.Common.DeviceInterfaces.IFilterWheelV2 Device => (base.DeviceV2 as ASCOM.Common.DeviceInterfaces.IFilterWheelV2);

        public static Func<ASCOM.Common.DeviceInterfaces.IAscomDeviceV2> DeviceAccess;

        public FilterWheel()
        {
            base.GetDevice = DeviceAccess;
        }

        public int[] FocusOffsets => Device.FocusOffsets;

        public string[] Names => Device.Names;

        public short Position { get => Device.Position; set => Device.Position = value; }
    }
}
