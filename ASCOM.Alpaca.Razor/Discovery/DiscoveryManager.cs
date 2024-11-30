// Ignore Spelling: ipv

using ASCOM.Alpaca.Discovery;
using ASCOM.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.Versioning;

namespace ASCOM.Alpaca
{
    [UnsupportedOSPlatform("browser")]
    public static class DiscoveryManager
    {
        public static Responder DiscoveryResponder
        {
            get;
            private set;
        }

        public static bool IsRunning => !DiscoveryResponder?.Disposed ?? false;

        public static List<IPAddress> AdapterAddress
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
                                    Logging.LogError($"Failed to read adapter address with error {ex.Message}");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logging.LogError($"Failed to read network adapter with error {ex.Message}");
                    }
                }
                return Addresses;
            }
        }

        public static void Start()
        {
            if (DeviceManager.Configuration.AllowDiscovery)
            {
                Console.WriteLine("Starting discovery responder from defaults");

                DiscoveryResponder = new Responder(DeviceManager.Configuration.ServerPort, true, false, Logging.Log)
                {
                    AllowRemoteAccess = DeviceManager.Configuration.AllowRemoteAccess,
                    LocalRespondOnlyToLocalHost = DeviceManager.Configuration.LocalRespondOnlyToLocalHost
                };
            }
        }

        public static void Start(int port, bool localHostOnly, bool ipv6)
        {
            if (DeviceManager.Configuration.AllowDiscovery)
            {
                Console.WriteLine($"Starting Discovery on port: 32227");

                if (!Dns.GetHostAddresses(Dns.GetHostName()).Any(o => o.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6))
                {
                    ipv6 = false;
                }

                DiscoveryResponder = new Responder(port, true, ipv6, Logging.Log)
                {
                    AllowRemoteAccess = !localHostOnly,
                    LocalRespondOnlyToLocalHost = DeviceManager.Configuration.LocalRespondOnlyToLocalHost
                };
            }
        }

        public static void Stop()
        {
            DiscoveryResponder.Dispose();
        }
    }
}