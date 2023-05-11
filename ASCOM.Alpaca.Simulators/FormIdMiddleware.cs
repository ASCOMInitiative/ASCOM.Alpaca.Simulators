namespace ASCOM.Alpaca.Simulators
{
    using ASCOM.Common;
    using ASCOM.Common.Alpaca;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Middleware to ignore mis-cased ClientTransactionID parameters
    /// </summary>
    /// <remarks>Removing mis-cased ClientTransactionID parameters from the key list ensures that the ClientTransactionID parameter is zero in the response.
    /// Removing mis-cased ClientID parameters from the key list ensures that a 200 OK response is returned instead of a 400 Bad Request.
    /// </remarks>
    internal class FormIdMiddleware
    {
        private readonly RequestDelegate _next;

        public FormIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Run the middleware code
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            // Only process requests that have form parameters
            if (context.Request.HasFormContentType)
            {
                // Check whether the ClientID or ClientTransactionID parameters are mis-cased
                if ((context.Request.Form.Keys.Any(key => !(key == AlpacaConstants.CLIENTID_PARAMETER_NAME) && (key.ToLowerInvariant() == AlpacaConstants.CLIENTID_PARAMETER_NAME.ToLowerInvariant()))) |
                    (context.Request.Form.Keys.Any(key => !(key == AlpacaConstants.CLIENTTRANSACTION_PARAMETER_NAME) && (key.ToLowerInvariant() == AlpacaConstants.CLIENTTRANSACTION_PARAMETER_NAME.ToLowerInvariant()))))
                {
                    // At least one of the two parameters is mis-cased so remove it/them from the key list

                    // Create a holder for the sub-setted parameter list
                    var formDictionary = new Dictionary<string, StringValues>();

                    // Remove the mis-cased parameters by copying all keys except for the bad ones
                    foreach (string key in context.Request.Form.Keys)
                    {
                        // Ignore a badly cased ClientID key
                        if ((!(key == AlpacaConstants.CLIENTID_PARAMETER_NAME)) & (key.ToLowerInvariant() == AlpacaConstants.CLIENTID_PARAMETER_NAME.ToLowerInvariant()))
                        {
                            // Ignore the key and log a warning
                            Logging.Log.LogWarning($"{context.Request.Path} Found incorrectly cased {AlpacaConstants.CLIENTID_PARAMETER_NAME} form parameter: {key}, ignoring it");
                            continue;
                        }

                        // Ignore a badly cased ClientTransactionID key
                        if ((!(key == AlpacaConstants.CLIENTTRANSACTION_PARAMETER_NAME)) & (key.ToLowerInvariant() == AlpacaConstants.CLIENTTRANSACTION_PARAMETER_NAME.ToLowerInvariant()))
                        {
                            // Ignore the key and log a warning
                            Logging.Log.LogWarning($"{context.Request.Path} Found incorrectly cased {AlpacaConstants.CLIENTTRANSACTION_PARAMETER_NAME} form parameter: {key}, ignoring it");
                            continue;
                        }

                        // The key is not a badly cased ClientID or ClientTransactionID parameter name so add it to the list of keys
                        context.Request.Form.TryGetValue(key, out StringValues formValues);
                        formDictionary.Add(key, formValues);
                    }

                    // Create a new form parameter collection from the sub-setted list of keys
                    FormCollection formCollection = new(formDictionary);

                    // Replace the original parameter collection with the new version without the bad ID parameters
                    context.Request.Form = formCollection;

                    // List the new parameter list to the debug log
                    foreach (string key in context.Request.Form.Keys)
                    {
                        Logging.Log.LogDebug($"{context.Request.Path} Revised key list includes: {key}");
                    }
                }
            }

            // Call the next delegate/middleware in the pipeline.
            await _next(context);
        }
    }
}
