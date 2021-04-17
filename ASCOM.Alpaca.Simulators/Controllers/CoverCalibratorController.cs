using Alpaca;
using ASCOM.Alpaca.Responses;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ASCOM.Alpaca.Simulators
{
    [ServiceFilter(typeof(AuthorizationFilter))]
    [ApiController]
    public class CoverCalibrator : AlpacaController
    {
        private const string APIRoot = "api/v1/covercalibrator/";

        #region Common Methods

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/Action")]
        public ActionResult<StringResponse> Action([DefaultValue(0)] int DeviceNumber, [Required][FromForm] string Action, [FromForm] string Parameters = "", [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCoverCalibrator(DeviceNumber).Action(Action, Parameters), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Action: {Action}, Parameters {Parameters}");
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/CommandBlind")]
        public ActionResult<Response> CommandBlind([DefaultValue(0)] int DeviceNumber, [Required][FromForm] string Command, [FromForm] bool Raw = false, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCoverCalibrator(DeviceNumber).CommandBlind(Command, Raw), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Command {Command}, Raw {Raw}");
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/CommandBool")]
        public ActionResult<BoolResponse> CommandBool([DefaultValue(0)] int DeviceNumber, [Required][FromForm] string Command, [FromForm] bool Raw = false, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCoverCalibrator(DeviceNumber).CommandBool(Command, Raw), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Command={Command}, Raw={Raw}");
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/CommandString")]
        public ActionResult<StringResponse> CommandString([DefaultValue(0)] int DeviceNumber, [Required][FromForm] string Command, [FromForm] bool Raw = false, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCoverCalibrator(DeviceNumber).CommandString(Command, Raw), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Command={Command}, Raw={Raw}");
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/Connected")]
        public ActionResult<BoolResponse> Connected([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCoverCalibrator(DeviceNumber).Connected, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/Connected")]
        public ActionResult<Response> Connected([DefaultValue(0)] int DeviceNumber, [Required][FromForm] bool Connected, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            if (Connected || !ServerSettings.PreventRemoteDisconnects)
            {
                return ProcessRequest(() => { DeviceManager.GetCoverCalibrator(DeviceNumber).Connected = Connected; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Connected={Connected}");
            }
            return ProcessRequest(() => { }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/Description")]
        public ActionResult<StringResponse> Description([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCoverCalibrator(DeviceNumber).Description, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/DriverInfo")]
        public ActionResult<StringResponse> DriverInfo([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCoverCalibrator(DeviceNumber).DriverInfo, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/DriverVersion")]
        public ActionResult<StringResponse> DriverVersion([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCoverCalibrator(DeviceNumber).DriverVersion, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/InterfaceVersion")]
        public ActionResult<IntResponse> InterfaceVersion([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCoverCalibrator(DeviceNumber).InterfaceVersion, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/Name")]
        public ActionResult<StringResponse> Name([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCoverCalibrator(DeviceNumber).Name, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/SupportedActions")]
        public ActionResult<StringListResponse> SupportedActions([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCoverCalibrator(DeviceNumber).SupportedActions, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        #region IDisposable Members

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/Dispose")]
        public ActionResult<Response> Dispose([DefaultValue(0)] int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            if (!ServerSettings.PreventRemoteDisposes)
            {
                return ProcessRequest(() => { DeviceManager.GetCoverCalibrator(DeviceNumber).Dispose(); }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
            }
            return ProcessRequest(() => { }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        #endregion IDisposable Members

        #endregion Common Methods

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/CoverState")]
        public ActionResult<IntResponse> CoverState([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => (int)DeviceManager.GetCoverCalibrator(DeviceNumber).CoverState, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/CalibratorState")]
        public ActionResult<IntResponse> CalibratorState([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => (int)DeviceManager.GetCoverCalibrator(DeviceNumber).CalibratorState, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/Brightness")]
        public ActionResult<IntResponse> Brightness([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCoverCalibrator(DeviceNumber).Brightness, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/MaxBrightness")]
        public ActionResult<IntResponse> MaxBrightness([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCoverCalibrator(DeviceNumber).MaxBrightness, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/OpenCover")]
        public ActionResult<Response> OpenCover([DefaultValue(0)] int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCoverCalibrator(DeviceNumber).OpenCover(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/CloseCover")]
        public ActionResult<Response> CloseCover([DefaultValue(0)] int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCoverCalibrator(DeviceNumber).CloseCover(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/HaltCover")]
        public ActionResult<Response> HaltCover([DefaultValue(0)] int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCoverCalibrator(DeviceNumber).HaltCover(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/CalibratorOn")]
        public ActionResult<Response> CalibratorOn([DefaultValue(0)] int DeviceNumber, [DefaultValue(0)][Required][FromForm] int Brightness, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCoverCalibrator(DeviceNumber).CalibratorOn(Brightness), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Brightness={Brightness}");
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/CalibratorOff")]
        public ActionResult<Response> CalibratorOff([DefaultValue(0)] int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCoverCalibrator(DeviceNumber).CalibratorOff(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }
    }
}