using ASCOM.DeviceInterface;
using System.Runtime.InteropServices;

namespace ASCOM.Simulators.LocalServer.Drivers
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class Dome : BaseDriver, ASCOM.DeviceInterface.IDomeV2, IDisposable
    {
        public ASCOM.Common.DeviceInterfaces.IDomeV2 Device => (base.DeviceV2 as ASCOM.Common.DeviceInterfaces.IDomeV2);

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
