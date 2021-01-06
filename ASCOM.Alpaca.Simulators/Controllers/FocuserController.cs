using ASCOM.Alpaca.Responses;
using ASCOM.Standard.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASCOM.Alpaca.Simulators
{
    [ServiceFilter(typeof(AuthorizationFilter))]
    [ApiController]
    public class FocuserController : Controller
    {
        private const string APIRoot = "api/v1/focuser/";

        #region Common Methods

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/Action")]
        public StringResponse Action(int DeviceNumber, [FromForm] string Action, [FromForm] string Parameters, [FromForm] int ClientID = -1, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new StringResponse(ClientTransactionID, ServerManager.ServerTransactionID, DeviceManager.GetFocuser(DeviceNumber).Action(Action, Parameters));
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<StringResponse>(ex, ClientTransactionID, ServerManager.ServerTransactionID);
            }
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/CommandBlind")]
        public Response CommandBlind(int DeviceNumber, [FromForm] string Command, [FromForm] bool Raw = false, [FromForm] int ClientID = -1, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                DeviceManager.GetFocuser(DeviceNumber).CommandBlind(Command, Raw);
                return new Response() { ClientTransactionID = ClientTransactionID, ServerTransactionID = ServerManager.ServerTransactionID };
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<Response>(ex, ClientTransactionID, ServerManager.ServerTransactionID);
            }
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/CommandBool")]
        public BoolResponse CommandBool(int DeviceNumber, [FromForm] string Command, [FromForm] bool Raw = false, [FromForm] int ClientID = -1, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new BoolResponse(ClientTransactionID, ServerManager.ServerTransactionID, DeviceManager.GetFocuser(DeviceNumber).CommandBool(Command, Raw));
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<BoolResponse>(ex, ClientTransactionID, ServerManager.ServerTransactionID);
            }
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/CommandString")]
        public StringResponse CommandString(int DeviceNumber, [FromForm] string Command, [FromForm] bool Raw = false, [FromForm] int ClientID = -1, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new StringResponse(ClientTransactionID, ServerManager.ServerTransactionID, DeviceManager.GetFocuser(DeviceNumber).CommandString(Command, Raw));
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<StringResponse>(ex, ClientTransactionID, ServerManager.ServerTransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/Connected")]
        public BoolResponse Connected(int DeviceNumber, int ClientID = -1, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new BoolResponse(ClientTransactionID, ServerManager.ServerTransactionID, DeviceManager.GetFocuser(DeviceNumber).Connected);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<BoolResponse>(ex, ClientTransactionID, ServerManager.ServerTransactionID);
            }
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/Connected")]
        public Response Connected(int DeviceNumber, [FromForm] bool Connected, [FromForm] int ClientID = -1, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                if (Connected || !ServerSettings.PreventRemoteDisconnects)
                {
                    DeviceManager.GetFocuser(DeviceNumber).Connected = Connected;
                }

                return new Response() { ClientTransactionID = ClientTransactionID, ServerTransactionID = ServerManager.ServerTransactionID };
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<Response>(ex, ClientTransactionID, ServerManager.ServerTransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/Description")]
        public StringResponse Description(int DeviceNumber, int ClientID = -1, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new StringResponse(ClientTransactionID, ServerManager.ServerTransactionID, DeviceManager.GetFocuser(DeviceNumber).Description);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<StringResponse>(ex, ClientTransactionID, ServerManager.ServerTransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/DriverInfo")]
        public StringResponse DriverInfo(int DeviceNumber, int ClientID = -1, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new StringResponse(ClientTransactionID, ServerManager.ServerTransactionID, DeviceManager.GetFocuser(DeviceNumber).DriverInfo);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<StringResponse>(ex, ClientTransactionID, ServerManager.ServerTransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/DriverVersion")]
        public StringResponse DriverVersion(int DeviceNumber, int ClientID = -1, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new StringResponse(ClientTransactionID, ServerManager.ServerTransactionID, DeviceManager.GetFocuser(DeviceNumber).DriverVersion);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<StringResponse>(ex, ClientTransactionID, ServerManager.ServerTransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/InterfaceVersion")]
        public IntResponse InterfaceVersion(int DeviceNumber, int ClientID = -1, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new IntResponse(ClientTransactionID, ServerManager.ServerTransactionID, DeviceManager.GetFocuser(DeviceNumber).InterfaceVersion);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<IntResponse>(ex, ClientTransactionID, ServerManager.ServerTransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/Name")]
        public StringResponse Name(int DeviceNumber, int ClientID = -1, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new StringResponse(ClientTransactionID, ServerManager.ServerTransactionID, DeviceManager.GetFocuser(DeviceNumber).Name);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<StringResponse>(ex, ClientTransactionID, ServerManager.ServerTransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/SupportedActions")]
        public StringListResponse SupportedActions(int DeviceNumber, int ClientID = -1, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new StringListResponse(ClientTransactionID, ServerManager.ServerTransactionID, new List<string>(DeviceManager.GetFocuser(DeviceNumber).SupportedActions.Cast<string>().ToList()));
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<StringListResponse>(ex, ClientTransactionID, ServerManager.ServerTransactionID);
            }
        }

        #region IDisposable Members

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/Dispose")]
        public Response Dispose(int DeviceNumber, [FromForm] int ClientID = -1, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                if (!ServerSettings.PreventRemoteDisposes)
                {
                    DeviceManager.GetFocuser(DeviceNumber).Dispose();
                }

                return new Response() { ClientTransactionID = ClientTransactionID, ServerTransactionID = ServerManager.ServerTransactionID };
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<Response>(ex, ClientTransactionID, ServerManager.ServerTransactionID);
            }
        }

        #endregion IDisposable Members

        #endregion Common Methods

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/Absolute")]
        public BoolResponse Absolute(int DeviceNumber, int ClientID = -1, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new BoolResponse(ClientTransactionID, ServerManager.ServerTransactionID, DeviceManager.GetFocuser(DeviceNumber).Absolute);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<BoolResponse>(ex, ClientTransactionID, ServerManager.ServerTransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/IsMoving")]
        public BoolResponse IsMoving(int DeviceNumber, int ClientID = -1, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new BoolResponse(ClientTransactionID, ServerManager.ServerTransactionID, DeviceManager.GetFocuser(DeviceNumber).IsMoving);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<BoolResponse>(ex, ClientTransactionID, ServerManager.ServerTransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/Link")]
        public BoolResponse Link(int DeviceNumber, int ClientID = -1, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new BoolResponse(ClientTransactionID, ServerManager.ServerTransactionID, DeviceManager.GetFocuser(DeviceNumber).Link);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<BoolResponse>(ex, ClientTransactionID, ServerManager.ServerTransactionID);
            }
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/Link")]
        public Response Link(int DeviceNumber, [FromForm] bool Link, [FromForm] int ClientID = -1, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);

                if (Link || !ServerSettings.PreventRemoteDisconnects)
                {
                    DeviceManager.GetFocuser(DeviceNumber).Link = Link;
                }

                return new Response() { ClientTransactionID = ClientTransactionID, ServerTransactionID = ServerManager.ServerTransactionID };
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<Response>(ex, ClientTransactionID, ServerManager.ServerTransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/MaxIncrement")]
        public IntResponse MaxIncrement(int DeviceNumber, int ClientID = -1, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new IntResponse(ClientTransactionID, ServerManager.ServerTransactionID, DeviceManager.GetFocuser(DeviceNumber).MaxIncrement);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<IntResponse>(ex, ClientTransactionID, ServerManager.ServerTransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/MaxStep")]
        public IntResponse MaxStep(int DeviceNumber, int ClientID = -1, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new IntResponse(ClientTransactionID, ServerManager.ServerTransactionID, DeviceManager.GetFocuser(DeviceNumber).MaxStep);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<IntResponse>(ex, ClientTransactionID, ServerManager.ServerTransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/Position")]
        public IntResponse Position(int DeviceNumber, int ClientID = -1, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new IntResponse(ClientTransactionID, ServerManager.ServerTransactionID, DeviceManager.GetFocuser(DeviceNumber).Position);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<IntResponse>(ex, ClientTransactionID, ServerManager.ServerTransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/StepSize")]
        public DoubleResponse StepSize(int DeviceNumber, int ClientID = -1, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new DoubleResponse(ClientTransactionID, ServerManager.ServerTransactionID, DeviceManager.GetFocuser(DeviceNumber).StepSize);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<DoubleResponse>(ex, ClientTransactionID, ServerManager.ServerTransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/TempComp")]
        public BoolResponse TempComp(int DeviceNumber, int ClientID = -1, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new BoolResponse(ClientTransactionID, ServerManager.ServerTransactionID, DeviceManager.GetFocuser(DeviceNumber).TempComp);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<BoolResponse>(ex, ClientTransactionID, ServerManager.ServerTransactionID);
            }
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/TempComp")]
        public Response TempComp(int DeviceNumber, [FromForm] bool TempComp, [FromForm] int ClientID = -1, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                DeviceManager.GetFocuser(DeviceNumber).TempComp = TempComp;

                return new Response() { ClientTransactionID = ClientTransactionID, ServerTransactionID = ServerManager.ServerTransactionID };
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<Response>(ex, ClientTransactionID, ServerManager.ServerTransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/TempCompAvailable")]
        public BoolResponse TempCompAvailable(int DeviceNumber, int ClientID = -1, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new BoolResponse(ClientTransactionID, ServerManager.ServerTransactionID, DeviceManager.GetFocuser(DeviceNumber).TempCompAvailable);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<BoolResponse>(ex, ClientTransactionID, ServerManager.ServerTransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/Temperature")]
        public DoubleResponse Temperature(int DeviceNumber, int ClientID = -1, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new DoubleResponse(ClientTransactionID, ServerManager.ServerTransactionID, DeviceManager.GetFocuser(DeviceNumber).Temperature);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<DoubleResponse>(ex, ClientTransactionID, ServerManager.ServerTransactionID);
            }
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/Halt")]
        public Response Halt(int DeviceNumber, [FromForm] int ClientID = -1, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                DeviceManager.GetFocuser(DeviceNumber).Halt();

                return new Response() { ClientTransactionID = ClientTransactionID, ServerTransactionID = ServerManager.ServerTransactionID };
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<Response>(ex, ClientTransactionID, ServerManager.ServerTransactionID);
            }
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/Move")]
        public Response Move(int DeviceNumber, [FromForm] int Position, [FromForm] int ClientID = -1, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                DeviceManager.GetFocuser(DeviceNumber).Move(Position);

                return new Response() { ClientTransactionID = ClientTransactionID, ServerTransactionID = ServerManager.ServerTransactionID };
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<Response>(ex, ClientTransactionID, ServerManager.ServerTransactionID);
            }
        }
    }
}