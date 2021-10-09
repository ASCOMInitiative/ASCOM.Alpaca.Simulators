using ASCOM.Common.Alpaca;
using ASCOM.Alpaca.Simulators;
using ASCOM.Common.DeviceInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using ASCOM.Common.Helpers;
using ASCOM.Tools;

namespace Alpaca
{
    /// <summary>
    /// A Custom REST MVC controller with additions to help handle Alpaca responses and error responses.
    /// This contains the Alpaca endpoints that are common to all devices.
    /// </summary>
    public abstract class AlpacaController : Controller
    {
        public abstract IAscomDevice GetDevice(int DeviceNumber);
        #region Common Methods

        [HttpPut]
        [Route("{DeviceNumber}/action")]
        public ActionResult<StringResponse> Action([Required][DefaultValue(0)] int DeviceNumber, [Required][FromForm] string Action, [FromForm] string Parameters = "", [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => GetDevice(DeviceNumber).Action(Action, Parameters), DeviceManager.ServerTransactionID, (uint)ClientID, (uint)ClientTransactionID, $"Action: {Action}, Parameters {Parameters}");
        }

        [HttpPut]
        [Route("{DeviceNumber}/commandblind")]
        public ActionResult<Response> CommandBlind([DefaultValue(0)] int DeviceNumber, [Required][FromForm] string Command, [FromForm] bool Raw = false, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => GetDevice(DeviceNumber).CommandBlind(Command, Raw), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Command {Command}, Raw {Raw}");
        }

        [HttpPut]
        [Route("{DeviceNumber}/commandbool")]
        public ActionResult<BoolResponse> CommandBool([DefaultValue(0)] int DeviceNumber, [Required][FromForm] string Command, [FromForm] bool Raw = false, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => GetDevice(DeviceNumber).CommandBool(Command, Raw), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Command={Command}, Raw={Raw}");
        }

        [HttpPut]
        [Route("{DeviceNumber}/commandstring")]
        public ActionResult<StringResponse> CommandString([DefaultValue(0)] int DeviceNumber, [Required][FromForm] string Command, [FromForm] bool Raw = false, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => GetDevice(DeviceNumber).CommandString(Command, Raw), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Command={Command}, Raw={Raw}");
        }

        [HttpGet]
        [Route("{DeviceNumber}/connected")]
        public ActionResult<BoolResponse> Connected([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => GetDevice(DeviceNumber).Connected, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/connected")]
        public ActionResult<Response> Connected([DefaultValue(0)] int DeviceNumber, [Required][FromForm] bool Connected, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            if (Connected || !ServerSettings.PreventRemoteDisconnects)
            {
                return ProcessRequest(() => { GetDevice(DeviceNumber).Connected = Connected; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Connected={Connected}");
            }
            return ProcessRequest(() => { }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/description")]
        public ActionResult<StringResponse> Description([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => GetDevice(DeviceNumber).Description, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/driverinfo")]
        public ActionResult<StringResponse> DriverInfo([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => GetDevice(DeviceNumber).DriverInfo, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/driverversion")]
        public ActionResult<StringResponse> DriverVersion([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => GetDevice(DeviceNumber).DriverVersion, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/interfaceversion")]
        public ActionResult<IntResponse> InterfaceVersion([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => GetDevice(DeviceNumber).InterfaceVersion, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/name")]
        public ActionResult<StringResponse> Name([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => GetDevice(DeviceNumber).Name, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/supportedactions")]
        public ActionResult<StringListResponse> SupportedActions([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => GetDevice(DeviceNumber).SupportedActions, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        #region IDisposable Members

        [HttpPut]
        [Route("{DeviceNumber}/dispose")]
        public ActionResult<Response> Dispose([DefaultValue(0)] int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            if (!ServerSettings.PreventRemoteDisposes)
            {
                return ProcessRequest(() => { GetDevice(DeviceNumber).Dispose(); }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
            }
            return ProcessRequest(() => { }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        #endregion IDisposable Members

        #endregion Common Methods

        /// <summary>
        /// This function logs the incoming API call then executes the passed function
        /// By executing the function this can catch any errors
        /// If it completes successfully a bool response is returned with an http 200
        /// If no device is available an HTTP 400 is returned
        /// If the device call fails an Alpaca JSON error is returned
        /// </summary>
        /// <param name="operation">The operation to preform on the device. Often this is just a lambda that returns a property. By passing it in as a function it can be executed inside a try catch and handle the exception.</param>
        /// <param name="TransactionID">The current server transaction id</param>
        /// <param name="ClientID">The client id</param>
        /// <param name="ClientTransactionID">The client transaction id</param>
        /// <param name="payload">Any payload values, optional, only used for logging</param>
        /// <returns></returns>
        internal ActionResult<BoolResponse> ProcessRequest(Func<bool> operation, uint TransactionID, uint ClientID, uint ClientTransactionID, string payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, payload);

                return Ok(new BoolResponse()
                {
                    ClientTransactionID = ClientTransactionID,
                    ServerTransactionID = TransactionID,
                    Value = operation.Invoke()
                });
            }
            catch (DeviceNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Ok(ResponseHelpers.ExceptionResponseBuilder<BoolResponse>(ex, ClientTransactionID, TransactionID));
            }
        }

        internal ActionResult<DriveRatesResponse> ProcessRequest(Func<ITrackingRates> p, uint TransactionID, uint ClientID = 0, uint ClientTransactionID = 0, string payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, payload);

                var rates = p.Invoke();

                IList<DriveRate> res = new List<DriveRate>();

                foreach (DriveRate rate in rates)
                {
                    res.Add(rate);
                }

                return Ok(new DriveRatesResponse()
                {
                    ClientTransactionID = ClientTransactionID,
                    ServerTransactionID = TransactionID,
                    Value = res
                });
            }
            catch (DeviceNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Ok(ResponseHelpers.ExceptionResponseBuilder<DriveRatesResponse>(ex, ClientTransactionID, TransactionID));
            }
        }

        internal ActionResult<DoubleResponse> ProcessRequest(Func<double> p, uint TransactionID, uint ClientID = 0, uint ClientTransactionID = 0, string payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, payload);

                return Ok(new DoubleResponse()
                {
                    ClientTransactionID = ClientTransactionID,
                    ServerTransactionID = TransactionID,
                    Value = p.Invoke()
                });
            }
            catch (DeviceNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Ok(ResponseHelpers.ExceptionResponseBuilder<DoubleResponse>(ex, ClientTransactionID, TransactionID));
            }
        }

        internal ActionResult<AxisRatesResponse> ProcessRequest(Func<IAxisRates> p, uint TransactionID, uint ClientID = 0, uint ClientTransactionID = 0, string payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, payload);

                var rates = p.Invoke();

                IList<AxisRate> res = new List<AxisRate>();

                foreach (IRate rate in rates)
                {
                    res.Add(new AxisRate(rate.Minimum, rate.Maximum));
                }

                return Ok(new AxisRatesResponse()
                {
                    ClientTransactionID = ClientTransactionID,
                    ServerTransactionID = TransactionID,
                    Value = res
                });
            }
            catch (DeviceNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Ok(ResponseHelpers.ExceptionResponseBuilder<AxisRatesResponse>(ex, ClientTransactionID, TransactionID));
            }
        }

        internal ActionResult<IntArray2DResponse> ProcessRequest(Func<int[,]> p, uint TransactionID, uint ClientID = 0, uint ClientTransactionID = 0, string payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, payload);

                return Ok(new IntArray2DResponse()
                {
                    ClientTransactionID = ClientTransactionID,
                    ServerTransactionID = TransactionID,
                    Value = p.Invoke()
                });
            }
            catch (DeviceNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Ok(ResponseHelpers.ExceptionResponseBuilder<IntArray2DResponse>(ex, ClientTransactionID, TransactionID));
            }
        }

        internal ActionResult<StringListResponse> ProcessRequest(Func<IList<string>> p, uint TransactionID, uint ClientID = 0, uint ClientTransactionID = 0, string payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, payload);

                return Ok(new StringListResponse()
                {
                    ClientTransactionID = ClientTransactionID,
                    ServerTransactionID = TransactionID,
                    Value = p.Invoke()
                });
            }
            catch (DeviceNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Ok(ResponseHelpers.ExceptionResponseBuilder<StringListResponse>(ex, ClientTransactionID, TransactionID));
            }
        }

        internal ActionResult<IntListResponse> ProcessRequest(Func<IList<int>> p, uint TransactionID, uint ClientID = 0, uint ClientTransactionID = 0, string payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, payload);

                return Ok(new IntListResponse()
                {
                    ClientTransactionID = ClientTransactionID,
                    ServerTransactionID = TransactionID,
                    Value = p.Invoke()
                });
            }
            catch (DeviceNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Ok(ResponseHelpers.ExceptionResponseBuilder<IntListResponse>(ex, ClientTransactionID, TransactionID));
            }
        }

        internal ActionResult<IntResponse> ProcessRequest(Func<int> p, uint TransactionID, uint ClientID = 0, uint ClientTransactionID = 0, string payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, payload);

                return Ok(new IntResponse()
                {
                    ClientTransactionID = ClientTransactionID,
                    ServerTransactionID = TransactionID,
                    Value = p.Invoke()
                });
            }
            catch (DeviceNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Ok(ResponseHelpers.ExceptionResponseBuilder<IntResponse>(ex, ClientTransactionID, TransactionID));
            }
        }

        internal ActionResult<StringResponse> ProcessRequest(Func<string> p, uint TransactionID, uint ClientID = 0, uint ClientTransactionID = 0, string payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, payload);

                return Ok(new StringResponse()
                {
                    ClientTransactionID = ClientTransactionID,
                    ServerTransactionID = TransactionID,
                    Value = p.Invoke()
                });
            }
            catch (DeviceNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Ok(ResponseHelpers.ExceptionResponseBuilder<StringResponse>(ex, ClientTransactionID, TransactionID));
            }
        }

        internal ActionResult<Response> ProcessRequest(Action p, uint TransactionID, uint ClientID = 0, uint ClientTransactionID = 0, string payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, payload);
                p.Invoke();
                return Ok(new Response() { ClientTransactionID = ClientTransactionID, ServerTransactionID = TransactionID });
            }
            catch (DeviceNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Ok(ResponseHelpers.ExceptionResponseBuilder<Response>(ex, ClientTransactionID, TransactionID));
            }
        }

        /// <summary>
        /// Log out an API request to the ASCOM Standard Logger Instance. This logs at a level of Verbose.
        /// </summary>
        /// <param name="remoteIpAddress">The IP Address of the remote computer</param>
        /// <param name="request">The requested API</param>
        /// <param name="clientID">The Client ID</param>
        /// <param name="clientTransactionID">The Client Transaction ID</param>
        /// <param name="transactionID">The Server Transaction ID</param>
        /// <param name="payload">The function payload if any exists</param>
        private static void LogAPICall(IPAddress remoteIpAddress, string request, uint clientID, uint clientTransactionID, uint transactionID, string payload = "")
        {
            if (payload == null || payload == string.Empty)
            {
                Logger.LogVerbose($"Transaction: {transactionID} - {remoteIpAddress} ({clientID}, {clientTransactionID}) requested {request}");
            }
            else
            {
                Logger.LogVerbose($"Transaction: {transactionID} - {remoteIpAddress} ({clientID}, {clientTransactionID}) requested {request} with payload {payload}");
            }
        }
    }

    internal class DeviceNotFoundException : Exception
    {
        internal DeviceNotFoundException(string message) : base(message)
        {
        }
    }
}