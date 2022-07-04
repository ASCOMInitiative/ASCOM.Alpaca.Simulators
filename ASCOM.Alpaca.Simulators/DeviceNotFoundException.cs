using System;

namespace ASCOM.Alpaca.Simulators
{
    internal class DeviceNotFoundException : Exception
    {
        internal DeviceNotFoundException(string message) : base(message)
        {
        }
    }
}
