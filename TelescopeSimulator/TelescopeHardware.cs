//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Simulated Telescope Hardware
//
// Description:	This implements a simulated Telescope Hardware
//
// Implements:	ASCOM Telescope interface version: 2.0
// Author:		(rbt) Robert Turner <robert@robertturnerastro.com>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 07-JUL-2009	rbt	1.0.0	Initial edit, from ASCOM Telescope Driver template
// 18-SEP-2102  Rick Burke  Improved support for simulating pulse guiding
// May/June 2014 cdr 6.1    Change the telescope hardware to use an axis based method
// --------------------------------------------------------------------------------
//

using ASCOM.Common;
using ASCOM.Common.DeviceInterfaces;
using ASCOM.Common.Interfaces;
using ASCOM.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;

[assembly: InternalsVisibleTo("ASCOM.Alpaca.Simulators")]

namespace ASCOM.Simulators
{
    public static class TelescopeHardware
    {
        #region How the simulator works

        // The telescope is implemented using two axes that represent the primary and secondary telescope axes.
        // The role of the axes varies depending on the mount type.
        // The primary axis is the azimuth axis for an AltAz mount and the hour angle axis for polar mounts.
        // The secondary axis is the altitude axis for AltAz mounts and the declination axis for polar mounts.

        // All motion is done and all positions are set by manipulating the axis angle values.

        // Vectors are used for pairs of angles that represent the various positions and rates
        // Vector.X is the primary axis = Axis angle, Hour angle, Right Ascension or Azimuth and Vector.Y is the secondary axis = Axis angle, Declination or Altitude.

        // Ra and hour angle vectors are in hours
        // Mount position, Declination, azimuth and altitude vectors are in degrees.

        // NORTHERN HEMISPHERE AXIS CONVENTIONS
        //
        // ALT/AZ ALIGNMENT - Slews always use the Normal (pierEast) pointing state even when the mount has been manually set to the TTP pointing state
        //   Primary axis - 0 degrees = North with value increasing clockwise giving 90 degrees = East, 180 degrees = South and 270 = West.
        //   Secondary axis, 0 degrees at the horizon increasing to 90 degrees at the zenith

        // POLAR ALIGNMENT - Slews always use the Normal (pierEast) pointing state even when the mount has been manually set to the TTP pointing state
        //   Primary axis - 0 degrees = hour angle 0, -180 degrees = Hour angle -12 and +180 degrees = hour angle +12
        //   Secondary axis, 0 degrees at declination 0, +90 at the northern equatorial pole. Negative values at negative declinations towards the south.

        // GERMAN POLAR ALIGNMENT - Normal pointing state (pierEast) i.e. when observing hour angles 0 to +12.
        //   Primary axis - 0 degrees = hour angle 0, +180 degrees = hour angle +12
        //   Secondary axis - 0 degrees at declination 0, +90 at the northern equatorial pole. Negative values at negative declinations towards the south.

        // GERMAN POLAR ALIGNMENT - Through The Pole pointing state (pierWest) i.e. When observing hour angles -12 to 0.
        //   Primary axis - 0 degrees = hour angle -12, +180 degrees = hour angle 0
        //   Secondary axis - +90 at the equatorial pole. 180-declination at smaller declinations. Note that at negative declinations the axis angle will be >180

        // SOUTHERN HEMISPHERE AXIS CONVENTIONS
        //
        // ALT/AZ ALIGNMENT - Slews always use the Normal (pierEast) pointing state even when the mount has been manually set to the TTP pointing state
        //   Primary axis - 0 degrees = North with value increasing clockwise giving 90 degrees = East, 180 degrees = South and 270 = West.
        //   Secondary axis, 0 degrees at the horizon increasing to 90 degrees at the zenith

        // POLAR ALIGNMENT - Slews always use the Normal (pierEast) pointing state even when the mount has been manually set to the TTP pointing state
        //   Primary axis - 0 degrees = hour angle 0, -180 degrees = Hour angle -12 and +180 degrees = hour angle +12
        //   Secondary axis, 0 degrees at declination 0, +90 at the southern equatorial pole. Negative values at positive declinations towards the north.

        // GERMAN POLAR ALIGNMENT - Normal pointing state (pierEast) i.e. when observing hour angles 0 to +12.
        //   Primary axis - 0 degrees = hour angle 0, +180 degrees = hour angle +12
        //   Secondary axis - 0 degrees at declination 0, +90 at the southern equatorial pole. Negative values at positive declinations towards the north.

        // GERMAN POLAR ALIGNMENT - Through The Pole pointing state (pierWest) i.e. When observing hour angles -12 to 0.
        //   Primary axis - 0 degrees = hour angle -12, +180 degrees = hour angle 0
        //   Secondary axis - +90 at the equatorial pole. 180-declination at smaller declinations. Note that at positive declinations the axis angle will be >180

        #endregion

        #region Constants

        // Startup options values
        private const string STARTUP_OPTION_SIMULATOR_DEFAULT_POSITION = "Start up at simulator Default Position";

        private const string STARTUP_OPTION_START_POSITION = "Start up at configured Start Position";
        private const string STARTUP_OPTION_PARKED_POSITION = "Start up at configured Park Position";
        private const string STARTUP_OPTION_LASTUSED_POSITION = "Start up at last Shutdown Position";
        private const string STARTUP_OPTION_HOME_POSITION = "Start up at configured Home Position";

        // Useful mathematical constants
        private const double SIDEREAL_RATE_DEG_SEC = 15.041 / 3600;

        private const double SOLAR_RATE_DEG_SEC = 15.0 / 3600;
        private const double LUNAR_RATE_DEG_SEC = 14.515 / 3600;
        private const double KING_RATE_DEG_SEC = 15.037 / 3600;
        private const double DEGREES_TO_ARCSECONDS = 3600.0;
        private const double ARCSECONDS_TO_DEGREES = 1.0 / DEGREES_TO_ARCSECONDS;
        private const double ARCSECONDS_PER_RA_SECOND = 15.0; // To convert "seconds of RA" (24 hours = a whole circle) to arc seconds (360 degrees = a whole circle)
        private const double SIDEREAL_SECONDS_TO_SI_SECONDS = 0.99726956631945;
        private const double SI_SECONDS_TO_SIDEREAL_SECONDS = 1.0 / SIDEREAL_SECONDS_TO_SI_SECONDS;
        private const double SIDEREAL_RATE_DEG_PER_SIDEREAL_SECOND = 360.0 / (24.0 * 60.0 * 60.0); // Degrees per sidereal second, given the earth's rotation of 360 degrees in 1 sidereal day
        private const double SIDEREAL_RATE_DEG_PER_SI_SECOND = SIDEREAL_RATE_DEG_PER_SIDEREAL_SECOND / SIDEREAL_SECONDS_TO_SI_SECONDS; // Degrees per SI second

        #endregion Constants

        #region Private variables

        // change to using a Windows timer to avoid threading problems
        private static System.Timers.Timer s_wTimer;

        private static long idCount; // Counter to generate ever increasing sequential ID numbers

        // this emulates a hardware connection
        // the dictionary maintains a list of connected drivers, a dictionary is used so only one of each
        // driver is maintained.
        // Connected will be reported as true if any driver is connected
        // each driver instance has a unique id generated using ObjectIDGenerator
        private static ConcurrentDictionary<long, bool> connectStates;// = new ConcurrentDictionary<long, bool>();

        private static readonly object getIdLockObj = new object();

        public static IProfile s_Profile;
        private static bool onTop;
        public static ILogger TL;

        //Capabilities
        private static bool canFindHome;

        private static bool canPark;
        private static bool versionOne;
        private static int numberMoveAxis;
        private static bool canPulseGuide;
        private static bool canDualAxisPulseGuide;
        private static bool canSetEquatorialRates;
        private static bool canSetGuideRates;
        private static bool canSetPark;
        private static bool canSetPointingState;
        private static bool canSetTracking;
        private static bool canSlew;
        private static bool canSlewAltAz;
        private static bool canAlignmentMode;
        private static bool canOptics;
        private static bool canSlewAltAzAsync;
        private static bool canSlewAsync;
        private static bool canSync;
        private static bool canSyncAltAz;
        private static bool canUnpark;
        private static bool canAltAz;
        private static bool canDateTime;
        private static bool canDoesRefraction;
        private static bool canEquatorial;
        private static bool canLatLongElev;
        private static bool canSiderealTime;
        private static bool canPointingState;
        private static bool canDestinationSideOfPier;
        private static bool canTrackingRates;

        //Telescope Implementation
        private static AlignmentMode alignmentMode;

        private static double apertureArea;
        private static double apertureDiameter;
        private static double focalLength;
        private static bool autoTrack;
        private static bool disconnectOnPark;
        private static bool refraction;
        private static int equatorialSystem;
        private static bool noCoordinatesAtPark;
        private static double latitude;
        private static double longitude;
        private static double elevation;
        private static int maximumSlewRate;
        private static bool noSyncPastMeridian;

        //
        // Vectors are used for pairs of angles that represent the various positions and rates
        //
        // X is the primary axis, Hour angle, Right Ascension or azimuth and Y is the secondary axis,
        // declination or altitude.
        //
        // Ra and hour angle are in hours and the mount positions, Declination, azimuth and altitude are in degrees.
        //

        /// <summary>
        /// Current azimuth (X) and altitude (Y )in degrees, derived from the mountAxes Vector
        /// </summary>
        private static Vector altAzm;

        /// <summary>
        /// Park axis positions, X primary, Y secondary in Alt/Az degrees
        /// </summary>
        private static Vector parkPosition;

        /// <summary>
        /// current Ra (X, hrs) and Dec (Y, deg), derived from the mount axes
        /// </summary>
        private static Vector currentRaDec;

        /// <summary>
        /// Target right ascension (X, hrs) and declination (Y, deg)
        /// </summary>
        private static Vector targetRaDec;

        /// <summary>
        /// Flag to say which Telescope position will be used when the simulator is started
        /// </summary>
        private static string startupMode;

        private static DateTime settleTime;

        //private static SlewType slewState;

        // speeds are in deg/sec.
        private static double slewSpeedFast;

        private static double slewSpeedMedium;
        private static double slewSpeedSlow;

        /// <summary>
        /// Shutdown position in Alt/Az degrees
        /// </summary>
        private static Vector shutdownPosition = new Vector();

        /// <summary>
        /// Right Ascension (X) and declination (Y) rates (deg/sec) set through the RightAscensionRate and DeclinationRate properties
        ///  The "Internal" vector holds values in the units used internally by the simulator (degrees per SI second)
        ///  The "External" vector holds values in the units specified in the telescope interface standard (arc-seconds per sidereal second for RightAscensionRate and arc-seconds per SI second for DeclinationRate)
        /// </summary>
        private static Vector rateRaDecOffsetInternal = new Vector();

        private static Vector rateRaDecOffsetExternal = new Vector();

        private static int dateDelta;

        // Telescope mount simulation variables
        // The telescope is implemented using two axes that represent the primary and secondary telescope axes.
        // The role of the axes varies depending on the mount type.
        // The primary axis is the azimuth axis for an AltAz mount
        // and the hour angle axis for polar mounts.
        // The secondary axis is the altitude axis for AltAz mounts and the declination axis for polar mounts.
        //
        // all motion is done and all positions are set and obtained using these axes.
        //

        /// <summary>
        /// Axis position in mount axis degrees. X is primary (RA or Azimuth axis), Y is secondary (Dec or Altitude axis)
        /// </summary>
        private static Vector mountAxes;

        /// <summary>
        /// Slew target in mount axis degrees
        /// </summary>
        private static Vector targetAxes;

        private static double hourAngleLimit = 20;     // the number of degrees a GEM can go past the meridian

        private static PointingState pointingState;
        private static TrackingMode trackingMode;
        private static bool slewing;

        private static DateTime lastUpdateTime;

        #endregion Private variables

        #region Internal variables

        // durations are in secs.
        internal static double GuideDurationShort { get; private set; }

        internal static double GuideDurationMedium { get; private set; }

        internal static double GuideDurationLong { get; private set; }

        // Internal variables used to communicate with the Startup / Park / Home configuration form
        /// <summary>
        /// Start position in Alt/Az degrees
        /// </summary>
        internal static Vector StartCoordinates = new Vector();

        /// <summary>
        /// Home position - X = Azimuth, Y= Altitude (degrees)
        /// </summary>
        internal static Vector HomePosition;

        internal static List<string> StartupOptions = new List<string>() { STARTUP_OPTION_SIMULATOR_DEFAULT_POSITION, STARTUP_OPTION_LASTUSED_POSITION, STARTUP_OPTION_START_POSITION, STARTUP_OPTION_PARKED_POSITION, STARTUP_OPTION_HOME_POSITION };

        #endregion Internal variables

        #region Public variables

        /// <summary>
        /// Guide rates, deg/sec. X Ra/Azm, Y Alt/Dec
        /// </summary>
        public static Vector guideRate = new Vector();

        public static bool isPulseGuidingRa;

        public static bool isPulseGuidingDec;

        /// <summary>
        /// duration in seconds for guiding
        /// </summary>
        public static Vector guideDuration = new Vector();

        /// <summary>
        /// Axis Rates (deg/sec) set by the MoveAxis method
        /// </summary>
        public static Vector rateMoveAxes = new Vector();

        #endregion Public variables

        #region Enums

        internal enum TrackingMode
        {
            Off,
            AltAz,
            EqN,
            EqS
        }

        /*private enum PointingState
        {
            Normal,
            ThroughThePole
        }*/

        #endregion Enums

        #region Initialiser, Simulator start and timer functions

        /// <summary>
        /// Static initialiser for the TelescopeHardware class
        /// </summary>
        static TelescopeHardware()
        {
        }

        public static void ClearProfile()
        {
            s_Profile.Clear();
        }

        public static void Init()
        {
            try
            {
                s_wTimer = new System.Timers.Timer();
                s_wTimer.Interval = (int)(SharedResources.TIMER_INTERVAL * 1000);
                s_wTimer.Elapsed += M_wTimer_Tick;

                SouthernHemisphere = false;
                //Connected = false;
                rateMoveAxes = new Vector();

                LogMessage("TelescopeHardware", string.Format("Alignment mode 1: {0}", alignmentMode));
                connectStates = new ConcurrentDictionary<long, bool>();
                idCount = 0; // Initialise count to zero

                // check if the profile settings are correct
                if (s_Profile.GetValue("RegVer", string.Empty) != SharedResources.REGISTRATION_VERSION)
                {
                    // load the default settings
                    //Main Driver Settings
                    s_Profile.WriteValue("RegVer", SharedResources.REGISTRATION_VERSION);
                    s_Profile.WriteValue("AlwaysOnTop", "false");

                    // Telescope Implemention
                    // Initialise mount type to German Polar
                    s_Profile.WriteValue("AlignMode", ((int)AlignmentMode.GermanPolar).ToString()); // 1 = Start as German Polar m9ount type
                    alignmentMode = AlignmentMode.GermanPolar; // Added by Peter because the Profile setting was set to German Polar but the alignment mode value at this point was still zer0 = Alt/Az!

                    s_Profile.WriteValue("ApertureArea", SharedResources.INSTRUMENT_APERTURE_AREA.ToString(CultureInfo.InvariantCulture));
                    s_Profile.WriteValue("Aperture", SharedResources.INSTRUMENT_APERTURE.ToString(CultureInfo.InvariantCulture));
                    s_Profile.WriteValue("FocalLength", SharedResources.INSTRUMENT_FOCAL_LENGTH.ToString(CultureInfo.InvariantCulture));
                    s_Profile.WriteValue("AutoTrack", "false");
                    s_Profile.WriteValue("DiscPark", "false");
                    s_Profile.WriteValue("NoCoordAtPark", "false");
                    s_Profile.WriteValue("Refraction", "true");
                    s_Profile.WriteValue("EquatorialSystem", "1");
                    s_Profile.WriteValue("MaxSlewRate", "20");

                    //' Geography
                    //'
                    //' Based on the UTC offset, create a longitude somewhere in
                    //' the time zone, a latitude between 0 and 60 and a site
                    //' elevation between 0 and 1000 metres. This gives the
                    //' client some geo position without having to open the
                    //' Setup dialog.
                    Random r = new Random();
                    var localZone = TimeZoneInfo.Local;
                    double lat = 51.07861;// (r.NextDouble() * 60); lock for testing
                    double lng = (((-(double)(localZone.GetUtcOffset(DateTime.Now).Seconds) / 3600) + r.NextDouble() - 0.5) * 15);
                    if (localZone.GetUtcOffset(DateTime.Now).Seconds == 0) lng = -0.29444; //lock for testing
                    s_Profile.WriteValue("Elevation", Math.Round((r.NextDouble() * 1000), 0).ToString(CultureInfo.InvariantCulture));
                    s_Profile.WriteValue("Longitude", lng.ToString(CultureInfo.InvariantCulture));
                    s_Profile.WriteValue("Latitude", lat.ToString(CultureInfo.InvariantCulture));

                    //Start the scope in parked position
                    if (lat >= 0)
                    {
                        s_Profile.WriteValue("StartAzimuth", "180");
                        s_Profile.WriteValue("ParkAzimuth", "180");
                    }
                    else
                    {
                        s_Profile.WriteValue("StartAzimuth", "90");
                        s_Profile.WriteValue("ParkAzimuth", "90");
                    }

                    s_Profile.WriteValue("StartAltitude", (90 - Math.Abs(lat)).ToString(CultureInfo.InvariantCulture));
                    s_Profile.WriteValue("ParkAltitude", (90 - Math.Abs(lat)).ToString(CultureInfo.InvariantCulture));

                    s_Profile.WriteValue("DateDelta", "0");

                    // set default home and configured start positions
                    HomePosition = new Vector();
                    LogMessage("TelescopeHardware", string.Format("Alignment mode 2: {0}", alignmentMode));
                    switch (alignmentMode)
                    {
                        case AlignmentMode.GermanPolar:
                            // looking at the pole, counterweight down
                            HomePosition.X = 0;
                            HomePosition.Y = lat;
                            LogMessage("TelescopeHardware", string.Format("German Polar - Setting HomeAxes to {0} {1}", HomePosition.X.ToString(CultureInfo.InvariantCulture), HomePosition.Y.ToString(CultureInfo.InvariantCulture)));
                            s_Profile.WriteValue("StartAzimuthConfigured", HomePosition.X.ToString(CultureInfo.InvariantCulture));
                            s_Profile.WriteValue("StartAltitudeConfigured", HomePosition.Y.ToString(CultureInfo.InvariantCulture));
                            break;

                        case AlignmentMode.Polar:
                            // looking East, tube level
                            HomePosition.X = 90;
                            HomePosition.Y = 0;
                            LogMessage("TelescopeHardware", string.Format("Polar - Setting HomeAxes to {0} {1}", HomePosition.X.ToString(CultureInfo.InvariantCulture), HomePosition.Y.ToString(CultureInfo.InvariantCulture)));
                            s_Profile.WriteValue("StartAltitudeConfigured", HomePosition.X.ToString(CultureInfo.InvariantCulture));
                            s_Profile.WriteValue("StartAzimuthConfigured", HomePosition.Y.ToString(CultureInfo.InvariantCulture));
                            break;

                        case AlignmentMode.AltAz:
                            HomePosition.X = 0;    // AltAz is North and Level, hope Meade don't mind!
                            HomePosition.Y = 0;
                            LogMessage("TelescopeHardware", string.Format("Alt/Az - Setting HomeAxes to {0} {1}", HomePosition.X.ToString(CultureInfo.InvariantCulture), HomePosition.Y.ToString(CultureInfo.InvariantCulture)));
                            s_Profile.WriteValue("StartAltitudeConfigured", HomePosition.X.ToString(CultureInfo.InvariantCulture));
                            s_Profile.WriteValue("StartAzimuthConfigured", HomePosition.Y.ToString(CultureInfo.InvariantCulture));
                            break;
                    }
                    s_Profile.WriteValue("HomeAzimuth", HomePosition.X.ToString(CultureInfo.InvariantCulture));
                    s_Profile.WriteValue("HomeAltitude", HomePosition.Y.ToString(CultureInfo.InvariantCulture));

                    s_Profile.WriteValue("ShutdownAzimuth", HomePosition.X.ToString(CultureInfo.InvariantCulture)); // Set some default last shutdown values
                    s_Profile.WriteValue("ShutdownAltitude", HomePosition.Y.ToString(CultureInfo.InvariantCulture));

                    s_Profile.WriteValue("StartUpMode", STARTUP_OPTION_SIMULATOR_DEFAULT_POSITION); // Set the original simulator behaviour as the default staretup mode

                    //Capabilities Settings
                    s_Profile.WriteValue("V1", "false");
                    s_Profile.WriteValue("CanFindHome", "true");
                    s_Profile.WriteValue("CanPark", "true");
                    s_Profile.WriteValue("NumMoveAxis", "2");
                    s_Profile.WriteValue("CanPulseGuide", "true");
                    s_Profile.WriteValue("CanSetEquRates", "true");
                    s_Profile.WriteValue("CanSetGuideRates", "true");
                    s_Profile.WriteValue("CanSetPark", "true");
                    s_Profile.WriteValue("CanSetPointingState", "true");
                    s_Profile.WriteValue("CanSetTracking", "true");
                    s_Profile.WriteValue("CanSlew", "true");
                    s_Profile.WriteValue("CanSlewAltAz", "true");
                    s_Profile.WriteValue("CanAlignMode", "true");
                    s_Profile.WriteValue("CanOptics", "true");
                    s_Profile.WriteValue("CanSlewAltAzAsync", "true");
                    s_Profile.WriteValue("CanSlewAsync", "true");
                    s_Profile.WriteValue("CanSync", "true");
                    s_Profile.WriteValue("CanSyncAltAz", "true");
                    s_Profile.WriteValue("CanUnpark", "true");
                    s_Profile.WriteValue("CanAltAz", "true");
                    s_Profile.WriteValue("CanDateTime", "true");
                    s_Profile.WriteValue("CanDoesRefraction", "true");
                    s_Profile.WriteValue("CanEquatorial", "true");
                    s_Profile.WriteValue("CanLatLongElev", "true");
                    s_Profile.WriteValue("CanSiderealTime", "true");
                    s_Profile.WriteValue("CanPointingState", "true");
                    s_Profile.WriteValue("CanDestinationSideOfPier", "true");
                    s_Profile.WriteValue("CanTrackingRates", "true");
                    s_Profile.WriteValue("CanDualAxisPulseGuide", "true");
                }

                //Load up the values from saved
                onTop = bool.Parse(s_Profile.GetValue("AlwaysOnTop"));

                switch (int.Parse(s_Profile.GetValue("AlignMode"), CultureInfo.InvariantCulture))
                {
                    case (int)AlignmentMode.AltAz:
                        alignmentMode = AlignmentMode.AltAz;
                        break;

                    case (int)AlignmentMode.Polar:
                        alignmentMode = AlignmentMode.Polar;
                        break;

                    case (int)AlignmentMode.GermanPolar:
                        alignmentMode = AlignmentMode.GermanPolar;
                        break;

                    default:
                        alignmentMode = AlignmentMode.GermanPolar;
                        break;
                }

                apertureArea = double.Parse(s_Profile.GetValue("ApertureArea"), CultureInfo.InvariantCulture);
                apertureDiameter = double.Parse(s_Profile.GetValue("Aperture"), CultureInfo.InvariantCulture);
                focalLength = double.Parse(s_Profile.GetValue("FocalLength"), CultureInfo.InvariantCulture);
                autoTrack = bool.Parse(s_Profile.GetValue("AutoTrack"));
                disconnectOnPark = bool.Parse(s_Profile.GetValue("DiscPark"));
                refraction = bool.Parse(s_Profile.GetValue("Refraction"));
                equatorialSystem = int.Parse(s_Profile.GetValue("EquatorialSystem"), CultureInfo.InvariantCulture);
                noCoordinatesAtPark = bool.Parse(s_Profile.GetValue("NoCoordAtPark"));
                elevation = double.Parse(s_Profile.GetValue("Elevation"), CultureInfo.InvariantCulture);
                latitude = double.Parse(s_Profile.GetValue("Latitude"), CultureInfo.InvariantCulture);
                longitude = double.Parse(s_Profile.GetValue("Longitude"), CultureInfo.InvariantCulture);
                maximumSlewRate = int.Parse(s_Profile.GetValue("MaxSlewRate"), CultureInfo.InvariantCulture);

                altAzm.Y = double.Parse(s_Profile.GetValue("StartAltitude", "0"), CultureInfo.InvariantCulture); // Get the default start position
                altAzm.X = double.Parse(s_Profile.GetValue("StartAzimuth", "0"), CultureInfo.InvariantCulture);
                StartCoordinates.Y = double.Parse(s_Profile.GetValue("StartAltitudeConfigured", "0"), CultureInfo.InvariantCulture); // Get the configured start position
                StartCoordinates.X = double.Parse(s_Profile.GetValue("StartAzimuthConfigured", "0"), CultureInfo.InvariantCulture);

                parkPosition.Y = double.Parse(s_Profile.GetValue("ParkAltitude", "0"), CultureInfo.InvariantCulture);
                parkPosition.X = double.Parse(s_Profile.GetValue("ParkAzimuth", "0"), CultureInfo.InvariantCulture);

                // Retrieve the Home position
                HomePosition.X = double.Parse(s_Profile.GetValue("HomeAzimuth", "0"), CultureInfo.InvariantCulture);
                HomePosition.Y = double.Parse(s_Profile.GetValue("HomeAltitude", "0"), CultureInfo.InvariantCulture);

                // Retrieve the previous shutdown position
                shutdownPosition.X = double.Parse(s_Profile.GetValue("ShutdownAzimuth", "0"), CultureInfo.InvariantCulture);
                shutdownPosition.Y = double.Parse(s_Profile.GetValue("ShutdownAltitude", "0"), CultureInfo.InvariantCulture);

                // Retrieve the startup mode
                startupMode = s_Profile.GetValue("StartUpMode", STARTUP_OPTION_SIMULATOR_DEFAULT_POSITION);

                // Select the configured startup position
                switch (startupMode)
                {
                    case STARTUP_OPTION_SIMULATOR_DEFAULT_POSITION: // No action just go with the built-in values already in altAzm
                        break;

                    case STARTUP_OPTION_PARKED_POSITION:
                        altAzm = parkPosition;
                        break;

                    case STARTUP_OPTION_START_POSITION:
                        altAzm = StartCoordinates;
                        break;

                    case STARTUP_OPTION_LASTUSED_POSITION:
                        altAzm = shutdownPosition;
                        break;

                    case STARTUP_OPTION_HOME_POSITION:
                        altAzm = HomePosition;
                        break;

                    default: // No action just go with the built-in simulator startup position already in altAzm
                        break;
                }

                //TODO allow for version 1, 2 or 3
                versionOne = bool.Parse(s_Profile.GetValue("V1"));
                canFindHome = bool.Parse(s_Profile.GetValue("CanFindHome"));
                canPark = bool.Parse(s_Profile.GetValue("CanPark"));
                numberMoveAxis = int.Parse(s_Profile.GetValue("NumMoveAxis"), CultureInfo.InvariantCulture);
                canPulseGuide = bool.Parse(s_Profile.GetValue("CanPulseGuide"));
                canSetEquatorialRates = bool.Parse(s_Profile.GetValue("CanSetEquRates"));
                canSetGuideRates = bool.Parse(s_Profile.GetValue("CanSetGuideRates"));
                canSetPark = bool.Parse(s_Profile.GetValue("CanSetPark"));
                canSetPointingState = bool.Parse(s_Profile.GetValue("CanSetPointingState"));
                canSetTracking = bool.Parse(s_Profile.GetValue("CanSetTracking"));
                canSlew = bool.Parse(s_Profile.GetValue("CanSlew"));
                canSlewAltAz = bool.Parse(s_Profile.GetValue("CanSlewAltAz"));
                canAlignmentMode = bool.Parse(s_Profile.GetValue("CanAlignMode"));
                canOptics = bool.Parse(s_Profile.GetValue("CanOptics"));
                canSlewAltAzAsync = bool.Parse(s_Profile.GetValue("CanSlewAltAzAsync"));
                canSlewAsync = bool.Parse(s_Profile.GetValue("CanSlewAsync"));
                canSync = bool.Parse(s_Profile.GetValue("CanSync"));
                canSyncAltAz = bool.Parse(s_Profile.GetValue("CanSyncAltAz"));
                canUnpark = bool.Parse(s_Profile.GetValue("CanUnpark"));
                canAltAz = bool.Parse(s_Profile.GetValue("CanAltAz"));
                canDateTime = bool.Parse(s_Profile.GetValue("CanDateTime"));
                canDoesRefraction = bool.Parse(s_Profile.GetValue("CanDoesRefraction"));
                canEquatorial = bool.Parse(s_Profile.GetValue("CanEquatorial"));
                canLatLongElev = bool.Parse(s_Profile.GetValue("CanLatLongElev"));
                canSiderealTime = bool.Parse(s_Profile.GetValue("CanSiderealTime"));
                canPointingState = bool.Parse(s_Profile.GetValue("CanPointingState"));
                canDestinationSideOfPier = bool.Parse(s_Profile.GetValue("CanDestinationSideOfPier", "True"));
                canTrackingRates = bool.Parse(s_Profile.GetValue("CanTrackingRates"));
                canDualAxisPulseGuide = bool.Parse(s_Profile.GetValue("CanDualAxisPulseGuide"));
                noSyncPastMeridian = bool.Parse(s_Profile.GetValue("NoSyncPastMeridian", "false"));

                dateDelta = int.Parse(s_Profile.GetValue("DateDelta"), CultureInfo.InvariantCulture);

                if (latitude < 0) { SouthernHemisphere = true; }

                slewSpeedFast = maximumSlewRate * SharedResources.TIMER_INTERVAL;
                slewSpeedMedium = slewSpeedFast * 0.1;
                slewSpeedSlow = slewSpeedFast * 0.02;
                SlewDirection = SlewDirection.SlewNone;

                GuideDurationShort = 0.8 * SharedResources.TIMER_INTERVAL;
                GuideDurationMedium = 2.0 * GuideDurationShort;
                GuideDurationLong = 2.0 * GuideDurationMedium;

                guideRate.X = 15.0 * (1.0 / 3600.0) / SharedResources.SIDRATE;
                guideRate.Y = guideRate.X;
                rateRaDecOffsetInternal.Y = 0;
                rateRaDecOffsetInternal.X = 0;

                TrackingRate = DriveRate.Sidereal;
                SlewSettleTime = 0;
                ChangePark(AtPark);

                // invalid target position
                targetRaDec = new Vector(double.NaN, double.NaN);
                SlewState = SlewType.SlewNone;

                mountAxes = MountFunctions.ConvertAltAzmToAxes(altAzm); // Convert the start position AltAz coordinates into the current axes representation and set this as the simulator start position
                LogMessage("TelescopeHardware New", string.Format("Startup mode: {0}, Azimuth: {1}, Altitude: {2}", startupMode, altAzm.X.ToString(CultureInfo.InvariantCulture), altAzm.Y.ToString(CultureInfo.InvariantCulture)));

                LogMessage("TelescopeHardware New", "Successfully initialised hardware");
            }
            catch (Exception ex)
            {
                TL.LogError($"TelescopeHardware Initialiser Exception: {ex.Message}");
            }
        }

        /// <summary>
        /// This was stored by a Form in the old simulator. For now stored by this function.
        /// </summary>
        internal static void StoreHomeParkStart()
        {
            s_Profile.WriteValue("HomeAzimuth", TelescopeHardware.HomePosition.X.ToString(CultureInfo.InvariantCulture));
            s_Profile.WriteValue("HomeAltitude", TelescopeHardware.HomePosition.Y.ToString(CultureInfo.InvariantCulture));
            s_Profile.WriteValue("ParkAzimuth", TelescopeHardware.ParkAzimuth.ToString(CultureInfo.InvariantCulture));
            s_Profile.WriteValue("ParkAltitude", TelescopeHardware.ParkAltitude.ToString(CultureInfo.InvariantCulture));
            s_Profile.WriteValue("StartAzimuthConfigured", TelescopeHardware.StartCoordinates.X.ToString(CultureInfo.InvariantCulture));
            s_Profile.WriteValue("StartAltitudeConfigured", TelescopeHardware.StartCoordinates.Y.ToString(CultureInfo.InvariantCulture));
        }

        internal static void ShutdownTelescope()
        {
            try { s_Profile.WriteValue("ShutdownAzimuth", TelescopeHardware.Azimuth.ToString(CultureInfo.InvariantCulture)); } catch { }
            try { s_Profile.WriteValue("ShutdownAltitude", TelescopeHardware.Altitude.ToString(CultureInfo.InvariantCulture)); } catch { }
        }

        public static void Start()
        {
            //Connected = false;
            Tracking = AutoTrack;
            AtPark = false;

            rateMoveAxes.X = 0;
            rateMoveAxes.Y = 0;

            lastUpdateTime = DateTime.Now;
            s_wTimer.Start();
        }

        //Update the Telescope Based on Timed Events
        private static void M_wTimer_Tick(object sender, EventArgs e)
        {
            MoveAxes();
        }

        /// <summary>
        /// This is called every TIMER_INTERVAL period and applies the current movement rates to the axes,
        /// copes with the range and updates the displayed values
        /// </summary>
        private static void MoveAxes()
        {
            // get the time since the last update. This avoids problems with the timer interval varying and greatly improves tracking.
            DateTime now = DateTime.Now;
            double timeInSecondsSinceLastUpdate = (now - lastUpdateTime).TotalSeconds;
            lastUpdateTime = now;

            // Find the hour angle change (in degrees) due to tracking that occurred during this interval
            double haChange = GetTrackingChangeInDegrees(timeInSecondsSinceLastUpdate);

            // This vector accumulates all changes to the current primary and secondary axis positions as a result of movement during this update interval
            Vector change = new Vector();

            // Apply tracking changes
            if ((rateMoveAxes.X == 0.0) & (rateMoveAxes.Y == 0.0)) // No MoveAxis rates have been set so handle normally
            {
                // Determine the changes in current axis position and target axis position required as a result of tracking
                if (Tracking) // Tracking is enabled
                {
                    switch (alignmentMode)
                    {
                        case AlignmentMode.GermanPolar: // In polar aligned mounts an HA change moves only the RA (primary) axis so update this, no change is required to the Dec (secondary) axis
                        case AlignmentMode.Polar:
                            // Set the change in the primary (RA) axis position due to tracking 
                            change.X = haChange; // Set the change in the RA (primary) current axis position due to tracking 

                            // Update the slew target's RA (primary) axis position that will also have changed due to tracking
                            targetAxes.X += haChange;

                            // Apply the RightAscensionRate offset
                            // The RA rate offset (rateRaDecOffsetInternal.X) is subtracted because the primary RA axis increases its angle value in a clockwise direction
                            // but RA decreases when moving in this direction
                            change.X -= rateRaDecOffsetInternal.X * timeInSecondsSinceLastUpdate;

                            // Apply the DeclinationRate offset
                            // The relationship between the declination axis rotation direction and the associated declination value switches from
                            // correlated (declination increases as mechanical angle increases) to inverted (declination decreases as mechanical angle increases) depending on the mount pointing state.
                            // In addition, the sense of required rate corrections is inverted when in the southern hemisphere compared to the northern hemisphere
                            if (SouthernHemisphere) // Southern hemisphere
                            {
                                change.Y += (SideOfPier == PointingState.Normal? -rateRaDecOffsetInternal.Y : +rateRaDecOffsetInternal.Y) * timeInSecondsSinceLastUpdate; // Add or subtract declination rate depending on pointing state
                            }
                            else // Northern hemisphere
                            {
                                change.Y += (SideOfPier == PointingState.Normal ? +rateRaDecOffsetInternal.Y : -rateRaDecOffsetInternal.Y) * timeInSecondsSinceLastUpdate; // Add or subtract declination rate depending on pointing state
                            }

                            TL.LogMessage(LogLevel.Verbose, "MoveAxes", $"RA internal offset rate: {rateRaDecOffsetInternal.X}, Dec internal offset rate: {rateRaDecOffsetInternal.Y}. " +
                                $"Total change this interval: {change.X}, {change.Y}. Time since last update: {timeInSecondsSinceLastUpdate}"
                                );
                            break;

                        case AlignmentMode.AltAz: // In Alt/Az aligned mounts the HA change moves both RA (primary) and Dec (secondary) axes so both need to be updated

                            // Set the change in the Azimuth (primary) and Altitude (secondary) axis positions due to tracking plus any RA / dec rate offsets
                            // The RA rate offset (rateRaDecOffsetInternal.X) is subtracted because the primary RA axis increases its angle value in a clockwise direction
                            // but RA decreases when moving in this direction
                            change = ConvertRateToAltAz(haChange / timeInSecondsSinceLastUpdate - rateRaDecOffsetInternal.X, rateRaDecOffsetInternal.Y, timeInSecondsSinceLastUpdate);

                            // Update the slew target's Azimuth (primary) and Altitude (secondary) axis positions that will also have changed due to tracking
                            targetAxes = MountFunctions.ConvertRaDecToAxes(targetRaDec, false);
                            break;
                    }

                    TL.LogMessage(LogLevel.Verbose, "MoveAxes", $"Time since last update: {timeInSecondsSinceLastUpdate} seconds. HA change {haChange} degrees. Alignment mode: {alignmentMode}.");
                    TL.LogMessage(LogLevel.Verbose, "MoveAxes", $"Movement - Primary axis: {change.X} degrees, Secondary axis: {change.Y} degrees.");
                    TL.LogMessage(LogLevel.Verbose, "MoveAxes", $"Movement rate - Primary axis: {change.X * DEGREES_TO_ARCSECONDS / timeInSecondsSinceLastUpdate} arc-seconds per SI second, " +
                        $"Secondary axis: {change.Y * DEGREES_TO_ARCSECONDS / timeInSecondsSinceLastUpdate} arc-seconds per SI second");
                    TL.LogMessage(LogLevel.Verbose, "MoveAxes", $"Movement includes RightAscensionRate/DeclinationRate movement of: {rateRaDecOffsetInternal.X * timeInSecondsSinceLastUpdate} degrees, {rateRaDecOffsetInternal.Y * timeInSecondsSinceLastUpdate} degrees.");
                } // Mount is tracking
                else // Mount is not tracking
                {
                    // No axis change
                    TL.LogMessage(LogLevel.Verbose, "MoveAxes", $"Tracking disabled - no changes.Time since last update: {timeInSecondsSinceLastUpdate} seconds.");
                } // Mount is not tracking

                // Move towards the target position if slewing
                change += DoSlew();

            } // MoveAxis is not active
            else // A moveAxis rate has been set so treat as a Move 
            {
                switch (alignmentMode)
                {
                    case AlignmentMode.AltAz:
                        // Direction sense is the same in both hemispheres
                        change = Vector.Multiply(rateMoveAxes, timeInSecondsSinceLastUpdate);
                        break;

                    case AlignmentMode.Polar:
                        // NORTHERN HEMISPHERE: Positive axis rates increase the secondary axis angle resulting in increases in declination, which is what we want
                        // SOUTHERN HEMISPHERE: Positive axis rates increase the secondary axis angle resulting in decreases in declination, so we reverse the sense here to ensure that positive axis rates result in increases in declination
                        if (SouthernHemisphere) // In the southern hemisphere
                        {
                            change.X = rateMoveAxes.X * timeInSecondsSinceLastUpdate; // Retain the primary axis direction sense
                            change.Y = -rateMoveAxes.Y * timeInSecondsSinceLastUpdate; // Swap the secondary axis direction sense
                        }
                        else // In the northern hemisphere
                        {
                            change = Vector.Multiply(rateMoveAxes, timeInSecondsSinceLastUpdate); // Retain both primary and secondary axis senses
                        }
                        break;

                    case AlignmentMode.GermanPolar:
                        // NORTHERN HEMISPHERE - NORMAL POINTING STATE (pierEast): Positive axis rates increase the secondary axis angle resulting in increases in declination, which is what we want
                        // NORTHERN HEMISPHERE - THROUGH THE POLE POINTING STATE (pierWest): Positive axis rates increase the secondary axis angle resulting in decreases in declination, which is what we want
                        // SOUTHERN HEMISPHERE - NORMAL POINTING STATE (pierEast): Positive axis rates increase the secondary axis angle resulting in decreases in declination, so we reverse the sense here to ensure that positive axis rates result in increases in declination
                        // SOUTHERN HEMISPHERE - THROUGH THE POLE POINTING STATE (pierWest): Positive axis rates increase the secondary axis angle resulting in decreases in declination, so we reverse the sense here to ensure that positive axis rates result in increases in declination
                        if (SouthernHemisphere) // In the southern hemisphere
                        {
                            change.X = rateMoveAxes.X * timeInSecondsSinceLastUpdate; // Retain the primary axis direction sense
                            change.Y = -rateMoveAxes.Y * timeInSecondsSinceLastUpdate; // Swap the secondary axis direction sense
                        }
                        else // In the northern hemisphere
                        {
                            change = Vector.Multiply(rateMoveAxes, timeInSecondsSinceLastUpdate); // Retain both primary and secondary axis senses
                        }
                        break;

                    default:
                        break;
                }
                TL.LogMessage(LogLevel.Verbose, "MoveAxes MoveAxis", $"Primary axis move rate: {rateMoveAxes.X}, Secondary axis move rate: {rateMoveAxes.Y}. Applied changes - Primary axis: {change.X}, Secondary axis: {change.Y}. Time since last update: {timeInSecondsSinceLastUpdate} seconds.");

            } // MoveAxis is active

            // Move towards the target position if slewing
            change += DoSlew();

            // handle HC button moves
            change += HcMoves();

            // Pulse guiding changes
            change += PulseGuide(timeInSecondsSinceLastUpdate);

            // Update the axis positions with the total change in this interval
            mountAxes += change;

            // check the axis values, stop movement past limits
            CheckAxisLimits(change.X);

            // update the displayed values
            UpdatePositions();

            // check and update slew state 
            switch (SlewState)
            {
                case SlewType.SlewSettle:
                    if (DateTime.Now >= settleTime)
                    {
                        SharedResources.TrafficLine(SharedResources.MessageType.Slew, "(Slew Complete)");
                        SlewState = SlewType.SlewNone;
                    }
                    break;
            }

            // List changes this cycle
            TL.LogMessage(LogLevel.Verbose, $"MoveAxes (Final)", $"RA: {currentRaDec.X.ToHMS()}, Dec: {currentRaDec.Y.ToDMS()}, Hour angle: {Utilities.ConditionHA(SiderealTime - currentRaDec.X).ToHMS()}, Sidereal Time: {SiderealTime.ToHMS()}");
            TL.LogMessage(LogLevel.Verbose, $"MoveAxes (Final)", $"Azimuth: {altAzm.X.ToDMS()}, Altitude: {altAzm.Y.ToDMS()}, Pointing state: {SideOfPier}");
            TL.LogMessage(LogLevel.Verbose, $"MoveAxes (Final)",
              $"Primary axis angle:  {mountAxes.X.ToDMS()}, Secondary axis angle:  {mountAxes.Y.ToDMS()}, " +
              $"Primary axis change: {change.X.ToDMS()}, Secondary axis change: {change.Y.ToDMS()}."
              );
            TL.BlankLine(LogLevel.Verbose);
        }

        #endregion Initialiser, Simulator start and timer functions

        #region Properties For Settings

        //I used some of these as dual purpose if the driver uses the same exact property
        public static AlignmentMode AlignmentMode
        {
            get { return alignmentMode; }
            set
            {
                alignmentMode = value;
                s_Profile.WriteValue("AlignMode", ((int)value).ToString());
            }
        }

        public static bool OnTop
        {
            get { return onTop; }
            set
            {
                onTop = value;
                s_Profile.WriteValue("OnTop", value.ToString());
            }
        }

        public static bool AutoTrack
        {
            get { return autoTrack; }
            set
            {
                autoTrack = value;
                s_Profile.WriteValue("AutoTrack", value.ToString());
            }
        }

        public static bool NoCoordinatesAtPark
        {
            get { return noCoordinatesAtPark; }
            set
            {
                noCoordinatesAtPark = value;
                s_Profile.WriteValue("NoCoordAtPark", value.ToString());
            }
        }

        public static bool VersionOneOnly
        {
            get { return versionOne; }
            set
            {
                s_Profile.WriteValue("V1", value.ToString());
                versionOne = value;
            }
        }

        public static bool DisconnectOnPark
        {
            get { return disconnectOnPark; }
            set
            {
                disconnectOnPark = value;
                s_Profile.WriteValue("DiscPark", value.ToString());
            }
        }

        public static bool Refraction
        {
            get { return refraction; }
            set
            {
                refraction = value;
                s_Profile.WriteValue("Refraction", value.ToString());
            }
        }

        public static int EquatorialSystem
        {
            get { return equatorialSystem; }
            set
            {
                equatorialSystem = value;
                s_Profile.WriteValue("EquatorialSystem", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        public static double Elevation
        {
            get { return elevation; }
            set
            {
                elevation = value;
                s_Profile.WriteValue("Elevation", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        public static double Latitude
        {
            get { return latitude; }
            set
            {
                latitude = value;
                s_Profile.WriteValue("Latitude", value.ToString(CultureInfo.InvariantCulture));

                // Assign the Southern hemisphere property to true or false depending on the site latitude
                SouthernHemisphere = latitude <= 0.0;
            }
        }

        public static double Longitude
        {
            get { return longitude; }
            set
            {
                longitude = value;
                s_Profile.WriteValue("Longitude", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        public static int MaximumSlewRate
        {
            get { return maximumSlewRate; }
            set
            {
                maximumSlewRate = value;
                s_Profile.WriteValue("MaxSlewRate", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        public static bool CanFindHome
        {
            get { return canFindHome; }
            set
            {
                canFindHome = value;
                s_Profile.WriteValue("CanFindHome", value.ToString());
            }
        }

        public static bool CanOptics
        {
            get { return canOptics; }
            set
            {
                canOptics = value;
                s_Profile.WriteValue("CanOptics", value.ToString());
            }
        }

        public static bool CanPark
        {
            get { return canPark; }
            set
            {
                canPark = value;
                s_Profile.WriteValue("CanPark", value.ToString());
            }
        }

        public static int NumberMoveAxis
        {
            get { return numberMoveAxis; }
            set
            {
                numberMoveAxis = value;
                s_Profile.WriteValue("NumMoveAxis", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        public static bool CanPulseGuide
        {
            get { return canPulseGuide; }
            set
            {
                canPulseGuide = value;
                s_Profile.WriteValue("CanPulseGuide", value.ToString());
            }
        }

        public static bool CanDualAxisPulseGuide
        {
            get { return canDualAxisPulseGuide; }
            set
            {
                canDualAxisPulseGuide = value;
                s_Profile.WriteValue("CanDualAxisPulseGuide", value.ToString());
            }
        }

        public static bool CanSetEquatorialRates
        {
            get { return canSetEquatorialRates; }
            set
            {
                canSetEquatorialRates = value;
                s_Profile.WriteValue("CanSetEquRates", value.ToString());
            }
        }

        public static bool CanSetGuideRates
        {
            get { return canSetGuideRates; }
            set
            {
                canSetGuideRates = value;
                s_Profile.WriteValue("CanSetGuideRates", value.ToString());
            }
        }

        public static bool CanSetPark
        {
            get { return canSetPark; }
            set
            {
                canSetPark = value;
                s_Profile.WriteValue("CanSetPark", value.ToString());
            }
        }

        public static bool CanPointingState
        {
            get { return canPointingState; }
            set
            {
                canPointingState = value;
                s_Profile.WriteValue("CanPointingState", value.ToString());
            }
        }

        public static bool CanDestinationSideofPier
        {
            get { return canDestinationSideOfPier; }
            set
            {
                canDestinationSideOfPier = value;
                s_Profile.WriteValue("CanDestinationSideOfPier", value.ToString());
            }
        }

        public static bool CanSetPointingState
        {
            get { return canSetPointingState; }
            set
            {
                canSetPointingState = value;
                s_Profile.WriteValue("CanSetPointingState", value.ToString());
            }
        }

        public static bool CanSetTracking
        {
            get { return canSetTracking; }
            set
            {
                canSetTracking = value;
                s_Profile.WriteValue("CanSetTracking", value.ToString());
            }
        }

        public static bool CanTrackingRates
        {
            get { return canTrackingRates; }
            set
            {
                canTrackingRates = value;
                s_Profile.WriteValue("CanTrackingRates", value.ToString());
            }
        }

        public static bool CanSlew
        {
            get { return canSlew; }
            set
            {
                canSlew = value;
                s_Profile.WriteValue("CanSlew", value.ToString());
            }
        }

        public static bool CanSync
        {
            get { return canSync; }
            set
            {
                canSync = value;
                s_Profile.WriteValue("CanSync", value.ToString());
            }
        }

        public static bool CanSlewAsync
        {
            get { return canSlewAsync; }
            set
            {
                canSlewAsync = value;
                s_Profile.WriteValue("CanSlewAsync", value.ToString());
            }
        }

        public static bool CanSlewAltAz
        {
            get { return canSlewAltAz; }
            set
            {
                canSlewAltAz = value;
                s_Profile.WriteValue("CanSlewAltAz", value.ToString());
            }
        }

        public static bool CanSyncAltAz
        {
            get { return canSyncAltAz; }
            set
            {
                canSyncAltAz = value;
                s_Profile.WriteValue("CanSyncAltAz", value.ToString());
            }
        }

        public static bool CanAltAz
        {
            get { return canAltAz; }
            set
            {
                canAltAz = value;
                s_Profile.WriteValue("CanAltAz", value.ToString());
            }
        }

        public static bool CanSlewAltAzAsync
        {
            get { return canSlewAltAzAsync; }
            set
            {
                canSlewAltAzAsync = value;
                s_Profile.WriteValue("CanSlewAltAzAsync", value.ToString());
            }
        }

        public static bool CanAlignmentMode
        {
            get { return canAlignmentMode; }
            set
            {
                canAlignmentMode = value;
                s_Profile.WriteValue("CanAlignMode", value.ToString());
            }
        }

        public static bool CanUnpark
        {
            get { return canUnpark; }
            set
            {
                canUnpark = value;
                s_Profile.WriteValue("CanUnpark", value.ToString());
            }
        }

        public static bool CanDateTime
        {
            get { return canDateTime; }
            set
            {
                canDateTime = value;
                s_Profile.WriteValue("CanDateTime", value.ToString());
            }
        }

        public static bool CanDoesRefraction
        {
            get { return canDoesRefraction; }
            set
            {
                canDoesRefraction = value;
                s_Profile.WriteValue("CanDoesRefraction", value.ToString());
            }
        }

        public static bool CanEquatorial
        {
            get { return canEquatorial; }
            set
            {
                canEquatorial = value;
                s_Profile.WriteValue("CanEquatorial", value.ToString());
            }
        }

        public static bool CanLatLongElev
        {
            get { return canLatLongElev; }
            set
            {
                canLatLongElev = value;
                s_Profile.WriteValue("CanLatLongElev", value.ToString());
            }
        }

        public static bool CanSiderealTime
        {
            get { return canSiderealTime; }
            set
            {
                canSiderealTime = value;
                s_Profile.WriteValue("CanSiderealTime", value.ToString());
            }
        }

        public static bool NoSyncPastMeridian
        {
            get { return noSyncPastMeridian; }
            set
            {
                noSyncPastMeridian = value;
                s_Profile.WriteValue("NoSyncPastMeridian", value.ToString());
            }
        }

        #endregion Properties For Settings

        #region Telescope Implementation

        public static double Altitude
        {
            get { return altAzm.Y; }
            set { altAzm.Y = value; }
        }

        public static bool AtPark { get; private set; }

        public static double Azimuth
        {
            get { return altAzm.X; }
            set { altAzm.X = value; }
        }

        public static double ParkAltitude
        {
            get { return parkPosition.Y; }
            set
            {
                parkPosition.Y = value;
                s_Profile.WriteValue("ParkAltitude", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        public static double ParkAzimuth
        {
            get { return parkPosition.X; }
            set
            {
                parkPosition.X = value;
                s_Profile.WriteValue("ParkAzimuth", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        public static long GetId()
        {
            lock (getIdLockObj)
            {
                Interlocked.Increment(ref idCount); // Increment the counter in a threadsafe fashion
                LogMessage("GetId", "Generated new ID: " + idCount.ToString());
                return idCount;
            }
        }

        public static bool Connected
        {
            get
            {
                TL.LogVerbose("Hardware.Connected Get - Number of connected devices: " + connectStates.Count + ", Returning: " + (connectStates.Count > 0).ToString());
                return connectStates.Count > 0;
            }
        }

        public static void SetConnected(long id, bool value)
        {
            // add or remove the instance, this is done once regardless of the number of calls
            if (value)
            {
                bool notAlreadyPresent = connectStates.TryAdd(id, true);
                LogMessage("Hardware.Connected Set", "Set Connected to: True, AlreadyConnected: " + (!notAlreadyPresent).ToString());
            }
            else
            {
                bool successfullyRemoved = connectStates.TryRemove(id, out value);
                LogMessage("Hardware.Connected Set", "Set Connected to: False, Successfully removed: " + successfullyRemoved.ToString());

                //Store out last location. Server shutdown should also store this
                if (!Connected)
                {
                    ShutdownTelescope();
                }
            }
        }

        public static bool CanMoveAxis(TelescopeAxis axis)
        {
            int ax = 0;
            switch (axis)
            {
                case TelescopeAxis.Primary:
                    ax = 1;
                    break;

                case TelescopeAxis.Secondary:
                    ax = 2;
                    break;

                case TelescopeAxis.Tertiary:
                    ax = 3;
                    break;
            }

            if (ax == 0 || ax > numberMoveAxis)
            { return false; }
            else
            { return true; }
        }

        public static bool CanSetDeclinationRate
        { get { return canSetEquatorialRates; } }

        public static bool CanSetRightAscensionRate
        { get { return canSetEquatorialRates; } }

        public static double ApertureArea
        {
            get { return apertureArea; }
            set
            {
                apertureArea = value;
                s_Profile.WriteValue("ApertureArea", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        public static double ApertureDiameter
        {
            get { return apertureDiameter; }
            set
            {
                apertureDiameter = value;
                s_Profile.WriteValue("Aperture", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        public static double FocalLength
        {
            get { return focalLength; }
            set
            {
                focalLength = value;
                s_Profile.WriteValue("FocalLength", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        public static bool SouthernHemisphere { get; private set; }

        public static double DeclinationRate
        {
            get { return rateRaDecOffsetExternal.Y; }
            set
            {
                rateRaDecOffsetExternal.Y = value; // Save the provided rate to be returned through the Get property
                rateRaDecOffsetInternal.Y = value * ARCSECONDS_TO_DEGREES; // Save the rate in the internal units that the simulator uses
            }
        }

        public static double Declination
        {
            get { return currentRaDec.Y; }
            set { currentRaDec.Y = value; }
        }

        public static double RightAscension
        {
            get { return currentRaDec.X; }
            set { currentRaDec.X = value; }
        }

        public static SlewType SlewState { get; private set; }

        public static SlewSpeed SlewSpeed { get; set; }

        public static SlewDirection SlewDirection { get; set; }

        /// <summary>
        /// report if the mount is at the home position by comparing it's position with the home position.
        /// </summary>
        public static bool AtHome
        {
            get
            {
                //LogMessage("AtHome", "Distance from Home: {0}, AtHome: {1}", (mountAxes - MountFunctions.ConvertAltAzmToAxes(HomePosition)).LengthSquared, (mountAxes - MountFunctions.ConvertAltAzmToAxes(HomePosition)).LengthSquared < 0.01);
                return (mountAxes - MountFunctions.ConvertAltAzmToAxes(HomePosition)).LengthSquared < 0.01;
            }
        }

        public static double SiderealTime { get; private set; }

        public static double TargetRightAscension
        {
            get { return targetRaDec.X; }
            set { targetRaDec.X = value; }
        }

        public static double TargetDeclination
        {
            get { return targetRaDec.Y; }
            set { targetRaDec.Y = value; }
        }

        public static bool Tracking
        {
            get
            {
                return trackingMode != TrackingMode.Off;
            }
            set
            {
                if (value)
                {
                    switch (AlignmentMode)
                    {
                        case AlignmentMode.AltAz:
                            trackingMode = TrackingMode.AltAz;
                            break;

                        case AlignmentMode.GermanPolar:
                        case AlignmentMode.Polar:
                            trackingMode = Latitude >= 0 ? TrackingMode.EqN : TrackingMode.EqS;
                            break;
                    }
                }
                else
                {
                    trackingMode = TrackingMode.Off;
                }
            }
        }

        public static int DateDelta
        {
            get { return dateDelta; }
            set
            {
                dateDelta = value;
                s_Profile.WriteValue("DateDelta", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Manages RightAscensionRate. Units are "seconds of RA per sidereal second".
        /// </summary>
        /// <remarks>
        /// 1) This property retains the value supplied by set RightAscensionRate in the rateRaDecOffsetExternal.X vector element so that it can be returned by get RightAscensionRate.
        /// 2) The set RightAscensionRate value is also converted to the internal units used by the simulator (arcsec per SI second) and stored in the rateRaDecOffsetInternal.X vector element
        /// </remarks>
        public static double RightAscensionRate
        {
            get { return rateRaDecOffsetExternal.X; }
            set
            {
                // Save the provided rate (seconds of RA per sidereal second) to be returned through the Get property
                rateRaDecOffsetExternal.X = value;

                // Save the provided rate for internal use in the units (degrees per SI second) that the simulator uses.
                // SIDEREAL_SECONDS_TO_SI_SECONDS converts from sidereal seconds to SI seconds
                // Have to divide by the SIDEREAL_SECONDS_TO_SI_SECONDS conversion factor (0.99726956631945) because SI seconds are longer than sidereal seconds and hence the simulator movement will be greater in one SI second than in one sidereal second
                // ARCSECONDS_PER_RA_SECOND converts from seconds of RA (1 circle = 24 hours) to arcseconds (1 circle = 360 degrees)
                // ARCSECONDS_TO_DEGREES converts from arc seconds to degrees
                rateRaDecOffsetInternal.X = (value / SIDEREAL_SECONDS_TO_SI_SECONDS) * ARCSECONDS_PER_RA_SECOND * ARCSECONDS_TO_DEGREES;
                TL.LogMessage(LogLevel.Information, "RightAscensionRate Set", $"Value to be set (as received): {value} seconds per sidereal second. Converted to internal rate of: {value / SIDEREAL_SECONDS_TO_SI_SECONDS} seconds per SI second = {rateRaDecOffsetInternal.X} degrees per SI second.");
            }
        }

        public static double GuideRateDeclination
        {
            get { return guideRate.Y; }
            set { guideRate.Y = value; }
        }

        public static double GuideRateRightAscension
        {
            get { return guideRate.X; }
            set { guideRate.X = value; }
        }

        public static DriveRate TrackingRate { get; set; }

        public static double SlewSettleTime { get; set; }

        public static bool IsPulseGuiding
        {
            get
            {
                return (isPulseGuidingDec || isPulseGuidingRa);
            }
        }

        public static bool IsParked
        {
            get { return AtPark; }
        }

        public static PointingState SideOfPier
        {
            get
            {
                return (mountAxes.Y <= 90 && mountAxes.Y >= -90) ?
                    PointingState.Normal : PointingState.ThroughThePole;
            }
            set
            {
                // check the new side can be reached
                var pa = AstronomyFunctions.RangeAzimuth(mountAxes.X - 180);
                if (pa >= hourAngleLimit + 180 && pa < -hourAngleLimit)
                {
                    throw new InvalidOperationException("set SideOfPier " + value.ToString() + " cannot be reached at the current position");
                }

                // change the pier side
                StartSlewAxes(pa, 180 - mountAxes.Y, SlewType.SlewRaDec);
            }
        }

        public static bool IsSlewing
        {
            get
            {
                if (SlewState != SlewType.SlewNone)
                    return true;
                if (slewing)
                    return true;
                if (rateMoveAxes.LengthSquared != 0)
                    return true;
                //if (rateRaDec.LengthSquared != 0) // Commented out by Peter 4th August 2018 because the Telescope specification says that RightAscensionRate and DeclinationRate do not affect the Slewing state
                //    return true;
                return slewing && rateMoveAxes.Y != 0 && rateMoveAxes.X != 0;
            }
        }

        public static void AbortSlew()
        {
            slewing = false;
            rateMoveAxes = new Vector();
            rateRaDecOffsetInternal = new Vector();
            SlewState = SlewType.SlewNone;
        }

        public static void SyncToTarget()
        {
            mountAxes = MountFunctions.ConvertRaDecToAxes(targetRaDec, true);
            UpdatePositions();
        }

        public static void SyncToAltAzm(double targetAzimuth, double targetAltitude)
        {
            mountAxes = MountFunctions.ConvertAltAzmToAxes(new Vector(targetAzimuth, targetAltitude));
            UpdatePositions();
        }

        public static void StartSlewRaDec(double rightAscension, double declination, bool doSideOfPier)
        {
            Vector raDec = new Vector(rightAscension, declination);
            targetAxes = MountFunctions.ConvertRaDecToAxes(raDec);

            StartSlewAxes(targetAxes, SlewType.SlewRaDec);
            LogMessage("StartSlewRaDec", "Ra {0}, dec {1}, doSOP {2}", rightAscension, declination, doSideOfPier);
        }

        public static void StartSlewAltAz(double altitude, double azimuth)
        {
            LogMessage("StartSlewAltAz", "{0}, {1}", altitude, azimuth);
            StartSlewAltAz(new Vector(azimuth, altitude));
            return;
        }

        public static void StartSlewAltAz(Vector targetAltAzm)
        {
            LogMessage("StartSlewAltAz", "Azm {0}, Alt {1}", targetAltAzm.X, targetAltAzm.Y);

            Vector target = MountFunctions.ConvertAltAzmToAxes(targetAltAzm);
            if (target.LengthSquared > 0)
            {
                StartSlewAxes(target, SlewType.SlewAltAz);
            }
        }

        public static void StartSlewAxes(double primaryAxis, double secondaryAxis, SlewType slewState)
        {
            StartSlewAxes(new Vector(primaryAxis, secondaryAxis), slewState);
        }

        /// <summary>
        /// Starts a slew to the target position in mount axis degrees.
        /// </summary>
        /// <param name="targetPosition">The position.</param>
        public static void StartSlewAxes(Vector targetPosition, SlewType slewState)
        {
            targetAxes = targetPosition;
            SlewState = slewState;
            slewing = true;
            ChangePark(false);
        }

        public static void Park()
        {
            Vector parkCoordinates;

            parkCoordinates = MountFunctions.ConvertAltAzmToAxes(parkPosition); // Convert the park position AltAz coordinates into the current axes representation
            Tracking = false;

            StartSlewAxes(parkCoordinates, SlewType.SlewPark);
        }

        public static void FindHome()
        {
            if (AtPark)
            {
                throw new ParkedException("Cannot find Home when Parked");
            }

            Tracking = false;
            LogMessage("FindHome", string.Format("HomePosition.X: {0}, HomePosition.Y: {1}", HomePosition.X.ToString(CultureInfo.InvariantCulture), HomePosition.Y.ToString(CultureInfo.InvariantCulture)));
            StartSlewAxes(MountFunctions.ConvertAltAzmToAxes(HomePosition), SlewType.SlewHome);
        }

        #endregion Telescope Implementation

        #region Helper Functions

        /// <summary>
        /// Gets the side of pier using the right ascension, assuming it depends on the
        /// hour aangle only.  Used for Destinaation side of Pier, NOT to determine the mount
        /// pointing state
        /// </summary>
        /// <param name="rightAscension">The right ascension.</param>
        /// <param name="declination">The declination.</param>
        /// <returns></returns>
        public static PointingState SideOfPierRaDec(double rightAscension, double declination)
        {
            PointingState sideOfPier;
            if (alignmentMode != AlignmentMode.GermanPolar)
            {
                return PointingState.Unknown;
            }
            else
            {
                double ha = AstronomyFunctions.HourAngle(rightAscension, longitude);
                if (ha < 0.0 && ha >= -12.0) sideOfPier = PointingState.ThroughThePole;
                else if (ha >= 0.0 && ha <= 12.0) sideOfPier = PointingState.Normal;
                else sideOfPier = PointingState.Unknown;
                LogMessage("SideOfPierRaDec", "Ra {0}, Dec {1}, Ha {2}, sop {3}", rightAscension, declination, ha, sideOfPier);

                return sideOfPier;
            }
        }

        public static void ChangePark(bool newValue)
        {
            AtPark = newValue;
        }

        public static double AvailableTimeInThisPointingState
        {
            get
            {
                if (AlignmentMode != AlignmentMode.GermanPolar)
                {
                    return double.MaxValue;
                }
                double degToLimit = mountAxes.X + hourAngleLimit + 360;
                while (degToLimit > 180) degToLimit -= 360;
                return degToLimit * 240;
            }
        }

        public static double TimeUntilPointingStateCanChange
        {
            get
            {
                if (AlignmentMode != AlignmentMode.GermanPolar)
                {
                    return double.MaxValue;
                }
                var degToLimit = mountAxes.X - hourAngleLimit + 360;
                while (degToLimit > 180) degToLimit -= 360;
                return degToLimit * 240;
            }
        }

        public static string StartUpMode
        {
            get
            {
                return startupMode;
            }
            set
            {
                startupMode = value;
                s_Profile.WriteValue("StartUpMode", value.ToString());
            }
        }

        internal static void LogMessage(string identifier, string format, params object[] args)
        {
            TL.LogInformation($"{identifier}: {string.Format(CultureInfo.InvariantCulture, format, args)}");
        }

        /// <summary>
        /// returns the mount tracking movement in hour angle during the update interval
        /// </summary>
        /// <param name="updateInterval">The update interval.</param>
        /// <returns></returns>
        private static double GetTrackingChangeInDegrees(double updateInterval)
        {
            if (!Tracking)
            {
                return 0;
            }

            double haChange = 0;
            // determine the change required as a result of tracking
            // generate the change in hour angle as a result of tracking
            switch (TrackingRate)
            {
                case DriveRate.Sidereal:
                    haChange = SIDEREAL_RATE_DEG_PER_SI_SECOND * updateInterval;     // change in degrees
                    break;
                case DriveRate.Solar:
                    haChange = SOLAR_RATE_DEG_SEC * updateInterval;     // change in degrees
                    break;
                case DriveRate.Lunar:
                    haChange = LUNAR_RATE_DEG_SEC * updateInterval;     // change in degrees
                    break;
                case DriveRate.King:
                    haChange = KING_RATE_DEG_SEC * updateInterval;     // change in degrees
                    break;
            }

            return haChange;
        }

        /// <summary>
        /// Return the axis movement as a result of any slew that's taking place
        /// </summary>
        /// <returns></returns>
        private static Vector DoSlew()
        {
            Vector change = new Vector();
            if (!slewing)
            {
                return change;
            }

            // Move towards the target position
            double delta;
            bool finished = true;

            // Check primary axis
            delta = targetAxes.X - mountAxes.X;
            while (delta < -180 || delta > 180)
            {
                if (delta < -180) delta += 360;
                if (delta > 180) delta -= 360;
            }
            int signDelta = delta < 0 ? -1 : +1;
            delta = Math.Abs(delta);

            if (delta < slewSpeedSlow)
            {
                change.X = delta * signDelta;
            }
            else if (delta < slewSpeedMedium * 2)
            {
                change.X = slewSpeedSlow * signDelta;
                finished = false;
            }
            else if (delta < slewSpeedFast * 2)
            {
                change.X = slewSpeedMedium * signDelta;
                finished = false;
            }
            else
            {
                change.X = slewSpeedFast * signDelta;
                finished = false;
            }

            // Check secondary axis
            delta = targetAxes.Y - mountAxes.Y;
            while (delta < -180 || delta > 180)
            {
                if (delta < -180) delta += 360;
                if (delta > 180) delta -= 360;
            }
            signDelta = delta < 0 ? -1 : +1;
            delta = Math.Abs(delta);
            if (delta < slewSpeedSlow)
            {
                change.Y = delta * signDelta;
            }
            else if (delta < slewSpeedMedium * 2)
            {
                change.Y = slewSpeedSlow * signDelta;
                finished = false;
            }
            else if (delta < slewSpeedFast * 2)
            {
                change.Y = slewSpeedMedium * signDelta;
                finished = false;
            }
            else
            {
                change.Y = slewSpeedFast * signDelta;
                finished = false;
            }

            // If finsihed then complete processing
            if (finished)
            {
                slewing = false;
                switch (SlewState)
                {
                    case SlewType.SlewRaDec:
                    case SlewType.SlewAltAz:
                        SlewState = SlewType.SlewSettle;
                        settleTime = DateTime.Now + TimeSpan.FromSeconds(SlewSettleTime);
                        LogMessage("Settle", "Moved from slew to settle");
                        break;

                    case SlewType.SlewPark:
                        LogMessage("DoSlew", "Parked done");
                        SlewState = SlewType.SlewNone;
                        ChangePark(true);
                        break;

                    case SlewType.SlewHome:
                        LogMessage("DoSlew", "Home done");
                        SlewState = SlewType.SlewNone;
                        break;

                    case SlewType.SlewNone:
                        break;
                    //case SlewType.SlewSettle:
                    //    break;
                    //case SlewType.SlewMoveAxis:
                    //    break;
                    //case SlewType.SlewHandpad:
                    //    break;
                    default:
                        SlewState = SlewType.SlewNone;
                        break;
                }
            }

            return change;
        }

        /// <summary>
        /// return the change in axis values as a result of any HC button presses
        /// </summary>
        /// <returns></returns>
        private static Vector HcMoves()
        {
            Vector change = new Vector();
            if (SlewDirection == SlewDirection.SlewNone)
            {
                return change;
            }
            // handle HC button moves
            double delta = 0;
            switch (SlewSpeed)
            {
                case SlewSpeed.SlewSlow:
                    delta = slewSpeedSlow;
                    break;

                case SlewSpeed.SlewMedium:
                    delta = slewSpeedMedium;
                    break;

                case SlewSpeed.SlewFast:
                    delta = slewSpeedFast;
                    break;
            }
            // check the button states
            switch (SlewDirection)
            {
                case SlewDirection.SlewNorth:
                case SlewDirection.SlewUp:
                    change.Y = delta;
                    break;

                case SlewDirection.SlewSouth:
                case SlewDirection.SlewDown:
                    change.Y = -delta;
                    break;

                case SlewDirection.SlewEast:
                case SlewDirection.SlewLeft:
                    change.X = delta;
                    break;

                case SlewDirection.SlewWest:
                case SlewDirection.SlewRight:
                    change.X = -delta;
                    break;

                case SlewDirection.SlewNone:
                    break;
            }
            return change;
        }

        /// <summary>
        /// Return the axis change as a result of any pulse guide operation during the update interval
        /// </summary>
        /// <param name="updateInterval">The update interval.</param>
        /// <returns></returns>
        private static Vector PulseGuide(double updateInterval)
        {
            Vector change = new Vector();
            double guideTime;

            // Handle AltAz alignment differently to Polar and German polar.
            switch (alignmentMode)
            {
                case AlignmentMode.AltAz:

                    // Only run the process if we are currently pulse guiding
                    if (IsPulseGuiding)
                    {
                        // Set a flag when RA pulse guiding is complete
                        if (guideDuration.X <= 0)
                        {
                            isPulseGuidingRa = false;
                            guideDuration.X = 0.0;
                        }

                        // Set a flag when declination pulse guiding is complete
                        if (guideDuration.Y <= 0)
                        {
                            isPulseGuidingDec = false;
                            guideDuration.Y = 0.0;
                        }

                        // If pulse guiding is active on either axis undertake the calculation
                        if ((guideDuration.X > 0.0) | (guideDuration.Y > 0.0))
                        {
                            // Calculate the time during which pulse guiding was actually active in this time interval, either the whole interval or part of it
                            double guideTimeRA = guideDuration.X > updateInterval ? updateInterval : guideDuration.X;
                            double guideTimeDeclination = guideDuration.Y > updateInterval ? updateInterval : guideDuration.Y;

                            // Update the remaining time of the pulse guide interval
                            if (guideDuration.X > 0.0)
                                guideDuration.X -= updateInterval;
                            if (guideDuration.Y > 0.0)
                                guideDuration.Y -= updateInterval;

                            // Calculate the change due to any RA and declination pulse guiding in this interval
                            change = ConvertRateToAltAz(guideRate.X * guideTimeRA / updateInterval, guideRate.Y * guideTimeDeclination / updateInterval, updateInterval);
                        }
                    }
                    break;

                case AlignmentMode.Polar:
                    if (isPulseGuidingRa)
                    {
                        if (guideDuration.X <= 0)
                        {
                            isPulseGuidingRa = false;
                        }
                        else
                        {
                            // assume polar mount only
                            guideTime = guideDuration.X > updateInterval ? updateInterval : guideDuration.X;
                            guideDuration.X -= guideTime;

                            // assumes guide rate is in deg/sec
                            change.X = guideRate.X * guideTime;
                        }
                    }
                    if (isPulseGuidingDec)
                    {
                        if (guideDuration.Y <= 0)
                        {
                            isPulseGuidingDec = false;
                        }
                        else
                        {
                            guideTime = guideDuration.Y > updateInterval ? updateInterval : guideDuration.Y;
                            guideDuration.Y -= guideTime;

                            // Calculate the change in this interval allowing for inversion of declination direction when in the southern hemisphere.
                            if (SouthernHemisphere) // Invert the change to match the simulator mechanical axis scale
                            {
                                change.Y = -guideRate.Y * guideTime;
                            }
                            else // Northern hemisphere
                            {
                                change.Y = guideRate.Y * guideTime;
                            }
                        }
                    }
                    break;

                case AlignmentMode.GermanPolar:
                    if (isPulseGuidingRa)
                    {
                        if (guideDuration.X <= 0)
                        {
                            isPulseGuidingRa = false;
                        }
                        else
                        {
                            // assume polar mount only
                            guideTime = guideDuration.X > updateInterval ? updateInterval : guideDuration.X;
                            guideDuration.X -= guideTime;

                            // assumes guide rate is in deg/sec
                            change.X = guideRate.X * guideTime;
                        }
                    }
                    if (isPulseGuidingDec)
                    {
                        if (guideDuration.Y <= 0)
                        {
                            isPulseGuidingDec = false;
                        }
                        else
                        {
                            guideTime = guideDuration.Y > updateInterval ? updateInterval : guideDuration.Y;
                            guideDuration.Y -= guideTime;

                            // Calculate the change in this interval allowing for inversion of declination direction when the pointing state is through the pole.
                            if (SideOfPier == PointingState.Normal) // Normal state
                            {
                                change.Y = guideRate.Y * guideTime;
                            }
                            else // Through the pole state
                            {
                                change.Y = -guideRate.Y * guideTime;
                            }

                            // Invert the direction of the declination change when in the southern hemisphere to match the simulator mechanical axis specification
                            if (SouthernHemisphere)
                            {
                                change.Y = -change.Y;
                            }
                        }
                    }
                    break;

                default:
                    break;
            }
            return change;
        }

        /// <summary>
        /// Checks the axis limits. AltAz and Polar mounts allow continuous movement,
        /// it is set to a sensible range.
        /// GEM mounts check the hour angle limit and stop movement past it.
        /// </summary>
        /// <param name="primaryChange">The primary change.</param>
        private static void CheckAxisLimits(double primaryChange)
        {
            // check the ranges of the axes
            // primary axis must be in the range 0 to 360 for AltAz or Polar
            // and -hourAngleLimit to 180 + hourAngleLimit for german polar
            switch (alignmentMode)
            {
                case AlignmentMode.AltAz:
                    // the primary axis must be in the range 0 to 360
                    mountAxes.X = AstronomyFunctions.RangeAzimuth(mountAxes.X);
                    break;

                case AlignmentMode.GermanPolar:
                    // the primary axis needs to be in the range -180 to +180 to correspond with hour angles
                    // of -12 to 12.
                    // check if we have hit the hour angle limit
                    if ((mountAxes.X >= hourAngleLimit + 180 && primaryChange > 0) ||
                        (mountAxes.X <= -hourAngleLimit && primaryChange < 0))
                    {
                        // undo the movement when the limit is hit
                        mountAxes.X -= primaryChange;
                    }
                    break;

                case AlignmentMode.Polar:
                    // the axis needs to be in the range -180 to +180 to correspond with hour angles
                    // of -12 to 12.
                    while (mountAxes.X <= -180.0 || mountAxes.X > 180.0)
                    {
                        if (mountAxes.X <= -180.0) mountAxes.X += 360;
                        if (mountAxes.X > 180) mountAxes.X -= 360;
                    }
                    break;
            }
            // secondary must be in the range -90 to 0 to +90 for normal
            // and +90 to 180 to 270 for through the pole.
            // rotation is continuous
            while (mountAxes.Y >= 270.0 || mountAxes.Y < -90.0)
            {
                if (mountAxes.Y >= 270) mountAxes.Y -= 360.0;
                if (mountAxes.Y < -90) mountAxes.Y += 360.0;
            }
        }

        /// <summary>
        /// Convert the move rate in hour angle to a change in altitude and azimuth
        /// </summary>
        /// <param name="haChange">The ha change.</param>
        /// <returns></returns>
        private static Vector ConvertRateToAltAz(double haChange, double decChange, double timeInSecondsThisInterval)
        {
            Vector change = new Vector();

            double phi = Latitude * SharedResources.DEG_RAD; // Site latitude in radians
            double ha = (SiderealTime - currentRaDec.X) * SharedResources.HRS_RAD; // Current hour angle in radians
            double dec = currentRaDec.Y * SharedResources.DEG_RAD; // Current declination in radians
            double hadot = haChange * SharedResources.DEG_RAD; // Rate of change in HA due to tracking plus any RA offset rate in radians per second
            double decdot = decChange * SharedResources.DEG_RAD; // Rate of change in declination due to any declination rate offset in radians per second

#pragma warning disable IDE0018 // Suppress In-line variable declaration

            double a; // Azimuth (N through E, radians)
            double e; // Elevation (radians)
            double ad; // Rate of change of azimuth (radians per unit time)
            double ed; // Rate of change of elevation (radians per unit time)

#pragma warning restore IDE0018 // Enable In-line variable declaration

            // Calculate azimuth and elevation rates (radians per second)
            Tran(phi, ha, dec, hadot, decdot, out a, out e, out ad, out ed);

            double azimuthChange = ad * timeInSecondsThisInterval; // Change in azimuth this interval in radians
            double elevationChange = ed * timeInSecondsThisInterval; // Change in elevation this interval in radians

            change.X = azimuthChange * SharedResources.RAD_DEG; // Convert azimuth change in radians to degrees
            change.Y = elevationChange * SharedResources.RAD_DEG; // Convert elevation change in radians to degrees

            return change;
        }

        /// <summary>
        /// Convert [HA,Dec] position and rate into [Az,El] position and rate.
        /// </summary>
        /// <param name="phi">site latitude (radians)</param>
        /// <param name="ha"> hour angle (radians)</param>
        /// <param name="dec">declination (radians)</param>
        /// <param name="hadot">rate of change of ha (radians per unit time)</param>
        /// <param name="decdot">rate of change of declination (radians per unit time)</param>
        /// <param name="a">azimuth (N through E, radians)</param>
        /// <param name="e">elevation (radians)</param>
        /// <param name="ad">rate of change of a (radians per unit time)</param>
        /// <param name="ed">rate of change of e (radians per unit time)</param>
        /// <remarks>
        /// 1) For sidereal tracking ha should include the sidereal rate as well as any differential rate.
        /// 
        /// 2) The units of the velocity arguments are up to the caller.
        ///
        /// This revision:  2023 January 9
        ///
        /// Author P.T.Wallace.
        /// 
        /// 
        /// </remarks>
        static void Tran(double phi, double ha, double dec, double hadot, double decdot, out double a, out double e, out double ad, out double ed)
        {
            double sh, ch, sd, cd, x, y, z, w, xd, yd, zd, sp, cp, rxy2, rxy, xyp;

            /* Functions of HA and Dec. */
            sh = Math.Sin(ha);
            ch = Math.Cos(ha);
            sd = Math.Sin(dec);
            cd = Math.Cos(dec);

            /* Unit vector (right-handed) P & V. */
            x = ch * cd;
            y = -sh * cd;
            z = sd;

            w = decdot * sd;
            xd = hadot * y - w * ch;
            yd = -hadot * x + w * sh;
            zd = decdot * cd;

            /* Rotate P & V into Cartesian [2pi-Az,El]. */
            sp = Math.Sin(phi);
            cp = Math.Cos(phi);
            w = x * sp - z * cp;
            z = x * cp + z * sp;
            x = w;

            w = xd * sp - zd * cp;
            zd = xd * cp + zd * sp;
            xd = w;

            /* Vector's component in XY plane. */
            rxy2 = x * x + y * y;
            rxy = Math.Sqrt(rxy2);

            /* Position and velocity in [Az,El]. */
            xyp = x * xd + y * yd;
            if (rxy != 0.0)
            {
                a = Math.Atan2(y, -x);
                e = Math.Atan2(z, rxy);
                ad = -(x * yd - y * xd) / rxy2;
                ed = (zd * rxy2 - z * xyp) / rxy;
            }
            else
            {
                a = 0.0;
                e = (z != 0.0) ? Math.Atan2(z, rxy) : 0.0;
                ad = 0.0;
                ed = 0.0;
            }
        }

        /// <summary>
        /// Update the mount positions and state from the axis positions
        /// </summary>
        private static void UpdatePositions()
        {
            SiderealTime = AstronomyFunctions.LocalSiderealTime(Longitude);

            pointingState = mountAxes.Y <= 90 ? PointingState.Normal : PointingState.ThroughThePole;

            altAzm = MountFunctions.ConvertAxesToAltAzm(mountAxes);
            currentRaDec = MountFunctions.ConvertAxesToRaDec(mountAxes);
        }

        #endregion Helper Functions
    }
}