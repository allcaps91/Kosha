using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComDbB
{
    public class PsmhFunc
    {
        /// <summary>
        /// 한글 2BYTE로 계산해서 문자개수 리턴
        /// </summary>
        /// <param name="strExp"></param>
        /// <returns></returns>
        public long GetWordByByte(string strExp)
        {
            System.Text.Encoding Enchar = null;
            Enchar = System.Text.Encoding.GetEncoding("ks_c_5601-1987");

            byte[] buf = Enchar.GetBytes(strExp);

            return buf.Length;
        }

        /// <summary>
        /// BYTE용 MID 함수
        /// </summary>
        /// <param name="strExp"></param>
        /// <param name="intLR"></param>
        /// <param name="intLen"></param>
        /// <returns></returns>
        public string GetMidStr(string strExp, int intLR, int intLen)
        {
            System.Text.Encoding Enchar = null;
            Enchar = System.Text.Encoding.GetEncoding("ks_c_5601-1987");
            byte[] buf = Enchar.GetBytes(strExp);

            //Right
            if (intLR > 0)
            {
                intLR = buf.Length - intLen;
            }

            return Enchar.GetString(buf, intLR, intLen);
        }

    }
}
