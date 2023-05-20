using ASCOM.Alpaca.Razor;

namespace ASCOM.Alpaca.Simulators
{
    public class AlpacaConfiguration : IAlpacaConfiguration
    {
        public bool RunInStrictAlpacaMode => ServerSettings.RunInStrictAlpacaMode;

        public bool PreventRemoteDisconnects => ServerSettings.PreventRemoteDisconnects;

        public string ServerName => ServerSettings.ServerName;

        public string Manufacturer => ServerSettings.Manufacturer;

        public string ServerVersion => ServerSettings.ServerVersion;

        public string Location => ServerSettings.Location;

        public bool AllowImageBytesDownload => ServerSettings.AllowImageBytesDownload;

        public bool AllowDiscovery => ServerSettings.AllowDiscovery;

        public int ServerPort => ServerSettings.ServerPort;

        public bool AllowRemoteAccess => ServerSettings.AllowRemoteAccess;

        public bool LocalRespondOnlyToLocalHost => ServerSettings.LocalRespondOnlyToLocalHost;
    }
}
