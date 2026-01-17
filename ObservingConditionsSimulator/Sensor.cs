using ASCOM.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ASCOM.Simulators
{
    /// <summary>
    /// Observing conditions simulator definition
    /// </summary>
    public class Sensor
    {
        public Sensor()
        {
            Readings = new List<OCSimulator.TimeValue>();
        }

        public Sensor(string Name)
        {
            Readings = new List<OCSimulator.TimeValue>();
            SensorName = Name;
        }

        public string SensorName { get; set; }
        public double SimCurrentValue { get; set; }
        public double SimToValue { get; set; }
        public double SimFromValue { get; set; }
        public double ValueCycleTime { get; set; }
        public bool IsImplemented { get; set; }
        public bool ShowNotReady { get; set; }
        public double NotReadyDelay { get; set; }
        public DateTime TimeOfLastUpdate { get; set; }
        public List<OCSimulator.TimeValue> Readings { get; set; }
        public OCSimulator.ValueCycleDirections ValueCycleDirection { get; set; }
        public bool Override { get; set; }
        public double OverrideValue { get; set; }

        public string Unit
        {
            get
            {
                try
                {
                    return OCSimulator.UnitString[SensorName];
                }
                catch
                {
                    return "Unknown";
                }
            }
        }

        public void ReadProfile(IProfile driverProfile)
        {

            SimFromValue = Convert.ToDouble(driverProfile.GetValue(SensorName + OCSimulator.SIMFROMVALUE_PROFILENAME, OCSimulator.SimulatorDefaultFromValues[SensorName].ToString(CultureInfo.InvariantCulture)), CultureInfo.InvariantCulture);

            SimToValue = Convert.ToDouble(driverProfile.GetValue(SensorName + OCSimulator.SIMTOVALUE_PROFILENAME, OCSimulator.SimulatorDefaultToValues[SensorName].ToString(CultureInfo.InvariantCulture)), CultureInfo.InvariantCulture);

            IsImplemented = Convert.ToBoolean(driverProfile.GetValue(SensorName + OCSimulator.IS_IMPLEMENTED_PROFILENAME, OCSimulator.IS_IMPLEMENTED_DEFAULT), CultureInfo.InvariantCulture);

            ShowNotReady = Convert.ToBoolean(driverProfile.GetValue(SensorName + OCSimulator.SHOW_NOT_READY_PROFILENAME, OCSimulator.SHOW_NOT_READY_DEFAULT), CultureInfo.InvariantCulture);

            NotReadyDelay = Convert.ToDouble(driverProfile.GetValue(SensorName + OCSimulator.NOT_READY_DELAY_PROFILENAME, OCSimulator.NOT_READY_DELAY_DEFAULT), CultureInfo.InvariantCulture);

            ValueCycleTime = Convert.ToDouble(driverProfile.GetValue(SensorName + OCSimulator.VALUE_CYCLE_TIME_PROFILE_NAME, OCSimulator.VALUE_CYCLE_TIME_DEFAULT), CultureInfo.InvariantCulture);

            Override = Convert.ToBoolean(driverProfile.GetValue(SensorName + OCSimulator.OVERRIDE_PROFILENAME, OCSimulator.OVERRIDE_DEFAULT), CultureInfo.InvariantCulture);

            OverrideValue = Convert.ToDouble(driverProfile.GetValue(SensorName + OCSimulator.OVERRIDE_VALUE_PROFILENAME, OCSimulator.SimulatorDefaultFromValues[SensorName].ToString(CultureInfo.InvariantCulture)), CultureInfo.InvariantCulture);
        }

        public void WriteProfile(IProfile driverProfile)
        {
            driverProfile.WriteValue(SensorName + OCSimulator.SIMFROMVALUE_PROFILENAME, SimFromValue.ToString(CultureInfo.InvariantCulture));

            driverProfile.WriteValue(SensorName + OCSimulator.SIMTOVALUE_PROFILENAME, SimToValue.ToString(CultureInfo.InvariantCulture));

            driverProfile.WriteValue(SensorName + OCSimulator.IS_IMPLEMENTED_PROFILENAME, IsImplemented.ToString(CultureInfo.InvariantCulture));

            driverProfile.WriteValue(SensorName + OCSimulator.SHOW_NOT_READY_PROFILENAME, ShowNotReady.ToString(CultureInfo.InvariantCulture));

            driverProfile.WriteValue(SensorName + OCSimulator.NOT_READY_DELAY_PROFILENAME, NotReadyDelay.ToString(CultureInfo.InvariantCulture));

            driverProfile.WriteValue(SensorName + OCSimulator.VALUE_CYCLE_TIME_PROFILE_NAME, ValueCycleTime.ToString(CultureInfo.InvariantCulture));

            driverProfile.WriteValue(SensorName + OCSimulator.OVERRIDE_PROFILENAME, Override.ToString(CultureInfo.InvariantCulture));

            driverProfile.WriteValue(SensorName + OCSimulator.OVERRIDE_VALUE_PROFILENAME, OverrideValue.ToString(CultureInfo.InvariantCulture));
        }
    }
}