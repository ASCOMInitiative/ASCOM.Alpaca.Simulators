using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

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

            if (args?.Any(str => str.Contains("--local-address")) ?? false)
            {
                Console.WriteLine($"http://localhost:{ServerSettings.ServerPort}");
            }

            //Already running, start the browser, detects based on port in use
            try
            {
                if(IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpConnections().Any(con => con.LocalEndPoint.Port == ServerSettings.ServerPort))
                {
                    WriteAndLog("Detected driver port already open, starting web browser on IP and Port. If this fails something else is using the port");
                    StartBrowser(ServerSettings.ServerPort);
                    return;
                }
            }
            catch(Exception ex)
            {
                Logging.LogError(ex.Message);
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

                //If set to allow remote access bind to all local ips, otherwise bind only to localhost
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
                Update.CheckForUpdates();
            }
            catch
            {

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
                Console.WriteLine(ex.Message);
                Logging.LogError(ex.Message);
            }
        }

        /// <summary>
        /// Starts the system default handler (normally a browser) for local host and the current port.
        /// </summary>
        /// <param name="port"></param>
        internal static void StartBrowser(int port)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = string.Format("http://localhost:{0}", port),
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch(Exception ex)
            {
                Logging.LogError(ex.Message);
            }
        }

        /// <summary>
        /// Creates a asp.net host builder. Sets the content root if a bundled application.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
#if BUNDLED
                    .UseContentRoot(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location))
#endif
                    .UseKestrel().ConfigureKestrel(options =>
                    {
                        try
                        {
                            if (ServerSettings.UseSSL)
                            {
                                if (!System.IO.File.Exists(ServerSettings.SSLCertPath))
                                {
                                    Logging.LogInformation("Generating Self Signed SSL Certificate");
                                    var cert = SSL.SelfCert.BuildSelfSignedServerCertificate("TestCert", ServerSettings.SSLCertPassword);
                                    SSL.SelfCert.SaveCertificate(cert, ServerSettings.SSLCertPassword, ServerSettings.SSLCertPath);
                                }

                                try
                                {
                                    options.Listen(IPAddress.Any, ServerSettings.SSLPort, listenOptions =>
                                    {
                                        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                                        listenOptions.UseHttps(ServerSettings.SSLCertPath, ServerSettings.SSLCertPassword);
                                    });
                                }
                                catch(Exception ex)
                                {
                                    Logging.LogError($"Failed to start SSL load with error: {ex.Message}");
                                }

                                if (ServerSettings.AllowRemoteAccess) {
                                    options.Listen(IPAddress.Any, ServerSettings.ServerPort);
                                }
                                else
                                {
                                    options.Listen(IPAddress.Loopback, ServerSettings.ServerPort);
                                }
                            }
                        }

                        catch (Exception ex)
                        {
                            Logging.LogError($"Failed to start SSL with error: {ex.Message}");
                        }
                    }
                    );
                });

            return builder;
        }

        /// <summary>
        /// Writes to the console and logs to the primary log provider at the informational level.
        /// </summary>
        /// <param name="message">The message to write out</param>
        private static void WriteAndLog(string message)
        {
            Console.WriteLine(message);
            Logging.LogInformation(message);
        }
    }
}