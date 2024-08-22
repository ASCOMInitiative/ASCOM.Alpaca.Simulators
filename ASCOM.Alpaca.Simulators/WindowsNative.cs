using System.Runtime.InteropServices;
using System;

namespace ASCOM.Alpaca.Simulators
{
	public class WindowsNative
	{
		internal static Int32 WM_SYSCOMMAND = 0x0112;
		internal static Int32 SC_MINIMIZE = 0x0F020;

		[DllImport("kernel32.dll")]
		public static extern Boolean AllocConsole();

		[DllImport("kernel32.dll")]
		public static extern Boolean AttachConsole(Int32 ProcessId);

		[DllImport("kernel32.dll")]
		public static extern Boolean FreeConsole();

		[DllImport("user32.dll")]
		internal static extern bool SendMessage(IntPtr hWnd, Int32 msg, Int32 wParam, Int32 lParam);
	}
}
