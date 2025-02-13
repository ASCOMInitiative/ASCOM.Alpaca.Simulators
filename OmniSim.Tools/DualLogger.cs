using ASCOM.Common.Interfaces;
using ASCOM.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniSim.Tools
{
    public class DualLogger : ILogger
    {
        private readonly TraceLogger traceLogger;

        private readonly ILogger secondLogger;

        /// <summary>
        /// Creates a Trace Logger and logs to it. Also copies messages to the provided second logger, which is the OmniSim log that contains all messages.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="secondLogger"></param>
        public DualLogger(string path, ILogger secondLogger)
        {
            this.traceLogger = new TraceLogger(path, true);
            this.secondLogger = secondLogger;
        }

        public LogLevel LoggingLevel { get; private set; }

        public void Log(LogLevel level, string message)
        {
            try
            {
                traceLogger?.Log(level, message);
            }
            catch
            {
            }

            try
            {
                secondLogger?.Log(level, message);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Sets the logging level of the unique per driver log, the second log is managed by the OmniSim primary log
        /// </summary>
        /// <param name="level"></param>
        public void SetMinimumLoggingLevel(LogLevel level)
        {
            LoggingLevel = level;
            traceLogger.SetMinimumLoggingLevel(level);
        }
    }
}
