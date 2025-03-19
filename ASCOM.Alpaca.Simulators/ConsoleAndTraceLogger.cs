namespace ASCOM.Alpaca.Simulators
{
    using ASCOM.Common.Interfaces;
    using ASCOM.Tools;

    /// <summary>
    /// A logger that sends the messages to both the Console and a Trace Logger.
    /// </summary>
    internal class ConsoleAndTraceLogger : ASCOM.Common.Interfaces.ILogger
    {
        private ConsoleLogger consoleLogger = new ConsoleLogger();
        private TraceLogger traceLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleAndTraceLogger"/> class.
        /// </summary>
        /// <param name="name">The default partial file name for the Trace File.</param>
        public ConsoleAndTraceLogger(string name)
        {
            this.traceLogger = new TraceLogger(name, true);
        }

        /// <summary>
        /// Gets the current Logging Level that is in use.
        /// </summary>
        public LogLevel LoggingLevel
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the log should write to the console at all.
        /// </summary>
        internal static bool LogToConsole
        {
            get => ServerSettings.LogToConsole;
            set => ServerSettings.LogToConsole = value;
        }

        /// <summary>
        /// Logs a message at the specified level.
        /// </summary>
        /// <param name="level">The level of the message. If it is of lower severity then the LoggingLevel it will be dropped.</param>
        /// <param name="message">The message to log.</param>
        public void Log(LogLevel level, string message)
        {
            try
            {
                if (LogToConsole)
                {
                    this.consoleLogger.Log(level, message);
                }

                this.traceLogger?.Log(level, message);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Set the minimum logging level. Messages of a lower severity then the LoggingLevel will be dropped.
        /// </summary>
        /// <param name="level">The new Level to use.</param>
        public void SetMinimumLoggingLevel(LogLevel level)
        {
            this.LoggingLevel = level;
            this.consoleLogger.SetMinimumLoggingLevel(level);
            this.traceLogger.SetMinimumLoggingLevel(level);
        }
    }
}