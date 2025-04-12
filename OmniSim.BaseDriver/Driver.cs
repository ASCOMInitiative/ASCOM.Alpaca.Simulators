using ASCOM;
using ASCOM.Common;
using ASCOM.Common.DeviceInterfaces;
using ASCOM.Common.Interfaces;
using ASCOM.Simulators;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;

namespace OmniSim.BaseDriver
{
    public abstract class Driver : ASCOM.Common.DeviceInterfaces.IAscomDevice, ASCOM.Common.DeviceInterfaces.IAscomDeviceV2, ISimulation, IAlpacaDevice
    {
        #region Internal Values

        public abstract DeviceTypes DeviceType {get;}

        public int DeviceNumber
        {
            get; set;
        } = 0;

        public abstract string DeviceName { get; }

        private const string UNIQUE_ID_PROFILE_NAME = "UniqueID";

        public string UniqueID
        {
            get;
            set;
        }

        public bool IsConnecting { get; set; } = false;

        public bool IsConnected { get; set; } = false;

        public Timer ConnectTimer = new();

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            IsConnected = true;
            IsConnecting = false;
        }

        public ILogger TraceLogger; // ASCOM Trace Logger component

        public IProfile Profile; //Access to device settings

        public void CheckConnected(string message)
        {
            if (!IsConnected)
            {
                throw new NotConnectedException(message);
            }
        }

        #endregion Internal Values

        #region ISimulation

        public virtual void LoadSettings()
        {

        }

        public virtual void ResetSettings()
        {
            Profile.Clear();
            LoadSettings();
        }

        public string GetXMLProfile()
        {
            return Profile.GetProfile();
        }

        #endregion ISimulation

        public LogLevel SavedLoggingLevel
        {
            get
            {
                if (Enum.TryParse(Profile.GetValue("LoggingLevel", LogLevel.Information.ToString()), out LogLevel result))
                {
                    return result;
                }
                TraceLogger?.SetMinimumLoggingLevel(LogLevel.Information);
                return LogLevel.Information;
            }
            set
            {
                TraceLogger?.SetMinimumLoggingLevel(value);
                Profile.WriteValue("LoggingLevel", value.ToString());
            }
        }

        public Driver()
        {

        }

        public Driver(int deviceNumber, ILogger logger, IProfile profile)
        {
            DeviceNumber = deviceNumber;
            TraceLogger = logger;
            Profile = profile;

            TraceLogger?.SetMinimumLoggingLevel(SavedLoggingLevel);

            ConnectTimer.AutoReset = false;
            ConnectTimer.Interval = 1500;
            ConnectTimer.Elapsed += OnTimedEvent;

            UniqueID = Guid.NewGuid() + DeviceNumber.ToString();
            //Create a Unique ID if it does not exist
            try
            {
                if (!Profile.ContainsKey(UNIQUE_ID_PROFILE_NAME))
                {
                    var uniqueid = Guid.NewGuid().ToString();
                    Profile.WriteValue(UNIQUE_ID_PROFILE_NAME, uniqueid);
                }
                UniqueID = Profile.GetValue(UNIQUE_ID_PROFILE_NAME);
            }
            catch (Exception ex)
            {
                TraceLogger.LogError($"FilterWheel {DeviceNumber} - {ex.Message}");
            }

            TraceLogger.LogInformation($"FilterWheel {DeviceNumber} - UUID of {UniqueID}");
        }

        public void CheckSupportedInterface(short requiredLevel, string call, string message = "")
        {
            if (SafeInterfaceVersion < requiredLevel)
            {
                if (string.IsNullOrEmpty(message))
                {
                    throw new ASCOM.DriverException($"ASCOM call {call} requires Interface Version {requiredLevel} but the simulator is running at {SafeInterfaceVersion}.");
                }
                else
                {
                    throw new ASCOM.DriverException($"ASCOM call {call} requires Interface Version {requiredLevel} but the simulator is running at {SafeInterfaceVersion}. Extra information {message}");
                }
            }
        }

        #region ASCOM Methods

        public virtual bool Connecting
        {
            get
            {
                return ProcessCommand(() => IsConnecting, DeviceType, MemberNames.Connecting, "Get");
            }
        }

        public virtual List<StateValue> DeviceState => throw new ASCOM.NotImplementedException();

        public virtual bool Connected
        {
            get
            {
                return ProcessCommand(() => IsConnected, DeviceType, MemberNames.Connected, "Get");
            }
            set
            {
                ProcessCommand(() =>
                {
                    if (value == IsConnected) return; // We are already in the required state so ignore the request

                    IsConnected = value;

                }, DeviceType, MemberNames.Connected, "Set");
            }
        }

        public virtual string Description => throw new ASCOM.NotImplementedException();

        public virtual string DriverInfo => throw new ASCOM.NotImplementedException();

        public virtual string DriverVersion
        {
            get
            {
                return ProcessCommand(() =>
                {
                    Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                    return $"{version.Major}.{version.Minor}";
                }, DeviceType, MemberNames.DriverVersion, "Get"
                );
            }
        }

        public virtual short InterfaceVersion => throw new ASCOM.NotImplementedException();

        public virtual short SafeInterfaceVersion => InterfaceVersion;

        public virtual string Name => throw new ASCOM.NotImplementedException();

        public virtual IList<string> SupportedActions
        {
            get
            {
                TraceLogger.LogVerbose($"SupportedActions - Returning empty list");
                return [];
            }
        }

        public virtual string Action(string actionName, string actionParameters)
        {
            ProcessCommand(() =>
            {
                throw new ASCOM.ActionNotImplementedException(actionName);
            }, DeviceType, MemberNames.Action, "Action");
            throw new ASCOM.ActionNotImplementedException(actionName);
        }

        public virtual void CommandBlind(string command, bool raw = false)
        {
            ProcessCommand(() =>
            {
                CheckConnected("CommandBlind");
                throw new ASCOM.MethodNotImplementedException("CommandBlind");
            }, DeviceType, MemberNames.CommandBlind, "Command");
            throw new ASCOM.ActionNotImplementedException("CommandBlind");
        }

        public virtual bool CommandBool(string command, bool raw = false)
        {
            ProcessCommand(() =>
            {
                CheckConnected("CommandBool");
                throw new ASCOM.MethodNotImplementedException("CommandBool");
            }, DeviceType, MemberNames.CommandBool, "Command");
            throw new ASCOM.ActionNotImplementedException("CommandBool");
        }

        public virtual string CommandString(string command, bool raw = false)
        {
            ProcessCommand(() =>
            {
                CheckConnected("CommandString");
                throw new ASCOM.MethodNotImplementedException("CommandString");
            }, DeviceType, MemberNames.CommandString, "Command");
            throw new ASCOM.ActionNotImplementedException("CommandString");
        }

        public virtual void Connect()
        {
            ProcessCommand(() =>
            {
                if (IsConnected) return;

                IsConnecting = true;

                ConnectTimer.Start();
            }, DeviceType, MemberNames.Connect, "Called");
        }

        public virtual void Disconnect()
        {
            ProcessCommand(() =>
            {
                Connected = false;
            }, DeviceType, MemberNames.Disconnect, "Called");
        }

        public void Dispose()
        {
            ProcessCommand(() =>
            {
                Connected = false;
            }, DeviceType, MemberNames.Dispose, "Called");
        }

        #endregion ASCOM Methods

        #region LogTools
        public T ProcessCommand<T>(Func<T> Operation, DeviceTypes ASCOMDeviceType, MemberNames Name, string CommandType)
        {
            Stopwatch stopWatch = new();
            TraceLogger.LogVerbose($"{ASCOMDeviceType} - {Name} - {CommandType} - Called");
            stopWatch.Start();
            try
            {
                this.CheckSupportedInterface(DeviceCapabilities.VersionIntroduced(Name, ASCOMDeviceType), Name.ToString());
                var result = Operation.Invoke();
                TraceLogger.LogVerbose($"{ASCOMDeviceType} - {Name} - {CommandType} - Succeed or started in {stopWatch.Elapsed.TotalSeconds} seconds with result: {result}");
                return result;
            }
            catch (Exception ex)
            {
                TraceLogger.LogInformation($"{ASCOMDeviceType} - {Name} - {CommandType} - Failed in {stopWatch.Elapsed.TotalSeconds} seconds with Exception {ex.Message}");
                throw;
            }
        }

        public void ProcessCommand(Action Operation, DeviceTypes ASCOMDeviceType, MemberNames Name, string CommandType)
        {
            Stopwatch stopWatch = new();
            TraceLogger.LogVerbose($"{ASCOMDeviceType} -  {Name} {CommandType} -Called");
            stopWatch.Start();
            try
            {
                this.CheckSupportedInterface(DeviceCapabilities.VersionIntroduced(Name, ASCOMDeviceType), Name.ToString());
                Operation.Invoke();
                TraceLogger.LogVerbose($"{ASCOMDeviceType} - {Name} - {CommandType} - Succeed or started in {stopWatch.Elapsed.TotalSeconds} seconds with no result.");
            }
            catch (Exception ex)
            {
                TraceLogger.LogInformation($"{ASCOMDeviceType} - {Name} - {CommandType} -Failed in {stopWatch.Elapsed.TotalSeconds} seconds with Exception {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// No-op but part of the ASCOM Interface. Settings are set through Alpaca.
        /// </summary>
        public void SetupDialog()
        {
            this.ProcessCommand(
                () => { }, DeviceType, MemberNames.SetupDialog, "SetupDialog");
        }

        #endregion LogTools
    }
}