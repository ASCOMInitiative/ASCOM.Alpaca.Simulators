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
    public class TelescopeController : AlpacaController
    {
        private const string APIRoot = "api/v1/telescope/";

        #region Common Methods

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/Action")]
        public ActionResult<StringResponse> Action([DefaultValue(0)] int DeviceNumber, [Required][FromForm] string Action, [FromForm] string Parameters = "", [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).Action(Action, Parameters), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Action: {Action}, Parameters {Parameters}");
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/CommandBlind")]
        public ActionResult<Response> CommandBlind([DefaultValue(0)] int DeviceNumber, [Required][FromForm] string Command, [FromForm] bool Raw = false, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CommandBlind(Command, Raw), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Command {Command}, Raw {Raw}");
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/CommandBool")]
        public ActionResult<BoolResponse> CommandBool([DefaultValue(0)] int DeviceNumber, [Required][FromForm] string Command, [FromForm] bool Raw = false, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CommandBool(Command, Raw), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Command={Command}, Raw={Raw}");
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/CommandString")]
        public ActionResult<StringResponse> CommandString([DefaultValue(0)] int DeviceNumber, [Required][FromForm] string Command, [FromForm] bool Raw = false, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CommandString(Command, Raw), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Command={Command}, Raw={Raw}");
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/Connected")]
        public ActionResult<BoolResponse> Connected([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).Connected, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/Connected")]
        public ActionResult<Response> Connected([DefaultValue(0)] int DeviceNumber, [Required][FromForm] bool Connected, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            if (Connected || !ServerSettings.PreventRemoteDisconnects)
            {
                return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).Connected = Connected; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Connected={Connected}");
            }
            return ProcessRequest(() => { }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/Description")]
        public ActionResult<StringResponse> Description([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).Description, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/DriverInfo")]
        public ActionResult<StringResponse> DriverInfo([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).DriverInfo, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/DriverVersion")]
        public ActionResult<StringResponse> DriverVersion([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).DriverVersion, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/InterfaceVersion")]
        public ActionResult<IntResponse> InterfaceVersion([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).InterfaceVersion, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/Name")]
        public ActionResult<StringResponse> Name([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).Name, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/SupportedActions")]
        public ActionResult<StringListResponse> SupportedActions([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SupportedActions, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        #region IDisposable Members

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/Dispose")]
        public ActionResult<Response> Dispose([DefaultValue(0)] int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            if (!ServerSettings.PreventRemoteDisposes)
            {
                return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).Dispose(); }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
            }
            return ProcessRequest(() => { }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        #endregion IDisposable Members

        #endregion Common Methods

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/AbortSlew")]
        public ActionResult<Response> AbortSlew([DefaultValue(0)] int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).AbortSlew(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/AlignmentMode")]
        public ActionResult<IntResponse> AlignmentMode([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => (int)DeviceManager.GetTelescope(DeviceNumber).AlignmentMode, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/Altitude")]
        public ActionResult<DoubleResponse> Altitude([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).Altitude, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/ApertureArea")]
        public ActionResult<DoubleResponse> ApertureArea([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).ApertureArea, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/ApertureDiameter")]
        public ActionResult<DoubleResponse> ApertureDiameter([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).ApertureDiameter, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/AtHome")]
        public ActionResult<BoolResponse> AtHome([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).AtHome, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/AtPark")]
        public ActionResult<BoolResponse> AtPark([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).AtPark, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/AxisRates")]
        public ActionResult<AxisRatesResponse> AxisRates([DefaultValue(0)] int DeviceNumber, TelescopeAxis Axis, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).AxisRates(Axis), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/Azimuth")]
        public ActionResult<DoubleResponse> Azimuth([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).Azimuth, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/CanFindHome")]
        public ActionResult<BoolResponse> CanFindHome([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanFindHome, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/CanMoveAxis")]
        public ActionResult<BoolResponse> CanMoveAxis([DefaultValue(0)] int DeviceNumber, TelescopeAxis Axis, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanMoveAxis(Axis), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/CanPark")]
        public ActionResult<BoolResponse> CanPark([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanPark, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/CanPulseGuide")]
        public ActionResult<BoolResponse> CanPulseGuide([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanPulseGuide, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/CanSetDeclinationRate")]
        public ActionResult<BoolResponse> CanSetDeclinationRate([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanSetDeclinationRate, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/CanSetGuideRates")]
        public ActionResult<BoolResponse> CanSetGuideRates([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanSetGuideRates, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/CanSetPark")]
        public ActionResult<BoolResponse> CanSetPark([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanSetPark, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/CanSetPierSide")]
        public ActionResult<BoolResponse> CanSetPierSide([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanSetPierSide, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/CanSetRightAscensionRate")]
        public ActionResult<BoolResponse> CanSetRightAscensionRate([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanSetRightAscensionRate, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/CanSetTracking")]
        public ActionResult<BoolResponse> CanSetTracking([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanSetTracking, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/CanSlew")]
        public ActionResult<BoolResponse> CanSlew([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanSlew, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/CanSlewAltAz")]
        public ActionResult<BoolResponse> CanSlewAltAz([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanSlewAltAz, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/CanSlewAltAzAsync")]
        public ActionResult<BoolResponse> CanSlewAltAzAsync([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanSlewAltAzAsync, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/CanSlewAsync")]
        public ActionResult<BoolResponse> CanSlewAsync([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanSlewAsync, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/CanSync")]
        public ActionResult<BoolResponse> CanSync([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanSync, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/CanSyncAltAz")]
        public ActionResult<BoolResponse> CanSyncAltAz([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanSyncAltAz, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/CanUnpark")]
        public ActionResult<BoolResponse> CanUnpark([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).CanUnpark, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/Declination")]
        public ActionResult<DoubleResponse> Declination([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).Declination, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/DeclinationRate")]
        public ActionResult<DoubleResponse> DeclinationRate([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).DeclinationRate, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/DeclinationRate")]
        public ActionResult<Response> DeclinationRate([DefaultValue(0)] int DeviceNumber, [FromForm] double DeclinationRate, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).DeclinationRate = DeclinationRate; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/DestinationSideOfPier")]
        public ActionResult<IntResponse> DestinationSideOfPier([DefaultValue(0)] int DeviceNumber, double RightAscension, double Declination, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => (int)DeviceManager.GetTelescope(DeviceNumber).DestinationSideOfPier(RightAscension, Declination), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/DoesRefraction")]
        public ActionResult<BoolResponse> DoesRefraction([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).DoesRefraction, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/DoesRefraction")]
        public ActionResult<Response> DoesRefraction([DefaultValue(0)] int DeviceNumber, [FromForm] bool DoesRefraction, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).DoesRefraction = DoesRefraction; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/EquatorialSystem")]
        public ActionResult<IntResponse> EquatorialSystem([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => (int)DeviceManager.GetTelescope(DeviceNumber).EquatorialSystem, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/FindHome")]
        public ActionResult<Response> FindHome([DefaultValue(0)] int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).FindHome(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/FocalLength")]
        public ActionResult<DoubleResponse> FocalLength([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).FocalLength, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/GuideRateDeclination")]
        public ActionResult<DoubleResponse> GuideRateDeclination([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).GuideRateDeclination, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/GuideRateDeclination")]
        public ActionResult<Response> GuideRateDeclination([DefaultValue(0)] int DeviceNumber, [FromForm] double GuideRateDeclination, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).GuideRateDeclination = GuideRateDeclination; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/GuideRateRightAscension")]
        public ActionResult<DoubleResponse> GuideRateRightAscension([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).GuideRateRightAscension, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/GuideRateRightAscension")]
        public ActionResult<Response> GuideRateRightAscension([DefaultValue(0)] int DeviceNumber, [FromForm] double GuideRateRightAscension, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).GuideRateRightAscension = GuideRateRightAscension; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/IsPulseGuiding")]
        public ActionResult<BoolResponse> IsPulseGuiding([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).IsPulseGuiding, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/MoveAxis")]
        public ActionResult<Response> MoveAxis([DefaultValue(0)] int DeviceNumber, [FromForm] TelescopeAxis Axis, [FromForm] double Rate, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).MoveAxis(Axis, Rate), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/Park")]
        public ActionResult<Response> Park([DefaultValue(0)] int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).Park(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/PulseGuide")]
        public ActionResult<Response> PulseGuide([DefaultValue(0)] int DeviceNumber, [FromForm] GuideDirection Direction, [FromForm] int Duration, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).PulseGuide(Direction, Duration), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/RightAscension")]
        public ActionResult<DoubleResponse> RightAscension([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).RightAscension, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/RightAscensionRate")]
        public ActionResult<DoubleResponse> RightAscensionRate([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).RightAscensionRate, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/RightAscensionRate")]
        public ActionResult<Response> RightAscensionRate([DefaultValue(0)] int DeviceNumber, [FromForm] double RightAscensionRate, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).RightAscensionRate = RightAscensionRate; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/SetPark")]
        public ActionResult<Response> SetPark([DefaultValue(0)] int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SetPark(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/SideOfPier")]
        public ActionResult<IntResponse> SideOfPier([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => (int)DeviceManager.GetTelescope(DeviceNumber).SideOfPier, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/SideOfPier")]
        public ActionResult<Response> SideOfPier([DefaultValue(0)] int DeviceNumber, [FromForm] PointingState SideOfPier, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).SideOfPier = SideOfPier; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/SiderealTime")]
        public ActionResult<DoubleResponse> SiderealTime([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SiderealTime, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/SiteElevation")]
        public ActionResult<DoubleResponse> SiteElevation([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SiteElevation, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/SiteElevation")]
        public ActionResult<Response> SiteElevation([DefaultValue(0)] int DeviceNumber, [FromForm] double SiteElevation, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).SiteElevation = SiteElevation; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/SiteLatitude")]
        public ActionResult<DoubleResponse> SiteLatitude([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SiteLatitude, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/SiteLatitude")]
        public ActionResult<Response> SiteLatitude([DefaultValue(0)] int DeviceNumber, [FromForm] double SiteLatitude, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).SiteLatitude = SiteLatitude; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/SiteLongitude")]
        public ActionResult<DoubleResponse> SiteLongitude([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SiteLongitude, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/SiteLongitude")]
        public ActionResult<Response> SiteLongitude([DefaultValue(0)] int DeviceNumber, [FromForm] double SiteLongitude, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).SiteLongitude = SiteLongitude; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/SlewSettleTime")]
        public ActionResult<IntResponse> SlewSettleTime([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SlewSettleTime, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/SlewSettleTime")]
        public ActionResult<Response> SlewSettleTime([DefaultValue(0)] int DeviceNumber, [FromForm] short SlewSettleTime, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).SlewSettleTime = SlewSettleTime; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/SlewToAltAz")]
        public ActionResult<Response> SlewToAltAz([DefaultValue(0)] int DeviceNumber, [FromForm] double Azimuth, [FromForm] double Altitude, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SlewToAltAz(Azimuth, Altitude), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/SlewToAltAzAsync")]
        public ActionResult<Response> SlewToAltAzAsync([DefaultValue(0)] int DeviceNumber, [FromForm] double Azimuth, [FromForm] double Altitude, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SlewToAltAzAsync(Azimuth, Altitude), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/SlewToCoordinates")]
        public ActionResult<Response> SlewToCoordinates([DefaultValue(0)] int DeviceNumber, [FromForm] double RightAscension, [FromForm] double Declination, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SlewToCoordinates(RightAscension, Declination), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/SlewToCoordinatesAsync")]
        public ActionResult<Response> SlewToCoordinatesAsync([DefaultValue(0)] int DeviceNumber, [FromForm] double RightAscension, [FromForm] double Declination, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SlewToCoordinatesAsync(RightAscension, Declination), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/SlewToTarget")]
        public ActionResult<Response> SlewToTarget([DefaultValue(0)] int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SlewToTarget(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/SlewToTargetAsync")]
        public ActionResult<Response> SlewToTargetAsync([DefaultValue(0)] int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SlewToTargetAsync(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/Slewing")]
        public ActionResult<BoolResponse> Slewing([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).Slewing, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/SyncToAltAz")]
        public ActionResult<Response> SyncToAltAz([DefaultValue(0)] int DeviceNumber, [FromForm] double Azimuth, [FromForm] double Altitude, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SyncToAltAz(Azimuth, Altitude), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/SyncToCoordinates")]
        public ActionResult<Response> SyncToCoordinates([DefaultValue(0)] int DeviceNumber, [FromForm] double RightAscension, [FromForm] double Declination, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SyncToCoordinates(RightAscension, Declination), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/SyncToTarget")]
        public ActionResult<Response> SyncToTarget([DefaultValue(0)] int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).SyncToTarget(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/TargetDeclination")]
        public ActionResult<DoubleResponse> TargetDeclination([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).TargetDeclination, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/TargetDeclination")]
        public ActionResult<Response> TargetDeclination([DefaultValue(0)] int DeviceNumber, [FromForm] double TargetDeclination, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).TargetDeclination = TargetDeclination; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/TargetRightAscension")]
        public ActionResult<DoubleResponse> TargetRightAscension([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).TargetRightAscension, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/TargetRightAscension")]
        public ActionResult<Response> TargetRightAscension([DefaultValue(0)] int DeviceNumber, [FromForm] double TargetRightAscension, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).TargetRightAscension = TargetRightAscension; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/Tracking")]
        public ActionResult<BoolResponse> Tracking([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).Tracking, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/Tracking")]
        public ActionResult<Response> Tracking([DefaultValue(0)] int DeviceNumber, [FromForm] bool Tracking, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).Tracking = Tracking; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/TrackingRate")]
        public ActionResult<IntResponse> TrackingRate([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => (int)DeviceManager.GetTelescope(DeviceNumber).TrackingRate, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/TrackingRate")]
        public ActionResult<Response> TrackingRate([DefaultValue(0)] int DeviceNumber, [FromForm] int TrackingRate, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).TrackingRate = (DriveRate)TrackingRate; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/TrackingRates")]
        public ActionResult<DriveRatesResponse> TrackingRates([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).TrackingRates, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/UTCDate")]
        public ActionResult<StringResponse> UTCDate([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).UTCDate.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ"), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/UTCDate")]
        public ActionResult<Response> UTCDate([DefaultValue(0)] int DeviceNumber, [FromForm] DateTime UTCDate, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetTelescope(DeviceNumber).UTCDate = UTCDate; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/UnPark")]
        public ActionResult<Response> UnPark([DefaultValue(0)] int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetTelescope(DeviceNumber).UnPark(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }
    }
}