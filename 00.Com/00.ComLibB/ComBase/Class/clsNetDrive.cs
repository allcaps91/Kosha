using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ComBase
{
    public class clsNetDrive
    {
        // 구조체 선언
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct NETRESOURCE
        {
            public uint dwScope;
            public uint dwType;
            public uint dwDisplayType;
            public uint dwUsage;
            public string lpLocalName;
            public string lpRemoteName;
            public string lpComment;
            public string lpProvider;
        }

        [DllImport("mpr.dll", CharSet = CharSet.Auto)]
        public static extern int WNetUseConnection(
                    IntPtr hwndOwner,
                    [MarshalAs(UnmanagedType.Struct)] ref NETRESOURCE lpNetResource,
                    string lpPassword,
                    string lpUserID,
                    uint dwFlags,
                    StringBuilder lpAccessName,
                    ref int lpBufferSize,
                    out uint lpResult);

        [DllImport("mpr.dll", EntryPoint = "WNetCancelConnection2", CharSet = CharSet.Auto)]
        public static extern int WNetCancelConnection2A(string lpName, int dwFlags, int fForce);


        //// 공유연결
        public static bool ConnectRemoteServer(string server, string strUserId, string strPassWord)
        {
            bool functionReturnValue = false;
            int capacity = 64;
            uint resultFlags = 0;
            uint flags = 0;
            System.Text.StringBuilder sb = new System.Text.StringBuilder(capacity);
            NETRESOURCE ns = new NETRESOURCE();

            ns.dwType = 1;
            // 공유 디스크
            ns.lpLocalName = "";
            // 로컬 드라이브 지정하지 않음
            ns.lpRemoteName = server;
            ns.lpProvider = "";

            int result = 0;

            result = WNetUseConnection(IntPtr.Zero, ref ns, strPassWord, strUserId, flags, sb, ref capacity, out resultFlags);

            // 0: 정상
            // 1219 : 이미 존재(연결되어 있음)

            //1203 : 경로에 오류가 있을 경우
            //1326 : 사용자/암호 일치 않음

            if (result == 0 | result == 1219)
            {
                functionReturnValue = true;
            }
            else
            {
                functionReturnValue = false;
            }

            return functionReturnValue;
        }

        // 공유해제
        public static void CencelRemoteServer(string server)
        {
            WNetCancelConnection2A(server, 1, 0);
        }

    }
}
