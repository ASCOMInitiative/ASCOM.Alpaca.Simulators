// Ignore Spelling: Behavoir

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ASCOM.Alpaca.Razor
{
    [UnsupportedOSPlatform("browser")]
    public static class StartupHelpers
    {
        public static void ConfigureSwagger(IServiceCollection services, string host_xml_file)
        {
            if (DeviceManager.Configuration.RunSwagger)
            {
                services.AddSwaggerGen(c =>
                {
                    //This allows for an interesting camera image function but has some issues
                    //This is off for now
                    //c.UseOneOfForPolymorphism();

                    c.SwaggerDoc("Alpaca", new OpenApiInfo { Title = $"{DeviceManager.Configuration.ServerName}", Description = "The Alpaca JSON API Specification. You can find the Alpaca HTML Specification and the OmniSim only configuration specification in the drop down. Please note that the Alpaca API documentation on the ASCOM website is the canonical version. There are several issues with this auto generated version that will be resolved in future versions. This is currently provided only for testing.", Version = "v1" });
                    c.SwaggerDoc("AlpacaSetup", new OpenApiInfo { Title = $"{DeviceManager.Configuration.ServerName}", Description = "Alpaca HTML Setup API - These are used to give the end user a GUI to configure device specific settings.", Version = "v1" });
                    c.SwaggerDoc("OmniSim", new OpenApiInfo { Title = "OmniSim JSON API", Description="API configuration that is unique to the OmniSim. These are not part of the Alpaca Spec but are helpful to automate testing with the OmniSim.", Version = "v1" });


                    if (File.Exists(host_xml_file))
                    {
                        c.IncludeXmlComments(host_xml_file);
                    }
                    else
                    {
                        Logging.LogWarning("Failed to find simulator xml documentation.");
                    }

                    if (File.Exists(Path.Combine(AppContext.BaseDirectory, "ASCOM.Common.xml")))
                    {
                        string xmlPath = Path.Combine(AppContext.BaseDirectory, "ASCOM.Common.xml");

                        c.IncludeXmlComments(xmlPath);
                    }
                    else
                    {
                        Logging.LogWarning("Failed to find ASCOM.Common xml documentation.");
                    }

                    if (File.Exists(Path.Combine(AppContext.BaseDirectory, "ASCOM.Alpaca.Razor.xml")))
                    {
                        string xmlPath = Path.Combine(AppContext.BaseDirectory, "ASCOM.Alpaca.Razor.xml");

                        c.IncludeXmlComments(xmlPath);
                    }
                    else
                    {
                        Logging.LogWarning("Failed to find ASCOM.Alpaca.Razor xml documentation.");
                    }

                    c.EnableAnnotations();
                    c.SchemaFilter<SwaggerExcludeFilter>();
                    c.MapType<uint>(() => new OpenApiSchema { Type = "integer", Format = "uint32", Minimum = 0, Maximum = 4294967295 });
                });
            }
        }

        public static void ConfigureSwagger(IApplicationBuilder app)
        {
            if (DeviceManager.Configuration.RunSwagger)
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint("/swagger/Alpaca/swagger.json",
                                 $"Alpaca JSON Endpoints - v1");
                    c.SwaggerEndpoint("/swagger/AlpacaSetup/swagger.json",
             $"Alpaca HTML Endpoints - v1");
                    c.SwaggerEndpoint("/swagger/OmniSim/swagger.json", "OmniSim Only Endpoints");
                    c.DocExpansion(DocExpansion.None);
                });
            }
        }

        public static void ConfigureAlpacaAPIBehavoir(IServiceCollection services)
        {
            //Add MVC
            services.AddMvc();

            //Do not automatically change JSON to Camel Case
            services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

            //Skip null entries, this should just be the optional driver exception
            services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);

            // Force API Validation errors to return HTTP 400, BadRequest
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    string ErrorMessage = "Unknown Error";
                    string ResourceMessage = "Unknown Resource";

                    try
                    {
                        ErrorMessage = context.ModelState.Values.First(m => m.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid).Errors.First().ErrorMessage;
                    }
                    catch (Exception ex)
                    {
                        Logging.LogError(ex.Message);
                    }

                    try
                    {
                        ResourceMessage = context.HttpContext.Request.Path;
                    }
                    catch (Exception ex)
                    {
                        Logging.LogError(ex.Message);
                    }

                    var message = $"A data validation error occurred for request {ResourceMessage} with error message <{ErrorMessage}>. See https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1.";

                    Logging.LogError(message);

                    return new BadRequestObjectResult(message);
                };
            });
        }

        public static void ConfigureAlpacaAPIBehavoir(WebApplication app)
        {
            app.MapControllers();
        }

        public static void ConfigureDiscovery(IApplicationBuilder app)
        {
            int port = DeviceManager.Configuration.ServerPort;

            //Parse out addresses for the server and start discovery on IPv4 / IPv6 based on which addresses are in use
            try
            {
                var serverAddressesFeature = app.ServerFeatures.Get<IServerAddressesFeature>();

                var Addresses = serverAddressesFeature.Addresses.ToArray();

                if (serverAddressesFeature.Addresses.Count > 0)
                {
                    var serverAddress = serverAddressesFeature.Addresses.First();
                    bool localHostOnly = false;
                    bool ipv6 = false;

                    if (Uri.TryCreate(serverAddress, UriKind.RelativeOrAbsolute, out Uri serverUri))
                    {
                        try
                        {
                            port = serverUri.Port;
                            if (serverUri.Host.ToLowerInvariant().Contains("localhost") || IPAddress.IsLoopback(IPAddress.Parse(serverUri.Host)))
                            {
                                localHostOnly = true;

                                if (IPAddress.TryParse(serverUri.Host, out IPAddress address))
                                {
                                    if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                                    {
                                        if (address.IsIPv6LinkLocal)
                                        {
                                            localHostOnly = false;
                                        }
                                        ipv6 = true;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Logging.LogError(ex.Message);
                        }
                    }
                    else //Invalid Uri, simply parse out port
                    {
                        if (serverAddress.Contains(":"))
                        {
                            if (int.TryParse(serverAddress.Split(':').Last(), out int result))
                            {
                                port = result;
                            }
                        }

                        ipv6 = serverAddress.Contains("*") || serverAddress.Contains("+");
                    }

                    DiscoveryManager.Start(port, localHostOnly, ipv6);
                }
                else
                {
                    DiscoveryManager.Start();
                }
            }
            catch (Exception ex)
            {
                Logging.LogError(ex.Message);
            }

        }

        public static void ConfigureAuthentication(IServiceCollection services)
        {
            // configure basic authentication
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
                options =>
                {
                    options.SlidingExpiration = true;
                    options.ExpireTimeSpan = TimeSpan.FromHours(1);
                }
                );

            // configure DI for application services
            services.AddScoped<AuthorizationFilter>();
        }

        public static void ConfigureAuthentication(IApplicationBuilder app)
        {
            //Allow authentication, either Cookie or Basic HTTP Auth
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCookiePolicy();
        }
    }
}
