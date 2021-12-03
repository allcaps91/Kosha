using System;
using System.Security.Cryptography;
using System.Text;

namespace ComBase
{
    /// <summary>
    /// 패스워드 해쉬 256,512
    /// </summary>
    public class clsSHA
    {
        /// <summary>
        /// 해쉬 256
        /// </summary>
        /// <param name="InputText"></param>
        /// <returns></returns>
        public static string SHA256(string InputText)
        {
            SHA256Managed crypt = new SHA256Managed();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(InputText), 0, Encoding.ASCII.GetByteCount(InputText));
            foreach (byte theByte in crypto)
            {
                hash += theByte.ToString("x2");
            }
            return hash;
        }

        /// <summary>
        /// 해쉬 512
        /// </summary>
        /// <param name="InputText"></param>
        /// <returns></returns>
        public static string SHA512(string InputText)
        {
            SHA512Managed crypt = new SHA512Managed();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(InputText), 0, Encoding.ASCII.GetByteCount(InputText));
            foreach (byte theByte in crypto)
            {
                hash += theByte.ToString("x2");
            }
            return hash;
        }

    }
}
