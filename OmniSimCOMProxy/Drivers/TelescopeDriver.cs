using ASCOM.DeviceInterface;
using ASCOM.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ASCOM.Simulators.LocalServer.Drivers
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("D3B92773-EF80-47F9-B01A-04E48838C130")]
    public class Telescope : BaseDriver, ASCOM.DeviceInterface.ITelescopeV4, IDisposable
    {
        ASCOM.Common.DeviceInterfaces.ITelescopeV4 Device = new ASCOM.Com.DriverAccess.Telescope("OmniSim.Telescope");
        TraceLogger TL = new TraceLogger("ComProxyTelescope", true);
        public Telescope()
        {
            base.GetDevice = () => (ASCOM.Common.DeviceInterfaces.IAscomDeviceV2)Device;
            TL.LogMessage("Telescope", "Started");
        }

        public AlignmentModes AlignmentMode => (AlignmentModes)Device.AlignmentMode;

        public double Altitude => Device.Altitude;

        public double ApertureArea => Device.ApertureArea;

        public double ApertureDiameter => Device.ApertureDiameter;

        public bool AtHome => Device.AtHome;

        public bool AtPark => Device.AtPark;

        public double Azimuth => Device.Azimuth;

        public bool CanFindHome => Device.CanFindHome;

        public bool CanPark => Device.CanPark;

        public bool CanPulseGuide => Device.CanPulseGuide;

        public bool CanSetDeclinationRate => Device.CanSetDeclinationRate;

        public bool CanSetGuideRates => Device.CanSetGuideRates;

        public bool CanSetPark => Device.CanSetPark;

        public bool CanSetPierSide => Device.CanSetPierSide;

        public bool CanSetRightAscensionRate => Device.CanSetRightAscensionRate;

        public bool CanSetTracking => Device.CanSetTracking;

        public bool CanSlew => Device.CanSlew;

        public bool CanSlewAltAz => Device.CanSlewAltAz;

        public bool CanSlewAltAzAsync => Device.CanSlewAltAzAsync;

        public bool CanSlewAsync => Device.CanSlewAsync;

        public bool CanSync => Device.CanSync;

        public bool CanSyncAltAz => Device.CanSyncAltAz;

        public bool CanUnpark => Device.CanUnpark;

        public double Declination => Device.Declination;

        public double DeclinationRate { get => Device.DeclinationRate; set => Device.DeclinationRate = value; }
        public bool DoesRefraction { get => Device.DoesRefraction; set => Device.DoesRefraction = value; }

        public EquatorialCoordinateType EquatorialSystem => (EquatorialCoordinateType)Device.EquatorialSystem;

        public double FocalLength => Device.FocalLength;

        public double GuideRateDeclination { get => Device.GuideRateDeclination; set => Device.GuideRateDeclination = value; }
        public double GuideRateRightAscension { get => Device.GuideRateRightAscension; set => Device.GuideRateRightAscension = value; }

        public bool IsPulseGuiding => Device.IsPulseGuiding;

        public double RightAscension => Device.RightAscension;

        public double RightAscensionRate { get => Device.RightAscensionRate; set => Device.RightAscensionRate = value; }
        public PierSide SideOfPier { get => (PierSide)Device.SideOfPier; set => Device.SideOfPier = (Common.DeviceInterfaces.PointingState)value; }

        public double SiderealTime => Device.SiderealTime;

        public double SiteElevation { get => Device.SiteElevation; set => Device.SiteElevation = value; }
        public double SiteLatitude { get => Device.SiteLatitude; set => Device.SiteLatitude = value; }
        public double SiteLongitude { get => Device.SiteLongitude; set => Device.SiteLongitude = value; }

        public bool Slewing => Device.Slewing;

        public short SlewSettleTime { get => Device.SlewSettleTime; set => Device.SlewSettleTime = value; }
        public double TargetDeclination { get => Device.TargetDeclination; set => Device.TargetDeclination = value; }
        public double TargetRightAscension { get => Device.TargetRightAscension; set => Device.TargetRightAscension = value; }
        public bool Tracking { get => Device.Tracking; set => Device.Tracking = value; }
        public DriveRates TrackingRate { get => (DriveRates)Device.TrackingRate; set => Device.TrackingRate = (Common.DeviceInterfaces.DriveRate)value; }

        public ITrackingRates TrackingRates
        {
            get
            {
                ASCOM.Common.DeviceInterfaces.ITrackingRates trackingRates = Device.TrackingRates;
                List<DriveRates> driveRates = new List<DriveRates>();
                foreach (ASCOM.Common.DeviceInterfaces.DriveRate rate in trackingRates)
                {
                    driveRates.Add((DriveRates)rate);
                }

                TrackingRates platformTrackingRates = new TrackingRates(driveRates);

                return platformTrackingRates;
            }
        }

        public DateTime UTCDate { get => Device.UTCDate; set => Device.UTCDate = value; }

        public void AbortSlew()
        {
            Device.AbortSlew();
        }

        public IAxisRates AxisRates(TelescopeAxes Axis)
        {
            TL.LogMessage("AxisRates", $"AxisRates entered");

            string axisRateString = Device.Action("AXISRATES", ((int)TelescopeAxes.axisPrimary).ToString());
            TL.LogMessage("AxisRates", $"Got axis rate string: {axisRateString}");

            AxisRateTransfer axisRateTransfer = JsonSerializer.Deserialize<AxisRateTransfer>(axisRateString);
            TL.LogMessage("AxisRates", $"Received {axisRateTransfer.AxisRates.Count} axis rates");

            TL.LogMessage("AxisRates", $"After Device.AxisRates");

            AxisRatesResponse platformAxisRates = null;
            try
            {
                List<RateResponse> rates = new List<RateResponse>();

                foreach (RateTransfer rate in axisRateTransfer.AxisRates)
                {

                    TL.LogMessage("AxisRates", $"Received rate: {rate.Minimum} to {rate.Maximum}");
                    RateResponse platformRate = new RateResponse(rate.Minimum, rate.Maximum);
                    rates.Add(platformRate);
                }
                TL.LogMessage("AxisRates", $"After RateResponse");

                platformAxisRates = new AxisRatesResponse(rates);
                TL.LogMessage("AxisRates", $"After AxisRatesResponse Count: {platformAxisRates.Count}");

            }
            catch (Exception ex)
            {
                TL.LogMessage("AxisRates", $"AxisRates exception 2: {ex}");
            }
            TL.LogMessage("AxisRates", $"platformAxisRates is null: {platformAxisRates is null}");

            return platformAxisRates;

        }

        public bool CanMoveAxis(TelescopeAxes Axis)
        {
            return Device.CanMoveAxis((Common.DeviceInterfaces.TelescopeAxis)Axis);
        }

        public PierSide DestinationSideOfPier(double RightAscension, double Declination)
        {
            return (PierSide)Device.DestinationSideOfPier(RightAscension, Declination);
        }

        public void FindHome()
        {
            Device.FindHome();
        }

        public void MoveAxis(TelescopeAxes Axis, double Rate)
        {
            Device.MoveAxis((Common.DeviceInterfaces.TelescopeAxis)Axis, Rate);
        }

        public void Park()
        {
            Device.Park();
        }

        public void PulseGuide(GuideDirections Direction, int Duration)
        {
            Device.PulseGuide((Common.DeviceInterfaces.GuideDirection)Direction, Duration);
        }

        public void SetPark()
        {
            Device.SetPark();
        }

        public void SlewToAltAz(double Azimuth, double Altitude)
        {
            Device.SlewToAltAz(Azimuth, Altitude);
        }

        public void SlewToAltAzAsync(double Azimuth, double Altitude)
        {
            Device.SlewToAltAzAsync(Azimuth, Altitude);
        }

        public void SlewToCoordinates(double RightAscension, double Declination)
        {
            Device.SlewToCoordinates(RightAscension, Declination);
        }

        public void SlewToCoordinatesAsync(double RightAscension, double Declination)
        {
            Device.SlewToCoordinatesAsync(RightAscension, Declination);
        }

        public void SlewToTarget()
        {
            Device.SlewToTarget();
        }

        public void SlewToTargetAsync()
        {
            Device.SlewToTargetAsync();
        }

        public void SyncToAltAz(double Azimuth, double Altitude)
        {
            Device.SyncToAltAz(Azimuth, Altitude);
        }

        public void SyncToCoordinates(double RightAscension, double Declination)
        {
            Device.SyncToCoordinates(RightAscension, Declination);
        }

        public void SyncToTarget()
        {
            Device.SyncToTarget();
        }

        public void Unpark()
        {
            Device.Unpark();
        }
    }

    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    [Guid("3389AF4B-24A5-44AF-A811-A6FE2FDAD132")]
    public class TrackingRates : ITrackingRates, IEnumerable
    {
        List<DriveRates> _driveRates = null;

        public DriveRates this[int index] => _driveRates[index];

        public int Count => _driveRates.Count;

        public TrackingRates(List<DriveRates> rates)
        {
            _driveRates = rates;
        }

        public void Dispose()
        {
        }

        public IEnumerator GetEnumerator()
        {
            return _driveRates.GetEnumerator();
        }
    }

    [Guid("9EFAAC4E-804D-43B7-A4D8-C371055A8DD2")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    [ProgId("OmniSim.RateResponse")]
    public class RateResponse : IRate, IDisposable
    {
        private double m_dMaximum = 0;
        private double m_dMinimum = 0;

        //
        // Default constructor - Internal prevents public creation
        // of instances. These are values for AxisRates.
        //
        internal RateResponse(double Minimum, double Maximum)
        {
            m_dMaximum = Maximum;
            m_dMinimum = Minimum;
        }

        #region IRate Members

        public double Maximum
        {
            get { return m_dMaximum; }
            set { m_dMaximum = value; }
        }

        public double Minimum
        {
            get { return m_dMinimum; }
            set { m_dMinimum = value; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            // nothing to do?
        }

        #endregion
    }

    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    [Guid("7CCA1489-2CA9-4562-9797-980C25B16B3D")]
    [ProgId("OmniSim.AxisRatesResponse")]
    public class AxisRatesResponse : IAxisRates, IEnumerable, IDisposable
    {
        List<RateResponse> _axisRates = null;

        public IRate this[int index] => _axisRates[index];

        public int Count => _axisRates.Count;

        public AxisRatesResponse(List<RateResponse> rates)
        {
            _axisRates = rates;
        }

        public AxisRatesResponse()
        {
            _axisRates = new List<RateResponse>() { new RateResponse(23.0, 46.0) };
        }

        public IEnumerator GetEnumerator()
        {
            return _axisRates.GetEnumerator();
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                _axisRates = null;
            }
        }

        #endregion

    }

    class AxisRateTransfer
    {
        public AxisRateTransfer() { }
        public List<RateTransfer> AxisRates { get; set; } = new List<RateTransfer>();
    }

    class RateTransfer
    {
        public RateTransfer() { }

        public double Minimum { get; set; } = 0.0;
        public double Maximum { get; set; } = 0.0;
    }


}
