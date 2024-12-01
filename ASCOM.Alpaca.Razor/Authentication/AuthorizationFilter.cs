using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net.Http.Headers;
using System.Text;

namespace ASCOM.Alpaca
{
    public class AuthorizationFilter : IAuthorizationFilter
    {
        public string BasicRealm { get; set; }

        private IUserService userService;

        public AuthorizationFilter(IUserService _userService)
        {
            userService = _userService;
        }

        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            // If not using Auth don't even check
            if (!userService.UseAuth)
            {
                return;
            }

            // Allow Anonymous endpoints without check
            var endpoint = filterContext.HttpContext.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
                return;

            // Check if client has an Asp.Net core auth token or cookie
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                return;
            }

            // Check for Authorization header and allow through if correct
            var req = filterContext.HttpContext.Request;
            var auth = req.Headers["Authorization"];
            if (!string.IsNullOrEmpty(auth))
            {
                var authHeader = AuthenticationHeaderValue.Parse(auth);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                var username = credentials[0];
                var password = credentials[1];

                if (userService.Authenticate(username, password).Result)
                {
                    return;
                }
            }

            // Auth failed, block request
            filterContext.HttpContext.Response.Headers.Append("WWW-Authenticate", string.Format("Basic realm=\"{0}\"", BasicRealm ?? "Alpaca"));
            filterContext.Result = new UnauthorizedResult();
        }
    }
}