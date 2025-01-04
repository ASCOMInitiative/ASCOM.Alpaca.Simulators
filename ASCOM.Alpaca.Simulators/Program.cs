using ASCOM.Common;
using ASCOM.Simulators;
using ASCOM.Tools;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.IO.Pipes;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json;
using System.Collections;

namespace ASCOM.Alpaca.Simulators
{
    public class Program
    {
        private static Guid ApplicationGUID = new Guid("{1389A00E-006F-4117-8930-EAFCAA7DC397}");

        private const string PipeGUID = "{2249563F-E844-4264-8956-73AC7A44BEA0}";



        public static void Main(string[] args)
        {
            // Unique ID for global mutex - Global prefix means it is global to the machine
            string mutexId = string.Format("Global\\{{{0}}}", ApplicationGUID);

            using (var mutex = new Mutex(false, mutexId, out bool createdNew))
            {
                var hasHandle = false;
                try
                {
                    try
                    {
                        //Time out fast, this is just to check if a copy is already running
                        hasHandle = mutex.WaitOne(10, false);

                        //This is the second copy to start, either it will process an argument itself or it will pass the command to the running copy
                        if (hasHandle == false)
                        {
                            var client = new NamedPipeClientStream(PipeGUID);
                            client.Connect();
                            StreamReader reader = new StreamReader(client);
                            StreamWriter writer = new StreamWriter(client);

                            if (args != null && args.Count() > 0)
                            {
                                foreach (var arg in args)
                                {

                                    if (arg.Contains("--local-address"))
                                    {
                                        Console.WriteLine($"http://localhost:{ServerSettings.ServerPort}");
                                        continue;
                                    }

                                    if (arg.Contains("--show-browser"))
                                    {
                                        StartBrowser(ServerSettings.ServerPort);
                                        continue;
                                    }

                                    writer.WriteLine(arg);
                                    writer.Flush();
                                }
                                return;
                            }
                            else
                            {
                                while (true)
                                {
                                    string input = Console.ReadLine();
                                    if (String.IsNullOrEmpty(input)) break;
                                    writer.WriteLine(input);
                                    writer.Flush();
                                    Console.WriteLine(reader.ReadLine());
                                }
                                throw new TimeoutException("Timeout waiting for exclusive access");
                            }
                        }
                    }
                    catch (AbandonedMutexException)
                    {
                        // Log the fact that the mutex was abandoned in another process,
                        // it will still get acquired
                        hasHandle = true;
                    }

                    PrintStartupInformation();

                    //This is the first copy to start. It will start the Alpaca service and any other functions. This task listens for external commands
                    Task.Factory.StartNew(() =>
                    {
                        var server = new NamedPipeServerStream(PipeGUID);
                        server.WaitForConnection();
                        StreamReader reader = new StreamReader(server);
                        StreamWriter writer = new StreamWriter(server);
                        while (true)
                        {
                            try
                            {
                                if (!server.IsConnected)
                                {
                                    server.Disconnect();
                                    server.WaitForConnection();
                                }
                                var line = reader.ReadLine();

                                if (line == null)
                                {
                                    System.Threading.Thread.Sleep(1);
                                    continue;
                                }

                                Console.WriteLine($"Received external command {line}");

                                ProcessArgs(new string[] { line }, true);

                                writer.Flush();

                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    });

                    // This is the first copy, process all args
                    if(ProcessArgs(args, false))
                    {
                        //Exit if only one copy and command calls for exit
                        return;
                    }

                    // Set console visibility (Windows only)
                    ShowConsole(ServerSettings.ConsoleDisplay);

                    var BlazorTask = InitServers(args);

                    BlazorTask.Start();

                    Console.ReadLine();

                    // Perform your work here.
                }
                finally
                {
                    // edited by acidzombie24, added if statement
                    if (hasHandle)
                        mutex.ReleaseMutex();
                }
            }


            /*try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    //Already running, start the browser, detects based on port in use
                    var con1 = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpConnections().Where(con => con.LocalEndPoint.Port == ServerSettings.ServerPort);
                    if (IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpConnections().Any(con => con.LocalEndPoint.Port == ServerSettings.ServerPort && (con.State == TcpState.Listen || con.State == TcpState.Established)))
                    {
                        WriteAndLog("Detected driver port already open, starting web browser on IP and Port. If this fails something else is using the port");
                        StartBrowser(ServerSettings.ServerPort);
                        return;
                    }
                }
                else
                {
                    //This was working fine for .Net Core 3.1. Initial tests for .Net 5 show a change in how single file deployments work on Linux
                    //This should probably be changed to a Mutex or another similar lock
                    if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1)
                    {
                        WriteAndLog("Detected driver already running, starting web browser on IP and Port");
                        StartBrowser(ServerSettings.ServerPort);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.LogError(ex.Message);
            }*/
        }

        private static Task InitServers(string[] args)
        {
            ASCOM.Alpaca.Logging.AttachLogger(Logging.Log);

            //Load configuration
            DeviceManager.LoadConfiguration(new AlpacaConfiguration());

            //Load devices
            DriverManager.LoadCamera(0);
            DriverManager.LoadCoverCalibrator(0);
            DriverManager.LoadDome(0);
            DriverManager.LoadFilterWheel(0);
            DriverManager.LoadFocuser(0);
            DriverManager.LoadObservingConditions(0);
            DriverManager.LoadRotator(0);
            DriverManager.LoadSafetyMonitor(0);
            DriverManager.LoadSwitch(0);
            DriverManager.LoadTelescope(0);

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

            ServerSettings.CheckForUpdates();


            return new Task(() =>
            {
                try
                {
#if ASCOM_COM
            OmniSim.LocalServer.Server.InitServer();
            OmniSim.LocalServer.Drivers.Camera.DeviceAccess = () => ASCOM.Alpaca.DeviceManager.GetCamera(0);
            OmniSim.LocalServer.Drivers.CoverCalibrator.DeviceAccess = () => ASCOM.Alpaca.DeviceManager.GetCoverCalibrator(0);
            OmniSim.LocalServer.Drivers.Dome.DeviceAccess = () => ASCOM.Alpaca.DeviceManager.GetDome(0);
            OmniSim.LocalServer.Drivers.FilterWheel.DeviceAccess = () => ASCOM.Alpaca.DeviceManager.GetFilterWheel(0);
            OmniSim.LocalServer.Drivers.Focuser.DeviceAccess = () => ASCOM.Alpaca.DeviceManager.GetFocuser(0);
            OmniSim.LocalServer.Drivers.ObservingConditions.DeviceAccess = () => ASCOM.Alpaca.DeviceManager.GetObservingConditions(0);
            OmniSim.LocalServer.Drivers.Rotator.DeviceAccess = () => ASCOM.Alpaca.DeviceManager.GetRotator(0);
            OmniSim.LocalServer.Drivers.SafetyMonitor.DeviceAccess = () => ASCOM.Alpaca.DeviceManager.GetSafetyMonitor(0);
            OmniSim.LocalServer.Drivers.Switch.DeviceAccess = () => ASCOM.Alpaca.DeviceManager.GetSwitch(0);
            OmniSim.LocalServer.Drivers.Telescope.DeviceAccess = () => ASCOM.Alpaca.DeviceManager.GetTelescope(0);

            if (!OmniSim.LocalServer.Server.ProcessAllArguments(args))
            {
                return;
            }
#endif

#if ASCOM_COM
            OmniSim.LocalServer.Server.StartServer();
#endif


                    CreateHostBuilder(args).Build().Run();
                }
                catch (OperationCanceledException)
                {
                    //Server was shutdown
                }
                catch (Exception ex)
                {
                    Logging.LogError(ex.Message);
                    Console.WriteLine("A fatal error has occurred and the OmniSim is shutting down.");
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                }
            });
        }

        private static bool ProcessArgs(string[] args, bool existing_instance)
        {
            //Reset all stored settings if requested
            if (args?.Any(str => str.Contains("--reset")) ?? false)
            {
                try
                {
                    //Load configuration
                    DeviceManager.LoadConfiguration(new AlpacaConfiguration());

                    //Load devices
                    DriverManager.LoadCamera(0);
                    DriverManager.LoadCoverCalibrator(0);
                    DriverManager.LoadDome(0);
                    DriverManager.LoadFilterWheel(0);
                    DriverManager.LoadFocuser(0);
                    DriverManager.LoadObservingConditions(0);
                    DriverManager.LoadRotator(0);
                    DriverManager.LoadSafetyMonitor(0);
                    DriverManager.LoadSwitch(0);
                    DriverManager.LoadTelescope(0);

                    WriteAndLog("Reseting stored settings");
                    WriteAndLog("Reseting Server settings");
                    ServerSettings.Reset();
                    WriteAndLog("Reseting Device settings");
                    DriverManager.Reset();
                    WriteAndLog("Settings reset, shutting down");
                    return true;
                }
                catch (Exception ex)
                {
                    WriteAndLog(ex.Message);
                    Logging.LogError(ex.Message);
                    return true;
                }
            }

            if (args?.Any(str => str.Contains("--set-no-browser")) ?? false)
            {
                WriteAndLog("Turning off auto start browser");
                ServerSettings.AutoStartBrowser = false;
                WriteAndLog("Auto start browser is off");
                return true;
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

            if (args?.Any(str => str.Contains("--show-browser")) ?? false)
            {
                StartBrowser(ServerSettings.ServerPort);
            }

            if (args?.Any(str => str.Contains("--settings")) ?? false)
            {
                var realwheel = (DeviceManager.FilterWheels[0] as ASCOM.Simulators.FilterWheel).FilterWheelHardware;
                foreach (var prop in GetSettingsProperties(realwheel.GetType()))
                {
                    dynamic setting = realwheel.GetType().GetProperty(prop.Name).GetValue(realwheel, null);
                    Console.WriteLine($"{setting.Key} - {setting.Value} - {setting.Description}");

                    Console.WriteLine(JsonSerializer.Serialize(setting));
                }

            }

            return false;
        }

        private static System.Collections.Generic.IEnumerable<PropertyInfo> GetSettingsProperties(Type DriverType)
        {
            var types = DriverType.GetProperties();
            var props = types.Where(p => p.PropertyType.ToString().Contains(typeof(OmniSim.BaseDriver.Setting<>).FullName.ToString()));
            return props;
        }

        private static void PrintStartupInformation()
        {
            WriteAndLog($"{ServerSettings.ServerName} version {ServerSettings.ServerVersion}");
            WriteAndLog($"Running on: {RuntimeInformation.OSDescription}.");
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
            catch (Exception ex)
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
                    .UseContentRoot(System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName))
#endif

#if ASCOM_COM
                    .UseContentRoot(System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName))
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
                                catch (Exception ex)
                                {
                                    Logging.LogError($"Failed to start SSL load with error: {ex.Message}");
                                }

                                if (ServerSettings.AllowRemoteAccess)
                                {
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

        /// <summary>
        /// Shows, minimises or hides the console window (Windows only)
        /// </summary>
        /// <param name="newConsoleState">The required console window state</param>
        internal static void ShowConsole(ConsoleDisplayOption newConsoleState)
        {
            // Protect these Windows specific calls from running on non Windows OS
            if (OperatingSystem.IsWindows())
            {
                try
                {
                    // Get the console window title
                    string consoleTitle = Console.Title;

                    // Get the Windows handle for the console window
                    IntPtr consolhWnd = WindowsNative.FindWindow(null, consoleTitle);
                    Logging.Log.LogDebug($"ShowConsole - Console title: {consoleTitle}, Console hWnd: {consolhWnd}.");

                    // Get the current console display state
                    ConsoleDisplayOption currentConsoleState = ConsoleWindowsState(consolhWnd);

                    if (newConsoleState != currentConsoleState)
                    {
                        // Call the WIndows API ShowWindow method with the appropriate window state parameter
                        switch (newConsoleState)
                        {
                            case ConsoleDisplayOption.StartNormally: // Console window visible on desktop

                                Logging.LogInformation($"Displaying console window.");
                                WindowsNative.ShowWindow(consolhWnd, WindowsNative.SW_RESTORE);
                                break;

                            case ConsoleDisplayOption.StartMinimized: // Console window minimised to task bar
                                Logging.LogInformation($"Minimising console window.");
                                WindowsNative.ShowWindow(consolhWnd, WindowsNative.SW_MINIMIZE);
                                break;

                            case ConsoleDisplayOption.NoConsole: // Console window hidden
                                Logging.LogInformation($"Hiding console window.");
                                WindowsNative.ShowWindow(consolhWnd, WindowsNative.SW_HIDE);
                                break;

                            default: // An unknown window state
                                Logging.LogError($"ShowConsole - Unable to set the requested {newConsoleState} window state because it is unknown. No action taken.");
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logging.LogError(ex.Message);
                }
            }
        }

        /// <summary>
        /// Return the <see cref="ConsoleDisplayOption"/> state of the window with the supplied handle
        /// </summary>
        /// <param name="hWnd">Windows handle of the window to check.</param>
        /// <returns>A <see cref="ConsoleDisplayOption"/> value describing the current window state.</returns>
        private static ConsoleDisplayOption ConsoleWindowsState(IntPtr hWnd)
        {
            // First check whether the window is hidden or visible in some way
            if (!WindowsNative.IsWindowVisible(hWnd)) // Window is hidden
                return ConsoleDisplayOption.NoConsole;

            
            // Second check whether the window is minimised
            if (WindowsNative.IsIconic(hWnd)) // Window is minimised
                return ConsoleDisplayOption.StartMinimized;

            // The window is not hidden or minimized so the console must be visible on the desktop either as a normal window or maximised
            return ConsoleDisplayOption.StartNormally;
        }
    }
}