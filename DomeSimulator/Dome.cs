// tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Dome driver for DomeSimulator
//
// Description:	A port of the VB6 ASCOM Dome simulator to VB.Net.
// Converted and built in Visual Studio 2008.
//
//
//
// Implements:	ASCOM Dome interface version: 5.1.0
// Author:		Robert Turner <rbturner@charter.net>
//
// Edit Log:
//
// Date			Who	Version	Description
// -----------	---	-----	-------------------------------------------------------
// 22-Jun-2009	rbt	1.0.0	Initial edit, from Dome template
// --------------------------------------------------------------------------------
//
// Your driver's ID is ASCOM.Simulator.Dome
//
// The Guid attribute sets the CLSID for ASCOM.DomeWheelSimulator.Dome
// The ClassInterface/None attribute prevents an empty interface called
// _Dome from being created and used as the [default] interface
//
using ASCOM.Alpaca.Responses;
using ASCOM.Standard.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASCOM.Simulators
{
    public class Dome : IDomeV2, IDisposable, IAlpacaDevice
    {
        private const string UNIQUE_ID_PROFILE_NAME = "UniqueID";

        ILogger Logger;

        public Dome(int deviceNumber, ILogger logger, IProfile profile)
        {
            Logger = logger;

            LogMessage("New", "Log started");

            //This should be replaced by the next bit of code but is semi-unique as a default.
            string UniqueID = Name + deviceNumber.ToString();
            //Create a Unique ID if it does not exist
            try
            {
                if (!profile.ContainsKey(UNIQUE_ID_PROFILE_NAME))
                {
                    var uniqueid = Guid.NewGuid().ToString();
                    profile.WriteValue(UNIQUE_ID_PROFILE_NAME, uniqueid);
                }
                UniqueID = profile.GetValue(UNIQUE_ID_PROFILE_NAME);
            }

            catch (Exception ex)
            {
                logger.LogError($"Dome {deviceNumber} - {ex.Message}");
            }

            logger.LogInformation($"Dome {deviceNumber} - UUID of {UniqueID}");

            Configuration = new AlpacaConfiguredDevice(Name, "Dome", deviceNumber, UniqueID);


            Hardware.g_dOCProgress = 0;
            Hardware.g_dOCDelay = 0;

            Hardware.g_dOCDelay = System.Convert.ToDouble(profile.GetValue("OCDelay", "3"));
            Hardware.g_dSetPark = System.Convert.ToDouble(profile.GetValue("SetPark", "180"));
            Hardware.g_dSetHome = System.Convert.ToDouble(profile.GetValue("SetHome", "0"));
            Hardware.g_dAltRate = System.Convert.ToDouble(profile.GetValue("AltRate", "30"));
            Hardware.g_dAzRate = System.Convert.ToDouble(profile.GetValue("AzRate", "15"));
            Hardware.g_dStepSize = System.Convert.ToDouble(profile.GetValue("StepSize", "5"));
            Hardware.g_dMaxAlt = System.Convert.ToDouble(profile.GetValue("MaxAlt", "90"));
            Hardware.g_dMinAlt = System.Convert.ToDouble(profile.GetValue("MinAlt", "0"));
            Hardware.g_bStartShutterError = System.Convert.ToBoolean(profile.GetValue("StartShutterError", "False"));
            Hardware.g_bSlewingOpenClose = System.Convert.ToBoolean(profile.GetValue("SlewingOpenClose", "False"));
            Hardware.g_bStandardAtHome = !System.Convert.ToBoolean(profile.GetValue("NonFragileAtHome", "False"));
            Hardware.g_bStandardAtPark = !System.Convert.ToBoolean(profile.GetValue("NonFragileAtPark", "False"));

            // Get Can capabilities
            Hardware.g_bCanFindHome = System.Convert.ToBoolean(profile.GetValue("CanFindHome", "True"));
            Hardware.g_bCanPark = System.Convert.ToBoolean(profile.GetValue("CanPark", "True"));
            Hardware.g_bCanSetAltitude = System.Convert.ToBoolean(profile.GetValue("CanSetAltitude", "True"));
            Hardware.g_bCanSetAzimuth = System.Convert.ToBoolean(profile.GetValue("CanSetAzimuth", "True"));
            Hardware.g_bCanSetPark = System.Convert.ToBoolean(profile.GetValue("CanSetPark", "True"));
            Hardware.g_bCanSetShutter = System.Convert.ToBoolean(profile.GetValue("CanSetShutter", "True"));
            Hardware.g_bCanSyncAzimuth = System.Convert.ToBoolean(profile.GetValue("CanSyncAzimuth", "True"));

            // Get Conform test variables, these should always be set to False for correct production behaviour
            Hardware.g_bConformInvertedCanBehaviour = System.Convert.ToBoolean(profile.GetValue("InvertedCanBehaviour", "False"));
            Hardware.g_bConformReturnWrongException = System.Convert.ToBoolean(profile.GetValue("ReturnWrongException", "False"));

            // get and range dome state
            Hardware.g_dDomeAz = System.Convert.ToDouble(profile.GetValue("DomeAz", System.Convert.ToString(Hardware.INVALID_COORDINATE)));
            Hardware.g_dDomeAlt = System.Convert.ToDouble(profile.GetValue("DomeAlt", System.Convert.ToString(Hardware.INVALID_COORDINATE)));
            if (Hardware.g_dDomeAlt < Hardware.g_dMinAlt)
                Hardware.g_dDomeAlt = Hardware.g_dMinAlt;
            if (Hardware.g_dDomeAlt > Hardware.g_dMaxAlt)
                Hardware.g_dDomeAlt = Hardware.g_dMaxAlt;
            if (Hardware.g_dDomeAz < 0 | Hardware.g_dDomeAz >= 360)
                Hardware.g_dDomeAz = Hardware.g_dSetPark;
            Hardware.g_dTargetAlt = Hardware.g_dDomeAlt;
            Hardware.g_dTargetAz = Hardware.g_dDomeAz;

            if (Hardware.g_bStartShutterError)
                Hardware.g_eShutterState = ShutterState.Error;
            else
            {
                string ret = profile.GetValue("ShutterState", "1");       // ShutterClosed
                Hardware.g_eShutterState = (ShutterState)Enum.Parse(typeof(ShutterState), ret);
            }

            Hardware.g_eSlewing = Going.slewNowhere;
            Hardware.g_bAtPark = Hardware.HW_AtPark;                   // its OK to wake up parked
            if (Hardware.g_bStandardAtHome)
                Hardware.g_bAtHome = false;                   // Standard: home is set by home() method, never wake up homed!
            else
                Hardware.g_bAtHome = Hardware.HW_AtHome;// Non standard, position, OK to wake up homed

            // Hardware.g_timer.Interval = TIMER_INTERVAL * 1000

            // Hardware.g_handBox.LabelButtons()
            // Hardware.g_handBox.RefreshLeds()

            LogMessage("New", "Starting thread");

            LogMessage("New", "New completed OK");
        }

        public AlpacaConfiguredDevice Configuration
        {
            get;
            private set;
        }

        private bool disposedValue; // To detect redundant calls

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {

                }
            }
            this.disposedValue = true;
        }

        // TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
        // Protected Overrides Sub Finalize()
        // ' Do not change this code.  Put clean-up code in Dispose(ByVal disposing As Boolean) above.
        // Dispose(False)
        // MyBase.Finalize()
        // End Sub

        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code.  Put clean-up code in Dispose(ByVal disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private static void check_connected()
        {
            if (!Hardware.g_bConnected)
                throw new NotConnectedException("Dome simulator is not connected");
        }

        private static void check_Az(double Az)
        {
            if (Az == Hardware.INVALID_COORDINATE)
                throw new InvalidValueException("Azimuth", "Invalid Coordinate", "0 to 360 degrees");
            if (Az >= 360.0 | Az < 0.0)
            {
                throw new InvalidValueException("Azimuth", Az.ToString(), "0 to 360.0 degrees");
            }
        }

        public string Action(string ActionName, string ActionParameters)
        {
            throw new MethodNotImplementedException("Action");
        }

        public IList<string> SupportedActions
        {
            get
            {
                return new List<string>();
            }
        }

        public string DriverVersion
        {
            get
            {
                System.Reflection.Assembly Ass;

                Ass = System.Reflection.Assembly.GetExecutingAssembly(); // Get our own assembly and report its version number
                return Ass.GetName().Version.Major.ToString() + "." + Ass.GetName().Version.Minor.ToString();
            }
        }

        public void AbortSlew()
        {
            check_connected();
            Hardware.HW_Halt();
        }

        public double Altitude
        {
            get
            {           
                if (!Hardware.g_bCanSetAltitude)
                    throw new PropertyNotImplementedException("Altitude", false);

                check_connected();

                if (Hardware.g_eShutterState == ShutterState.Error)
                    LogMessage("Altitude", "Shutter in ErrorState");
                if (Hardware.g_eShutterState != ShutterState.Open)
                    LogMessage("Altitude","Shutter not Open");

                return Hardware.g_dDomeAlt;
            }
        }

        public bool AtHome
        {
            get
            {
                check_connected();
                return Hardware.g_bAtHome;
            }
        }

        public bool AtPark
        {
            get
            {
                check_connected();
                return Hardware.g_bAtPark;
            }
        }

        public double Azimuth
        {
            get
            {
                if (!Hardware.g_bCanSetAzimuth)
                    throw new PropertyNotImplementedException("Azimuth", false);

                check_connected();
                return Hardware.g_dDomeAz;
            }
        }

        public bool CanFindHome
        {
            get
            {
                return Hardware.g_bCanFindHome;
            }
        }

        public bool CanPark
        {
            get
            {
                return Hardware.g_bCanPark;
            }
        }

        public bool CanSetAltitude
        {
            get
            {
                return Hardware.g_bCanSetAltitude;
            }
        }

        public bool CanSetAzimuth
        {
            get
            {
                return Hardware.g_bCanSetAzimuth;
            }
        }

        public bool CanSetPark
        {
            get
            {
                return Hardware.g_bCanSetPark;
            }
        }

        public bool CanSetShutter
        {
            get
            {
                return Hardware.g_bCanSetShutter;
            }
        }

        public bool CanSlave
        {
            get
            {
                return false;
            }
        }

        public bool CanSyncAzimuth
        {
            get
            {
                return Hardware.g_bCanSyncAzimuth;
            }
        }

        public void CloseShutter()
        {
            if (Hardware.g_bConformInvertedCanBehaviour)
            {
                if (Hardware.g_bCanSetShutter)
                {
                    if (Hardware.g_bConformReturnWrongException)
                        throw new System.NotImplementedException("CloseShutter");
                    else
                        throw new MethodNotImplementedException("CloseShutter");
                }
            }
            else if (!Hardware.g_bCanSetShutter)
            {
                if (Hardware.g_bConformReturnWrongException)
                    throw new System.NotImplementedException("CloseShutter");
                else
                    throw new MethodNotImplementedException("CloseShutter");
            }

            check_connected();
            Hardware.HW_CloseShutter();
        }

        public void CommandBlind(string Command, bool Raw = false)
        {
            throw new MethodNotImplementedException("CommandBlind");
        }

        public bool CommandBool(string Command, bool Raw = false)
        {
            throw new MethodNotImplementedException("CommandBool");
        }

        public string CommandString(string Command, bool Raw = false)
        {
            throw new MethodNotImplementedException("CommandString");
        }

        public bool Connected
        {
            get
            {
                return Hardware.g_bConnected;
            }

            set
            {
                try
                {
                    LogMessage("Connected", string.Format("Connected set: {0}", value));

                    Hardware.g_bConnected = value;
                    // Hardware.g_timer.Enabled = value

                    LogMessage("Connected", "Starting thread");
                }
                catch (Exception ex)
                {
                    LogMessage("Connected Set", "Exception: " + ex.ToString());
                }
            }
        }

        public string Description
        {
            get
            {
                return Hardware.INSTRUMENT_DESCRIPTION;
            }
        }

        public string DriverInfo
        {
            get
            {
                return "ASCOM Platform 6 Dome Simulator in VB.NET";
            }
        }

        public void FindHome()
        {
            if (Hardware.g_bConformInvertedCanBehaviour)
            {
                if (Hardware.g_bCanFindHome)
                {
                    if (Hardware.g_bConformReturnWrongException)
                        throw new System.NotImplementedException("FindHome");
                    else
                        throw new MethodNotImplementedException("FindHome");
                }
            }
            else if (!Hardware.g_bCanFindHome)
            {
                if (Hardware.g_bConformReturnWrongException)
                    throw new System.NotImplementedException("FindHome");
                else
                    throw new MethodNotImplementedException("FindHome");
            }

            check_connected();
            if (!Hardware.g_bAtHome)
                Hardware.HW_FindHome();
        }

        public short InterfaceVersion
        {
            get
            {
                return 2;
            }
        }

        public string Name
        {
            get
            {
                return Hardware.INSTRUMENT_NAME;
            }
        }

        public void OpenShutter()
        {
            if (Hardware.g_bConformInvertedCanBehaviour)
            {
                if (Hardware.g_bCanSetShutter)
                {
                    if (Hardware.g_bConformReturnWrongException)
                        throw new System.NotImplementedException("OpenShutter");
                    else
                        throw new MethodNotImplementedException("OpenShutter");
                }
            }
            else if (!Hardware.g_bCanSetShutter)
            {
                if (Hardware.g_bConformReturnWrongException)
                    throw new System.NotImplementedException("OpenShutter");
                else
                    throw new MethodNotImplementedException("OpenShutter");
            }

            if (Hardware.g_bConformInvertedCanBehaviour)
            {
                if (Hardware.g_bCanSetShutter)
                    throw new MethodNotImplementedException("OpenShutter"); // Invert normal behaviour to make sure that Conform detects this as an error
            }
            else if (!Hardware.g_bCanSetShutter)
                throw new MethodNotImplementedException("OpenShutter");// Normal behaviour

            check_connected();

            if (Hardware.g_eShutterState == ShutterState.Error)
                throw new InvalidOperationException("Shutter failed to open");

            Hardware.HW_OpenShutter();
        }

        public void Park()
        {
            if (Hardware.g_bConformInvertedCanBehaviour)
            {
                if (Hardware.g_bCanPark)
                {
                    if (Hardware.g_bConformReturnWrongException)
                        throw new System.NotImplementedException("Park");
                    else
                        throw new MethodNotImplementedException("Park");
                }
            }
            else if (!Hardware.g_bCanPark)
            {
                if (Hardware.g_bConformReturnWrongException)
                    throw new System.NotImplementedException("Park");
                else
                    throw new MethodNotImplementedException("Park");
            }

            check_connected();
            if (!Hardware.g_bAtPark)
                Hardware.HW_Park();
        }

        public void SetPark()
        {
            if (Hardware.g_bConformInvertedCanBehaviour)
            {
                if (Hardware.g_bCanSetPark)
                {
                    if (Hardware.g_bConformReturnWrongException)
                        throw new System.NotImplementedException("SetPark");
                    else
                        throw new MethodNotImplementedException("SetPark");
                }
            }
            else if (!Hardware.g_bCanSetPark)
            {
                if (Hardware.g_bConformReturnWrongException)
                    throw new System.NotImplementedException("Park");
                else
                    throw new MethodNotImplementedException("Park");
            }

            check_connected();
            Hardware.g_dSetPark = Hardware.g_dDomeAz;

            if (!Hardware.g_bStandardAtPark)
            {
                Hardware.g_bAtPark = true;
            }
        }

        public void SetupDialog()
        {
        }

        public ShutterState ShutterStatus
        {
            get
            {
                if (!Hardware.g_bCanSetShutter)
                    throw new PropertyNotImplementedException("ShutterStatus", false);

                check_connected();
                return Hardware.g_eShutterState;
            }
        }

        public bool Slaved
        {
            get
            {
                return false;
            }
            set
            {
                if (value)
                    throw new PropertyNotImplementedException("Slaved", true);
            }
        }

        public bool Slewing
        {
            get
            {
                check_connected();

                return Hardware.HW_Slewing;
            }
        }

        public void SlewToAltitude(double Altitude)
        {
            if (Hardware.g_bConformInvertedCanBehaviour)
            {
                if (Hardware.g_bCanSetAltitude)
                {
                    if (Hardware.g_bConformReturnWrongException)
                        throw new System.NotImplementedException("SlewToAltitude");
                    else
                        throw new MethodNotImplementedException("SlewToAltitude");
                }
            }
            else if (!Hardware.g_bCanSetAltitude)
            {
                if (Hardware.g_bConformReturnWrongException)
                    throw new System.NotImplementedException("SlewToAltitude");
                else
                    throw new MethodNotImplementedException("SlewToAltitude");
            }

            check_connected();

            if (Hardware.g_eShutterState == ShutterState.Error)
                LogMessage("Altitude", "Shutter in ErrorState");
            if (Hardware.g_eShutterState != ShutterState.Open)
                LogMessage("Altitude", "Shutter not Open");
            if (Altitude < Hardware.g_dMinAlt | Altitude > Hardware.g_dMaxAlt)
                throw new InvalidValueException("SlewToAltitude", Altitude.ToString(), Hardware.g_dMinAlt.ToString() + " to " + Hardware.g_dMaxAlt.ToString());

            Hardware.HW_MoveShutter(Altitude);
        }

        public void SlewToAzimuth(double Azimuth)
        {
            if (Hardware.g_bConformInvertedCanBehaviour)
            {
                if (Hardware.g_bCanSetAzimuth)
                {
                    if (Hardware.g_bConformReturnWrongException)
                        throw new System.NotImplementedException("SlewToAzimuth");
                    else
                        throw new MethodNotImplementedException("SlewToAzimuth");
                }
            }
            else if (!Hardware.g_bCanSetAzimuth)
            {
                if (Hardware.g_bConformReturnWrongException)
                    throw new System.NotImplementedException("SlewToAzimuth");
                else
                    throw new MethodNotImplementedException("SlewToAzimuth");
            }

            check_connected();
            check_Az(Azimuth);
            Hardware.HW_Move(Azimuth);
        }

        public void SyncToAzimuth(double Azimuth)
        {
            if (Hardware.g_bConformInvertedCanBehaviour)
            {
                if (Hardware.g_bCanSyncAzimuth)
                {
                    if (Hardware.g_bConformReturnWrongException)
                        throw new System.NotImplementedException("SyncToAzimuth");
                    else
                        throw new MethodNotImplementedException("SyncToAzimuth");
                }
            }
            else if (!Hardware.g_bCanSyncAzimuth)
            {
                if (Hardware.g_bConformReturnWrongException)
                    throw new System.NotImplementedException("SyncToAzimuth");
                else
                    throw new MethodNotImplementedException("SyncToAzimuth");
            }

            check_connected();
            check_Az(Azimuth);
            Hardware.HW_Sync(Azimuth);
        }

        private void LogMessage(string message, string details)
        {
            Logger.LogVerbose(message + " - " + details); 
        }
    }
}