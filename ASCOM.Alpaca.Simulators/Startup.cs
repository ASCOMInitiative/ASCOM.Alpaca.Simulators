using Blazored.Toast;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Linq;
using System.Net;

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
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = $"{ServerSettings.ServerName}", Version = "v1" });
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            if (ServerSettings.RunSwagger)
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json",
                                 $"{ServerSettings.ServerName} v1"));
            }

            int port = ServerSettings.ServerPort;

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

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCookiePolicy();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            lifetime.ApplicationStarted.Register(() =>
            {
                Logging.LogInformation($"{ServerSettings.ServerName} Starting");

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

            lifetime.ApplicationStopped.Register(() =>
            {
                Logging.LogInformation($"{ServerSettings.ServerName} Stopped");
            });

            Lifetime = lifetime;
        }
    }
}