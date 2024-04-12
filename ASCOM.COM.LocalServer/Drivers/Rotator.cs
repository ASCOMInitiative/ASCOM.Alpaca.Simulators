using System.Runtime.InteropServices;

namespace OmniSim.LocalServer.Drivers
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class Rotator : BaseDriver, ASCOM.DeviceInterface.IRotatorV4, IDisposable
    {
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
