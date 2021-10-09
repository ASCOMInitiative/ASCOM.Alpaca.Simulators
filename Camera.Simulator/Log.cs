using ASCOM.Common;
using ASCOM.Common.Interfaces;

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