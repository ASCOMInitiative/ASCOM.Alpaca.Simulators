using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Linq;

namespace ASCOM.Alpaca.Simulators
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WriteAndLog($"{ServerSettings.ServerName} version {ServerSettings.ServerVersion}");

            //Reset all stored settings if requested
            if (args?.Any(str => str.Contains("--reset")) ?? false)
            {
                WriteAndLog("Reseting stored settings");
                WriteAndLog("Reseting Server settings");
                ServerSettings.Reset();
                WriteAndLog("Reseting Device settings");
                DeviceManager.Reset();
                WriteAndLog("Settings reset, shutting down");
                return;
            }

            //Turn off Authentication. Once off the user can change the password and re-enable authentication
            if (args?.Any(str => str.Contains("--reset-auth")) ?? false)
            {
                WriteAndLog("Turning off Authentication to allow password reset.");
                ServerSettings.UseAuth = false;
                WriteAndLog("Authentication off, you can change the password and then re-enable Authentication.");
            }

            //Already running, start the browser
            //This was working fine for .Net Core 3.1. Initial tests for .Net 5 show a change in how single file deployments work on Linux
            //This should probably be changed to a Mutex or another similar lock
            if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1)
            {
                WriteAndLog("Detected driver already running, starting web browser on IP and Port");
                StartBrowser(ServerSettings.ServerPort);
                return;
            }

            //Add the --urls argument for IHostBuilder
            if (!args?.Any(str => str.Contains("--urls")) ?? true)
            {
                if (args == null)
                {
                    args = new string[0];
                }

                WriteAndLog("No startup url args detected, binding to saved server settings.");

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

                WriteAndLog("Startup URL args: " + startupURLArg);

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

        internal static void StartBrowser(int port)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = string.Format("http://localhost:{0}", port),
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void WriteAndLog(string message)
        {
            Console.WriteLine(message);
            Logging.LogInformation(message);
        }
    }
}