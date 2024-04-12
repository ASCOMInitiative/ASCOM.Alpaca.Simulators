using ASCOM.DeviceInterface;
using System.Runtime.InteropServices;

namespace OmniSim.LocalServer.Drivers
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class CoverCalibrator : BaseDriver, ASCOM.DeviceInterface.ICoverCalibratorV2, IDisposable
    {
        public ASCOM.Common.DeviceInterfaces.ICoverCalibratorV2 Device => (base.DeviceV2 as ASCOM.Common.DeviceInterfaces.ICoverCalibratorV2);

        public CoverStatus CoverState => (CoverStatus)Device.CoverState;

        public CalibratorStatus CalibratorState => (CalibratorStatus)Device.CalibratorState;

        public int Brightness => Device.Brightness;

        public int MaxBrightness => Device.MaxBrightness;

        public static Func<ASCOM.Common.DeviceInterfaces.IAscomDeviceV2> DeviceAccess;

        public CoverCalibrator()
        {
            base.GetDevice = DeviceAccess;
        }

        public void OpenCover()
        {
            Device.OpenCover();
        }

        public void CloseCover()
        {
            Device.CloseCover();
        }

        public void HaltCover()
        {
            Device.HaltCover();
        }

        public void CalibratorOn(int Brightness)
        {
            Device.CalibratorOn(Brightness);
        }

        public void CalibratorOff()
        {
            Device.CalibratorOff();
        }
        public bool CalibratorChanging
        {
            get
            {
                return Device.CalibratorChanging;
            }
        }
        public bool CoverMoving
        {
            get
            {
                return Device.CoverMoving;
            }
        }
    }
}
