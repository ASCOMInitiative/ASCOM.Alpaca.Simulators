namespace ASCOM.Alpaca.Simulators
{
    /// <summary>
    /// Supported log levels for ILogger devices
    /// </summary>
    public enum ConsoleDisplayOption
    {
        /// <summary>
        /// Start the console as an application window that is visible on screen.
        /// </summary>
        StartNormally,

        /// <summary>
        /// Start the console as an application window minimized to the task bar..
        /// </summary>
        StartMinimized,

        /// <summary>
        /// No visible console window.
        /// </summary>
        NoConsole,
    }
}
