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
    public class Rotator : BaseDriver, ASCOM.DeviceInterface.IRotatorV4, IDisposable
    {
        ASCOM.Common.DeviceInterfaces.IRotatorV4 Device = new ASCOM.Com.DriverAccess.Rotator("OmniSim.Rotator");

        public Rotator()
        {
            base.GetDevice = () => (ASCOM.Common.DeviceInterfaces.IAscomDeviceV2)Device;
        }

		public bool CanReverse => Device.CanReverse;

		public bool IsMoving => Device.IsMoving;

		public float Position => Device.Position;

		public bool Reverse { get => Device.Reverse; set => Device.Reverse = value; }

		public float StepSize => Device.StepSize;

		public float TargetPosition => Device.TargetPosition;

		public float MechanicalPosition => Device.MechanicalPosition;

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

		public void MoveMechanical(float Position)
		{
			Device.MoveMechanical(Position);
		}

		public void Sync(float Position)
		{
			Device.Sync(Position);
		}
	}
}
