using ASCOM.Common;
using ASCOM.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ASCOM.Alpaca
{
    public class Logging
    {
        static internal ILogger Log
        {
            get;
            private set;
        } = new ASCOM.Tools.ConsoleLogger();

        public static void AttachLogger(ILogger log)
        {
            Log = log;
        }


        internal static void LogError(string message)
        {
            try
            {
                Log.LogError(message);
            }
            catch
            { 
                //Log should never throw.
            }
        }

        internal static void LogVerbose(string message)
        {
            try
            {
                Log.LogVerbose(message);
            }
            catch
            {
                //Log should never throw.
            }
        }

        internal static void LogWarning(string message)
        {
            try
            {
                Log.LogWarning(message);
            }
            catch
            {
                //Log should never throw.
            }
        }

        internal static void LogAPICall(IPAddress remoteIpAddress, string request, uint clientID, uint clientTransactionID, uint transactionID)
        {
            LogVerbose($"Transaction: {transactionID} - {remoteIpAddress} ({clientID}, {clientTransactionID}) requested {request}");
        }
    }
}
