using System;
using System.Runtime.InteropServices;

namespace CappuChat
{
    static class NativeMethods
    {
        [DllImport("user32")]
        public static extern int FlashWindow(IntPtr hwnd, bool bInvert);
    }
}
