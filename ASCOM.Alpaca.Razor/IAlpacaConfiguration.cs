namespace ASCOM.Alpaca
{
    public interface IAlpacaConfiguration
    {
        public bool RunInStrictAlpacaMode
        {
            get;
        }

        public bool PreventRemoteDisconnects
        {
            get;
        }

        public string ServerName 
        { 
            get; 
        }

        public string Manufacturer 
        { 
            get; 
        }

        public string ServerVersion 
        { 
            get; 
        }

        public string Location 
        { 
            get; 
        }

        public bool AllowImageBytesDownload 
        { 
            get; 
        }
    }
}