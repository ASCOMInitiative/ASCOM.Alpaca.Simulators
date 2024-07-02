using ASCOM.DeviceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ASCOM.Simulators.LocalServer.Drivers
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class Telescope : BaseDriver, ASCOM.DeviceInterface.ITelescopeV4, IDisposable
    {
        ASCOM.Common.DeviceInterfaces.ITelescopeV3 Device = new ASCOM.Com.DriverAccess.Telescope("OmniSim.Telescope");

        public Telescope()
        {
            base.GetDevice = () => (ASCOM.Common.DeviceInterfaces.IAscomDeviceV2)Device;
        }

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

		public bool CanSyncAltAz => Device.CanSyncAltAz;

		public bool CanUnpark => Device.CanUnpark;

		public double Declination => Device.Declination;

		public double DeclinationRate { get => Device.DeclinationRate; set => Device.DeclinationRate = value; }
		public bool DoesRefraction { get => Device.DoesRefraction; set => Device.DoesRefraction = value; }

		public EquatorialCoordinateType EquatorialSystem => (EquatorialCoordinateType) Device.EquatorialSystem;

		public double FocalLength => Device.FocalLength;

		public double GuideRateDeclination { get => Device.GuideRateDeclination; set => Device.GuideRateDeclination = value; }
		public double GuideRateRightAscension { get => Device.GuideRateRightAscension; set => Device.GuideRateRightAscension = value; }

		public bool IsPulseGuiding => Device.IsPulseGuiding;

		public double RightAscension => Device.RightAscension;

		public double RightAscensionRate { get => Device.RightAscensionRate; set => Device.RightAscensionRate = value; }
		public PierSide SideOfPier { get => (PierSide) Device.SideOfPier; set => Device.SideOfPier = (Common.DeviceInterfaces.PointingState)value; }

		public double SiderealTime => Device.SiderealTime;

		public double SiteElevation { get => Device.SiteElevation; set => Device.SiteElevation = value; }
		public double SiteLatitude { get => Device.SiteLatitude; set => Device.SiteLatitude = value; }
		public double SiteLongitude { get => Device.SiteLongitude; set => Device.SiteLongitude = value; }

		public bool Slewing => Device.Slewing;

		public short SlewSettleTime { get => Device.SlewSettleTime; set => Device.SlewSettleTime = value; }
		public double TargetDeclination { get => Device.TargetDeclination; set => Device.TargetDeclination = value; }
		public double TargetRightAscension { get => Device.TargetRightAscension; set => Device.TargetRightAscension = value; }
		public bool Tracking { get => Device.Tracking; set => Device.Tracking = value; }
		public DriveRates TrackingRate { get => (DriveRates) Device.TrackingRate; set => Device.TrackingRate = (Common.DeviceInterfaces.DriveRate)value; }

		public ITrackingRates TrackingRates => throw new System.NotImplementedException();

		public DateTime UTCDate { get => Device.UTCDate; set => Device.UTCDate = value; }

		public void AbortSlew()
		{
			Device.AbortSlew();
		}

		public IAxisRates AxisRates(TelescopeAxes Axis)
		{
			throw new System.NotImplementedException();
		}

		public bool CanMoveAxis(TelescopeAxes Axis)
		{
			return Device.CanMoveAxis((Common.DeviceInterfaces.TelescopeAxis)Axis);
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
			Device.MoveAxis((Common.DeviceInterfaces.TelescopeAxis)Axis, Rate);
		}

		public void Park()
		{
			Device.Park();
		}

		public void PulseGuide(GuideDirections Direction, int Duration)
		{
			Device.PulseGuide((Common.DeviceInterfaces.GuideDirection) Direction, Duration);
		}

		public void SetPark()
		{
			Device.SetPark();
		}

		public void SlewToAltAz(double Azimuth, double Altitude)
		{
			Device.SlewToAltAz(Azimuth,Altitude);
		}

		public void SlewToAltAzAsync(double Azimuth, double Altitude)
		{
			Device.SlewToAltAzAsync(Azimuth,Altitude);
		}

		public void SlewToCoordinates(double RightAscension, double Declination)
		{
			Device.SlewToCoordinates(RightAscension,Declination);
		}

		public void SlewToCoordinatesAsync(double RightAscension, double Declination)
		{
			Device.SlewToCoordinatesAsync(RightAscension,Declination);
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
}
