using Blazored.Toast;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Globalization;
using System.IO;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddBlazoredToast();

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            ASCOM.Alpaca.Razor.StartupHelpers.ConfigureSwagger(services, xmlPath);
            ASCOM.Alpaca.Razor.StartupHelpers.ConfigureAlpacaAPIBehavoir(services);
            ASCOM.Alpaca.Razor.StartupHelpers.ConfigureAuthentication(services);

            services.AddScoped<IUserService, UserService>();
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
            Razor.StartupHelpers.ConfigureSwagger(app);

            //Configure Discovery
            Razor.StartupHelpers.ConfigureDiscovery(app);

            //Serve static files, mostly CSS
            app.UseStaticFiles();

            app.UseRouting();

            Razor.StartupHelpers.ConfigureAuthentication(app);

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
                    if (ServerSettings.StartBrowserAtStart) //AutoStart Browser
                    {
                        Program.StartBrowser(ServerSettings.ServerPort);
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