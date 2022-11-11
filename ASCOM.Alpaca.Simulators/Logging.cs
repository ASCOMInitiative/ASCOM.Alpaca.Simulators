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
            Log = new MultiLogger(ServerSettings.LogFileName);

            Log.SetMinimumLoggingLevel(ServerSettings.LoggingLevel);
        }

        internal static void LogInformation(string message)
        {
            Logging.Log.LogInformation(message);
        }

        internal static void LogAPICall(IPAddress remoteIpAddress, string request, uint clientID, uint clientTransactionID, uint transactionID)
        {
            Logging.Log.LogVerbose($"Transaction: {transactionID} - {remoteIpAddress} ({clientID}, {clientTransactionID}) requested {request}");
        }

        internal static void LogError(string message)
        {
            Logging.Log.LogError(message);
        }
    }
}