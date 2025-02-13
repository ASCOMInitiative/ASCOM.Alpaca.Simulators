using ASCOM.Common;
using ASCOM.Common.Interfaces;
using System.Net;

namespace ASCOM.Alpaca.Simulators
{
    /// <summary>
    /// Support code and storage for an ASCOM logger.
    /// </summary>
    internal static class Logging
    {
        internal static ILogger Log
        {
            get;
            private set;
        }

        static Logging()
        {
            Log = new ConsoleAndTraceLogger(ServerSettings.LogFileName);

            Log.SetMinimumLoggingLevel(ServerSettings.LoggingLevel);
        }

        internal static void LogInformation(string message)
        {
            Logging.Log.LogInformation(message);
        }

        internal static void LogError(string message)
        {
            Logging.Log.LogError(message);
        }
    }
}