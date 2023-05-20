using System;

namespace ASCOM.Alpaca
{
    internal class DeviceNotFoundException : Exception
    {
        internal DeviceNotFoundException(string message) : base(message)
        {
        }
    }
}