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
    public class FilterWheelController
    {
        private const string APIRoot = "api/v1/filterwheel/";

        #region Common Methods

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/Action")]
        public StringResponse Action(int DeviceNumber, [FromForm] string Action, [FromForm] string Parameters, [FromForm] int ClientID = -1, [FromForm] uint ClientTransactionID = 0)
        {
            try
            {
                return new StringResponse(ClientTransactionID, ServerManager.ServerTransactionID, DeviceManager.GetFilterWheel(DeviceNumber).Action(Action, Parameters));
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
            try
            {
                DeviceManager.GetFilterWheel(DeviceNumber).CommandBlind(Command, Raw);
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
            try
            {
                return new BoolResponse(ClientTransactionID, ServerManager.ServerTransactionID, DeviceManager.GetFilterWheel(DeviceNumber).CommandBool(Command, Raw));
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
            try
            {
                return new StringResponse(ClientTransactionID, ServerManager.ServerTransactionID, DeviceManager.GetFilterWheel(DeviceNumber).CommandString(Command, Raw));
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
            try
            {
                return new BoolResponse(ClientTransactionID, ServerManager.ServerTransactionID, DeviceManager.GetFilterWheel(DeviceNumber).Connected);
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
            try
            {
                if (Connected || !ServerSettings.PreventRemoteDisconnects)
                {
                    DeviceManager.GetFilterWheel(DeviceNumber).Connected = Connected;
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
            try
            {
                return new StringResponse(ClientTransactionID, ServerManager.ServerTransactionID, DeviceManager.GetFilterWheel(DeviceNumber).Description);
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
            try
            {
                return new StringResponse(ClientTransactionID, ServerManager.ServerTransactionID, DeviceManager.GetFilterWheel(DeviceNumber).DriverInfo);
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
            try
            {
                return new StringResponse(ClientTransactionID, ServerManager.ServerTransactionID, DeviceManager.GetFilterWheel(DeviceNumber).DriverVersion);
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
            try
            {
                return new IntResponse(ClientTransactionID, ServerManager.ServerTransactionID, DeviceManager.GetFilterWheel(DeviceNumber).InterfaceVersion);
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
            try
            {
                return new StringResponse(ClientTransactionID, ServerManager.ServerTransactionID, DeviceManager.GetFilterWheel(DeviceNumber).Name);
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
            try
            {
                return new StringListResponse(ClientTransactionID, ServerManager.ServerTransactionID, new List<string>(DeviceManager.GetFilterWheel(DeviceNumber).SupportedActions.Cast<string>().ToList()));
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
            try
            {
                if (!ServerSettings.PreventRemoteDisposes)
                {
                    DeviceManager.GetFilterWheel(DeviceNumber).Dispose();
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
        [Route(APIRoot + "{DeviceNumber}/focusoffsets")]
        public IntListResponse FocusOffsets(int DeviceNumber, int ClientID = -1, uint ClientTransactionID = 0)
        {
            try
            {
                return new IntListResponse(ClientTransactionID, ServerManager.ServerTransactionID, DeviceManager.GetFilterWheel(DeviceNumber).FocusOffsets);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<IntListResponse>(ex, ClientTransactionID, ServerManager.ServerTransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/names")]
        public StringListResponse Names(int DeviceNumber, int ClientID = -1, uint ClientTransactionID = 0)
        {
            try
            {
                return new StringListResponse(ClientTransactionID, ServerManager.ServerTransactionID, DeviceManager.GetFilterWheel(DeviceNumber).Names);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<StringListResponse>(ex, ClientTransactionID, ServerManager.ServerTransactionID);
            }
        }

        [HttpGet]
        [Route(APIRoot + "{DeviceNumber}/position")]
        public IntResponse Position(int DeviceNumber, int ClientID = -1, uint ClientTransactionID = 0)
        {
            try
            {
                return new IntResponse(ClientTransactionID, ServerManager.ServerTransactionID, DeviceManager.GetFilterWheel(DeviceNumber).Position);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<IntResponse>(ex, ClientTransactionID, ServerManager.ServerTransactionID);
            }
        }

        [HttpPut]
        [Route(APIRoot + "{DeviceNumber}/position")]
        public Response Position(int DeviceNumber, [FromForm] short position, [FromForm] int ClientID = -1, [FromForm] uint ClientTransactionID = 0)
        {
            try
            {

                DeviceManager.GetFilterWheel(DeviceNumber).Position = position;

                return new Response() { ClientTransactionID = ClientTransactionID, ServerTransactionID = ServerManager.ServerTransactionID };
            }
            catch (Exception ex)
            {
                return ResponseHelpers.ExceptionResponseBuilder<Response>(ex, ClientTransactionID, ServerManager.ServerTransactionID);
            }
        }
    }
}
