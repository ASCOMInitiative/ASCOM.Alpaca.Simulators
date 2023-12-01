using System.Collections;
using System.Runtime.InteropServices;

namespace ASCOM.Simulators.LocalServer.Drivers
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class FilterWheel : BaseDriver, ASCOM.DeviceInterface.IFilterWheelV3, IDisposable
    {
        public ArrayList DeviceState
        {
            get
            {
                // The StateValue class used by the ASCOM library is different to the COM visible StateValue class which is installed by the ASCOM Platform only on the Windows OS.
                // The following code converts the OmniSim / ASCOM Library version of StateValue into the COM visible Windows version for return to COM clients by the OmniSim local server.

                // Create an empty return list in case the device does not return any state values
                ArrayList returnValue = new ArrayList();

                // Iterate over the simulator's list of ASCOM.Common.DeviceInterfaces.StateValue response instances, convert each to an ASCOM.DeviceInterface.StateValue instance and add it to the response ArrayList
                foreach (ASCOM.Common.DeviceInterfaces.StateValue value in DeviceV2.DeviceState)
                {
                    DeviceInterface.StateValue stateValue = new DeviceInterface.StateValue(value.Name, value.Value);
                    returnValue.Add(stateValue);
                }

                return returnValue;
            }
        }

        public ASCOM.Common.DeviceInterfaces.IFilterWheelV3 Device => (base.DeviceV2 as ASCOM.Common.DeviceInterfaces.IFilterWheelV3);

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
