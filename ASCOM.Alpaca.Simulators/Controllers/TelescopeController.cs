using Alpaca;
using ASCOM.Common.Alpaca;
using ASCOM.Common.DeviceInterfaces;
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
        [Route("{DeviceNumber}/abortslew")]
        public ActionResult<Response> AbortSlew([DefaultValue(0)] int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).AbortSlew(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/alignmentmode")]
        public ActionResult<IntResponse> AlignmentMode([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => (int)DeviceManager.GetTelescope(DeviceNumber).AlignmentMode, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/altitude")]
        public ActionResult<DoubleResponse> Altitude([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).Altitude, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/aperturearea")]
        public ActionResult<DoubleResponse> ApertureArea([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).ApertureArea, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/aperturediameter")]
        public ActionResult<DoubleResponse> ApertureDiameter([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).ApertureDiameter, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/athome")]
        public ActionResult<BoolResponse> AtHome([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).AtHome, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/atpark")]
        public ActionResult<BoolResponse> AtPark([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).AtPark, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/axisrates")]
        public ActionResult<AxisRatesResponse> AxisRates([DefaultValue(0)] int DeviceNumber, TelescopeAxis Axis, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).AxisRates(Axis), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/azimuth")]
        public ActionResult<DoubleResponse> Azimuth([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).Azimuth, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/canfindhome")]
        public ActionResult<BoolResponse> CanFindHome([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanFindHome, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/canmoveaxis")]
        public ActionResult<BoolResponse> CanMoveAxis([DefaultValue(0)] int DeviceNumber, TelescopeAxis Axis, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanMoveAxis(Axis), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/canpark")]
        public ActionResult<BoolResponse> CanPark([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanPark, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/canpulseguide")]
        public ActionResult<BoolResponse> CanPulseGuide([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanPulseGuide, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/cansetdeclinationrate")]
        public ActionResult<BoolResponse> CanSetDeclinationRate([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanSetDeclinationRate, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/cansetguiderates")]
        public ActionResult<BoolResponse> CanSetGuideRates([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanSetGuideRates, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/cansetpark")]
        public ActionResult<BoolResponse> CanSetPark([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanSetPark, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/cansetpierside")]
        public ActionResult<BoolResponse> CanSetPierSide([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanSetPierSide, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/cansetrightascensionrate")]
        public ActionResult<BoolResponse> CanSetRightAscensionRate([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanSetRightAscensionRate, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/cansettracking")]
        public ActionResult<BoolResponse> CanSetTracking([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanSetTracking, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/canslew")]
        public ActionResult<BoolResponse> CanSlew([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanSlew, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/canslewaltaz")]
        public ActionResult<BoolResponse> CanSlewAltAz([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanSlewAltAz, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/canslewaltazasync")]
        public ActionResult<BoolResponse> CanSlewAltAzAsync([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanSlewAltAzAsync, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/canslewasync")]
        public ActionResult<BoolResponse> CanSlewAsync([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanSlewAsync, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/cansync")]
        public ActionResult<BoolResponse> CanSync([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanSync, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/cansyncaltaz")]
        public ActionResult<BoolResponse> CanSyncAltAz([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanSyncAltAz, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/canunpark")]
        public ActionResult<BoolResponse> CanUnpark([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanUnpark, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/declination")]
        public ActionResult<DoubleResponse> Declination([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).Declination, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/declinationrate")]
        public ActionResult<DoubleResponse> DeclinationRate([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).DeclinationRate, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/declinationrate")]
        public ActionResult<Response> DeclinationRate([DefaultValue(0)] int DeviceNumber, [FromForm] double DeclinationRate, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).DeclinationRate = DeclinationRate; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/destinationsideofpier")]
        public ActionResult<IntResponse> DestinationSideOfPier([DefaultValue(0)] int DeviceNumber, double RightAscension, double Declination, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => (int)DeviceManager.GetTelescope(DeviceNumber).DestinationSideOfPier(RightAscension, Declination), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/doesrefraction")]
        public ActionResult<BoolResponse> DoesRefraction([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).DoesRefraction, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/doesrefraction")]
        public ActionResult<Response> DoesRefraction([DefaultValue(0)] int DeviceNumber, [FromForm] bool DoesRefraction, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).DoesRefraction = DoesRefraction; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/equatorialsystem")]
        public ActionResult<IntResponse> EquatorialSystem([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => (int)DeviceManager.GetTelescope(DeviceNumber).EquatorialSystem, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/findhome")]
        public ActionResult<Response> FindHome([DefaultValue(0)] int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).FindHome(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/focallength")]
        public ActionResult<DoubleResponse> FocalLength([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).FocalLength, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/guideratedeclination")]
        public ActionResult<DoubleResponse> GuideRateDeclination([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).GuideRateDeclination, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/guideratedeclination")]
        public ActionResult<Response> GuideRateDeclination([DefaultValue(0)] int DeviceNumber, [FromForm] double GuideRateDeclination, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).GuideRateDeclination = GuideRateDeclination; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/guideraterightascension")]
        public ActionResult<DoubleResponse> GuideRateRightAscension([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).GuideRateRightAscension, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/guideraterightascension")]
        public ActionResult<Response> GuideRateRightAscension([DefaultValue(0)] int DeviceNumber, [FromForm] double GuideRateRightAscension, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).GuideRateRightAscension = GuideRateRightAscension; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/ispulseguiding")]
        public ActionResult<BoolResponse> IsPulseGuiding([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).IsPulseGuiding, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/moveaxis")]
        public ActionResult<Response> MoveAxis([DefaultValue(0)] int DeviceNumber, [FromForm] TelescopeAxis Axis, [FromForm] double Rate, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).MoveAxis(Axis, Rate), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/park")]
        public ActionResult<Response> Park([DefaultValue(0)] int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).Park(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/pulseguide")]
        public ActionResult<Response> PulseGuide([DefaultValue(0)] int DeviceNumber, [FromForm] GuideDirection Direction, [FromForm] int Duration, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).PulseGuide(Direction, Duration), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/rightascension")]
        public ActionResult<DoubleResponse> RightAscension([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).RightAscension, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/rightascensionrate")]
        public ActionResult<DoubleResponse> RightAscensionRate([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).RightAscensionRate, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/rightascensionrate")]
        public ActionResult<Response> RightAscensionRate([DefaultValue(0)] int DeviceNumber, [FromForm] double RightAscensionRate, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).RightAscensionRate = RightAscensionRate; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/setpark")]
        public ActionResult<Response> SetPark([DefaultValue(0)] int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SetPark(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/sideofpier")]
        public ActionResult<IntResponse> SideOfPier([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => (int)DeviceManager.GetTelescope(DeviceNumber).SideOfPier, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/sideofpier")]
        public ActionResult<Response> SideOfPier([DefaultValue(0)] int DeviceNumber, [FromForm] PointingState SideOfPier, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).SideOfPier = SideOfPier; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/siderealtime")]
        public ActionResult<DoubleResponse> SiderealTime([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SiderealTime, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/siteelevation")]
        public ActionResult<DoubleResponse> SiteElevation([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SiteElevation, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/siteelevation")]
        public ActionResult<Response> SiteElevation([DefaultValue(0)] int DeviceNumber, [FromForm] double SiteElevation, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).SiteElevation = SiteElevation; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/sitelatitude")]
        public ActionResult<DoubleResponse> SiteLatitude([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SiteLatitude, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/sitelatitude")]
        public ActionResult<Response> SiteLatitude([DefaultValue(0)] int DeviceNumber, [FromForm] double SiteLatitude, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).SiteLatitude = SiteLatitude; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/sitelongitude")]
        public ActionResult<DoubleResponse> SiteLongitude([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SiteLongitude, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/sitelongitude")]
        public ActionResult<Response> SiteLongitude([DefaultValue(0)] int DeviceNumber, [FromForm] double SiteLongitude, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).SiteLongitude = SiteLongitude; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/slewsettletime")]
        public ActionResult<IntResponse> SlewSettleTime([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SlewSettleTime, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/slewsettletime")]
        public ActionResult<Response> SlewSettleTime([DefaultValue(0)] int DeviceNumber, [FromForm] short SlewSettleTime, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).SlewSettleTime = SlewSettleTime; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/slewtoaltaz")]
        public ActionResult<Response> SlewToAltAz([DefaultValue(0)] int DeviceNumber, [FromForm] double Azimuth, [FromForm] double Altitude, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SlewToAltAz(Azimuth, Altitude), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/slewtoaltazasync")]
        public ActionResult<Response> SlewToAltAzAsync([DefaultValue(0)] int DeviceNumber, [FromForm] double Azimuth, [FromForm] double Altitude, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SlewToAltAzAsync(Azimuth, Altitude), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/slewtocoordinates")]
        public ActionResult<Response> SlewToCoordinates([DefaultValue(0)] int DeviceNumber, [FromForm] double RightAscension, [FromForm] double Declination, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SlewToCoordinates(RightAscension, Declination), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/slewtocoordinatesasync")]
        public ActionResult<Response> SlewToCoordinatesAsync([DefaultValue(0)] int DeviceNumber, [FromForm] double RightAscension, [FromForm] double Declination, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SlewToCoordinatesAsync(RightAscension, Declination), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/slewtotarget")]
        public ActionResult<Response> SlewToTarget([DefaultValue(0)] int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SlewToTarget(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/slewtotargetasync")]
        public ActionResult<Response> SlewToTargetAsync([DefaultValue(0)] int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SlewToTargetAsync(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/slewing")]
        public ActionResult<BoolResponse> Slewing([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).Slewing, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/synctoaltaz")]
        public ActionResult<Response> SyncToAltAz([DefaultValue(0)] int DeviceNumber, [FromForm] double Azimuth, [FromForm] double Altitude, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SyncToAltAz(Azimuth, Altitude), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/synctocoordinates")]
        public ActionResult<Response> SyncToCoordinates([DefaultValue(0)] int DeviceNumber, [FromForm] double RightAscension, [FromForm] double Declination, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SyncToCoordinates(RightAscension, Declination), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/synctotarget")]
        public ActionResult<Response> SyncToTarget([DefaultValue(0)] int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SyncToTarget(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/targetdeclination")]
        public ActionResult<DoubleResponse> TargetDeclination([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).TargetDeclination, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/targetdeclination")]
        public ActionResult<Response> TargetDeclination([DefaultValue(0)] int DeviceNumber, [FromForm] double TargetDeclination, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).TargetDeclination = TargetDeclination; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/targetrightascension")]
        public ActionResult<DoubleResponse> TargetRightAscension([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).TargetRightAscension, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/targetrightascension")]
        public ActionResult<Response> TargetRightAscension([DefaultValue(0)] int DeviceNumber, [FromForm] double TargetRightAscension, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).TargetRightAscension = TargetRightAscension; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/tracking")]
        public ActionResult<BoolResponse> Tracking([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).Tracking, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/tracking")]
        public ActionResult<Response> Tracking([DefaultValue(0)] int DeviceNumber, [FromForm] bool Tracking, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).Tracking = Tracking; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/trackingrate")]
        public ActionResult<IntResponse> TrackingRate([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => (int)DeviceManager.GetTelescope(DeviceNumber).TrackingRate, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/trackingrate")]
        public ActionResult<Response> TrackingRate([DefaultValue(0)] int DeviceNumber, [FromForm] int TrackingRate, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).TrackingRate = (DriveRate)TrackingRate; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/trackingrates")]
        public ActionResult<DriveRatesResponse> TrackingRates([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).TrackingRates, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/utcdate")]
        public ActionResult<StringResponse> UTCDate([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).UTCDate.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ"), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/utcdate")]
        public ActionResult<Response> UTCDate([DefaultValue(0)] int DeviceNumber, [FromForm] DateTime UTCDate, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).UTCDate = UTCDate; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/unpark")]
        public ActionResult<Response> UnPark([DefaultValue(0)] int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).UnPark(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }
    }
}