using ASCOM.Common.DeviceInterfaces;
using System.Runtime.InteropServices;

namespace OmniSim.LocalServer.Drivers
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class Focuser : BaseDriver, ASCOM.DeviceInterface.IFocuserV4, IDisposable
    {
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
