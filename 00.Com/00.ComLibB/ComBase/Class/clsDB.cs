using System;
using System.Data;
using System.IO;
using FarPoint.Win.Spread;
using ComDbB;
using ComBase; //기본 클래스
using System.Reflection;
using System.Data.Common;
using System.Collections.Generic;
using ComBase.Mvc;
using System.Text;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Utils;
using ComBase.Mvc.Exceptions;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace ComBase
{
    public class clsDB
    {
        public static PsmhDb DbCon = null;  //기본 연결 객체
        public static PsmhDb DbConErr = null;  //에러 로그 연결 객체

        public static string strDbIniFile = @"C:\HealthSoft\HSMain\psmh.ini";
        public static string strDevDbIniFile = @"C:\PSMHEXE\exenet\psmhDev.ini";
        public static string strSource = "PSMH";
        public static string strDbIp = "";
        public static string strDbPort = "";
        public static string strUser = "KOSMOS_PMPA";
        public static string strPassWord = "";
        public static string strDbOption = "";

        /// <summary>
        /// DB접속 정보를 INI 파일에서 가지고 온다
        /// </summary>
        public static void GetDbInfo()
        {
            PsmhAES pAES = new PsmhAES();
            PsmhIniFile myIniFile = new PsmhIniFile(strDbIniFile);

            strDbIp = myIniFile.ReadValue("DBINFO", "DbIp", "");
            strDbPort = myIniFile.ReadValue("DBINFO", "DbPort", "");
            strSource = pAES.Base64Decode(myIniFile.ReadValue("DBINFO", "Source", "").Replace("^", "="));
            strUser = pAES.Base64Decode(myIniFile.ReadValue("DBINFO", "User", "").Replace("^", "="));
            strPassWord = myIniFile.ReadValue("DBINFO", "PassWord", "").Replace("^", "=");
            strPassWord = GetPassWord(strPassWord);

            strDbOption = myIniFile.ReadValue("DBOPTION", "Option", "");
            
            pAES = null;
            myIniFile = null;
        }

        /// <summary>
        /// DB 접속 비밀번호
        /// </summary>
        /// <returns></returns>
        private static string GetPassWord(string pPassWord)
        {
            clsAES pAES = new clsAES();
            string strPassWord = "";
            string strPassWordTmp = clsAES.Base64Decode(pPassWord);


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
            return strPassWord;
        }

        /// <summary>
        /// DB Connect 함수 입니다.(기본)
        /// </summary>
        /// <returns>DataBase Connection 객체</returns>
        public static PsmhDb DBConnect_Cloud()
        {
            try
            {
                PsmhDb pPsmhDb = new PsmhDb();

                pPsmhDb.strDbIp = "";

                if (pPsmhDb.DBConnect_Cloud() == true)
                {
                    return pPsmhDb;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exc)
            {
                ComFunc.MsgBox(exc.Message);
                return null;
            }
        }

        /// <summary>
        /// DB Connect 함수 입니다.(기본)
        /// </summary>
        /// <returns>DataBase Connection 객체</returns>
        public static PsmhDb DBConnect()
        {
            try
            {
                PsmhDb pPsmhDb = new PsmhDb();

                if (pPsmhDb.DBConnect() == true)
                {
                    return pPsmhDb;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exc)
            {
                ComFunc.MsgBox(exc.Message);
                return null;
            }
        }

        public static PsmhDb DBConnectEx(string DbMode)
        {
            try
            {
                PsmhDb pPsmhDb = new PsmhDb();

                if (DbMode.Equals("DEV"))
                {
                    pPsmhDb.strDbIniFile = pPsmhDb.strDevDbIniFile;
                }

                if (pPsmhDb.DBConnect() == true)
                {
                    return pPsmhDb;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exc)
            {
                ComFunc.MsgBox(exc.Message);
                return null;
            }
        }

        public static PsmhDb DBConnectEx2(string DbMode)
        {
            try
            {
                PsmhDb pPsmhDb = new PsmhDb();

                if (DbMode.Equals("DEV"))
                {
                    pPsmhDb.strDbIniFile = pPsmhDb.strDevDbIniFile;
                }

                if (pPsmhDb.DBConnectEx() == true)
                {
                    return pPsmhDb;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exc)
            {
                ComFunc.MsgBox(exc.Message);
                return null;
            }
        }


        /// <summary>
        /// DB Connect 함수 : PSMH가 아닌 타 데이타 베이스 접속시 사용
        /// </summary>
        /// <param name="pstrDbIp"></param>
        /// <param name="pstrDbPort"></param>
        /// <param name="pstrSource"></param>
        /// <param name="pstrUser"></param>
        /// <param name="pstrPassWord"></param>
        /// <returns>DataBase Connection 객체</returns>
        public static PsmhDb DBConnect(string pstrDbIp, string pstrDbPort, string pstrSource, string pstrUser, string pstrPassWord)
        {
            #region //사용법
            //PsmhDb pPsmhDb = null;
            //pPsmhDb = clsDB.DBConnect(pstrDbIp, pstrDbPort, pstrSource, pstrUser, pstrPassWord);
            //if (pPsmhDb == null)
            //{
            //    ComFunc.MsgBox("접속실패");
            //}
            //clsDB.setBeginTran(pPsmhDb);
            //SqlErr = clsDB.GetDataTable(ref dt, SQL, pPsmhDb);
            //SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pPsmhDb);
            //clsDB.setCommitTran(pPsmhDb);
            //clsDB.setRollbackTran(pPsmhDb);
            //clsDB.DisDBConnect(pPsmhDb);
            #endregion

            try
            {
                PsmhDb pPsmhDb = new PsmhDb();

                if (pPsmhDb.DBConnectEx(pstrDbIp, pstrDbPort, pstrSource, pstrUser, pstrPassWord) == true)
                {
                    return pPsmhDb;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exc)
            {
                ComFunc.MsgBox(exc.Message);
                return null;
            }
        }

        /// <summary>
        /// DB 연결해제 함수 입니다.
        /// </summary>
        public static void DisDBConnect(PsmhDb pDbCon)
        {
            try
            {
                pDbCon.Con.Close();
                pDbCon.Con.Dispose();
                pDbCon.Con = null;
                pDbCon = null;
            }
            catch (Exception exc)
            {
                //ComFunc.MsgBox(exc.Message);
                pDbCon = null;
            }
        }

        /// <summary>
        /// Transaction 객체 생성
        /// </summary>
        /// <returns></returns>
        public static void setBeginTran(PsmhDb pDbCon)
        {
            try
            {
                if (pDbCon == null)
                {
                    pDbCon = clsDB.DbCon;
                }
                pDbCon.Trs = pDbCon.Con.BeginTransaction(IsolationLevel.ReadCommitted);
                return;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                pDbCon.Trs = null;
                return;
            }
        }

        /// <summary>
        /// Transaction Commit
        /// </summary>
        /// <param name="pTran">Transaction 객체</param>
        public static void setCommitTran(PsmhDb pDbCon)
        {
            try
            {
                if (pDbCon == null)
                {
                    pDbCon = clsDB.DbCon;
                }
                pDbCon.Trs.Commit();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
            finally
            {
                pDbCon.Trs = null;
            }
        }

        /// <summary>
        /// Transaction을 취소한다
        /// </summary>
        /// <param name="pTran">Transaction 객체</param>
        public static void setRollbackTran(PsmhDb pDbCon)
        {
            try
            {
                if (pDbCon == null)
                {
                    pDbCon = clsDB.DbCon;
                }
                pDbCon.Trs.Rollback();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
            finally
            {
                pDbCon.Trs = null;
            }
        }

        /// <summary>
        /// DB 단순 쿼리에 결과 값을 DataSet 값으로 리턴 시켜줍니다. 
        /// </summary>
        /// <param name="ds">DataSet</param>
        /// <param name="SQL">Query 문</param>
        /// <param name="pTran">Transaction</param>
        /// <returns>에러로그 + DataSet</returns>
        public static string GetDataSet(ref DataSet ds, string SQL, PsmhDb pDbCon)
        {
            OracleDataAdapter Adapter = default(OracleDataAdapter);
            DataSet ds1 = new DataSet();
            string rtnVal = "";

            try
            {
                if (pDbCon == null)
                {
                    pDbCon = clsDB.DbCon;
                }
                Adapter = new OracleDataAdapter(SQL, pDbCon.Con);
                if (pDbCon.Trs != null)
                {
                    Adapter.SelectCommand.Transaction = pDbCon.Trs;
                }
                Adapter.SelectCommand.InitialLONGFetchSize = -1;

                Adapter.Fill(ds1);
                ds = ds1;
                ds1.Dispose();
                ds1 = null;

                SaveSqlLog(SQL, pDbCon); //Query Log 저장

                return rtnVal;
            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return rtnVal;
            }
            finally
            {
                if ((Adapter != null))
                {
                    Adapter.Dispose();
                    Adapter = null;
                }
                if ((ds1 != null))
                {
                    ds1.Dispose();
                    ds1 = null;
                }
            }
        }

        /// <summary>
        /// DB 단순 쿼리에 결과 값을 DataSet 값으로 리턴 시켜줍니다. : 로그 안남김
        /// 시스템시간 가져오기 등 너무 많이 사용것 혹은 개인정보와 관계 없는 쿼리시 사용
        /// </summary>
        /// <param name="ds">DataSet</param>
        /// <param name="SQL">Query 문</param>
        /// <param name="pTran">Transaction</param>
        /// <returns>에러로그 + DataSet</returns>
        public static string GetDataSetEx(ref DataSet ds, string SQL, PsmhDb pDbCon)
        {
            OracleDataAdapter Adapter = default(OracleDataAdapter);
            DataSet ds1 = new DataSet();
            string rtnVal = "";

            try
            {
                if (pDbCon == null)
                {
                    pDbCon = clsDB.DbCon;
                }
                Adapter = new OracleDataAdapter(SQL, pDbCon.Con);
                if (pDbCon.Trs != null)
                {
                    Adapter.SelectCommand.Transaction = pDbCon.Trs;
                }
                Adapter.SelectCommand.InitialLONGFetchSize = -1;

                Adapter.Fill(ds1);
                ds = ds1;
                ds1.Dispose();
                ds1 = null;

                return rtnVal;
            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return rtnVal;
            }
            finally
            {
                if ((Adapter != null))
                {
                    Adapter.Dispose();
                    Adapter = null;
                }
                if ((ds1 != null))
                {
                    ds1.Dispose();
                    ds1 = null;
                }
            }
        }

        /// <summary>
        /// DB 단순 쿼리에 결과 값을 DataTable 값으로 리턴 시켜줍니다. 
        /// </summary>
        /// <param name="dt">Datatable</param>
        /// <param name="SQL">Query문</param>
        /// <param name="pTran">트렌젝션</param>
        /// <returns>에러로그 + Datatable</returns>
        public static string GetDataTable_nnn(ref DataTable dt, string SQL, PsmhDb pDbCon)
        {
            OracleDataReader reader = null;
            OracleCommand cmd = null;
            string rtnVal = "";
            try
            {
                if (pDbCon == null)
                {
                    pDbCon = clsDB.DbCon;
                }

                cmd = pDbCon.Con.CreateCommand();

                dt = new DataTable();

                cmd.CommandText = SQL;
                cmd.CommandTimeout = 30;
                reader = cmd.ExecuteReader();

                dt.Load(reader);
                reader.Dispose();
                reader = null;

                cmd.Dispose();
                cmd = null;

                SaveSqlLog(SQL, pDbCon); //Query Log 저장

                return rtnVal;

            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return rtnVal;
            }
            finally
            {
                if ((reader != null))
                    reader.Dispose();
                reader = null;
            }
        }

        public static string GetDataTable(ref DataTable dt, string SQL, PsmhDb pDbCon)
        {
            string rtnVal = "";
            try
            {
                if (pDbCon == null)
                {
                    pDbCon = clsDB.DbCon;
                }

                dt = new DataTable();
                using (OracleCommand cmd = new OracleCommand(SQL, pDbCon.Con))
                {
                    cmd.InitialLONGFetchSize = -1;
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }

                // SaveSqlLog(SQL, pDbCon); //Query Log 저장

                return rtnVal;

            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return rtnVal;
            }
            //finally
            //{
            //    if ((da != null))
            //    {
            //        da.Dispose();
            //        da = null;
            //    }
            //    if ((ds != null))
            //    {
            //        ds.Dispose();
            //        ds = null;
            //    }
            //}
        }

        /// <summary>
        /// DB 단순 쿼리에 결과 값을 DataTable 값으로 리턴 시켜줍니다. : 로그 안남김
        /// 시스템시간 가져오기 등 너무 많이 사용것 혹은 개인정보와 관계 없는 쿼리시 사용
        /// </summary>
        /// <param name="dt">Datatable</param>
        /// <param name="SQL">Query문</param>
        /// <param name="pTran">트렌젝션</param>
        /// <returns>에러로그 + Datatable</returns>
        public static string GetDataTableEx_nnn(ref DataTable dt, string SQL, PsmhDb pDbCon)
        {
            OracleDataReader reader = null;
            OracleCommand cmd = null;
            string rtnVal = "";
            try
            {
                if (pDbCon == null)
                {
                    pDbCon = clsDB.DbCon;
                }

                cmd = pDbCon.Con.CreateCommand();

                dt = new DataTable();

                cmd.CommandText = SQL;
                cmd.CommandTimeout = 30;
                reader = cmd.ExecuteReader();

                dt.Load(reader);
                reader.Dispose();
                reader = null;

                cmd.Dispose();
                cmd = null;

                return rtnVal;

            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return rtnVal;
            }
            finally
            {
                if ((reader != null))
                    reader.Dispose();
                reader = null;
            }
        }

        public static string GetDataTableEx(ref DataTable dt, string SQL, PsmhDb pDbCon)
        {
            OracleDataAdapter da = default(OracleDataAdapter);
            DataSet ds = null;
            string rtnVal = "";
            try
            {
                if (pDbCon == null)
                {
                    pDbCon = clsDB.DbCon;
                }
                da = new OracleDataAdapter(SQL, pDbCon.Con);

                if (pDbCon.Trs == null)
                {
                    da.SelectCommand.Transaction = pDbCon.Trs;
                }

                da.SelectCommand.InitialLONGFetchSize = -1;

                ds = new DataSet();

                da.Fill(ds);
                da.Dispose();
                da = null;
                dt = ds.Tables[0];

                return rtnVal;

            }
            catch (OracleException sqlExc)
            {
                if (VB.L(sqlExc.Message, "not logged") > 1 || VB.L(sqlExc.Message, "not connected") > 1)
                {
                    Application.ExitThread();
                    Environment.Exit(0);
                }
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return rtnVal;
            }
            finally
            {
                if ((da != null))
                {
                    da.Dispose();
                    da = null;
                }
                if ((ds != null))
                {
                    ds.Dispose();
                    ds = null;
                }
            }
        }

        public static string GetDataTableR(ref DataTable dt, string SQL, PsmhDb pDbCon)
        {
            OracleDataReader reader = null;
            OracleCommand cmd = null;
            string rtnVal = "";
            try
            {
                if (pDbCon == null)
                {
                    pDbCon = clsDB.DbCon;
                }

                cmd = pDbCon.Con.CreateCommand();

                dt = new DataTable();

                cmd.CommandText = SQL;
                cmd.CommandTimeout = 30;
                reader = cmd.ExecuteReader();

                dt.Load(reader);
                reader.Dispose();
                reader = null;

                cmd.Dispose();
                cmd = null;

                SaveSqlLog(SQL, pDbCon); //Query Log 저장

                return rtnVal;

            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return rtnVal;
            }
            finally
            {
                if ((reader != null))
                    reader.Dispose();
                reader = null;
            }
        }

        public static string GetDataTableRP(ref DataTable dt, string SQL, PsmhDb pDbCon, ClsParameter clsParameter)
        {
            OracleDataReader reader = null;
            OracleCommand cmd = null;
            string rtnVal = "";
            try
            {
                if (pDbCon == null)
                {
                    pDbCon = clsDB.DbCon;
                }

                cmd = pDbCon.Con.CreateCommand();

                dt = new DataTable();

                cmd.CommandText = SQL;
                cmd.CommandTimeout = 30;

                if (clsParameter != null)
                {
                    foreach (OracleParameter parameter in clsParameter.Parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }
                }

                reader = cmd.ExecuteReader();

                dt.Load(reader);
                reader.Dispose();
                reader = null;

                cmd.Dispose();
                cmd = null;

                SaveSqlLog(SQL, pDbCon); //Query Log 저장

                return rtnVal;

            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return rtnVal;
            }
            finally
            {
                if ((reader != null))
                    reader.Dispose();
                reader = null;
            }
        }

        public static string GetDataTableREx(ref DataTable dt, string SQL, PsmhDb pDbCon)
        {
            OracleDataReader reader = null;
            OracleCommand cmd = null;
            string rtnVal = "";
            try
            {
                if (pDbCon == null)
                {
                    pDbCon = clsDB.DbCon;
                }

                cmd = pDbCon.Con.CreateCommand();

                dt = new DataTable();

                cmd.InitialLONGFetchSize = -1;
                cmd.CommandText = SQL;
                //cmd.CommandTimeout = 60;
                reader = cmd.ExecuteReader();

                dt.Load(reader);
                reader.Dispose();
                reader = null;

                cmd.Dispose();
                cmd = null;

                return rtnVal;

            }
            catch (OracleException sqlExc)
            {
                if (VB.L(sqlExc.Message, "not logged") > 1 || VB.L(sqlExc.Message, "not connected") > 1)
                {
                    Application.ExitThread();
                    Environment.Exit(0);
                }
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return rtnVal;
            }
            finally
            {
                if ((reader != null))
                    reader.Dispose();
                reader = null;
            }
        }

        public static string GetDataTableRExP(ref DataTable dt, string SQL, PsmhDb pDbCon, ClsParameter clsParameter)
        {
            OracleDataReader reader = null;
            OracleCommand cmd = null;
            string rtnVal = "";
            try
            {
                if (pDbCon == null)
                {
                    pDbCon = clsDB.DbCon;
                }

                cmd = pDbCon.Con.CreateCommand();

                dt = new DataTable();

                cmd.InitialLONGFetchSize = -1;
                cmd.CommandText = SQL;
                //cmd.CommandTimeout = 30;

                if (clsParameter != null)
                {
                    foreach (OracleParameter parameter in clsParameter.Parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }
                }

                reader = cmd.ExecuteReader();

                dt.Load(reader);
                reader.Dispose();
                reader = null;

                cmd.Dispose();
                cmd = null;

                return rtnVal;

            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return rtnVal;
            }
            finally
            {
                if ((reader != null))
                    reader.Dispose();
                reader = null;
            }
        }

        public static string GetAdoRs(ref OracleDataReader reader, string SQL, PsmhDb pDbCon)
        {

            try
            {
                if (pDbCon == null)
                {
                    pDbCon = clsDB.DbCon;
                }
                using (OracleCommand cmd = pDbCon.Con.CreateCommand())
                {
                    cmd.InitialLONGFetchSize = -1;
                    cmd.CommandText = SQL;
                    cmd.CommandTimeout = 30;

                    reader = cmd.ExecuteReader();
                }

                return string.Empty;
            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                return sqlExc.Message;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return ex.Message;
            }
        }

        public static string GetAdoRsP(ref OracleDataReader reader, string SQL, PsmhDb pDbCon, ClsParameter clsParameter)
        {
            OracleCommand cmd = null;

            try
            {
                if (pDbCon == null)
                {
                    pDbCon = clsDB.DbCon;
                }
                cmd = pDbCon.Con.CreateCommand();
                cmd.CommandText = SQL;
                cmd.CommandTimeout = 30;

                if (clsParameter != null)
                {
                    foreach (OracleParameter parameter in clsParameter.Parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }
                }

                reader = cmd.ExecuteReader();

                cmd.Dispose();
                cmd = null;

                return "";
            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                return sqlExc.Message;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return ex.Message;
            }
        }

        /// <summary>
        /// Query 문을 실행한다 - CommandTimeout 타임 임의 변경 가능 로직
        /// </summary>
        /// <date>2018.02.23</date>
        /// <author>김홍록</author>
        /// <param name="SQL">Query 문</param>
        /// <param name="RowAffected">적용된 Data Row 수</param>
        /// <param name="pTran">트렌젝션</param>
        /// <param name="nComTimeOUT">링크타임 설정</param>
        /// <returns>에러로그</returns>
        public static string ExecuteNonQuery(string SQL, ref int RowAffected, PsmhDb pDbCon, int nComTimeOUT)
        {
            OracleCommand Cmd = null;
            string rtnVal = "";
            try
            {
                if (pDbCon == null)
                {
                    pDbCon = clsDB.DbCon;
                }

                if (Cmd == null)
                {
                    Cmd = pDbCon.Con.CreateCommand();
                }

                Cmd.CommandText = SQL;
                Cmd.CommandTimeout = nComTimeOUT;

                if (pDbCon.Trs != null)
                {
                    Cmd.Transaction = pDbCon.Trs;
                }

                RowAffected = Cmd.ExecuteNonQuery();

                SaveSqlLog(SQL, pDbCon); //Query Log 저장

                return rtnVal;
            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return rtnVal;
            }
            finally
            {
                if (Cmd != null)
                {
                    Cmd.Dispose();
                    Cmd = null;
                }
            }
        }

        /// <summary>
        /// Query 문을 실행한다
        /// </summary>
        /// <param name="SQL">Query 문</param>
        /// <param name="RowAffected">적용된 Data Row 수</param>
        /// <param name="pTran">트렌젝션</param>
        /// <returns>에러로그</returns>
        public static string ExecuteNonQuery(string SQL, ref int RowAffected, PsmhDb pDbCon)
        {
            OracleCommand Cmd = null;
            string rtnVal = "";
            try
            {
                if (pDbCon == null)
                {
                    pDbCon = clsDB.DbCon;
                }

                if (Cmd == null)
                {
                    Cmd = pDbCon.Con.CreateCommand();
                }

                Cmd.CommandText = SQL;
                Cmd.CommandTimeout = 60;

                if (pDbCon.Trs != null)
                {
                    Cmd.Transaction = pDbCon.Trs;
                }

                RowAffected = Cmd.ExecuteNonQuery();

                SaveSqlLog(SQL, pDbCon); //Query Log 저장

                return rtnVal;
            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return rtnVal;
            }
            finally
            {
                if (Cmd != null)
                {
                    Cmd.Dispose();
                    Cmd = null;
                }
            }
        }


        /// <summary>
        /// Query 문을 실행한다 - CommandTimeout 타임 임의 변경 가능 로직  : SaveSqlLog 제외
        /// </summary>
        /// <date>2018.02.23</date>
        /// <author>김홍록</author>
        /// <param name="SQL">Query 문</param>
        /// <param name="RowAffected">적용된 Data Row 수</param>
        /// <param name="pTran">트렌젝션</param>
        /// <param name="nComTimeOUT">링크타임 설정</param>
        /// <returns>에러로그</returns>
        public static string ExecuteNonQueryEx(string SQL, ref int RowAffected, PsmhDb pDbCon, int nComTimeOUT)
        {
            OracleCommand Cmd = null;
            string rtnVal = "";
            try
            {
                if (pDbCon == null)
                {
                    pDbCon = clsDB.DbCon;
                }

                if (Cmd == null)
                {
                    Cmd = pDbCon.Con.CreateCommand();
                }

                Cmd.CommandText = SQL;
                Cmd.CommandTimeout = nComTimeOUT;

                if (pDbCon.Trs != null)
                {
                    Cmd.Transaction = pDbCon.Trs;
                }

                RowAffected = Cmd.ExecuteNonQuery();

                //SaveSqlLog(SQL, pDbCon); //Query Log 저장

                return rtnVal;
            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return rtnVal;
            }
            finally
            {
                if (Cmd != null)
                {
                    Cmd.Dispose();
                    Cmd = null;
                }
            }
        }

        /// <summary>
        /// Query 문을 실행한다 : SaveSqlLog 제외
        /// </summary>
        /// <param name="SQL">Query 문</param>
        /// <param name="RowAffected">적용된 Data Row 수</param>
        /// <param name="pTran">트렌젝션</param>
        /// <returns>에러로그</returns>
        public static string ExecuteNonQueryEx(string SQL, ref int RowAffected, PsmhDb pDbCon)
        {
            OracleCommand Cmd = null;
            string rtnVal = "";
            try
            {
                if (pDbCon == null)
                {
                    pDbCon = clsDB.DbCon;
                }

                if (Cmd == null)
                {
                    Cmd = pDbCon.Con.CreateCommand();
                }

                Cmd.CommandText = SQL;
                Cmd.CommandTimeout = 60;

                if (pDbCon.Trs != null)
                {
                    Cmd.Transaction = pDbCon.Trs;
                }

                RowAffected = Cmd.ExecuteNonQuery();

                //SaveSqlLog(SQL, pDbCon); //Query Log 저장

                return rtnVal;
            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return rtnVal;
            }
            finally
            {
                if (Cmd != null)
                {
                    Cmd.Dispose();
                    Cmd = null;
                }
            }
        }

        /// <summary>
        /// Long Type 컬럼이 있는 테이블에 작업을 할 경우 사용
        /// </summary>
        /// <param name="SQL">Query 문</param>
        /// <param name="strLong">Long 컬럼의 값</param>
        /// <param name="RowAffected">적용된 Data Row 수</param>
        /// <param name="pTran">트렌젝션</param>
        /// <returns>에러로그</returns>
        public static string ExecuteLongQuery(string SQL, string strLong, ref int RowAffected, PsmhDb pDbCon)
        {
            OracleCommand Cmd = null;
            string rtnVal = "";
            try
            {
                if (Cmd == null)
                {
                    Cmd = pDbCon.Con.CreateCommand();
                }

                Cmd.CommandText = SQL;
                Cmd.CommandTimeout = 60;
                if (pDbCon.Trs != null)
                {
                    Cmd.Transaction = pDbCon.Trs;
                }
                Cmd.Parameters.Add("@longdata", OracleDbType.Long).Value = strLong;

                RowAffected = Cmd.ExecuteNonQuery();

                SaveSqlLog(SQL, pDbCon); //Query Log 저장

                return rtnVal;
            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return rtnVal;
            }
            finally
            {
                if (Cmd != null)
                {
                    Cmd.Dispose();
                    Cmd = null;
                }
            }
        }

        /// <summary>
        /// Long Type 컬럼이 있는 테이블에 작업을 할 경우 사용 : SaveSqlLog 제외
        /// </summary>
        /// <param name="SQL">Query 문</param>
        /// <param name="strLong">Long 컬럼의 값</param>
        /// <param name="RowAffected">적용된 Data Row 수</param>
        /// <param name="pTran">트렌젝션</param>
        /// <returns>에러로그</returns>
        public static string ExecuteLongQueryEx(string SQL, string strLong, ref int RowAffected, PsmhDb pDbCon)
        {
            OracleCommand Cmd = null;
            string rtnVal = "";
            try
            {
                if (Cmd == null)
                {
                    Cmd = pDbCon.Con.CreateCommand();
                }

                Cmd.CommandText = SQL;
                Cmd.CommandTimeout = 60;
                if (pDbCon.Trs != null)
                {
                    Cmd.Transaction = pDbCon.Trs;
                }
                Cmd.Parameters.Add("@longdata", OracleDbType.Long).Value = strLong;

                RowAffected = Cmd.ExecuteNonQuery();

                //SaveSqlLog(SQL, pDbCon); //Query Log 저장

                return rtnVal;
            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return rtnVal;
            }
            finally
            {
                if (Cmd != null)
                {
                    Cmd.Dispose();
                    Cmd = null;
                }
            }
        }

        /// <summary>
        /// BLOB 데이타를 저장합니다. = 필드 하나만
        /// </summary>
        /// <param name="SQL">쿼리 내용 입니다.</param>
        /// <param name="strBlob">BLOB 컬럼 값</param>
        /// <param name="RowAffected">적용된 Data Row 수</param>
        /// <param name="pTran">트렌젝션</param>
        /// <returns>에러로그</returns>
        public static string ExecuteBlobQuery(string SQL, string strBlob, ref int RowAffected, PsmhDb pDbCon)
        {
            OracleCommand Cmd = null;
            string rtnVal = "";
            try
            {
                if (Cmd == null)
                {
                    Cmd = pDbCon.Con.CreateCommand();
                }

                Cmd.CommandText = SQL;
                Cmd.CommandTimeout = 60;
                if (pDbCon.Trs != null)
                {
                    Cmd.Transaction = pDbCon.Trs;
                }
                Cmd.Parameters.Add("@blobdata", OracleDbType.Blob).Value = strBlob;

                RowAffected = Cmd.ExecuteNonQuery();

                SaveSqlLog(SQL, pDbCon); //Query Log 저장

                return rtnVal;
            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return rtnVal;
            }
            finally
            {
                if (Cmd != null)
                {
                    Cmd.Dispose();
                    Cmd = null;
                }
            }
        }

        /// <summary>
        /// BLOB 데이타를 저장합니다. = 필드 하나만  SaveSqlLog 제외
        /// </summary>
        /// <param name="SQL">쿼리 내용 입니다.</param>
        /// <param name="strBlob">BLOB 컬럼 값</param>
        /// <param name="RowAffected">적용된 Data Row 수</param>
        /// <param name="pTran">트렌젝션</param>
        /// <returns>에러로그</returns>
        public static string ExecuteBlobQueryEx(string SQL, string strBlob, ref int RowAffected, PsmhDb pDbCon)
        {
            OracleCommand Cmd = null;
            string rtnVal = "";
            try
            {
                if (Cmd == null)
                {
                    Cmd = pDbCon.Con.CreateCommand();
                }

                Cmd.CommandText = SQL;
                Cmd.CommandTimeout = 60;
                if (pDbCon.Trs != null)
                {
                    Cmd.Transaction = pDbCon.Trs;
                }
                Cmd.Parameters.Add("@blobdata", OracleDbType.Blob).Value = strBlob;

                RowAffected = Cmd.ExecuteNonQuery();

                //SaveSqlLog(SQL, pDbCon); //Query Log 저장

                return rtnVal;
            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return rtnVal;
            }
            finally
            {
                if (Cmd != null)
                {
                    Cmd.Dispose();
                    Cmd = null;
                }
            }
        }

        /// <summary>LongRaw 컬럼 이 있는 테이블에 작업을 할 경우 사용</summary>
        /// <author>김홍록</author>
        /// <date>2017-11-14</date>
        /// <param name="SQL">쿼리 내용 입니다.</param>
        /// <param name="Blob">BLOB 컬럼 값</param>
        /// <param name="RowAffected">적용된 Data Row 수</param>
        /// <param name="pDbCon">트렌젝션</param>
        /// <returns></returns>
        public static string ExecuteLongRawQuery(string SQL, byte[] Blob, ref int RowAffected, PsmhDb pDbCon)
        {
            OracleCommand Cmd = null;
            string rtnVal = "";
            try
            {
                if (Cmd == null)
                {
                    Cmd = pDbCon.Con.CreateCommand();
                }

                Cmd.CommandText = SQL;
                Cmd.CommandTimeout = 60;
                if (pDbCon.Trs != null)
                {
                    Cmd.Transaction = pDbCon.Trs;
                }

                OracleParameter img = new OracleParameter();
                img.ParameterName = "IMG";
                img.OracleDbType = OracleDbType.LongRaw;
                img.Direction = ParameterDirection.Input;
                img.Value = Blob;

                Cmd.Parameters.Add(img);

                RowAffected = Cmd.ExecuteNonQuery();

                SaveSqlLog(SQL, pDbCon); //Query Log 저장

                return rtnVal;
            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return rtnVal;
            }
            finally
            {
                if (Cmd != null)
                {
                    Cmd.Dispose();
                    Cmd = null;
                }
            }
        }

        /// <summary>LongRaw 컬럼 이 있는 테이블에 작업을 할 경우 사용 : SaveSqlLog 제외 </summary>
        /// <author>김홍록</author>
        /// <date>2017-11-14</date>
        /// <param name="SQL">쿼리 내용 입니다.</param>
        /// <param name="Blob">BLOB 컬럼 값</param>
        /// <param name="RowAffected">적용된 Data Row 수</param>
        /// <param name="pDbCon">트렌젝션</param>
        /// <returns></returns>
        public static string ExecuteLongRawQueryEx(string SQL, byte[] Blob, ref int RowAffected, PsmhDb pDbCon)
        {
            OracleCommand Cmd = null;
            string rtnVal = "";
            try
            {
                if (Cmd == null)
                {
                    Cmd = pDbCon.Con.CreateCommand();
                }

                Cmd.CommandText = SQL;
                Cmd.CommandTimeout = 60;
                if (pDbCon.Trs != null)
                {
                    Cmd.Transaction = pDbCon.Trs;
                }

                OracleParameter img = new OracleParameter();
                img.ParameterName = "IMG";
                img.OracleDbType = OracleDbType.LongRaw;
                img.Direction = ParameterDirection.Input;
                img.Value = Blob;

                Cmd.Parameters.Add(img);

                RowAffected = Cmd.ExecuteNonQuery();

                //SaveSqlLog(SQL, pDbCon); //Query Log 저장

                return rtnVal;
            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return rtnVal;
            }
            finally
            {
                if (Cmd != null)
                {
                    Cmd.Dispose();
                    Cmd = null;
                }
            }
        }

        /// <summary>
        /// BLOB 데이타를 저장합니다. = 필드 여러개일경우
        /// </summary>
        /// <param name="SQL">쿼리 내용 입니다.</param>
        /// <param name="strBlob">BLOB 컬럼 값</param>
        /// <param name="RowAffected">적용된 Data Row 수</param>
        /// <param name="pTran">트렌젝션</param>
        /// <returns>에러로그</returns>
        public static string ExecuteBlobQuery(string SQL, string[] strBlob, ref int RowAffected, PsmhDb pDbCon)
        {
            OracleCommand Cmd = null;
            string rtnVal = "";
            try
            {
                if (Cmd == null)
                {
                    Cmd = pDbCon.Con.CreateCommand();
                }

                Cmd.CommandText = SQL;
                Cmd.CommandTimeout = 60;
                if (pDbCon.Trs != null)
                {
                    Cmd.Transaction = pDbCon.Trs;
                }
                for (int i = 0; i <= strBlob.Length - 1; i++)
                {
                    Cmd.Parameters.Add("@blobdata", OracleDbType.Blob).Value = clsFuncImage.StringToByte(strBlob[i]);
                }

                RowAffected = Cmd.ExecuteNonQuery();

                SaveSqlLog(SQL, pDbCon); //Query Log 저장

                return rtnVal;
            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return rtnVal;
            }
            finally
            {
                if (Cmd != null)
                {
                    Cmd.Dispose();
                    Cmd = null;
                }
            }
        }

        /// <summary>
        /// BLOB 데이타를 저장합니다. = 필드 여러개일경우: SaveSqlLog 제외
        /// </summary>
        /// <param name="SQL">쿼리 내용 입니다.</param>
        /// <param name="strBlob">BLOB 컬럼 값</param>
        /// <param name="RowAffected">적용된 Data Row 수</param>
        /// <param name="pTran">트렌젝션</param>
        /// <returns>에러로그</returns>
        public static string ExecuteBlobQueryEx(string SQL, string[] strBlob, ref int RowAffected, PsmhDb pDbCon)
        {
            OracleCommand Cmd = null;
            string rtnVal = "";
            try
            {
                if (Cmd == null)
                {
                    Cmd = pDbCon.Con.CreateCommand();
                }

                Cmd.CommandText = SQL;
                Cmd.CommandTimeout = 60;
                if (pDbCon.Trs != null)
                {
                    Cmd.Transaction = pDbCon.Trs;
                }
                for (int i = 0; i < strBlob.Length; i++)
                {
                    Cmd.Parameters.Add("@blobdata", OracleDbType.Blob).Value = clsFuncImage.StringToByte(strBlob[i]);
                }

                RowAffected = Cmd.ExecuteNonQuery();

                //SaveSqlLog(SQL, pDbCon); //Query Log 저장

                return rtnVal;
            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return rtnVal;
            }
            finally
            {
                if (Cmd != null)
                {
                    Cmd.Dispose();
                    Cmd = null;
                }
            }
        }

        /// <summary>
        /// CLOB 데이타를 저장합니다. = 필드 하나만
        /// </summary>
        /// <param name="SQL">쿼리 내용 입니다.</param>
        /// <param name="strClob">CLOB 컬럼 값</param>
        /// <param name="RowAffected">적용된 Data Row 수</param>
        /// <param name="pTran">트렌젝션</param>
        /// <returns>에러로그</returns>
        public static string ExecuteClobQuery(string SQL, string strClob, ref int RowAffected, PsmhDb pDbCon)
        {
            OracleCommand Cmd = null;
            string rtnVal = "";
            try
            {
                if (Cmd == null)
                {
                    Cmd = pDbCon.Con.CreateCommand();
                }

                Cmd.CommandText = SQL;
                Cmd.CommandTimeout = 60;
                if (pDbCon.Trs != null)
                {
                    Cmd.Transaction = pDbCon.Trs;
                }
                Cmd.Parameters.Add("@clobdata", OracleDbType.Clob).Value = strClob;

                RowAffected = Cmd.ExecuteNonQuery();

                SaveSqlLog(SQL, pDbCon); //Query Log 저장

                return rtnVal;
            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return rtnVal;
            }
            finally
            {
                if (Cmd != null)
                {
                    Cmd.Dispose();
                    Cmd = null;
                }
            }
        }

        /// <summary>
        /// CLOB 데이타를 저장합니다. = 필드 하나만: SaveSqlLog 제외
        /// </summary>
        /// <param name="SQL">쿼리 내용 입니다.</param>
        /// <param name="strClob">CLOB 컬럼 값</param>
        /// <param name="RowAffected">적용된 Data Row 수</param>
        /// <param name="pTran">트렌젝션</param>
        /// <returns>에러로그</returns>
        public static string ExecuteClobQueryEx(string SQL, string strClob, ref int RowAffected, PsmhDb pDbCon)
        {
            OracleCommand Cmd = null;
            string rtnVal = "";
            try
            {
                if (Cmd == null)
                {
                    Cmd = pDbCon.Con.CreateCommand();
                }

                Cmd.CommandText = SQL;
                Cmd.CommandTimeout = 60;
                if (pDbCon.Trs != null)
                {
                    Cmd.Transaction = pDbCon.Trs;
                }
                Cmd.Parameters.Add("@clobdata", OracleDbType.Clob).Value = strClob;

                RowAffected = Cmd.ExecuteNonQuery();

                //SaveSqlLog(SQL, pDbCon); //Query Log 저장

                return rtnVal;
            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return rtnVal;
            }
            finally
            {
                if (Cmd != null)
                {
                    Cmd.Dispose();
                    Cmd = null;
                }
            }
        }

        /// <summary>
        /// CLOB 데이타를 저장합니다. = 필드 여러개일 경우
        /// </summary>
        /// <param name="SQL">쿼리 내용 입니다.</param>
        /// <param name="strClob">CLOB 컬럼 값(여러개 컬럼)</param>
        /// <param name="RowAffected">적용된 Data Row 수</param>
        /// <param name="pTran">트렌젝션</param>
        /// <returns>에러로그</returns>
        public static string ExecuteClobQuery(string SQL, string[] strClob, ref int RowAffected, PsmhDb pDbCon)
        {
            OracleCommand Cmd = null;
            string rtnVal = "";
            try
            {
                if (Cmd == null)
                {
                    Cmd = pDbCon.Con.CreateCommand();
                }

                Cmd.CommandText = SQL;
                Cmd.CommandTimeout = 60;
                if (pDbCon.Trs != null)
                {
                    Cmd.Transaction = pDbCon.Trs;
                }
                for (int i = 0; i <= strClob.Length - 1; i++)
                {
                    Cmd.Parameters.Add("@clobdata", OracleDbType.Clob).Value = strClob[i];
                }

                RowAffected = Cmd.ExecuteNonQuery();

                SaveSqlLog(SQL, pDbCon); //Query Log 저장

                return rtnVal;
            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return rtnVal;
            }
            finally
            {
                if (Cmd != null)
                {
                    Cmd.Dispose();
                    Cmd = null;
                }
            }
        }

        /// <summary>
        /// CLOB 데이타를 저장합니다. = 필드 여러개일 경우: SaveSqlLog 제외
        /// </summary>
        /// <param name="SQL">쿼리 내용 입니다.</param>
        /// <param name="strClob">CLOB 컬럼 값(여러개 컬럼)</param>
        /// <param name="RowAffected">적용된 Data Row 수</param>
        /// <param name="pTran">트렌젝션</param>
        /// <returns>에러로그</returns>
        public static string ExecuteClobQueryEx(string SQL, string[] strClob, ref int RowAffected, PsmhDb pDbCon)
        {
            OracleCommand Cmd = null;
            string rtnVal = "";
            try
            {
                if (Cmd == null)
                {
                    Cmd = pDbCon.Con.CreateCommand();
                }

                Cmd.CommandText = SQL;
                Cmd.CommandTimeout = 60;
                if (pDbCon.Trs != null)
                {
                    Cmd.Transaction = pDbCon.Trs;
                }
                for (int i = 0; i <= strClob.Length - 1; i++)
                {
                    Cmd.Parameters.Add("@clobdata", OracleDbType.Clob).Value = strClob[i];
                }

                RowAffected = Cmd.ExecuteNonQuery();

                //SaveSqlLog(SQL, pDbCon); //Query Log 저장

                return rtnVal;
            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return rtnVal;
            }
            finally
            {
                if (Cmd != null)
                {
                    Cmd.Dispose();
                    Cmd = null;
                }
            }
        }

        /// <summary>
        /// XML 데이타를 저장합니다. = 필드 하나만
        /// </summary>
        /// <param name="SQL">쿼리 내용 입니다.</param>
        /// <param name="strXml">XML 컬럼 값</param>
        /// <param name="RowAffected">적용된 Data Row 수</param>
        /// <param name="pTran">트렌젝션</param>
        /// <returns>에러로그</returns>
        public static string ExecuteXmlQuery(string SQL, string strXml, ref int RowAffected, PsmhDb pDbCon)
        {
            OracleCommand Cmd = null;
            string rtnVal = "";
            try
            {
                if (Cmd == null)
                {
                    Cmd = pDbCon.Con.CreateCommand();
                }

                Cmd.CommandText = SQL;
                Cmd.CommandTimeout = 60;
                if (pDbCon.Trs != null)
                {
                    Cmd.Transaction = pDbCon.Trs;
                }
                Cmd.Parameters.Add("@xmldata", OracleDbType.XmlType).Value = strXml;

                RowAffected = Cmd.ExecuteNonQuery();

                SaveSqlLog(SQL, pDbCon); //Query Log 저장

                return rtnVal;
            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return rtnVal;
            }
            finally
            {
                if (Cmd != null)
                {
                    Cmd.Dispose();
                    Cmd = null;
                }
            }
        }

        /// <summary>
        /// XML 데이타를 저장합니다. = 필드 하나만 : SaveSqlLog 제외
        /// </summary>
        /// <param name="SQL">쿼리 내용 입니다.</param>
        /// <param name="strXml">XML 컬럼 값</param>
        /// <param name="RowAffected">적용된 Data Row 수</param>
        /// <param name="pTran">트렌젝션</param>
        /// <returns>에러로그</returns>
        public static string ExecuteXmlQueryEx(string SQL, string strXml, ref int RowAffected, PsmhDb pDbCon)
        {
            OracleCommand Cmd = null;
            string rtnVal = "";
            try
            {
                if (Cmd == null)
                {
                    Cmd = pDbCon.Con.CreateCommand();
                }

                Cmd.CommandText = SQL;
                Cmd.CommandTimeout = 60;
                if (pDbCon.Trs != null)
                {
                    Cmd.Transaction = pDbCon.Trs;
                }
                Cmd.Parameters.Add("@xmldata", OracleDbType.XmlType).Value = strXml;

                RowAffected = Cmd.ExecuteNonQuery();

                //SaveSqlLog(SQL, pDbCon); //Query Log 저장

                return rtnVal;
            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return rtnVal;
            }
            finally
            {
                if (Cmd != null)
                {
                    Cmd.Dispose();
                    Cmd = null;
                }
            }
        }

        /// <summary>
        /// XML 데이타를 저장합니다. = 필드 여러개일 경우
        /// </summary>
        /// <param name="SQL">쿼리 내용 입니다.</param>
        /// <param name="strXml">XML 컬럼 값(여러개 컬럼)</param>
        /// <param name="RowAffected">적용된 Data Row 수</param>
        /// <param name="pTran">트렌젝션</param>
        /// <returns>에러로그</returns>
        public static string ExecuteXmlQuery(string SQL, string[] strXml, ref int RowAffected, PsmhDb pDbCon)
        {
            OracleCommand Cmd = null;
            string rtnVal = "";
            try
            {
                if (Cmd == null)
                {
                    Cmd = pDbCon.Con.CreateCommand();
                }

                Cmd.CommandText = SQL;
                Cmd.CommandTimeout = 60;
                if (pDbCon.Trs != null)
                {
                    Cmd.Transaction = pDbCon.Trs;
                }
                for (int i = 0; i <= strXml.Length - 1; i++)
                {
                    Cmd.Parameters.Add("@clobdata", OracleDbType.XmlType).Value = strXml[i];
                }

                RowAffected = Cmd.ExecuteNonQuery();

                SaveSqlLog(SQL, pDbCon); //Query Log 저장

                return rtnVal;
            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return rtnVal;
            }
            finally
            {
                if (Cmd != null)
                {
                    Cmd.Dispose();
                    Cmd = null;
                }
            }
        }

        /// <summary>
        /// XML 데이타를 저장합니다. = 필드 여러개일 경우 : SaveSqlLog 제외
        /// </summary>
        /// <param name="SQL">쿼리 내용 입니다.</param>
        /// <param name="strXml">XML 컬럼 값(여러개 컬럼)</param>
        /// <param name="RowAffected">적용된 Data Row 수</param>
        /// <param name="pTran">트렌젝션</param>
        /// <returns>에러로그</returns>
        public static string ExecuteXmlQueryEx(string SQL, string[] strXml, ref int RowAffected, PsmhDb pDbCon)
        {
            OracleCommand Cmd = null;
            string rtnVal = "";
            try
            {
                if (Cmd == null)
                {
                    Cmd = pDbCon.Con.CreateCommand();
                }

                Cmd.CommandText = SQL;
                Cmd.CommandTimeout = 60;
                if (pDbCon.Trs != null)
                {
                    Cmd.Transaction = pDbCon.Trs;
                }
                for (int i = 0; i <= strXml.Length - 1; i++)
                {
                    Cmd.Parameters.Add("@clobdata", OracleDbType.XmlType).Value = strXml[i];
                }

                RowAffected = Cmd.ExecuteNonQuery();

                //SaveSqlLog(SQL, pDbCon); //Query Log 저장

                return rtnVal;
            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return rtnVal;
            }
            finally
            {
                if (Cmd != null)
                {
                    Cmd.Dispose();
                    Cmd = null;
                }
            }
        }

        /// <summary>
        /// SQL Log를 저장을 한다 KMC
        /// ETC_SQLLOG 테이블의 EXECODE 컬럼사이즈를 최대 varchar2(100) 까지 늘리면 좋을듯합니다.
        /// </summary>
        /// <param name="strQuery">Query문</param>
        public static void SaveSqlLog(string strQuery, PsmhDb pDbCon)
        {
            OracleCommand Cmd = null;
            OracleDataAdapter Adapter = new OracleDataAdapter();

            string rtnVal = "";
            string SQL = "";
            int RowAffected = 0;

            strQuery = strQuery.Replace("'", "`");
            strQuery = strQuery.Replace("\r", " ");

            string strSQL = "";
            string strSQL1 = "";
            string strSQL2 = "";
            string strSQL3 = "";
            int intLenTot = (int)ComFunc.GetWordByByte(strQuery);

            if (clsDB.strSource == "PSMH_DEV") return;

            #region Reflaction + Call Stack 을 사용하여 최초 함수를 호출한 폼이름을 가져옴.

            string strForm = "";
            int nSCnt = new StackTrace().FrameCount;    //호출 스택 총 카운트
            StackTrace vST = new StackTrace();
            MethodBase vMethod = null;

            for (int i = 0; i < nSCnt; i++)
            {
                vMethod = vST.GetFrame(i).GetMethod(); 
                if (vMethod.DeclaringType.FullName.ToLower().Contains("frm"))   //폼이름이 들어간 문구 추출
                {
                    strForm = vMethod.DeclaringType.FullName + "." + vMethod.Name;
                    strForm = VB.Left(strForm, 100);
                    break;
                }
            }

            #endregion

            if (intLenTot > 16000) return;

            try
            {
                //if (pDbCon == null)
                //{
                //    pDbCon = clsDB.DbCon;
                //}

                if (clsDB.DbConErr != null)
                {
                    pDbCon = clsDB.DbConErr;
                }

                //pTran = null; //
                //string GstrSysDate = "20170613";
                //long GnJobSabun = 0;
                string strTable = "";
                if (clsPublic.GstrSysDate != "")
                {
                    strTable = clsPublic.GstrSysDate.Replace("-", "");
                }
                else
                {
                    strTable = ComQuery.CurrentDateTime(pDbCon, "D");
                }

                #region //테이블 조회 조건 제외

                //DataSet ds = null;
                //SQL = "";
                //SQL = SQL + "\r\n" + "SELECT * FROM USER_TABLES          ";
                ////SQL = SQL + "\r\n" + "WHERE TABLESPACE_NAME = 'SQLLOG2'          ";
                //SQL = SQL + "\r\n" + "WHERE TABLE_NAME = 'ETC_SQLLOG_" + strTable + "' ";

                //Adapter = new OracleDataAdapter(SQL, pDbCon.Con);

                //if (pDbCon.Trs != null)
                //{
                //    Adapter.SelectCommand.Transaction = pDbCon.Trs;
                //}
                //Adapter.SelectCommand.InitialLONGFetchSize = -1;
                //Adapter.Fill(ds);

                //if (ds.Tables[0].Rows.Count == 0)
                //{
                //    SQL = "";
                //    SQL = SQL + "\r\n" + "CREATE TABLE " + ComNum.DB_PMPA + "ETC_SQLLOG_" + strTable;
                //    SQL = SQL + "\r\n" + "(                                                     ";
                //    SQL = SQL + "\r\n" + "  ACTDATE   DATE,                                     ";
                //    SQL = SQL + "\r\n" + "  EXECODE   CHAR(20 BYTE),                            ";
                //    SQL = SQL + "\r\n" + "  JOBSABUN  CHAR(6 BYTE),                             ";
                //    SQL = SQL + "\r\n" + "  IP        VARCHAR2(15 BYTE),                        ";
                //    SQL = SQL + "\r\n" + "  GUBUN     VARCHAR2(2000 BYTE),                      ";
                //    SQL = SQL + "\r\n" + "  SQL       VARCHAR2(4000 BYTE),                      ";
                //    SQL = SQL + "\r\n" + "  SQL1       VARCHAR2(4000 BYTE),                      ";
                //    SQL = SQL + "\r\n" + "  SQL2       VARCHAR2(4000 BYTE),                      ";
                //    SQL = SQL + "\r\n" + "  SQL3       VARCHAR2(4000 BYTE),                      ";
                //    SQL = SQL + "\r\n" + "  ENTDATE   DATE,                                     ";
                //    SQL = SQL + "\r\n" + "  SENDLOG   CHAR(1 BYTE)                              ";
                //    SQL = SQL + "\r\n" + ")                                                     ";
                //    SQL = SQL + "\r\n" + "TABLESPACE SQLLOG2                                    ";
                //    SQL = SQL + "\r\n" + "PCTUSED    40                                         ";
                //    SQL = SQL + "\r\n" + "PCTFREE    10                                         ";
                //    SQL = SQL + "\r\n" + "INITRANS   1                                          ";
                //    SQL = SQL + "\r\n" + "MAXTRANS   255                                        ";
                //    SQL = SQL + "\r\n" + "STORAGE    (                                          ";
                //    SQL = SQL + "\r\n" + "            INITIAL          64K                    ";
                //    SQL = SQL + "\r\n" + "            MINEXTENTS       1                        ";
                //    SQL = SQL + "\r\n" + "            MAXEXTENTS       UNLIMITED                ";
                //    SQL = SQL + "\r\n" + "            PCTINCREASE      0                        ";
                //    SQL = SQL + "\r\n" + "            FREELISTS        10                       ";
                //    SQL = SQL + "\r\n" + "            FREELIST GROUPS  4                        ";
                //    SQL = SQL + "\r\n" + "            BUFFER_POOL      DEFAULT                  ";
                //    SQL = SQL + "\r\n" + "           )                                          ";
                //    SQL = SQL + "\r\n" + "LOGGING                                               ";
                //    SQL = SQL + "\r\n" + "NOCOMPRESS                                            ";
                //    SQL = SQL + "\r\n" + "NOCACHE                                               ";
                //    SQL = SQL + "\r\n" + "NOPARALLEL                                            ";
                //    SQL = SQL + "\r\n" + "NOMONITORING                                          ";
                //    Cmd = pDbCon.Con.CreateCommand();
                //    Cmd.CommandText = SQL;
                //    Cmd.CommandTimeout = 60;

                //    if (pDbCon.Trs != null)
                //    {
                //        Cmd.Transaction = pDbCon.Trs;
                //    }

                //    RowAffected = Cmd.ExecuteNonQuery();
                //}
                //Adapter.Dispose();
                //Adapter = null;
                //ds.Dispose();
                //ds = null;

                //Cmd = null;
                //RowAffected = 0;

                #endregion //테이블 조회 조건 제외

                if (intLenTot <= 4000)
                {
                    strSQL = strQuery;
                }
                else if (intLenTot > 4000 && intLenTot <= 8000)
                {
                    strSQL = ComFunc.GetMidStr(strQuery, 0, 4000);
                    strSQL1 = ComFunc.GetMidStr(strQuery, 4000, intLenTot - 4000);
                }
                else if (intLenTot > 8000 && intLenTot <= 12000)
                {
                    strSQL = ComFunc.GetMidStr(strQuery, 0, 4000);
                    strSQL1 = ComFunc.GetMidStr(strQuery, 4000, 4000);
                    strSQL2 = ComFunc.GetMidStr(strQuery, 8000, intLenTot - 8000);
                }
                else if (intLenTot > 12000 && intLenTot <= 16000)
                {
                    strSQL = ComFunc.GetMidStr(strQuery, 0, 4000);
                    strSQL1 = ComFunc.GetMidStr(strQuery, 4000, 4000);
                    strSQL2 = ComFunc.GetMidStr(strQuery, 8000, 4000);
                    strSQL3 = ComFunc.GetMidStr(strQuery, 12000, intLenTot - 12000);
                }

                SQL = "";
                SQL = SQL + "\r\n" + " INSERT INTO " + ComNum.DB_PMPA + "ETC_SQLLOG_" + strTable;
                SQL = SQL + "\r\n" + " ( ACTDATE, EXECODE,  JOBSABUN, IP, GUBUN, SQL, SQL1, SQL2, SQL3, ENTDATE) VALUES ( ";
                SQL = SQL + "\r\n" + " TRUNC(SYSDATE), '" + strForm + "' , '" + clsPublic.GnJobSabun + "',  '" + clsCompuInfo.gstrCOMIP + "', ";
                SQL = SQL + "\r\n" + " 'PSMH', '" + strSQL + "',  '" + strSQL1 + "',  '" + strSQL2 + "',  '" + strSQL3 + "' , SYSDATE) ";

                Cmd = pDbCon.Con.CreateCommand();
                Cmd.CommandText = SQL;
                Cmd.CommandTimeout = 60;

                if (pDbCon.Trs != null)
                {
                    Cmd.Transaction = pDbCon.Trs;
                }

                RowAffected = Cmd.ExecuteNonQuery();

                return;
            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return;
            }
            finally
            {
                if (Cmd != null)
                {
                    Cmd.Dispose();
                    Cmd = null;
                }
            }
        }

        /// <summary>
        /// SQL Log를 저장을 한다
        /// </summary>
        /// <param name="strQuery">Query문</param>
        public static void SaveSqlLog_OLD(string strQuery, PsmhDb pDbCon)
        {
            OracleCommand Cmd = null;
            OracleDataAdapter Adapter = new OracleDataAdapter();

            string rtnVal = "";
            string SQL = "";
            int RowAffected = 0;

            strQuery = strQuery.Replace("'", "`");
            strQuery = strQuery.Replace("\r", " ");

            string strSQL = "";
            string strSQL1 = "";
            string strSQL2 = "";
            string strSQL3 = "";
            int intLenTot = (int)ComFunc.GetWordByByte(strQuery);

            if (clsDB.strSource == "PSMH_DEV") return;

            if (intLenTot > 16000) return;

            try
            {
                //if (pDbCon == null)
                //{
                //    pDbCon = clsDB.DbCon;
                //}

                if (clsDB.DbConErr != null)
                {
                    pDbCon = clsDB.DbConErr;
                }

                //pTran = null; //
                //string GstrSysDate = "20170613";
                //long GnJobSabun = 0;
                string strTable = "";
                if (clsPublic.GstrSysDate != "")
                {
                    strTable = clsPublic.GstrSysDate.Replace("-", "");
                }
                else
                {
                    strTable = ComQuery.CurrentDateTime(pDbCon, "D");
                }

                #region //테이블 조회 조건 제외

                //DataSet ds = null;
                //SQL = "";
                //SQL = SQL + "\r\n" + "SELECT * FROM USER_TABLES          ";
                ////SQL = SQL + "\r\n" + "WHERE TABLESPACE_NAME = 'SQLLOG2'          ";
                //SQL = SQL + "\r\n" + "WHERE TABLE_NAME = 'ETC_SQLLOG_" + strTable + "' ";

                //Adapter = new OracleDataAdapter(SQL, pDbCon.Con);

                //if (pDbCon.Trs != null)
                //{
                //    Adapter.SelectCommand.Transaction = pDbCon.Trs;
                //}
                //Adapter.SelectCommand.InitialLONGFetchSize = -1;
                //Adapter.Fill(ds);

                //if (ds.Tables[0].Rows.Count == 0)
                //{
                //    SQL = "";
                //    SQL = SQL + "\r\n" + "CREATE TABLE " + ComNum.DB_PMPA + "ETC_SQLLOG_" + strTable;
                //    SQL = SQL + "\r\n" + "(                                                     ";
                //    SQL = SQL + "\r\n" + "  ACTDATE   DATE,                                     ";
                //    SQL = SQL + "\r\n" + "  EXECODE   CHAR(20 BYTE),                            ";
                //    SQL = SQL + "\r\n" + "  JOBSABUN  CHAR(6 BYTE),                             ";
                //    SQL = SQL + "\r\n" + "  IP        VARCHAR2(15 BYTE),                        ";
                //    SQL = SQL + "\r\n" + "  GUBUN     VARCHAR2(2000 BYTE),                      ";
                //    SQL = SQL + "\r\n" + "  SQL       VARCHAR2(4000 BYTE),                      ";
                //    SQL = SQL + "\r\n" + "  SQL1       VARCHAR2(4000 BYTE),                      ";
                //    SQL = SQL + "\r\n" + "  SQL2       VARCHAR2(4000 BYTE),                      ";
                //    SQL = SQL + "\r\n" + "  SQL3       VARCHAR2(4000 BYTE),                      ";
                //    SQL = SQL + "\r\n" + "  ENTDATE   DATE,                                     ";
                //    SQL = SQL + "\r\n" + "  SENDLOG   CHAR(1 BYTE)                              ";
                //    SQL = SQL + "\r\n" + ")                                                     ";
                //    SQL = SQL + "\r\n" + "TABLESPACE SQLLOG2                                    ";
                //    SQL = SQL + "\r\n" + "PCTUSED    40                                         ";
                //    SQL = SQL + "\r\n" + "PCTFREE    10                                         ";
                //    SQL = SQL + "\r\n" + "INITRANS   1                                          ";
                //    SQL = SQL + "\r\n" + "MAXTRANS   255                                        ";
                //    SQL = SQL + "\r\n" + "STORAGE    (                                          ";
                //    SQL = SQL + "\r\n" + "            INITIAL          64K                    ";
                //    SQL = SQL + "\r\n" + "            MINEXTENTS       1                        ";
                //    SQL = SQL + "\r\n" + "            MAXEXTENTS       UNLIMITED                ";
                //    SQL = SQL + "\r\n" + "            PCTINCREASE      0                        ";
                //    SQL = SQL + "\r\n" + "            FREELISTS        10                       ";
                //    SQL = SQL + "\r\n" + "            FREELIST GROUPS  4                        ";
                //    SQL = SQL + "\r\n" + "            BUFFER_POOL      DEFAULT                  ";
                //    SQL = SQL + "\r\n" + "           )                                          ";
                //    SQL = SQL + "\r\n" + "LOGGING                                               ";
                //    SQL = SQL + "\r\n" + "NOCOMPRESS                                            ";
                //    SQL = SQL + "\r\n" + "NOCACHE                                               ";
                //    SQL = SQL + "\r\n" + "NOPARALLEL                                            ";
                //    SQL = SQL + "\r\n" + "NOMONITORING                                          ";
                //    Cmd = pDbCon.Con.CreateCommand();
                //    Cmd.CommandText = SQL;
                //    Cmd.CommandTimeout = 60;

                //    if (pDbCon.Trs != null)
                //    {
                //        Cmd.Transaction = pDbCon.Trs;
                //    }

                //    RowAffected = Cmd.ExecuteNonQuery();
                //}
                //Adapter.Dispose();
                //Adapter = null;
                //ds.Dispose();
                //ds = null;

                //Cmd = null;
                //RowAffected = 0;

                #endregion //테이블 조회 조건 제외

                if (intLenTot <= 4000)
                {
                    strSQL = strQuery;
                }
                else if (intLenTot > 4000 && intLenTot <= 8000)
                {
                    strSQL = ComFunc.GetMidStr(strQuery, 0, 4000);
                    strSQL1 = ComFunc.GetMidStr(strQuery, 4000, intLenTot - 4000);
                }
                else if (intLenTot > 8000 && intLenTot <= 12000)
                {
                    strSQL = ComFunc.GetMidStr(strQuery, 0, 4000);
                    strSQL1 = ComFunc.GetMidStr(strQuery, 4000, 4000);
                    strSQL2 = ComFunc.GetMidStr(strQuery, 8000, intLenTot - 8000);
                }
                else if (intLenTot > 12000 && intLenTot <= 16000)
                {
                    strSQL = ComFunc.GetMidStr(strQuery, 0, 4000);
                    strSQL1 = ComFunc.GetMidStr(strQuery, 4000, 4000);
                    strSQL2 = ComFunc.GetMidStr(strQuery, 8000, 4000);
                    strSQL3 = ComFunc.GetMidStr(strQuery, 12000, intLenTot - 12000);
                }

                SQL = "";
                SQL = SQL + "\r\n" + " INSERT INTO " + ComNum.DB_PMPA + "ETC_SQLLOG_" + strTable;
                SQL = SQL + "\r\n" + " ( ACTDATE, EXECODE,  JOBSABUN, IP, GUBUN, SQL, SQL1, SQL2, SQL3, ENTDATE) VALUES ( ";
                SQL = SQL + "\r\n" + " TRUNC(SYSDATE), 'PSMH' , '" + clsPublic.GnJobSabun + "',  '" + clsCompuInfo.gstrCOMIP + "', ";
                SQL = SQL + "\r\n" + " 'PSMH', '" + strSQL + "',  '" + strSQL1 + "',  '" + strSQL2 + "',  '" + strSQL3 + "' , SYSDATE) ";

                Cmd = pDbCon.Con.CreateCommand();
                Cmd.CommandText = SQL;
                Cmd.CommandTimeout = 60;

                if (pDbCon.Trs != null)
                {
                    Cmd.Transaction = pDbCon.Trs;
                }

                RowAffected = Cmd.ExecuteNonQuery();

                return;
            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return;
            }
            finally
            {
                if (Cmd != null)
                {
                    Cmd.Dispose();
                    Cmd = null;
                }
            }
        }

        /// <summary>
        /// 에러 로그를 저장을 한다.
        /// </summary>
        /// <param name="strError">에러</param>
        /// <param name="strQuery">Query 문</param>
        /// <param name="pTran"></param>
        /// <remarks>에러 로그는 트랙젝션 처리 하지 않는다.</remarks>
        public static void SaveSqlErrLog(string strError, string strQuery, PsmhDb pDbCon)
        {
            OracleCommand Cmd = null;
            string rtnVal = "";
            string SQL = "";
            int RowAffected = 0;

            strQuery = strQuery.Replace("'", "`");
            strQuery = strQuery.Replace("\r", " ");

            string strSQL = "";
            string strSQL1 = "";
            string strSQL2 = "";
            string strSQL3 = "";
            int intLenTot = (int)ComFunc.GetWordByByte(strQuery);

            if (intLenTot > 16000) return;

            //pTran = null; //
            //long GnJobSabun = 0;

            try
            {
                //에러로그는 별도의 접속을 하도록 변경함
                //if (pDbCon == null)
                //{
                //    pDbCon = clsDB.DbCon;
                //}
                if (clsDB.DbConErr != null)
                {
                    pDbCon = clsDB.DbConErr;
                }

                if (intLenTot <= 4000)
                {
                    strSQL = strQuery;
                }
                else if (intLenTot > 4000 && intLenTot <= 8000)
                {
                    strSQL = ComFunc.GetMidStr(strQuery, 0, 4000);
                    strSQL1 = ComFunc.GetMidStr(strQuery, 4000, intLenTot - 4000);
                }
                else if (intLenTot > 8000 && intLenTot <= 12000)
                {
                    strSQL = ComFunc.GetMidStr(strQuery, 0, 4000);
                    strSQL1 = ComFunc.GetMidStr(strQuery, 4000, 4000);
                    strSQL2 = ComFunc.GetMidStr(strQuery, 8000, intLenTot - 8000);
                }
                else if (intLenTot > 12000 && intLenTot <= 16000)
                {
                    strSQL = ComFunc.GetMidStr(strQuery, 0, 4000);
                    strSQL1 = ComFunc.GetMidStr(strQuery, 4000, 4000);
                    strSQL2 = ComFunc.GetMidStr(strQuery, 8000, 4000);
                    strSQL3 = ComFunc.GetMidStr(strQuery, 12000, intLenTot - 12000);
                }

                strError = strError.Replace("'", "`");

                SQL = "";
                SQL = SQL + "\r\n" + " INSERT INTO " + ComNum.DB_PMPA + "ETC_SQLERROR ";
                SQL = SQL + "\r\n" + " ( ACTDATE, EXECODE,  JOBSABUN, ERR_NUMBER, ERR_SOURCE, ERR_DESCRIPTION, SQL, SQL1, SQL2, SQL3, ENTDATE) VALUES ( ";
                SQL = SQL + "\r\n" + " TRUNC(SYSDATE), 'PSMH' , '" + clsPublic.GnJobSabun + "',  '0', ";
                SQL = SQL + "\r\n" + "  '', '" + strError + "',  '" + strSQL + "',  '" + strSQL1 + "',  '" + strSQL2 + "',  '" + strSQL3 + "', SYSDATE) ";

                Cmd = pDbCon.Con.CreateCommand();
                Cmd.CommandText = SQL;
                Cmd.CommandTimeout = 60;

                if (pDbCon.Trs != null)
                {
                    Cmd.Transaction = pDbCon.Trs;
                }

                RowAffected = Cmd.ExecuteNonQuery();

                return;
            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return;
            }
            finally
            {
                if (Cmd != null)
                {
                    Cmd.Dispose();
                    Cmd = null;
                }
            }
        }

        /// <summary>
        /// DataSet을 Spread에 DataSource로 매핑
        /// </summary>
        /// <param name="DS"></param>
        /// <param name="argSprName"></param>
        public static void DataSetToSpd(DataSet DS, FpSpread argSprName)
        {
            argSprName.Sheets[0].RowCount = 1;

            if (DS.Tables[0].Rows.Count > 0)
            {
                argSprName.ActiveSheet.DataAutoSizeColumns = false;
                argSprName.DataSource = DS;
                argSprName.ShowRow(0, argSprName.Sheets[0].ActiveRowIndex, VerticalPosition.Top);
            }
        }

        /// <summary>
        /// DataSet을 Spread에 한줄씩 뿌린다
        /// </summary>
        /// <param name="DS">DataSet</param>
        /// <param name="argSpreadName">Spread Name</param>
        /// <param name="RowCount">Data Count</param>
        /// <param name="StartRow">시작 Row 위치</param>
        public static void DataSetToSpdRow(DataSet DS, FpSpread argSpdName, long RowCount, int StartRow)
        {
            if (RowCount > 0)
            {
                if (StartRow == 0)
                {
                    argSpdName.ActiveSheet.RowCount = 0;
                    argSpdName.ActiveSheet.ClearControls();

                    argSpdName.ActiveSheet.RowCount = (int)RowCount;

                    for (int i = 0; i < (int)argSpdName.ActiveSheet.RowCount; i++)
                    {
                        for (int j = 0; j < (int)argSpdName.ActiveSheet.ColumnCount; j++)
                        {
                            argSpdName.ActiveSheet.Cells[i, j].Text = DS.Tables[0].Rows[i][j].ToString();
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < RowCount; i++)
                    {
                        if (argSpdName.ActiveSheet.RowCount - 1 < i + StartRow)
                        {
                            argSpdName.ActiveSheet.RowCount = i + StartRow + 1;
                        }

                        for (int j = 0; j < (int)argSpdName.ActiveSheet.ColumnCount; j++)
                        {
                            argSpdName.ActiveSheet.Cells[i + StartRow, j].Text = DS.Tables[0].Rows[i][j].ToString();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 스프레드 Display DataTable 방식 /// 
        /// </summary>
        /// <param name="DT"></param>
        /// <param name="argSpreadName"></param>
        /// <param name="RowCount"></param>
        /// <param name="StartRow"></param>
        public static void DataTableToSpdRow(DataTable DT, FpSpread argSpdName, int StartRow, bool isTrim = true)
        {
            if (DT.Rows.Count > 0)
            {
                if (StartRow == 0)
                {
                    argSpdName.ActiveSheet.RowCount = DT.Rows.Count;

                    for (int i = 0; i < argSpdName.ActiveSheet.RowCount; i++)
                    {
                        for (int j = 0; j < argSpdName.ActiveSheet.ColumnCount; j++)
                        {
                            if (j == DT.Columns.Count && DT.Columns.Count < argSpdName.ActiveSheet.ColumnCount)
                            {
                                break;
                            }
                            if (isTrim == true)
                            {
                                argSpdName.ActiveSheet.Cells[i, j].Text = DT.Rows[i][j].ToString().Trim();
                            }
                            else
                            {
                                argSpdName.ActiveSheet.Cells[i, j].Text = DT.Rows[i][j].ToString();
                            }

                        }
                    }
                }
                else
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        if (argSpdName.ActiveSheet.RowCount - 1 < i + StartRow)
                        {
                            argSpdName.ActiveSheet.RowCount = i + StartRow + 1;
                        }

                        for (int j = 0; j < argSpdName.Sheets[0].ColumnCount; j++)
                        {
                            if (j == DT.Columns.Count && DT.Columns.Count < argSpdName.ActiveSheet.ColumnCount)
                            {
                                break;
                            }
                            if (isTrim == true)
                            {
                                argSpdName.ActiveSheet.Cells[i + StartRow, j].Text = DT.Rows[i][j].ToString().Trim();
                            }
                            else
                            {
                                argSpdName.ActiveSheet.Cells[i + StartRow, j].Text = DT.Rows[i][j].ToString();
                            }

                        }
                    }
                }
            }
        }


        /// <summary>
        /// BLOB 데이터 저장
        /// </summary>
        /// <param name="SQL">쿼리 내용 입니다.</param>
        /// <param name="strXml">XML 컬럼 값(여러개 컬럼)</param>
        /// <param name="RowAffected">적용된 Data Row 수</param>
        /// <param name="pTran">트렌젝션</param>
        /// <returns>에러로그</returns>
        public static string ExecuteBlob(string SQL, Dictionary<string, object> param, ref int RowAffected, PsmhDb pDbCon)
        {
            OracleCommand Cmd = null;
            string rtnVal = "";
            try
            {
                if (Cmd == null)
                {
                    Cmd = pDbCon.Con.CreateCommand();
                }

                Cmd.CommandText = SQL;
                Cmd.CommandTimeout = 60;
                if (pDbCon.Trs != null)
                {
                    Cmd.Transaction = pDbCon.Trs;
                }

                foreach (KeyValuePair<string, object> item in param)
                {
                    OracleDbType dbType = OracleDbType.Varchar2;

                    if (item.Value != null)
                    {
                        if (item.Value.GetType() == typeof(Byte[]))
                        {
                            dbType = OracleDbType.Blob;
                        }
                        else if (item.Value.GetType() == typeof(Double))
                        {
                            dbType = OracleDbType.Double;
                        }
                        else if (item.Value.GetType() == typeof(Int16))
                        {
                            dbType = OracleDbType.Int16;
                        }
                        else if (item.Value.GetType() == typeof(Int32))
                        {
                            dbType = OracleDbType.Int32;
                        }
                        else if (item.Value.GetType() == typeof(Int64))
                        {
                            dbType = OracleDbType.Int64;
                        }
                    }

                    Cmd.Parameters.Add(item.Key, dbType, item.Value, ParameterDirection.Input);
                }

                RowAffected = Cmd.ExecuteNonQuery();

                //SaveSqlLog(SQL, pDbCon); //Query Log 저장

                return rtnVal;
            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return rtnVal;
            }
            finally
            {
                if (Cmd != null)
                {
                    Cmd.Dispose();
                    Cmd = null;
                }
            }
        }





        #region //EMR
        /// <summary>
        /// AEMRCHARTROW INSERT
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strSql"></param>
        /// <param name="dblEmrNoNew"></param>
        /// <param name="strITEMCD"></param>
        /// <param name="strITEMNO"></param>
        /// <param name="strITEMINDEX"></param>
        /// <param name="strITEMTYPE"></param>
        /// <param name="strITEMVALUE"></param>
        /// <param name="strDSPSEQ"></param>
        /// <param name="strITEMVALUE1"></param>
        /// <returns></returns>
        public static string ExecuteChartRow(PsmhDb pDbCon, string strSql, double dblEmrNoNew, double dblEmrHisNo,
            string[] strITEMCD, string[] strITEMNO, string[] strITEMINDEX, string[] strITEMTYPE, string[] strITEMVALUE,
            string[] strDSPSEQ, string[] strITEMVALUE1)
        {
            string rtnVal = "";
            OracleCommand Cmd = null;

            try
            {
                if (Cmd == null)
                {
                    Cmd = pDbCon.Con.CreateCommand();
                }

                Cmd.CommandText = strSql;
                Cmd.CommandTimeout = 60;
                if (pDbCon.Trs != null)
                {
                    Cmd.Transaction = pDbCon.Trs;
                }

                Cmd.Parameters.Add(":ITEMCD", OracleDbType.Varchar2, 15);
                Cmd.Parameters.Add(":ITEMNO", OracleDbType.Varchar2, 11);
                Cmd.Parameters.Add(":ITEMINDEX", OracleDbType.Int32);
                Cmd.Parameters.Add(":ITEMTYPE", OracleDbType.Varchar2, 10);
                Cmd.Parameters.Add(":ITEMVALUE", OracleDbType.Varchar2, 4000);
                Cmd.Parameters.Add(":DSPSEQ", OracleDbType.Int32);
                Cmd.Parameters.Add(":ITEMVALUE1", OracleDbType.Varchar2, 4000);
                Cmd.Prepare();

                for (int i = 0; i < strITEMCD.Length; i++)
                {
                    Cmd.Parameters[":ITEMCD"].Value = strITEMCD[i];
                    Cmd.Parameters[":ITEMNO"].Value = strITEMNO[i];
                    Cmd.Parameters[":ITEMINDEX"].Value = strITEMINDEX[i];
                    Cmd.Parameters[":ITEMTYPE"].Value = strITEMTYPE[i];
                    Cmd.Parameters[":ITEMVALUE"].Value = strITEMVALUE[i];
                    Cmd.Parameters[":DSPSEQ"].Value = strDSPSEQ[i];
                    Cmd.Parameters[":ITEMVALUE1"].Value = strITEMVALUE1[i];
                    Cmd.ExecuteNonQuery();
                }

                Cmd.Dispose();
                Cmd = null;
                return rtnVal;
            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return rtnVal;
            }
            finally
            {
                if (Cmd != null)
                {
                    Cmd.Dispose();
                    Cmd = null;
                }
            }
        }

        /// <summary>
        /// AEMRCHARTROW INSERT
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strSql"></param>
        /// <param name="dblEmrNoNew"></param>
        /// <param name="strITEMCD"></param>
        /// <param name="strITEMNO"></param>
        /// <param name="strITEMINDEX"></param>
        /// <param name="strITEMTYPE"></param>
        /// <param name="strITEMVALUE"></param>
        /// <param name="strDSPSEQ"></param>
        /// <param name="strITEMVALUE1"></param>
        /// <param name="strITEMVALUE2"></param>
        /// <returns></returns>
        public static string ExecuteChartRow(PsmhDb pDbCon, string strSql, double dblEmrNoNew, double dblEmrHisNo,
            string[] strITEMCD, string[] strITEMNO, string[] strITEMINDEX, string[] strITEMTYPE, string[] strITEMVALUE,
            string[] strDSPSEQ, string[] strITEMVALUE1, string[] strITEMVALUE2)
        {
            string rtnVal = "";
            OracleCommand Cmd = null;

            try
            {
                if (Cmd == null)
                {
                    Cmd = pDbCon.Con.CreateCommand();
                }

                Cmd.CommandText = strSql;
                Cmd.CommandTimeout = 60;
                if (pDbCon.Trs != null)
                {
                    Cmd.Transaction = pDbCon.Trs;
                }

                Cmd.Parameters.Add(":ITEMCD", OracleDbType.Varchar2, 15);
                Cmd.Parameters.Add(":ITEMNO", OracleDbType.Varchar2, 11);
                Cmd.Parameters.Add(":ITEMINDEX", OracleDbType.Int32);
                Cmd.Parameters.Add(":ITEMTYPE", OracleDbType.Varchar2, 10);
                Cmd.Parameters.Add(":ITEMVALUE", OracleDbType.Varchar2, 4000);
                Cmd.Parameters.Add(":DSPSEQ", OracleDbType.Int32);
                Cmd.Parameters.Add(":ITEMVALUE1", OracleDbType.Varchar2, 4000);
                Cmd.Parameters.Add(":ITEMVALUE2", OracleDbType.Varchar2, 4000);
                Cmd.Prepare();

                for (int i = 0; i < strITEMCD.Length; i++)
                {
                    Cmd.Parameters[":ITEMCD"].Value = strITEMCD[i];
                    Cmd.Parameters[":ITEMNO"].Value = strITEMNO[i];
                    Cmd.Parameters[":ITEMINDEX"].Value = strITEMINDEX[i];
                    Cmd.Parameters[":ITEMTYPE"].Value = strITEMTYPE[i];
                    Cmd.Parameters[":ITEMVALUE"].Value = strITEMVALUE[i];
                    Cmd.Parameters[":DSPSEQ"].Value = strDSPSEQ[i];
                    Cmd.Parameters[":ITEMVALUE1"].Value = strITEMVALUE1[i];
                    Cmd.Parameters[":ITEMVALUE2"].Value = strITEMVALUE2[i];
                    Cmd.ExecuteNonQuery();
                }

                Cmd.Dispose();
                Cmd = null;
                return rtnVal;
            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return rtnVal;
            }
            finally
            {
                if (Cmd != null)
                {
                    Cmd.Dispose();
                    Cmd = null;
                }
            }
        }

        /// <summary>
        /// AEMRCHARTROW INSERT (작성자, 작성일자, 작성시간 추가)
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strSql"></param>
        /// <param name="dblEmrNoNew"></param>
        /// <param name="dblEmrHisNo"></param>
        /// <param name="strITEMCD"></param>
        /// <param name="strITEMNO"></param>
        /// <param name="strITEMINDEX"></param>
        /// <param name="strITEMTYPE"></param>
        /// <param name="strITEMVALUE"></param>
        /// <param name="strDSPSEQ"></param>
        /// <param name="strITEMVALUE1"></param>
        /// <param name="strINPUSEID"></param>
        /// <param name="strINPDATE"></param>
        /// <param name="strINPTIME"></param>
        /// <returns></returns>
        public static string ExecuteChartRowEx(PsmhDb pDbCon, string strSql, double dblEmrNoNew, double dblEmrHisNo,
            string[] strITEMCD, string[] strITEMNO, string[] strITEMINDEX, string[] strITEMTYPE, string[] strITEMVALUE,
            string[] strDSPSEQ, string[] strITEMVALUE1, string[] strINPUSEID, string[] strINPDATE, string[] strINPTIME)
        {
            string rtnVal = "";
            OracleCommand Cmd = null;

            try
            {
                if (Cmd == null)
                {
                    Cmd = pDbCon.Con.CreateCommand();
                }

                Cmd.CommandText = strSql;
                Cmd.CommandTimeout = 60;
                if (pDbCon.Trs != null)
                {
                    Cmd.Transaction = pDbCon.Trs;
                }

                Cmd.Parameters.Add(":ITEMCD", OracleDbType.Varchar2, 15);
                Cmd.Parameters.Add(":ITEMNO", OracleDbType.Varchar2, 11);
                Cmd.Parameters.Add(":ITEMINDEX", OracleDbType.Int32);
                Cmd.Parameters.Add(":ITEMTYPE", OracleDbType.Varchar2, 10);
                Cmd.Parameters.Add(":ITEMVALUE", OracleDbType.Varchar2, 4000);
                Cmd.Parameters.Add(":DSPSEQ", OracleDbType.Int32);
                Cmd.Parameters.Add(":ITEMVALUE1", OracleDbType.Varchar2, 4000);
                Cmd.Parameters.Add(":INPUSEID", OracleDbType.Varchar2, 8);
                Cmd.Parameters.Add(":INPDATE", OracleDbType.Varchar2, 8);
                Cmd.Parameters.Add(":INPTIME", OracleDbType.Varchar2, 6);
                Cmd.Prepare();

                for (int i = 0; i < strITEMCD.Length; i++)
                {
                    Cmd.Parameters[":ITEMCD"].Value = strITEMCD[i];
                    Cmd.Parameters[":ITEMNO"].Value = strITEMNO[i];
                    Cmd.Parameters[":ITEMINDEX"].Value = strITEMINDEX[i];
                    Cmd.Parameters[":ITEMTYPE"].Value = strITEMTYPE[i];
                    Cmd.Parameters[":ITEMVALUE"].Value = strITEMVALUE[i];
                    Cmd.Parameters[":DSPSEQ"].Value = strDSPSEQ[i];
                    Cmd.Parameters[":ITEMVALUE1"].Value = strITEMVALUE1[i];
                    Cmd.Parameters[":INPUSEID"].Value = strINPUSEID[i];
                    Cmd.Parameters[":INPDATE"].Value = strINPDATE[i];
                    Cmd.Parameters[":INPTIME"].Value = strINPTIME[i];
                    Cmd.ExecuteNonQuery();
                }

                Cmd.Dispose();
                Cmd = null;
                return rtnVal;
            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return rtnVal;
            }
            finally
            {
                if (Cmd != null)
                {
                    Cmd.Dispose();
                    Cmd = null;
                }
            }
        }

        #endregion //EMR




        #region MVC

        /// <summary>
        /// Single Row Select 쿼리를 실행합니다
        /// 2. Parameters 가 있으면 PrepareStatement 쿼리가 실행됩니다.
        /// 3. Single Row (DTO) 를 반홥니다
        /// </summary>
        /// <created>Mentorsoft : donghoonkim 2018-12-26,23:46</created>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// <exception cref="MTSDatabaseException">ExecuteReader 오류일때</exception>
        public static DTO ExecuteReaderSingle<DTO>(MParameter mParameter, PsmhDb pDbCon)
        {
            using (OracleCommand command = new OracleCommand(mParameter.SQL, pDbCon.Con))
            {
                command.CommandType = mParameter.commandType;
                command.BindByName = true;
                command.InitialLONGFetchSize = -1;
                //if (oracleTransaction != null)
                //{
                //    command.Transaction = oracleTransaction;
                //}
                if (pDbCon.Trs != null)
                {
                    command.Transaction = pDbCon.Trs;
                }
                foreach (OracleParameter parameter in mParameter.Parameters)
                {
                    command.Parameters.Add(parameter);
                }

                using (OracleDataReader oracleDataReader = command.ExecuteReader())
                {
                    command.Parameters.Clear(); // 반드시 clear 해야함.

                    while (oracleDataReader.Read())
                    {
                        return ConvertDto<DTO>(oracleDataReader);
                    }
                }

                return default(DTO);
            }
        }



        /// <summary>
        /// DataReader 쿼리 수행
        /// </summary>
        /// <created>Mentorsoft : donghoonkim 2018-12-26,23:46</created>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="oracleConnection"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static List<DTO> ExecuteReader<DTO>(MParameter mParameter, PsmhDb pDbCon)
        {
            List<DTO> rows = new List<DTO>();

            using (OracleCommand command = new OracleCommand(mParameter.SQL, pDbCon.Con))
            {
                command.CommandType = mParameter.commandType;
                command.BindByName = true;
                command.InitialLONGFetchSize = -1;
                //if (oracleTransaction != null)
                //{
                //    command.Transaction = oracleTransaction;
                //}
                if (pDbCon.Trs != null)
                {
                    command.Transaction = pDbCon.Trs;
                }
                foreach (OracleParameter parameter in mParameter.Parameters)
                {
                    command.Parameters.Add(parameter);
                }

                using (OracleDataReader oracleDataReader = command.ExecuteReader())
                {
                    while (oracleDataReader.Read())
                    {
                        rows.Add(ConvertDto<DTO>(oracleDataReader));
                    }

                    command.Parameters.Clear(); // 반드시 clear 해야함.
                }
            }

            return rows;
        }

        /// <summary>
        /// DataReader 쿼리 수행
        /// </summary>
        /// <created>Mentorsoft : donghoonkim 2018-12-26,23:46</created>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="oracleConnection"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// 
        public static BindingList<DTO> BindingReader<DTO>(MParameter mParameter, PsmhDb pDbCon)
        {
            BindingList<DTO> collection = new BindingList<DTO>();

            using (OracleCommand command = new OracleCommand(mParameter.SQL, pDbCon.Con))
            {
                command.CommandType = mParameter.commandType;
                command.BindByName = true;
                command.InitialLONGFetchSize = -1;
                //if (oracleTransaction != null)
                //{
                //    command.Transaction = oracleTransaction;
                //}
                if (pDbCon.Trs != null)
                {
                    command.Transaction = pDbCon.Trs;
                }
                foreach (OracleParameter parameter in mParameter.Parameters)
                {
                    command.Parameters.Add(parameter);
                }

                try
                {
                    using (OracleDataReader oracleDataReader = command.ExecuteReader())
                    {
                        int i = 1;
                        while (oracleDataReader.Read())
                        {
                            collection.Add(ConvertDto<DTO>(oracleDataReader));
                            i++;
                        }

                        command.Parameters.Clear(); // 반드시 clear 해야함.
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }
            }

            return collection;
        }
        public static T ExecuteScalar<T>(MParameter mParameter, PsmhDb pDbCon)
        {
            object obj = null;
            using (OracleCommand command = new OracleCommand(mParameter.SQL, pDbCon.Con))
            {
                command.CommandType = mParameter.commandType;
                command.BindByName = true;
                command.InitialLONGFetchSize = -1;
                //if (oracleTransaction != null)
                //{
                //    command.Transaction = oracleTransaction;
                //}
                if (pDbCon.Trs != null)
                {
                    command.Transaction = pDbCon.Trs;
                }
                foreach (OracleParameter parameter in mParameter.Parameters)
                {
                    command.Parameters.Add(parameter);
                }

                obj = command.ExecuteScalar();
            }

            if (obj == null)
            {
                return default(T);
            }

            if (Convert.IsDBNull(obj))
            {
                return default(T);
            }

            if (obj is T)
            {
                return (T)obj;
            }
            else
            {
                return (T)Convert.ChangeType(obj, typeof(T));
            }
        }

        public static List<Dictionary<string, object>> ExecuteReader(MParameter mParameter, PsmhDb pDbCon)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

            using (OracleCommand command = new OracleCommand(mParameter.SQL, pDbCon.Con))
            {
                command.CommandType = mParameter.commandType;
                command.BindByName = true;
                command.InitialLONGFetchSize = -1;
                //if (oracleTransaction != null)
                //{
                //    command.Transaction = oracleTransaction;
                //}
                if (pDbCon.Trs != null)
                {
                    command.Transaction = pDbCon.Trs;
                }
                foreach (OracleParameter parameter in mParameter.Parameters)
                {
                    command.Parameters.Add(parameter);
                }

                using (OracleDataReader oracleDataReader = command.ExecuteReader())
                {
                    while (oracleDataReader.Read())
                    {
                        Dictionary<string, object> dictionary = new Dictionary<string, object>();
                        for (int j = 0; j < oracleDataReader.FieldCount; j++)
                        {
                            string columnName = oracleDataReader.GetName(j);
                            dictionary.Add(columnName, oracleDataReader[j]);
                        }

                        list.Add(dictionary);
                    }

                    command.Parameters.Clear(); // 반드시 clear 해야함.
                }
            }

            return list;
        }

        public static Dictionary<string, object> ExecuteReaderSingle(MParameter mParameter, PsmhDb pDbCon)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            using (OracleCommand command = new OracleCommand(mParameter.SQL, pDbCon.Con))
            {
                command.CommandType = mParameter.commandType;
                command.BindByName = true;
                command.InitialLONGFetchSize = -1;
                //if (oracleTransaction != null)
                //{
                //    command.Transaction = oracleTransaction;
                //}
                if (pDbCon.Trs != null)
                {
                    command.Transaction = pDbCon.Trs;
                }
                foreach (OracleParameter parameter in mParameter.Parameters)
                {
                    command.Parameters.Add(parameter);
                }

                using (OracleDataReader oracleDataReader = command.ExecuteReader())
                {
                    while (oracleDataReader.Read())
                    {
                        for (int j = 0; j < oracleDataReader.FieldCount; j++)
                        {
                            string columnName = oracleDataReader.GetName(j);
                            dictionary.Add(columnName, oracleDataReader[j]);
                        }

                    }

                    command.Parameters.Clear(); // 반드시 clear 해야함.
                }
            }

            return dictionary;
        }

        /// <summary>
        /// 저장
        /// </summary>
        /// <param name="mParameter"></param>
        /// <param name="pDbCon"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(MParameter mParameter, PsmhDb pDbCon)
        {
            int rowCount = -1;
            using (OracleCommand command = new OracleCommand(mParameter.SQL, pDbCon.Con))
            {
                command.CommandType = mParameter.commandType;
                command.BindByName = true;
                //if (oracleTransaction != null)
                //{
                //    command.Transaction = oracleTransaction;
                //}
                if (pDbCon.Trs != null)
                {
                    command.Transaction = pDbCon.Trs;
                }
                if (mParameter != null)
                {
                    foreach (OracleParameter parameter in mParameter.Parameters)
                    {
                        command.Parameters.Add(parameter);
                    }
                }
                // Log File.AppendAllText
                // System.IO.File.WriteAllText(@"C:\temp\sql.txt", mParameter.SQL);
                System.IO.File.AppendAllText(@"C:\temp\sql.txt","Time = " + DateTime.Now.ToString("yyyy-MM=dd hh:mm:ss"));
                System.IO.File.AppendAllText(@"C:\temp\sql.txt", mParameter.SQL);
                rowCount = command.ExecuteNonQuery();
            }

            return rowCount;
        }

        public static Dictionary<string, object> ExecuteProc(MParameter mParameter, PsmhDb pDbCon)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            using (OracleCommand command = new OracleCommand(mParameter.SQL, pDbCon.Con))
            {
                command.CommandType = mParameter.commandType;
                command.BindByName = true;
                //if (oracleTransaction != null)
                //{
                //    command.Transaction = oracleTransaction;
                //}
                if (pDbCon.Trs != null)
                {
                    command.Transaction = pDbCon.Trs;
                }
                if (mParameter != null)
                {
                    foreach (OracleParameter parameter in mParameter.Parameters)
                    {
                        command.Parameters.Add(parameter);
                    }
                }

                command.ExecuteNonQuery();

                foreach (OracleParameter parameter in mParameter.Parameters)
                {
                    dic.Add(parameter.ParameterName, parameter.Value);
                }
            }

            return dic;
        }

        /// <summary>
        /// DB 컬럼 값이 Null이 아닌것만 Convert
        /// TODO: 속도문제대문에 ReaderExists 삭제함 다시 살펴볼것 dhkim 20190424
        /// 
        /// 속도 너무 느려서 변경
        /// </summary>
        /// <typeparam name="DTO"></typeparam>
        /// <param name="oracleDataReader"></param>
        /// <returns></returns>
        private static DTO ConvertDto<DTO>(DbDataReader dbDataReader)
        {
            DTO t = (DTO)Activator.CreateInstance(typeof(DTO));
            PropertyInfo[] propertys = typeof(DTO).GetProperties();

            for (int i = 0; i < dbDataReader.FieldCount; i++)
            {
                string name = dbDataReader.GetName(i);
                if (name == "SIGNDATE")
                {
                    string xx = "";
                }
                
                //object gridKey = null;
                foreach (PropertyInfo pi in propertys)
                {
                    if (name.ToUpper() == pi.Name.ToUpper())
                    {
                        if (!pi.CanWrite)
                        {
                            continue;
                        }

                        //2020-08-26 장례식장오류로 막음
                        object value = DBNull.Value;

                        //if(Nullable.GetUnderlyingType(pi.PropertyType))

                        if (pi.PropertyType == typeof(decimal?) || pi.PropertyType == typeof(double?) ||
                            pi.PropertyType == typeof(Int16?) || pi.PropertyType == typeof(Int32?) || pi.PropertyType == typeof(Int64?))
                        {
                            if (dbDataReader.IsDBNull(i))
                            {
                                try
                                {
                                    pi.SetValue(t, ConvertDto(pi.PropertyType, null), null);
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    string message = t.GetType() + " ( " + pi.Name + " : " + pi.PropertyType + " 타입이 올바르지 않습니다) DB Value:" + value + " type: " + value.GetType();
                                    throw new MTSException(message);
                                }
                            }
                        }

                        if (dbDataReader.GetFieldType(i) == typeof(decimal))
                        {
                            try
                            {
                                value = dbDataReader.IsDBNull(i) ? 0 : dbDataReader.GetDecimal(i);
                            }
                            catch (Exception ex)
                            {
                                value = dbDataReader.IsDBNull(i) ? 0 : dbDataReader.GetDouble(i);

                                //Log.Debug(string.Format("필드타입 : {0}, DTO 타입 : {1}, Value :{2}", dbDataReader.GetFieldType(i).Name, pi.PropertyType.Name, value));
                                //Log.Debug(ex.Message);
                            }
                        }
                        else if (dbDataReader.GetFieldType(i) == typeof(double))
                        {
                            value = dbDataReader.IsDBNull(i) ? 0 : dbDataReader.GetDouble(i);
                        }
                        else if (dbDataReader.GetFieldType(i) == typeof(Int16))
                        {
                            value = dbDataReader.IsDBNull(i) ? 0 : dbDataReader.GetInt16(i);
                        }
                        else if (dbDataReader.GetFieldType(i) == typeof(Int32))
                        {
                            value = dbDataReader.IsDBNull(i) ? 0 : dbDataReader.GetInt32(i);
                        }
                        else if (dbDataReader.GetFieldType(i) == typeof(Int64))
                        {
                            value = dbDataReader.IsDBNull(i) ? 0 : dbDataReader.GetInt64(i);
                        }
                        else
                        {
                            value = dbDataReader.GetValue(i);
                        }
                        
                        //object value = dbDataReader.GetValue(i);
                        if (value != DBNull.Value)
                        {
                            try
                            {
                                pi.SetValue(t, ConvertDto(pi.PropertyType, value), null);
                                
                                break;
                            }
                            catch (Exception ex)
                            {
                                string message = t.GetType() + " ( " + pi.Name + " : " + pi.PropertyType + " 타입이 올바르지 않습니다) DB Value:" + value + " type: " + value.GetType();
                                throw new MTSException(message);
                            }
                        }
                    }
                }
            }
            return t;
        }


        /// <summary>
        /// DTO 리플렉스 데이타 타입 맵핑
        /// </summary>
        /// <created>Mentorsoft : donghoonkim 2018-12-26,23:45</created>
        /// <param name="value"></param>
        /// <returns></returns>
        private static object ConvertDto(Type type, object value)
        {
            string typeName = string.Empty;
            var nullableType = Nullable.GetUnderlyingType(type);
            if (nullableType != null)
            {
                typeName = nullableType.Name;
            }
            else
            {
                typeName = type.Name;
            }

            if (value is decimal)
            {
                if (typeName == "Double")
                {
                    return Convert.ToDouble(value);
                }
                else if (type == typeof(decimal))
                {
                    return (decimal)value;
                }
                else if (type == typeof(Int64?))
                {
                    if (value == null)
                    {
                        return null;
                    }
                    return long.Parse(value.ToString());
                }
                else if (type == typeof(decimal?))
                {
                    if (value == null)
                    {
                        return null;
                    }

                    return decimal.Parse(value.ToString());
                }
                else
                {
                    return long.Parse(value.ToString());
                }
            }
            else if (value is float)
            {
                if (typeName == "Double")
                {
                    return double.Parse(value.ToString());
                }
                else
                {
                    return decimal.Parse(value.ToString());
                }

            }
            
            else if (value is double)
            {
                 if (type == typeof(decimal?) || type == typeof(decimal))
                 {
                    return decimal.Parse(value.ToString());
                 }

                 // 보건대행 관련 오류 인해 추가
                 // 2021.03.10
                 if(type == typeof(long) || type == typeof(long?))
                 {
                    return long.Parse(value.ToString());
                 }
                 return double.Parse(value.ToString());
            }
            else if (typeName == "DateTime")
            {
                if (value is string)
                {
                    return Convert.ToDateTime(value);
                }
                else
                {
                    return value;
                }
            }
            else if (value is DateTime)
            {
                if (typeName == "string" || typeName == "String")
                {
                    return Convert.ToDateTime(value).ToString("yyyy-MM-dd");
                }
                else
                {
                    return value;
                }
            }
            else if (type.IsEnum)
            {

                return Enum.Parse(type, value.ToString(), true);
            }
            else if (value is string)
            {
                return value.ToString();
            }
            else if (value is int)
            {
                if (typeName == "string" || typeName == "String")
                {
                    return value.ToString(); //Convert.ToInt16(value);
                }
                else if (typeName == "Decimal")
                {
                    return decimal.Parse(value.ToString());
                }
                else
                {
                    if (type == typeof(Int64?))
                    {
                        return Int64.Parse(value.ToString());
                    }
                    else if (type == typeof(Int32?))
                    {
                        return Int32.Parse(value.ToString());
                    }
                    else if (type == typeof(Int16?))
                    {
                        return Int16.Parse(value.ToString());
                    }
                    return value;

                }
            }
            else if (value is Int16 || value is Int32 || value is Int64)
            {
                if (typeName == "string")
                {
                    return value.ToString(); //Convert.ToInt16(value);
                }
                else if (typeName == "Decimal")
                {
                    return decimal.Parse(value.ToString());
                }
                else if (typeName == "Int64")
                {
                    return long.Parse(value.ToString());
                }
                else if (typeName == "Int32" || typeName == "Int16")
                {
                    return int.Parse(value.ToString());
                }
                else
                {
                    return value;

                }
            }
            //else if (value is double)
            //{
            //    return long.Parse(value.ToString());
            //}
            else if (value is long)
            {
                return long.Parse(value.ToString());
            }
            else if (value is char)
            {
                return value.ToString();
            }
            else if (value is Single)
            {
                return double.Parse(value.ToString());
            }
            else
            {
                return value;
            }
        }


        #endregion
    }
}
