using System;
using System.Runtime.InteropServices;

namespace ComBase
{
    public static class IdleCheck
    {

        [StructLayout(LayoutKind.Sequential)]
        private struct LASTINPUTINFO
        {

            [MarshalAs(UnmanagedType.U4)]

            public int cbSize;

            [MarshalAs(UnmanagedType.U4)]

            public int dwTime;

        }

        [DllImport("user32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO x);

        public static int GetLastInputTime()
        {

            var inf = new LASTINPUTINFO();

            inf.cbSize = Marshal.SizeOf(inf);

            inf.dwTime = 0;

            return (GetLastInputInfo(ref inf)) ? Environment.TickCount - inf.dwTime : 0;

        }
    }
}
