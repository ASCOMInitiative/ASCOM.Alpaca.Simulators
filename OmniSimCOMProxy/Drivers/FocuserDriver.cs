using ASCOM.DeviceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ASCOM.Simulators.LocalServer.Drivers
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class Focuser : BaseDriver, ASCOM.DeviceInterface.IFocuserV4, IDisposable
    {
        ASCOM.Common.DeviceInterfaces.IFocuserV4 Device = new ASCOM.Com.DriverAccess.Focuser("OmniSim.Focuser");

        public Focuser()
        {
            base.GetDevice = () => (ASCOM.Common.DeviceInterfaces.IAscomDeviceV2)Device;
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
