//
// ================
// Shared Resources
// ================
//
// This class is a container for all shared resources that may be needed
// by the drivers served by the Local Server.
//
// NOTES:
//
//	* ALL DECLARATIONS MUST BE STATIC HERE!! INSTANCES OF THIS CLASS MUST NEVER BE CREATED!
//
// Written by:	Bob Denny	29-May-2007
// Modified by Chris Rowland and Peter Simpson to handle multiple hardware devices March 2011
//
using ASCOM.Standard.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace ASCOM.Simulators
{
    /// <summary>
    /// The resources shared by all drivers and devices, in this example it's a serial port with a shared SendMessage method
    /// an idea for locking the message and handling connecting is given.
    /// In reality extensive changes will probably be needed.
    /// Multiple drivers means that several applications connect to the same hardware device, aka a hub.
    /// Multiple devices means that there are more than one instance of the hardware, such as two focusers.
    /// In this case there needs to be multiple instances of the hardware connector, each with it's own connection count.
    /// </summary>
    public class OCSimulator
    {
        #region Public variables and constants

        // Valid properties constants
        public const string PROPERTY_CLOUDCOVER = "CloudCover";

        public const string PROPERTY_DEWPOINT = "DewPoint";
        public const string PROPERTY_HUMIDITY = "Humidity";
        public const string PROPERTY_PRESSURE = "Pressure";
        public const string PROPERTY_RAINRATE = "RainRate";
        public const string PROPERTY_SKYBRIGHTNESS = "SkyBrightness";
        public const string PROPERTY_SKYQUALITY = "SkyQuality";
        public const string PROPERTY_STARFWHM = "StarFWHM";
        public const string PROPERTY_SKYTEMPERATURE = "SkyTemperature";
        public const string PROPERTY_TEMPERATURE = "Temperature";
        public const string PROPERTY_WINDDIRECTION = "WindDirection";
        public const string PROPERTY_WINDGUST = "WindGust";
        public const string PROPERTY_WINDSPEED = "WindSpeed";

        public const string DRIVER_PROGID = "ASCOM.Simulator.ObservingConditions";
        public const string DRIVER_DISPLAY_NAME = "ASCOM Observing Conditions Simulator"; // Driver description that displays in the ASCOM Chooser.
        public const string DEVICE_TYPE = "ObservingConditions";
        public const string OBSERVINGCONDITIONS_DEVICE_NAME = "ObservingConditions";
        public const string SIMULATOR_PROGID = "Simulator";
        public const string SENSORVIEW_CONTROL_PREFIX = "sensorView";
        public const string NOT_CONNECTED_MESSAGE = DRIVER_DISPLAY_NAME + " is not connected.";

        // Profile persistence constants
        public const string SIMFROMVALUE_PROFILENAME = "Simulated From Value"; // Default values are held in Dictionary SimulatorDefaultFromValues

        public const string SIMTOVALUE_PROFILENAME = "Simulated To Value"; // Default values are held in Dictionary SimulatorDefaultToValues
        public const string IS_IMPLEMENTED_PROFILENAME = "Is Implemented"; public const string IS_IMPLEMENTED_DEFAULT = "True";
        public const string SHOW_NOT_READY_PROFILENAME = "Show NotReady"; public const string SHOW_NOT_READY_DEFAULT = "False";
        public const string NOT_READY_DELAY_PROFILENAME = "Not Ready Delay"; public const string NOT_READY_DELAY_DEFAULT = "0.0";
        public const string VALUE_CYCLE_TIME_PROFILE_NAME = "Value Cycle Time"; public const string VALUE_CYCLE_TIME_DEFAULT = "60.0";
        public const string TRACE_LEVEL_PROFILENAME = "Trace Level"; public const string TRACE_LEVEL_DEFAULT = "False";
        public const string DEBUG_TRACE_PROFILENAME = "Include Debug Trace"; public const string DEBUG_TRACE_DEFAULT = "False";
        public const string CONNECT_TO_DRIVERS_PROFILENAME = "Connect To Drivers"; public const string CONNECT_TO_DRIVERS_DEFAULT = "False";
        public const string SENSOR_READ_PERIOD_PROFILENAME = "Sensor Read Period"; public const string SENSOR_READ_PERIOD_DEFAULT = "1.0";
        public const string AVERAGE_PERIOD_PROFILENAME = "Average Period"; public const string AVERAGE_PERIOD_DEFAULT = "0.0";
        public const string NUMBER_OF_READINGS_PROFILENAME = "Number Of Readings"; public const string NUMBER_OF_READINGS_DEFAULT = "10";
        public const string OVERRIDE_PROFILENAME = "Override"; public const string OVERRIDE_DEFAULT = "false";
        public const string OVERRIDE_VALUE_PROFILENAME = "Override Value"; // No default value, these are picked from the simulator "from" values
        public const string MINIMISE_ON_START_PROFILENAME = "Minimise On Start"; public const string MINIMISE_ON_START_DEFAULT = "True";

        public static ILogger TL;

        public static bool DebugTraceState;

        // Setup dialogue configuration variables
        public static bool TraceState;

        public static double SensorQueryInterval;
        public static double AveragePeriod;
        public static int NumberOfReadingsToAverage;

        // Main dialogue configuration variables
        public static bool MinimiseOnStart;

        // List of ObservingConditions properties that are dynamically simulated
        public static List<string> SimulatedProperties = new List<string> { // Array containing a list of properties that are calculated but excluding dew point, which is derived
            PROPERTY_CLOUDCOVER,
            PROPERTY_HUMIDITY,
            PROPERTY_PRESSURE,
            PROPERTY_RAINRATE,
            PROPERTY_SKYBRIGHTNESS,
            PROPERTY_SKYQUALITY,
            PROPERTY_STARFWHM,
            PROPERTY_SKYTEMPERATURE,
            PROPERTY_TEMPERATURE,
            PROPERTY_WINDDIRECTION,
            PROPERTY_WINDGUST,
            PROPERTY_WINDSPEED };

        // List of valid ObservingConditions properties
        public static List<string> DriverProperties = new List<string> { // Array containing a list of all valid properties
            PROPERTY_CLOUDCOVER,
            PROPERTY_DEWPOINT,
            PROPERTY_HUMIDITY,
            PROPERTY_PRESSURE,
            PROPERTY_RAINRATE,
            PROPERTY_SKYBRIGHTNESS,
            PROPERTY_SKYQUALITY,
            PROPERTY_STARFWHM,
            PROPERTY_SKYTEMPERATURE,
            PROPERTY_TEMPERATURE,
            PROPERTY_WINDDIRECTION,
            PROPERTY_WINDGUST,
            PROPERTY_WINDSPEED };

        // Hub simulator information
        public static Dictionary<string, double> SimulatorDefaultFromValues = new Dictionary<string, double>()
        {
            {PROPERTY_CLOUDCOVER, 0.0},
            {PROPERTY_HUMIDITY, 50.0},
            {PROPERTY_PRESSURE, 1020.5},
            {PROPERTY_RAINRATE, 0.0},
            {PROPERTY_SKYBRIGHTNESS, 85.0},
            {PROPERTY_SKYQUALITY, 18.0},
            {PROPERTY_STARFWHM, 0.85},
            {PROPERTY_SKYTEMPERATURE, -28.0},
            {PROPERTY_TEMPERATURE, 5.4},
            {PROPERTY_WINDDIRECTION, 174.4},
            {PROPERTY_WINDGUST, 2.34},
            {PROPERTY_WINDSPEED, 0.29}
        };

        public static Dictionary<string, double> SimulatorDefaultToValues = new Dictionary<string, double>()
        {
            {PROPERTY_CLOUDCOVER, 5.0},
            {PROPERTY_HUMIDITY, 55.0},
            {PROPERTY_PRESSURE, 1032.5},
            {PROPERTY_RAINRATE, 0.0},
            {PROPERTY_SKYBRIGHTNESS, 95.0},
            {PROPERTY_SKYQUALITY, 20.0},
            {PROPERTY_STARFWHM, 1.35},
            {PROPERTY_SKYTEMPERATURE, -25.0},
            {PROPERTY_TEMPERATURE, 8.8},
            {PROPERTY_WINDDIRECTION, 253.8},
            {PROPERTY_WINDGUST, 5.45},
            {PROPERTY_WINDSPEED, 2.38}
        };

        public static Dictionary<string, double> OverrideFromValues = new Dictionary<string, double>()
        {
            {PROPERTY_CLOUDCOVER, 0.0},
            {PROPERTY_HUMIDITY, 0.0},
            {PROPERTY_PRESSURE, 950.0},
            {PROPERTY_RAINRATE, 0.0},
            {PROPERTY_SKYBRIGHTNESS, 0.0},
            {PROPERTY_SKYQUALITY, 2.0},
            {PROPERTY_STARFWHM, 0.1},
            {PROPERTY_SKYTEMPERATURE, -75.0},
            {PROPERTY_TEMPERATURE, -50.0},
            {PROPERTY_WINDDIRECTION, 0.0},
            {PROPERTY_WINDGUST, 0},
            {PROPERTY_WINDSPEED, 0}
        };

        public static Dictionary<string, double> OverrideToValues = new Dictionary<string, double>()
        {
            {PROPERTY_CLOUDCOVER, 100.0},
            {PROPERTY_HUMIDITY, 100.0},
            {PROPERTY_PRESSURE, 1075.0},
            {PROPERTY_RAINRATE, 100.0},
            {PROPERTY_SKYBRIGHTNESS, 10000.0},
            {PROPERTY_SKYQUALITY, 22.0},
            {PROPERTY_STARFWHM, 10.0},
            {PROPERTY_SKYTEMPERATURE, 50.0},
            {PROPERTY_TEMPERATURE, 50.0},
            {PROPERTY_WINDDIRECTION, 360.0},
            {PROPERTY_WINDGUST, 100.0},
            {PROPERTY_WINDSPEED, 100.0}
        };

        #endregion Public variables and constants

        #region Public Enums and Structs

        public enum ValueCycleDirections
        {
            FromTowardsTo,
            ToTowardsFrom
        }

        public struct TimeValue
        {
            public TimeValue(DateTime obsTime, double sensValue)
            {
                ObservationTime = obsTime;
                SensorValue = sensValue;
            }

            public DateTime ObservationTime;
            public double SensorValue;
        }

        #endregion Public Enums and Structs

        #region Private variables and constants

        // Supported actions
        private const string OCH_TAG = "OCHTag"; private const string OCH_TAG_UPPER_CASE = "OCHTAG";

        private const string OCH_TEST_WEATHER_REPORT = "OCHTestWeatherReport"; private const string OCH_TEST_WEATHER_REPORT_UPPER_CASE = "OCHTESTWEATHERREPORT";

        // Profile persistence constants and variable to control whether OCHTag is supported by the driver - no UI is provided for changing this value, it must be changed by editing the Profile directly
        private const string EXPOSE_OCHTAG_NAME = "Expose OCH Tag";

        private const bool EXPOSE_OCHTAG_DEFAULT = true;
        private static bool exposeOCHState;

        // Miscellaneous variables
        private static int uniqueClientNumber = 0; // Unique number that increments on each call to UniqueClientNumber

        private readonly static object connectLockObject = new object();
        private static ConcurrentDictionary<long, bool> connectStates;
        private static DateTime initialConnectionTime;
        private static DateTime mostRecentUpdateTime;
        private static System.Timers.Timer sensorQueryTimer;
        private static System.Timers.Timer averagePeriodTimer;

        //Sensor information and devices
        public static Dictionary<string, Sensor> Sensors = new Dictionary<string, Sensor>();

        internal static IProfile driverProfile
        {
            get;
            set;
        }

        #endregion Private variables and constants

        #region Initialiser

        /// <summary>
        /// Static initialiser to set up the objects we need at run time
        /// </summary>
        static OCSimulator()
        {
            
        }

        internal static void Init()
        {
            try
            {
                // Create sensor objects ready to be populated from the Profile
                // This must be done before reading the Profile
                foreach (string Property in DriverProperties)
                {
                    Sensors.Add(Property, new Sensor(Property));
                }

                ReadProfile(); // Read device configuration from the ASCOM Profile store

                LogMessage("OCSimulator", "Simulator initialising");
                sensorQueryTimer = new System.Timers.Timer();
                sensorQueryTimer.Elapsed += RefreshTimer_Elapsed;
                averagePeriodTimer = new System.Timers.Timer();
                averagePeriodTimer.Elapsed += AveragePeriodTimer_Elapsed;

                connectStates = new ConcurrentDictionary<long, bool>();
                mostRecentUpdateTime = DateTime.Now;

                LogMessage("OCSimulator", "Setting sensor initial values");
                foreach (string Property in SimulatedProperties)
                {
                    Sensors[Property].SimCurrentValue = Sensors[Property].SimFromValue;
                    Sensors[Property].ValueCycleDirection = ValueCycleDirections.FromTowardsTo;
                    Sensors[Property].TimeOfLastUpdate = DateTime.Now;
                }

                // Dew point is calculated from humidity so initialise that here
                Sensors[PROPERTY_DEWPOINT].IsImplemented = Sensors[PROPERTY_HUMIDITY].IsImplemented;
                Sensors[PROPERTY_DEWPOINT].SimCurrentValue = Humidity2DewPoint(Sensors[PROPERTY_HUMIDITY].SimCurrentValue, Sensors[PROPERTY_TEMPERATURE].SimCurrentValue);
                Sensors[PROPERTY_DEWPOINT].TimeOfLastUpdate = DateTime.Now;

                LogMessage("OCSimulator", "Simulator initialisation complete.");
            }
            catch (Exception ex)
            {
                TL.LogError(ex.Message);
                throw ex;
            }
        }

        #endregion Initialiser

        #region ASCOM Common Methods

        public static string Action(string actionName, string actionParameters)
        {
            switch (actionName.ToUpperInvariant())
            {
                case OCH_TAG_UPPER_CASE when exposeOCHState:
                    return "OCSimulator";

                case OCH_TEST_WEATHER_REPORT_UPPER_CASE:
                    return "The weather will be very nice today! Supplied parameters: " + actionParameters;

                default:
                    throw new ASCOM.ActionNotImplementedException("Action " + actionName + " is not implemented by this driver");
            }
        }

        public static void CommandBlind(string command)
        {
            throw new MethodNotImplementedException("CommandBlind");
        }

        public static void CommandBlind(string command, bool raw)
        {
            throw new MethodNotImplementedException("CommandBlind");
        }

        public static bool CommandBool(string command)
        {
            throw new MethodNotImplementedException("CommandBool");
        }

        public static bool CommandBool(string command, bool raw)
        {
            throw new MethodNotImplementedException("CommandBool");
        }

        public static string CommandString(string command)
        {
            throw new MethodNotImplementedException("CommandString");
        }

        public static string CommandString(string command, bool raw)
        {
            throw new MethodNotImplementedException("CommandString");
        }

        public static void Connect()
        {
            if (DebugTraceState) LogMessage("Connect", "Acquiring connection lock");
            lock (connectLockObject) // Ensure that only one connection attempt can happen at a time
            {
                LogMessage("Connect", "Has connection lock");

                try
                {
                    LogMessage("Connect", "Attempting to connect to devices");
                    bool notAlreadyPresent = connectStates.TryAdd(1, true);
                    LogMessage("Connect", "Successfully connected, AlreadyConnected: " + (!notAlreadyPresent).ToString() + ", number of connections: " + connectStates.Count);
                    if (ConnectionCount == 1) // This is the first successful connection
                    {
                        LogMessage("Connect", "This is the first connection so set the connection start time");
                        initialConnectionTime = DateTime.Now;
                        foreach (string property in SimulatedProperties)
                        {
                            Sensors[property].ValueCycleDirection = ValueCycleDirections.FromTowardsTo;
                            Sensors[property].SimCurrentValue = Sensors[property].SimFromValue;
                            Sensors[property].TimeOfLastUpdate = initialConnectionTime;
                        }

                        // Dew point is calculated from humidity so initialise that here
                        Sensors[PROPERTY_DEWPOINT].SimCurrentValue = Humidity2DewPoint(Sensors[PROPERTY_HUMIDITY].SimCurrentValue, Sensors[PROPERTY_TEMPERATURE].SimCurrentValue);
                        Sensors[PROPERTY_DEWPOINT].TimeOfLastUpdate = initialConnectionTime;
                        mostRecentUpdateTime = initialConnectionTime;

                        sensorQueryTimer.Interval = SensorQueryInterval * 1000.0;
                        sensorQueryTimer.Enabled = true;
                        ConfigureAveragePeriodTimer();
                    }
                }
                catch (Exception ex)
                {
                    LogMessage("Connect", "Exception: " + ex.ToString());
                    throw;
                }
            }
        }

        public static void Disconnect()
        {
            bool lastValue;
            bool successfullyRemoved = connectStates.TryRemove(1, out lastValue);
            LogMessage("Hardware.Connected Set", "Set Connected to: False, Successfully removed: " + successfullyRemoved.ToString());

            if (ConnectionCount == 0) // The last connection has dropped so stop the timer
            {
                sensorQueryTimer.Enabled = false;
            }
        }

        public static string Description()
        {
            CheckConnected("Description");

            LogMessage("Description", DRIVER_DISPLAY_NAME);
            return DRIVER_DISPLAY_NAME;
        }

        public static string DriverInfo()
        {
            CheckConnected("DriverInfo");

            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            string driverInfo = DRIVER_DISPLAY_NAME + ". Version: " + version.ToString();
            LogMessage("DriverInfo", driverInfo);
            return driverInfo;
        }

        public static string DriverVersion()
        {
            CheckConnected("DriverVersion");

            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            string driverVersion = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
            LogMessage("DriverVersion", driverVersion);
            return driverVersion;
        }

        public static short InterfaceVersion()
        {
            short interfaceVersion = 1;
            LogMessage("InterfaceVersion", interfaceVersion.ToString());
            return interfaceVersion;
        }

        public static string Name()
        {
            string name = DRIVER_DISPLAY_NAME;
            LogMessage("Name", name);
            return name;
        }

        public static IList<string> SupportedActions()
        {
            CheckConnected("SupportedActions");

            if (exposeOCHState)
            {
                LogMessage("SupportedActions", string.Format("Returning {0} and {1} in the arraylist", OCH_TAG, OCH_TEST_WEATHER_REPORT));
                return new List<string>() { OCH_TAG, OCH_TEST_WEATHER_REPORT };
            }
            else
            {
                LogMessage("SupportedActions", string.Format("Returning {0} in the arraylist, not returning {1} because exposeOCHState is false", OCH_TEST_WEATHER_REPORT, OCH_TAG));
                return new List<string> { OCH_TEST_WEATHER_REPORT };
            }
        }

        #endregion ASCOM Common Methods

        #region ASCOM ObservingConditions Methods

        public static double AveragePeriodGet()
        {
            CheckConnected("AveragePeriodGet");

            LogMessage("AveragePeriodGet", AveragePeriod.ToString());
            return AveragePeriod;
        }

        public static void AveragePeriodSet(double value)
        {
            CheckConnected("AveragePeriodSet");

            if (value >= 0.0)
            {
                AveragePeriod = value;

                ConfigureAveragePeriodTimer();
                LogMessage("AveragePeriodSet", value.ToString());
            }
            else
            {
                LogMessage("AveragePeriodSet", "Bad value: " + value.ToString() + ", throwing InvalidValueException");
                throw new InvalidValueException("AveragePeriod Set", value.ToString(), "0.0 upwards");
            }
        }

        public static double CloudCover()
        {
            double cloudCover = GetSensorValue(PROPERTY_CLOUDCOVER);
            LogMessage("CloudCover", cloudCover.ToString());
            return cloudCover;
        }

        public static double DewPoint()
        {
            double dewPoint = GetSensorValue(PROPERTY_DEWPOINT); ;
            LogMessage("DewPoint", dewPoint.ToString());
            return dewPoint;
        }

        public static double Humidity()
        {
            double humidity = GetSensorValue(PROPERTY_HUMIDITY); ;
            LogMessage("Humidity", humidity.ToString());
            return humidity;
        }

        public static double Pressure()
        {
            double pressure = GetSensorValue(PROPERTY_PRESSURE); ;
            LogMessage("Pressure", pressure.ToString());
            return pressure;
        }

        public static double RainRate()
        {
            double rainRate = GetSensorValue(PROPERTY_RAINRATE); ;
            LogMessage("RainRate", rainRate.ToString());
            return rainRate;
        }

        public static string SensorDescription(string PropertyName)
        {
            CheckConnected("SensorDescription");

            if (IsValidProperty(PropertyName))
            {
                if (Sensors[PropertyName].IsImplemented)
                    return "ObservingConditions Simulated " + PropertyName + " sensor";
                else throw new MethodNotImplementedException("SensorDescription(\"" + PropertyName + "\")");
            }
            else
            {
                LogMessage("SensorDescription", PropertyName + " is invalid, throwing InvalidValueException");
                throw new InvalidValueException("SensorDescription: \"" + PropertyName + "\" is not a valid ObservingConditions property");
            }
        }

        public static void Refresh()
        {
            // No action required for devices that are simulated
        }

        public static double SkyBrightness()
        {
            double skyBrightness = GetSensorValue(PROPERTY_SKYBRIGHTNESS); ;
            LogMessage("SkyBrightness", skyBrightness.ToString());
            return skyBrightness;
        }

        public static double SkyQuality()
        {
            double skyQuality = GetSensorValue(PROPERTY_SKYQUALITY); ;
            LogMessage("SkyQuality", skyQuality.ToString());
            return skyQuality;
        }

        public static double StarFWHM()
        {
            double starFWHM = GetSensorValue(PROPERTY_STARFWHM); ;
            LogMessage("StarFWHM", starFWHM.ToString());
            return starFWHM;
        }

        public static double SkyTemperature()
        {
            double skyTemperature = GetSensorValue(PROPERTY_SKYTEMPERATURE); ;
            LogMessage("SkyTemperature", skyTemperature.ToString());
            return skyTemperature;
        }

        public static double Temperature()
        {
            double temperature = GetSensorValue(PROPERTY_TEMPERATURE); ;
            LogMessage("Temperature", temperature.ToString());
            return temperature;
        }

        public static double TimeSinceLastUpdate(string PropertyName)
        {
            CheckConnected("TimeSinceLastUpdate");

            if (PropertyName  == null || PropertyName == "") // Return the most recent update time of any sensor
            {
                double timeSinceLastUpdate = DateTime.Now.Subtract(mostRecentUpdateTime).TotalSeconds;
                LogMessage("TimeSinceLastUpdate", "Most recent sensor update time: " + timeSinceLastUpdate.ToString());
                return timeSinceLastUpdate;
            }
            else
            {
                if (IsValidProperty(PropertyName))
                {
                    if (Sensors[PropertyName].IsImplemented) // Sensor is implemented
                    {
                        if (DateTime.Now.Subtract(initialConnectionTime).TotalSeconds < Sensors[PropertyName].NotReadyDelay) // Check whether the device is still initialising
                        {
                            return -1.0; // Still initialising so return a negative value
                        }
                        else //
                        {
                            double timeSinceLastUpdate = DateTime.Now.Subtract(Sensors[PropertyName].TimeOfLastUpdate).TotalSeconds;
                            LogMessage("TimeSinceLastUpdate", PropertyName + ": " + timeSinceLastUpdate.ToString());
                            return timeSinceLastUpdate; // Operating normally so return current value
                        }
                    }
                    else throw new MethodNotImplementedException(PropertyName); // This sensor is not implemented
                }
                else
                {
                    LogMessage("TimeSinceLastUpdate", "\"" + PropertyName + "\" is invalid, throwing InvalidValueException");
                    throw new InvalidValueException("TimeSinceLastUpdate: \"" + PropertyName + "\" is not a valid ObservingConditions property");
                }
            }
        }

        public static double WindDirection()
        {
            double windDirection = GetSensorValue(PROPERTY_WINDDIRECTION); ;
            LogMessage("WindDirection", windDirection.ToString());
            return windDirection;
        }

        public static double WindGust()
        {
            double windGust = GetSensorValue(PROPERTY_WINDGUST); ;
            LogMessage("WindGust", windGust.ToString());
            return windGust;
        }

        public static double WindSpeed()
        {
            double windSpeed = GetSensorValue(PROPERTY_WINDSPEED); ;
            LogMessage("WindSpeed", windSpeed.ToString());
            return windSpeed;
        }

        #endregion ASCOM ObservingConditions Methods

        #region Timer event handlers

        private static void AveragePeriodTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            DateTime now = DateTime.Now;
            foreach (string Property in DriverProperties) //  SimulatedProperties)
            {
                double sensorValue = Sensors[Property].SimCurrentValue;
                Sensors[Property].Readings.Add(new TimeValue(now, sensorValue));
                int beforeTrim = Sensors[Property].Readings.Count;
                Sensors[Property].Readings.RemoveAll(TimeRemovePredicate);
                int afterTrim = Sensors[Property].Readings.Count;
                if (DebugTraceState) // List the sensor readings that will be averaged
                {
                    LogMessage("AveragePeriodTimer", $"{Property} reading {sensorValue} added to reading collection. Before trim count: {beforeTrim}, After trim count: {afterTrim}");
                    foreach (TimeValue tv in Sensors[Property].Readings)
                    {
                        LogMessage("AveragePeriodTimer", $"  {Property} value at {tv.ObservationTime.ToString("hh:mm:ss.fff")} was {tv.SensorValue}");
                    }
                }
            }
        }

        private static void RefreshTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            foreach (string Property in SimulatedProperties)
            {
                double fromValue = Sensors[Property].SimFromValue;
                double toValue = Sensors[Property].SimToValue;
                double currentValue = Sensors[Property].SimCurrentValue;
                double sensorValueCycleTime = Sensors[Property].ValueCycleTime;
                double newValue = 0.0;

                switch (Sensors[Property].ValueCycleDirection)
                {
                    case ValueCycleDirections.FromTowardsTo: // Going from the From value towards the To Value
                        newValue = currentValue + (toValue - fromValue) * (SensorQueryInterval / sensorValueCycleTime);
                        if (toValue >= fromValue) // We are increasing in value going from From towards To
                        {
                            if (newValue > toValue) // We are out of bounds so reverse direction and reset to To value
                            {
                                newValue = toValue;
                                Sensors[Property].ValueCycleDirection = ValueCycleDirections.ToTowardsFrom;
                            }
                            else // We are within in bounds so no further action required
                            { }
                        }
                        else // We are decreasing in value as we go from From towards To
                        {
                            if (newValue < toValue) // We are out of bounds so reverse direction and reset to To value
                            {
                                newValue = toValue;
                                Sensors[Property].ValueCycleDirection = ValueCycleDirections.ToTowardsFrom;
                            }
                            else // We are within in bounds so no further action required
                            { }
                        }
                        break;

                    case ValueCycleDirections.ToTowardsFrom: // Going from the To value towards the From value
                        newValue = currentValue - (toValue - fromValue) * (SensorQueryInterval / sensorValueCycleTime);
                        if (toValue >= fromValue) // We are increasing in value going from From towards To
                        {
                            if (newValue < fromValue) // We are out of bounds so reverse direction and reset to To value
                            {
                                newValue = fromValue;
                                Sensors[Property].ValueCycleDirection = ValueCycleDirections.FromTowardsTo;
                            }
                            else // We are within in bounds so no further action required
                            { }
                        }
                        else // We are decreasing in value as we go from From towards To
                        {
                            if (newValue > fromValue) // We are out of bounds so reverse direction and reset to To value
                            {
                                newValue = fromValue;
                                Sensors[Property].ValueCycleDirection = ValueCycleDirections.FromTowardsTo;
                            }
                            else // We are within in bounds so no further action required
                            { }
                        }
                        break;

                    default:
                        break;
                }

                mostRecentUpdateTime = DateTime.Now; // Set the last updated times
                Sensors[Property].TimeOfLastUpdate = mostRecentUpdateTime;
                Sensors[Property].SimCurrentValue = newValue; // Set the new simulator value

                // If we are processing a humidity or temperature value, calculate the dew point because this is not "measured" but is derived directly from the current humidity and temperature
                if ((Property == PROPERTY_HUMIDITY) || (Property == PROPERTY_TEMPERATURE))
                {
                    Sensors[PROPERTY_DEWPOINT].TimeOfLastUpdate = mostRecentUpdateTime;
                    Sensors[PROPERTY_DEWPOINT].SimCurrentValue = Humidity2DewPoint(Sensors[PROPERTY_HUMIDITY].SimCurrentValue, Sensors[PROPERTY_TEMPERATURE].SimCurrentValue); // Set the new simulator dewpoint value
                }
            }
        }

        #endregion Timer event handlers

        #region Support code

        /// <summary>
        /// Returns a unique client number to the calling instance
        /// </summary>
        public static int GetUniqueClientNumber()
        {
            Interlocked.Increment(ref uniqueClientNumber);
            LogMessage("UniqueClientNumber", "Generated new ID: " + uniqueClientNumber.ToString());
            return uniqueClientNumber;
        }

        /// <summary>
        /// Determine whether a supplied property name is one of the valid property names
        /// </summary>
        /// <param name="PropertyName">Property name to test</param>
        /// <returns>Boolean true if the property name is valid</returns>
        internal static bool IsValidProperty(string PropertyName)
        {
            return DriverProperties.Contains(PropertyName.Trim(), StringComparer.OrdinalIgnoreCase); // Make the test case insensitive as well as leading and trailing space insensitive
        }

        /// <summary>
        /// Reads a sensor value
        /// </summary>
        /// <param name="PropertyName">Name of the property to read</param>
        /// <returns>Double value read from device</returns>
        private static double GetSensorValue(string PropertyName)
        {
            CheckConnected(PropertyName);
            if (Sensors[PropertyName].Override) // Override in effect so just return the specified value
            {
                LogMessage("GetDouble", PropertyName + " override value: " + Sensors[PropertyName].OverrideValue);
                return Sensors[PropertyName].OverrideValue;
            }
            else // No override so return the simulated value
            {
                if (Sensors[PropertyName].IsImplemented) // Sensor is implemented
                {
                    if (DateTime.Now.Subtract(initialConnectionTime).TotalSeconds >= Sensors[PropertyName].NotReadyDelay) // Check whether the device is still initialising
                    {
                        // Sensor is initialised so return its value
                        if (AveragePeriod == 0.0) // No averaging so just return current value
                        {
                            LogMessage("GetDouble", PropertyName + " current value: " + Sensors[PropertyName].SimCurrentValue);
                            return Sensors[PropertyName].SimCurrentValue;
                        }
                        else // Calculate the average and return this
                        {
                            double averageValue = 0.0;
                            int numberOfSensorReadings = Sensors[PropertyName].Readings.Count;

                            if (numberOfSensorReadings > 0) // There are one or more readings so calculate the average value
                            {
                                foreach (TimeValue tv in Sensors[PropertyName].Readings) // Add the sensor readings
                                {
                                    averageValue += tv.SensorValue;
                                }
                                averageValue /= numberOfSensorReadings; // Calculate the average sensor reading
                            }
                            else // There are no readings so just return the current value
                            {
                                averageValue = Sensors[PropertyName].SimCurrentValue;
                            }

                            LogMessage("GetDouble", PropertyName + " average value: " + averageValue + ", from " + numberOfSensorReadings + " readings, AveragePeriod: " + AveragePeriod + " hours.");
                            return averageValue; // Return the average sensor value
                        }
                    }
                    else
                    {
                        // Still initialising so throw InvalidOperationException
                        LogMessage("GetDouble", PropertyName + ": Sensor not ready");
                        throw new InvalidOperationException(PropertyName + " sensor is not ready");
                    }
                }
                else
                {
                    // Not implemented so throw a PropertyNotImplementedException
                    LogMessage("GetDouble", PropertyName + ": Sensor not implemented");
                    throw new PropertyNotImplementedException(PropertyName, false);
                }
            }
        }

        /// <summary>
        /// Remove stale values from the collection of PC-Mount time difference measurements
        /// </summary>
        /// <param name="timevalue">The time value to test</param>
        /// <returns>Boolean value indicating whether a particular time value should be removed from the collection</returns>
        /// <remarks>Originally this was treating AveragePeriod as being in minutes, it now treats it as hours per the specification.</remarks>
        private static bool TimeRemovePredicate(TimeValue timevalue)
        {
            // Next line revised to treat AveragePeriod as being in hours per the interface specification, previously it was treating it as being in minutes. Peter Simpson 29th May 2019
            return DateTime.Now.Subtract(timevalue.ObservationTime).TotalHours > AveragePeriod;
        }

        /// <summary>
        /// Configure and enabled the average period timer
        /// </summary>
        private static void ConfigureAveragePeriodTimer()
        {
            if (averagePeriodTimer.Enabled) averagePeriodTimer.Stop();
            if (AveragePeriod > 0.0)
            {
                // Next line revised to treat Averageperiod as being in hours per the interface specification, previously it was treating it as being in minutes. Peter Simpson 29th May 2019
                averagePeriodTimer.Interval = AveragePeriod * 3600000.0 / NumberOfReadingsToAverage; // AveragePeriod in hours, convert to milliseconds and divide by the number of readings required
                if (IsHardwareConnected()) averagePeriodTimer.Enabled = true;
            }
        }

        /// <summary>
        /// Test whether the we are connected, if not throw a NotConnectedException
        /// </summary>
        /// <param name="MethodName">Name of the calling method</param>
        private static void CheckConnected(string MethodName)
        {
            if (!IsHardwareConnected()) throw new NotConnectedException(MethodName + " - " + NOT_CONNECTED_MESSAGE);
        }

        /// <summary>
        /// Tests whether the hub is already connected
        /// </summary>
        /// <param name="clientNumber">Number of the client making the call</param>
        /// <returns>Boolean true if the hub is already connected</returns>
        public static bool IsHardwareConnected()
        {
            if (DebugTraceState) LogMessage("IsHardwareConnected", "Number of connected devices: " + connectStates.Count + ", Returning: " + (connectStates.Count > 0).ToString());
            return connectStates.Count > 0;
        }

        /// <summary>
        /// Test whether a particular client is already connected
        /// </summary>
        /// <param name="clientNumber">Number of the calling client</param>
        /// <returns></returns>
        public static bool IsClientConnected()
        {
            LogMessage("IsClientConnected", "Number of connected devices: " + connectStates.Count + ", Returning: " + connectStates.ContainsKey(1).ToString());

            return connectStates.ContainsKey(1);
        }

        /// <summary>
        /// Returns the number of connected clients
        /// </summary>
        public static int ConnectionCount
        {
            get
            {
                LogMessage("ConnectionCount", connectStates.Count.ToString());
                return connectStates.Count;
            }
        }

        #endregion Support code

        #region Profile management

        /// <summary>
        /// Read the device configuration from the ASCOM Profile store
        /// </summary>
        public static void ReadProfile()
        {
            // Initialise the logging trace state from the Profile
            TraceState = Convert.ToBoolean(driverProfile.GetValue(TRACE_LEVEL_PROFILENAME, TRACE_LEVEL_DEFAULT), CultureInfo.InvariantCulture);
            exposeOCHState = Convert.ToBoolean(driverProfile.GetValue(EXPOSE_OCHTAG_NAME, EXPOSE_OCHTAG_DEFAULT.ToString()));

            // Initialise other variables from the Profile
            DebugTraceState = Convert.ToBoolean(driverProfile.GetValue(DEBUG_TRACE_PROFILENAME, DEBUG_TRACE_DEFAULT), CultureInfo.InvariantCulture);
            SensorQueryInterval = Convert.ToDouble(driverProfile.GetValue(SENSOR_READ_PERIOD_PROFILENAME, SENSOR_READ_PERIOD_DEFAULT), CultureInfo.InvariantCulture);

            // Due to an oversight, the initial implementation of the simulator treated AveragePeriod as being in minutes rather than hours; this has been the case for a number of years.
            // *** For backward compatibility AveragePeriod will continue to be persisted in units of MINUTES. ***
            // Within the simulator, AveragePeriod is now treated as being in hours and consequently the persisted value in minutes must be converted to hours after retrieval.
            double averagePeriod = Convert.ToDouble(driverProfile.GetValue(AVERAGE_PERIOD_PROFILENAME, AVERAGE_PERIOD_DEFAULT), CultureInfo.InvariantCulture);
            AveragePeriod = averagePeriod / 60.0; // Convert the AveragePeriod retrieved value in minutes to hours

            NumberOfReadingsToAverage = Convert.ToInt32(driverProfile.GetValue(NUMBER_OF_READINGS_PROFILENAME, NUMBER_OF_READINGS_DEFAULT), CultureInfo.InvariantCulture);
            MinimiseOnStart = Convert.ToBoolean(driverProfile.GetValue(MINIMISE_ON_START_PROFILENAME, MINIMISE_ON_START_DEFAULT), CultureInfo.InvariantCulture);

            // Initialise the sensor collection from the Profile
            foreach (string Property in SimulatedProperties)
            {
                LogMessage("ReadProfile", "Reading profile for: " + Property);
                Sensors[Property].ReadProfile(driverProfile);
            }

            Sensors[PROPERTY_DEWPOINT].IsImplemented = Sensors[PROPERTY_HUMIDITY].IsImplemented; // Align dewpoint values with Humidity "master" values
            Sensors[PROPERTY_DEWPOINT].NotReadyDelay = Sensors[PROPERTY_HUMIDITY].NotReadyDelay;
        }

        /// <summary>
        /// Write the device configuration to the  ASCOM  Profile store
        /// </summary>
        public static void WriteProfile()
        {
            // Save the variable state to the Profile
            driverProfile.WriteValue(TRACE_LEVEL_PROFILENAME, TraceState.ToString(CultureInfo.InvariantCulture));
            driverProfile.WriteValue(DEBUG_TRACE_PROFILENAME, DebugTraceState.ToString(CultureInfo.InvariantCulture));
            driverProfile.WriteValue(SENSOR_READ_PERIOD_PROFILENAME, SensorQueryInterval.ToString(CultureInfo.InvariantCulture));

            // Due to an oversight, the initial implementation of the simulator treated AveragePeriod as being in minutes rather than hours; this has been the case for a number of years.
            // *** For backward compatibility AveragePeriod will continue to be persisted in units of MINUTES. ***
            // Within the simulator, AveragePeriod is now treated as being in hours and consequently the internal value in hours must be converted to minutes before being persisted.
            double averagePeriod = AveragePeriod * 60.0;
            driverProfile.WriteValue(AVERAGE_PERIOD_PROFILENAME, averagePeriod.ToString(CultureInfo.InvariantCulture));

            driverProfile.WriteValue(NUMBER_OF_READINGS_PROFILENAME, NumberOfReadingsToAverage.ToString(CultureInfo.InvariantCulture));
            driverProfile.WriteValue(MINIMISE_ON_START_PROFILENAME, MinimiseOnStart.ToString(CultureInfo.InvariantCulture));

            // Save the sensor collection to the Profile
            foreach (string Property in SimulatedProperties)
            {
                LogMessage("WriteProfile", "Writing profile for: " + Property);
                Sensors[Property].WriteProfile(driverProfile);
            }
        }

        #endregion Profile management

        public static void LogMessage(string message)
        {
            TL.LogVerbose(message);
        }

        public static void LogMessage(string message, string details)
        {
            TL.LogVerbose(message + " - " + details);
        }

        public static void LogMessage(string message, string details, object more)
        {
            TL.LogVerbose(message + " - " + details + " - " + more);
        }

        /// <summary>

        ///Calculate the dew point (°Celsius) given the ambient temperature (°Celsius) and relative humidity (%)

        ///</summary>

        ///<param name="RelativeHumidity">Relative humidity expressed in percent (0.0 .. 100.0)</param>

        ///<param name="AmbientTemperature">Ambient temperature (°Celsius)</param>

        ///<returns>Dew point (°Celsius)</returns>

        ///<exception cref="InvalidValueException">When relative humidity &lt; 0.0% or &gt; 100.0%></exception>

        ///<exception cref="InvalidValueException">When ambient temperature &lt; absolute zero or &gt; 100.0C></exception>

        /// <remarks>'Calculation uses Vaisala formula for water vapour saturation pressure and is accurate to 0.083% over -20C - +50°C

        ///<para>http://www.vaisala.com/Vaisala%20Documents/Application%20notes/Humidity_Conversion_Formulas_B210973EN-F.pdf </para>

        ///</remarks>
        public static double Humidity2DewPoint(double RelativeHumidity, double AmbientTemperature)
        {
            // Formulae taken from Vaisala: http://www.vaisala.com/Vaisala%20Documents/Application%20notes/Humidity_Conversion_Formulas_B210973EN-F.pdf
            double Pws, Pw, Td;

            double ABSOLUTE_ZERO_CELSIUS = -273.15;

            // Constants from Vaisala document
            const double A = 6.116441;
            const double m = 7.591386;
            const double Tn = 240.7263;

            // Validate input values
            if ((RelativeHumidity < 0.0) | (RelativeHumidity > 100.0))
                throw new InvalidValueException("Humidity2DewPoint - Relative humidity is < 0.0% or > 100.0%: " + RelativeHumidity.ToString());
            if ((AmbientTemperature < ABSOLUTE_ZERO_CELSIUS) | (AmbientTemperature > 100.0))
                throw new InvalidValueException("Humidity2DewPoint - Ambient temperature is < " + ABSOLUTE_ZERO_CELSIUS + "C or > 100.0C: " + AmbientTemperature.ToString());

            Pws = A * Math.Pow(10.0, m * AmbientTemperature / (AmbientTemperature + Tn)); // Calculate water vapor saturation pressure, Pws, from Vaisala formula (6) - In hPa
            Pw = Pws * RelativeHumidity / 100.0; // Calculate measured vapor pressure, Pw
            Td = Tn / ((m / Math.Log10(Pw / A)) - 1.0); // Finally, calculate dew-point in °C

            LogMessage("Humidity2DewPoint", "DewPoint: " + Td + ", Given Relative Humidity: " + RelativeHumidity + ", Given Ambient temperaure: " + AmbientTemperature + ", Pws: " + Pws + ", Pw: " + Pw);

            return Td;
        }
    }
}