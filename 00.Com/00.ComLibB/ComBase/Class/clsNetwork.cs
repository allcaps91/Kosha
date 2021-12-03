using Microsoft.Win32;
using System;
using System.Data;
using System.Windows.Forms;         //messagebox

namespace ComBase
{
    public class Network : MTSDisposable
    {
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        //string SQL;
        //long rowcounter;
        //string strValue;

        //DataTable dt = null;        
        //string SqlErr = ""; //에러문 받는 변수
        //int intRowAffected = 0; //변경된 Row 받는 변수

        //사용자 IP_ADDRESS 불러오는 Fuction
        public string Read_IPAddress_SQL()
        {
            string SQL = "";
            long rowcounter = 0;
            string strValue = "";

            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수            

            try
            {
                SQL = "";
                SQL += " SELECT SYS_CONTEXT('USERENV','IP_ADDRESS') IPADDRESS     \r";
                SQL += "   FROM DUAL                                              \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);;

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    strValue = "";
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    //ComFunc.MsgBox("해당 DATA가 없습니다.");
                    strValue = "";
                }

                rowcounter = 0;

                rowcounter = dt.Rows.Count;

                if (rowcounter == 1)
                {
                    strValue = dt.Rows[0]["IPADDRESS"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                return strValue;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return strValue;
            }
        }

        //컴퓨터 현재 위치를 레지스트리에서 읽어오기
        public string Read_Reg_Position()
        {
            //string SQL;
            //long rowcounter;
            //string strValue;

            //DataTable dt = null;
            //string SqlErr = ""; //에러문 받는 변수
            //int intRowAffected = 0; //변경된 Row 받는 변수
            //* 레지스트리 세팅값 가져오기
            const string RegRoot = @"SOFTWARE\VMS\3.0\SiteClient\General\";
            string setting = RegRoot;// +@"Setting";
            string sUserName = "";
            string sUserPartName = "";

            RegistryKey reg = Registry.LocalMachine;
            //reg = reOpenSubKey(setting, true);

            if (reg != null)
            {
                //체크드 리스트 박스 데이터 값 입력
                //sUserName = Convert.ToString(reGetValue("UserName"));
                //sUserPartName = Convert.ToString(reGetValue("UserPartName"));
            }
            //reClose();


            return sUserPartName + " / " + sUserName;

        }

        //사용자 IP_ADDRESS 불러오는 Fuction
        public string Read_SESSIONID_SQL()
        {
            string SQL = "";
            long rowcounter = 0;
            string strValue = "";

            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수
            
            try
            {
                SQL = "";
                SQL += " SELECT SYS_CONTEXT('USERENV','SESSIONID') SESSIONID      \r";
                SQL += "   FROM DUAL                                              \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);;

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    strValue = "";
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    //ComFunc.MsgBox("해당 DATA가 없습니다.");
                    strValue = "";
                }

                rowcounter = 0;
                rowcounter = dt.Rows.Count;

                if (rowcounter == 1)
                {
                    strValue = dt.Rows[0]["SESSIONID"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                return strValue;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return strValue;
            }
        }

        //세션별 LOCK환자 모두 지우기
        public void DELETE_SESSIONID_SQL()
        {
            string SQL = "";
            
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL += " DELETE                                          \r";
                SQL += "   FROM BASIC.BG_LOCK_PATIENT                    \r";
                SQL += "  WHERE SESSIONID = USERENV('SESSIONID')         \r";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }
    }
}
