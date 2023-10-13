using System.Runtime.InteropServices;

namespace ASCOM.Simulators.LocalServer
{
    internal static class NativeMethods
    {
        [DllImport("User32.dll", EntryPoint = "MessageBox",CharSet = CharSet.Auto)]
        internal static extern int MessageBox(IntPtr hWnd, string lpText, string lpCaption, uint uType);
    }
}
