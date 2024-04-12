using ASCOM.DeviceInterface;
using System.Collections;
using System.Runtime.InteropServices;

namespace OmniSim.LocalServer.Drivers
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class Telescope : BaseDriver, ASCOM.DeviceInterface.ITelescopeV4, IDisposable
    {
        public ASCOM.Common.DeviceInterfaces.ITelescopeV4 Device => (base.DeviceV2 as ASCOM.Common.DeviceInterfaces.ITelescopeV4);

        public AlignmentModes AlignmentMode => (AlignmentModes) Device.AlignmentMode;

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

        public bool CanSyncAltAz => Device.CanSlewAltAz;

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
        public PierSide SideOfPier { get => (PierSide)Device.SideOfPier; set => Device.SideOfPier = (ASCOM.Common.DeviceInterfaces.PointingState) value; }

        public double SiderealTime => Device.SiderealTime;

        public double SiteElevation { get => Device.SiteElevation; set => Device.SiteElevation = value; }
        public double SiteLatitude { get => Device.SiteLatitude; set => Device.SiteLatitude = value; }
        public double SiteLongitude { get => Device.SiteLongitude; set => Device.SiteLongitude = value; }

        public bool Slewing => Device.Slewing;

        public short SlewSettleTime { get => Device.SlewSettleTime; set => Device.SlewSettleTime = value; }
        public double TargetDeclination { get => Device.TargetDeclination; set => Device.TargetDeclination = value; }
        public double TargetRightAscension { get => Device.TargetRightAscension; set => Device.TargetRightAscension = value; }
        public bool Tracking { get => Device.Tracking; set => Device.Tracking = value; }
        public DriveRates TrackingRate { get => (DriveRates) Device.TrackingRate; set => Device.TrackingRate = (ASCOM.Common.DeviceInterfaces.DriveRate)value; }

        public ITrackingRates TrackingRates => new TrackingRates(Device.TrackingRates);

        public DateTime UTCDate { get => Device.UTCDate; set => Device.UTCDate = value; }

        public static Func<ASCOM.Common.DeviceInterfaces.IAscomDeviceV2> DeviceAccess;

        public Telescope()
        {
            base.GetDevice = DeviceAccess;
        }

        public void AbortSlew()
        {
            Device.AbortSlew();
        }

        public IAxisRates AxisRates(TelescopeAxes Axis)
        {
            return new AxisRate(Device.AxisRates((ASCOM.Common.DeviceInterfaces.TelescopeAxis)Axis));
        }

        public bool CanMoveAxis(TelescopeAxes Axis)
        {
            return Device.CanMoveAxis((ASCOM.Common.DeviceInterfaces.TelescopeAxis)Axis);
        }

        public PierSide DestinationSideOfPier(double RightAscension, double Declination)
        {
            return (PierSide) Device.DestinationSideOfPier(RightAscension, Declination);
        }

        public void FindHome()
        {
            Device.FindHome();
        }

        public void MoveAxis(TelescopeAxes Axis, double Rate)
        {
            Device.MoveAxis((ASCOM.Common.DeviceInterfaces.TelescopeAxis)Axis, Rate);
        }

        public void Park()
        {
            Device.Park();
        }

        public void PulseGuide(GuideDirections Direction, int Duration)
        {
            Device.PulseGuide((ASCOM.Common.DeviceInterfaces.GuideDirection) Direction, Duration);
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

    public class AxisRate : IAxisRates
    {
        ASCOM.Common.DeviceInterfaces.IAxisRates _rates = null;

        public IRate this[int index] => new Rate(_rates[index].Minimum, _rates[index].Maximum);

        public int Count => _rates.Count;

        public AxisRate(ASCOM.Common.DeviceInterfaces.IAxisRates rates) 
        {
            _rates = rates;
        }

        public void Dispose()
        {
        }

        public IEnumerator GetEnumerator()
        {
            return _rates.GetEnumerator();
        }
    }

    public class Rate : IRate
    {
        public double Maximum { get; set; }
        public double Minimum { get; set; }

        public Rate(double Min, double Max) 
        { 
            Maximum = Max; Minimum = Min;
        }

        public void Dispose()
        {
            
        }
    }

    public class TrackingRates : ITrackingRates
    {
        ASCOM.Common.DeviceInterfaces.ITrackingRates _driveRates = null;

        public DriveRates this[int index] => (DriveRates) _driveRates[index];

        public int Count => _driveRates.Count;

        public TrackingRates(ASCOM.Common.DeviceInterfaces.ITrackingRates rates)
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
}
