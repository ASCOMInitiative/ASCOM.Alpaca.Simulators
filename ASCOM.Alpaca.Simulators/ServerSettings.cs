using ASCOM.Common.DeviceInterfaces;
using ASCOM.Common.Interfaces;
using ASCOM.Tools;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ASCOM.Alpaca.Simulators
{
    internal static class ServerSettings
    {
        internal const string ServerName = "ASCOM Alpaca Simulators";
        internal const string Manufacturer = "ASCOM Initiative";
        internal static readonly int[] APIVersions = { 1 };

        internal static string ServerVersion
        {
            get
            {
                try
                {
                    return System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).ProductVersion;
                }
                catch
                {
                    return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
                }
            }
        }

        internal static string Version
        {
            get
            {
                try
                {
                    System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                    return FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;
                }
                catch
                {
                }
                return "1.0.0-Error";
            }
        }

        //Change this to be unique for your server, it is the name of the settings folder
        private const string _settingFolderName = "ASCOM-Alpaca-Simulator";


        internal static string SettingsFolderName
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return _settingFolderName;
                }
                else
                {
                    return _settingFolderName.ToLowerInvariant();
                }
            }
        }

        //Change this to be unique for your server, it is the name of the log file
        private const string _logFileName = "Alpaca-Simulator";

        internal static string LogFileName
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return _logFileName;
                }
                else
                {
                    return _logFileName.ToLowerInvariant();
                }
            }
        }

        private static string ServerFolderName
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return "Server";
                }
                else
                {
                    return "Server".ToLowerInvariant();
                }
            }
        }

        private readonly static XMLProfile Profile = new XMLProfile(SettingsFolderName, ServerFolderName);

        internal static void Reset()
        {
            try
            {
                Profile.Clear();
            }
            catch (Exception ex)
            {
                Logging.LogError(ex.Message);
            }
        }

        internal static string Location
        {
            get
            {
                return Profile.GetValue("Location", "Unknown");
            }
            set
            {
                Profile.WriteValue("Location", value.ToString());
            }
        }

        internal static bool AutoStartBrowser
        {
            get
            {
                if (bool.TryParse(Profile.GetValue("AutoStartBrowser", true.ToString()), out bool result))
                {
                    return result;
                }
                return true;
            }
            set
            {
                Profile.WriteValue("AutoStartBrowser", value.ToString());
            }
        }

        internal static ushort ServerPort
        {
            get
            {
                if (ushort.TryParse(Profile.GetValue("ServerPort", 32323.ToString()), out ushort result))
                {
                    return result;
                }
                return 32323;
            }
            set
            {
                Profile.WriteValue("ServerPort", value.ToString());
            }
        }

        internal static bool AllowRemoteAccess
        {
            get
            {
                if (bool.TryParse(Profile.GetValue("AllowRemoteAccess", true.ToString()), out bool result))
                {
                    return result;
                }
                return true;
            }
            set
            {
                Discovery.DiscoveryManager.DiscoveryResponder.AllowRemoteAccess = value;
                Profile.WriteValue("AllowRemoteAccess", value.ToString());
            }
        }

        internal static bool AllowDiscovery
        {
            get
            {
                if (bool.TryParse(Profile.GetValue("AllowDiscovery", true.ToString()), out bool result))
                {
                    return result;
                }
                return true;
            }
            set
            {
                Profile.WriteValue("AllowDiscovery", value.ToString());

                if (value)
                {
                    if (!Discovery.DiscoveryManager.IsRunning)
                    {
                        Discovery.DiscoveryManager.Start(ServerPort, LocalRespondOnlyToLocalHost, true);
                    }
                }
                else
                {
                    if (Discovery.DiscoveryManager.IsRunning)
                    {
                        Discovery.DiscoveryManager.Stop();
                    }
                }
            }
        }

        internal static ushort DiscoveryPort
        {
            get
            {
                if (ushort.TryParse(Profile.GetValue("DiscoveryPort", 32227.ToString()), out ushort result))
                {
                    return result;
                }
                return 32227;
            }
            set
            {
                Profile.WriteValue("DiscoveryPort", value.ToString());
            }
        }

        internal static bool LocalRespondOnlyToLocalHost
        {
            get
            {
                if (bool.TryParse(Profile.GetValue("LocalRespondOnlyToLocalHost", true.ToString()), out bool result))
                {
                    return result;
                }
                return true;
            }
            set
            {
                Discovery.DiscoveryManager.DiscoveryResponder.LocalRespondOnlyToLocalHost = value;
                Profile.WriteValue("LocalRespondOnlyToLocalHost", value.ToString());
            }
        }

        internal static bool PreventRemoteDisconnects
        {
            get
            {
                if (bool.TryParse(Profile.GetValue("PreventRemoteDisconnects", false.ToString()), out bool result))
                {
                    return result;
                }
                return false;
            }
            set
            {
                Discovery.DiscoveryManager.DiscoveryResponder.LocalRespondOnlyToLocalHost = value;
                Profile.WriteValue("PreventRemoteDisconnects", value.ToString());
            }
        }

        internal static bool UseAuth
        {
            get
            {
                if (bool.TryParse(Profile.GetValue("UseAuth", false.ToString()), out bool result))
                {
                    return result;
                }
                return false;
            }
            set
            {
                Profile.WriteValue("UseAuth", value.ToString());
            }
        }

        internal static string UserName
        {
            get
            {
                return Profile.GetValue("UserName", "User");
            }
            set
            {
                Profile.WriteValue("UserName", value.ToString());
            }
        }

        internal static string Password
        {
            get
            {
                return Profile.GetValue("Password");
            }
            set
            {
                Profile.WriteValue("Password", Hash.GetStoragePassword(value));
            }
        }

        internal static LogLevel LoggingLevel
        {
            get
            {
                if (Enum.TryParse(Profile.GetValue("LoggingLevel", LogLevel.Information.ToString()), out LogLevel result))
                {
                    return result;
                }
                return LogLevel.Information;
            }
            set
            {
                Logging.Log.SetMinimumLoggingLevel(value);
                Profile.WriteValue("LoggingLevel", value.ToString());
            }
        }

        internal static bool LogToConsole
        {
            get
            {
                if (bool.TryParse(Profile.GetValue("LogToConsole", false.ToString()), out bool result))
                {
                    return result;
                }
                return true;
            }
            set
            {
                Profile.WriteValue("LogToConsole", value.ToString());
            }
        }

        internal static bool RunSwagger
        {
            get
            {
                if (bool.TryParse(Profile.GetValue("RunSwagger", true.ToString()), out bool result))
                {
                    return result;
                }
                return true;
            }
            set
            {
                Profile.WriteValue("RunSwagger", value.ToString());
            }
        }

        internal static bool AllowImageBytesDownload
        {
            get
            {
                if (bool.TryParse(Profile.GetValue("CanImageBytesDownload", true.ToString()), out bool result))
                {
                    return result;
                }
                return true;
            }
            set
            {
                Profile.WriteValue("CanImageBytesDownload", value.ToString());
            }
        }

        internal static bool RequireStrictURLCase
        {
            get
            {
                if (bool.TryParse(Profile.GetValue("RequireStrictURLCase", true.ToString()), out bool result))
                {
                    return result;
                }
                return true;
            }
            set
            {
                Profile.WriteValue("RequireStrictURLCase", value.ToString());
            }
        }

        #region SSL Settings
        internal static bool UseSSL
        {
            get
            {
                if (bool.TryParse(Profile.GetValue("UseSSL", false.ToString()), out bool result))
                {
                    return result;
                }
                return true;
            }
            set
            {
                Profile.WriteValue("UseSSL", value.ToString());
            }
        }

        internal static string SSLCertPath
        {
            get
            {
                var path = System.IO.Path.Combine(XMLProfile.AlpacaDataPath, SettingsFolderName, "test.pfx");
                return Profile.GetValue("SSLCertPath", path);
            }
            set
            {
                Profile.WriteValue("SSLCertPath", value.ToString());
            }
        }

        internal static string SSLCertPassword
        {
            get;
            set;
        } = "1234567";

        internal static ushort SSLPort
        {
            get
            {
                if(ServerPort == ushort.MaxValue)
                {
                    return (ushort)(ServerPort - 1);
                }
                else
                {
                    return (ushort)(ServerPort + 1);
                }
            }
        }

        #endregion
    }
}