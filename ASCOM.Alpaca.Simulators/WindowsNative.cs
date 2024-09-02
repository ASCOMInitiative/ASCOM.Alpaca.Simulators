using System.Runtime.InteropServices;
using System;

namespace ASCOM.Alpaca.Simulators
{
	public class WindowsNative
	{
        internal const Int32 SW_HIDE = 0;
        internal const Int32 SW_SHOW = 5;
		internal const Int32 SW_MINIMIZE = 6;
		internal const Int32 SW_RESTORE = 9;

        /// <summary>
        /// Return the Windows handle of the windows with the supplied title
        /// </summary>
        /// <param name="lpClassName">Class name of the windows or null</param>
        /// <param name="lpWindowName">Title of the window or null</param>
        /// <returns>Windows handle (hWnd) of the window if found or zero if not found</returns>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// Change the visibility state of the window with the given handle
        /// </summary>
        /// <param name="hWnd">Windows handle (hWnd) of the window</param>
        /// <param name="swOption">Desired WindowsNative.SW_XXX window state</param>
        /// <returns>1 if successful, 0 if failed</returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool ShowWindow(IntPtr hWnd, int swOption);

        /// <summary>
        /// Determine whether a window is hidden or visible in some fashion
        /// </summary>
        /// <param name="hWnd">Windows handle (hWnd) of the window</param>
        /// <returns>True if the window is visible in some way (minimised, maximised or normal window) or false if it is hidden.</returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool IsWindowVisible(IntPtr hWnd);

        /// <summary>
        /// Determine whether a window is minimised
        /// </summary>
        /// <param name="hWnd">Windows handle (hWnd) of the window</param>
        /// <returns>True if the window is minimised or false if it is not.</returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool IsIconic(IntPtr hWnd);
    }
}
