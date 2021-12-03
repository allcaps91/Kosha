using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    public class clsConPatInfo
    {
        /// <summary>
        /// 감염정보 이미지 Load
        /// </summary>
        /// <param name="sGubun"></param>
        /// <returns></returns>
        public static object InfactResource(string sGubun)
        {
            // 1    2    3    4    5    6    7
            //혈액 접촉 격리 공기 비말  보호 해외
            object rtnVal = null;
            if (sGubun == "0000001")
            {
                rtnVal = Properties.Resources.I0000001;
            }
            else if (sGubun == "0000010")
            {
                rtnVal = Properties.Resources.I0000010;
            }
            else if (sGubun == "0000011")
            {
                rtnVal = Properties.Resources.I0000011;
            }
            else if (sGubun == "0000100")
            {
                rtnVal = Properties.Resources.I0000100;
            }
            else if (sGubun == "0000101")
            {
                rtnVal = Properties.Resources.I0000101;
            }
            else if (sGubun == "0000110")
            {
                rtnVal = Properties.Resources.I0000110;
            }
            else if (sGubun == "0000111")
            {
                rtnVal = Properties.Resources.I0000111;
            }
            else if (sGubun == "0001000")
            {
                rtnVal = Properties.Resources.I0001000;
            }
            else if (sGubun == "0001001")
            {
                rtnVal = Properties.Resources.I0001001;
            }
            else if (sGubun == "0001010")
            {
                rtnVal = Properties.Resources.I0001010;
            }
            else if (sGubun == "0001011")
            {
                rtnVal = Properties.Resources.I0001011;
            }
            else if (sGubun == "0001100")
            {
                rtnVal = Properties.Resources.I0001100;
            }
            else if (sGubun == "0001101")
            {
                rtnVal = Properties.Resources.I0001101;
            }
            else if (sGubun == "0001110")
            {
                rtnVal = Properties.Resources.I0001110;
            }
            else if (sGubun == "0001111")
            {
                rtnVal = Properties.Resources.I0001111;
            }
            else if (sGubun == "0010000")
            {
                rtnVal = Properties.Resources.I0010000;
            }
            else if (sGubun == "0010001")
            {
                rtnVal = Properties.Resources.I0010001;
            }
            else if (sGubun == "0010010")
            {
                rtnVal = Properties.Resources.I0010010;
            }
            else if (sGubun == "0010011")
            {
                rtnVal = Properties.Resources.I0010011;
            }
            else if (sGubun == "0010100")
            {
                rtnVal = Properties.Resources.I0010100;
            }
            else if (sGubun == "0010101")
            {
                rtnVal = Properties.Resources.I0010101;
            }
            else if (sGubun == "0010110")
            {
                rtnVal = Properties.Resources.I0010110;
            }
            else if (sGubun == "0010111")
            {
                rtnVal = Properties.Resources.I0010111;
            }
            else if (sGubun == "0011000")
            {
                rtnVal = Properties.Resources.I0011000;
            }
            else if (sGubun == "0011001")
            {
                rtnVal = Properties.Resources.I0011001;
            }
            else if (sGubun == "0011010")
            {
                rtnVal = Properties.Resources.I0011010;
            }
            else if (sGubun == "0011011")
            {
                rtnVal = Properties.Resources.I0011011;
            }
            else if (sGubun == "0011100")
            {
                rtnVal = Properties.Resources.I0011100;
            }
            else if (sGubun == "0011101")
            {
                rtnVal = Properties.Resources.I0011101;
            }
            else if (sGubun == "0011110")
            {
                rtnVal = Properties.Resources.I0011110;
            }
            else if (sGubun == "0011111")
            {
                rtnVal = Properties.Resources.I0011111;
            }
            else if (sGubun == "0100000")
            {
                rtnVal = Properties.Resources.I0100000;
            }
            else if (sGubun == "0100001")
            {
                rtnVal = Properties.Resources.I0100001;
            }
            else if (sGubun == "0100010")
            {
                rtnVal = Properties.Resources.I0100010;
            }
            else if (sGubun == "0100011")
            {
                rtnVal = Properties.Resources.I0100011;
            }
            else if (sGubun == "0100100")
            {
                rtnVal = Properties.Resources.I0100100;
            }
            else if (sGubun == "0100101")
            {
                rtnVal = Properties.Resources.I0100101;
            }
            else if (sGubun == "0100110")
            {
                rtnVal = Properties.Resources.I0100110;
            }
            else if (sGubun == "0100111")
            {
                rtnVal = Properties.Resources.I0100111;
            }
            else if (sGubun == "0101000")
            {
                rtnVal = Properties.Resources.I0101000;
            }
            else if (sGubun == "0101001")
            {
                rtnVal = Properties.Resources.I0101001;
            }
            else if (sGubun == "0101010")
            {
                rtnVal = Properties.Resources.I0101010;
            }
            else if (sGubun == "0101011")
            {
                rtnVal = Properties.Resources.I0101011;
            }
            else if (sGubun == "0101100")
            {
                rtnVal = Properties.Resources.I0101100;
            }
            else if (sGubun == "0101101")
            {
                rtnVal = Properties.Resources.I0101101;
            }
            else if (sGubun == "0101110")
            {
                rtnVal = Properties.Resources.I0101110;
            }
            else if (sGubun == "0101111")
            {
                rtnVal = Properties.Resources.I0101111;
            }
            else if (sGubun == "0110000")
            {
                rtnVal = Properties.Resources.I0110000;
            }
            else if (sGubun == "0110001")
            {
                rtnVal = Properties.Resources.I0110001;
            }
            else if (sGubun == "0110010")
            {
                rtnVal = Properties.Resources.I0110010;
            }
            else if (sGubun == "0110011")
            {
                rtnVal = Properties.Resources.I0110011;
            }
            else if (sGubun == "0110100")
            {
                rtnVal = Properties.Resources.I0110100;
            }
            else if (sGubun == "0110101")
            {
                rtnVal = Properties.Resources.I0110101;
            }
            else if (sGubun == "0110110")
            {
                rtnVal = Properties.Resources.I0110110;
            }
            else if (sGubun == "0110111")
            {
                rtnVal = Properties.Resources.I0110111;
            }
            else if (sGubun == "0111000")
            {
                rtnVal = Properties.Resources.I0111000;
            }
            else if (sGubun == "0111001")
            {
                rtnVal = Properties.Resources.I0111001;
            }
            else if (sGubun == "0111010")
            {
                rtnVal = Properties.Resources.I0111010;
            }
            else if (sGubun == "0111011")
            {
                rtnVal = Properties.Resources.I0111011;
            }
            else if (sGubun == "0111100")
            {
                rtnVal = Properties.Resources.I0111100;
            }
            else if (sGubun == "0111101")
            {
                rtnVal = Properties.Resources.I0111101;
            }
            else if (sGubun == "0111110")
            {
                rtnVal = Properties.Resources.I0111110;
            }
            else if (sGubun == "0111111")
            {
                rtnVal = Properties.Resources.I0111111;
            }
            else if (sGubun == "1000000")
            {
                rtnVal = Properties.Resources.I1000000;
            }
            else if (sGubun == "1000001")
            {
                rtnVal = Properties.Resources.I1000001;
            }
            else if (sGubun == "1000010")
            {
                rtnVal = Properties.Resources.I1000010;
            }
            else if (sGubun == "1000011")
            {
                rtnVal = Properties.Resources.I1000011;
            }
            else if (sGubun == "1000100")
            {
                rtnVal = Properties.Resources.I1000100;
            }
            else if (sGubun == "1000101")
            {
                rtnVal = Properties.Resources.I1000101;
            }
            else if (sGubun == "1000110")
            {
                rtnVal = Properties.Resources.I1000110;
            }
            else if (sGubun == "1000111")
            {
                rtnVal = Properties.Resources.I1000111;
            }
            else if (sGubun == "1001000")
            {
                rtnVal = Properties.Resources.I1001000;
            }
            else if (sGubun == "1001001")
            {
                rtnVal = Properties.Resources.I1001001;
            }
            else if (sGubun == "1001010")
            {
                rtnVal = Properties.Resources.I1001010;
            }
            else if (sGubun == "1001011")
            {
                rtnVal = Properties.Resources.I1001011;
            }
            else if (sGubun == "1001100")
            {
                rtnVal = Properties.Resources.I1001100;
            }
            else if (sGubun == "1001101")
            {
                rtnVal = Properties.Resources.I1001101;
            }
            else if (sGubun == "1001110")
            {
                rtnVal = Properties.Resources.I1001110;
            }
            else if (sGubun == "1001111")
            {
                rtnVal = Properties.Resources.I1001111;
            }
            else if (sGubun == "1010000")
            {
                rtnVal = Properties.Resources.I1010000;
            }
            else if (sGubun == "1010001")
            {
                rtnVal = Properties.Resources.I1010001;
            }
            else if (sGubun == "1010010")
            {
                rtnVal = Properties.Resources.I1010010;
            }
            else if (sGubun == "1010011")
            {
                rtnVal = Properties.Resources.I1010011;
            }
            else if (sGubun == "1010100")
            {
                rtnVal = Properties.Resources.I1010100;
            }
            else if (sGubun == "1010101")
            {
                rtnVal = Properties.Resources.I1010101;
            }
            else if (sGubun == "1010110")
            {
                rtnVal = Properties.Resources.I1010110;
            }
            else if (sGubun == "1010111")
            {
                rtnVal = Properties.Resources.I1010111;
            }
            else if (sGubun == "1011000")
            {
                rtnVal = Properties.Resources.I1011000;
            }
            else if (sGubun == "1011001")
            {
                rtnVal = Properties.Resources.I1011001;
            }
            else if (sGubun == "1011010")
            {
                rtnVal = Properties.Resources.I1011010;
            }
            else if (sGubun == "1011011")
            {
                rtnVal = Properties.Resources.I1011011;
            }
            else if (sGubun == "1011100")
            {
                rtnVal = Properties.Resources.I1011100;
            }
            else if (sGubun == "1011101")
            {
                rtnVal = Properties.Resources.I1011101;
            }
            else if (sGubun == "1011110")
            {
                rtnVal = Properties.Resources.I1011110;
            }
            else if (sGubun == "1011111")
            {
                rtnVal = Properties.Resources.I1011111;
            }
            else if (sGubun == "1100000")
            {
                rtnVal = Properties.Resources.I1100000;
            }
            else if (sGubun == "1100001")
            {
                rtnVal = Properties.Resources.I1100001;
            }
            else if (sGubun == "1100010")
            {
                rtnVal = Properties.Resources.I1100010;
            }
            else if (sGubun == "1100011")
            {
                rtnVal = Properties.Resources.I1100011;
            }
            else if (sGubun == "1100100")
            {
                rtnVal = Properties.Resources.I1100100;
            }
            else if (sGubun == "1100101")
            {
                rtnVal = Properties.Resources.I1100101;
            }
            else if (sGubun == "1100110")
            {
                rtnVal = Properties.Resources.I1100110;
            }
            else if (sGubun == "1100111")
            {
                rtnVal = Properties.Resources.I1100111;
            }
            else if (sGubun == "1101000")
            {
                rtnVal = Properties.Resources.I1101000;
            }
            else if (sGubun == "1101001")
            {
                rtnVal = Properties.Resources.I1101001;
            }
            else if (sGubun == "1101010")
            {
                rtnVal = Properties.Resources.I1101010;
            }
            else if (sGubun == "1101011")
            {
                rtnVal = Properties.Resources.I1101011;
            }
            else if (sGubun == "1101100")
            {
                rtnVal = Properties.Resources.I1101100;
            }
            else if (sGubun == "1101101")
            {
                rtnVal = Properties.Resources.I1101101;
            }
            else if (sGubun == "1101110")
            {
                rtnVal = Properties.Resources.I1101110;
            }
            else if (sGubun == "1101111")
            {
                rtnVal = Properties.Resources.I1101111;
            }
            else if (sGubun == "1110000")
            {
                rtnVal = Properties.Resources.I1110000;
            }
            else if (sGubun == "1110001")
            {
                rtnVal = Properties.Resources.I1110001;
            }
            else if (sGubun == "1110010")
            {
                rtnVal = Properties.Resources.I1110010;
            }
            else if (sGubun == "1110011")
            {
                rtnVal = Properties.Resources.I1110011;
            }
            else if (sGubun == "1110100")
            {
                rtnVal = Properties.Resources.I1110100;
            }
            else if (sGubun == "1110101")
            {
                rtnVal = Properties.Resources.I1110101;
            }
            else if (sGubun == "1110110")
            {
                rtnVal = Properties.Resources.I1110110;
            }
            else if (sGubun == "1110111")
            {
                rtnVal = Properties.Resources.I1110111;
            }
            else if (sGubun == "1111000")
            {
                rtnVal = Properties.Resources.I1111000;
            }
            else if (sGubun == "1111001")
            {
                rtnVal = Properties.Resources.I1111001;
            }
            else if (sGubun == "1111010")
            {
                rtnVal = Properties.Resources.I1111010;
            }
            else if (sGubun == "1111011")
            {
                rtnVal = Properties.Resources.I1111011;
            }
            else if (sGubun == "1111100")
            {
                rtnVal = Properties.Resources.I1111100;
            }
            else if (sGubun == "1111101")
            {
                rtnVal = Properties.Resources.I1111101;
            }
            else if (sGubun == "1111110")
            {
                rtnVal = Properties.Resources.I1111110;
            }
            else if (sGubun == "1111111")
            {
                rtnVal = Properties.Resources.I1111111;
            }

            return rtnVal;
        }
    }
}
