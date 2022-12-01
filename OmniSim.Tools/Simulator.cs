using ASCOM.Simulators;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniSim.Tools
{
    public abstract class Simulator : ISimulation, IAlpacaDevice
    {
        public string DeviceName => throw new NotImplementedException();

        public int DeviceNumber => throw new NotImplementedException();

        public string UniqueID => throw new NotImplementedException();

        public string GetXMLProfile()
        {
            throw new NotImplementedException();
        }

        public void ResetSettings()
        {
            throw new NotImplementedException();
        }
    }
}
