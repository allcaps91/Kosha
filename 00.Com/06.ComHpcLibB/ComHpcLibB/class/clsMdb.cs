using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data;

namespace ComHpcLibB
{
    /// <summary>
    /// InBody 관련 MDB 접속 Class
    /// </summary>
    public class clsMdb
    {
        public static OleDbConnection m_pConn;
        public static OleDbCommand Comm = null;
        public string path = @"Z:\Database\LookinBody.MDB";
        public static string strMDBConnYN = "";

        public void Connect_Binding()
        {
            string oleDbConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + @path + ";";

            if (ConnectDB(oleDbConnStr))
            {
                strMDBConnYN = "SUCCESS";
            }
            else
            {   //연결실패
                strMDBConnYN = "FAIL";
            }
        }

        public bool ConnectDB(String sConStr)
        {
            //m_sConStr = sConStr;
            bool bResult = false;
            try
            {
                m_pConn = new OleDbConnection(sConStr);
                m_pConn.Open();
                if (m_pConn.State == ConnectionState.Open)
                {
                    bResult = true;
                }
                else
                {

                    bResult = false;
                }
            }
            catch (Exception e)
            {
                bResult = false;
                MessageBox.Show(e.Message);     //예외 메세지             
            }

            return bResult;
        }

        /// <summary>
        ///  OLEDB 연결 끊기
        /// </summary>
        public void DBClose()
        {
            try
            {
                m_pConn.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);     //예외 메세지                  }
            }

        }

        /// <summary>
        /// 실행..생성,수정,삭제,삽입 등
        /// </summary>
        /// <param name="sQuery"></param>
        /// <returns></returns>
        public int ExcuteQuery(string sQuery)
        {
            int iResultCnt = 0;  //반영된 행의 곗수
            try
            {
                OleDbCommand command = new OleDbCommand(sQuery);
                command.Connection = m_pConn;
                iResultCnt = command.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                if ((m_pConn.State == ConnectionState.Closed) || (m_pConn.State == ConnectionState.Broken))
                {
                    //디비 커넥션 연결 실패
                }
                MessageBox.Show(e.Message);
            }

            return iResultCnt;
        }

        /// <summary>
        /// SELECT DataSet
        /// </summary>
        /// <param name="sQuery"></param>
        /// <returns></returns>
        public DataSet GetDataSet(String sQuery)
        {
            DataSet pDataset = new DataSet();

            try
            {
                OleDbDataAdapter pAdapter = new OleDbDataAdapter();
                pAdapter.SelectCommand = new OleDbCommand(sQuery, m_pConn);
                pAdapter.Fill(pDataset);
            }
            catch (Exception e)
            {
                if ((m_pConn.State == ConnectionState.Closed) || (m_pConn.State == ConnectionState.Broken))
                {
                    //디비 커넥션 연결 실패
                }
                MessageBox.Show(e.Message);
            }
            return pDataset;
        }

        /// <summary>
        /// SELECT DataTable
        /// </summary>
        /// <param name="sQuery"></param>
        /// <returns></returns>
        public DataTable GetTable(String sQuery)
        {
            DataTable pTable = new DataTable();

            try
            {
                OleDbDataAdapter pAdapter = new OleDbDataAdapter();
                pAdapter.SelectCommand = new OleDbCommand(sQuery, m_pConn);
                pAdapter.Fill(pTable);
            }
            catch (Exception e)
            {
                if ((m_pConn.State == ConnectionState.Closed) || (m_pConn.State == ConnectionState.Broken))
                {
                    MessageBox.Show("MDB Disconnected. Please Connect MDB!", "MDB Disconnected");
                }
                MessageBox.Show(e.Message);
            }
            return pTable;
        }
    }
}
