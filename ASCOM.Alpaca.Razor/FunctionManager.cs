namespace ASCOM.Alpaca.Razor
{
    public static class FunctionManager
    {
        public static bool RunInStrictAlpacaMode
        {
            get;
            set;
        } = false;

        public static bool PreventRemoteDisconnects
        {
            get;
            set;
        } = true;
        public static string ServerName { get; set; } = "Test";
        public static string Manufacturer { get; set; } = "Test";
        public static string ServerVersion { get; set; } = "Test";
        public static string Location { get; set; } = "Test";
        public static bool AllowImageBytesDownload { get; internal set; } = true;
    }
}