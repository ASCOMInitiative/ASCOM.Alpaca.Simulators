using ASCOM.Alpaca.Responses;
using ASCOM.Standard.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ASCOM.Alpaca.Simulators
{
    [ServiceFilter(typeof(AuthorizationFilter))]
    [ApiController]
    public class SafetyMonitorController : Controller
    {
        private const string APIRoot = "api/v1/safetymonitor/";

        #region Common Methods

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/Action")]
        public StringResponse Action(int DeviceNumber, [FromForm] string Action, [FromForm] string Parameters, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new StringResponse(ClientTransactionID, TransactionID, DeviceManager.GetSafetyMonitor(DeviceNumber).Action(Action, Parameters));
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<StringResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/CommandBlind")]
        public Response CommandBlind(int DeviceNumber, [FromForm] string Command, [FromForm] bool Raw = false, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                DeviceManager.GetSafetyMonitor(DeviceNumber).CommandBlind(Command, Raw);
                return new Response() { ClientTransactionID = ClientTransactionID, ServerTransactionID = TransactionID };
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<Response>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/CommandBool")]
        public BoolResponse CommandBool(int DeviceNumber, [FromForm] string Command, [FromForm] bool Raw = false, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new BoolResponse(ClientTransactionID, TransactionID, DeviceManager.GetSafetyMonitor(DeviceNumber).CommandBool(Command, Raw));
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<BoolResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/CommandString")]
        public StringResponse CommandString(int DeviceNumber, [FromForm] string Command, [FromForm] bool Raw = false, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new StringResponse(ClientTransactionID, TransactionID, DeviceManager.GetSafetyMonitor(DeviceNumber).CommandString(Command, Raw));
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<StringResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/Connected")]
        public BoolResponse Connected(int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new BoolResponse(ClientTransactionID, TransactionID, DeviceManager.GetSafetyMonitor(DeviceNumber).Connected);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<BoolResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/Connected")]
        public Response Connected(int DeviceNumber, [FromForm] bool Connected, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                if (Connected || !ServerSettings.PreventRemoteDisconnects)
                {
                    DeviceManager.GetSafetyMonitor(DeviceNumber).Connected = Connected;
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
        public StringResponse Description(int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new StringResponse(ClientTransactionID, TransactionID, DeviceManager.GetSafetyMonitor(DeviceNumber).Description);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<StringResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/DriverInfo")]
        public StringResponse DriverInfo(int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new StringResponse(ClientTransactionID, TransactionID, DeviceManager.GetSafetyMonitor(DeviceNumber).DriverInfo);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<StringResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/DriverVersion")]
        public StringResponse DriverVersion(int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new StringResponse(ClientTransactionID, TransactionID, DeviceManager.GetSafetyMonitor(DeviceNumber).DriverVersion);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<StringResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/InterfaceVersion")]
        public IntResponse InterfaceVersion(int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new IntResponse(ClientTransactionID, TransactionID, DeviceManager.GetSafetyMonitor(DeviceNumber).InterfaceVersion);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<IntResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/Name")]
        public StringResponse Name(int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new StringResponse(ClientTransactionID, TransactionID, DeviceManager.GetSafetyMonitor(DeviceNumber).Name);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<StringResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/SupportedActions")]
        public StringListResponse SupportedActions(int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new StringListResponse(ClientTransactionID, TransactionID, new List<string>(DeviceManager.GetSafetyMonitor(DeviceNumber).SupportedActions));
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<StringListResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        #region IDisposable Members

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/Dispose")]
        public Response Dispose(int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                if (!ServerSettings.PreventRemoteDisposes)
                {
                    DeviceManager.GetSafetyMonitor(DeviceNumber).Dispose();
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
        [Route(APIRoot + "{DeviceNumber}/issafe")]
        public BoolResponse IsSafe(int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new BoolResponse(ClientTransactionID, TransactionID, DeviceManager.GetSafetyMonitor(DeviceNumber).IsSafe);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<BoolResponse>(ex, ClientTransactionID, TransactionID);
            }
        }
    }
}
