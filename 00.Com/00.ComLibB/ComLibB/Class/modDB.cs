using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
//using System.Data.OleDb;
using System.Windows.Forms;
using System.Text;

namespace ComLibB
{
    public class modDB
    {
        private static string gDataBase = null;
        private static OracleConnection dc = null;
        public static OracleTransaction trs = null;

        public static OracleConnection gDc = null;
        public static OracleTransaction gTrs = null;

        private static OracleCommand cmd = null;

        //[STAThread]
        //static void Main()
        //{
        //    Environment.SetEnvironmentVariable("PATH", Environment.CurrentDirectory, EnvironmentVariableTarget.Process);
        //    Environment.SetEnvironmentVariable("PATH", @"C:\mentorsoft\exenet\dll\", EnvironmentVariableTarget.Process);
        //}
        /// <summary>
        /// DB Connect 함수 입니다.(기본)
        /// </summary>
        /// <returns></returns>
        public static bool DBConnect()
        {
            //string strConnectString = null;

            //string strDbIp = "192.168.104.200";
            //string strDbPort = "1521";
            string strSource = "PSMH";
            string strUser = "KOSMOS_PMPA";
            string strPassWord = "hospital";

            //strConnectString = "Data Source=(DESCRIPTION=(ADDRESS_LIST=";
            //strConnectString = strConnectString + "(ADDRESS=(PROTOCOL=TCP)(HOST=" + strDbIp + ")(PORT=" + strDbPort + ")))";
            //strConnectString = strConnectString + "(CONNECT_DATA=";
            //strConnectString = strConnectString + "(SERVER=DEDICATED)";     //이부분이 없다.
            //strConnectString = strConnectString + "(SERVICE_NAME=" + strSource + ")));";
            //strConnectString = strConnectString + "User Id=" + strUser + ";Password=" + strPassWord + ";";

            string strConnectString =
                    "User Id =" + strUser +
                    ";Password = " + strPassWord +
                    ";Data Source =" + strSource;

            try
            {
                gDc = new OracleConnection(strConnectString);

                gDc.Open();
                return true;
            }
            catch (Exception exc)
            {
                ComFunc.MsgBox(exc.Message);
                gDc = null;
                return false;
            }
        }

        /// <summary>
        /// DB Connect 함수 입니다.
        /// </summary>
        /// <param name="strDbIp">서버IP(이름)</param>
        /// <param name="strDbPort">포터</param>
        /// <param name="strSource">Data소스</param>
        /// <param name="strUser">사용자</param>
        /// <param name="strPassWord">비밀번호</param>
        /// <returns></returns>
        public static bool DBConnect(string strDbIp, string strDbPort, string strSource, string strUser, string strPassWord)
        {
            string strConnectString =
                    "User Id =" + strUser +
                    ";Password = " + strPassWord +
                    ";Data Source =" + strSource;

            //strConnectString = "Data Source=(DESCRIPTION=(ADDRESS_LIST=";
            //strConnectString = strConnectString + "(ADDRESS=(PROTOCOL=TCP)(HOST=" + strDbIp + ")(PORT=" + strDbPort + ")))";
            //strConnectString = strConnectString + "(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=" + strSource + ")));";
            //strConnectString = strConnectString + "User Id=" + strUser + ";Password=" + strPassWord + ";";

            try
            {
                gDc = new OracleConnection(strConnectString);

                gDc.Open();
                return true;
            }
            catch (Exception exc)
            {
                ComFunc.MsgBox(exc.Message);
                gDc = null;
                return false;
            }
        }

        /// <summary>
        /// DB 연결해제 함수 입니다.
        /// </summary>
        public static void DisDBConnect()
        {
            try
            {
                gDc.Close();
                gDc.Dispose();
                gDc = null;
            }
            catch (Exception exc)
            {
                ComFunc.MsgBox(exc.Message);
            }
        }

        /// <summary>
        /// DB 연결해제 함수 입니다.
        /// </summary>
        /// <param name="dc1">DBConnection을 Close, Dispose, Nothing 시킵니다.</param>
        public static void DisDBConnect(OracleConnection dc1)
        {
            try
            {
                dc1.Close();
                dc1.Dispose();
                dc1 = null;
            }
            catch (Exception exc)
            {
                ComFunc.MsgBox(exc.Message);
            }
        }

        /// <summary>
        /// DB 단순 쿼리에 결과 값을 DataSet 값으로 리턴 시켜줍니다. 
        /// </summary>
        /// <param name="SQL"></param>
        /// <returns></returns>
        public static DataSet GetDataSet(string SQL)
        {
            OracleDataAdapter Adapter = new OracleDataAdapter();
            DataSet ds = null;

            try
            {
                if (cmd == null)
                {
                    cmd = gDc.CreateCommand();
                }
                //Orcle gDc가 트랜젹션을 가지고 있음.
                if (gTrs != null)
                {
                    cmd.Transaction = gTrs;
                }
                ds = new DataSet();

                Adapter.SelectCommand = new OracleCommand(SQL, gDc);
                Adapter.Fill(ds);

                return ds;
            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                return null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return null;
            }
            finally
            {
                if ((ds != null))
                    ds.Dispose();
                ds = null;
            }
        }

        /// <summary>
        /// DB 단순 쿼리에 결과 값을 DataTable 값으로 리턴 시켜줍니다. 
        /// </summary>
        /// <param name="SQL">쿼리 내용입니다.</param>
        /// <returns>DataTable</returns>
        /// <remarks>항상 쿼리를 보낸 후 값이 Nothing가 아니면 Dispose 시켜줍니다.</remarks>
        public static DataTable GetDataTable(string SQL)
        {

            OracleDataAdapter da = default(OracleDataAdapter);
            DataSet ds = null;
            try
            {
                da = new OracleDataAdapter(SQL, gDc);

                if (gTrs == null)
                {
                    ds = new DataSet();
                }
                else
                {
                    da.SelectCommand.Transaction = gTrs;
                    ds = new DataSet();
                }

                da.Fill(ds);
                da.Dispose();

                return ds.Tables[0];

            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                return null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return null;
            }
            finally
            {
                if ((ds != null))
                    ds.Dispose();
            }
        }

        /// <summary>
        /// DB 단순 쿼리에 결과 값을 DataTable 값으로 리턴 시켜줍니다. 
        /// Long Data Type의 경우 사용 바랍니다.
        /// </summary>
        /// <param name="SQL">쿼리 내용입니다.</param>
        /// <returns>DataTable</returns>
        /// <remarks>항상 쿼리를 보낸 후 값이 Nothing가 아니면 Dispose 시켜줍니다.</remarks>
        public static DataTable GetDataTableEx(string SQL)
        {
            OracleCommand Cmd = null;
            OracleDataReader reader = null;
            DataTable dtx = null;
            string rtnVal = "";
            try
            {
                if (Cmd == null)
                {
                    Cmd = gDc.CreateCommand();
                }

                Cmd.CommandText = SQL;
                Cmd.CommandTimeout = 60;

                if (gTrs != null)
                {
                    Cmd.Transaction = gTrs;
                }

                Cmd.InitialLONGFetchSize = -1;
                reader = Cmd.ExecuteReader();
                dtx = new DataTable();
                dtx.Load(reader);
                return dtx;
            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return null;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Dispose();
                    reader = null;
                }
            }
        }

        /// <summary>
        /// DB 단순 쿼리에 결과 값을 DataTable 값으로 리턴 시켜줍니다. 
        /// Long Data Type의 경우 사용 바랍니다.
        /// </summary>
        /// <param name="SQL">쿼리 내용입니다.</param>
        /// <returns>DataTable</returns>
        /// <remarks>항상 쿼리를 보낸 후 값이 Nothing가 아니면 Dispose 시켜줍니다.</remarks>
        public static OracleDataReader GetDataReder(string SQL)
        {
            OracleCommand Cmd = null;
            OracleDataReader reader = null;
            string rtnVal = "";
            try
            {
                if (Cmd == null)
                {
                    Cmd = gDc.CreateCommand();
                }

                Cmd.CommandText = SQL;
                Cmd.CommandTimeout = 60;

                if (gTrs != null)
                {
                    Cmd.Transaction = gTrs;
                }

                Cmd.InitialLONGFetchSize = -1;
                reader = Cmd.ExecuteReader();

                cmd.Dispose();
                cmd = null;
                return reader;
            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return null;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                    cmd = null;
                }
            }
        }

        /// <summary>
        /// Transaction을 시작한다
        /// </summary>
        public static void setBeginTran()
        {
            try
            {
                if (gTrs == null)
                {
                    gTrs = gDc.BeginTransaction(IsolationLevel.ReadCommitted);
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }

        /// <summary>
        /// Transaction을 완료한다
        /// </summary>
        public static void setCommitTran()
        {
            try
            {
                gTrs.Commit();
                gTrs = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }

        /// <summary>
        /// Transaction을 취소한다
        /// </summary>
        public static void setRollbackTran()
        {
            try
            {
                gTrs.Rollback();
                gTrs = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }

        /// <summary>
        ///  DB 트랜잭션 쿼리(Insert, Delete, Update)에 Boolean 값으로 리턴 시켜줍니다.
        /// </summary>
        /// <param name="SQL">쿼리 내용 입니다.</param>
        /// <returns>True/False</returns>
        /// <remarks>setBeginTx, setCommitTx, setRollbackTx 을 사용해서 구성한다.</remarks>
        public static bool ExecuteNonQuery(string SQL)
        {
            try
            {
                if (cmd == null)
                {
                    cmd = gDc.CreateCommand();
                }

                cmd.CommandText = SQL;
                cmd.CommandTimeout = 60;

                if (gTrs != null)
                {
                    cmd.Transaction = gTrs;
                }

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                cmd = null;
                return true;
            }
            catch (OracleException sqlExc)
            {
                if ((gTrs != null))
                {
                    throw new Exception(sqlExc.Message);
                }

                ComFunc.MsgBox(sqlExc.Message);
                return false;
            }
            catch (Exception exc)
            {
                if ((gTrs != null))
                {
                    throw new Exception(exc.Message);
                }

                ComFunc.MsgBox(exc.Message);
                return false;
            }
        }

        /// <summary>
        /// XML 데이타를 저장합니다. = 필드 하나만
        /// </summary>
        /// <param name="SQL">쿼리 내용 입니다.</param>
        /// <param name="strXml">XML 필드 데이타입니다</param>
        /// <returns>True/False</returns>
        public static bool ExecuteXmlQuery(string SQL, string strXml)
        {
            try
            {
                if (cmd == null)
                {
                    cmd = gDc.CreateCommand();
                }

                cmd.CommandText = SQL;
                cmd.CommandTimeout = 60;
                cmd.Parameters.Add("@emrxml", OracleDbType.XmlType).Value = strXml;

                if (gTrs != null)
                {
                    cmd.Transaction = gTrs;
                }

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                cmd = null;

                return true;
            }
            catch (OracleException sqlExc)
            {
                if ((gTrs != null))
                {
                    throw new Exception(sqlExc.Message);
                }

                ComFunc.MsgBox(sqlExc.Message);
                return false;
            }
            catch (Exception exc)
            {
                if ((gTrs != null))
                {
                    throw new Exception(exc.Message);
                }

                ComFunc.MsgBox(exc.Message);
                return false;
            }
        }

        /// <summary>
        /// XML 데이타를 저장합니다. = 여러개의 필드
        /// </summary>
        /// <param name="SQL">쿼리 내용 입니다.</param>
        /// <param name="strXml">XML 필드 데이타입니다</param>
        /// <returns>True/False</returns>
        public static bool ExecuteXmlQuery(string SQL, string[] strXml)
        {
            int i = -1;

            try
            {
                if (cmd == null)
                {
                    cmd = gDc.CreateCommand();
                }

                cmd.CommandText = SQL;
                cmd.CommandTimeout = 60;
                for (i = 0; i <= strXml.Length - 1; i++)
                {
                    cmd.Parameters.Add("@emrxml", OracleDbType.XmlType).Value = strXml[i];
                }

                if (gTrs != null)
                {
                    cmd.Transaction = gTrs;
                }

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                cmd = null;

                return true;
            }
            catch (OracleException sqlExc)
            {
                if ((gTrs != null))
                {
                    throw new Exception(sqlExc.Message);
                }

                ComFunc.MsgBox(sqlExc.Message);
                return false;
            }
            catch (Exception exc)
            {
                if ((gTrs != null))
                {
                    throw new Exception(exc.Message);
                }

                ComFunc.MsgBox(exc.Message);
                return false;
            }
        }

        /// <summary>
        /// CLOB 데이타를 저장합니다. = 필드 하나만
        /// </summary>
        /// <param name="SQL">쿼리 내용 입니다.</param>
        /// <param name="strClob">CLOB 필드 데이타입니다</param>
        /// <returns>True/False</returns>
        public static bool ExecuteClobQuery(string SQL, string strClob)
        {
            try
            {
                if (cmd == null)
                {
                    cmd = gDc.CreateCommand();
                }

                cmd.CommandText = SQL;
                cmd.CommandTimeout = 60;
                cmd.Parameters.Add("@clobdata", OracleDbType.Clob).Value = strClob;

                if (gTrs != null)
                {
                    cmd.Transaction = gTrs;
                }

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                cmd = null;

                return true;
            }
            catch (OracleException sqlExc)
            {
                if ((gTrs != null))
                {
                    throw new Exception(sqlExc.Message);
                }

                ComFunc.MsgBox(sqlExc.Message);
                return false;
            }
            catch (Exception exc)
            {
                if ((gTrs != null))
                {
                    throw new Exception(exc.Message);
                }

                ComFunc.MsgBox(exc.Message);
                return false;
            }
        }

        /// <summary>
        /// CLOB 데이타를 저장합니다. = 여러개의 필드
        /// </summary>
        /// <param name="SQL">쿼리 내용 입니다.</param>
        /// <param name="strClob">CLOB 필드 데이타입니다</param>
        /// <returns>True/False</returns>
        public static bool ExecuteClobQuery(string SQL, string[] strClob)
        {
            int i = -1;

            try
            {
                if (cmd == null)
                {
                    cmd = gDc.CreateCommand();
                }

                cmd.CommandText = SQL;
                cmd.CommandTimeout = 60;
                for (i = 0; i <= strClob.Length - 1; i++)
                {
                    cmd.Parameters.Add("@clobdata", OracleDbType.XmlType).Value = strClob[i];
                }

                if (gTrs != null)
                {
                    cmd.Transaction = gTrs;
                }

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                cmd = null;

                return true;
            }
            catch (OracleException sqlExc)
            {
                if ((gTrs != null))
                {
                    throw new Exception(sqlExc.Message);
                }

                ComFunc.MsgBox(sqlExc.Message);
                return false;
            }
            catch (Exception exc)
            {
                if ((gTrs != null))
                {
                    throw new Exception(exc.Message);
                }

                ComFunc.MsgBox(exc.Message);
                return false;
            }
        }

        /// <summary>
        /// Long 형식을 저장,수정 한다
        /// </summary>
        /// <param name="SQL">SQL문</param>
        /// <param name="strLong">값</param>
        /// <returns></returns>
        public static bool ExecuteLongQuery(string SQL, string strLong)
        {
            //SQL = SQL + ComNum.VBLF + " MEMO = :1";
            OracleCommand Cmd = null;
            bool rtnVal = false;
            try
            {
                if (Cmd == null)
                {
                    Cmd = gDc.CreateCommand();
                }

                Cmd.CommandText = SQL;
                Cmd.CommandTimeout = 60;
                Cmd.Parameters.Add("@Long", OracleDbType.Long).Value = strLong;

                if (gTrs != null)
                {
                    Cmd.Transaction = gTrs;
                }

                Cmd.ExecuteNonQuery();
                Cmd.Dispose();
                Cmd = null;

                rtnVal = true;
                return rtnVal;
            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        public static bool ExecuteChartRow(string SQL, double dblEmrNoNew, string[] strITEMCD, string[] strITEMNO, string[] strITEMINDEX, string[] strITEMTYPE, string[] strITEMVALUE, string[] strDSPSEQ, string[] strITEMVALUE1)
        {
            try
            {
                if (cmd == null)
                {
                    cmd = gDc.CreateCommand();
                }

                cmd.CommandText = SQL;
                cmd.CommandTimeout = 60;
                if (gTrs != null)
                {
                    cmd.Transaction = gTrs;
                }

                SQL = "";
                SQL = SQL + "\r\n" + "INSERT INTO MHEMR.AEMRCHARTROW ";
                SQL = SQL + "\r\n" + "    (EMRNO       ,ITEMCD       ,ITEMNO      ,ITEMINDEX    ,ITEMTYPE   , ITEMVALUE, DSPSEQ, ITEMVALUE1 )";
                SQL = SQL + "\r\n" + "VALUES (";
                SQL = SQL + "\r\n" + dblEmrNoNew.ToString() + ",";    //EMRNO
                SQL = SQL + "\r\n" + " ?,";   //ITEMCD
                SQL = SQL + "\r\n" + " ?,"; //ITEMNO
                SQL = SQL + "\r\n" + " ?,"; //ITEMINDEX
                SQL = SQL + "\r\n" + " ?,";   //ITEMTYPE
                SQL = SQL + "\r\n" + " ?,";   //ITEMVALUE
                SQL = SQL + "\r\n" + " ?,";   //DSPSEQ
                SQL = SQL + "\r\n" + " ?";   //ITEMVALUE1
                SQL = SQL + "\r\n" + ")";

                SQL = "";
                SQL = SQL + "\r\n" + "INSERT INTO MHEMR.AEMRCHARTROW ";
                SQL = SQL + "\r\n" + "    (EMRNO       ,ITEMCD       ,ITEMNO      ,ITEMINDEX    ,ITEMTYPE   , ITEMVALUE, DSPSEQ, ITEMVALUE1 )";
                SQL = SQL + "\r\n" + "VALUES (";
                SQL = SQL + "\r\n" + dblEmrNoNew.ToString() + ",";    //EMRNO
                SQL = SQL + "\r\n" + " @ITEMCD,";   //ITEMCD
                SQL = SQL + "\r\n" + " @ITEMNO,"; //ITEMNO
                SQL = SQL + "\r\n" + " @ITEMINDEX,"; //ITEMINDEX
                SQL = SQL + "\r\n" + " @ITEMTYPE,";   //ITEMTYPE
                SQL = SQL + "\r\n" + " @ITEMVALUE,";   //ITEMVALUE
                SQL = SQL + "\r\n" + " @DSPSEQ,";   //DSPSEQ
                SQL = SQL + "\r\n" + " @ITEMVALUE1";   //ITEMVALUE
                SQL = SQL + "\r\n" + ")";

                cmd.Parameters.Add("@ITEMCD", OracleDbType.Varchar2, 15);
                cmd.Parameters.Add("@ITEMNO", OracleDbType.Varchar2, 11);
                cmd.Parameters.Add("@ITEMINDEX", OracleDbType.Int32);
                cmd.Parameters.Add("@ITEMTYPE", OracleDbType.Varchar2, 10);
                cmd.Parameters.Add("@ITEMVALUE", OracleDbType.Varchar2, 4000);
                cmd.Parameters.Add("@DSPSEQ", OracleDbType.Int32);
                cmd.Parameters.Add("@ITEMVALUE1", OracleDbType.Varchar2, 4000);
                cmd.Prepare();

                for (int i = 0; i < strITEMCD.Length; i++)
                {
                    cmd.Parameters["@ITEMCD"].Value = strITEMCD[i];
                    cmd.Parameters["@ITEMNO"].Value = strITEMNO[i];
                    cmd.Parameters["@ITEMINDEX"].Value = strITEMINDEX[i];
                    cmd.Parameters["@ITEMTYPE"].Value = strITEMTYPE[i];
                    cmd.Parameters["@ITEMVALUE"].Value = strITEMVALUE[i];
                    cmd.Parameters["@DSPSEQ"].Value = strDSPSEQ[i];
                    cmd.Parameters["@ITEMVALUE1"].Value = strITEMVALUE1[i];
                    cmd.ExecuteNonQuery();
                }
                cmd.Dispose();
                cmd = null;
                return true;
            }
            catch (OracleException sqlExc)
            {
                if ((gTrs != null))
                {
                    throw new Exception(sqlExc.Message);
                }

                ComFunc.MsgBox( sqlExc.Message);
                return false;
            }
            catch (Exception exc)
            {
                if ((gTrs != null))
                {
                    throw new Exception(exc.Message);
                }

                ComFunc.MsgBox( exc.Message);
                return false;
            }
        }

        public static bool ExecuteChartForm(string SQL, double dblEmrNoNew,
            string[] strCONTROLNAME, string[] strCONTROTYPE, string[] strCONTROLPARENT, string[] strLOCATIONX, string[] strLOCATIONY,
            string[] strSIZEWIDTH, string[] strSIZEHEIGHT, string[] strTAG, string[] strCHILDINDEX, string[] strBACKCOLOR, string[] strFORECOLOR,
            string[] strBOARDSTYLE, string[] strDOCK, string[] strENABLED, string[] strVISIBLED, string[] strTEXT, string[] strFONTS,
            string[] strMULTILINE, string[] strSCROLLBARS, string[] strTEXTALIGN, string[] strIMAGE, string[] strIMAGESIZEMODE, string[] strCHECKED
            )
        {
            try
            {
                if (cmd == null)
                {
                    cmd = gDc.CreateCommand();
                }

                cmd.CommandText = SQL;
                cmd.CommandTimeout = 60;
                if (gTrs != null)
                {
                    cmd.Transaction = gTrs;
                }

                SQL = "INSERT INTO MHEMR.AEMRCHARTFORM";
                SQL = SQL + "\r\n" + "    (EMRNO, CONTROLNAME,CONTROTYPE,CONTROLPARENT,LOCATIONX,LOCATIONY,";
                SQL = SQL + "\r\n" + "    SIZEWIDTH,SIZEHEIGHT,TAG,CHILDINDEX, BACKCOLOR,FORECOLOR,BOARDSTYLE,DOCK,ENABLED,VISIBLED,TEXT,";
                SQL = SQL + "\r\n" + "    FONTS,MULTILINE,SCROLLBARS,TEXTALIGN,IMAGE,IMAGESIZEMODE,CHECKED)";
                SQL = SQL + "\r\n" + "VALUES (";
                SQL = SQL + "\r\n" + dblEmrNoNew.ToString() + ",";    //EMRNO
                SQL = SQL + "\r\n" + " @CONTROLNAME,";  //CONTROLNAME    ,
                SQL = SQL + "\r\n" + " @CONTROTYPE ,";  //CONTROTYPE    ,
                SQL = SQL + "\r\n" + " @CONTROLPARENT,";   //CONTROLPARENT  ,
                SQL = SQL + "\r\n" + " @LOCATIONX,";   //LOCATIONX      ,
                SQL = SQL + "\r\n" + " @LOCATIONY ,";   //LOCATIONY      ,
                SQL = SQL + "\r\n" + " @SIZEWIDTH ,";   //SIZEWIDTH      ,
                SQL = SQL + "\r\n" + " @SIZEHEIGHT ,";  //SIZEHEIGHT      ,
                SQL = SQL + "\r\n" + " @TAG ,";   //TAG      ,
                SQL = SQL + "\r\n" + " @CHILDINDEX ,";  //CHILDINDEX      ,
                SQL = SQL + "\r\n" + " @BACKCOLOR ,";   //BACKCOLOR      ,
                SQL = SQL + "\r\n" + " @FORECOLOR ,";   //FORECOLOR      ,
                SQL = SQL + "\r\n" + " @BOARDSTYLE ,";  //BOARDSTYLE     ,
                SQL = SQL + "\r\n" + " @DOCK ,";   //DOCK           ,
                SQL = SQL + "\r\n" + " @ENABLED ,";   //ENABLED        ,
                SQL = SQL + "\r\n" + " @VISIBLED ,";   //VISIBLED        ,
                SQL = SQL + "\r\n" + " @TEXT,";   //TEXT           ,
                SQL = SQL + "\r\n" + " @FONTS ,";   //FONTS          ,
                SQL = SQL + "\r\n" + " @MULTILINE ,";   //MULTILINE      ,
                SQL = SQL + "\r\n" + " @SCROLLBARS ,";  //SCROLLBARS     ,
                SQL = SQL + "\r\n" + " @TEXTALIGN ,";  //TEXTALIGN      ,
                SQL = SQL + "\r\n" + " @IMAGE ,";   //IMAGE          ,
                SQL = SQL + "\r\n" + " @IMAGESIZEMODE,";   //IMAGESIZEMODE  ,
                SQL = SQL + "\r\n" + " @CHECKED)"; //CHECKED  ,

                cmd.Parameters.Add("@CONTROLNAME", OracleDbType.Varchar2, 40);
                cmd.Parameters.Add("@CONTROTYPE", OracleDbType.Varchar2, 40);
                cmd.Parameters.Add("@CONTROLPARENT", OracleDbType.Varchar2, 40);
                cmd.Parameters.Add("@LOCATIONX", OracleDbType.Int32);
                cmd.Parameters.Add("@LOCATIONY", OracleDbType.Int32);
                cmd.Parameters.Add("@SIZEWIDTH", OracleDbType.Int32);
                cmd.Parameters.Add("@SIZEHEIGHT", OracleDbType.Int32);
                cmd.Parameters.Add("@TAG", OracleDbType.Varchar2, 100);
                cmd.Parameters.Add("@CHILDINDEX", OracleDbType.Int32);
                cmd.Parameters.Add("@BACKCOLOR", OracleDbType.Varchar2, 20);
                cmd.Parameters.Add("@FORECOLOR", OracleDbType.Varchar2, 20);
                cmd.Parameters.Add("@BOARDSTYLE", OracleDbType.Varchar2, 10);
                cmd.Parameters.Add("@DOCK", OracleDbType.Varchar2, 10);
                cmd.Parameters.Add("@ENABLED", OracleDbType.Varchar2, 10);
                cmd.Parameters.Add("@VISIBLED", OracleDbType.Varchar2, 10);
                cmd.Parameters.Add("@TEXT", OracleDbType.Varchar2, 200);
                cmd.Parameters.Add("@FONTS", OracleDbType.Varchar2, 20);
                cmd.Parameters.Add("@MULTILINE", OracleDbType.Varchar2, 10);
                cmd.Parameters.Add("@SCROLLBARS", OracleDbType.Varchar2, 10);
                cmd.Parameters.Add("@TEXTALIGN", OracleDbType.Varchar2, 20);
                cmd.Parameters.Add("@IMAGE", OracleDbType.Varchar2, 20);
                cmd.Parameters.Add("@IMAGESIZEMODE", OracleDbType.Varchar2, 20);
                cmd.Parameters.Add("@CHECKED", OracleDbType.Varchar2, 10);
                cmd.Prepare();

                for (int i = 0; i < strCONTROLNAME.Length; i++)
                {
                    cmd.Parameters["@CONTROLNAME"].Value = strCONTROLNAME[i];
                    cmd.Parameters["@CONTROTYPE"].Value = strCONTROTYPE[i];
                    cmd.Parameters["@CONTROLPARENT"].Value = strCONTROLPARENT[i];
                    cmd.Parameters["@LOCATIONX"].Value = strLOCATIONX[i];
                    cmd.Parameters["@LOCATIONY"].Value = strLOCATIONY[i];
                    cmd.Parameters["@SIZEWIDTH"].Value = strSIZEWIDTH[i];
                    cmd.Parameters["@SIZEHEIGHT"].Value = strSIZEHEIGHT[i];
                    cmd.Parameters["@TAG"].Value = strTAG[i];
                    cmd.Parameters["@CHILDINDEX"].Value = strCHILDINDEX[i];
                    cmd.Parameters["@BACKCOLOR"].Value = strBACKCOLOR[i];
                    cmd.Parameters["@FORECOLOR"].Value = strFORECOLOR[i];
                    cmd.Parameters["@BOARDSTYLE"].Value = strBOARDSTYLE[i];
                    cmd.Parameters["@DOCK"].Value = strDOCK[i];
                    cmd.Parameters["@ENABLED"].Value = strENABLED[i];
                    cmd.Parameters["@VISIBLED"].Value = strVISIBLED[i];
                    cmd.Parameters["@TEXT"].Value = strTEXT[i];
                    cmd.Parameters["@FONTS"].Value = strFONTS[i];
                    cmd.Parameters["@MULTILINE"].Value = strMULTILINE[i];
                    cmd.Parameters["@SCROLLBARS"].Value = strSCROLLBARS[i];
                    cmd.Parameters["@TEXTALIGN"].Value = strTEXTALIGN[i];
                    cmd.Parameters["@IMAGE"].Value = strIMAGE[i];
                    cmd.Parameters["@IMAGESIZEMODE"].Value = strIMAGESIZEMODE[i];
                    cmd.Parameters["@CHECKED"].Value = strCHECKED[i];
                    cmd.ExecuteNonQuery();
                }
                cmd.Dispose();
                cmd = null;
                return true;
            }
            catch (OracleException sqlExc)
            {
                if ((gTrs != null))
                {
                    throw new Exception(sqlExc.Message);
                }

                ComFunc.MsgBox( sqlExc.Message);
                return false;
            }
            catch (Exception exc)
            {
                if ((gTrs != null))
                {
                    throw new Exception(exc.Message);
                }

                ComFunc.MsgBox( exc.Message);
                return false;
            }
        }

        /// <summary>
        /// 인증저장
        /// </summary>
        /// <param name="SQL"></param>
        /// <param name="p_EMRNO"></param>
        /// <param name="p_CERTIUSEID"></param>
        /// <param name="p_CERTIDATE"></param>
        /// <param name="p_CERTITIME"></param>
        /// <param name="p_CERTIKEY"></param>
        /// <param name="p_CERTITEXT"></param>
        /// <returns></returns>
        public static bool ExecuteChartCerty(string SQL, string p_EMRNO, string p_CERTIUSEID, string p_CERTIPW, string p_CERTIDATE, string p_CERTITIME, string p_CERTIKEY, string p_CERTITEXT)
        {
            try
            {
                if (cmd == null)
                {
                    cmd = gDc.CreateCommand();
                }

                cmd.CommandTimeout = 60;
                if (gTrs != null)
                {
                    //cmd.Transaction = gTrs;
                }

                SQL = "MHEMR.XMLCERTYINSRT ";
                cmd.CommandText = SQL;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("p_EMRNO", OracleDbType.Double).Value = Convert.ToDouble(p_EMRNO);
                cmd.Parameters.Add("p_CERTIUSEID", OracleDbType.Varchar2).Value = p_CERTIUSEID;
                cmd.Parameters.Add("p_CERTIPW", OracleDbType.Varchar2).Value = p_CERTIPW;
                cmd.Parameters.Add("p_CERTIDATE", OracleDbType.Varchar2).Value = p_CERTIDATE;
                cmd.Parameters.Add("p_CERTITIME", OracleDbType.Varchar2).Value = p_CERTITIME;
                cmd.Parameters.Add("p_CERTIKEY", OracleDbType.Varchar2).Value = p_CERTIKEY;
                cmd.Parameters.Add("p_CERTITEXT", OracleDbType.Varchar2).Value = p_CERTITEXT;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                cmd = null;
                return true;
            }
            catch (OracleException sqlExc)
            {
                if ((gTrs != null))
                {
                    throw new Exception(sqlExc.Message);
                }

                ComFunc.MsgBox(sqlExc.Message);
                return false;
            }
            catch (Exception exc)
            {
                if ((gTrs != null))
                {
                    throw new Exception(exc.Message);
                }

                ComFunc.MsgBox( exc.Message);
                return false;
            }
        }
        //Xml, Clob 
        //SQL = "INSERT INTO TABLE ";
        //SQL = SQL + "\r\n" + "(COLUMN1, COLUMN2, XML_COLUMN1, XML_COLUMN2)
        //SQL = SQL + "\r\n" + "VALUES (";
        //SQL = SQL + "\r\n" + "ColValue1,"
        //SQL = SQL + "\r\n" + "ColValue1,"
        //SQL = SQL + "\r\n" + ":1,"
        //SQL = SQL + "\r\n" + ":1)"
    }
}
