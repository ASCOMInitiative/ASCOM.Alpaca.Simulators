using ASCOM.DeviceInterface;
using System.Runtime.InteropServices;

namespace ASCOM.Simulators.LocalServer.Drivers
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class CoverCalibrator : BaseDriver, ASCOM.DeviceInterface.ICoverCalibratorV1, IDisposable
    {
        public ASCOM.Common.DeviceInterfaces.ICoverCalibratorV1 Device => (base.DeviceV2 as ASCOM.Common.DeviceInterfaces.ICoverCalibratorV1);

        public CoverStatus CoverState => (CoverStatus)Device.CoverState;

        public CalibratorStatus CalibratorState => (CalibratorStatus)Device.CalibratorState;

        public int Brightness => Device.Brightness;

        public int MaxBrightness => Device.MaxBrightness;

        public CoverCalibrator()
        {
            base.GetDevice = () => ASCOM.Alpaca.DeviceManager.GetCoverCalibrator(0);
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
    }
}
