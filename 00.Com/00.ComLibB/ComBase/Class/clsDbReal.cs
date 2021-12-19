using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace ComBase
{
    public class clsDbReal
    {
        private static string gDataBase = null;
        private static OracleConnection dc = null;
        public static OracleTransaction trs = null;

        public static OracleConnection gDc = null;
        public static OracleTransaction gTrs = null;

        private static OracleCommand cmd = null;

        public static string strDbIniFile = @"C:\HealthSoft\HSMain\psmh.ini";
        public static string strSource = "ORA7";
        public static string strDbIp = "";
        public static string strDbPort = "";
        public static string strUser = "KOSMOS_PMPA";
        public static string strPassWord = "";

        public static string strDbOption = "";

        private static string GetPassWord(string pPassWord)
        {
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

            return strPassWord;
        }

        public static bool DBConnect()
        {
            string tstrPassWord = "aGFzb3Nkc3BkcGRkaWRkdHNzYWRrbA==";
            strPassWord = GetPassWord(tstrPassWord);

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
        /// DB 연결해제 함수 입니다.
        /// </summary>
        public static bool DBConnect(string strDbIp, string strDbPort, string strSource, string strUser, string strPassWord)
        {
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
        /// <param name="strSql"></param>
        /// <returns></returns>
        public static DataSet GetDataSet(string strSql)
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

                Adapter.SelectCommand = new OracleCommand(strSql, gDc);
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
        /// <param name="strSql">쿼리 내용입니다.</param>
        /// <returns>DataTable</returns>
        /// <remarks>항상 쿼리를 보낸 후 값이 Nothing가 아니면 Dispose 시켜줍니다.</remarks>
        public static DataTable GetDataTable(string strSql)
        {

            OracleDataAdapter da = default(OracleDataAdapter);
            DataSet ds = null;
            try
            {
                da = new OracleDataAdapter(strSql, gDc);

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

        public static DataTable GetDataTableEx(string strSql)
        {
            OracleDataReader reader = null;
            DataTable table = null;

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
                table = new DataTable();

                cmd.CommandText = strSql;
                cmd.CommandTimeout = 30;
                reader = cmd.ExecuteReader();

                table.Load(reader);
                reader.Dispose();
                reader = null;

                cmd.Dispose();
                cmd = null;

                return table;
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
                if ((reader != null))
                    reader.Dispose();
                reader = null;
            }
        }

        /// <summary>
        /// DB 단순 쿼리에 결과 값을 DataTable 값으로 리턴 시켜줍니다. 
        /// </summary>
        /// <param name="strSql">쿼리 내용입니다.</param>
        /// <returns>DataTable</returns>
        /// <remarks>항상 쿼리를 보낸 후 값이 Nothing가 아니면 Dispose 시켜줍니다.</remarks>
        public static DataTable GetDataReder(string strSql)
        {
            OracleDataReader reader = null;
            DataTable table = null;

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
                table = new DataTable();

                cmd.CommandText = strSql;
                cmd.CommandTimeout = 30;
                reader = cmd.ExecuteReader();

                table.Load(reader);
                reader.Dispose();
                reader = null;

                cmd.Dispose();
                cmd = null;

                return table;
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
                if ((reader != null))
                    reader.Dispose();
                reader = null;
            }
        }


        /// <summary>
        /// reader를 반환한다.
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public static OracleDataReader GetAdoRs(string strSql)
        {
            //using System.Data.OleDb; 상속을 받아서 사용해야 함
            OracleDataReader reader = null;

            try
            {
                if (cmd == null)
                {
                    cmd = gDc.CreateCommand();
                }
                if (gTrs != null)
                {
                    cmd.Transaction = gTrs;
                }
                cmd.CommandText = strSql;
                cmd.CommandTimeout = 30;
                reader = cmd.ExecuteReader();

                cmd.Dispose();
                cmd = null;

                return reader;
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
        /// <param name="strSql">쿼리 내용 입니다.</param>
        /// <returns>True/False</returns>
        /// <remarks>setBeginTx, setCommitTx, setRollbackTx 을 사용해서 구성한다.</remarks>
        public static bool ExecuteNonQuery(string strSql)
        {
            try
            {
                if (cmd == null)
                {
                    cmd = gDc.CreateCommand();
                }

                cmd.CommandText = strSql;
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



    }
}
