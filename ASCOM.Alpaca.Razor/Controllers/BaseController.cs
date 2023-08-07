using ASCOM.Alpaca.Razor;
using ASCOM.Common;
using ASCOM.Common.Alpaca;
using ASCOM.Common.DeviceInterfaces;
using ASCOM.Common.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ASCOM.Alpaca
{
    public class ProcessBaseController : Controller
    {
        internal bool BadRequestAlpacaProtocol(out BadRequestObjectResult Result, ref uint ClientID, ref uint ClientTransactionID)
        {
            Result = null;
            //Only check on Alpaca routes, all others may pass
            if (!HttpContext.Request.Path.ToString().Contains("api/"))
            {
                return false;
            }

            if (HttpContext.Request.Path.ToString().Any(char.IsUpper))
            {
                Result = BadRequest(Strings.URLCapitalizationDescription + HttpContext.Request.Path.ToString());
                Logging.LogError($"Error on request {HttpContext.Request.Path} with details: {Result.Value?.ToString()}");
                return true;
            }

            if (HttpContext.Request.HasFormContentType)
            {
                foreach(var key in HttpContext.Request.Form.Keys)
                {
                    var Validator = ValidAlpacaKeys.AlpacaFormValidators.FirstOrDefault(x => x.ExternalKeyFailsValidation(key), null);

                    if(Validator != null)
                    {
                        Logging.LogWarning($"Incorrect capitalization on optional key {Validator.Key}, received {key}");
                        //We zero out optional keys
                        if (Validator.IsOptional)
                        {
                            if (Validator.Key == "ClientID")
                            {
                                ClientID = 0;
                            }
                            else if (Validator.Key == "ClientTransactionID")
                            {
                                ClientTransactionID = 0;
                            }
                        }
                        else
                        {
                            Result = BadRequest(Strings.FormCapitalizationDescription + $"{Validator.Key}, received: {key}");
                            Logging.LogError($"Error on request {HttpContext.Request.Path} with details: {Result.Value?.ToString()}");
                            return true;
                        }
                    }
                }

                if (HttpContext.Request.Query.Any())
                {
                    var keys = HttpContext.Request.Query.Keys;
                    Result = BadRequest(Strings.FormWithQueryDescription + string.Join(", ", keys));
                    Logging.LogError($"Error on request {HttpContext.Request.Path} with details: {Result.Value?.ToString()}");
                    return true;
                }
            }

            if (HttpContext.Request.Method == "GET" && HttpContext.Request.HasFormContentType && HttpContext.Request.Form.Keys.Any())
            {
                var keys = HttpContext.Request.Form.Keys;
                Result = BadRequest(Strings.QueryWithFormDescription + string.Join(", ", keys));
                Logging.LogError($"Error on request {HttpContext.Request.Path} with details: {Result.Value?.ToString()}");
                return true;
            }

            return false;
        }

        /// <summary>
        /// This function logs the incoming API call then executes the passed function
        /// By executing the function this can catch any errors
        /// If it completes successfully a bool response is returned with an http 200
        /// If no device is available an HTTP 400 is returned
        /// If the device call fails an Alpaca JSON error is returned
        /// </summary>
        /// <param name="Operation">The operation to preform on the device. Often this is just a lambda that returns a property. By passing it in as a function it can be executed inside a try catch and handle the exception.</param>
        /// <param name="TransactionID">The current server transaction id</param>
        /// <param name="ClientID">The client id</param>
        /// <param name="ClientTransactionID">The client transaction id</param>
        /// <param name="Payload">Any payload values, optional, only used for logging</param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult<BoolResponse> ProcessRequest(Func<bool> Operation, uint TransactionID, uint ClientID, uint ClientTransactionID, string Payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, Payload);

                if (DeviceManager.Configuration.RunInStrictAlpacaMode)
                {
                    if (BadRequestAlpacaProtocol(out BadRequestObjectResult result, ref ClientID, ref ClientTransactionID))
                    {
                        return result;
                    }
                }

                return Ok(new BoolResponse()
                {
                    ClientTransactionID = ClientTransactionID,
                    ServerTransactionID = TransactionID,
                    Value = Operation.Invoke()
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

        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult<DriveRatesResponse> ProcessRequest(Func<ITrackingRates> Request, uint TransactionID, uint ClientID = 0, uint ClientTransactionID = 0, string Payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, Payload);

                if (DeviceManager.Configuration.RunInStrictAlpacaMode)
                {
                    if (BadRequestAlpacaProtocol(out BadRequestObjectResult result, ref ClientID, ref ClientTransactionID))
                    {
                        return result;
                    }
                }

                var rates = Request.Invoke();

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

        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult<DoubleResponse> ProcessRequest(Func<double> Request, uint TransactionID, uint ClientID = 0, uint ClientTransactionID = 0, string Payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, Payload);

                if (DeviceManager.Configuration.RunInStrictAlpacaMode)
                {
                    if (BadRequestAlpacaProtocol(out BadRequestObjectResult result, ref ClientID, ref ClientTransactionID))
                    {
                        return result;
                    }
                }

                return Ok(new DoubleResponse()
                {
                    ClientTransactionID = ClientTransactionID,
                    ServerTransactionID = TransactionID,
                    Value = Request.Invoke()
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

        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult<AxisRatesResponse> ProcessRequest(Func<IAxisRates> Request, uint TransactionID, uint ClientID = 0, uint ClientTransactionID = 0, string Payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, Payload);

                if (DeviceManager.Configuration.RunInStrictAlpacaMode)
                {
                    if (BadRequestAlpacaProtocol(out BadRequestObjectResult result, ref ClientID, ref ClientTransactionID))
                    {
                        return result;
                    }
                }

                var rates = Request.Invoke();

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

        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult<IntArray2DResponse> ProcessRequest(Func<int[,]> Request, uint TransactionID, uint ClientID = 0, uint ClientTransactionID = 0, string Payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, Payload);

                if (DeviceManager.Configuration.RunInStrictAlpacaMode)
                {
                    if (BadRequestAlpacaProtocol(out BadRequestObjectResult result, ref ClientID, ref ClientTransactionID))
                    {
                        return result;
                    }
                }

                return Ok(new IntArray2DResponse()
                {
                    ClientTransactionID = ClientTransactionID,
                    ServerTransactionID = TransactionID,
                    Value = Request.Invoke()
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

        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult<StringListResponse> ProcessRequest(Func<IList<string>> Request, uint TransactionID, uint ClientID = 0, uint ClientTransactionID = 0, string Payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, Payload);

                if (DeviceManager.Configuration.RunInStrictAlpacaMode)
                {
                    if (BadRequestAlpacaProtocol(out BadRequestObjectResult result, ref ClientID, ref ClientTransactionID))
                    {
                        return result;
                    }
                }

                return Ok(new StringListResponse()
                {
                    ClientTransactionID = ClientTransactionID,
                    ServerTransactionID = TransactionID,
                    Value = Request.Invoke()
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

        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult<IntListResponse> ProcessRequest(Func<IList<int>> Request, uint TransactionID, uint ClientID = 0, uint ClientTransactionID = 0, string Payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, Payload);

                if (DeviceManager.Configuration.RunInStrictAlpacaMode)
                {
                    if (BadRequestAlpacaProtocol(out BadRequestObjectResult result, ref ClientID, ref ClientTransactionID))
                    {
                        return result;
                    }
                }

                return Ok(new IntListResponse()
                {
                    ClientTransactionID = ClientTransactionID,
                    ServerTransactionID = TransactionID,
                    Value = Request.Invoke()
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

        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult<IntResponse> ProcessRequest(Func<int> Request, uint TransactionID, uint ClientID = 0, uint ClientTransactionID = 0, string Payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, Payload);

                if (DeviceManager.Configuration.RunInStrictAlpacaMode)
                {
                    if (BadRequestAlpacaProtocol(out BadRequestObjectResult result, ref ClientID, ref ClientTransactionID))
                    {
                        return result;
                    }
                }

                return Ok(new IntResponse()
                {
                    ClientTransactionID = ClientTransactionID,
                    ServerTransactionID = TransactionID,
                    Value = Request.Invoke()
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

        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult<StringResponse> ProcessRequest(Func<string> Request, uint TransactionID, uint ClientID = 0, uint ClientTransactionID = 0, string Payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, Payload);

                if (DeviceManager.Configuration.RunInStrictAlpacaMode)
                {
                    if (BadRequestAlpacaProtocol(out BadRequestObjectResult result, ref ClientID, ref ClientTransactionID))
                    {
                        return result;
                    }
                }

                return Ok(new StringResponse()
                {
                    ClientTransactionID = ClientTransactionID,
                    ServerTransactionID = TransactionID,
                    Value = Request.Invoke()
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

        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult<Response> ProcessRequest(Action Request, uint TransactionID, uint ClientID = 0, uint ClientTransactionID = 0, string Payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, Payload);

                if (DeviceManager.Configuration.RunInStrictAlpacaMode)
                {
                    if (BadRequestAlpacaProtocol(out BadRequestObjectResult result, ref ClientID, ref ClientTransactionID))
                    {
                        return result;
                    }
                }

                Request.Invoke();
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

        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult<DeviceStateResponse> ProcessRequest(Func<IList<IStateValue>> Request, uint TransactionID, uint ClientID = 0, uint ClientTransactionID = 0, string Payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, Payload);

                if (DeviceManager.Configuration.RunInStrictAlpacaMode)
                {
                    if (BadRequestAlpacaProtocol(out BadRequestObjectResult result, ref ClientID, ref ClientTransactionID))
                    {
                        return result;
                    }
                }

                return Ok(new DeviceStateResponse()
                {
                    ClientTransactionID = ClientTransactionID,
                    ServerTransactionID = TransactionID,
                    Value = Request.Invoke()
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
                Logging.LogVerbose($"Transaction: {transactionID} - {remoteIpAddress} ({clientID}, {clientTransactionID}) requested {request}");
            }
            else
            {
                Logging.LogVerbose($"Transaction: {transactionID} - {remoteIpAddress} ({clientID}, {clientTransactionID}) requested {request} with payload {payload}");
            }
        }
    }
}