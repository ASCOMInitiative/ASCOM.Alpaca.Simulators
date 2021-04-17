using ASCOM.Alpaca.Responses;
using ASCOM.Standard.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace ASCOM.Alpaca.Simulators
{
    [ServiceFilter(typeof(AuthorizationFilter))]
    [ApiController]
    public class SwitchController : Controller
    {
        private const string APIRoot = "api/v1/switch/";

        #region Common Methods

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/Action")]
        public StringResponse Action([DefaultValue(0)]int DeviceNumber, [Required][FromForm] string Action, [FromForm] string Parameters = "", [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = DeviceManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new StringResponse(ClientTransactionID, TransactionID, DeviceManager.GetSwitch(DeviceNumber).Action(Action, Parameters));
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<StringResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/CommandBlind")]
        public Response CommandBlind([DefaultValue(0)]int DeviceNumber, [Required][FromForm] string Command, [FromForm] bool Raw = false, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = DeviceManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                DeviceManager.GetSwitch(DeviceNumber).CommandBlind(Command, Raw);
                return new Response() { ClientTransactionID = ClientTransactionID, ServerTransactionID = TransactionID };
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<Response>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/CommandBool")]
        public BoolResponse CommandBool([DefaultValue(0)]int DeviceNumber, [Required][FromForm] string Command, [FromForm] bool Raw = false, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = DeviceManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new BoolResponse(ClientTransactionID, TransactionID, DeviceManager.GetSwitch(DeviceNumber).CommandBool(Command, Raw));
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<BoolResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/CommandString")]
        public StringResponse CommandString([DefaultValue(0)]int DeviceNumber, [Required][FromForm] string Command, [FromForm] bool Raw = false, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = DeviceManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new StringResponse(ClientTransactionID, TransactionID, DeviceManager.GetSwitch(DeviceNumber).CommandString(Command, Raw));
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<StringResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/Connected")]
        public BoolResponse Connected([DefaultValue(0)]int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = DeviceManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new BoolResponse(ClientTransactionID, TransactionID, DeviceManager.GetSwitch(DeviceNumber).Connected);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<BoolResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/Connected")]
        public Response Connected([DefaultValue(0)]int DeviceNumber, [FromForm] bool Connected, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = DeviceManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                if (Connected || !ServerSettings.PreventRemoteDisconnects)
                {
                    DeviceManager.GetSwitch(DeviceNumber).Connected = Connected;
                }

                return new Response() { ClientTransactionID = ClientTransactionID, ServerTransactionID = TransactionID };
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<Response>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/Description")]
        public StringResponse Description([DefaultValue(0)]int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = DeviceManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new StringResponse(ClientTransactionID, TransactionID, DeviceManager.GetSwitch(DeviceNumber).Description);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<StringResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/DriverInfo")]
        public StringResponse DriverInfo([DefaultValue(0)]int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = DeviceManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new StringResponse(ClientTransactionID, TransactionID, DeviceManager.GetSwitch(DeviceNumber).DriverInfo);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<StringResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/DriverVersion")]
        public StringResponse DriverVersion([DefaultValue(0)]int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = DeviceManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new StringResponse(ClientTransactionID, TransactionID, DeviceManager.GetSwitch(DeviceNumber).DriverVersion);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<StringResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/InterfaceVersion")]
        public IntResponse InterfaceVersion([DefaultValue(0)]int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = DeviceManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new IntResponse(ClientTransactionID, TransactionID, DeviceManager.GetSwitch(DeviceNumber).InterfaceVersion);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<IntResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/Name")]
        public StringResponse Name([DefaultValue(0)]int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = DeviceManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new StringResponse(ClientTransactionID, TransactionID, DeviceManager.GetSwitch(DeviceNumber).Name);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<StringResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/SupportedActions")]
        public StringListResponse SupportedActions([DefaultValue(0)]int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = DeviceManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new StringListResponse(ClientTransactionID, TransactionID, new List<string>(DeviceManager.GetSwitch(DeviceNumber).SupportedActions.Cast<string>().ToList()));
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<StringListResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        #region IDisposable Members

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/Dispose")]
        public Response Dispose([DefaultValue(0)]int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = DeviceManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                if (ServerSettings.PreventRemoteDisposes)
                {
                    DeviceManager.GetSwitch(DeviceNumber).Dispose();
                }

                return new Response() { ClientTransactionID = ClientTransactionID, ServerTransactionID = TransactionID };
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<Response>(ex, ClientTransactionID, TransactionID);
            }
        }

        #endregion IDisposable Members

        #endregion Common Methods

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/maxswitch")]
        public IntResponse MaxSwitch([DefaultValue(0)]int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = DeviceManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new IntResponse(ClientTransactionID, TransactionID, DeviceManager.GetSwitch(DeviceNumber).MaxSwitch);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<IntResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/canwrite")]
        public BoolResponse CanWrite([DefaultValue(0)]int DeviceNumber, short id, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = DeviceManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new BoolResponse(ClientTransactionID, TransactionID, DeviceManager.GetSwitch(DeviceNumber).CanWrite(id));
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<BoolResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/getswitch")]
        public BoolResponse GetSwitch([DefaultValue(0)]int DeviceNumber, short id, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = DeviceManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new BoolResponse(ClientTransactionID, TransactionID, DeviceManager.GetSwitch(DeviceNumber).GetSwitch(id));
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<BoolResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/getswitchdescription")]
        public StringResponse GetSwitchDescription([DefaultValue(0)]int DeviceNumber, short id, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = DeviceManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new StringResponse(ClientTransactionID, TransactionID, DeviceManager.GetSwitch(DeviceNumber).GetSwitchDescription(id));
            }
            catch (Exception ex)
            {
                var temp = ResponseHelpers.ExceptionResponseBuilder<StringResponse>(ex, ClientTransactionID, TransactionID);
                temp.Value = string.Empty;
                return temp;
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/getswitchname")]
        public StringResponse GetSwitchName([DefaultValue(0)]int DeviceNumber, short id, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = DeviceManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new StringResponse(ClientTransactionID, TransactionID, DeviceManager.GetSwitch(DeviceNumber).GetSwitchName(id));
            }
            catch (Exception ex)
            {
                var temp = ResponseHelpers.ExceptionResponseBuilder<StringResponse>(ex, ClientTransactionID, TransactionID);
                temp.Value = string.Empty;
                return temp;
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/getswitchvalue")]
        public DoubleResponse GetSwitchValue([DefaultValue(0)]int DeviceNumber, short id, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = DeviceManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new DoubleResponse(ClientTransactionID, TransactionID, DeviceManager.GetSwitch(DeviceNumber).GetSwitchValue(id));
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<DoubleResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/minswitchvalue")]
        public DoubleResponse MinSwitchValue([DefaultValue(0)]int DeviceNumber, short id, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = DeviceManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new DoubleResponse(ClientTransactionID, TransactionID, DeviceManager.GetSwitch(DeviceNumber).MinSwitchValue(id));
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<DoubleResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/maxswitchvalue")]
        public DoubleResponse MaxSwitchValue([DefaultValue(0)]int DeviceNumber, short id, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = DeviceManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new DoubleResponse(ClientTransactionID, TransactionID, DeviceManager.GetSwitch(DeviceNumber).MaxSwitchValue(id));
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<DoubleResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/switchstep")]
        public DoubleResponse SwitchStep([DefaultValue(0)]int DeviceNumber, short id, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = DeviceManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new DoubleResponse(ClientTransactionID, TransactionID, DeviceManager.GetSwitch(DeviceNumber).SwitchStep(id));
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<DoubleResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/setswitch")]
        public Response SetSwitch([DefaultValue(0)]int DeviceNumber, [FromForm] short id, [FromForm] bool State, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = DeviceManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);

                DeviceManager.GetSwitch(DeviceNumber).SetSwitch(id, State);

                return new Response() { ClientTransactionID = ClientTransactionID, ServerTransactionID = TransactionID };
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<Response>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/setswitchname")]
        public Response SetSwitchName([DefaultValue(0)]int DeviceNumber, [FromForm] short id, [FromForm] string name, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = DeviceManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);

                DeviceManager.GetSwitch(DeviceNumber).SetSwitchName(id, name);

                return new Response() { ClientTransactionID = ClientTransactionID, ServerTransactionID = TransactionID };
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<Response>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/setswitchvalue")]
        public Response SetSwitchValue([DefaultValue(0)]int DeviceNumber, [FromForm] short id, [FromForm] double value, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = DeviceManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);

                DeviceManager.GetSwitch(DeviceNumber).SetSwitchValue(id, value);

                return new Response() { ClientTransactionID = ClientTransactionID, ServerTransactionID = TransactionID };
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<Response>(ex, ClientTransactionID, TransactionID);
            }
        }
    }
}
