using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text;
using Microsoft.Win32; 
using System.Runtime.InteropServices;  
using System.Diagnostics;   
using System.Linq;   

namespace ComBase
{
    public class clsApi
    {
        const int SW_HIDE = 0;
        const int SW_SHOW = 1;
        const int WM_USER = 0x400;
        const int EM_HIDESELECTION = WM_USER + 63;
        const int WH_KEYBOARD_LL = 13;
        const Int32 WM_COPYDATA = 0x004A;
        const int WM_SETTEXT = 0x000C;
        const int HWND_BROADCAST = 0xffff;
        const int WM_FONTCHANGE = 0x001D;
        const int WM_CLOSE = 0x10;
        const int WM_SETREDRAW = 11;

        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public UInt32 cbData;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;
        }

        public struct KBDLLHOOKSTRUCT
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }
        public static int intLLKey;

        UInt32 SB_VERT = 1;
        UInt32 OBJID_VSCROLL = 0xFFFFFFFB;

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public Int32 left;
            public Int32 top;
            public Int32 right;
            public Int32 bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SCROLLBARINFO
        {
            public Int32 cbSize;
            public RECT rcScrollBar;
            public Int32 dxyLineButton;
            public Int32 xyThumbTop;
            public Int32 xyThumbBottom;
            public Int32 reserved;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public Int32[] rgstate;
        }

        [DllImport("user32", EntryPoint = "SetWindowsHookExA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int SetWindowsHookEx(int idHook, LowLevelKeyboardProcDelegate lpfn, int hMod, int dwThreadId);

        [DllImport("user32", EntryPoint = "UnhookWindowsHookEx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int UnhookWindowsHookEx(int hHook);
        public delegate int LowLevelKeyboardProcDelegate(int nCode, int wParam, ref KBDLLHOOKSTRUCT lParam);

        [DllImport("user32", EntryPoint = "CallNextHookEx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int CallNextHookEx(int hHook, int nCode, int wParam, ref KBDLLHOOKSTRUCT lParam);
        
        /*code needed to disable start menu*/
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string className, string windowText);


        [DllImport("user32.dll")]
        public static extern int ShowWindow(int hwnd, int command);

        [DllImport("user32.dll")]
        public static extern Int32 GetScrollRange(IntPtr hWnd, UInt32 nBar, out Int32 lpMinPos, out Int32 lpMaxPos);

        [DllImport("user32.dll")]
        public static extern Int32 GetScrollBarInfo(IntPtr hWnd, UInt32 idObject, ref SCROLLBARINFO psbi);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, ref COPYDATASTRUCT lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, StringBuilder lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, [MarshalAs(UnmanagedType.LPStr)] string lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessageW")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, UInt32 Msg, IntPtr wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, Int32 wParam, Int32 lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageA(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SendMessage(HandleRef hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessage(int hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

        [DllImport("gdi32.dll", EntryPoint = "AddFontResourceW", SetLastError = true)]
        public static extern int AddFontResource([In][MarshalAs(UnmanagedType.LPWStr)] string lpFileName);

        [DllImport("advapi32.dll", EntryPoint = "RegOpenKeyEx")]
        public static extern int RegOpenKeyExA(int hKey, string lpSubKey, int ulOptions, int samDesired, ref int phkResult);

        [DllImportAttribute("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SetProcessWorkingSetSize(IntPtr process, int minimumWorkingSetSize, int maximumWorkingSetSize);

        [DllImport("imm32.dll")]
        private static extern IntPtr ImmGetDefaultIMEWnd(IntPtr hWnd);

        private const int WM_IME_CONTROL = 643;

        #region //IMEMODE
        Int32 ALPHANUMERIC = 0x0;
        Int32 NATIVE = 0x1;
        Int32 FULLSHAPE = 0x8;
        Int32 ROMAN = 0x10;
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr IParam);
        #endregion //IMEMODE

        public static string GetIME(string ExeName)
        {
            string rtnVal = "ENG";
            //ExeName = PSMH
            Process p = Process.GetProcessesByName(ExeName).FirstOrDefault();

            if (p == null)
                return rtnVal;

            IntPtr hwnd = p.MainWindowHandle;
            IntPtr hime = ImmGetDefaultIMEWnd(hwnd);
            IntPtr status = SendMessage(hime, WM_IME_CONTROL, new IntPtr(0x5), new IntPtr(0));

            if (status.ToInt32() != 0)
            {
                //Console.WriteLine(p.MainWindowTitle + " 한글입니다."); //한글
                rtnVal = "KOR";
            }
            else
            {
                //Console.WriteLine(p.MainWindowTitle + " 영문입니다."); //영문
            }
            return rtnVal;
        }

        /// <summary>
        /// Description : 특정프로그램이 실행중인지 알아내기 위함
        /// Author : 박병규
        /// Create Date : 2017.09.18
        /// </summary>
        public void Tablet_Shell_Check(string ArgEXE, string ArgGubun)
        {
            //// 윈도우 타이틀명으로 핸들을 찾는다 
            //IntPtr hWnd = FindWindow(null, ArgGubun);
            //if (!hWnd.Equals(IntPtr.Zero))
            //{
            //    IntPtr RetVal = SendMessageA(hWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            //}

            System.Diagnostics.Process[] procs = System.Diagnostics.Process.GetProcessesByName(ArgEXE);

            if (procs.Length > 0)
            {
                // 윈도우 타이틀명으로 핸들을 찾는다
                IntPtr hWnd = FindWindow(null, ArgGubun);

                //// 중복실행시 처리
                procs[0].Kill();

                //if (!hWnd.Equals(IntPtr.Zero))
                //{
                //    procs[0].Kill();
                //}
            }
        }

        /// <summary>
        /// 메모리 해제용도로 사용 : 권장하지 않음
        /// Author : 박웅규
        /// Create Date : 2017.12.08
        /// </summary>
        public static void FlushMemoryEx()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            }

            //try
            //{
            //    GC.AddMemoryPressure(90000000);
            //}
            //catch(OutOfMemoryException)
            //{
            //    GC.RemoveMemoryPressure(90000000);
            //}
            //GC.RemoveMemoryPressure(90000000);
        }

        public static void FlushMemory()
        {
            //GC.Collect();
            //GC.WaitForPendingFinalizers();
            //GC.Collect();
        }

        /// <summary>
        /// 컨트롤을 그릴때 화면을 갱신하는 것을 막는다
        /// </summary>
        /// <param name="parent"></param>
        public static void SuspendDrawing(Control parent)
        {
            SendMessage(parent.Handle, WM_SETREDRAW, false, 0);
        }

        /// <summary>
        /// 화면을 갱신한다
        /// </summary>
        /// <param name="parent"></param>
        public static void ResumeDrawing(Control parent)
        {
            SendMessage(parent.Handle, WM_SETREDRAW, true, 0);
            parent.Refresh();
        }


    }
}
