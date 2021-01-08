using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASCOM.Alpaca.Simulators
{
    internal static class DeviceManager
    {
        private readonly static Dictionary<int,ASCOM.Standard.Interfaces.ICoverCalibratorV1> coverCalibratorV1s = new Dictionary<int,ASCOM.Standard.Interfaces.ICoverCalibratorV1>();

        /*static DeviceManager()
        {
            //Only one instance
            coverCalibratorV1s.Add(0,new ASCOMSimulators.CoverCalibratorSimulator(0, Logging.Log,
                new ASCOM.Standard.Utilities.XMLProfile(ServerSettings.ServerFileName, "CoverCalibrator", 0)));
        }

        internal static void Reset()
        {
            foreach (var covcal in coverCalibratorV1s)
            {
                try
                {
                    (covcal.Value as ASCOMSimulators.CoverCalibratorSimulator)?.ResetSettings();
                }
                catch(Exception ex)
                {
                    Logging.LogError(ex.Message);
                }
            }
        }

        */

        internal static ASCOM.Standard.Interfaces.ICoverCalibratorV1 GetCoverCalibrator(int DeviceID)
        {
            /*if (coverCalibratorV1s.ContainsKey(DeviceID))
            {
                return coverCalibratorV1s[DeviceID];
            }
            else
            {*/
                throw new Exception(string.Format("Instance {0} does not exist in this server.", DeviceID));
            //}
        }

        internal static ASCOM.Standard.Interfaces.IDomeV2 GetDome(int DeviceID)
        {
            throw new Exception(string.Format("Instance {0} does not exist in this server.", DeviceID));
        }

        internal static ASCOM.Standard.Interfaces.IFilterWheelV2 GetFilterWheel(int DeviceID)
        {
            throw new Exception(string.Format("Instance {0} does not exist in this server.", DeviceID));
        }

        internal static ASCOM.Standard.Interfaces.IFocuserV3 GetFocuser(int DeviceID)
        {
            throw new Exception(string.Format("Instance {0} does not exist in this server.", DeviceID));
        }

        internal static ASCOM.Standard.Interfaces.IRotatorV3 GetRotator(int DeviceID)
        {
            throw new Exception(string.Format("Instance {0} does not exist in this server.", DeviceID));
        }

        internal static ASCOM.Standard.Interfaces.ISafetyMonitor GetSafetyMonitor(int DeviceID)
        {
            throw new Exception(string.Format("Instance {0} does not exist in this server.", DeviceID));
        }

        internal static ASCOM.Standard.Interfaces.ITelescopeV3 GetTelescope (int DeviceID)
        {
            throw new Exception(string.Format("Instance {0} does not exist in this server.", DeviceID));
        }

        internal static List<ASCOM.Standard.Interfaces.ICoverCalibratorV1> GetCoverCalibrators()
        {
            return coverCalibratorV1s.Values.ToList();
        }
    }
}
