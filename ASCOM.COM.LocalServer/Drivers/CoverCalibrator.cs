using ASCOM.DeviceInterface;
using System.Collections;
using System.Runtime.InteropServices;

namespace ASCOM.Simulators.LocalServer.Drivers
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class CoverCalibrator : BaseDriver, ASCOM.DeviceInterface.ICoverCalibratorV2, IDisposable
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
        public bool CalibratorReady
        {
            get
            {
                return Device.CalibratorReady;
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
