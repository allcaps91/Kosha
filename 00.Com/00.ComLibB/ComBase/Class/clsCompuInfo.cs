using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;

/// <summary>
/// 컴퓨터 정보
/// </summary>
namespace ComBase
{
    /// <summary>
    /// 컴퓨터 정보
    /// </summary>
    public class clsCompuInfo
    {
        public static string gstrCOMIP = "";    //컴퓨터 IP
        public static string gstrComNm = "";    //컴퓨터 명
        public static string gstrMacAddr = "";  //컴퓨터 맥어드레스

        /// <summary>
        /// 컴퓨터 정보를 변수에 할당한다
        /// </summary>
        public static void SetComputerInfo()
        {
            string host = Dns.GetHostName();
            IPHostEntry ip = Dns.GetHostEntry(host);
            for (int i = 0; i < ip.AddressList.Length; i++)
            {
                if (ip.AddressList[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    clsCompuInfo.gstrCOMIP = ip.AddressList[i].ToString();
                }
            }
            clsCompuInfo.gstrComNm = SystemInformation.ComputerName;
            clsCompuInfo.gstrMacAddr = NetworkInterface.GetAllNetworkInterfaces()[0].GetPhysicalAddress().ToString();
        }

        /// <summary>
        /// 컴퓨터 맥 어드래스 가져오기
        /// </summary>
        /// <returns></returns>
        public static string GetMACAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            return sMacAddress;
        }
    }
}
