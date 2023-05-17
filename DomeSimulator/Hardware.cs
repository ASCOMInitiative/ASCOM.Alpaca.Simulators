// ==============
// Hardware.vb
// ==============
//
// Dome hardware abstraction layer.  Same interfaces can be used for real dome.
//
// Written:  15-Jun-03   Jon Brewster
//
// Edits:
//
// When      Who     What
// --------- ---     -----------------------------------------------------------
// 15-Jun-03 jab     Initial edit
// 27-Jun-03 jab     Initial release
// 03-Sep-03 jab     Additional checks for home/park positions
// 31-Jan-04 jab     Treat home/park as state, not position
// 03-Dec-04 rbd     Add "Start up with shutter error" mode, dome azimuth,
// altitude, and shutter state are now persistent.
// 06-Dec-04 rbd     4.0.2 - More non-standard behavior - AtHome/AtPark by
// position, Slewing = True while opening/closing shutter.
// Move slewing detection logic into HW layer. New props
// HW_AtPark and HW_AtHome take into account the step size
// instead of requiring position to be exactly the set pos.
// 23-Jun-09 rbt     Port to Visual Basic .NET
// -----------------------------------------------------------------------------
using ASCOM.Common.DeviceInterfaces;
using System;
using System.Runtime.CompilerServices;
using System.Timers;

[assembly: InternalsVisibleTo("ASCOM.Alpaca.Simulators")]

namespace ASCOM.Simulators
{
    public enum Going
    {
        slewCCW = -1        // just running till halt
,
        slewNowhere = 0     // stopped, complete. not slewing
,
        slewCW = 1          // just running till halt
,
        slewSomewhere = 2   // specific Az based slew
,
        slewPark = 3        // parking
,
        slewHome = 4        // going home
    }

    internal static class Hardware
    {
        private const int vbObjectError = -2147221504;

        public static string ERR_SOURCE = "ASCOM Dome Simulator .NET";

        public static long SCODE_NOT_IMPLEMENTED = vbObjectError + 0x400;
        public static string MSG_NOT_IMPLEMENTED = " is not implemented by this dome driver object.";
        public static long SCODE_DLL_LOADFAIL = vbObjectError + 0x401;

        // Error message for above generated at run time
        public static long SCODE_NOT_CONNECTED = vbObjectError + 0x402;

        public static string MSG_NOT_CONNECTED = "The dome is not connected";
        public static long SCODE_PROP_NOT_SET = vbObjectError + 0x403;
        public static string MSG_PROP_NOT_SET = "This property has not yet been set";
        public static long SCODE_NO_TARGET_COORDS = vbObjectError + 0x404;
        public static string MSG_NO_TARGET_COORDS = "Target coordinates have not yet been set";
        public static long SCODE_VAL_OUTOFRANGE = vbObjectError + 0x405;
        public static string MSG_VAL_OUTOFRANGE = "The property value is out of range";
        public static long SCODE_SHUTTER_NOT_OPEN = vbObjectError + 0x406;
        public static string MSG_SHUTTER_NOT_OPEN = "The shutter is not open";
        public static long SCODE_SHUTTER_ERROR = vbObjectError + 0x406;
        public static string MSG_SHUTTER_ERROR = "The shutter is in an unknown state, close it.";
        public static long SCODE_START_CONFLICT = vbObjectError + 0x407;
        public static string MSG_START_CONFLICT = "Failed: Attempt to create Dome during manual startup";

        // Constants
        public static string ALERT_TITLE = "ASCOM Dome Simulator .NET";

        public static string INSTRUMENT_NAME = "Alpaca Dome Simulator";
        public static string INSTRUMENT_DESCRIPTION = "ASCOM Dome Simulator .NET";

        public static double INVALID_COORDINATE = -100000.0;

        public static double TIMER_INTERVAL = 0.25;        // seconds per tick
        public static double PARK_HOME_TOL = 1.0;           // Tolerance (deg) for Park/Home position

        // ASCOM Identifiers
        public static string ID = "ASCOM.Simulator.Dome";

        // State Variables
        public static double g_dAltRate;                 // degrees per sec

        public static double g_dAzRate;                  // degrees per sec
        public static double g_dStepSize;                // degrees per GUI step
        public static double g_dDomeAlt;                 // Current Alt for Dome
        public static double g_dDomeAz;                  // Current Az for Dome
        public static double g_dMinAlt;                  // degrees altitude limit
        public static double g_dMaxAlt;                  // degrees altitude limit

        // Non-standard behaviors
        public static bool g_bStartShutterError;      // Start up in "shutter error" condition

        public static bool g_bStandardAtHome;         // False (non-std) means AtHome true whenever az = home
        public static bool g_bStandardAtPark;         // False (non-std) means AtPark true whenever az = home
        public static bool g_bSlewingOpenClose;       // Slewing true when shutter opening/closing

        public static double g_dSetPark;                 // Park position
        public static double g_dSetHome;                 // Home position
        public static double g_dTargetAlt;               // Target Alt
        public static double g_dTargetAz;                // Target Az
        public static double g_dOCDelay;                 // Target Az
        public static double g_dOCProgress;              // Target Az

        public static bool g_bConnected;              // Whether dome is connected
        public static bool g_bAtHome;                 // Home state
        public static bool g_bAtPark;                 // Park state
        public static ShutterState g_eShutterState;      // shutter status
        public static Going g_eSlewing;                  // Move in progress

        // Dome Capabilities
        public static bool g_bCanFindHome;

        public static bool g_bCanPark;
        public static bool g_bCanSetAltitude;
        public static bool g_bCanSetAzimuth;
        public static bool g_bCanSetPark;
        public static bool g_bCanSetShutter;
        public static bool g_bCanSyncAzimuth;

        // Conform test variables, correct behaviour requires these variables to be set False
        // When set True, various "bad" behaviours are exhibited to confirm that Conform detects them correctly
        public static bool g_bConformInvertedCanBehaviour;

        public static bool g_bConformReturnWrongException;

        internal static System.Threading.Thread handboxThread;

        // Driver ID and descriptive string that shows in the Chooser
        public static string g_csDriverID = "ASCOM.Simulator.Dome";

        public static string g_csDriverDescription = "Dome Simulator .NET";

        // ---------
        // UTILITIES
        // ---------

        // range the azimuth parameter, full floating point (cannot use Mod)
        public static double AzScale(double Az)
        {
            while (Az < 0.0)
                Az = Az + 360.0;
            while (Az >= 360.0)
                Az = Az - 360.0;

            return Az;
        }

        private static Timer timer = new Timer(100)
        {
            AutoReset = true,
        };

        static Hardware()
        {
            timer.Elapsed += OnTimedEvent;
            timer.Start();
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            double slew;
            double distance;

            // Azimuth slew simulation
            if (g_eSlewing != Going.slewNowhere)
            {
                slew = g_dAzRate * TIMER_INTERVAL;
                if (g_eSlewing > Going.slewCW)
                {
                    distance = g_dTargetAz - g_dDomeAz;
                    if (distance < 0)
                        slew = -slew;
                    if (distance > 180)
                        slew = -slew;
                    if (distance < -180)
                        slew = -slew;
                }
                else
                {
                    distance = slew * 2;
                    slew = slew * (int)g_eSlewing;
                }

                // Are we there yet ?
                if (System.Math.Abs(distance) < System.Math.Abs(slew))
                {
                    g_dDomeAz = g_dTargetAz;

                    // Handle standard (fragile) and non-standard park/home changes
                    if (g_bStandardAtHome)
                    {
                        if (g_eSlewing == Going.slewHome)
                            g_bAtHome = true; // Fragile (standard)
                    }
                    else
                        g_bAtHome = HW_AtHome;// Position (non-standard)

                    if (g_bStandardAtPark)
                    {
                        if (g_eSlewing == Going.slewPark)
                            g_bAtPark = true; // Fragile (standard)
                    }
                    else
                        g_bAtPark = HW_AtPark;// Position (non-standard)

                    g_eSlewing = Going.slewNowhere;
                }
                else
                    g_dDomeAz = AzScale(g_dDomeAz + slew);
            }

            // shutter altitude control simulation
            if ((g_dDomeAlt != g_dTargetAlt))
            {
                slew = g_dAltRate * TIMER_INTERVAL;
                distance = g_dTargetAlt - g_dDomeAlt;
                if (distance < 0)
                    slew = -slew;

                // Are we there yet ?
                if (System.Math.Abs(distance) < System.Math.Abs(slew))
                {
                    g_dDomeAlt = g_dTargetAlt;
                }
                else
                    g_dDomeAlt = g_dDomeAlt + slew;
            }

            // shutter open/close simulation
            if (g_dOCProgress > 0)
            {
                g_dOCProgress = g_dOCProgress - TIMER_INTERVAL;
                if (g_dOCProgress <= 0)
                {
                    if (g_eShutterState == ShutterState.Opening)
                        g_eShutterState = ShutterState.Open;
                    else
                        g_eShutterState = ShutterState.Closed;
                }
            }

            /*if (g_dDomeAz == INVALID_COORDINATE)
                this.txtDomeAz.Text = "---.-";
            else
                this.txtDomeAz.Text = Format(AzScale(g_dDomeAz), "000.0");
            // Shutter = g_dDomeAlt
            if (g_dDomeAlt == INVALID_COORDINATE | !g_bCanSetShutter)
                this.txtShutter.Text = "----";
            else
                switch (g_eShutterState)
                {
                    case object _ when ShutterState.shutterOpen:
                        {
                            if (g_bCanSetAltitude)
                                this.txtShutter.Text = Format(g_dDomeAlt, "0.0");
                            else
                                this.txtShutter.Text = "Open";
                            break;
                        }

                    case object _ when ShutterState.shutterClosed:
                        {
                            this.txtShutter.Text = "Closed";
                            break;
                        }

                    case object _ when ShutterState.shutterOpening:
                        {
                            this.txtShutter.Text = "Opening";
                            break;
                        }

                    case object _ when ShutterState.shutterClosing:
                        {
                            this.txtShutter.Text = "Closing";
                            break;
                        }

                    case object _ when ShutterState.shutterError:
                        {
                            this.txtShutter.Text = "Error";
                            break;
                        }
                }*/
        }

        public static bool HW_Slewing
        {
            get
            {
                // Non-standard, Slewing true if shutter is opening/closing
                if (g_bSlewingOpenClose)
                {
                    return (g_eSlewing != Going.slewNowhere) | (g_dDomeAlt != g_dTargetAlt) | (g_eShutterState == ShutterState.Closing) | (g_eShutterState == ShutterState.Opening);
                }
                else
                {
                    // slewing is true if either Alt or Az are in motion
                    return (g_eSlewing != Going.slewNowhere) | (g_dDomeAlt != g_dTargetAlt);
                }
            }
        }

        //
        // Indicates if the azimuth is "close enough" to the Set Park position
        //
        public static bool HW_AtPark
        {
            get
            {
                double X;

                X = AzScale(System.Math.Abs(g_dDomeAz - g_dSetPark));
                if (X > 180)
                    X = 360 - X;
                return (X < PARK_HOME_TOL);
            }
        }

        //
        // Indicates if the azimuth is "close enough" to the Set Home position
        //
        public static bool HW_AtHome
        {
            get
            {
                double X;

                X = AzScale(System.Math.Abs(g_dDomeAz - g_dSetHome));
                if (X > 180)
                    X = 360 - X;
                return (X < PARK_HOME_TOL);
            }
        }

        public static void HW_CloseShutter()
        {
            if (g_eShutterState == ShutterState.Closed)
                return;

            g_dOCProgress = g_dOCDelay;
            g_eShutterState = ShutterState.Closing;
        }

        public static void HW_FindHome()
        {
            g_bAtHome = false;
            g_bAtPark = false;
            g_dTargetAz = g_dSetHome;
            g_eSlewing = Going.slewHome;
        }

        public static void HW_Halt()
        {
            g_dTargetAlt = g_dDomeAlt;
            g_eSlewing = Going.slewNowhere;

            // clear home / park (state is fragile in standard)
            if (g_bStandardAtPark)
                g_bAtPark = false;
            if (g_bStandardAtHome)
                g_bAtHome = false;

            // If the shutter is in motion, then cause it to jam
            if (g_dOCProgress > 0)
            {
                g_dOCProgress = 0;
                g_eShutterState = ShutterState.Error;
            }
        }

        public static void HW_Move(double Az)
        {
            g_bAtHome = false;
            g_bAtPark = false;
            g_dTargetAz = Az;
            g_eSlewing = Going.slewSomewhere;
        }

        public static void HW_MoveShutter(double Alt)
        {
            // If the shutter is opening or closing, then cause it to jam
            if (g_dOCProgress > 0)
            {
                g_dOCProgress = 0;
                g_eShutterState = ShutterState.Error;
            }
            else
                g_dTargetAlt = Alt;
        }

        public static void HW_OpenShutter()
        {
            // Ensure that the Alt stays in bounds
            if (g_dMinAlt > g_dDomeAlt)
            {
                g_dTargetAlt = g_dMinAlt;
                g_dDomeAlt = g_dMinAlt;
            }

            if (g_dMaxAlt < g_dDomeAlt)
            {
                g_dTargetAlt = g_dMaxAlt;
                g_dDomeAlt = g_dMaxAlt;
            }

            if (g_eShutterState == ShutterState.Open)
                return;

            if (g_eShutterState == ShutterState.Error)
                return;

            g_dOCProgress = g_dOCDelay;
            g_eShutterState = ShutterState.Opening;
        }

        public static void HW_Park()
        {
            g_bAtHome = false;
            g_bAtPark = false;
            g_dTargetAz = g_dSetPark;
            g_eSlewing = Going.slewPark;
        }

        public static void HW_Run(bool Dir)
        {
            g_bAtHome = false;
            g_bAtPark = false;
            g_eSlewing = Dir ? Going.slewCW : Going.slewCCW;
        }

        public static void HW_Sync(double Az)
        {
            g_eSlewing = Going.slewNowhere;
            g_dTargetAz = Az;
            g_dDomeAz = g_dTargetAz;

            // Handle standard (fragile) and non-standard park/home changes
            if (g_bStandardAtHome)
                g_bAtHome = false;                           // Fragile (standard)
            else
                g_bAtHome = HW_AtHome;// Position (non-standard)

            if (g_bStandardAtPark)
                g_bAtPark = false;                           // Fragile (standard)
            else
                g_bAtPark = HW_AtPark;// Position (non-standard)
        }
    }
}