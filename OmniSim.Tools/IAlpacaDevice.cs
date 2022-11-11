namespace ASCOM.Simulators
{
    public interface IAlpacaDevice
    {
        string DeviceName { get; }
        int DeviceNumber { get; }
        string UniqueID { get; }
    }
}