using ASCOM.Standard.Interfaces;
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
            OCSimulator.LogMessage("Sensor.ReadProfile", "Starting to read profile values");

            SimFromValue = Convert.ToDouble(driverProfile.GetValue(SensorName + OCSimulator.SIMFROMVALUE_PROFILENAME, OCSimulator.SimulatorDefaultFromValues[SensorName].ToString(CultureInfo.InvariantCulture)), CultureInfo.InvariantCulture);
            OCSimulator.LogMessage("Sensor.ReadProfile" + " SimFromValue: " + SimFromValue);

            SimToValue = Convert.ToDouble(driverProfile.GetValue(SensorName + OCSimulator.SIMTOVALUE_PROFILENAME, OCSimulator.SimulatorDefaultToValues[SensorName].ToString(CultureInfo.InvariantCulture)), CultureInfo.InvariantCulture);
            OCSimulator.LogMessage("Sensor.ReadProfile" + " SimToValue: " + SimToValue);

            IsImplemented = Convert.ToBoolean(driverProfile.GetValue(SensorName + OCSimulator.IS_IMPLEMENTED_PROFILENAME, OCSimulator.IS_IMPLEMENTED_DEFAULT), CultureInfo.InvariantCulture);
            OCSimulator.LogMessage("Sensor.ReadProfile" + " Is Implemented: " + IsImplemented.ToString());

            ShowNotReady = Convert.ToBoolean(driverProfile.GetValue(SensorName + OCSimulator.SHOW_NOT_READY_PROFILENAME, OCSimulator.SHOW_NOT_READY_DEFAULT), CultureInfo.InvariantCulture);
            OCSimulator.LogMessage("Sensor.ReadProfile" + " ShowNotReady: " + ShowNotReady.ToString());

            NotReadyDelay = Convert.ToDouble(driverProfile.GetValue(SensorName + OCSimulator.NOT_READY_DELAY_PROFILENAME, OCSimulator.NOT_READY_DELAY_DEFAULT), CultureInfo.InvariantCulture);
            OCSimulator.LogMessage("Sensor.ReadProfile" + " NotReadyDelay: " + NotReadyDelay.ToString());

            ValueCycleTime = Convert.ToDouble(driverProfile.GetValue(SensorName + OCSimulator.VALUE_CYCLE_TIME_PROFILE_NAME, OCSimulator.VALUE_CYCLE_TIME_DEFAULT), CultureInfo.InvariantCulture);
            OCSimulator.LogMessage("Sensor.ReadProfile" + " Value CycleTime: " + ValueCycleTime.ToString());

            Override = Convert.ToBoolean(driverProfile.GetValue(SensorName + OCSimulator.OVERRIDE_PROFILENAME, OCSimulator.OVERRIDE_DEFAULT), CultureInfo.InvariantCulture);
            OCSimulator.LogMessage("Sensor.ReadProfile" + " Override: " + Override.ToString());

            OverrideValue = Convert.ToDouble(driverProfile.GetValue(SensorName + OCSimulator.OVERRIDE_VALUE_PROFILENAME, OCSimulator.SimulatorDefaultFromValues[SensorName].ToString(CultureInfo.InvariantCulture)), CultureInfo.InvariantCulture);
            OCSimulator.LogMessage("Sensor.ReadProfile" + " OverrideValue: " + OverrideValue.ToString());

            OCSimulator.LogMessage("Sensor.ReadProfile", "Completed reading profile values");
        }

        public void WriteProfile(IProfile driverProfile)
        {
            OCSimulator.LogMessage("Sensor.WriteProfile", "Starting to write profile values");

            OCSimulator.LogMessage("Sensor.WriteProfile" + " SimFromValue: " + SimFromValue);
            driverProfile.WriteValue(SensorName + OCSimulator.SIMFROMVALUE_PROFILENAME, SimFromValue.ToString(CultureInfo.InvariantCulture));

            OCSimulator.LogMessage("Sensor.WriteProfile" + " SimToValue: " + SimToValue);
            driverProfile.WriteValue(SensorName + OCSimulator.SIMTOVALUE_PROFILENAME, SimToValue.ToString(CultureInfo.InvariantCulture));

            OCSimulator.LogMessage("Sensor.WriteProfile" + " Is Implemented: " + IsImplemented);
            driverProfile.WriteValue(SensorName + OCSimulator.IS_IMPLEMENTED_PROFILENAME, IsImplemented.ToString(CultureInfo.InvariantCulture));

            OCSimulator.LogMessage("Sensor.WriteProfile" + " ShowNotReady: " + ShowNotReady);
            driverProfile.WriteValue(SensorName + OCSimulator.SHOW_NOT_READY_PROFILENAME, ShowNotReady.ToString(CultureInfo.InvariantCulture));

            OCSimulator.LogMessage("Sensor.WriteProfile" + " NotReadyDelay: " + NotReadyDelay);
            driverProfile.WriteValue(SensorName + OCSimulator.NOT_READY_DELAY_PROFILENAME, NotReadyDelay.ToString(CultureInfo.InvariantCulture));

            OCSimulator.LogMessage("Sensor.WriteProfile" + " Value Cycle Time: " + ValueCycleTime);
            driverProfile.WriteValue(SensorName + OCSimulator.VALUE_CYCLE_TIME_PROFILE_NAME, ValueCycleTime.ToString(CultureInfo.InvariantCulture));

            OCSimulator.LogMessage("Sensor.WriteProfile" + " Override: " + Override);
            driverProfile.WriteValue(SensorName + OCSimulator.OVERRIDE_PROFILENAME, Override.ToString(CultureInfo.InvariantCulture));

            OCSimulator.LogMessage("Sensor.WriteProfile" + " OverrideValue: " + OverrideValue);
            driverProfile.WriteValue(SensorName + OCSimulator.OVERRIDE_VALUE_PROFILENAME, OverrideValue.ToString(CultureInfo.InvariantCulture));

            OCSimulator.LogMessage("Sensor.WriteProfile" + " Completed writing profile values");
        }
    }
}
