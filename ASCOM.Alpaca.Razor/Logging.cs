using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ASCOM.Alpaca
{
    internal class Logging
    {
        internal static void LogError(string message)
        {

        }

        internal static void LogVerbose(string v)
        {

        }

        internal static void LogWarning(string v)
        {

        }

        internal static void LogAPICall(IPAddress remoteIpAddress, string request, uint clientID, uint clientTransactionID, uint transactionID)
        {
            LogVerbose($"Transaction: {transactionID} - {remoteIpAddress} ({clientID}, {clientTransactionID}) requested {request}");
        }
    }
}
