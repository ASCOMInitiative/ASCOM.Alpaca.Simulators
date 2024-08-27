using System.Runtime.InteropServices;
using System;

namespace ASCOM.Alpaca.Simulators
{
	public class WindowsNative
	{
		internal static Int32 WM_SYSCOMMAND = 0x0112;
		internal static Int32 SC_MINIMIZE = 0x0F020;

        internal const Int32 SW_HIDE = 0;
        internal const Int32 SW_SHOW = 5;
		internal const Int32 SW_MINIMIZE = 6;

        [DllImport("kernel32.dll")]
		public static extern Boolean AllocConsole();

		[DllImport("kernel32.dll")]
		public static extern Boolean AttachConsole(Int32 ProcessId);

		[DllImport("kernel32.dll")]
		public static extern Boolean FreeConsole();

		[DllImport("user32.dll")]
		internal static extern bool SendMessage(IntPtr hWnd, Int32 msg, Int32 wParam, Int32 lParam);

        [DllImport("user32.dll")]
        internal static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    }
}
