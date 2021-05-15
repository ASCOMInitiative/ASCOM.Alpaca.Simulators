using ASCOM.Standard.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCOM.Simulators
{
    internal static class Log
    {
        internal static ILogger log;


        internal static void LogMessage(string identifier, string message, params object[] args)
        {

            log?.LogVerbose(identifier + " - " + string.Format(message, args));
        }

    }
}
