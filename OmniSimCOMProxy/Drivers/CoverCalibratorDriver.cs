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
    public class CoverCalibrator : BaseDriver, ASCOM.DeviceInterface.ICoverCalibratorV2, IDisposable
    {
        ASCOM.Common.DeviceInterfaces.ICoverCalibratorV2 Device = new ASCOM.Com.DriverAccess.CoverCalibrator("OmniSim.CoverCalibrator");

        public CoverCalibrator()
        {
            base.GetDevice = () => (ASCOM.Common.DeviceInterfaces.IAscomDeviceV2)Device;
        }

        public CoverStatus CoverState => (CoverStatus) Device.CoverState;

        public CalibratorStatus CalibratorState => (CalibratorStatus) Device.CalibratorState;

        public int Brightness => Device.Brightness;

        public int MaxBrightness => Device.MaxBrightness;

        public bool CalibratorChanging => Device.CalibratorChanging;

        public bool CoverMoving => Device.CoverMoving;

        public void CalibratorOff()
        {
            Device.CalibratorOff();
        }

        public void CalibratorOn(int Brightness)
        {
            Device.CalibratorOn(Brightness);
        }

        public void CloseCover()
        {
            Device?.CloseCover();
        }

        public void HaltCover()
        {
            Device.HaltCover();
        }

        public void OpenCover()
        {
            Device.OpenCover();
        }
    }
}
