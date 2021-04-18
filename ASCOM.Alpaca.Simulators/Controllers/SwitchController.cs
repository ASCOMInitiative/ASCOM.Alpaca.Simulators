using Alpaca;
using ASCOM.Alpaca.Responses;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ASCOM.Alpaca.Simulators
{
    [ServiceFilter(typeof(AuthorizationFilter))]
    [ApiController]
    public class SwitchController : AlpacaController
    {
        private const string APIRoot = "api/v1/switch/";

        #region Common Methods

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/Action")]
        public ActionResult<ActionResult<StringResponse>> Action([DefaultValue(0)] int DeviceNumber, [Required][FromForm] string Action, [FromForm] string Parameters = "", [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).Action(Action, Parameters), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Action: {Action}, Parameters {Parameters}");
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/CommandBlind")]
        public ActionResult<Response> CommandBlind([DefaultValue(0)] int DeviceNumber, [Required][FromForm] string Command, [FromForm] bool Raw = false, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).CommandBlind(Command, Raw), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Command {Command}, Raw {Raw}");
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/CommandBool")]
        public  ActionResult<BoolResponse> CommandBool([DefaultValue(0)] int DeviceNumber, [Required][FromForm] string Command, [FromForm] bool Raw = false, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).CommandBool(Command, Raw), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Command={Command}, Raw={Raw}");
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/CommandString")]
        public ActionResult<ActionResult<StringResponse>> CommandString([DefaultValue(0)] int DeviceNumber, [Required][FromForm] string Command, [FromForm] bool Raw = false, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).CommandString(Command, Raw), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Command={Command}, Raw={Raw}");
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/Connected")]
        public  ActionResult<BoolResponse> Connected([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).Connected, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/Connected")]
        public ActionResult<Response> Connected([DefaultValue(0)] int DeviceNumber, [Required][FromForm] bool Connected, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            if (Connected || !ServerSettings.PreventRemoteDisconnects)
            {
                return ProcessRequest(() => { DeviceManager.GetSwitch(DeviceNumber).Connected = Connected; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Connected={Connected}");
            }
            return ProcessRequest(() => { }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/Description")]
        public ActionResult<ActionResult<StringResponse>> Description([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).Description, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/DriverInfo")]
        public ActionResult<ActionResult<StringResponse>> DriverInfo([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).DriverInfo, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/DriverVersion")]
        public ActionResult<ActionResult<StringResponse>> DriverVersion([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).DriverVersion, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/InterfaceVersion")]
        public ActionResult<IntResponse> InterfaceVersion([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).InterfaceVersion, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/Name")]
        public ActionResult<ActionResult<StringResponse>> Name([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).Name, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/SupportedActions")]
        public ActionResult<StringListResponse> SupportedActions([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).SupportedActions, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        #region IDisposable Members

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/Dispose")]
        public ActionResult<Response> Dispose([DefaultValue(0)] int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            if (!ServerSettings.PreventRemoteDisposes)
            {
                return ProcessRequest(() => { DeviceManager.GetSwitch(DeviceNumber).Dispose(); }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
            }
            return ProcessRequest(() => { }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        #endregion IDisposable Members

        #endregion Common Methods

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/maxswitch")]
        public ActionResult<IntResponse> MaxSwitch([DefaultValue(0)]int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).MaxSwitch, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/canwrite")]
        public ActionResult<BoolResponse> CanWrite([DefaultValue(0)] int DeviceNumber, [Required][DefaultValue(0)] short id, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).CanWrite(id), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/getswitch")]
        public ActionResult<BoolResponse> GetSwitch([DefaultValue(0)]int DeviceNumber, [Required][DefaultValue(0)] short id, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).CanWrite(id), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/getswitchdescription")]
        public ActionResult<StringResponse> GetSwitchDescription([DefaultValue(0)]int DeviceNumber, [Required][DefaultValue(0)] short id, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).GetSwitchDescription(id), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/getswitchname")]
        public ActionResult<StringResponse> GetSwitchName([DefaultValue(0)]int DeviceNumber, [Required][DefaultValue(0)] short id, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).GetSwitchName(id), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/getswitchvalue")]
        public ActionResult<DoubleResponse> GetSwitchValue([DefaultValue(0)]int DeviceNumber, [Required][DefaultValue(0)] short id, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).GetSwitchValue(id), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/minswitchvalue")]
        public ActionResult<DoubleResponse> MinSwitchValue([DefaultValue(0)]int DeviceNumber, [Required][DefaultValue(0)] short id, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).MinSwitchValue(id), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/maxswitchvalue")]
        public ActionResult<DoubleResponse> MaxSwitchValue([DefaultValue(0)]int DeviceNumber, [Required][DefaultValue(0)] short id, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).MaxSwitchValue(id), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/switchstep")]
        public ActionResult<DoubleResponse> SwitchStep([DefaultValue(0)]int DeviceNumber, [Required][DefaultValue(0)] short id, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).SwitchStep(id), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/setswitch")]
        public ActionResult<Response> SetSwitch([DefaultValue(0)]int DeviceNumber, [FromForm] [Required][DefaultValue(0)] short id, [FromForm][Required] bool State, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).SetSwitch(id, State), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/setswitchname")]
        public ActionResult<Response> SetSwitchName([DefaultValue(0)]int DeviceNumber, [FromForm] [Required][DefaultValue(0)] short id, [Required][FromForm] string name, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).SetSwitchName(id, name), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/setswitchvalue")]
        public ActionResult<Response> SetSwitchValue([DefaultValue(0)]int DeviceNumber, [FromForm][Required][DefaultValue(0)] short id, [Required][FromForm] double value, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).SetSwitchValue(id, value), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }
    }
}
