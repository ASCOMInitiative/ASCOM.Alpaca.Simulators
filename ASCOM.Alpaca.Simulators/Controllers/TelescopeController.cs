using Alpaca;
using ASCOM.Alpaca.Responses;
using ASCOM.Standard.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ASCOM.Alpaca.Simulators
{
    [ServiceFilter(typeof(AuthorizationFilter))]
    [ApiController]
    [Route("api/v1/telescope/")]
    public class TelescopeController : AlpacaController
    {
        [NonAction]
        public override IAscomDevice GetDevice(int DeviceNumber)
        {
            return DeviceManager.GetTelescope(DeviceNumber);
        }

        [HttpPut]
        [Route("{DeviceNumber}/AbortSlew")]
        public ActionResult<Response> AbortSlew([DefaultValue(0)] int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).AbortSlew(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/AlignmentMode")]
        public ActionResult<IntResponse> AlignmentMode([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => (int)DeviceManager.GetTelescope(DeviceNumber).AlignmentMode, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/Altitude")]
        public ActionResult<DoubleResponse> Altitude([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).Altitude, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/ApertureArea")]
        public ActionResult<DoubleResponse> ApertureArea([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).ApertureArea, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/ApertureDiameter")]
        public ActionResult<DoubleResponse> ApertureDiameter([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).ApertureDiameter, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/AtHome")]
        public ActionResult<BoolResponse> AtHome([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).AtHome, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/AtPark")]
        public ActionResult<BoolResponse> AtPark([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).AtPark, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/AxisRates")]
        public ActionResult<AxisRatesResponse> AxisRates([DefaultValue(0)] int DeviceNumber, TelescopeAxis Axis, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).AxisRates(Axis), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/Azimuth")]
        public ActionResult<DoubleResponse> Azimuth([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).Azimuth, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/CanFindHome")]
        public ActionResult<BoolResponse> CanFindHome([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanFindHome, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/CanMoveAxis")]
        public ActionResult<BoolResponse> CanMoveAxis([DefaultValue(0)] int DeviceNumber, TelescopeAxis Axis, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanMoveAxis(Axis), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/CanPark")]
        public ActionResult<BoolResponse> CanPark([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanPark, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/CanPulseGuide")]
        public ActionResult<BoolResponse> CanPulseGuide([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanPulseGuide, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/CanSetDeclinationRate")]
        public ActionResult<BoolResponse> CanSetDeclinationRate([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanSetDeclinationRate, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/CanSetGuideRates")]
        public ActionResult<BoolResponse> CanSetGuideRates([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanSetGuideRates, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/CanSetPark")]
        public ActionResult<BoolResponse> CanSetPark([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanSetPark, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/CanSetPierSide")]
        public ActionResult<BoolResponse> CanSetPierSide([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanSetPierSide, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/CanSetRightAscensionRate")]
        public ActionResult<BoolResponse> CanSetRightAscensionRate([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanSetRightAscensionRate, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/CanSetTracking")]
        public ActionResult<BoolResponse> CanSetTracking([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanSetTracking, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/CanSlew")]
        public ActionResult<BoolResponse> CanSlew([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanSlew, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/CanSlewAltAz")]
        public ActionResult<BoolResponse> CanSlewAltAz([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanSlewAltAz, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/CanSlewAltAzAsync")]
        public ActionResult<BoolResponse> CanSlewAltAzAsync([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanSlewAltAzAsync, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/CanSlewAsync")]
        public ActionResult<BoolResponse> CanSlewAsync([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanSlewAsync, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/CanSync")]
        public ActionResult<BoolResponse> CanSync([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanSync, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/CanSyncAltAz")]
        public ActionResult<BoolResponse> CanSyncAltAz([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanSyncAltAz, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/CanUnpark")]
        public ActionResult<BoolResponse> CanUnpark([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanUnpark, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/Declination")]
        public ActionResult<DoubleResponse> Declination([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).Declination, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/DeclinationRate")]
        public ActionResult<DoubleResponse> DeclinationRate([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).DeclinationRate, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/DeclinationRate")]
        public ActionResult<Response> DeclinationRate([DefaultValue(0)] int DeviceNumber, [FromForm] double DeclinationRate, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).DeclinationRate = DeclinationRate; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/DestinationSideOfPier")]
        public ActionResult<IntResponse> DestinationSideOfPier([DefaultValue(0)] int DeviceNumber, double RightAscension, double Declination, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => (int)DeviceManager.GetTelescope(DeviceNumber).DestinationSideOfPier(RightAscension, Declination), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/DoesRefraction")]
        public ActionResult<BoolResponse> DoesRefraction([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).DoesRefraction, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/DoesRefraction")]
        public ActionResult<Response> DoesRefraction([DefaultValue(0)] int DeviceNumber, [FromForm] bool DoesRefraction, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).DoesRefraction = DoesRefraction; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/EquatorialSystem")]
        public ActionResult<IntResponse> EquatorialSystem([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => (int)DeviceManager.GetTelescope(DeviceNumber).EquatorialSystem, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/FindHome")]
        public ActionResult<Response> FindHome([DefaultValue(0)] int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).FindHome(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/FocalLength")]
        public ActionResult<DoubleResponse> FocalLength([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).FocalLength, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/GuideRateDeclination")]
        public ActionResult<DoubleResponse> GuideRateDeclination([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).GuideRateDeclination, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/GuideRateDeclination")]
        public ActionResult<Response> GuideRateDeclination([DefaultValue(0)] int DeviceNumber, [FromForm] double GuideRateDeclination, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).GuideRateDeclination = GuideRateDeclination; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/GuideRateRightAscension")]
        public ActionResult<DoubleResponse> GuideRateRightAscension([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).GuideRateRightAscension, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/GuideRateRightAscension")]
        public ActionResult<Response> GuideRateRightAscension([DefaultValue(0)] int DeviceNumber, [FromForm] double GuideRateRightAscension, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).GuideRateRightAscension = GuideRateRightAscension; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/IsPulseGuiding")]
        public ActionResult<BoolResponse> IsPulseGuiding([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).IsPulseGuiding, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/MoveAxis")]
        public ActionResult<Response> MoveAxis([DefaultValue(0)] int DeviceNumber, [FromForm] TelescopeAxis Axis, [FromForm] double Rate, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).MoveAxis(Axis, Rate), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/Park")]
        public ActionResult<Response> Park([DefaultValue(0)] int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).Park(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/PulseGuide")]
        public ActionResult<Response> PulseGuide([DefaultValue(0)] int DeviceNumber, [FromForm] GuideDirection Direction, [FromForm] int Duration, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).PulseGuide(Direction, Duration), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/RightAscension")]
        public ActionResult<DoubleResponse> RightAscension([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).RightAscension, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/RightAscensionRate")]
        public ActionResult<DoubleResponse> RightAscensionRate([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).RightAscensionRate, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/RightAscensionRate")]
        public ActionResult<Response> RightAscensionRate([DefaultValue(0)] int DeviceNumber, [FromForm] double RightAscensionRate, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).RightAscensionRate = RightAscensionRate; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/SetPark")]
        public ActionResult<Response> SetPark([DefaultValue(0)] int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SetPark(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/SideOfPier")]
        public ActionResult<IntResponse> SideOfPier([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => (int)DeviceManager.GetTelescope(DeviceNumber).SideOfPier, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/SideOfPier")]
        public ActionResult<Response> SideOfPier([DefaultValue(0)] int DeviceNumber, [FromForm] PointingState SideOfPier, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).SideOfPier = SideOfPier; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/SiderealTime")]
        public ActionResult<DoubleResponse> SiderealTime([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SiderealTime, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/SiteElevation")]
        public ActionResult<DoubleResponse> SiteElevation([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SiteElevation, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/SiteElevation")]
        public ActionResult<Response> SiteElevation([DefaultValue(0)] int DeviceNumber, [FromForm] double SiteElevation, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).SiteElevation = SiteElevation; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/SiteLatitude")]
        public ActionResult<DoubleResponse> SiteLatitude([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SiteLatitude, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/SiteLatitude")]
        public ActionResult<Response> SiteLatitude([DefaultValue(0)] int DeviceNumber, [FromForm] double SiteLatitude, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).SiteLatitude = SiteLatitude; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/SiteLongitude")]
        public ActionResult<DoubleResponse> SiteLongitude([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SiteLongitude, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/SiteLongitude")]
        public ActionResult<Response> SiteLongitude([DefaultValue(0)] int DeviceNumber, [FromForm] double SiteLongitude, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).SiteLongitude = SiteLongitude; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/SlewSettleTime")]
        public ActionResult<IntResponse> SlewSettleTime([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SlewSettleTime, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/SlewSettleTime")]
        public ActionResult<Response> SlewSettleTime([DefaultValue(0)] int DeviceNumber, [FromForm] short SlewSettleTime, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).SlewSettleTime = SlewSettleTime; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/SlewToAltAz")]
        public ActionResult<Response> SlewToAltAz([DefaultValue(0)] int DeviceNumber, [FromForm] double Azimuth, [FromForm] double Altitude, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SlewToAltAz(Azimuth, Altitude), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/SlewToAltAzAsync")]
        public ActionResult<Response> SlewToAltAzAsync([DefaultValue(0)] int DeviceNumber, [FromForm] double Azimuth, [FromForm] double Altitude, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SlewToAltAzAsync(Azimuth, Altitude), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/SlewToCoordinates")]
        public ActionResult<Response> SlewToCoordinates([DefaultValue(0)] int DeviceNumber, [FromForm] double RightAscension, [FromForm] double Declination, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SlewToCoordinates(RightAscension, Declination), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/SlewToCoordinatesAsync")]
        public ActionResult<Response> SlewToCoordinatesAsync([DefaultValue(0)] int DeviceNumber, [FromForm] double RightAscension, [FromForm] double Declination, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SlewToCoordinatesAsync(RightAscension, Declination), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/SlewToTarget")]
        public ActionResult<Response> SlewToTarget([DefaultValue(0)] int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SlewToTarget(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/SlewToTargetAsync")]
        public ActionResult<Response> SlewToTargetAsync([DefaultValue(0)] int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SlewToTargetAsync(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/Slewing")]
        public ActionResult<BoolResponse> Slewing([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).Slewing, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/SyncToAltAz")]
        public ActionResult<Response> SyncToAltAz([DefaultValue(0)] int DeviceNumber, [FromForm] double Azimuth, [FromForm] double Altitude, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SyncToAltAz(Azimuth, Altitude), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/SyncToCoordinates")]
        public ActionResult<Response> SyncToCoordinates([DefaultValue(0)] int DeviceNumber, [FromForm] double RightAscension, [FromForm] double Declination, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SyncToCoordinates(RightAscension, Declination), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/SyncToTarget")]
        public ActionResult<Response> SyncToTarget([DefaultValue(0)] int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SyncToTarget(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/TargetDeclination")]
        public ActionResult<DoubleResponse> TargetDeclination([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).TargetDeclination, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/TargetDeclination")]
        public ActionResult<Response> TargetDeclination([DefaultValue(0)] int DeviceNumber, [FromForm] double TargetDeclination, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).TargetDeclination = TargetDeclination; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/TargetRightAscension")]
        public ActionResult<DoubleResponse> TargetRightAscension([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).TargetRightAscension, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/TargetRightAscension")]
        public ActionResult<Response> TargetRightAscension([DefaultValue(0)] int DeviceNumber, [FromForm] double TargetRightAscension, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).TargetRightAscension = TargetRightAscension; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/Tracking")]
        public ActionResult<BoolResponse> Tracking([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).Tracking, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/Tracking")]
        public ActionResult<Response> Tracking([DefaultValue(0)] int DeviceNumber, [FromForm] bool Tracking, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).Tracking = Tracking; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/TrackingRate")]
        public ActionResult<IntResponse> TrackingRate([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => (int)DeviceManager.GetTelescope(DeviceNumber).TrackingRate, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/TrackingRate")]
        public ActionResult<Response> TrackingRate([DefaultValue(0)] int DeviceNumber, [FromForm] int TrackingRate, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).TrackingRate = (DriveRate)TrackingRate; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/TrackingRates")]
        public ActionResult<DriveRatesResponse> TrackingRates([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).TrackingRates, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/UTCDate")]
        public ActionResult<StringResponse> UTCDate([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).UTCDate.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ"), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/UTCDate")]
        public ActionResult<Response> UTCDate([DefaultValue(0)] int DeviceNumber, [FromForm] DateTime UTCDate, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).UTCDate = UTCDate; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/UnPark")]
        public ActionResult<Response> UnPark([DefaultValue(0)] int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).UnPark(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }
    }
}