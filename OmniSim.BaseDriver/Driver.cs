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

        public short Platform7DriverInterfaceVersion { get; private set; }

        public short ActionDriverInterfaceVersion { get; private set; }

        public int DeviceNumber
        {
            get; set;
        } = 0;

        public string DeviceName { get; private set; }

        private const string UNIQUE_ID_PROFILE_NAME = "UniqueID";

        public string UniqueID
        {
            get;
            set;
        }

        public bool IsConnected { get; set; } = false;

        internal Timer ConnectTimer = new();

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            IsConnected = true;
            Connecting = false;
        }

        public ILogger TraceLogger; // ASCOM Trace Logger component

        internal IProfile Profile; //Access to device settings

        public void CheckConnected(string message)
        {
            if (!IsConnected)
            {
                throw new NotConnectedException(message);
            }
        }

        #endregion Internal Values

        #region ISimulation

        public void ResetSettings()
        {
            Profile.Clear();
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

        public Driver(int deviceNumber, ILogger logger, IProfile profile, string safeName, short platform7DriverInterfaceVersion, short actionDriverInterfaceVersion)
        {
            DeviceName = safeName;
            DeviceNumber = deviceNumber;
            TraceLogger = logger;
            Profile = profile;

            Platform7DriverInterfaceVersion = platform7DriverInterfaceVersion;
            ActionDriverInterfaceVersion = actionDriverInterfaceVersion;

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

        public bool Connecting { get; set; } = false;

        public virtual List<StateValue> DeviceState => throw new ASCOM.NotImplementedException();

        public bool Connected
        {
            get
            {
                return ProcessCommand(() => IsConnected, "Connected", "Get", 1);
            }
            set
            {
                ProcessCommand(() =>
                {
                    if (value == IsConnected) return; // We are already in the required state so ignore the request

                    if (value)
                    {
                        IsConnected = true;
                    }
                    else
                    {
                        IsConnected = false;
                    }
                }, "Connected", "Set", 1);
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
                },
                "DriverVersion",
                "Get",
                ActionDriverInterfaceVersion
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
            }, "Action", "Action", ActionDriverInterfaceVersion);
            throw new ASCOM.ActionNotImplementedException(actionName);
        }

        public virtual void CommandBlind(string command, bool raw = false)
        {
            ProcessCommand(() =>
            {
                CheckConnected("CommandBlind");
                throw new ASCOM.MethodNotImplementedException("CommandBlind");
            }, "Command", "CommandBlind", ActionDriverInterfaceVersion);
            throw new ASCOM.ActionNotImplementedException("CommandBlind");
        }

        public virtual bool CommandBool(string command, bool raw = false)
        {
            ProcessCommand(() =>
            {
                CheckConnected("CommandBool");
                throw new ASCOM.MethodNotImplementedException("CommandBool");
            }, "Command", "CommandBool", ActionDriverInterfaceVersion);
            throw new ASCOM.ActionNotImplementedException("CommandBool");
        }

        public virtual string CommandString(string command, bool raw = false)
        {
            ProcessCommand(() =>
            {
                CheckConnected("CommandString");
                throw new ASCOM.MethodNotImplementedException("CommandString");
            }, "Command", "CommandString", ActionDriverInterfaceVersion);
            throw new ASCOM.ActionNotImplementedException("CommandString");
        }

        public void Connect()
        {
            ProcessCommand(() =>
            {
                if (IsConnected) return;

                Connecting = true;

                ConnectTimer.Start();
            }, "Connect", "Start", Platform7DriverInterfaceVersion);
        }

        public void Disconnect()
        {
            ProcessCommand(() =>
            {
                Connected = false;
            }, "Disconnect", "Call", Platform7DriverInterfaceVersion);
        }

        public void Dispose()
        {
            ProcessCommand(() =>
            {
                Connected = false;
            }, "Dispose", "Call", ActionDriverInterfaceVersion);
        }

        #endregion ASCOM Methods

        #region LogTools

        public T ProcessCommand<T>(Func<T> Operation, string Command, string Type, short RequiredInterfaceVersion)
        {
            Stopwatch stopWatch = new();
            TraceLogger.LogVerbose($"{Command} - {Type} {Command}");
            stopWatch.Start();
            try
            {
                this.CheckSupportedInterface(RequiredInterfaceVersion, Command);
                var result = Operation.Invoke();
                TraceLogger.LogVerbose($"{Command} - {Type} {Command} - Succeed or started in {stopWatch.Elapsed.TotalSeconds} seconds with result: {result}");
                return result;
            }
            catch (Exception ex)
            {
                TraceLogger.LogInformation($"{Command} - {Type} {Command} - Failed in {stopWatch.Elapsed.TotalSeconds} seconds with Exception {ex.Message}");
                throw;
            }
        }

        public void ProcessCommand(Action Operation, string Command, string Type, short RequiredInterfaceVersion)
        {
            Stopwatch stopWatch = new();
            TraceLogger.LogVerbose($"{Command} - {Type} {Command}");
            stopWatch.Start();
            try
            {
                this.CheckSupportedInterface(RequiredInterfaceVersion, Command);
                Operation.Invoke();
                TraceLogger.LogVerbose($"{Command} - {Type} {Command} - Succeed or started in {stopWatch.Elapsed.TotalSeconds} seconds with no result.");
            }
            catch (Exception ex)
            {
                TraceLogger.LogInformation($"{Command} - {Type} {Command} - Failed in {stopWatch.Elapsed.TotalSeconds} seconds with Exception {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// No-op but part of the ASCOM Interface. Settings are set through Alpaca.
        /// </summary>
        public void SetupDialog()
        {
            this.ProcessCommand(
                () => { },
                "SetupDialog",
                "SetupDialog",
                1);
        }

        #endregion LogTools
    }
}