using System.Runtime.InteropServices;

namespace ASCOM.Simulators.LocalServer.Drivers
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class ObservingConditions : BaseDriver, ASCOM.DeviceInterface.IObservingConditions, IDisposable
    {
        public ASCOM.Common.DeviceInterfaces.IObservingConditions Device => (base.DeviceV2 as ASCOM.Common.DeviceInterfaces.IObservingConditions);

        public double AveragePeriod { get => Device.AveragePeriod; set => Device.AveragePeriod = value; }

        public double CloudCover => Device.CloudCover;

        public double DewPoint => Device.DewPoint;

        public double Humidity => Device.Humidity;

        public double Pressure => Device.Pressure;

        public double RainRate => Device.RainRate;

        public double SkyBrightness => Device.SkyBrightness;

        public double SkyQuality => Device.SkyQuality;

        public double StarFWHM => Device.StarFWHM;

        public double SkyTemperature => Device.SkyTemperature;

        public double Temperature => Device.Temperature;

        public double WindDirection => Device.WindDirection;

        public double WindGust => Device.WindGust;

        public double WindSpeed => Device.WindSpeed;

        public static Func<ASCOM.Common.DeviceInterfaces.IAscomDeviceV2> DeviceAccess;

        public ObservingConditions()
        {
            base.GetDevice = DeviceAccess;
        }

        public double TimeSinceLastUpdate(string PropertyName)
        {
            return Device.TimeSinceLastUpdate(PropertyName);
        }

        public string SensorDescription(string PropertyName)
        {
            return Device.SensorDescription(PropertyName);
        }

        public void Refresh()
        {
            Device.Refresh();
        }
    }
}
