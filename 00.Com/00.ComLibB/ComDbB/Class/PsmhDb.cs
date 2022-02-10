using System;
using System.Data;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace ComDbB
{
    public class PsmhDb
    {

        public string IdNumber { get; set; }
        public string CompIp { get; set; }
        public string CompName { get; set; }
        public string MacAddr { get; set; }
        public string SysDate { get; set; }
        public string SysTime { get; set; }
        public string DbOption { get; set; } //로컬 DB 사용하고 있는지

        private string gDataBase = null;
        //private  OracleConnection dc = null;
        //public  OracleTransaction trs = null;
        //private OracleCommand cmd = null;
        public OracleConnection Con = null;
        public OracleTransaction Trs = null;

        public string strDbIniFile = @"C:\HealthSoft\exenet\psmh.ini";
        public string strDevDbIniFile = @"C:\HealthSoft\exenet\psmhDev.ini";
        public string strSource = "PSMH";
        public string strDbIp = "";
        public string strDbPort = "";
        public string strUser = "ADMIN";
        public string strPassWord = "";
        public string strDbOption = "";

        //2020-08-12 DEV DB SOURCE
        public string psmhDev = "(DESCRIPTION="
            + "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.100.35)(PORT=1525)))"
            + "(CONNECT_DATA=(SERVICE_NAME=PSMH_DEV)))";

        //2020-08-25 DEV DB2 SOURCE
        public string psmhDev2 = "(DESCRIPTION="
            + "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.100.36)(PORT=1527)))"
            + "(CONNECT_DATA=(SERVICE_NAME=PSMH_DEV)))";

        /// <summary>
        /// DB 접속 비밀번호
        /// </summary>
        /// <returns></returns>
        private string GetPassWord(string pPassWord)
        {
            PsmhAES pAES = new PsmhAES();
            PsmhVB VB = new PsmhVB();

            string strPassWord = "";
            string strPassWordTmp = pAES.Base64Decode(pPassWord);


            int Spt = 3;
            for (int i = 0; i < strPassWordTmp.Length; i++)
            {
                if (i == 0)
                {
                    strPassWord = strPassWord + VB.Mid(strPassWordTmp, 1, 1);
                }
                else
                {
                    if (i % Spt == 0)
                    {
                        strPassWord = strPassWord + VB.Mid(strPassWordTmp, i + 1, 1);
                    }
                }
            }

            pAES = null;
            VB = null;
            return strPassWord;
        }

        /// <summary>
        /// DB접속 정보를 INI 파일에서 가지고 온다
        /// </summary>
        private void GetDbInfo()
        {
            PsmhAES pAES = new PsmhAES();
            PsmhIniFile myIniFile = new PsmhIniFile(strDbIniFile);

            strDbIp = myIniFile.ReadValue("DBINFO", "DbIp", "");
            strDbPort = myIniFile.ReadValue("DBINFO", "DbPort", "");
            strSource = pAES.Base64Decode(myIniFile.ReadValue("DBINFO", "Source", "").Replace("^", "="));
            if (strSource.Equals("PSMH_DEV"))
            {
                strSource = psmhDev;
            }
            strUser = pAES.Base64Decode(myIniFile.ReadValue("DBINFO", "User", "").Replace("^", "="));
            strPassWord = myIniFile.ReadValue("DBINFO", "PassWord", "").Replace("^", "=");
            strPassWord = GetPassWord(strPassWord);

            strDbOption = myIniFile.ReadValue("DBOPTION", "Option", "");

            pAES = null;
            myIniFile = null;
        }

        private void GetDbInfoEx()
        {
            PsmhAES pAES = new PsmhAES();
            PsmhIniFile myIniFile = new PsmhIniFile(strDbIniFile);

            strDbIp = myIniFile.ReadValue("DBINFO", "DbIp", "");
            strDbPort = myIniFile.ReadValue("DBINFO", "DbPort", "");
            strSource = pAES.Base64Decode(myIniFile.ReadValue("DBINFO", "Source", "").Replace("^", "="));
            if (strSource.Equals("PSMH_DEV"))
            {
                strSource = psmhDev2;
            }
            strUser = pAES.Base64Decode(myIniFile.ReadValue("DBINFO", "User", "").Replace("^", "="));
            strPassWord = myIniFile.ReadValue("DBINFO", "PassWord", "").Replace("^", "=");
            strPassWord = GetPassWord(strPassWord);

            strDbOption = myIniFile.ReadValue("DBOPTION", "Option", "");

            pAES = null;
            myIniFile = null;
        }


        /// <summary>
        /// 컴퓨터 정보를 변수에 할당한다
        /// </summary>
        private void SetComputerInfo()
        {
            string host = Dns.GetHostName();
            IPHostEntry ip = Dns.GetHostEntry(host);
            for (int i = 0; i < ip.AddressList.Length; i++)
            {
                if (ip.AddressList[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    CompIp = ip.AddressList[i].ToString();
                }
            }
            CompName = SystemInformation.ComputerName;
            MacAddr = NetworkInterface.GetAllNetworkInterfaces()[0].GetPhysicalAddress().ToString();
        }

        /// <summary>
        /// 초기값 세팅
        /// </summary>
        /// <param name="Con"></param>
        private void InitClass(OracleConnection Con)
        {
            //DbOption = "SVR";
            //SetComputerInfo();
            //setCurrentDateTime(Con);
        }

        #region //Db Connect
        /// <summary>
        /// DB 함수 입니다.
        /// </summary>
        public bool DBConnect_Cloud()
        {
            //GetDbInfo();
            //Enter your ADB's user id, password, and net service name
            string conString = "User Id=ADMIN;Password=QjelQjel!@12;Data Source=kosha_high;Connection Timeout=30;";

            //Enter directory where you unzipped your cloud credentials
            if (OracleConfiguration.TnsAdmin == null)
            {
                OracleConfiguration.TnsAdmin = @"C:\Oracle11g\product\11.2.0\client_1\network\admin";
                OracleConfiguration.WalletLocation = OracleConfiguration.TnsAdmin;
            }

            try
            {
                Con = new OracleConnection(conString);
                Con.Open();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(new Form() { TopMost = true }, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// DB 함수 입니다.
        /// </summary>
        public bool DBConnect()
        {
            GetDbInfo();

            string strConnectString =
                    "User Id =" + strUser +
                    ";Password = " + strPassWord +
                    ";Data Source =" + strSource +
                    ";Pooling = false";
            try
            {
                Con = new OracleConnection(strConnectString);
                Con.Open();

                //InitClass(Con);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(new Form() { TopMost = true }, ex.Message);
                return false;
            }
        }

        public bool DBConnectEx()
        {
            GetDbInfoEx();

            string strConnectString =
                    "User Id =" + strUser +
                    ";Password = " + strPassWord +
                    ";Data Source =" + strSource +
                    ";Pooling = false";
            try
            {
                Con = new OracleConnection(strConnectString);
                Con.Open();

                //InitClass(Con);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(new Form() { TopMost = true }, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// DB Connect 함수 입니다.(기본)
        /// </summary>
        /// <param name="pstrDbIp"></param>
        /// <param name="pstrDbPort"></param>
        /// <param name="pstrSource"></param>
        /// <param name="pstrUser"></param>
        /// <param name="pstrPassWord"></param>
        public bool DBConnect(string pstrDbIp, string pstrDbPort, string pstrSource, string pstrUser, string pstrPassWord)
        {
            GetDbInfo();

            strDbIp = pstrDbIp;
            strDbPort = pstrDbPort;
            strSource = pstrSource;
            strUser = pstrUser;

            string strConnectString =
                    "User Id =" + strUser +
                    ";Password = " + strPassWord +
                    ";Data Source =" + strSource +
                    ";Pooling = false";


            try
            {
                Con = new OracleConnection(strConnectString);
                Con.Open();

                //InitClass(Con);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(new Form() { TopMost = true }, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pstrDbIp"></param>
        /// <param name="pstrDbPort"></param>
        /// <param name="pstrSource"></param>
        /// <param name="pstrUser"></param>
        /// <param name="pstrPassWord"></param>
        /// <returns></returns>
        public bool DBConnectEx(string pstrDbIp, string pstrDbPort, string pstrSource, string pstrUser, string pstrPassWord)
        {

            strDbIp = pstrDbIp;
            strDbPort = pstrDbPort;
            strSource = pstrSource;
            strUser = pstrUser;
            strPassWord = pstrPassWord;

            string strConnectString =
                    "User Id =" + strUser +
                    ";Password = " + strPassWord +
                    ";Data Source =" + strSource +
                    ";Pooling = false";


            try
            {
                Con = new OracleConnection(strConnectString);
                Con.Open();

                //InitClass(Con);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(new Form() { TopMost = true }, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// DB 연결해제 함수 입니다.
        /// </summary>
        /// <param name="dc1">DBConnection을 Close, Dispose, Nothing 시킵니다.</param>
        public void DisDBConnect()
        {
            try
            {
                Con.Close();
                Con.Dispose();
                OracleConnection.ClearPool(Con);
                Con = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(new Form() { TopMost = true }, ex.Message);
            }
        }

        /// <summary>
        /// Transaction 객체 생성
        /// </summary>
        /// <returns></returns>
        public bool setBeginTran()
        {
            try
            {
                Trs = Con.BeginTransaction(IsolationLevel.ReadCommitted);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(new Form() { TopMost = true }, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Transaction Commit
        /// </summary>
        /// <param name="Trs">Transaction 객체</param>
        public void setCommitTran()
        {
            try
            {
                Trs.Commit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(new Form() { TopMost = true }, ex.Message);
            }
            finally
            {
                Trs = null;
            }
        }

        /// <summary>
        /// Transaction을 취소한다
        /// </summary>
        /// <param name="Trs">Transaction 객체</param>
        public void setRollbackTran()
        {
            try
            {
                Trs.Rollback();
            }
            catch (Exception ex)
            {
                MessageBox.Show(new Form() { TopMost = true }, ex.Message);
            }
            finally
            {
                Trs = null;
            }
        }

        //private void setCurrentDateTime(OracleConnection Con, OracleTransaction Trs = null)
        //{
        //    string SQL = "";
        //    OracleDataAdapter Adapter = default(OracleDataAdapter);
        //    DataSet ds = new DataSet();

        //    SQL = "";
        //    SQL = SQL + " SELECT TO_CHAR(SYSDATE,'YYYYMMDD') AS CURRENTDATE,      \r\n";
        //    SQL = SQL + "        TO_CHAR(SYSDATE,'HH24MISS') AS CURRENTTIME,      \r\n";
        //    SQL = SQL + "        SYSDATE                                          \r\n";
        //    SQL = SQL + "   FROM DUAL                                             \r\n";

        //    try
        //    {
        //        Adapter = new OracleDataAdapter(SQL, Con);
        //        if (Trs != null)
        //        {
        //            Adapter.SelectCommand.Transaction = Trs;
        //        }
        //        Adapter.SelectCommand.InitialLONGFetchSize = -1;

        //        Adapter.Fill(ds);
        //        Adapter.Dispose();
        //        Adapter = null;

        //        SysDate = ds.Tables[0].Rows[0]["CURRENTDATE"].ToString().Trim();
        //        SysTime = ds.Tables[0].Rows[0]["CURRENTTIME"].ToString().Trim();

        //        ds.Dispose();
        //        ds = null;

        //        return;
        //    }
        //    catch (OracleException sqlExc)
        //    {
        //        MessageBox.Show(new Form() { TopMost = true }, sqlExc.Message);
        //        return;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(new Form() { TopMost = true }, ex.Message);
        //        return ;
        //    }
        //    finally
        //    {
        //        if ((Adapter != null))
        //        {
        //            Adapter.Dispose();
        //            Adapter = null;
        //        }
        //        if ((ds != null))
        //        {
        //            ds.Dispose();
        //            ds = null;
        //        }
        //    }
        //}


        #endregion //Db Connect

        //#region //Sql Log
        ///// <summary>
        ///// SQL Log를 저장을 한다
        ///// </summary>
        ///// <param name="strQuery">Query문</param>
        //public void SaveSqlLog(string strQuery, OracleConnection Con = null, OracleTransaction Trs = null)
        //{
        //    PsmhFunc Func = new PsmhFunc();

        //    OracleCommand Cmd = null;
        //    OracleDataAdapter Adapter = new OracleDataAdapter();
        //    DataSet ds = null;
        //    string rtnVal = "";
        //    string SQL = "";
        //    string DB_PMPA = "ADMIN.";

        //    int RowAffected = 0;

        //    strQuery = strQuery.Replace("'", "`");
        //    strQuery = strQuery.Replace("\r", " ");

        //    string strSQL = "";
        //    string strSQL1 = "";
        //    string strSQL2 = "";
        //    string strSQL3 = "";

        //    int intLenTot = (int)Func.GetWordByByte(strQuery);
        //    if (DbOption == "LOCAL")
        //    {
        //        DB_PMPA = "";
        //    }

        //    if (intLenTot > 16000) return;

        //    Trs = null; //

        //    string strTable = "";
        //    if (SysDate != "")
        //    {
        //        strTable = SysDate.Replace("-", "");
        //    }
        //    else
        //    {
        //        //서버시간을 세팅을 한다 
        //        setCurrentDateTime(Con);
        //        strTable = SysDate; //무한루프가 돈다
        //    }

        //    try
        //    {
        //        ds = new DataSet();

        //        SQL = "";
        //        SQL = SQL + "\r\n" + "SELECT * FROM USER_TABLES          ";
        //        SQL = SQL + "\r\n" + "WHERE TABLE_NAME = 'ETC_SQLLOG_" + strTable + "' ";

        //        Adapter = new OracleDataAdapter(SQL, Con);

        //        if (Trs != null)
        //        {
        //            Adapter.SelectCommand.Transaction = Trs;
        //        }
        //        Adapter.SelectCommand.InitialLONGFetchSize = -1;
        //        Adapter.Fill(ds);

        //        if (ds.Tables[0].Rows.Count == 0)
        //        {
        //            SQL = "";
        //            SQL = SQL + "\r\n" + "CREATE TABLE " + DB_PMPA + "ETC_SQLLOG_" + strTable;
        //            SQL = SQL + "\r\n" + "(                                                     ";
        //            SQL = SQL + "\r\n" + "  ACTDATE   DATE,                                     ";
        //            SQL = SQL + "\r\n" + "  EXECODE   CHAR(20 BYTE),                            ";
        //            SQL = SQL + "\r\n" + "  JOBSABUN  CHAR(6 BYTE),                             ";
        //            SQL = SQL + "\r\n" + "  IP        VARCHAR2(15 BYTE),                        ";
        //            SQL = SQL + "\r\n" + "  GUBUN     VARCHAR2(2000 BYTE),                      ";
        //            SQL = SQL + "\r\n" + "  SQL       VARCHAR2(4000 BYTE),                      ";
        //            SQL = SQL + "\r\n" + "  SQL1       VARCHAR2(4000 BYTE),                      ";
        //            SQL = SQL + "\r\n" + "  SQL2       VARCHAR2(4000 BYTE),                      ";
        //            SQL = SQL + "\r\n" + "  SQL3       VARCHAR2(4000 BYTE),                      ";
        //            SQL = SQL + "\r\n" + "  ENTDATE   DATE,                                     ";
        //            SQL = SQL + "\r\n" + "  SENDLOG   CHAR(1 BYTE)                              ";
        //            SQL = SQL + "\r\n" + ")                                                     ";
        //            SQL = SQL + "\r\n" + "TABLESPACE SQLLOG2                                    ";
        //            SQL = SQL + "\r\n" + "PCTUSED    40                                         ";
        //            SQL = SQL + "\r\n" + "PCTFREE    10                                         ";
        //            SQL = SQL + "\r\n" + "INITRANS   1                                          ";
        //            SQL = SQL + "\r\n" + "MAXTRANS   255                                        ";
        //            SQL = SQL + "\r\n" + "STORAGE    (                                          ";
        //            SQL = SQL + "\r\n" + "            INITIAL          64K                    ";
        //            SQL = SQL + "\r\n" + "            MINEXTENTS       1                        ";
        //            SQL = SQL + "\r\n" + "            MAXEXTENTS       UNLIMITED                ";
        //            SQL = SQL + "\r\n" + "            PCTINCREASE      0                        ";
        //            SQL = SQL + "\r\n" + "            FREELISTS        10                       ";
        //            SQL = SQL + "\r\n" + "            FREELIST GROUPS  4                        ";
        //            SQL = SQL + "\r\n" + "            BUFFER_POOL      DEFAULT                  ";
        //            SQL = SQL + "\r\n" + "           )                                          ";
        //            SQL = SQL + "\r\n" + "LOGGING                                               ";
        //            SQL = SQL + "\r\n" + "NOCOMPRESS                                            ";
        //            SQL = SQL + "\r\n" + "NOCACHE                                               ";
        //            SQL = SQL + "\r\n" + "NOPARALLEL                                            ";
        //            SQL = SQL + "\r\n" + "NOMONITORING                                          ";
        //            Cmd = Con.CreateCommand();
        //            Cmd.CommandText = SQL;
        //            Cmd.CommandTimeout = 60;

        //            if (Trs != null)
        //            {
        //                Cmd.Transaction = Trs;
        //            }

        //            RowAffected = Cmd.ExecuteNonQuery();
        //        }
        //        Adapter.Dispose();
        //        Adapter = null;
        //        ds.Dispose();
        //        ds = null;

        //        Cmd = null;
        //        RowAffected = 0;

        //        if (intLenTot <= 4000)
        //        {
        //            strSQL = strQuery;
        //        }
        //        else if (intLenTot > 4000 && intLenTot <= 8000)
        //        {
        //            strSQL = Func.GetMidStr(strQuery, 0, 4000);
        //            strSQL1 = Func.GetMidStr(strQuery, 4000, intLenTot - 4000);
        //        }
        //        else if (intLenTot > 8000 && intLenTot <= 12000)
        //        {
        //            strSQL = Func.GetMidStr(strQuery, 0, 4000);
        //            strSQL1 = Func.GetMidStr(strQuery, 4000, 4000);
        //            strSQL2 = Func.GetMidStr(strQuery, 8000, intLenTot - 8000);
        //        }
        //        else if (intLenTot > 12000 && intLenTot <= 16000)
        //        {
        //            strSQL = Func.GetMidStr(strQuery, 0, 4000);
        //            strSQL1 = Func.GetMidStr(strQuery, 4000, 4000);
        //            strSQL2 = Func.GetMidStr(strQuery, 8000, 4000);
        //            strSQL3 = Func.GetMidStr(strQuery, 12000, intLenTot - 12000);
        //        }

        //        SQL = "";
        //        SQL = SQL + "\r\n" + " INSERT INTO " + DB_PMPA + "ETC_SQLLOG_" + strTable;
        //        SQL = SQL + "\r\n" + " ( ACTDATE, EXECODE,  JOBSABUN, IP, GUBUN, SQL, SQL1, SQL2, SQL3, ENTDATE) VALUES ( ";
        //        SQL = SQL + "\r\n" + " TRUNC(SYSDATE), 'PSMH' , '" + IdNumber + "',  '" + CompIp + "', ";
        //        SQL = SQL + "\r\n" + " 'PSMH', '" + strSQL + "',  '" + strSQL1 + "',  '" + strSQL2 + "',  '" + strSQL3 + "' , SYSDATE) ";

        //        Cmd = Con.CreateCommand();
        //        Cmd.CommandText = SQL;
        //        Cmd.CommandTimeout = 60;

        //        if (Trs != null)
        //        {
        //            Cmd.Transaction = Trs;
        //        }

        //        RowAffected = Cmd.ExecuteNonQuery();
        //        Func = null;
        //        return;
        //    }
        //    catch (OracleException sqlExc)
        //    {
        //        MessageBox.Show(new Form() { TopMost = true }, sqlExc.Message);
        //        rtnVal = sqlExc.Message;
        //        return;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(new Form() { TopMost = true }, ex.Message);
        //        rtnVal = ex.Message;
        //        return;
        //    }
        //    finally
        //    {
        //        if (Cmd != null)
        //        {
        //            Cmd.Dispose();
        //            Cmd = null;
        //        }
        //        if (Func != null)
        //        {
        //            Func = null;
        //        }
        //    }
        //}

        ///// <summary>
        ///// 에러 로그를 저장을 한다.
        ///// </summary>
        ///// <param name="strError">에러</param>
        ///// <param name="strQuery">Query 문</param>
        ///// <param name="Trs"></param>
        ///// <remarks>에러 로그는 트랙젝션 처리 하지 않는다.</remarks>
        //public void SaveSqlErrLog(string strError, string strQuery, OracleConnection Con = null, OracleTransaction Trs = null)
        //{
        //    PsmhFunc Func = new PsmhFunc();

        //    OracleCommand Cmd = null;
        //    string rtnVal = "";
        //    string SQL = "";
        //    string DB_PMPA = "ADMIN.";

        //    int RowAffected = 0;

        //    strQuery = strQuery.Replace("'", "`");
        //    strQuery = strQuery.Replace("\r", " ");

        //    string strSQL = "";
        //    string strSQL1 = "";
        //    string strSQL2 = "";
        //    string strSQL3 = "";
        //    int intLenTot = (int)Func.GetWordByByte(strQuery);
        //    if (DbOption == "LOCAL")
        //    {
        //        DB_PMPA = "";
        //    }

        //    if (intLenTot > 16000) return;

        //    try
        //    {
        //        if (intLenTot <= 4000)
        //        {
        //            strSQL = strQuery;
        //        }
        //        else if (intLenTot > 4000 && intLenTot <= 8000)
        //        {
        //            strSQL = Func.GetMidStr(strQuery, 0, 4000);
        //            strSQL1 = Func.GetMidStr(strQuery, 4000, intLenTot - 4000);
        //        }
        //        else if (intLenTot > 8000 && intLenTot <= 12000)
        //        {
        //            strSQL = Func.GetMidStr(strQuery, 0, 4000);
        //            strSQL1 = Func.GetMidStr(strQuery, 4000, 4000);
        //            strSQL2 = Func.GetMidStr(strQuery, 8000, intLenTot - 8000);
        //        }
        //        else if (intLenTot > 12000 && intLenTot <= 16000)
        //        {
        //            strSQL = Func.GetMidStr(strQuery, 0, 4000);
        //            strSQL1 = Func.GetMidStr(strQuery, 4000, 4000);
        //            strSQL2 = Func.GetMidStr(strQuery, 8000, 4000);
        //            strSQL3 = Func.GetMidStr(strQuery, 12000, intLenTot - 12000);
        //        }

        //        strError = strError.Replace("'", "`");

        //        SQL = "";
        //        SQL = SQL + "\r\n" + " INSERT INTO " + DB_PMPA + "ETC_SQLERROR ";
        //        SQL = SQL + "\r\n" + " ( ACTDATE, EXECODE,  JOBSABUN, ERR_NUMBER, ERR_SOURCE, ERR_DESCRIPTION, SQL, SQL1, SQL2, SQL3, ENTDATE) VALUES ( ";
        //        SQL = SQL + "\r\n" + " TRUNC(SYSDATE), 'PSMH' , '" + IdNumber + "',  '0', ";
        //        SQL = SQL + "\r\n" + "  '', '" + strError + "',  '" + strSQL + "',  '" + strSQL1 + "',  '" + strSQL2 + "',  '" + strSQL3 + "', SYSDATE) ";

        //        Cmd = Con.CreateCommand();
        //        Cmd.CommandText = SQL;
        //        Cmd.CommandTimeout = 60;

        //        if (Trs != null)
        //        {
        //            Cmd.Transaction = Trs;
        //        }

        //        RowAffected = Cmd.ExecuteNonQuery();
        //        Func = null;
        //        return;
        //    }
        //    catch (OracleException sqlExc)
        //    {
        //        MessageBox.Show(new Form() { TopMost = true }, sqlExc.Message);
        //        rtnVal = sqlExc.Message;
        //        return;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(new Form() { TopMost = true }, ex.Message);
        //        rtnVal = ex.Message;
        //        return;
        //    }
        //    finally
        //    {
        //        if (Cmd != null)
        //        {
        //            Cmd.Dispose();
        //            Cmd = null;
        //        }
        //        if (Func != null)
        //        {
        //            Func = null;
        //        }
        //    }
        //}
        //#endregion //

        ///// <summary>
        ///// DB 단순 쿼리에 결과 값을 DataSet 값으로 리턴 시켜줍니다. 
        ///// </summary>
        ///// <param name="ds">DataSet</param>
        ///// <param name="SQL">Query 문</param>
        ///// <param name="Trs">Transaction</param>
        ///// <returns>에러로그 + DataSet</returns>
        //public string GetDataSet(ref DataSet ds, string SQL, OracleConnection Con, OracleTransaction Trs = null)
        //{
        //    OracleDataAdapter Adapter = default(OracleDataAdapter);
        //    DataSet ds1 = new DataSet();
        //    string rtnVal = "";

        //    try
        //    {
        //        Adapter = new OracleDataAdapter(SQL, Con);
        //        if (Trs != null)
        //        {
        //            Adapter.SelectCommand.Transaction = Trs;
        //        }
        //        Adapter.SelectCommand.InitialLONGFetchSize = -1;

        //        Adapter.Fill(ds1);
        //        ds = ds1;
        //        ds1.Dispose();
        //        ds1 = null;

        //        //TODO : 2017.08.17.김홍록:시연을 위해서 막음
        //        SaveSqlLog(SQL, Con, Trs); //Query Log 저장

        //        return rtnVal;
        //    }
        //    catch (OracleException sqlExc)
        //    {
        //        MessageBox.Show(new Form() { TopMost = true }, sqlExc.Message);
        //        rtnVal = sqlExc.Message;
        //        return rtnVal;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(new Form() { TopMost = true }, ex.Message);
        //        rtnVal = ex.Message;
        //        return rtnVal;
        //    }
        //    finally
        //    {
        //        if ((Adapter != null))
        //        {
        //            Adapter.Dispose();
        //            Adapter = null;
        //        }
        //        if ((ds1 != null))
        //        {
        //            ds1.Dispose();
        //            ds1 = null;
        //        }
        //    }
        //}



    }
}
