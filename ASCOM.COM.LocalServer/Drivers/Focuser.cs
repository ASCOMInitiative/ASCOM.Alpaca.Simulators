using ASCOM.Common.DeviceInterfaces;
using System.Collections;
using System.Runtime.InteropServices;

namespace ASCOM.Simulators.LocalServer.Drivers
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class Focuser : BaseDriver, ASCOM.DeviceInterface.IFocuserV4, IDisposable
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

        public IFocuserV4 Device => (base.DeviceV2 as IFocuserV4);

        public static Func<ASCOM.Common.DeviceInterfaces.IAscomDeviceV2> DeviceAccess;

        public Focuser()
        {
            base.GetDevice = DeviceAccess;
        }

        public bool Absolute => Device.Absolute;

        public bool IsMoving => Device.IsMoving;

        public bool Link { get => Device.Connected; set => Device.Connected = value; }

        public int MaxIncrement => Device.MaxIncrement;

        public int MaxStep => Device.MaxStep;

        public int Position => Device.Position;

        public double StepSize => Device.StepSize;

        public bool TempComp { get => Device.TempComp; set => Device.TempComp = value; }

        public bool TempCompAvailable => Device.TempCompAvailable;

        public double Temperature => Device.Temperature;

        public void Halt()
        {
            Device.Halt();
        }

        public void Move(int Position)
        {
            Device.Move(Position);
        }
    }
}
