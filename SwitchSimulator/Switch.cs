//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Switch driver for Simulator
//
// Description:	ASCOM Switch V2 Simulator.
//
// Implements:	ASCOM Switch interface version: 2
// Author:		(CDR) Chris Rowland <chris.rowland@cherryfield.me.uk>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 25-Sep-2013	CDR	6.0.0	Initial edit, created from ASCOM driver template
// --------------------------------------------------------------------------------
//

using ASCOM.Common;
using ASCOM.Common.DeviceInterfaces;
using ASCOM.Common.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Threading;

[assembly: InternalsVisibleTo("ASCOM.Alpaca.Simulators")]

namespace ASCOM.Simulators
{
    //
    // Your driver's DeviceID is ASCOM.Simulator.Switch
    //
    // The Guid attribute sets the CLSID for ASCOM.Simulator.Switch
    // The ClassInterface/None attribute prevents an empty interface called
    // _Simulator from being created and used as the [default] interface
    //

    /// <summary>
    /// ASCOM Switch Driver for Simulator.
    /// </summary>
    public class Switch : ISwitchV3, IDisposable, IAlpacaDevice, ISimulation
    {
        /// <summary>
        /// ASCOM DeviceID (COM ProgID) for this driver.
        /// The DeviceID is used by ASCOM applications to load the driver at runtime.
        /// </summary>
        internal static string driverID = "ASCOM.Simulator.Switch";

        /// <summary>
        /// Driver description that displays in the ASCOM Chooser.
        /// </summary>
        private static string driverDescription = "ASCOM SwitchV2 Simulator Driver.";

        internal static string traceStateProfileName = "Trace Level";
        internal static string traceStateDefault = "false";
        private const string EXPOSE_OCHTAG_NAME = "Expose OCH Tag";
        private const bool EXPOSE_OCHTAG_DEFAULT = true;

        // Supported actions
        private const string OCH_TAG = "OCHTag"; private const string OCH_TAG_UPPER_CASE = "OCHTAG";

        private const string OCH_TEST_POWER_REPORT = "OCHTestPowerReport"; private const string OCH_TEST_POWER_REPORT_UPPER_CASE = "OCHTESTPOWERREPORT";

        internal static bool traceState;
        private static bool exposeOCHState;

        /// <summary>
        /// Private variable to hold the connected state
        /// </summary>
        private bool connectedState;

        /// <summary>
        /// Private variable to hold the trace logger object (creates a diagnostic log file with information that you specify)
        /// </summary>
        private readonly ILogger tl;

        /// <summary>
        ///
        /// </summary>
        private readonly IProfile Profile;

        private const string UNIQUE_ID_PROFILE_NAME = "UniqueID";

        private readonly object loggerLockObject = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="Simulator"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public Switch(int deviceNumber, ILogger logger, IProfile profile)
        {
            tl = logger;
            Profile = profile;

            ReadProfile(); // Read device configuration from the ASCOM Profile store

            LogMessage($"New Switch {deviceNumber}", "Starting initialisation");

            DeviceNumber = deviceNumber;

            connectedState = false; // Initialise connected to false
                                    //TODO: Implement your additional construction here

            //This should be replaced by the next bit of code but is semi-unique as a default.
            UniqueID = Name + deviceNumber.ToString();
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
                logger.LogError($"Switch {deviceNumber} - {ex.Message}");
            }

            logger.LogInformation($"Switch {deviceNumber} - UUID of {UniqueID}");

            LogMessage("Switch", "Completed initialisation");
        }

        public string DeviceName { get => Name; }
        public int DeviceNumber { get; private set; }
        public string UniqueID { get; private set; }

        //
        // PUBLIC COM INTERFACE ISwitchV2 IMPLEMENTATION
        //

        #region Common properties and methods.

        public IList<string> SupportedActions
        {
            get
            {
                if (exposeOCHState)
                {
                    LogMessage("SupportedActions Get", string.Format("Returning {0} and {1} in the arraylist", OCH_TAG, OCH_TEST_POWER_REPORT));
                    return new List<string>() { OCH_TAG, OCH_TEST_POWER_REPORT };
                }
                else
                {
                    LogMessage("SupportedActions Get", string.Format("Returning {0} in the arraylist, not returning {1} because exposeOCHState is false", OCH_TEST_POWER_REPORT, OCH_TAG));
                    return new List<string>() { OCH_TEST_POWER_REPORT };
                }
            }
        }

        public string Action(string actionName, string actionParameters)
        {
            switch (actionName.ToUpperInvariant())
            {
                case OCH_TAG_UPPER_CASE when exposeOCHState:
                    return "SwitchSimulator";

                case OCH_TEST_POWER_REPORT_UPPER_CASE:
                    return "All observatory power systems are functioning properly. Supplied parameters: " + actionParameters;

                default:
                    throw new ASCOM.ActionNotImplementedException("Action " + actionName + " is not implemented by this driver");
            }
        }

        public void CommandBlind(string command, bool raw)
        {
            throw new ASCOM.MethodNotImplementedException("CommandBlind");
        }

        public bool CommandBool(string command, bool raw)
        {
            throw new ASCOM.MethodNotImplementedException("CommandBool");
        }

        public string CommandString(string command, bool raw)
        {
            throw new ASCOM.MethodNotImplementedException("CommandString");
        }

        public void Dispose()
        {
        }

        public bool Connected
        {
            get
            {
                LogMessage("Connected Get", IsConnected.ToString());
                return IsConnected;
            }
            set
            {
                LogMessage("Connected Set", value.ToString());
                if (value == IsConnected)
                    return;

                if (value)
                {
                    connectedState = true;
                    // TODO connect to the device
                }
                else
                {
                    connectedState = false;
                    // TODO disconnect from the device
                }
            }
        }

        public string Description
        {
            // TODO customise this device description
            get
            {
                LogMessage("Description Get", driverDescription);
                return driverDescription;
            }
        }

        public string DriverInfo
        {
            get
            {
                FileVersionInfo FV = Process.GetCurrentProcess().MainModule.FileVersionInfo; //Get the name of the executable without path or file extension
                string driverInfo = "Switch V2 Simulator, version: " + FV.FileVersion;
                LogMessage("DriverInfo Get", driverInfo);
                return driverInfo;
            }
        }

        public string DriverVersion
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                return $"{version.Major}.{version.Minor}";
            }
        }

        public short InterfaceVersion
        {
            // set by the driver wizard
            get
            {
                LogMessage("InterfaceVersion Get", "3");
                return 3;
            }
        }

        public string Name
        {
            get
            {
                string name = "Alpaca Switch Simulator";
                LogMessage("Name Get", name);
                return name;
            }
        }

#endregion Common properties and methods.

        #region ISwitchV2 Implementation

        /// <summary>
        /// list of switches used for simulation
        /// </summary>
        internal List<LocalSwitch> switches;

        /// <summary>
        /// The number of switches managed by this driver
        /// </summary>
        public short MaxSwitch
        {
            get
            {
                CheckConnected("MaxSwitch");
                LogMessage("MaxSwitch Get", switches.Count.ToString());
                return (short)switches.Count;
            }
        }

        /// <summary>
        /// Return the name of switch n
        /// </summary>
        /// <param name="id">The switch number to return</param>
        /// <returns>
        /// The name of the switch
        /// </returns>
        public string GetSwitchName(short id)
        {
            Validate("GetSwitchName", id);
            return switches[id].Name;
        }

        /// <summary>
        /// Sets a switch name to a specified value
        /// </summary>
        /// <param name="id">The number of the switch whose name is to be set</param>
        /// <param name="name">The name of the switch</param>
        public void SetSwitchName(short id, string name)
        {
            // not sure if this should be set or not
            //throw new MethodNotImplementedException("SetSwitchName");
            Validate("SetSwitchName", id);
            switches[id].Name = name;
            switches[id].Save(Profile, id);
        }

        /// <summary>
        /// Gets the description of the specified switch. This is to allow a fuller description of
        /// the switch to be returned, for example for a tool tip.
        /// </summary>
        /// <param name="id">The number of the switch whose description is to be returned</param>
        /// <returns></returns>
        /// <exception cref="T:ASCOM.MethodNotImplementedException">If the method is not implemented</exception>
        /// <exception cref="T:ASCOM.InvalidValueException">If id is outside the range 0 to MaxSwitch - 1</exception>
        public string GetSwitchDescription(short id)
        {
            Validate("GetSwitchDescription", id);
            return switches[id].Description;
        }

        public bool CanWrite(short id)
        {
            Validate("CanWrite", id);
            return switches[id].CanWrite;
        }

        #region boolean switch members

        /// <summary>
        /// Return the state of switch n
        /// </summary>
        /// <param name="id">The switch number to return</param>
        /// <returns>
        /// True or false
        /// </returns>
        public bool GetSwitch(short id)
        {
            Validate("GetSwitch", id);
            // returns true if the value is closer to the maximum than the minimum
            return switches[id].Maximum - switches[id].Value <= switches[id].Value - switches[id].Minimum;
        }

        /// <summary>
        /// Sets a switch to the specified state
        /// If the switch cannot be set then throws a MethodNotImplementedException.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        public void SetSwitch(short id, bool state)
        {
            Validate("SetSwitch", id);
            switches[id].SetValue(state ? switches[id].Maximum : switches[id].Minimum, "SetSwitch");
        }

        #endregion boolean switch members

        #region analogue switch members

        /// <summary>
        /// returns the maximum value for this switch
        /// boolean switches must return 1.0
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public double MaxSwitchValue(short id)
        {
            Validate("MaxSwitchValue", id);
            return switches[id].Maximum;
        }

        /// <summary>
        /// returns the minimum value for this switch
        /// boolean switches must return 0.0
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public double MinSwitchValue(short id)
        {
            Validate("MinSwitchValue", id);
            return switches[id].Minimum;
        }

        /// <summary>
        /// returns the step size that this switch supports. This gives the difference between
        /// successive values of the switch.
        /// The number of values is ((MaxSwitchValue - MinSwitchValue) / SwitchStep) + 1
        /// boolean switches must return 1.0, giving two states.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public double SwitchStep(short id)
        {
            Validate("SwitchStep", id);
            return switches[id].StepSize;
        }

        /// <summary>
        /// returns the analogue switch value for switch id
        /// boolean switches will return 1.0 or 0.0
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public double GetSwitchValue(short id)
        {
            Validate("GetSwitchValue", id);
            return switches[id].Value;
        }

        /// <summary>
        /// set the analogue value for this switch.
        /// If the switch cannot be set then throws a MethodNotImplementedException.
        /// If the value is not between the maximum and minimum then throws an InvalidValueException
        /// boolean switches will be set to true if the value is closer to the maximum than the minimum.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public void SetSwitchValue(short id, double value)
        {
            Validate("SetSwitchValue", id, value);
            switches[id].SetValue(value, "SetSwitchValue");
        }

        #endregion analogue switch members

        #endregion ISwitchV2 Implementation

        #region Private properties and methods

        /// <summary>
        /// Checks that we are connected and the switch id is in range and throws an InvalidValueException if it isn't
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="id">The id.</param>
        private void Validate(string message, short id)
        {
            if (id < 0 || id >= switches.Count)
            {
                LogMessage(message, string.Format("Switch {0} not available, range is 0 to {1}", id, switches.Count - 1));
                throw new InvalidValueException(message, id.ToString(), string.Format("0 to {0}", switches.Count - 1));
            }

            CheckConnected(message);

            if (NumStates(id) < 2)
            {
                LogMessage(message, string.Format("Device {0} has too few states", id));
                throw new InvalidValueException(message, switches[id].Name, "too few states");
            }
        }

        private int NumStates(short id)
        {
            var sw = switches[id];
            return (int)((sw.Maximum - sw.Minimum) / sw.StepSize) + 1;
        }

        /// <summary>
        /// Check we are connected, the switch id is valid, that the switch is a multi-value switch and that the value is in range.
        /// Throw exceptions if this is incorrect
        /// </summary>
        /// <param name="message"></param>
        /// <param name="id"></param>
        /// <param name="value"></param>
        private void Validate(string message, short id, double value)
        {
            Validate(message, id);
            var sw = switches[id];
            if (value < sw.Minimum || value > sw.Maximum)
            {
                LogMessage(message, string.Format("Switch {0} value {1} out of range {2} to {3}", id, value, sw.Minimum, sw.Maximum));
                throw new InvalidValueException(message, id.ToString(), (string.Format("{0} to {1}", sw.Minimum, sw.Maximum)));
            }
        }

        /// <summary>
        /// Returns true if there is a valid connection to the driver hardware
        /// </summary>
        private bool IsConnected
        {
            get
            {
                // TODO check that the driver hardware connection exists and is connected to the hardware
                // simulator has no hardware
                return connectedState;
            }
        }

        /// <summary>
        /// Use this function to throw an exception if we aren't connected to the hardware
        /// </summary>
        /// <param name="message"></param>
        private void CheckConnected(string message)
        {
            if (!IsConnected)
            {
                throw new ASCOM.NotConnectedException(message);
            }
        }

        /// <summary>
        /// Read the device configuration from the ASCOM Profile store
        /// </summary>
        internal void ReadProfile()
        {
            exposeOCHState = Convert.ToBoolean(Profile.GetValue(EXPOSE_OCHTAG_NAME, EXPOSE_OCHTAG_DEFAULT.ToString()));

            switches = new List<LocalSwitch>();
            int numSwitch;
            if (int.TryParse(Profile.GetValue("NumSwitches", string.Empty), out numSwitch))
            {
                for (short i = 0; i < numSwitch; i++)
                {
                    switches.Add(new LocalSwitch(Profile, i));
                }
            }
            else
            {
                LoadDefaultSwitches();
            }
        }

        /// <summary>
        /// Write the device configuration to the  ASCOM  Profile store
        /// </summary>
        internal void WriteProfile()
        {
            Profile.WriteValue("NumSwitches", switches.Count.ToString());
            int i = 0;
            foreach (var item in switches)
            {
                item.Save(Profile, i++);
            }
        }

        public void ResetSettings()
        {
            Profile.Clear();
        }

        public string GetXMLProfile()
        {
            return Profile.GetProfile();
        }

        /// <summary>
        /// Loads a default set of switches.
        /// </summary>
        private void LoadDefaultSwitches()
        {
            switches.Add(new LocalSwitch("Power1") { Description = "Asynchronous switch", CanAsync = true, Duration = 3.0 });
            switches.Add(new LocalSwitch("Power2") { Description = "Generic Power switch" });
            switches.Add(new LocalSwitch("Light Box", 100, 0, 10, 0) { Description = "Light box , 0 to 100%" });
            switches.Add(new LocalSwitch("Flat Panel", 255, 0, 1, 0) { Description = "Flat panel , 0 to 255" });
            switches.Add(new LocalSwitch("Cover State") { Description = "Scope cover switch.", CanWrite = false });
            switches.Add(new LocalSwitch("Parked") { Description = "Scope parked switch.", CanWrite = false });
            switches.Add(new LocalSwitch("Cloud Cover", 8, 0, 1, 0, false) { Description = "Cloud cover (okta)" });
            switches.Add(new LocalSwitch("Temperature", 30, -20, 0.1, 12, false) { Description = "Temperature in deg C" });
            switches.Add(new LocalSwitch("Humidity", 100, 0, 1, 50, false) { Description = "Relative humidity %" });
            switches.Add(new LocalSwitch("Raining") { Description = "Rain monitor, true if raining", CanWrite = false });
        }

        #endregion Private properties and methods

        #region ISwitchV3 implementation
        public void Connect()
        {
            Connected = true;
        }

        public void Disconnect()
        {
            Connected = false;
        }

        public bool Connecting
        {
            get
            {
                return false;
            }
        }

        public List<StateValue> DeviceState
        {
            get
            {
                List<StateValue> deviceState = new List<StateValue>();

                for (short i = 0; i < MaxSwitch; i++)
                {
                    try { deviceState.Add(new StateValue($"GetSwitch{i}", GetSwitch(i))); } catch { }
                }

                for (short i = 0; i < MaxSwitch; i++)
                {
                    try { deviceState.Add(new StateValue($"GetSwitchValue{i}", GetSwitchValue(i))); } catch { }
                }

                for (short i = 0; i < MaxSwitch; i++)
                {
                    try { deviceState.Add(new StateValue($"StateChangeComplete{i}", StateChangeComplete(i))); } catch { }
                }

                try { deviceState.Add(new StateValue(DateTime.Now)); } catch { }

                return deviceState;
            }
        }

        public void SetAsync(short id, bool state)
        {
            // Validate the id parameter
            Validate("SetAsync", id);

            // Convert the bool value to an appropriate double value representing true or false
            SetAsyncValue(id, state ? switches[id].Maximum : switches[id].Minimum);
        }

        public void SetAsyncValue(short id, double value)
        {
            // Validate the id parameter
            Validate("SetAsyncValue", id);

            // Validate the incoming state value
            Validate("SetAsyncValue", id, value);

            LogMessage("SetAsyncValue", $"Entry - cancellation token is none: {switches[id].CancellationToken == CancellationToken.None}, State change complete: {switches[id].StateChangeComplete}");

            // If an async operation is already running cancel it
            if (switches[id].CancellationToken != CancellationToken.None)
            {
                LogMessage("SetAsyncValue", $"Before cancellation - cancellation token is none: {switches[id].CancellationToken == CancellationToken.None}, State change complete: {switches[id].StateChangeComplete}");
                switches[id].CancellationTokenSource.Cancel();
                LogMessage("SetAsyncValue", $"After cancellation - cancellation token is none: {switches[id].CancellationToken == CancellationToken.None}, State change complete: {switches[id].StateChangeComplete}");

                // Wait for the task to cancel
                Stopwatch sw = Stopwatch.StartNew();
                LogMessage("SetAsyncValue", $"Started waiting for task to complete");
                try
                {
                    Task.WaitAny(switches[id].Task);
                }
                catch (Exception e)
                {
                    LogMessage("SetAsyncValue", $"Waiting for previous task to complete: {e.Message}\r\n{e}");
                }
                LogMessage("SetAsyncValue", $"Finished waiting for task to complete. Wait duration: {sw.ElapsedMilliseconds}ms.");


                //Thread.Sleep(100); // Wait for a short while for cancellation to happen
                LogMessage("SetAsyncValue", $"After wait - cancellation token is none: {switches[id].CancellationToken == CancellationToken.None}, State change complete: {switches[id].StateChangeComplete}");
            }

            // Flag that an operation is underway
            switches[id].StateChangeComplete = false;

            // Clear any async exception
            switches[id].AsyncException = null;

            // Create a cancellation token source and save it for use if CancelAsync is called for this switch
            CancellationTokenSource cts = new CancellationTokenSource();

            switches[id].CancellationTokenSource = cts;
            switches[id].CancellationToken = cts.Token;

            // Create a task to set the switch state asynchronously
            LogMessage("SetAsyncValue", $"Running task - cancellation token is none: {switches[id].CancellationToken == CancellationToken.None}, State change complete: {switches[id].StateChangeComplete}");
            switches[id].Task = Task.Run(() =>
            {
                try
                {
                    // Start the switch operation
                    Stopwatch sw = Stopwatch.StartNew();
                    LogMessage("SetAsyncValueTask", $"Starting SetValue");
                    switches[id].SetValue(value, "SetAsyncValue");
                    LogMessage("SetAsyncValueTask", $"SetValue completed in {sw.Elapsed.TotalSeconds:0.00} seconds.");
                }
                finally // Ensure that the operation complete flag is always set
                {
                    // Clear the cancellation token to show that no task is running for this switch
                    LogMessage("SetAsyncValueTask", $"Clearing cancellation token");
                    switches[id].CancellationToken = CancellationToken.None;

                    // Set the operation complete flag
                    LogMessage("SetAsyncValueTask", $"Setting state change complete.");
                    switches[id].StateChangeComplete = true;
                }
            }, switches[id].CancellationToken);

            LogMessage("SetAsyncValue", $"Exit - cancellation token is none: {switches[id].CancellationToken == CancellationToken.None}, State change complete: {switches[id].StateChangeComplete}");
        }

        public bool CanAsync(short id)
        {
            // Validate the id parameter
            Validate("CanAsync", id);

            // Return the configured CanAsync state
            return switches[id].CanAsync;
        }

        public bool StateChangeComplete(short id)
        {
            // Validate the id parameter
            Validate("StateChangeComplete", id);

            // Throw any exception from the async process (Only OperationCanelledException in this simulator)
            if (!(switches[id].AsyncException is null)) // There is an exception to throw
            {
                throw switches[id].AsyncException;
            }

            // No exception so return that operation completion state
            return switches[id].StateChangeComplete;
        }

        public void CancelAsync(short id)
        {
            LogMessage("CancelAsync", $"Cancelling operation for switch {id}");
            // Validate the id parameter
            Validate("CancelAsync", id);

            if (switches[id].CancellationToken != CancellationToken.None)
            {
                switches[id].CancellationTokenSource.Cancel();
                switches[id].AsyncException = new OperationCancelledException($"The asynchronous operation for switch {id} was cancelled.");

                // Wait for the task to cancel
                Stopwatch sw = Stopwatch.StartNew();
                LogMessage("CancelAsync", $"Started waiting for task to complete");
                try
                {
                    Task.WaitAny(switches[id].Task);
                }
                catch (Exception e)
                {
                    LogMessage("CancelAsync", $"Waiting for previous task to complete: {e.Message}\r\n{e}");
                }
                LogMessage("CancelAsync", $"Finished waiting for task to complete. Wait duration: {sw.ElapsedMilliseconds}ms.");

            }
        }
        #endregion

        private void LogMessage(string source, string details)
        {
            lock (loggerLockObject)
            {
                tl?.LogDebug(source + " - " + details);
            }
        }

    }
}