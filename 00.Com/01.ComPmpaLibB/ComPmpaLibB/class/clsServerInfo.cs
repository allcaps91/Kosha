using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using System;
using System.Data;

namespace ComPmpaLibB
{
    public class clsServerInfo
    {
        //DataTable Dt = new DataTable();
        //string SQL = string.Empty;
        //string SqlErr = string.Empty;
        DateTime rtnVal;

        private DateTime DtServerDateTime;

        public DateTime ServerDateTime
        {
            get { return DtServerDateTime; }
        }

        public clsServerInfo()
        {
            DtServerDateTime = DateTime.Now;
            Load(clsDB.DbCon);
        }

        private void Load(PsmhDb pDbCon)
        {
            DtServerDateTime = GetServerDateTime(pDbCon);
        }


        private DateTime GetServerDateTime(PsmhDb pDbCon)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable Dt = new DataTable();

            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(SYSDATE, 'YYYY-MM-DD HH24:MI') ServerDate ";
            SQL += ComNum.VBLF + "   FROM DUAL ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            if (Dt.Rows.Count >= 1)
                rtnVal = Convert.ToDateTime( Dt.Rows[0]["ServerDate"].ToString());

            Dt.Dispose();
            Dt = null;

            return rtnVal;
        }



    }
}
