using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASCOM.Alpaca.Simulators
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Reset all stored settings if requested
            if (args?.Any(str => str.Contains("--reset")) ?? false)
            {
                Console.WriteLine("Reseting stored settings");
                Logging.LogInformation("Reseting stored settings");
                Console.WriteLine("Reseting Server settings");
                ServerSettings.Reset();
                Console.WriteLine("Reseting Device settings");
                //DeviceManager.Reset();
                Console.WriteLine("Settings reset, shutting down");
                Logging.LogInformation("Settings reset, shutting down");
                return;
            }

            if (args?.Any(str => str.Contains("--reset-auth")) ?? false)
            {
                Console.WriteLine("Turning off Authentication to allow password reset.");
                ServerSettings.UseAuth = false;
                Console.WriteLine("You can change the password and then re-enable Authentication.");
            }

            //Already running, start the browser
            //This was working fine for .Net Core 3.1. Initial tests for .Net 5 show a change in how single file deployments work on Linux
            //This should probably be changed to a Mutex or another similar lock
            if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1)
            {
                ServerManager.StartBrowser(ServerSettings.ServerPort);
                return;
            }

            //Add the --urls argument for IHostBuilder
            if (!args?.Any(str => str.Contains("--urls")) ?? true)
            {
                if (args == null)
                {
                    args = new string[0];
                }

                Console.WriteLine("No startup url args detected, binding to saved server settings.");

                var temparray = new string[args.Length + 1];

                args.CopyTo(temparray, 0);

                string startupURLArg = "--urls=http://";

                if (ServerSettings.AllowRemoteAccess)
                {
                    startupURLArg += "*";
                }
                else
                {
                    startupURLArg += "localhost";
                }

                startupURLArg += ":" + ServerSettings.ServerPort;

                Console.WriteLine("Startup URL args: " + startupURLArg);
                Logging.LogInformation("Startup URL args: " + startupURLArg);

                temparray[args.Length] = startupURLArg;

                args = temparray;
            }

            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (OperationCanceledException)
            {
                //Server was shutdown
            }
            catch (Exception ex)
            {
                Logging.LogError(ex.Message);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
