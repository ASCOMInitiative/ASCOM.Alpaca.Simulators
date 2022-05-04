using ASCOM.Common.DeviceInterfaces;
using ASCOM.Common.Interfaces;
using ASCOM.Tools;
using System.Net;

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
            Log = new MultiLogger(ServerSettings.LogFileName);

            Log.SetMinimumLoggingLevel(ServerSettings.LoggingLevel);

            //Set platform logging
            //In this case the platform uses the same logger as the driver.
            Logger.SetLogProvider(Log);
        }

        internal static void LogInformation(string message)
        {
            Logger.LogInformation(message);
        }

        internal static void LogAPICall(IPAddress remoteIpAddress, string request, uint clientID, uint clientTransactionID, uint transactionID)
        {
            Logger.LogVerbose($"Transaction: {transactionID} - {remoteIpAddress} ({clientID}, {clientTransactionID}) requested {request}");
        }

        internal static void LogAPICall(IPAddress remoteIpAddress, string request, uint clientID, uint clientTransactionID, uint transactionID, string payload)
        {
            Logger.LogVerbose($"Transaction: {transactionID} - {remoteIpAddress} ({clientID}, {clientTransactionID}) requested {request} with payload {payload}");
        }

        internal static void LogError(string message)
        {
            Logger.LogError(message);
        }
    }
}