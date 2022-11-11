using Blazored.Toast;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;

namespace ASCOM.Alpaca.Simulators
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        internal static IHostApplicationLifetime Lifetime
        {
            get;
            private set;
        }

        internal static string[] Addresses
        {
            get;
            private set;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddMvc();

            if (ServerSettings.RunSwagger)
            {
                services.AddSwaggerGen(c =>
                {
                    //This allows for an interesting camera image function but has some issues
                    //This is off for now
                    //c.UseOneOfForPolymorphism();

                    c.SwaggerDoc("v1", new OpenApiInfo { Title = $"{ServerSettings.ServerName}", Description = "Please note that the Alpaca API documentation on the ASCOM website is the canonical version. There are several issues with this auto generated version that will be resolved in future versions. This is currently provided only for testing the simulators.", Version = "v0" });

                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                    if (File.Exists(xmlPath))
                    {
                        c.IncludeXmlComments(xmlPath);
                    }
                    else
                    {
                        Logging.LogInformation("Failed to find simulator xml documentation.");
                    }

                    if (File.Exists(Path.Combine(AppContext.BaseDirectory, "ASCOM.Common.xml")))
                    {
                        xmlPath = Path.Combine(AppContext.BaseDirectory, "ASCOM.Common.xml");

                        c.IncludeXmlComments(xmlPath);
                    }
                    else
                    {
                        Logging.LogInformation("Failed to find ASCOM.Common xml documentation.");
                    }

                    c.EnableAnnotations();
                });
            }

            // configure basic authentication
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
                options =>
                {
                    options.SlidingExpiration = true;
                    options.ExpireTimeSpan = TimeSpan.FromHours(1);
                }
                );

            // configure DI for application services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<AuthorizationFilter>();
            services.AddBlazoredToast();

            //Do not automatically change JSON to Camel Case
            services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

            //Use Development exceptions if in development mode. These are disabled in production for security.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            //Start Swagger on the Swagger endpoints if enabled.
            if (ServerSettings.RunSwagger)
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json",
                                 $"{ServerSettings.ServerName} v1"));
            }

            int port = ServerSettings.ServerPort;

            //Parse out addresses for the server and start discovery on IPv4 / IPv6 based on which addresses are in use
            try
            {
                var serverAddressesFeature = app.ServerFeatures.Get<IServerAddressesFeature>();

                Addresses = serverAddressesFeature.Addresses.ToArray();

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

                    Discovery.DiscoveryManager.Start(port, localHostOnly, ipv6);
                }
                else
                {
                    Discovery.DiscoveryManager.Start();
                }
            }
            catch (Exception ex)
            {
                Logging.LogError(ex.Message);
            }

            //Serve static files, mostly CSS
            app.UseStaticFiles();

            app.UseRouting();

            //Allow authentication, either Cookie or Basic HTTP Auth
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCookiePolicy();

            //Map Endpoints, primarily Blazor UI and REST Controllers
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            //Put code here that needs to run on startup
            lifetime.ApplicationStarted.Register(() =>
            {
                Logging.LogInformation($"{ServerSettings.ServerName} Starting Services");

                try
                {
                    if (ServerSettings.AutoStartBrowser) //AutoStart Browser
                    {
                        Program.StartBrowser(port);
                    }
                }
                catch (Exception ex)
                {
                    Logging.LogError(ex.Message);
                }
            });

            //Put code here that runs at shutdown
            lifetime.ApplicationStopping.Register(() =>
            {
                ASCOM.Simulators.TelescopeHardware.ShutdownTelescope();
                Logging.LogInformation($"{ServerSettings.ServerName} Stopping");
            });

            //Put code here that needs to run after shutdown of ASP.Net Core systems.
            lifetime.ApplicationStopped.Register(() =>
            {
                Logging.LogInformation($"{ServerSettings.ServerName} Stopped");
            });

            Lifetime = lifetime;
        }
    }
}