using ASCOM.Alpaca.Responses;
using ASCOM.Standard.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ASCOM.Alpaca.Simulators
{
    [ServiceFilter(typeof(AuthorizationFilter))]
    [ApiController]
    public class ManagementController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("management/apiversions")]
        public IntListResponse ApiVersions(int ClientID = -1, uint ClientTransactionID = 0)
        {
            var TransactionID = ServerManager.ServerTransactionID;
            Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);
            return new IntListResponse(ClientTransactionID, TransactionID, ServerSettings.APIVersions);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("management/v1/description")]
        public AlpacaDescriptionResponse Description(int ClientID = -1, uint ClientTransactionID = 0)
        {


            var TransactionID = ServerManager.ServerTransactionID;
            Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);

            return new AlpacaDescriptionResponse(ClientTransactionID, TransactionID, new AlpacaDeviceDescription(ServerSettings.ServerName, ServerSettings.Manufacturer, ServerSettings.Version, ServerSettings.Location));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("management/v1/configureddevices")]
        public AlpacaConfiguredDevicesResponse ConfiguredDevices(int ClientID = -1, uint ClientTransactionID = 0)
        {
            List<AlpacaConfiguredDevice> devices = new List<AlpacaConfiguredDevice>();
            try
            {
                    devices.Add((DeviceManager.GetCoverCalibrator(0) as IAlpacaDevice).Configuration);
            }
            catch(Exception ex)
            {
                Logging.LogError(ex.Message);
            }

            var TransactionID = ServerManager.ServerTransactionID;
            Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);

            return new AlpacaConfiguredDevicesResponse(ClientTransactionID, TransactionID, devices);
        }
    }
}