using System;
using System.Text;

namespace ComDbB
{
    public class PsmhAES
    {
        #region //DB 접속정보 암호화 파일
        /// <summary>
        /// 복호화
        /// </summary>
        /// <param name="base64EncodedData"></param>
        /// <returns></returns>
        public string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <summary>
        /// 암호화
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// INI 정보 복호화
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string Cert(string str)
        {
            string strRet = "";

            str = Base64Decode(str);

            for (int i = 0; i < str.Length; i += 3)
            {
                strRet = strRet + str.Substring(i, 1);
            }

            return strRet;
        }

        #endregion //DB 접속정보 암호화 파일
    }
}
