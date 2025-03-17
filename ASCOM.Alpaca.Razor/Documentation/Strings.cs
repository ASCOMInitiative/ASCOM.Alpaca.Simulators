namespace ASCOM.Alpaca
{
    public class Strings
    {
        public const string ClientTransactionIDDescription = "Client's transaction ID. (1 to 4294967295). The client should start this count at 1 and increment by one on each successive transaction. This will aid associating entries in device logs with corresponding entries in client side logs.";
        public const string ServerTransactionIDDescription = "Server's transaction ID (1 to 4294967295), should be unique for each client transaction so that log messages on the client can be associated with logs on the device.";
        public const string DeviceIDDescription = @"Zero based device number as set on the server (A uint32 with a range of 0 to 4294967295)";
        public const string ClientIDDescription = @"Client's unique ID. (A uint32 with a range of 1 to 4294967295). The client should choose a value at start-up, e.g. a random value between 1 and 65535, and send this value on every transaction to help associate entries in device logs with this particular client.";

        public const string URLCapitalizationDescription = @"The API URL is required to be lowercase. Supplied URL: ";
        public const string FormCapitalizationDescription = @"The API Form is required to match the Alpaca Specification. Bad Form Key Capitalization(s): ";
        public const string QueryCapitalizationDescription = @"The API Query is required to match the Alpaca Specification. Unknown Query Key(s): ";
        public const string FormWithQueryDescription = @"A Form request should not have any Query Keys. Unknown Query Key(s): ";
        public const string QueryWithFormDescription = @"A Query request should not have any Form Keys. Unknown Form Key(s): ";
    }
}