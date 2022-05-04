using ASCOM.Common.Interfaces;
using ASCOM.Tools;

namespace ASCOM.Alpaca.Simulators
{
    internal class MultiLogger : ASCOM.Common.Interfaces.ILogger
    {
        internal static bool LogToConsole
        {
            get => ServerSettings.LogToConsole;
            set => ServerSettings.LogToConsole = value;
        }

        private ConsoleLogger consoleLogger = new ConsoleLogger();
        private TraceLogger traceLogger;

        public LogLevel LoggingLevel 
        {
            get; 
            private set;
        }

        public void Log(LogLevel level, string message)
        {
            try
            {
                if (LogToConsole)
                {
                    consoleLogger.Log(level, message);
                }
                traceLogger?.Log(level, message);
            }
            catch
            {

            }
        }

        public void SetMinimumLoggingLevel(LogLevel level)
        {
            LoggingLevel = level;
            consoleLogger.SetMinimumLoggingLevel(level);
            traceLogger.SetMinimumLoggingLevel(level);

        }

        public MultiLogger(string path)
        {
            traceLogger = new TraceLogger(path, true); ;
        }
    }
}
