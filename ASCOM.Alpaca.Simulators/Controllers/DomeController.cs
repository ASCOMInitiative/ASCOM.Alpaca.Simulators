using ASCOM.Alpaca.Responses;
using ASCOM.Standard.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace ASCOM.Alpaca.Simulators
{
    [ServiceFilter(typeof(AuthorizationFilter))]
    [ApiController]
    public class DomeController : Controller
    {
        private const string APIRoot = "api/v1/dome/";

        #region Common Methods

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/Action")]
        public StringResponse Action([DefaultValue(0)]int DeviceNumber, [FromForm] string Action, [FromForm] string Parameters, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new StringResponse(ClientTransactionID, TransactionID, DeviceManager.GetDome(DeviceNumber).Action(Action, Parameters));
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<StringResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/CommandBlind")]
        public Response CommandBlind([DefaultValue(0)]int DeviceNumber, [FromForm] string Command, [FromForm] bool Raw = false, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                DeviceManager.GetDome(DeviceNumber).CommandBlind(Command, Raw);
                return new Response() { ClientTransactionID = ClientTransactionID, ServerTransactionID = TransactionID };
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<Response>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/CommandBool")]
        public BoolResponse CommandBool([DefaultValue(0)]int DeviceNumber, [FromForm] string Command, [FromForm] bool Raw = false, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new BoolResponse(ClientTransactionID, TransactionID, DeviceManager.GetDome(DeviceNumber).CommandBool(Command, Raw));
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<BoolResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/CommandString")]
        public StringResponse CommandString([DefaultValue(0)]int DeviceNumber, [FromForm] string Command, [FromForm] bool Raw = false, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new StringResponse(ClientTransactionID, TransactionID, DeviceManager.GetDome(DeviceNumber).CommandString(Command, Raw));
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
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new BoolResponse(ClientTransactionID, TransactionID, DeviceManager.GetDome(DeviceNumber).Connected);
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
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                if (Connected || !ServerSettings.PreventRemoteDisconnects)
                {
                    DeviceManager.GetDome(DeviceNumber).Connected = Connected;
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
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new StringResponse(ClientTransactionID, TransactionID, DeviceManager.GetDome(DeviceNumber).Description);
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
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new StringResponse(ClientTransactionID, TransactionID, DeviceManager.GetDome(DeviceNumber).DriverInfo);
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
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new StringResponse(ClientTransactionID, TransactionID, DeviceManager.GetDome(DeviceNumber).DriverVersion);
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
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new IntResponse(ClientTransactionID, TransactionID, DeviceManager.GetDome(DeviceNumber).InterfaceVersion);
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
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new StringResponse(ClientTransactionID, TransactionID, DeviceManager.GetDome(DeviceNumber).Name);
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
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new StringListResponse(ClientTransactionID, TransactionID, new List<string>(DeviceManager.GetDome(DeviceNumber).SupportedActions.Cast<string>().ToList()));
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
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                if (ServerSettings.PreventRemoteDisposes)
                {
                    DeviceManager.GetDome(DeviceNumber).Dispose();
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
        [Route(APIRoot + "{DeviceNumber}/Altitude")]
        public DoubleResponse Altitude([DefaultValue(0)]int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new DoubleResponse(ClientTransactionID, TransactionID, DeviceManager.GetDome(DeviceNumber).Altitude);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<DoubleResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/AtHome")]
        public BoolResponse AtHome([DefaultValue(0)]int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new BoolResponse(ClientTransactionID, TransactionID, DeviceManager.GetDome(DeviceNumber).AtHome);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<BoolResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/AtPark")]
        public BoolResponse AtPark([DefaultValue(0)]int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new BoolResponse(ClientTransactionID, TransactionID, DeviceManager.GetDome(DeviceNumber).AtPark);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<BoolResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/Azimuth")]
        public DoubleResponse Azimuth([DefaultValue(0)]int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new DoubleResponse(ClientTransactionID, TransactionID, DeviceManager.GetDome(DeviceNumber).Azimuth);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<DoubleResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/CanFindHome")]
        public BoolResponse CanFindHome([DefaultValue(0)]int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new BoolResponse(ClientTransactionID, TransactionID, DeviceManager.GetDome(DeviceNumber).CanFindHome);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<BoolResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/CanPark")]
        public BoolResponse CanPark([DefaultValue(0)]int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new BoolResponse(ClientTransactionID, TransactionID, DeviceManager.GetDome(DeviceNumber).CanPark);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<BoolResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/CanSetAltitude")]
        public BoolResponse CanSetAltitude([DefaultValue(0)]int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new BoolResponse(ClientTransactionID, TransactionID, DeviceManager.GetDome(DeviceNumber).CanSetAltitude);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<BoolResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/CanSetAzimuth")]
        public BoolResponse CanSetAzimuth([DefaultValue(0)]int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new BoolResponse(ClientTransactionID, TransactionID, DeviceManager.GetDome(DeviceNumber).CanSetAzimuth);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<BoolResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/CanSetPark")]
        public BoolResponse CanSetPark([DefaultValue(0)]int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new BoolResponse(ClientTransactionID, TransactionID, DeviceManager.GetDome(DeviceNumber).CanSetPark);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<BoolResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/CanSetShutter")]
        public BoolResponse CanSetShutter([DefaultValue(0)]int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new BoolResponse(ClientTransactionID, TransactionID, DeviceManager.GetDome(DeviceNumber).CanSetShutter);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<BoolResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/CanSlave")]
        public BoolResponse CanSlave([DefaultValue(0)]int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new BoolResponse(ClientTransactionID, TransactionID, DeviceManager.GetDome(DeviceNumber).CanSlave);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<BoolResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/CanSyncAzimuth")]
        public BoolResponse CanSyncAzimuth([DefaultValue(0)]int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new BoolResponse(ClientTransactionID, TransactionID, DeviceManager.GetDome(DeviceNumber).CanSyncAzimuth);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<BoolResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/ShutterStatus")]
        public ShutterStateResponse ShutterStatus([DefaultValue(0)]int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new ShutterStateResponse(ClientTransactionID, TransactionID, DeviceManager.GetDome(DeviceNumber).ShutterStatus);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<ShutterStateResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/Slaved")]
        public BoolResponse Slaved([DefaultValue(0)]int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new BoolResponse(ClientTransactionID, TransactionID, DeviceManager.GetDome(DeviceNumber).Slaved);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<BoolResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/Slaved")]
        public Response Slaved([DefaultValue(0)]int DeviceNumber, [FromForm] bool Slaved, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);

                DeviceManager.GetDome(DeviceNumber).Slaved = Slaved;

                return new Response() { ClientTransactionID = ClientTransactionID, ServerTransactionID = TransactionID };
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<Response>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/Slewing")]
        public BoolResponse Slewing([DefaultValue(0)]int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new BoolResponse(ClientTransactionID, TransactionID, DeviceManager.GetDome(DeviceNumber).Slewing);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<BoolResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/AbortSlew")]
        public Response AbortSlew([DefaultValue(0)]int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);

                DeviceManager.GetDome(DeviceNumber).AbortSlew();

                return new Response() { ClientTransactionID = ClientTransactionID, ServerTransactionID = TransactionID };
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<Response>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/CloseShutter")]
        public Response CloseShutter([DefaultValue(0)]int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);

                DeviceManager.GetDome(DeviceNumber).CloseShutter();

                return new Response() { ClientTransactionID = ClientTransactionID, ServerTransactionID = TransactionID };
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<Response>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/FindHome")]
        public Response FindHome([DefaultValue(0)]int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);

                DeviceManager.GetDome(DeviceNumber).FindHome();

                return new Response() { ClientTransactionID = ClientTransactionID, ServerTransactionID = TransactionID };
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<Response>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/OpenShutter")]
        public Response OpenShutter([DefaultValue(0)]int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);

                DeviceManager.GetDome(DeviceNumber).OpenShutter();

                return new Response() { ClientTransactionID = ClientTransactionID, ServerTransactionID = TransactionID };
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<Response>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/Park")]
        public Response Park([DefaultValue(0)]int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);

                DeviceManager.GetDome(DeviceNumber).Park();

                return new Response() { ClientTransactionID = ClientTransactionID, ServerTransactionID = TransactionID };
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<Response>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/SetPark")]
        public Response SetPark([DefaultValue(0)]int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);

                DeviceManager.GetDome(DeviceNumber).SetPark();

                return new Response() { ClientTransactionID = ClientTransactionID, ServerTransactionID = TransactionID };
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<Response>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/SlewToAltitude")]
        public Response SlewToAltitude([DefaultValue(0)]int DeviceNumber, [FromForm] double Altitude, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);

                DeviceManager.GetDome(DeviceNumber).SlewToAltitude(Altitude);

                return new Response() { ClientTransactionID = ClientTransactionID, ServerTransactionID = TransactionID };
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<Response>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/SlewToAzimuth")]
        public Response SlewToAzimuth([DefaultValue(0)]int DeviceNumber, [FromForm] double Azimuth, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);

                DeviceManager.GetDome(DeviceNumber).SlewToAzimuth(Azimuth);

                return new Response() { ClientTransactionID = ClientTransactionID, ServerTransactionID = TransactionID };
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<Response>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/SyncToAzimuth")]
        public Response SyncToAzimuth([DefaultValue(0)]int DeviceNumber, [FromForm] double Azimuth, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);

                DeviceManager.GetDome(DeviceNumber).SyncToAzimuth(Azimuth);

                return new Response() { ClientTransactionID = ClientTransactionID, ServerTransactionID = TransactionID };
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<Response>(ex, ClientTransactionID, TransactionID);
            }
        }
    }
}
