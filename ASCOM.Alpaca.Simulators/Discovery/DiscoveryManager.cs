using ASCOM.Alpaca.Discovery;
using ASCOM.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace ASCOM.Alpaca.Simulators.Discovery
{
    internal static class DiscoveryManager
    {
        internal static Responder DiscoveryResponder
        {
            get;
            private set;
        }

        internal static bool IsRunning => !DiscoveryResponder?.Disposed ?? false;

        internal static List<IPAddress> AdapterAddress
        {
            get
            {
                List<IPAddress> Addresses = new List<IPAddress>();
                NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface adapter in adapters)
                {
                    try
                    {
                        //Do not try and use non-operational adapters
                        if (adapter.OperationalStatus != OperationalStatus.Up)
                            continue;

                        IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                        if (adapterProperties == null)
                            continue;

                        UnicastIPAddressInformationCollection uniCast = adapterProperties.UnicastAddresses;
                        if (uniCast.Count > 0)
                        {
                            foreach (UnicastIPAddressInformation uni in uniCast)
                            {
                                try
                                {
                                    if (uni.Address.AddressFamily != AddressFamily.InterNetwork && uni.Address.AddressFamily != AddressFamily.InterNetworkV6)
                                        continue;

                                    Addresses.Add(uni.Address);
                                }
                                catch (Exception ex)
                                {
                                    Logging.Log.Log(LogLevel.Error, $"Failed to read adapter address with error {ex.Message}");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logging.Log.Log(LogLevel.Error, $"Failed to read network adapter with error {ex.Message}");
                    }
                }
                return Addresses;
            }
        }

        internal static void Start()
        {
            if (ServerSettings.AllowDiscovery)
            {
                Console.WriteLine("Starting discovery responder from defaults");

                DiscoveryResponder = new Responder(ServerSettings.ServerPort, true, false)
                {
                    AllowRemoteAccess = ServerSettings.AllowRemoteAccess,
                    LocalRespondOnlyToLocalHost = ServerSettings.LocalRespondOnlyToLocalHost
                };
            }
        }

        internal static void Start(int port, bool localHostOnly, bool ipv6)
        {
            if (ServerSettings.AllowDiscovery)
            {
                Console.WriteLine($"Starting Discovery on port: {port}");

                if (!Dns.GetHostAddresses(Dns.GetHostName()).Any(o => o.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6))
                {
                    ipv6 = false;
                }

                DiscoveryResponder = new Responder(port, true, ipv6)
                {
                    AllowRemoteAccess = !localHostOnly,
                    LocalRespondOnlyToLocalHost = ServerSettings.LocalRespondOnlyToLocalHost
                };
            }
        }

        internal static void Stop()
        {
            DiscoveryResponder.Dispose();
        }
    }
}