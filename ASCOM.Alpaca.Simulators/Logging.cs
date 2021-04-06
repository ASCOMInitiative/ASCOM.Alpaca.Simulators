using ASCOM.Standard.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ASCOM.Alpaca.Simulators
{
    internal static class Logging
    {
        internal static ILogger Log
        {
            get;
            private set;
        }

        static Logging()
        {
            Log = new ASCOM.Standard.Utilities.TraceLogger(ServerSettings.ServerFileName, true);

            Log.SetMinimumLoggingLevel(ServerSettings.LoggingLevel);

            //Set platform logging 
            //In this case the platform uses the same logger as the driver.
            ASCOM.Standard.Utilities.Logger.SetLogProvider(Log);
        }

        internal static void LogInformation(string message)
        {
            ASCOM.Standard.Utilities.Logger.LogInformation(message);
        }

        internal static void LogAPICall(IPAddress remoteIpAddress, string request, uint clientID, uint clientTransactionID, uint transactionID)
        {
            Log.LogVerbose($"Transaction: {transactionID} - {remoteIpAddress} ({clientID}, {clientTransactionID}) requested {request}");
        }

        internal static void LogAPICall(IPAddress remoteIpAddress, string request, uint clientID, uint clientTransactionID, uint transactionID, string payload)
        {
            Log.LogVerbose($"Transaction: {transactionID} - {remoteIpAddress} ({clientID}, {clientTransactionID}) requested {request} with payload {payload}");
        }

        internal static void LogError(string message)
        {
            ASCOM.Standard.Utilities.Logger.LogError(message);
        }
    }
}
