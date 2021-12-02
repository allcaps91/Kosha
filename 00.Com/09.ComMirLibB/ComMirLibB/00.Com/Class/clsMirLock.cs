using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComBase;
using System.Data;
using ComDbB;

namespace ComMirLibB.Com
{
    public class clsMirLock
    {
        PsmhDb pDbCon = new PsmhDb();
        string SQL = "";
        string SqlErr = "";
        int intRowAffected = 0;
        DataTable dt = null;

        public void Mir_Lock_Delete(string argJob, long argWrtno)
        {
            pDbCon.DBConnect();

            clsDB.setBeginTran(pDbCon);

            SQL = "";
            SQL += "DELETE FROM   KOSMOS_PMPA.MIR_LOCK ";
            SQL += " WHERE JobGubun = '" + argJob + "' ";
            SQL += "   AND WRTNO    =  " + argWrtno + " ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            clsDB.setCommitTran(pDbCon);

            pDbCon.DisDBConnect();
        }

        public bool Mir_Lock_Insert(string argJob, string argComment, long argWrtno)
        {
            pDbCon.DBConnect();

            bool rtnVal = false;



            SQL = "";
            SQL += "SELECT TO_CHAR(WrtTime, 'yyyy-mm-dd hh24:mi') Jtime,";
            SQL += "       UserName, JobComment   ";
            SQL += "  FROM KOSMOS_PMPA.MIR_LOCK               ";
            SQL += " WHERE JobGubun = '" + argJob + "' ";
            SQL += "   AND WRTNO    =  " + argWrtno + " ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

            if (dt.Rows.Count == 1)
            {
                string strMsg = "작업자명 : " + dt.Rows[0]["UserName"].ToString() + ComNum.VBLF;
                strMsg += "작업내용 : " + dt.Rows[0]["JobComment"].ToString() + ComNum.VBLF;
                strMsg += "시작시간 : " + dt.Rows[0]["JTime"].ToString() + ComNum.VBLF;
                strMsg += "잠시후에 다시 작업을 하시거나 다른 환자에 대한 작업을 하십시오 !";

                ComFunc.MsgBox(strMsg, "주의 !");
                rtnVal = false;
            }
            else
            {
                clsDB.setBeginTran(pDbCon);

                SQL = "";
                SQL += "INSERT INTO MIR_LOCK (JobGubun,WRTNO,UserName,JobComment,WrtTime) ";
                SQL += "VALUES";
                SQL += "('" + argJob + "', " + argWrtno + ", '" + clsType.User.JobName + "', '" + argComment + "', SYSDATE) ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {

                    clsDB.setRollbackTran(pDbCon);
                    rtnVal = false;
                }

                clsDB.setCommitTran(pDbCon);
                rtnVal = true;
            }

            pDbCon.DisDBConnect();
            return rtnVal;
        }
    }
}
