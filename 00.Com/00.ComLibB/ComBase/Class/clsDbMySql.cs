using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace ComBase
{
    public class clsDbMySql
    {
        public static MySqlConnection gDc = null;
        public static MySqlTransaction gTrs = null;
        public static MySqlCommand cmd = null;
        
        public static bool DBConnect(string strServer, string strPort, string strUser, string strPWD, string strDB)
        {
            string strConnectString = null;

            strConnectString = "server=" + strServer + ";user=" + strUser + ";database=" + strDB + ";port=" + strPort + ";password=" + strPWD + ";SslMode = none";

            try
            {
                gDc = new MySqlConnection(strConnectString);

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
        /// DB 단순 쿼리에 결과 값을 DataTable 값으로 리턴 시켜줍니다. 
        /// </summary>
        /// <param name="strSql">쿼리 내용입니다.</param>
        /// <returns>DataTable</returns>
        /// <remarks>항상 쿼리를 보낸 후 값이 Nothing가 아니면 Dispose 시켜줍니다.</remarks>
        public static DataTable GetDataTable(string strSql)
        {
            MySqlDataAdapter da = default(MySqlDataAdapter);
            DataSet ds = null;
            try
            {
                da = new MySqlDataAdapter(strSql, gDc);

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
            catch (MySqlException sqlExc)
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
            catch (MySqlException sqlExc)
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
