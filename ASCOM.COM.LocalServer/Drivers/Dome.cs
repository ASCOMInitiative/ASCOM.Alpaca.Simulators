using ASCOM.DeviceInterface;
using System.Collections;
using System.Runtime.InteropServices;

namespace ASCOM.Simulators.LocalServer.Drivers
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class Dome : BaseDriver, ASCOM.DeviceInterface.IDomeV3, IDisposable
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

        public ASCOM.Common.DeviceInterfaces.IDomeV3 Device => (base.DeviceV2 as ASCOM.Common.DeviceInterfaces.IDomeV3);

        public double Altitude => Device.Altitude;

        public bool AtHome => Device.AtHome;

        public bool AtPark => Device.AtPark;

        public double Azimuth => Device.Azimuth;

        public bool CanFindHome => Device.CanFindHome;

        public bool CanPark => Device.CanPark;

        public bool CanSetAltitude => Device.CanSetAltitude;

        public bool CanSetAzimuth => Device.CanSetAzimuth;

        public bool CanSetPark => Device.CanSetPark;

        public bool CanSetShutter => Device.CanSetShutter;

        public bool CanSlave => Device.CanSlave;

        public bool CanSyncAzimuth => Device.CanSyncAzimuth;

        public ShutterState ShutterStatus => (ShutterState)Device.ShutterStatus;

        public bool Slaved { get => Device.Slaved; set => Device.Slaved = value; }

        public bool Slewing => Device.Slewing;

        public static Func<ASCOM.Common.DeviceInterfaces.IAscomDeviceV2> DeviceAccess;

        public Dome()
        {
            base.GetDevice = DeviceAccess;
        }

        public void AbortSlew()
        {
            Device.AbortSlew();
        }

        public void CloseShutter()
        {
            Device.CloseShutter();
        }

        public void FindHome()
        {
            Device.FindHome();
        }

        public void OpenShutter()
        {
            Device.OpenShutter();
        }

        public void Park()
        {
            Device.Park();
        }

        public void SetPark()
        {
            Device.SetPark();
        }

        public void SlewToAltitude(double Altitude)
        {
            Device.SlewToAltitude(Altitude);
        }

        public void SlewToAzimuth(double Azimuth)
        {
            Device.SlewToAzimuth(Azimuth);
        }

        public void SyncToAzimuth(double Azimuth)
        {
            Device.SyncToAzimuth(Azimuth);
        }
    }
}
