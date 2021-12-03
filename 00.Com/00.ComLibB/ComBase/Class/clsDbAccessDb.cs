using System;
using System.Data;
using System.Data.OleDb;

namespace ComBase
{
    //===== SELECT ======//
    //dt = clsDbAccessDb.GetDataTable(SQL);
    //
    //if (dt == null)
    //{
    //	ComFunc.MsgBox("조회중 문제가 발생했습니다");
    //	return;
    //}
    //if (dt.Rows.Count == 0)
    //{
    //	dt.Dispose();
    //	dt = null;
    //	ComFunc.MsgBox("해당 DATA가 없습니다.");
    //	return;
    //}

    //===== TRANSACTION ======//
    //DataTable dt = null;
    //string strSql = "";
    //Cursor.Current = Cursors.WaitCursor;
    //clsDbAccessDb.setBeginTran();
    //
    //try
    //{
    //	strSql = "";
    //	strSql = strSql + clsComNum.VBLF + " ";
    //	clsDbAccessDb.ExecuteNonQuery(strSql);
    //	
    //	clsDbAccessDb.setCommitTran();
    //	ComFunc.MsgBox("저장하였습니다.");
    //	Cursor.Current = Cursors.Default;
    //	return true;
    //}
    //catch (Exception ex)
    //{
    //	clsDbAccessDb.setRollbackTran();
    //	ComFunc.MsgBox(ex.Message);
    //	Cursor.Current = Cursors.Default;
    //	return false;
    //}
    

    public class clsDbAccessDb
    {
        private static OleDbConnection dc = null;
        public static OleDbTransaction trs = null;

        public static OleDbConnection gDc = null;
        public static OleDbTransaction gTrs = null;

        private static OleDbCommand cmd = null;

        public static bool DBConnect(string strSource)
        {
            string strConnectString = null;

            strConnectString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strSource + ";";

            try
            {
                gDc = new OleDbConnection(strConnectString);

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

        public static bool DBConnect(string strSource, string strUser, string strPassWord)
        {
            string strConnectString = null;

            strConnectString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strSource + ";User ID=" + strUser + ";Password=" + strPassWord + ";";

            try
            {
                gDc = new OleDbConnection(strConnectString);

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
            OleDbDataAdapter da = default(OleDbDataAdapter);
            DataSet ds = null;
            try
            {
                da = new OleDbDataAdapter(strSql, gDc);

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
            catch (OleDbException sqlExc)
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
            catch (OleDbException sqlExc)
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
