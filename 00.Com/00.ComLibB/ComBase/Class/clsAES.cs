using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Data;
using ComDbB; //DB연결

namespace ComBase 
{ 
    /// <summary>
    /// 암호화, 목호화
    /// </summary>
    public class clsAES
    {
        public static string GstrAesJumin1 = "";
        public static string GstrAesJumin2 = "";
        public static string GstrAesJuminF = "";  //뒷자리 별표처리

        private const string AES_PASS = "fUuEt/RHUhMinUkf/nvU=ZCEilDRm96736bVsvLS71n+";
        private const string AES_PASS_STR = "2894349054";
        private static string AES_PASS_NEW = "";

        public const string PSMH_INI = @"C:\cmc\psmh.ini"; //DB접속정보

        #region //AES256암복화
        /// <summary>
        /// AES256암복화 : AES_Pass_SET
        /// </summary>
        /// <param name="InputText"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        private static void AesPassSet(string InputText, string Password)
        {
            clsAES aes256 = new clsAES();

            string ss = InputText;

            ss = VB.Right(ss, ss.Length - 21) + VB.Left(ss, 21);


            AES_PASS_NEW = aes256.DES(ss, Password);
        }

        /// <summary>
        /// AES 키와 코드를 가지고 온다
        /// </summary>
        /// <returns></returns>
        private static void GetAESKeyAndCode(ref string strKeyCode, ref string strKey)
        {
            strKeyCode = "0000";
            strKey = "2894349054";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT  ";
                SQL = SQL + ComNum.VBLF + "    KEYCODE, KEY  ";
                SQL = SQL + ComNum.VBLF + "FROM ENCRYPTKEYSET ";
                SQL = SQL + ComNum.VBLF + "WHERE STARTDATE <= TO_CHAR(SYSDATE , 'YYYYMMDD') ";
                SQL = SQL + ComNum.VBLF + "AND ENDDATE >= TO_CHAR(SYSDATE , 'YYYYMMDD') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }
                strKeyCode = dt.Rows[0]["KEYCODE"].ToString().Trim();
                strKey = dt.Rows[0]["KEY"].ToString().Trim();
                dt.Dispose();
                dt = null;
                return;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        /// <summary>
        /// AES Key를 가지고 온다
        /// </summary>
        /// <param name="strKeyCode"></param>
        /// <returns></returns>
        private static string GetAESKeyByKeyCode(string strKeyCode)
        {
            string rtnVal = "2894349054";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT  ";
                SQL = SQL + ComNum.VBLF + "    KEYCODE, KEY  ";
                SQL = SQL + ComNum.VBLF + "FROM ENCRYPTKEYSET ";
                SQL = SQL + ComNum.VBLF + "WHERE KEYCODE = TO_CHAR(SYSDATE , 'YYYYMMDD') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return rtnVal;
                }
                rtnVal = dt.Rows[0]["KEY"].ToString().Trim();
                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        /// <summary>
        /// 암호화
        /// </summary>
        /// <param name="InputText"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static string AES(string InputText, string pass = "")
        {
            string EncryptedData = "";

            if (AES_PASS_NEW == "") AesPassSet(AES_PASS, AES_PASS_STR);

            string Password = AES_PASS_NEW;
            if (pass != "") Password = pass;

            RijndaelManaged RijndaelCipher = new RijndaelManaged();

            byte[] PlainText = System.Text.Encoding.Unicode.GetBytes(InputText);
            byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());


            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);
            ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));

            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);

            cryptoStream.Write(PlainText, 0, PlainText.Length);

            cryptoStream.FlushFinalBlock();

            byte[] CipherBytes = memoryStream.ToArray();

            memoryStream.Close();
            cryptoStream.Close();

            EncryptedData = Convert.ToBase64String(CipherBytes);

            return EncryptedData;

        }

        public static string AES_X(string InputText, string pass = "")
        {
            string EncryptedData = "";
            string strKeyCode = "0000";
            string strKey = "2894349054";

            GetAESKeyAndCode(ref strKeyCode, ref strKey);

            //if (AES_PASS_NEW == "") AesPassSet(AES_PASS, AES_PASS_STR);
            if (AES_PASS_NEW == "") AesPassSet(AES_PASS, strKey);

            string Password = AES_PASS_NEW;
            if (pass != "") Password = pass;

            RijndaelManaged RijndaelCipher = new RijndaelManaged();

            byte[] PlainText = System.Text.Encoding.Unicode.GetBytes(InputText);
            byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());


            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);
            ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));

            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);

            cryptoStream.Write(PlainText, 0, PlainText.Length);

            cryptoStream.FlushFinalBlock();

            byte[] CipherBytes = memoryStream.ToArray();

            memoryStream.Close();
            cryptoStream.Close();

            EncryptedData = Convert.ToBase64String(CipherBytes);

            EncryptedData = EncryptedData + " " + AESKey(strKeyCode);
            return EncryptedData;

        }

        public static string AESKey(string InputText, string pass = "")
        {
            string EncryptedData = "";

            if (AES_PASS_NEW == "") AesPassSet(AES_PASS, AES_PASS_STR);

            string Password = AES_PASS_NEW;
            if (pass != "") Password = pass;

            RijndaelManaged RijndaelCipher = new RijndaelManaged();

            byte[] PlainText = System.Text.Encoding.Unicode.GetBytes(InputText);
            byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());


            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);
            ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));

            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);

            cryptoStream.Write(PlainText, 0, PlainText.Length);

            cryptoStream.FlushFinalBlock();

            byte[] CipherBytes = memoryStream.ToArray();

            memoryStream.Close();
            cryptoStream.Close();

            EncryptedData = Convert.ToBase64String(CipherBytes);

            return EncryptedData;

        }

        /// <summary>
        /// 복호화
        /// </summary>
        /// <param name="InputText"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static string DeAES(string InputText, string pass = "")
        {
            string DecryptedData = "";

            //2017.07.19.김홍록: 널값을 보낼 경우에 대한 처리 추가
            if (string.IsNullOrEmpty(InputText) == true)
            {
                return "";
            }

            try
            {

                if (AES_PASS_NEW == "") AesPassSet(AES_PASS, AES_PASS_STR);

                string Password = AES_PASS_NEW;
                if (pass != "") Password = pass;

                RijndaelManaged RijndaelCipher = new RijndaelManaged();


                byte[] EncryptedData = Convert.FromBase64String(InputText);
                byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());

                PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);

                ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
                MemoryStream memoryStream = new MemoryStream(EncryptedData);

                CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);

                byte[] PlainText = new byte[EncryptedData.Length];

                int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);


                memoryStream.Close();
                cryptoStream.Close();
                DecryptedData = Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);
            }
            catch (Exception)
            {
                return "";
            }

            return DecryptedData;

        }

        public static string DeAES_X(string InputText, string pass = "")
        {
            string DecryptedData = "";
            string strKeyCode = "0000";
            string strKey = "2894349054";
            string strArry = "";

            //2017.07.19.김홍록: 널값을 보낼 경우에 대한 처리 추가
            if (string.IsNullOrEmpty(InputText) == true)
            {
                return "";
            }

            try
            {
                if (InputText.Length > 25)
                {
                    if (InputText.IndexOf(" ") >= 0)
                    {
                        InputText = ComFunc.SptChar(InputText, 0, " ").Trim();
                        strKeyCode = DeAESKey(ComFunc.SptChar(InputText, 1, " ").Trim(), "");
                        strKey = GetAESKeyByKeyCode(strKeyCode);
                    }
                    //InputText = VB.Left(InputText, 24).Trim();
                    //strKeyCode = DeAESKey(VB.Right(InputText, 24).Trim(), "");
                    //strKey = GetAESKeyByKeyCode(strKeyCode);
                }

                //if (AES_PASS_NEW == "") AesPassSet(AES_PASS, AES_PASS_STR);
                if (AES_PASS_NEW == "") AesPassSet(AES_PASS, strKey);

                string Password = AES_PASS_NEW;
                if (pass != "") Password = pass;

                RijndaelManaged RijndaelCipher = new RijndaelManaged();


                byte[] EncryptedData = Convert.FromBase64String(InputText);
                byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());

                PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);

                ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
                MemoryStream memoryStream = new MemoryStream(EncryptedData);

                CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);

                byte[] PlainText = new byte[EncryptedData.Length];

                int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);


                memoryStream.Close();
                cryptoStream.Close();
                DecryptedData = Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);
            }
            catch (Exception)
            {
                return "";
            }

            return DecryptedData;

        }

        public static string DeAESKey(string InputText, string pass = "")
        {
            string DecryptedData = "";

            //2017.07.19.김홍록: 널값을 보낼 경우에 대한 처리 추가
            if (string.IsNullOrEmpty(InputText) == true)
            {
                return "";
            }

            try
            {
                if (AES_PASS_NEW == "") AesPassSet(AES_PASS, AES_PASS_STR);

                string Password = AES_PASS_NEW;
                if (pass != "") Password = pass;

                RijndaelManaged RijndaelCipher = new RijndaelManaged();


                byte[] EncryptedData = Convert.FromBase64String(InputText);
                byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());

                PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);

                ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
                MemoryStream memoryStream = new MemoryStream(EncryptedData);

                CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);

                byte[] PlainText = new byte[EncryptedData.Length];

                int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);


                memoryStream.Close();
                cryptoStream.Close();
                DecryptedData = Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);
            }
            catch (Exception)
            {
                return "";
            }

            return DecryptedData;

        }

        /// <summary>
        /// 복호화
        /// </summary>
        /// <param name="InputText"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        private string DES(string InputText, string pass = "")
        {
            string DecryptedData = "";

            //2017.07.19.김홍록: 널값을 보낼 경우에 대한 처리 추가
            if (string.IsNullOrEmpty(InputText) == true)
            {
                return "";
            }

            try
            {
                string Password = AES_PASS_NEW;
                if (pass != "") Password = pass;

                RijndaelManaged RijndaelCipher = new RijndaelManaged();

                byte[] EncryptedData = Convert.FromBase64String(InputText);
                byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());

                PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);

                ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
                MemoryStream memoryStream = new MemoryStream(EncryptedData);

                CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);

                byte[] PlainText = new byte[EncryptedData.Length];

                int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);

                memoryStream.Close();
                cryptoStream.Close();

                DecryptedData = Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);
            }
            catch (Exception)
            {
                return "";
            }

            return DecryptedData;

        }

        #endregion //AES256암복화

        #region //DB 접속정보 암호화 파일
        /// <summary>
        /// 복호화
        /// </summary>
        /// <param name="base64EncodedData"></param>
        /// <returns></returns>
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <summary>
        /// 암호화
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// INI 정보 복호화
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Cert(string str)
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

        /// <summary>
        /// DataBase안에 있는 주민번호 복호화
        /// </summary>
        /// <param name="pDbCon">커넥션 객체</param>
        /// <param name="ArgString"></param>
        /// <param name="ArgGb"></param>
        /// <returns></returns>
        public static string Read_Jumin_AES(PsmhDb pDbCon, string ArgString, string ArgGb = "")
        {
            string SQL = "";    //Query문
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            string rtnVal = "";
            string strJumin1 = "";
            string strJumin2 = "";
            string strJumin3 = "";

            GstrAesJumin1 = "";
            GstrAesJumin2 = "";
            GstrAesJuminF = "";

            try
            {
                if (ArgGb == "" || ArgGb == "1") //환자 마스트 read
                {
                    SQL = " SELECT JUMIN1, JUMIN2, JUMIN3 FROM ADMIN.BAS_PATIENT ";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + ArgString + "' ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strJumin1 = dt.Rows[0]["JUMIN1"].ToString().Trim();
                        strJumin2 = dt.Rows[0]["JUMIN2"].ToString().Trim();
                        if (dt.Rows.Count > 0)
                        {
                            strJumin3 = DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim());
                            rtnVal = strJumin1 + strJumin3;
                            GstrAesJumin1 = strJumin1;
                            GstrAesJumin2 = strJumin3;
                        }
                        else
                        {
                            rtnVal = strJumin1 + strJumin2;
                            GstrAesJumin1 = strJumin1;
                            GstrAesJumin2 = strJumin2;
                        }
                        GstrAesJuminF = GstrAesJumin1 + VB.Left(GstrAesJumin2, 1) + "******";
                    }
                    dt.Dispose();
                    dt = null;
                }
                else if (ArgGb == "2") //인사 read
                {
                    SQL = " SELECT JUMIN, JUMIN3 FROM ADMIN.INSA_MST ";
                    SQL = SQL + ComNum.VBLF + " WHERE SABUN = '" + ArgString.Trim() + "' ";
                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                    if (dt.Rows.Count > 0)
                    {
                        strJumin1 = VB.Left(dt.Rows[0]["JUMIN"].ToString().Trim(), 6);
                        strJumin2 = VB.Right(dt.Rows[0]["JUMIN"].ToString().Trim(), dt.Rows[0]["JUMIN"].ToString().Trim().Length - 6);

                        if (dt.Rows[0]["Jumin3"].ToString().Trim() != "")
                        {
                            strJumin3 = DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim());
                            rtnVal = strJumin3;
                            GstrAesJumin1 = VB.Left(strJumin3, 6);
                            GstrAesJumin2 = VB.Right(strJumin3, strJumin3.Length - 6);
                        }
                        else
                        {
                            rtnVal = strJumin1;
                            GstrAesJumin1 = VB.Left(strJumin1, 6);
                            GstrAesJumin2 = VB.Right(strJumin1, strJumin1.Length - 6);
                        }
                        GstrAesJuminF = GstrAesJumin1 + VB.Left(GstrAesJumin2, 1) + "******";
                    }
                    dt.Dispose();
                    dt = null;
                }
                else if (ArgGb == "3") //건강보험 청구내역
                {
                    SQL = " SELECT JUMIN1, JUMIN2, JUMIN3 FROM ADMIN.MIR_INSID";
                    SQL = SQL + ComNum.VBLF + " WHERE WRTNO = '" + ArgString + "' ";
                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                    if (dt.Rows.Count > 0)
                    {
                        strJumin1 = dt.Rows[0]["JUMIN1"].ToString().Trim();
                        strJumin2 = dt.Rows[0]["JUMIN2"].ToString().Trim();

                        if (dt.Rows[0]["Jumin3"].ToString().Trim() != "")
                        {
                            strJumin3 = DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim());
                            rtnVal = strJumin1 + strJumin3;
                            GstrAesJumin1 = strJumin1;
                            GstrAesJumin2 = strJumin3;
                        }
                        else
                        {
                            rtnVal = strJumin1 + strJumin2;
                            GstrAesJumin1 = strJumin1;
                            GstrAesJumin2 = strJumin2;
                        }
                        GstrAesJuminF = GstrAesJumin1 + VB.Left(GstrAesJumin2, 1) + "******";
                    }
                    dt.Dispose();
                    dt = null;
                }
                else if (ArgGb == "4") //산재
                {
                    SQL = " SELECT JUMIN1, JUMIN2, JUMIN3 FROM ADMIN.MIR_SANID";
                    SQL = SQL + ComNum.VBLF + " WHERE WRTNO = '" + ArgString + "' ";
                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                    if (dt.Rows.Count > 0)
                    {
                        strJumin1 = dt.Rows[0]["JUMIN1"].ToString().Trim();
                        strJumin2 = dt.Rows[0]["JUMIN2"].ToString().Trim();

                        if (dt.Rows[0]["Jumin3"].ToString().Trim() != "")
                        {
                            strJumin3 = DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim());
                            rtnVal = strJumin1 + strJumin3;
                            GstrAesJumin1 = strJumin1;
                            GstrAesJumin2 = strJumin3;
                        }
                        else
                        {
                            rtnVal = strJumin1 + strJumin2;
                            GstrAesJumin1 = strJumin1;
                            GstrAesJumin2 = strJumin2;
                        }
                        GstrAesJuminF = GstrAesJumin1 + VB.Left(GstrAesJumin2, 1) + "******";
                    }
                    dt.Dispose();
                    dt = null;
                }
                else if (ArgGb == "5") //자보
                {
                    SQL = " SELECT JUMIN1, JUMIN2, JUMIN3 FROM ADMIN.MIR_TAID";
                    SQL = SQL + ComNum.VBLF + " WHERE WRTNO = '" + ArgString.Trim() + "' ";
                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                    if (dt.Rows.Count > 0)
                    {
                        strJumin1 = dt.Rows[0]["JUMIN1"].ToString().Trim();
                        strJumin2 = dt.Rows[0]["JUMIN2"].ToString().Trim();

                        if (dt.Rows[0]["Jumin3"].ToString().Trim() != "")
                        {
                            strJumin3 = DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim());
                            rtnVal = strJumin1 + strJumin3;
                            GstrAesJumin1 = strJumin1;
                            GstrAesJumin2 = strJumin3;
                        }
                        else
                        {
                            rtnVal = strJumin1 + strJumin2;
                            GstrAesJumin1 = strJumin1;
                            GstrAesJumin2 = strJumin2;
                        }
                        GstrAesJuminF = GstrAesJumin1 + VB.Left(GstrAesJumin2, 1) + "******";
                    }
                    dt.Dispose();
                    dt = null;
                }
                else if (ArgGb == "6") //계약처 청구
                {
                    SQL = " SELECT JUMIN1, JUMIN2, JUMIN3 FROM ADMIN.MIR_POID";
                    SQL = SQL + ComNum.VBLF + " WHERE WRTNO = '" + ArgString.Trim() + "' ";
                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                    if (dt.Rows.Count > 0)
                    {
                        strJumin1 = dt.Rows[0]["JUMIN1"].ToString().Trim();
                        strJumin2 = dt.Rows[0]["JUMIN2"].ToString().Trim();

                        if (dt.Rows[0]["Jumin3"].ToString().Trim() != "")
                        {
                            strJumin3 = DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim());
                            rtnVal = strJumin1 + strJumin3;
                            GstrAesJumin1 = strJumin1;
                            GstrAesJumin2 = strJumin3;
                        }
                        else
                        {
                            rtnVal = strJumin1 + strJumin2;
                            GstrAesJumin1 = strJumin1;
                            GstrAesJumin2 = strJumin2;
                        }
                        GstrAesJuminF = GstrAesJumin1 + VB.Left(GstrAesJumin2, 1) + "******";
                    }
                    dt.Dispose();
                    dt = null;
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }
    }
}
