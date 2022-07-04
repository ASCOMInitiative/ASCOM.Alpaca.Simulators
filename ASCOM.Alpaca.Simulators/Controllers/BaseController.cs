using Alpaca;
using ASCOM.Common;
using ASCOM.Common.Alpaca;
using ASCOM.Common.DeviceInterfaces;
using ASCOM.Common.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ASCOM.Alpaca.Simulators
{
    public class ProcessBaseController : Controller
    {
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
        internal ActionResult<BoolResponse> ProcessRequest(Func<bool> Operation, uint TransactionID, uint ClientID, uint ClientTransactionID, string Payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, Payload);

                if (ServerSettings.RequireStrictURLCase)
                {
                    if (HttpContext.Request.Path.ToString().Any(char.IsUpper))
                    {
                        return BadRequest(Strings.URLCapitalizationDescription);
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

        internal ActionResult<DriveRatesResponse> ProcessRequest(Func<ITrackingRates> Request, uint TransactionID, uint ClientID = 0, uint ClientTransactionID = 0, string Payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, Payload);

                if (ServerSettings.RequireStrictURLCase)
                {
                    if (HttpContext.Request.Path.ToString().Any(char.IsUpper))
                    {
                        return BadRequest(Strings.URLCapitalizationDescription);
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

        internal ActionResult<DoubleResponse> ProcessRequest(Func<double> Request, uint TransactionID, uint ClientID = 0, uint ClientTransactionID = 0, string Payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, Payload);

                if (ServerSettings.RequireStrictURLCase)
                {
                    if (HttpContext.Request.Path.ToString().Any(char.IsUpper))
                    {
                        return BadRequest(Strings.URLCapitalizationDescription);
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

        internal ActionResult<AxisRatesResponse> ProcessRequest(Func<IAxisRates> Request, uint TransactionID, uint ClientID = 0, uint ClientTransactionID = 0, string Payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, Payload);

                if (ServerSettings.RequireStrictURLCase)
                {
                    if (HttpContext.Request.Path.ToString().Any(char.IsUpper))
                    {
                        return BadRequest(Strings.URLCapitalizationDescription);
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

        internal ActionResult<IntArray2DResponse> ProcessRequest(Func<int[,]> Request, uint TransactionID, uint ClientID = 0, uint ClientTransactionID = 0, string Payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, Payload);

                if (ServerSettings.RequireStrictURLCase)
                {
                    if (HttpContext.Request.Path.ToString().Any(char.IsUpper))
                    {
                        return BadRequest(Strings.URLCapitalizationDescription);
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

        internal ActionResult<StringListResponse> ProcessRequest(Func<IList<string>> Request, uint TransactionID, uint ClientID = 0, uint ClientTransactionID = 0, string Payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, Payload);

                if (ServerSettings.RequireStrictURLCase)
                {
                    if (HttpContext.Request.Path.ToString().Any(char.IsUpper))
                    {
                        return BadRequest(Strings.URLCapitalizationDescription);
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

        internal ActionResult<IntListResponse> ProcessRequest(Func<IList<int>> Request, uint TransactionID, uint ClientID = 0, uint ClientTransactionID = 0, string Payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, Payload);

                if (ServerSettings.RequireStrictURLCase)
                {
                    if (HttpContext.Request.Path.ToString().Any(char.IsUpper))
                    {
                        return BadRequest(Strings.URLCapitalizationDescription);
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

        internal ActionResult<IntResponse> ProcessRequest(Func<int> Request, uint TransactionID, uint ClientID = 0, uint ClientTransactionID = 0, string Payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, Payload);

                if (ServerSettings.RequireStrictURLCase)
                {
                    if (HttpContext.Request.Path.ToString().Any(char.IsUpper))
                    {
                        return BadRequest(Strings.URLCapitalizationDescription);
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

        internal ActionResult<StringResponse> ProcessRequest(Func<string> Request, uint TransactionID, uint ClientID = 0, uint ClientTransactionID = 0, string Payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, Payload);

                if (ServerSettings.RequireStrictURLCase)
                {
                    if (HttpContext.Request.Path.ToString().Any(char.IsUpper))
                    {
                        return BadRequest(Strings.URLCapitalizationDescription);
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

        internal ActionResult<Response> ProcessRequest(Action Request, uint TransactionID, uint ClientID = 0, uint ClientTransactionID = 0, string Payload = "")
        {
            try
            {
                LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID, Payload);

                if (ServerSettings.RequireStrictURLCase)
                {
                    if (HttpContext.Request.Path.ToString().Any(char.IsUpper))
                    {
                        return BadRequest(Strings.URLCapitalizationDescription);
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
                Logging.Log.LogVerbose($"Transaction: {transactionID} - {remoteIpAddress} ({clientID}, {clientTransactionID}) requested {request}");
            }
            else
            {
                Logging.Log.LogVerbose($"Transaction: {transactionID} - {remoteIpAddress} ({clientID}, {clientTransactionID}) requested {request} with payload {payload}");
            }
        }
    }
}
