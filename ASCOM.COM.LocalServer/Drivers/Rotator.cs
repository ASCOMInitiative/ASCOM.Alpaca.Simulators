using System.Runtime.InteropServices;

namespace ASCOM.Simulators.LocalServer.Drivers
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class Rotator : BaseDriver, ASCOM.DeviceInterface.IRotatorV3, IDisposable
    {
        public ASCOM.Common.DeviceInterfaces.IRotatorV3 Device => (base.DeviceV2 as ASCOM.Common.DeviceInterfaces.IRotatorV3);

        public bool CanReverse => Device.CanReverse;

        public bool IsMoving => Device.IsMoving;

        public float Position => Device.Position;

        public bool Reverse { get => Device.Reverse; set => Device.Reverse = value; }

        public float StepSize => Device.StepSize;

        public float TargetPosition => Device.TargetPosition;

        public float MechanicalPosition => Device.MechanicalPosition;

#if ASCOM_7_PREVIEW
        public static Func<ASCOM.Common.DeviceInterfaces.IAscomDeviceV2> DeviceAccess;
#else
        public static Func<ASCOM.Common.DeviceInterfaces.IAscomDevice> DeviceAccess;
#endif

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
