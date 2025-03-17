using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ASCOM.Alpaca
{
    [ApiExplorerSettings(GroupName = "OmniSim")]
    public class LoginController : ControllerBase
    {
        IUserService _userService;

        public LoginController(IUserService service)
        {
            _userService = service;
        }

        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> Login([FromForm] string username, [FromForm] string password, string url = "/")
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch
            {
            }

            bool auth = false;

            try
            {
                if (_userService.UseAuth)
                {
                    auth = await _userService.Authenticate(username, password);
                }
            }
            catch
            {
                return Redirect(url);
            }

            if (!auth)
                return Redirect(url);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username ?? "Default"),
            };

            var claimsIdentity = new ClaimsIdentity(

                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                RedirectUri = this.Request.Host.Value
            };

            try
            {
                await HttpContext.SignInAsync(

                CookieAuthenticationDefaults.AuthenticationScheme,

                new ClaimsPrincipal(claimsIdentity),

                authProperties);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex.Message);
            }

            return Redirect(url);
        }

        [HttpGet]
        [Route("/logout")]
        public async Task<IActionResult> Logout(string returnUrl = "/")
        {
            await HttpContext

                .SignOutAsync(

                CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect(returnUrl);
        }
    }
}