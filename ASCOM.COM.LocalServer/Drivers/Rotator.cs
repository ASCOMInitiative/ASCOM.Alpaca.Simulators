using System.Collections;
using System.Runtime.InteropServices;

namespace ASCOM.Simulators.LocalServer.Drivers
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class Rotator : BaseDriver, ASCOM.DeviceInterface.IRotatorV4, IDisposable
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

        public ASCOM.Common.DeviceInterfaces.IRotatorV4 Device => (base.DeviceV2 as ASCOM.Common.DeviceInterfaces.IRotatorV4);

        public bool CanReverse => Device.CanReverse;

        public bool IsMoving => Device.IsMoving;

        public float Position => Device.Position;

        public bool Reverse { get => Device.Reverse; set => Device.Reverse = value; }

        public float StepSize => Device.StepSize;

        public float TargetPosition => Device.TargetPosition;

        public float MechanicalPosition => Device.MechanicalPosition;
        
        public static Func<ASCOM.Common.DeviceInterfaces.IAscomDeviceV2> DeviceAccess;

        public Rotator()
        {
            base.GetDevice = DeviceAccess;
        }

        public void Halt()
        {
            Device.Halt();
        }

        public void Move(float Position)
        {
            Device.Move(Position);
        }

        public void MoveAbsolute(float Position)
        {
            Device.MoveAbsolute(Position);
        }

        public void Sync(float Position)
        {
            Device.Sync(Position);
        }

        public void MoveMechanical(float Position)
        {
            Device.MoveMechanical(Position);
        }
    }
}
