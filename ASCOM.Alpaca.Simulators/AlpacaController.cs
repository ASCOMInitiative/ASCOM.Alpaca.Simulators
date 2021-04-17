using ASCOM.Alpaca.Responses;
using ASCOM.Standard.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Alpaca
{
    /// <summary>
    /// A Custom REST MVC controller with additions to help handle Alpaca responses and error responses.
    /// </summary>
    public class AlpacaController : Controller
    {
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
                ASCOM.Standard.Utilities.Logger.LogVerbose($"Transaction: {transactionID} - {remoteIpAddress} ({clientID}, {clientTransactionID}) requested {request}");
            }
            else
            {
                ASCOM.Standard.Utilities.Logger.LogVerbose($"Transaction: {transactionID} - {remoteIpAddress} ({clientID}, {clientTransactionID}) requested {request} with payload {payload}");
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
