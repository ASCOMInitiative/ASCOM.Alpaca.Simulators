using System.Collections;
using System.Runtime.InteropServices;

namespace ASCOM.Simulators.LocalServer.Drivers
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class ObservingConditions : BaseDriver, ASCOM.DeviceInterface.IObservingConditionsV2, IDisposable
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

        public ASCOM.Common.DeviceInterfaces.IObservingConditionsV2 Device => (base.DeviceV2 as ASCOM.Common.DeviceInterfaces.IObservingConditionsV2);

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
