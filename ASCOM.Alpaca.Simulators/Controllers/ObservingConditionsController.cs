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
    public class ObservingConditionsController : Controller
    {
        private const string APIRoot = "api/v1/observingconditions/";

        #region Common Methods

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/Action")]
        public StringResponse Action([DefaultValue(0)]int DeviceNumber, [FromForm] string Action, [FromForm] string Parameters, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new StringResponse(ClientTransactionID, TransactionID, DeviceManager.GetObservingConditions(DeviceNumber).Action(Action, Parameters));
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
                DeviceManager.GetObservingConditions(DeviceNumber).CommandBlind(Command, Raw);
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
                return new BoolResponse(ClientTransactionID, TransactionID, DeviceManager.GetObservingConditions(DeviceNumber).CommandBool(Command, Raw));
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
                return new StringResponse(ClientTransactionID, TransactionID, DeviceManager.GetObservingConditions(DeviceNumber).CommandString(Command, Raw));
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
                return new BoolResponse(ClientTransactionID, TransactionID, DeviceManager.GetObservingConditions(DeviceNumber).Connected);
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
                    DeviceManager.GetObservingConditions(DeviceNumber).Connected = Connected;
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
                return new StringResponse(ClientTransactionID, TransactionID, DeviceManager.GetObservingConditions(DeviceNumber).Description);
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
                return new StringResponse(ClientTransactionID, TransactionID, DeviceManager.GetObservingConditions(DeviceNumber).DriverInfo);
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
                return new StringResponse(ClientTransactionID, TransactionID, DeviceManager.GetObservingConditions(DeviceNumber).DriverVersion);
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
                return new IntResponse(ClientTransactionID, TransactionID, DeviceManager.GetObservingConditions(DeviceNumber).InterfaceVersion);
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
                return new StringResponse(ClientTransactionID, TransactionID, DeviceManager.GetObservingConditions(DeviceNumber).Name);
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
                return new StringListResponse(ClientTransactionID, TransactionID, new List<string>(DeviceManager.GetObservingConditions(DeviceNumber).SupportedActions.Cast<string>().ToList()));
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
                    DeviceManager.GetObservingConditions(DeviceNumber).Dispose();
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
        [Route(APIRoot + "{DeviceNumber}/averageperiod")]
        public DoubleResponse AveragePeriod([DefaultValue(0)]int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new DoubleResponse(ClientTransactionID, TransactionID, DeviceManager.GetObservingConditions(DeviceNumber).AveragePeriod);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<DoubleResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/averageperiod")]
        public Response AveragePeriod([DefaultValue(0)]int DeviceNumber, [FromForm] double averageperiod, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);

                DeviceManager.GetObservingConditions(DeviceNumber).AveragePeriod = averageperiod;

                return new Response() { ClientTransactionID = ClientTransactionID, ServerTransactionID = TransactionID };
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<Response>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/cloudcover")]
        public DoubleResponse CloudCover([DefaultValue(0)]int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new DoubleResponse(ClientTransactionID, TransactionID, DeviceManager.GetObservingConditions(DeviceNumber).CloudCover);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<DoubleResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/dewpoint")]
        public DoubleResponse DewPoint([DefaultValue(0)]int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new DoubleResponse(ClientTransactionID, TransactionID, DeviceManager.GetObservingConditions(DeviceNumber).DewPoint);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<DoubleResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/humidity")]
        public DoubleResponse Humidity([DefaultValue(0)]int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new DoubleResponse(ClientTransactionID, TransactionID, DeviceManager.GetObservingConditions(DeviceNumber).Humidity);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<DoubleResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/pressure")]
        public DoubleResponse Pressure([DefaultValue(0)]int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new DoubleResponse(ClientTransactionID, TransactionID, DeviceManager.GetObservingConditions(DeviceNumber).Pressure);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<DoubleResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/rainrate")]
        public DoubleResponse RainRate([DefaultValue(0)]int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new DoubleResponse(ClientTransactionID, TransactionID, DeviceManager.GetObservingConditions(DeviceNumber).RainRate);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<DoubleResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/skybrightness")]
        public DoubleResponse SkyBrightness([DefaultValue(0)]int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new DoubleResponse(ClientTransactionID, TransactionID, DeviceManager.GetObservingConditions(DeviceNumber).SkyBrightness);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<DoubleResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/skyquality")]
        public DoubleResponse SkyQuality([DefaultValue(0)]int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new DoubleResponse(ClientTransactionID, TransactionID, DeviceManager.GetObservingConditions(DeviceNumber).SkyQuality);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<DoubleResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/skytemperature")]
        public DoubleResponse SkyTemperature([DefaultValue(0)]int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new DoubleResponse(ClientTransactionID, TransactionID, DeviceManager.GetObservingConditions(DeviceNumber).SkyTemperature);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<DoubleResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/starfwhm")]
        public DoubleResponse StarFWHM([DefaultValue(0)]int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new DoubleResponse(ClientTransactionID, TransactionID, DeviceManager.GetObservingConditions(DeviceNumber).StarFWHM);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<DoubleResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/temperature")]
        public DoubleResponse Temperature([DefaultValue(0)]int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new DoubleResponse(ClientTransactionID, TransactionID, DeviceManager.GetObservingConditions(DeviceNumber).Temperature);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<DoubleResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/winddirection")]
        public DoubleResponse WindDirection([DefaultValue(0)]int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new DoubleResponse(ClientTransactionID, TransactionID, DeviceManager.GetObservingConditions(DeviceNumber).WindDirection);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<DoubleResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/windgust")]
        public DoubleResponse WindGust([DefaultValue(0)]int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new DoubleResponse(ClientTransactionID, TransactionID, DeviceManager.GetObservingConditions(DeviceNumber).WindGust);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<DoubleResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/windspeed")]
        public DoubleResponse WindSpeed([DefaultValue(0)]int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new DoubleResponse(ClientTransactionID, TransactionID, DeviceManager.GetObservingConditions(DeviceNumber).WindSpeed);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<DoubleResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/refresh")]
        public Response Refresh([DefaultValue(0)]int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);

                DeviceManager.GetObservingConditions(DeviceNumber).Refresh();

                return new Response() { ClientTransactionID = ClientTransactionID, ServerTransactionID = TransactionID };
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<Response>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/sensordescription")]
        public StringResponse SensorDescription([DefaultValue(0)]int DeviceNumber, string sensorname, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new StringResponse(ClientTransactionID, TransactionID, DeviceManager.GetObservingConditions(DeviceNumber).SensorDescription(sensorname));
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<StringResponse>(ex, ClientTransactionID, TransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/timesincelastupdate")]
        public DoubleResponse TimeSinceLastUpdate([DefaultValue(0)]int DeviceNumber, string sensorname, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
                return new DoubleResponse(ClientTransactionID, TransactionID, DeviceManager.GetObservingConditions(DeviceNumber).TimeSinceLastUpdate(sensorname));
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<DoubleResponse>(ex, ClientTransactionID, TransactionID);
            }
        }
    }
}
