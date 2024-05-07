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
    public class ObservingConditions : BaseDriver, ASCOM.DeviceInterface.IObservingConditionsV2, IDisposable
    {
        ASCOM.Common.DeviceInterfaces.IObservingConditionsV2 Device = new ASCOM.Com.DriverAccess.ObservingConditions("OmniSim.ObservingConditions");

        public ObservingConditions()
        {
            base.GetDevice = () => (ASCOM.Common.DeviceInterfaces.IAscomDeviceV2)Device;
        }

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

		public void Refresh()
		{
			Device.Refresh();
		}

		public string SensorDescription(string PropertyName)
		{
			return Device.SensorDescription(PropertyName);
		}

		public double TimeSinceLastUpdate(string PropertyName)
		{
			return Device.TimeSinceLastUpdate(PropertyName);
		}
	}
}
