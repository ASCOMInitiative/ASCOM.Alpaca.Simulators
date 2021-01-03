using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ASCOM.Alpaca.Simulators
{
    internal static class ServerManager
    {
        private static uint transactionID = 0;

        internal static uint ServerTransactionID
        {
            get
            {
                return transactionID++;
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
    }
}
