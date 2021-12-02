using ComBase;
using ComDbB;
using ComLibB;
using Oracle.DataAccess.Client;
using System;
using System.Data;

namespace ComMedLibB
{
    /// <summary>
    /// 작성자 : 이상훈
    /// 처방 공통 Class
    /// </summary>
    public class clsMedFunction
    {
        DataTable dt = null;
        DataTable dt1 = null;
        string SQL = "";    //Query문
        string SqlErr = ""; //에러문 받는 변수
        int rowcounter;
        string strValue;

        //clsPrint Print = new clsPrint();
        //ComPrintApi comPrintApi = new ComPrintApi();

        public string PrintName { get; private set; }

        /// <summary>바코드 종류별 분류</summary>
        public enum enmPrintType { USB, COM_PORT };

        public string Get_DeptName(string ArgCode)
        {
            try
            {   
                SQL = "";
                SQL += " select DEPTNAMEK                            \r";
                SQL += "   from " + ComNum.DB_PMPA + "BAS_CLINICDEPT \r";
                SQL += "  where DEPTCODE = '" + ArgCode.Trim() + "'  \r";
                clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    strValue = "";
                    return strValue;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    //ComFunc.MsgBox("해당 DATA가 없습니다.");
                    strValue = "";
                    return strValue;
                }

                rowcounter = 0;
                rowcounter = dt.Rows.Count;

                if (rowcounter > 0)
                {   
                    strValue = dt.Rows[0]["DEPTNAMEK"].ToString().Trim();
                    dt.Dispose();
                    dt = null;
                    return strValue;
                }
                else
                {
                    dt.Dispose();
                    dt = null;
                    strValue = "";
                    return strValue;
                }
            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return strValue;
            }
        }

        public string Get_DRName(string ArgCode)
        {
            try
            {
                SQL = "";
                SQL += " select DRNAME                              \r";
                SQL += "   from " + ComNum.DB_PMPA + "BAS_DOCTOR    \r";
                SQL += "  where DRCODE = '" + ArgCode.Trim() + "'   \r";
                clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    strValue = "";
                    return strValue;
                }
                if (dt.Rows.Count == 0)
                {   
                    //ComFunc.MsgBox("해당 DATA가 없습니다.");
                    strValue = "";
                    dt.Dispose();
                    dt = null;
                    return strValue;
                }

                rowcounter = 0;
                rowcounter = dt.Rows.Count;

                if (rowcounter > 0)
                {   
                    strValue = dt.Rows[0]["DRNAME"].ToString().Trim();
                    dt.Dispose();
                    dt = null;
                }
                return strValue;
            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return strValue;
            }
        }

        public string Get_BiName(string ArgCode)
        {
            try
            {
                SQL = "";
                SQL += " select NAME                            \r";
                SQL += "   from kosmos_pmpa.bas_bcode           \r";
                SQL += "  where gubun = 'BAS_환자종류'          \r";
                SQL += "    and code = '" + ArgCode.Trim() + "' \r";
                clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    strValue = "";
                    return strValue;
                }
                if (dt.Rows.Count == 0)
                {
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    strValue = "";
                    dt.Dispose();
                    dt = null;
                    return strValue;
                }

                rowcounter = 0;
                rowcounter = dt.Rows.Count;

                if (rowcounter > 0)
                {
                    strValue = dt.Rows[0]["NAME"].ToString().Trim();
                    dt.Dispose();
                    dt = null;
                }
                return strValue;
            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return strValue;
            }
        }

        public string Read_Pano_SELECT_MST_OP(string ArgPano, string ArgIO, string ArgDrCode, string ArgBDate, long ArgIpdNo)
        {
            string strBDate;
            long nIpdNo;

            strBDate = ArgBDate;
            nIpdNo = ArgIpdNo;
            strValue = "";

            if (ArgIO == "I")
            {
                if (READ_IPD_NEW_MASTER_INDATE_CHK(nIpdNo) != "OK") return strValue;
            }

            try
            {
                SQL = "";
                SQL += " SELECT Pano, SETC6                                         \r";
                SQL += "      , TO_CHAR(SDATE,'YYYY-MM-DD') SDATE                   \r";
                SQL += "      , TO_CHAR(EDATE,'YYYY-MM-DD') EDATE                   \r";
                SQL += "   FROM KOSMOS_PMPA.BAS_SELECT_MST                          \r";
                SQL += "  WHERE PANO = '" + ArgPano + "'                            \r";
                SQL += "    AND DRCODE = '" + ArgDrCode + "'                        \r";
                SQL += "    AND GUBUN = '" + ArgIO + "'                             \r";
                SQL += "    AND SDate <= TO_DATE('" + strBDate + "','YYYY-MM-DD')   \r";
                SQL += "    AND (DelDate IS NULL OR DelDate ='')                    \r";
                SQL += "    AND SET6 ='Y'                                           \r";
                SQL += "  ORDER BY SDate DESC                                       \r";
                clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strValue;
                }
                if (dt.Rows.Count == 0)
                {   
                    dt.Dispose();
                    dt = null;
                    return strValue;
                }

                rowcounter = dt.Rows.Count;

                if (rowcounter > 0)
                {
                    if (dt.Rows[0]["EDATE"].ToString().Trim() == "")
                    {
                        strValue = "OK" + dt.Rows[0]["SETC6"].ToString().Trim();
                    }
                    else if (DateTime.Parse(strBDate) <= DateTime.Parse(dt.Rows[0]["EDATE"].ToString().Trim()))
                    {
                        strValue = "OK" + dt.Rows[0]["SETC6"].ToString().Trim();
                    }
                    else
                    {
                        strValue = "";
                    }
                }
                dt.Dispose();
                dt = null;
                return strValue;
            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return strValue;
            }
        }

        public string READ_IPD_NEW_MASTER_INDATE_CHK(long ArgIpdNo)
        {
            strValue = "";

            try
            {
                SQL = "";
                SQL += "SELECT Pano,IPDNO                                               \r";
                SQL += "  FROM KOSMOS_PMPA.IPD_NEW_MASTER                               \r";
                SQL += " WHERE IPDNO = " + ArgIpdNo + "                                 \r";
                SQL += "   AND INDATE >= TO_DATE('2011-06-01 00:01','YYYY-MM-DD HH24:MI')\r";
                SQL += "   AND GBSTS NOT IN ('9')                                       \r";
                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    strValue = "";
                    return strValue;
                }
                if (dt.Rows.Count == 0)
                {   
                    strValue = "";
                    dt1.Dispose();
                    dt1 = null;
                    return strValue;
                }

                rowcounter = 0;
                rowcounter = dt.Rows.Count;

                if (rowcounter > 0)
                {
                    strValue = "OK";
                    
                }
                dt1.Dispose();
                dt1 = null;
                return strValue;
            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return strValue;
            }
        }

        public static void SimsaMsg_Check(PsmhDb pDbCon, string sOrdCode, string sBi)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string strMsg = "";

            clsOrdFunction OF = new clsOrdFunction();

            try
            {
                SQL = "";
                SQL += " SELECT SUCODE, REMARK                      \r";
                SQL += "   FROM KOSMOS_PMPA.JSIM_SIMSAMSG           \r";
                SQL += "  WHERE  SUCODE = '" + sOrdCode.Trim() + "' \r";
                SQL += "    AND  DDATE IS NULL                      \r";
                switch (sBi.Substring(0, 1))
                {
                    case "1":
                        SQL += "    AND B1 = 'Y'                    \r"; //보험
                        break;
                    case "2":
                        SQL += "    AND B2 = 'Y'                    \r"; //보호
                        break;
                    case "3":
                        SQL += "    AND B3 = 'Y'                    \r"; //산재
                        break;
                    case "5":
                        SQL += "    AND B4 = 'Y'                    \r"; //자보
                        break;
                    default:
                        break;
                }
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strMsg = dt.Rows[0]["REMARK"].ToString().Trim();
                    FrmMedDocMsgBox f = new FrmMedDocMsgBox(strMsg, "");
                    f.ShowDialog();
                    OF.fn_ClearMemory(f);
                }
                dt.Dispose();
                dt = null;
            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return;
            }
        }

        public void setCtrlLoad(System.Windows.Forms.Panel pan, System.Windows.Forms.Form frm)
        {

            pan.Controls.Clear();


            frm.TopLevel = false;
            frm.Dock = System.Windows.Forms.DockStyle.Fill;
            frm.ControlBox = false;
            frm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            frm.Show();

            pan.Controls.Add(frm);

        }
    }
}
